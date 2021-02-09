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

public partial class xtsz_glqxList : System.Web.UI.Page
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
                if (Page.Request.QueryString["userCode"].ToString().Trim() == "")
                {
                    this.Button1.Visible = false;
                    this.lblUserInfo.Text = "请选择相关人员后进行管理权限设置...";
                }
                else
                {
                    this.lblUserInfo.Text = "当前人员：[" + Page.Request.QueryString["userCode"].ToString().Trim() + "]" + server.GetCellValue("select username from bill_users where usercode='" + Page.Request.QueryString["usercode"].ToString().Trim() + "'");
                }

                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
                //tNode.NavigateUrl = "deptList.aspx?deptCode=";
                //tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOffice(tNode, "", "list", "", true, "../Resources/images/treeview/", "", false, "", "", "");

                if (Page.Request.QueryString["userCode"].ToString().Trim() != "")
                {
                    this.bindData();
                }
            }
        }
    }

    void bindData()
    {
        string deptCodes = (new Departments()).GetUserRightDepartments(Page.Request.QueryString["userCode"].ToString().Trim(), "");
        deptCodes = "'" + deptCodes.Replace(",", "','") + "'";
        for (int i = 0; i <= this.TreeView1.Nodes[0].ChildNodes.Count - 1; i++)
        {
            if (deptCodes.IndexOf("'" + this.TreeView1.Nodes[0].ChildNodes[i].Value + "'") >= 0)
            {
                this.TreeView1.Nodes[0].ChildNodes[i].Checked = true;
            }

            this.bindDataNextLevel(this.TreeView1.Nodes[0].ChildNodes[i], deptCodes);
        }
    }

    void bindDataNextLevel(TreeNode pNode, string deptCodes)
    {
        for (int i = 0; i <= pNode.ChildNodes.Count - 1; i++)
        {
            if (deptCodes.IndexOf("'" + pNode.ChildNodes[i].Value + "'") >= 0)
            {
                pNode.ChildNodes[i].Checked = true;
            }

            this.bindDataNextLevel(pNode.ChildNodes[i], deptCodes);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string selectDeptCodes = "";
        for (int i = 0; i <= this.TreeView1.Nodes[0].ChildNodes.Count - 1; i++)
        {
            if (this.TreeView1.Nodes[0].ChildNodes[i].Checked == true)
            {
                selectDeptCodes += "'" + this.TreeView1.Nodes[0].ChildNodes[i].Value + "',";
            }
            else
            {
                this.getNextLevel(this.TreeView1.Nodes[0].ChildNodes[i], ref selectDeptCodes);
            }
        }

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_userright where userCode='" + Page.Request.QueryString["userCode"].ToString().Trim() + "' and rightType='2'");

        if (selectDeptCodes != "")
        {
            selectDeptCodes = selectDeptCodes.Substring(0, selectDeptCodes.Length - 1);
            list.Add("insert into bill_userright(userCode,objectid,righttype) select '" + Page.Request.QueryString["userCode"].ToString().Trim() + "',deptCode,'2' from bill_departments where deptCode in (" + selectDeptCodes + ")");
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
    }

    void getNextLevel(TreeNode pNode, ref string selectDeptCodes)
    {
        for (int i = 0; i <= pNode.ChildNodes.Count - 1; i++)
        {
            if (pNode.ChildNodes[i].Checked == true)
            {
                selectDeptCodes += "'" + pNode.ChildNodes[i].Value + "',";
            }
            else
            {
                this.getNextLevel(pNode.ChildNodes[i], ref selectDeptCodes);
            }
        }
    }
}
