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
                if (Page.Request.QueryString["userCode"].ToString().Trim() == "")
                {
                    this.Button1.Visible = false;
                    this.lblUserInfo.Text = "请选择相关人员后进行操作权限设置...";
                }
                else
                {
                    this.lblUserInfo.Text = "当前人员：[" + Page.Request.QueryString["userCode"].ToString().Trim() + "]" + server.GetCellValue("select username from bill_users where usercode='" + Page.Request.QueryString["usercode"].ToString().Trim() + "'");
                }
                if (Page.Request.QueryString["userCode"].ToString().Trim() != "")
                {
                    this.bindData();
                }
            }
        }
    }

    void bindData()
    {
        DataSet temp = server.GetDataSet("select * from bill_userright where usercode='" + Page.Request.QueryString["userCode"].ToString().Trim() + "' and righttype='1'");
        if (temp.Tables[0].Rows.Count == 0)//没有设置人员权限
        {
            temp = server.GetDataSet("select * from bill_userright where usercode=(select userGroup from bill_users where usercode='" + Page.Request.QueryString["userCode"].ToString().Trim() + "') and righttype='3'");
        }
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
        list.Add("delete from bill_userright where usercode='" + Page.Request.QueryString["usercode"].ToString().Trim() + "' and righttype='1'");

        DataSet temp = server.GetDataSet("select * from bill_sysmenu");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)Page.FindControl("chk" + temp.Tables[0].Rows[i]["menuid"].ToString().Trim());
            if (chk != null&&chk.Checked==true)
            {
                list.Add("insert into bill_userright(usercode,objectid,righttype) values('" + Page.Request.QueryString["userCode"].ToString().Trim() + "','" + temp.Tables[0].Rows[i]["menuid"].ToString().Trim() + "','1')");
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
