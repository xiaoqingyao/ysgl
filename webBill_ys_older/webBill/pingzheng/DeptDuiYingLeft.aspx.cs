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

public partial class webBill_pingzheng_DeptDuiYingLeft : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                TreeNode tNode = new TreeNode("所有单位", "00");
                tNode.NavigateUrl = "DeptDuiYingList.aspx?deptName=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                //DataSet temp = server.GetDataSet("select * from bill_departments where deptCode in (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')=''))");
                //for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                //{
                //    TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["deptName"].ToString().Trim(), temp.Tables[0].Rows[i]["deptCode"].ToString().Trim());
                //    sNode.NavigateUrl = "DeptDuiYingList.aspx?deptName=" + temp.Tables[0].Rows[i]["deptName"].ToString().Trim();
                //    sNode.Target = "list";
                //    sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
                //    tNode.ChildNodes.Add(sNode);
                   
                //}
                (new Departments()).BindOffice(tNode, "DeptDuiYingList.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");
            }
        }
    }
}
