using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TLibrary.ObjectHelper;

public partial class pwd_changePwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
        if (Cookie != null)
        {
            this.yhm.Text = Server.UrlDecode(Cookie.Values["userName"]);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ymm = this.ysmm.Text.Trim();
        string yhm = this.yhm.Text.Trim();
        //检查原始密码是否正确
        string rel = new sqlHelper.sqlHelper().GetCellValue("select count(*) from bill_users where usercode='" + yhm + "' and userpwd='" + ymm + "'");
        if (rel == "0")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('对不起，原密码输入错误');", true);
            return;
        }
        new sqlHelper.sqlHelper().ExecuteNonQuery("update bill_users set userpwd='" + this.xmm.Text.Trim() + "' where usercode='" + yhm + "'");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('密码修改成功');window.location.href='../Index.aspx'", true);
    }
}