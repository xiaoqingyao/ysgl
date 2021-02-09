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
using Bll.Zichan;

public partial class ZiChan_ZiChanGuanLi_ZiChanZengJianFangShi : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ZengJianFangShiBll zjfsbll = new ZengJianFangShiBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            if (!IsPostBack)
            {

                //this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                this.BindDataGrid();
            }
        }

    }

    void BindDataGrid()
    {

        Models.ZiChan_ZengJianFangShi model = new Models.ZiChan_ZengJianFangShi();

        //编号
        if (txtCode.Text != null && txtCode.Text != "")
        {
            model.FangshiCode = txtCode.Text;
        }
        //增减名称
        if (txtname.Text != null && txtname.Text != "")
        {
            model.Fangshiname = txtname.Text;
        }
        //增减方式
        if (DropDownList1.SelectedValue != null && DropDownList1.SelectedValue!= "")
        {

            model.ZjType = DropDownList1.SelectedValue;

        }

        DataTable temp = zjfsbll.GetAllListBySql1(model);
       

        #region 计算分页相关数据

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
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        string strCode = this.hdDelCode.Value;
        string msg = "";
        int iRel = new ZengJianFangShiBll().Delete(strCode);
        if (iRel > 0)
        {
            showMessage("删除成功！", false, "");
        }
        else
        {
            showMessage("删除失败：原因：" + msg, false, "");
        }
        BindDataGrid();
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
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
        //    string zt = e.Item.Cells[0].Text;
        //    if (zt == "end")
        //    {
        //        e.Item.Cells[5].Text = "审批通过";
        //    }
        //    else
        //    {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
        //        string billcode = e.Item.Cells[0].Text;
        //        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        //        string state = bll.WFState(billcode);
        //        e.Item.Cells[5].Text = state;
        //    }
        //    string strBillCode = e.Item.Cells[0].Text;
        //    e.Item.Cells[2].Text = new RemittanceBll().getBillTruckCode(strBillCode);
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

    protected void Button6_Click1(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    public delegate void MyDelegate(DataGrid gv);
}
