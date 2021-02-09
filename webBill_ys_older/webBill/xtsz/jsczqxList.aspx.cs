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

public partial class xtsz_czqxList : System.Web.UI.Page
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
                if (Page.Request.QueryString["groupID"].ToString().Trim() == "")
                {
                    this.Button1.Visible = false;
                    this.lblUserInfo.Text = "请选择相关角色后进行操作权限设置...";
                }
                else
                {
                    this.lblUserInfo.Text = "当前角色：[" + Page.Request.QueryString["groupID"].ToString().Trim() + "]" + server.GetCellValue("select groupname from bill_userGroup where groupID='" + Page.Request.QueryString["groupID"].ToString().Trim() + "'");
                }
                if (Page.Request.QueryString["groupID"].ToString().Trim() != "")
                {
                    this.bindData();
                }
            }
        }
    }

    void bindData()
    {
        DataSet temp = server.GetDataSet("select * from bill_userright where usercode='" + Page.Request.QueryString["groupID"].ToString().Trim() + "' and righttype='3'");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)Page.FindControl("chk" + temp.Tables[0].Rows[i]["objectID"].ToString().Trim());
            if (chk != null)
            {
                chk.Checked = true;
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_userright where usercode='" + Page.Request.QueryString["groupID"].ToString().Trim() + "' and righttype='3'");

        DataSet temp = server.GetDataSet("select * from bill_sysmenu");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)Page.FindControl("chk" + temp.Tables[0].Rows[i]["menuid"].ToString().Trim());
            if (chk != null&&chk.Checked==true)
            {
                list.Add("insert into bill_userright(usercode,objectid,righttype) values('" + Page.Request.QueryString["groupID"].ToString().Trim() + "','" + temp.Tables[0].Rows[i]["menuid"].ToString().Trim() + "','3')");
            }
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
}
