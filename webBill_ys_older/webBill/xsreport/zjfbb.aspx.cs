using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class webBill_xsreport_zjfbb : System.Web.UI.Page
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
                this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";

                //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
                this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                bindData();
            }
        }
    }
    void bindData()
    {
        string begDate = this.TextBox1.Text.Trim();
        string endDate = this.TextBox2.Text.Trim();
        SqlParameter[] sps = { 
                                 new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj",endDate)
                             };
        DataSet ds = server.ExecuteProcedure("bill_pro_report_zjfbb_xs", sps);
        this.myGrid.DataSource = ds;
        this.myGrid.DataBind();
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        bindData();
    }
    protected void btn_excel_Click(object sender, EventArgs e)
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
}
