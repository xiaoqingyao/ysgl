using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class webBill_tjbb_deptYsTj_Dept : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strflowid = "ybbx";
    string strdydj = "02";
    string kssj = "";
    string jzsj = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}


        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["flowid"]))
            {
                strflowid = Request["flowid"];
                strdydj = server.GetCellValue(@"	  select diccode from bill_datadic where dictype='18' and note2='"+strflowid+"' ");
            }
            this.bindData();
        }
    }

    public void bindData()
    {
         kssj = Page.Request.QueryString["kssj"].ToString().Trim();
         jzsj = Page.Request.QueryString["jzsj"].ToString().Trim();
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string kmCode = Page.Request.QueryString["fykm"].ToString().Trim();
        string stepid = Page.Request.QueryString["stepid"].ToString().Trim();
        deptCode = deptCode.Replace('|', ',');
        string strsql = @"exec pro_bill_bxtj_dept_detail2 '" + kssj + "','" + jzsj+" 23:59:59" + "','" + deptCode + "','" + kmCode + "','" + stepid + "','" + strflowid + "'";
        //Response.Write(strsql);
        DataSet temp = server.GetDataSet(strsql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (e.Item.Cells[0].Text.ToString().Trim().IndexOf("合计") >= 0)
            { }
            else
            {
                if (e.Item.Cells[9].Text.ToString().Trim() == "一般报销")
                {
                    e.Item.Cells[6].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailFinal.aspx?dydj=" + strdydj + "&type=look&billCode=" + e.Item.Cells[8].Text.ToString().Trim() + "');\">" + (e.Item.Cells[6].Text.ToString().Trim() == "" ? "暂无摘要" : e.Item.Cells[6].Text.ToString().Trim()) + "</a>";
                }
                else
                {
                    e.Item.Cells[6].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailFinal.aspx?dydj=" + strdydj + "&type=look&billCode=" + e.Item.Cells[8].Text.ToString().Trim() + "');\">" + (e.Item.Cells[6].Text.ToString().Trim() == "" ? "暂无摘要" : e.Item.Cells[6].Text.ToString().Trim()) + "</a>";
                }
            }
        }
    }

}
