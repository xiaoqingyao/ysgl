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
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_ZiChanGuanLi_WeiXiuShenQingList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
            ClientScript.RegisterArrayDeclaration("availableTagsPersion", GetUsersAll());
            ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeptAll());
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e) {
        if (e == null){ return; }
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string billcode = e.Item.Cells[0].Text; 
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
                if (!billcode.Equals(""))
                {
                    WorkFlowRecordManager bll = new WorkFlowRecordManager();
                    string state = bll.WFState(billcode);
                    e.Item.Cells[6].Text = state; 
                }
            }
            if (!billcode.Equals(""))
            {
                string strZiChan = getZiChanName(billcode);
                e.Item.Cells[5].Text = strZiChan;
            }
        }
    }
    /// <summary>
    /// 页面初始化绑定
    /// </summary>
    private void BindDataGrid() {
        string strsql = "select billCode,(select '['+userCode+']'+userName from bill_users where userCode=billUser) as billUserName,billUser,(select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept) as billDeptName,billDept,billDate,billJe,stepid,(select top 1 SUBSTRING(ShuoMing,1,15)+'...' from ZiChan_WeiXiuShenQing where MainCode=a.billcode) as ShuoMing from bill_main a where flowID='wxsq'";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        strsql += " and billDept in (" + deptCodes + ")";

        #region 查询条件
        //申请开始日期
        if (txtAppDateFrm.Text != "")
        {
            strsql += " and  billDate >=cast ('" + txtAppDateFrm.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txtAppDateTo.Text != "")
        {
            strsql += " and  billDate <=cast ('" + txtAppDateTo.Text + "' as datetime  ) ";
        }
        string strBillCode = txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            strsql += " and billCode like '%" + strBillCode + "%'";
        }
        string strDept = txtDeptCode.Text.Trim();
        try
        {
            strDept = strDept.Substring(1, strDept.IndexOf("]") - 1);
        }
        catch (Exception) { strDept = ""; }
        if (!string.IsNullOrEmpty(strDept))
        {
            strsql += " and billDept = '" + strDept + "'";
        }
        string strUser = this.txtAppPersioncode.Text.Trim();
        try
        {
            strUser = strUser.Substring(1, strUser.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            strUser="";
        }
        if (!string.IsNullOrEmpty(strUser))
        {
            strsql += " and billUser = '" + strUser + "'";
        }
        #endregion
        strsql += " order by billDate desc ";
        DataSet temp = server.GetDataSet(strsql);
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
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Select_click(object sender, EventArgs e) {
        BindDataGrid();
    }
    #region 私有方法
    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
    }
    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }

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
    #endregion

    protected void btnExportExcel_Click(object sender, EventArgs e) {
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + DateTime.Today.ToString("yyyyMMdd") + ".xls");//设置文件名，如果包含中文，则会造成名称乱码，暂无解决方案，如有朋友知道，烦请告知，谢谢！// 如果设置为 GetEncoding("GB2312");导出的文件将会出现乱码！
        Response.ContentEncoding = System.Text.Encoding.UTF7;
        Response.ContentType = "application/ms-excel";//设置输出文件类型为excel文件。
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter); 
        this.myGrid.RenderControl(oHtmlTextWriter);//将gridview输出，你也可以类似的写TextBox1.RenderControl... 
        Response.Output.Write(oStringWriter.ToString());//Output the stream.   
        Response.Flush();      
        Response.End();
    }

    private string getZiChanName(string strCode) {
        string strSql = "select (select ZiChanName from dbo.ZiChan_Jilu where ZiChanCode=ZiChan_WeiXiuShenQing.ZiChanCode) as ZiChan from ZiChan_WeiXiuShenQing where MainCode='"+strCode+"'";
        DataTable dtRel = server.GetDataTable(strSql, null);
        if (dtRel==null||dtRel.Rows.Count==0)
        {
            return "";
        }
        string strEnd = "";
        for (int i = 0; i < dtRel.Rows.Count; i++)
        {
            strEnd += dtRel.Rows[i][0].ToString()+",";
        }
        if (strEnd.Length>1)
        {
            strEnd.Substring(0, strEnd.Length - 1);
        }
        return strEnd;
    }
}