using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WorkFlow_wkLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet temp = server.GetDataSet("select * from bill_workFlow order by orderBy");

            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                TreeNode tNode = new TreeNode();
                tNode.Text = temp.Tables[0].Rows[i]["flowText"].ToString().Trim();
                tNode.Value = temp.Tables[0].Rows[i]["flowID"].ToString().Trim();

                tNode.NavigateUrl = "webflow_only.html?flowID=" + tNode.Value;
                tNode.Target = "list";

                tNode.ImageUrl = "../webBill/Resources/images/treeview/treeNode.gif";

                this.TreeView1.Nodes[0].ChildNodes.Add(tNode);
            }
        }
    }
}
