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

public partial class webBill_fykmYsTj_Yskm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string dydj = "02";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }

        if (!IsPostBack)
        {
            this.bindData();
        }
    }

    public void bindData()
    {
        string kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string jzsj = Page.Request.QueryString["jzsj"].ToString().Trim();
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string fykm = Page.Request.QueryString["fykm"].ToString().Trim();
        string flowid = Request["flowid"].ToString().Trim();
        dydj = Request["dydj"].ToString();

        string strsql = "  exec pro_bill_bxtj_yskm_detail2 '" + kssj + "','" + jzsj + "','" + deptCode + "','" + fykm + "'";//,'" + flowid + "'
        Response.Write(strsql);
        DataSet temp = server.GetDataSet(strsql); //server.GetDataSet("exec pro_bill_bxtj_yskm_detail2 '" + kssj + "','" + jzsj + "','" + deptCode + "','" + fykm + "'");
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
                    e.Item.Cells[6].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailFinal.aspx?type=look&dydj=" + dydj + "&billCode=" + e.Item.Cells[8].Text.ToString().Trim() + "');\">" + (e.Item.Cells[6].Text.ToString().Trim() == "" ? "暂无摘要" : e.Item.Cells[6].Text.ToString().Trim()) + "</a>";
                }
                else
                {
                    e.Item.Cells[6].Text = "<a href=# onclick=\"openDetail('../bxgl/bxDetailFinal.aspx?type=look&&dydj=" + dydj + "&billCode=" + e.Item.Cells[8].Text.ToString().Trim() + "');\">" + (e.Item.Cells[6].Text.ToString().Trim() == "" ? "暂无摘要" : e.Item.Cells[6].Text.ToString().Trim()) + "</a>";
                }
            }
        }
    }
    /// <summary>
    /// 导出 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
