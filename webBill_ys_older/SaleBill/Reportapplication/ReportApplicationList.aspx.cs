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

public partial class SaleBill_ReportApplication_ReportApplicationList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Bll.ReportAppBLL.ReportApplicationBLL rebll = new Bll.ReportAppBLL.ReportApplicationBLL();
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
                this.TextBox1.Attributes.Add("onfocus", "javascript:setday(this);");
                this.TextBox2.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                this.BindDataGrid();
            }
        }

    }

    void BindDataGrid()
    {
        //string sql = getSelectSql();
        Models.T_ReportApplication model = new Models.T_ReportApplication();
        ////申请日期始
        //if (txtDateFrm.Text != null && txtDateFrm.Text != "")
        //{
        //    specimode.AppDate = txtDateFrm.Text;
        //}
        ////申请日期末
        //if (txtDateTo.Text != null && txtDateTo.Text != "")
        //{
        //    specimode.Note1 = txtDateTo.Text;
        //}
        ////申请单号
        //if (txtCode.Text != null && txtCode.Text != "")
        //{
        //    specimode.Code = txtCode.Text;
        //}
        ////车架号
        //if (txtcarcode.Text != null && txtcarcode.Text != "")
        //{
        //    specimode.TruckCode = txtcarcode.Text;
        //}
        ////有效期始
        //if (TextBox1.Text != null && TextBox1.Text != "")
        //{
        //    specimode.EffectiveDateFrm = TextBox1.Text;

        //}
        ////有效期末
        //if (TextBox2.Text != null && TextBox2.Text != "")
        //{
        //    specimode.EffectiveDateTo = TextBox2.Text;
        //}
        DataTable temp = rebll.getalltable(model);//.GetAllListBySql1(model);



        #region 计算分页相关数据1

        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Rows.Count.ToString();
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
        if (temp.Rows.Count == 0)
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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //{
        //    string zt = e.Item.Cells[1].Text;
        //    if (zt == "end")
        //    {
        //        e.Item.Cells[8].Text = "审批通过";
        //    }
        //    else
        //    {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
        //        string billcode = e.Item.Cells[0].Text;
        //        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        //        string state = bll.WFState(billcode);
        //        e.Item.Cells[8].Text = state;
        //    }
        //}
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../bxgl/bxDetailFinal.aspx?type=look&billCode=" + billCode + "');", true);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {

        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=ybbx&billCode=" + billCode + "');", true);
    }

    public delegate void MyDelegate(DataGrid gv);
 
}
