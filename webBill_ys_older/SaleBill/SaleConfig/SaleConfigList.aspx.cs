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
using System.Collections.Generic;

public partial class SaleBill_SaleConfig_SaleConfigList : System.Web.UI.Page
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
        }
    }

    public void BindDataGrid()
    {
        string sql = "select Code,CName,ControlNameFirst,ControlNameSecond ,(case Status when '1' then '正常' when '0' then '禁用' end) as status,Months,Remark from T_ControlItem where 1=1";

        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and ( CName like '%" + this.TextBox1.Text.ToString().Trim() + "%' or Remark like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        
        DataSet temp = server.GetDataSet(sql);
        //#region 计算分页相关数据
        //this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        //this.lblItemCount.Text = temp.Tables[0].Rows.Count.ToString();
        //double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        //int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        //this.lblPageCount.Text = pageCount.ToString();
        //this.drpPageIndex.Items.Clear();
        //for (int i = 0; i <= pageCount - 1; i++)
        //{
        //    int pIndex = i + 1;
        //    ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
        //    if (pIndex == this.myGrid.CurrentPageIndex + 1)
        //    {
        //        li.Selected = true;
        //    }
        //    this.drpPageIndex.Items.Add(li);
        //}
        //this.showStatus();
        //#endregion
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    //#region showStatus 分页相关
    //void showStatus()
    //{
    //    if (this.drpPageIndex.Items.Count == 0)
    //    {
    //        this.lBtnFirstPage.Enabled = false;
    //        this.lBtnPrePage.Enabled = false;
    //        this.lBtnNextPage.Enabled = false;
    //        this.lBtnLastPage.Enabled = false;
    //        return;
    //    }
    //    if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
    //    {
    //        this.lBtnNextPage.Enabled = false;
    //        this.lBtnLastPage.Enabled = false;
    //    }
    //    else
    //    {
    //        this.lBtnNextPage.Enabled = true;
    //        this.lBtnLastPage.Enabled = true;
    //    }
    //    //if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
    //    //{
    //    //    this.lBtnFirstPage.Enabled = false;
    //    //    this.lBtnPrePage.Enabled = false;
    //    //}
    //    //else
    //    //{
    //    //    this.lBtnFirstPage.Enabled = true;
    //    //    this.lBtnPrePage.Enabled = true;
    //    //}
    //}

    ////protected void lBtnFirstPage_Click(object sender, EventArgs e)
    ////{
    ////    this.myGrid.CurrentPageIndex = 0;
    ////    this.BindDataGrid();
    ////}
    //protected void lBtnPrePage_Click(object sender, EventArgs e)
    //{
    //    this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
    //    this.BindDataGrid();
    //}
    //protected void lBtnNextPage_Click(object sender, EventArgs e)
    //{
    //    this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
    //    this.BindDataGrid();
    //}
    //protected void lBtnLastPage_Click(object sender, EventArgs e)
    //{
    //    this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
    //    this.BindDataGrid();
    //}
    //protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
    //    this.BindDataGrid();
    //}
    //#endregion

    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    //添加
    protected void Button1_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('SaleConfigDail.aspx?type=add');", true);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        this.hf_sp.Value = this.spcode.Value;
        string kmCodes = this.hf_sp.Value;
        if (kmCodes!=null&&kmCodes!="")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('SaleConfigDail.aspx?type=edit&mCode=" + kmCodes + "');", true);
 
        }
       
        
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string strCwkmCode = spcode.Value.ToString();
        int selectCount = 0;
       
            List<string> list = new List<string>();
            list.Add("delete from T_ControlItem where Code = '" + strCwkmCode + "'");


            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！'); ", true);
            }

       
        this.BindDataGrid();
    }
    /// <summary>
    /// 启用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button5_Click(object sender, EventArgs e)
    {
        string strcode = spcode.Value.ToString();
        string sql = "update T_ControlItem set Status=1 where Code='"+strcode+"'";
        if (server.ExecuteNonQuery(sql)!=-1)
        {

        }
        this.BindDataGrid();
    }
    /// <summary>
    /// 禁用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button6_Click(object sender, EventArgs e)
    {
        string strcode = spcode.Value.ToString();
        string sql = "update T_ControlItem set Status=0 where Code='" + strcode + "'";
        if (server.ExecuteNonQuery(sql) != -1)
        {

        }
        this.BindDataGrid();
    }
}
