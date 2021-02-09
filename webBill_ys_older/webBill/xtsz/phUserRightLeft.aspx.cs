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
public partial class webBill_xtsz_phUserRightLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("角色信息", "00");
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                DataSet temp = server.GetDataSet("select * from bill_usergroup  order by groupid ");
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    TreeNode node = new TreeNode();
                    node.Text = "[" + temp.Tables[0].Rows[i]["groupid"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["groupname"].ToString().Trim();
                    node.Value = temp.Tables[0].Rows[i]["groupid"].ToString().Trim();

                    node.NavigateUrl = "phUserRightList.aspx?groupID=" + node.Value;
                    node.Target = "list";
                    node.ImageUrl = "../Resources/Images/treeView/treeNode.gif";

                    tNode.ChildNodes.Add(node);
                }
            }
        }
    }
}
