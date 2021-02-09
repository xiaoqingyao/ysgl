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
using Bll.Zichan;

public partial class webBill_ZiChanGuanLi_ZiChan_Select : System.Web.UI.Page
{
    ZiChan_JiluBll jlbll = new ZiChan_JiluBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindDataGrid();
        }
    }
    void BindDataGrid()
    {
        Models.ZiChan_Jilu model = new Models.ZiChan_Jilu();

        //编号
        if (txtCode.Text != null && txtCode.Text != "")
        {
            model.ZiChanCode = txtCode.Text;
        }
        //增减名称
        if (txtname.Text != null && txtname.Text != "")
        {
            model.ZiChanName = txtname.Text;
        }
        //资产类别
        string strzclb = txtzclb.Text.Trim();
        try
        {
            strzclb = strzclb.Substring(1, strzclb.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            txtzclb.Text = "";
            strzclb="";
        }

        if (!strzclb.Equals(""))
        {

            model.LeiBieCode = strzclb;
        }
        //增减方式
        string strzjfs = txtzjfs.Text.Trim();
        try
        {
            strzjfs = strzjfs.Substring(1, strzjfs.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strzjfs="";
        }

        if (!strzjfs.Equals(""))
        {
            model.ZengJianFangShiCode = strzjfs;
        }
        //使用状况
        string strsyzk = this.txtsyzk.Text;
        try
        {
            strsyzk = strsyzk.Substring(1, strsyzk.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strsyzk="";
        }

        if (!strsyzk.Equals(""))
        {
           
            model.ShiYongZhuangKuangCode = strsyzk;

        }
        //规格型号
        if (txtggxh.Text != null && txtggxh.Text != "")
        {
            model.GuiGeXingHao = txtggxh.Text;
        }
        //使用部门
        string strsydept = txtsydept.Text;
        try
        {
            strsydept = strsydept.Substring(1, strsydept.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strsydept="";
        }

        if (!strsydept.Equals(""))
        {
            
            model.ShiYongBuMenCode = strsydept;
        }
        //采购部门

        string strcgdept = txtcgdept.Text;
        try
        {
            strcgdept = strcgdept.Substring(1, strcgdept.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strcgdept="";
        }

        if (!strcgdept.Equals(""))
        {
           
            model.CaiGouBuMenCode = strcgdept;
        }


        DataTable temp = jlbll.GetAllListBySql1(model);


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
    /// <summary>
    /// 确定选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_sure_click(object sender, EventArgs e)
    {
        int iCount = this.myGrid.Items.Count;
        int selectCount = 0;
        string StrZiChanCode = "";
        for (int i = 0; i < iCount; i++)
        {
            CheckBox chb = this.myGrid.Items[i].FindControl("cb") as CheckBox;
            if (chb == null)
            {
                continue;
            }
            if (chb.Checked)
            {
                string streveZiChanCode = this.myGrid.Items[i].Cells[1].Text.Trim();
                streveZiChanCode = streveZiChanCode.Replace("&nbsp;", "");
                if (!streveZiChanCode.Equals(""))
                {
                    StrZiChanCode += streveZiChanCode + "|&|";
                    selectCount++;
                }
            }
        }
        if (selectCount == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择人员！');</script>");
        }
        else {
            StrZiChanCode = StrZiChanCode.Substring(0, StrZiChanCode.Length - 3);
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + StrZiChanCode + "\";self.close();", true);
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
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
}
