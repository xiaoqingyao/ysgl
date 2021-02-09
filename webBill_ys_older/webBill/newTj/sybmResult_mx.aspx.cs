using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Models;
using Bll.UserProperty;

public partial class webBill_newTj_sybmResult_mx : System.Web.UI.Page
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
                string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
                string yskm = Page.Request.QueryString["kmCode"].ToString().Trim();
                if (dept == "")
                { dept = "统计单位：所有单位"; }
                else
                {
                    SysManager mgr = new SysManager();
                    dept = "统计单位：" + mgr.GetDeptByCode(dept).DeptName.ToString().Trim();
                }
                this.Label1.Text = "开始时间：" + Page.Request.QueryString["kssj"].ToString().Trim() + "  截止时间：" + Page.Request.QueryString["jzsj"].ToString().Trim() + "  " + dept;
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        DateTime begDate = Convert.ToDateTime(Page.Request.QueryString["kssj"].ToString().Trim());
        DateTime endDate = Convert.ToDateTime(DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString());
        string deptcode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string yskm = Page.Request.QueryString["kmCode"].ToString().Trim();

        QueryManger qm = new QueryManger();
        DataTable temp = qm.SearchSybmReportMx(begDate, endDate, deptcode, yskm);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Write( "<script> history.go(-2) </script>" );
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        Response.AddHeader("content-disposition", "attachment; filename=XXExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        this.myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
    }
}
