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

public partial class webBill_makebxd_selectyskm : System.Web.UI.Page
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
                (new yskm()).BindYskm(this.TreeView1.Nodes[0], "", "", "../Resources/Images/treeView/treeNode.gif", false, "'1'");
            }
        }
        int nodescount = TreeView1.Nodes[0].ChildNodes.Count;
        TreeView1.Nodes[0].ExpandAll();
        TreeView1.Attributes.Add("OnClick", "OnTreeNodeChecked(event)");
    }
}
