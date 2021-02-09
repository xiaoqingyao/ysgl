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

public partial class webBill_select_selectTravelRepBill : System.Web.UI.Page
{
    string strUserCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else {
            strUserCode = Session["userCode"].ToString().Trim();
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
            if (!IsPostBack)
            {
                BindDataGrid();
            }
        }
        ClientScript.RegisterArrayDeclaration("userAll", GetUserAll());
        ClientScript.RegisterArrayDeclaration("deptAll", GetDeptAll());
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    private void BindDataGrid() {
        string sql = @"select a.billCode,a.billName,a.stepID,a.billDate,a.billJe,
        (select userName from bill_users where userCode=a.billUser) as billUser
        ,(select deptName from dbo.bill_departments where a.billDept=deptCode) as deptName,
        b.* from bill_main a ,(select maincode,TravelProcess,WorkProcess,Result from bill_travelReport ) b
        where a.billCode=b.mainCode and a.stepID='end' and  a.flowID='ccbg' ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and a.billDept in (" + deptCodes + ")";

        #region 查询条件

        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >=cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <=cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        //申请单位
        if (!this.txtAppDept.Text.Trim().Equals(""))
        {
            string strAppDept = this.txtAppDept.Text.Trim();
            if (strAppDept.IndexOf("]") > -1)
            {
                strAppDept = strAppDept.Substring(1, strAppDept.IndexOf("]") - 1);
            }
            sql += " and a.billDept = '" + strAppDept + "'";
        }
        //申请人
        if (!this.txtAppPersion.Text.Trim().Equals(""))
        {
            string strAppPersion = this.txtAppPersion.Text.Trim();
            if (strAppPersion.IndexOf("]") > -1)
            {
                strAppPersion = strAppPersion.Substring(1, strAppPersion.IndexOf("]") - 1);
            }
            sql += " and a.billUser='" + strAppPersion + "'";
        }
        #endregion

        sql += " order by a.billDate desc ";

        DataSet temp = server.GetDataSet(sql);
        #region 计算分页相关数据
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
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
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

    private string GetDeptAll()
    {
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
}
