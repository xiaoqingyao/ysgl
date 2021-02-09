using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class yskm_yskmLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("预算科目", "预算科目");
                tNode.NavigateUrl = "yskmList.aspx?wdheight=" + Convert.ToString(Request["wdheight"]) + "&kmCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);
                (new yskm()).BindYskmH(this.TreeView1.Nodes[0], "yskmList.aspx", "list", "../Resources/Images/treeView/treeNode.gif", false, "'1'", "&wdheight=" + Convert.ToString(Request["wdheight"]));
            }
        }
    }
}
