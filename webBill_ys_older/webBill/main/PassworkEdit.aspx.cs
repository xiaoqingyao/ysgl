using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Bll.UserProperty;

public partial class webBill_main_PassworkEdit : System.Web.UI.Page
{
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
                this.TextBox4.Text = Convert.ToString(Session["userCode"]);
                UserMessage userMgr = new UserMessage(Convert.ToString(Session["userCode"]));

                if (userMgr.Users.IsSystem == "1")
                {
                    this.TextBox4.Enabled = true;
                }
                else
                {
                    this.TextBox4.Enabled = false;
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        string userCode = this.TextBox4.Text.ToString().Trim();
        UserMessage userMgr = new UserMessage(userCode);
        if (userCode == Convert.ToString(Session["userCode"]))
        {
            if (userMgr.Users.UserPwd != TextBox1.Text)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('原密码错误!')", true);
            }
            else
            {
                userMgr.EditPwd(TextBox2.Text);
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功!')", true);
            }
        }
        else
        {
            userMgr.EditPwd(TextBox2.Text);
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功!')", true);
        }
    }
}
