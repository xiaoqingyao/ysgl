using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dept_deptLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
                tNode.NavigateUrl = "qtdeptList.aspx?deptCode=&mxGuid=" + Page.Request.QueryString["mxGuid"].ToString().Trim();
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOffice(tNode, "qtdeptList.aspx", "list", "&mxGuid=" + Page.Request.QueryString["mxGuid"].ToString().Trim(), false, "../Resources/images/treeview/", "", false, "", "", "");
            }
        }
    }
}
