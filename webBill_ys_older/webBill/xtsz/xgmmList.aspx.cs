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
using System.Data;

public partial class webBill_xtsz_xgmmList : System.Web.UI.Page
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
                this.TextBox1.Text = "[" + Page.Request.QueryString["userCode"].ToString().Trim() + "]" + server.GetCellValue("select userName from bill_users where usercode='" + Page.Request.QueryString["userCode"].ToString().Trim() + "'");
            }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (this.TextBox2.Text.ToString().Trim() != this.TextBox3.Text.ToString().Trim())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('两次输入密码不一致！');", true);
            return;
        }
       

        string sql = "update bill_users set userpwd='" + this.TextBox2.Text.ToString().Trim() + "' where usercode='" + Page.Request.QueryString["userCode"].ToString().Trim() + "'";
        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('密码修改失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('密码修改成功！');", true);
        }
    }
}
