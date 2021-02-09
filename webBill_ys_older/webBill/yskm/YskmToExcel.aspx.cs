using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using System.IO;
using System.Drawing;

public partial class webBill_yskm_YskmToExcel : System.Web.UI.Page
{
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
                SysManager sysMgr = new SysManager();
                GridView1.DataSource = sysMgr.GetYskmAll();
                GridView1.DataBind();
            }
        }
       
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        GridView1.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string sjdept = e.Row.Cells[2].Text;
            if (sjdept != "&nbsp;")
            {
                if (sjdept == "01")
                {
                    e.Row.Cells[2].Text = "单位填报";
                }
                else if (sjdept == "02")
                {
                    e.Row.Cells[2].Text = "财务填报";
                    e.Row.Cells[2].ForeColor = Color.Red;
                }
            }
        }
    }
}
