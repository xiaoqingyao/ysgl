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

public partial class webBill_yskm_selectCwkmLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string deptCode = "";
    public string yskmCode = "";
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
                deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
                yskmCode = Page.Request.QueryString["yskmCode"].ToString().Trim();
                (new cwkm()).BindCwkm(this.TreeView1.Nodes[0], "selectCwkmList.aspx?deptcode=" + deptCode + "&yskmCode=" + yskmCode + "", "list", "../Resources/Images/treeView/treeNode.gif");
            }
        }
    }
}
