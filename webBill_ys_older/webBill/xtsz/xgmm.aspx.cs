using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class xtsz_xgmm : System.Web.UI.Page
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
            if (Session["userCode"].ToString().Trim() == "admin")
            {
                this.btnRedirect.Visible = true;
            }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (this.TextBox2.Text.ToString().Trim() != this.TextBox3.Text.ToString().Trim())
        {
            ClientScript.RegisterStartupScript(this.GetType(),"","alert('两次输入密码不一致！');",true);
            return;
        }
        DataSet temp = server.GetDataSet("select * from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "' and userPwd='" + this.TextBox1.Text.ToString().Trim() + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('原密码输入错误！');", true);
            return;
        }
        string sql = "update bill_users set userpwd='" + this.TextBox2.Text.ToString().Trim() + "' where usercode='" + Session["userCode"].ToString().Trim() + "'";
        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('密码修改失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('密码修改成功！');", true);
        }
    }
    protected void btnRedirect_Click(object sender, EventArgs e)
    {
        Response.Redirect("xgmmFrame.aspx");
    }
}
