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

public partial class SaleBill_SaleFee_SaleDeptFeelist : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdatefrom = "";
    string strdateto = "";
    string strdeptcode = "";
    string stryskmcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (Request["datefrom"] != null)
        {
            strdatefrom = Request["datefrom"].ToString();
            this.lblfee.Text = "开始日期：" + strdatefrom;
            hdDateFrm.Value = strdatefrom;
        }
        if (Request["dateto"] != null)
        {
            strdateto = Request["dateto"].ToString();
            this.lblfee.Text += "截止日期：" + strdateto;
            hdDateTo.Value = strdateto;
        }
        if (Request["deptcode"] != null)
        {
            strdeptcode = Request["deptcode"].ToString();
            this.lblfee.Text += "部门编号："+strdeptcode;
            hdDeptCode.Value = strdeptcode;
        }
        if (Request["yskmcode"] != null)
        {
            stryskmcode = Request["yskmcode"].ToString();
        }
        if (!IsPostBack)
        {
            this.BindDataGrid();
        }
    }

    public void BindDataGrid()
    {

        string strsql = @"exec SaleBill_SaleDept_yskmInOutTongji '" + strdeptcode + "','"+stryskmcode+"', '" + strdatefrom + "','" + strdateto + "'";
        DataTable temp = server.RunQueryCmdToTable(strsql);
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

        this.myGrid.DataSource = temp;
        myGrid.DataBind();
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    public delegate void MyDelegate(DataGrid gv);

    float fNianChuFenPeiAmount = 0;
    float fFanliAmount = 0;
    float fBaoXiaoAmount = 0;
    float fKeZhiPeiAmount = 0;
    protected void myGridItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer) {
            if (e.Item.Cells[8].Text=="0")
            {
                e.Item.CssClass = "hiddenbill";
            }
            string strNianChuFenPei=e.Item.Cells[4].Text;//年初分配
            string strFanLi=e.Item.Cells[5].Text;//返利
            string strBaoXiao=e.Item.Cells[6].Text;//报销总额
            string strKeZhiPei=e.Item.Cells[7].Text;//可支配
            float fEveNianChuFenPeiAmount = 0;
            if (float.TryParse(strNianChuFenPei, out fEveNianChuFenPeiAmount))
            {
                fNianChuFenPeiAmount += fEveNianChuFenPeiAmount;
            }
            float fEveFanLi = 0;
            if (float.TryParse(strFanLi, out fEveFanLi))
            {
                fFanliAmount += fEveFanLi;
            }
            float fEveBaoXiaoAmount = 0;
            if (float.TryParse(strBaoXiao, out fEveBaoXiaoAmount))
            {
                fBaoXiaoAmount += fEveBaoXiaoAmount;
            }
            float fEveKeZhiPeiAmount = 0;
            if (float.TryParse(strKeZhiPei, out fEveKeZhiPeiAmount))
            {
                fKeZhiPeiAmount += fEveKeZhiPeiAmount;
            }
        }
        else if (e.Item.ItemType==ListItemType.Footer)
        {
            e.Item.Cells[1].Text = "合计：";
            e.Item.Cells[1].CssClass = "myGridItemRight";
            e.Item.Cells[4].Text = fNianChuFenPeiAmount.ToString() ;
            e.Item.Cells[5].Text = fFanliAmount.ToString();
            e.Item.Cells[6].Text = fBaoXiaoAmount.ToString();
            e.Item.Cells[7].Text = fKeZhiPeiAmount.ToString();
        }
    }
}
