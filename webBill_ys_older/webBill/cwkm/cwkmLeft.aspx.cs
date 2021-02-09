using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cwkm_cwkmLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string deptCode = "";
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
                TreeNode tNode = new TreeNode("财务科目", "财务科目");
                tNode.NavigateUrl = "cwkmList.aspx?wdheight=" + Convert.ToString(Request["wdheight"]) + "&kmCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);
                (new cwkm()).BindCwkm(this.TreeView1.Nodes[0], "cwkmList.aspx?wdheight=" + Convert.ToString(Request["wdheight"])+"&deptcode="+deptCode+"", "list", "../Resources/Images/treeView/treeNode.gif");
            }
        }
    }
}
