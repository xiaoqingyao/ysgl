using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_userLeft : System.Web.UI.Page
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
            object objFlg = Request["Flg"];
            if (!IsPostBack)
            {

                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
                if (objFlg != null)
                {
                    tNode.NavigateUrl = "userList.aspx?deptCode=&Flg=" + objFlg.ToString();
                }
                else {
                    tNode.NavigateUrl = "userList.aspx?deptCode=";
                }
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOffice(tNode, "userList.aspx", "list", "&Flg=" + objFlg.ToString(), false, "../Resources/images/treeview/", "", false, "", "", "");
               // (new Departments()).BindOffice(tNode, "userList.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");

            }
        }
    }
}
