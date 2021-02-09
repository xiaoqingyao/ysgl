using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_Dept_deptGkLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】归口单位", "00");
                tNode.NavigateUrl = "deptGkList.aspx?wdheight=" + Convert.ToString(Request["wdheight"]) + "&deptCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOfficeGk(tNode, "deptGkList.aspx", "list", "&wdheight=" + Convert.ToString(Request["wdheight"]), false, "../Resources/images/treeview/", "", false, "", "", "");
            }
        }
    }
}