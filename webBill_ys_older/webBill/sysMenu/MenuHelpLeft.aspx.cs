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

public partial class webBill_sysMenu_MenuHelpLeft : System.Web.UI.Page
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
                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】系统菜单", "00");
                tNode.NavigateUrl = "MenuHelpDetail.aspx?id=";
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

               BindOffice(tNode, "MenuHelpDetail.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");
            }
        }

    }

    /// <summary>
    /// 绑定单位的一级菜单
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="otherParameter"></param>
    /// <param name="showChk"></param>
    /// <param name="OrgImgUrl"></param>
    /// <param name="officeImgUrl"></param>
    /// <param name="Status">格式： '1','2','0'</param>
    public void BindOffice(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string Status, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {

        string sql = "  select menuid,menuName,showName from bill_sysMenu where    menuUrl='' and isnull( menuState,1) <> 'D' order by menuOrder  asc";
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["menuid"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["showName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["menuid"].ToString().Trim();

            if (url != "")
            {
                tNode.NavigateUrl = url;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevel(tNode, url, target, otherParameter, showChk, officeImgUrl, Status, showUser, userUrl, userTarget, userImageUrl);
        }
    }

    /// <summary>
    /// 递归绑定下级菜单
    /// </summary>
    /// <param name="orgCode"></param>
    /// <param name="pNode"></param>
    /// <param name="officeDataSet"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="otherParameter"></param>
    /// <param name="showChk"></param>
    /// <param name="OrgImgUrl"></param>
    /// <param name="officeImgUrl"></param>
    /// <param name="Status">格式： '1','2','0'</param>
    public void BindOfficeNextLevel(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string Status, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {

        string sql = " select menuid,menuName,showName from bill_sysMenu where   menuid like '"+pNode.Value+"%' and isnull( menuState,1) <> 'D' and  menuid !='"+pNode.Value+"' order by menuOrder asc";
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["menuid"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["showName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["menuid"].ToString().Trim();

            if (url != "")
            {
                tNode.NavigateUrl = url + "?id=" + tNode.Value + "&name=" + officeDataSet.Tables[0].Rows[i]["showName"].ToString().Trim() + otherParameter;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevel(tNode, url, target, otherParameter, showChk, officeImgUrl, Status, showUser, userUrl, userTarget, userImageUrl);

        }


    }


    public void bindOfficeUser(TreeNode pNode, string userUrl, string target, string imgUrl, string otherParameter)
    {

        //DataSet temp = server.GetDataSet("select * from bill_users where userDept='" + pNode.Value + "' and userStatus='1'");
        //for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        //{
        //    TreeNode tNode = new TreeNode();
        //    tNode.Text = "[" + temp.Tables[0].Rows[i]["userCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["userName"].ToString().Trim();
        //    tNode.Value = temp.Tables[0].Rows[i]["userCode"].ToString().Trim();

        //    if (userUrl != "")
        //    {
        //        tNode.NavigateUrl = userUrl + "?userCode=" + tNode.Value + otherParameter;
        //        tNode.Target = target;
        //    }
        //    if (userUrl != "")
        //    {
        //        tNode.ImageUrl = imgUrl + "user.gif";
        //    }
        //    pNode.ChildNodes.Add(tNode);
        //}
    }

}
