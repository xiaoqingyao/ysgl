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

public partial class webBill_yskm_selectCwkm : System.Web.UI.Page
{
    public string deptCode = "";
    public string yskmCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        yskmCode = Page.Request.QueryString["yskmCode"].ToString().Trim();
    }
}