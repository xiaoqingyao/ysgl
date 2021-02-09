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

public partial class webBill_search_ystzLeft : System.Web.UI.Page
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
                string type = "";
                string urllist = "";
                if (!string.IsNullOrEmpty(Request["type"]))
                {
                    type = Request["type"].ToString();
                    if (type=="km")
                    {
                        urllist = "kmystzCx";
                    }
                    else
                    {
                        urllist = "ystzList";
                    }
                }
                else
                {
                    urllist = "ystzList";
                }
                
                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
                tNode.NavigateUrl = urllist+".aspx?wdheight=" + Convert.ToString(Request["wdheight"])+"&deptCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);


                string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
                DataSet temp = server.GetDataSet("select * from bill_departments where deptCode in (" + deptCodes + ") and deptCode in (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')=''))");
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["deptName"].ToString().Trim(), temp.Tables[0].Rows[i]["deptCode"].ToString().Trim());
                    sNode.NavigateUrl = urllist+".aspx?deptCode=" + temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "&wdheight=" + Convert.ToString(Request["wdheight"]);
                    sNode.Target = "list";
                    sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
                    tNode.ChildNodes.Add(sNode);
                }
                //(new Departments()).BindOffice(tNode, "deptList.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");
            } 
            //if (!IsPostBack)
            //{
            //    TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
            //    tNode.NavigateUrl = "ystzList.aspx?deptCode=";
            //    tNode.Target = "list";
            //    tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
            //    this.TreeView1.Nodes.Add(tNode);

            //    (new Departments()).BindOffice(tNode, "ystzList.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");
            //}
        }
    }
}
