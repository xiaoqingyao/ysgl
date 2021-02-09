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

public partial class webBill_yskm_deptyskmcwLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("所有核算部门/科室", "00");
                tNode.NavigateUrl = "deptyskmcwList.aspx?deptCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);
                string strflg = new Bll.ConfigBLL().GetValueByKey("pingzhengbygkorsy");
                string strsql = "select * from bill_departments where sjdeptcode='000001'";
                if (strflg.Equals("1"))//使用部门
                {
                    strsql = "select deptCode,deptName from bill_departments where sjdeptcode!='000001' and deptcode!='000001' and deptcode not in (select deptcode from bill_departments where sjdeptcode='000001')";
                }

                DataSet temp = server.GetDataSet(strsql);
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["deptName"].ToString().Trim(), temp.Tables[0].Rows[i]["deptCode"].ToString().Trim());
                    sNode.NavigateUrl = "deptyskmcwList.aspx?deptCode=" + temp.Tables[0].Rows[i]["deptCode"].ToString().Trim();
                    sNode.Target = "list";
                    sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
                    tNode.ChildNodes.Add(sNode);
                }
            }
        }
    }
}
