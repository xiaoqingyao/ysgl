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
using WorkFlowLibrary.WorkFlowBll;
using Bll;

public partial class webBill_search_ExpenseQuery : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    my_fzl.bindClss bindCl = new my_fzl.bindClss();
    ConfigBLL bllConfig = new ConfigBLL();
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
                if (this.txtBillCode.Text == null || this.txtBillCode.Text == "")
                {
                    DataTable dt = new DataTable();
                    myGrid.DataSource = dt;
                    myGrid.DataBind();

                    //通过配置项添加附加单据

                    if (bllConfig.GetModuleDisabled("HasBGSQ"))
                    {
                        this.selectBill.Items.Add(new ListItem("报告单", "bg"));
                    }
                    if (bllConfig.GetModuleDisabled("HasCGSP"))
                    {
                        this.selectBill.Items.Add(new ListItem("采购单", "cg"));
                    }
                    if (bllConfig.GetModuleDisabled("HasCCSQ"))
                    {
                        this.selectBill.Items.Add(new ListItem("出差单", "cc"));
                    }

                }
                else
                {
                    BindDataGrid();
                }


            }
        }

    }

    protected void BindDataGrid()
    {
        string sql = "";
        string strcode = "";
        if (this.txtBillCode.Text != null && this.txtBillCode.Text != "")
        {
            strcode = this.txtBillCode.Text.Trim();
            sql = @"select a.*, (select '['+userCode+']'+userName from dbo.bill_users where userCode=a.billUser )as username,
         (select '['+deptCode+']'+ deptName from dbo.bill_departments where deptCode=a.billDept)as deptname,
   (select top 1 sqCode from bill_ybbx_fysq where sqCode='" + strcode + "')as sqCode  from bill_main a where billCode in (select billCode from bill_ybbx_fysq where sqCode='" + strcode + "')";
        }

        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");


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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e == null)
        { return; }

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[6].Text = state;
            }
            // e.Item.Cells[7].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../fysq/travelReportDetail.aspx?Ctrl=View&Code=" + e.Item.Cells[7].Text + "');\">" + e.Item.Cells[7].Text + "</a>";
            e.Item.Cells[1].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../bxgl/bxDetailFinal.aspx?type=look&billCode=" + e.Item.Cells[0].Text + "');\">" + e.Item.Cells[1].Text + "</a>";
            //bxDetailFinal.aspx?type=look&djtype=qtbx&billCode=
            string strcode = e.Item.Cells[7].Text.Trim();

            string codeType = strcode.Substring(0, 4);
            if (codeType == "ccsq")
            {
                //出差申请
                e.Item.Cells[7].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../fysq/travelReportDetail2.aspx?Ctrl=View&AppCode=" + e.Item.Cells[7].Text + "');\">" + e.Item.Cells[7].Text + "</a>";

                // window.showModalDialog("../fysq/travelReportDetail.aspx?Ctrl=View&AppCode=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
            }
            else if (codeType == "lscg")
            {
                //报告申请
                e.Item.Cells[7].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../fysq/lscgDetail.aspx?type=look&cgbh=" + e.Item.Cells[7].Text + "');\">" + e.Item.Cells[7].Text + "</a>";

                //window.showModalDialog("../fysq/lscgDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:560px;dialogWidth:940px;status:no;scroll:yes');
            }
            else if (codeType == "cgsp")
            {
                //采购审批
                e.Item.Cells[7].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../fysq/cgspDetail.aspx?type=look&cgbh=" + e.Item.Cells[7].Text + "');\">" + e.Item.Cells[7].Text + "</a>";

                //  window.showModalDialog("../fysq/cgspDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
            }
            else
            {
            }
        }
    }
}
