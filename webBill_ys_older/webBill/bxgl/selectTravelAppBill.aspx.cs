using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Bll;

public partial class selectTravelAppBill : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    my_fzl.bindClss bindCl = new my_fzl.bindClss();
    string strUserCode = "";
    string strStatus = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objStatus = Request["Status"];
        if (objStatus != null)
        {
            strStatus = objStatus.ToString();
        }
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
            strUserCode = Session["userCode"].ToString().Trim();
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
            if (!IsPostBack)
            {
                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("ccsq");
                BindDataGrid();
            }
        //}
        ClientScript.RegisterArrayDeclaration("userAll", GetUserAll());
        ClientScript.RegisterArrayDeclaration("deptAll", GetDeptAll());
    }

    protected void BindDataGrid()
    {
        //出差申请单 和报告单都审核通过了的数据
        string sql = @"select distinct b.billCode,(select dicname from bill_datadic where diccode=a.typecode and dictype='06') as travelType
            ,b.billDate,(select deptName from bill_departments where deptCode=b.billDept) as billDept,
            (select userName from bill_users where userCode=b.billUser) as  billUser,
            (case b.stepID when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else 
            (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='ccsq' and bill_workFlowStep.stepid=b.stepID ) end) as stepID,a.reasion,a.needAmount from bill_main b,bill_travelApplication a where b.flowid='ccsq' and a.maincode=b.billCode 
            ";
        string deptCodes = (new Departments()).GetUserRightDepartments(strUserCode, "");
        //and b.billDept in (" + deptCodes + ") 
        sql += " and b.stepID='end' and b.billCode not in (select sqCode from bill_ybbx_fysq where status in ('1','2'))";//权限单位+审核通过+未报销过


        //借款单 过来已经冲减的  begin edit by zyl 2015-3-11
        if (Request["from"] == "jkd")
        {
            string codes = "";
            string sqlTemp = "  select item.note7 from bill_main main,  T_LoanList item where main.billcode =item.ListId and len(item.note7)>0 ";

            if (!string.IsNullOrEmpty(deptCodes))
                sqlTemp += " and main.billDept in(" + deptCodes + ") ";

            DataTable temp1 = server.GetDataTable(sqlTemp, null);
            for (int i = 0; i < temp1.Rows.Count; i++)
                codes += "'" + Convert.ToString(temp1.Rows[i]["note7"]) + "',";

            if (codes.Length > 1)
            {
                codes = codes.Substring(0, codes.Length - 1);

                sql += "  and b.billCode not in (" + codes + ")";
            }

        }
        //借款单 过来已经冲减的   end
        #region 查询条件
        //单据编号
        string strBillCode = txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and b.billCode like '%"+strBillCode+"%'";
        }
        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  b.billDate >cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  b.billDate <cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        //申请单位
        if (!this.txtAppDept.Text.Trim().Equals(""))
        {
            string strAppDept=this.txtAppDept.Text.Trim();
            if (strAppDept.IndexOf("]")>-1)
	        {
                strAppDept=strAppDept.Substring(1,strAppDept.IndexOf("]")-1);
	        }
            sql += " and b.billDept = '" + strAppDept + "'";
        }
        //申请人
        if (!this.txtAppPersion.Text.Trim().Equals(""))
        {
            string strAppPersion = this.txtAppPersion.Text.Trim();
            if (strAppPersion.IndexOf("]")>-1)
            {
                strAppPersion = strAppPersion.Substring(1, strAppPersion.IndexOf("]") - 1);
            }
            sql += " and b.billUser='" + strAppPersion + "'";
        }
        //Ctrl 是否控制状态 
        
        //显示已经附加了出差报告单的出差申请单
        if (strStatus.Equals("HasRepBill"))
        {
            sql += " and a.ReportCode in (select billCode from bill_main where flowid='ccbg' and stepID='end')";
        }
        sql += " order by b.billDate desc";
        #endregion

        DataSet temp = server.GetDataSet(sql);
        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Tables[0].Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();
        #endregion
        if (temp.Tables[0].Rows.Count == 0)
        {
            temp = null;
        }
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    private string GetDeptAll() {
        DataSet ds = server.GetDataSet("select '['+deptCode+']'+deptName as deptName from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }


    protected void myGrid_ItemDataBound(object sender,DataGridItemEventArgs e) 
    {

      
        if (e.Item.ItemType!=ListItemType.Header&&e.Item.ItemType!=ListItemType.Footer)
        {
             string strsql="";
            DataTable dttype=new DataTable();
            string strtype = e.Item.Cells[4].Text;
            string strmaincode = e.Item.Cells[0].Text;
            if (!strmaincode.Equals("&nbsp;")&&!strmaincode.Equals(""))
            {
                strsql = @"select (select dicName from bill_datadic where dicType='11' 
and dicCode =( select note1 from bill_travelReport where maincode = t.reportcode)) as bglb,
* from dbo.bill_travelApplication t where maincode='" + strmaincode + "'";
            }
           
            dttype = server.RunQueryCmdToTable(strsql);
            if (strtype.Equals("") || strtype == null || strtype.Equals("&nbsp;"))
            {
                if (dttype!=null)
                {
                    e.Item.Cells[4].Text = dttype.Rows[0]["bglb"].ToString();

                }
            }
        }
    }
}
