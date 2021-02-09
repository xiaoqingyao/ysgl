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

public partial class webBill_ysgl_ystzDetailLook : System.Web.UI.Page
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
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        DataSet temp = server.GetDataSet("select a.sCode,b.yskmCode as yskm,b.yskmbm as kmbm,b.yskmmc as kmmc,a.sJe as je,a.sJe_Before as jeBefore from bill_ystz a,bill_yskm b where b.yskmCode=a.skm and a.billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        this.myGrid1.DataSource = temp;
        this.myGrid1.DataBind();
        this.Label3.Text = temp.Tables[0].Rows[0]["sCode"].ToString().Trim();
        this.Label1.Text = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["sCode"].ToString().Trim() + "'");

        temp = server.GetDataSet("select a.tCode,b.yskmCode as yskm,b.yskmbm as kmbm,b.yskmmc as kmmc,a.tJe as je,a.tJe_Before as jeBefore from bill_ystz a,bill_yskm b where b.yskmCode=a.tkm and a.billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        this.myGrid2.DataSource = temp;
        this.myGrid2.DataBind();
        this.Label4.Text = temp.Tables[0].Rows[0]["tCode"].ToString().Trim();
        this.Label2.Text = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["tCode"].ToString().Trim() + "'");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        if (Page.Request.QueryString["type"].ToString().Trim() == "look")
        {
            Response.Redirect("ystz.aspx");
        }
        else if (Page.Request.QueryString["type"].ToString().Trim() == "shLook")
        {
            Response.Redirect("ystzSh.aspx");
        }
        else if (Page.Request.QueryString["type"].ToString().Trim() == "search")
        {
            Response.Redirect("../search/ystzList.aspx?deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
        }
    }
}
