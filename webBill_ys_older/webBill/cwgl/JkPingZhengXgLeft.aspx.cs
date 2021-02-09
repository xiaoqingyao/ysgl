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

public partial class webBill_cwgl_JkPingZhengXgLeft : System.Web.UI.Page
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
                tNode.NavigateUrl = "JkPingZhengXgList.aspx?wdheight=" + Convert.ToString(Request["wdheight"]) + "&deptCode=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOffice(tNode, "JkPingZhengXgList.aspx", "list", "&wdheight=" + Convert.ToString(Request["wdheight"]), false, "../Resources/images/treeview/", "", false, "", "", "");
            }
        }
    }
}
