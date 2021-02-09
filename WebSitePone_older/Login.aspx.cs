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
using System.Data.SqlClient;
using Bll;
using Dal;
using TLibrary.ObjectHelper;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            trValidate.Visible = false;
            if (Request["clear"] == null || !Request["clear"].ToString().Equals("1"))
            {
                HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");

                if (Cookie != null)
                {

                    LoginCheck(Server.UrlDecode(Cookie.Values["userName"]), Server.UrlDecode(Cookie.Values["usertPwd"]));
                    this.CheckBox1.Checked = true;
                }
            }

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string code = this.txtUserCode.Value.Trim();
        string pwd = this.txtUserPwd.Value.Trim();
        if (trValidate.Visible == true)
        {

            if (Session["VNum"] == null)
            {
                Server.Transfer("Login.aspx");
                msg.InnerHtml = "";
            }
            else if (string.IsNullOrEmpty(txtCheckCode.Text.Trim()))
            {
                msg.InnerHtml = "【友情提示】：请输入验证码";
            }
            else if (this.txtCheckCode.Text.Trim() != Session["VNum"].ToString().Trim())
            {
                msg.InnerHtml = "【友情提示】：验证码错误";
                txtCheckCode.Focus();
            }
            else
            {
                LoginCheck(code, pwd);
            }
        }
        else
        {
            LoginCheck(code, pwd);
        }

    }

    private void LoginCheck(string code, string pwd)
    {
        string sql = "select * from bill_users where userCode=@userCode and userPwd=@userPwd and userStatus='1'";
        SqlParameter[] par = new SqlParameter[] { 
            new SqlParameter("@userCode",SqlDbType.VarChar,20),
            new SqlParameter("@userPwd",SqlDbType.VarChar,32)
        };
        par[0].Value = code;
        par[1].Value = pwd;
        DataTable temp = new DataTable();
        temp = DataHelper.GetDataTable(sql, par, false);//server.GetDataSet(sql, par);
        if (temp.Rows.Count == 0)
        {
            //msg.InnerHtml = code + ":" + pwd;
            msg.InnerHtml = "【友情提示】：用户名或密码错误";
            trValidate.Visible = true;
        }
        else
        {
            string strusercode = temp.Rows[0]["userCode"].ToString().Trim();
            Session["userCode"] = strusercode;
            Session["userGroup"] = temp.Rows[0]["userGroup"].ToString().Trim();
            Session["userName"] = temp.Rows[0]["userName"].ToString().Trim();
            Session["isSystem"] = temp.Rows[0]["isSystem"].ToString().Trim();
            #region 控制点数
            //addUserOnline(strusercode);
            #endregion
            if (SetCookie(code, pwd))
                Response.Redirect("Index.aspx");
        }
    }

    private bool SetCookie(string name, string pwd)
    {
        Request.Cookies.Remove("UserInfo");
        HttpCookie Cookie = new HttpCookie("UserInfo");
        Cookie.Values.Add("userName", Server.UrlEncode(name));
        Cookie.Values.Add("usertPwd", Server.UrlEncode(pwd));
        //设置Cookie过期时间
        Cookie.Expires = DateTime.Now.AddMonths(60);//DateTime.Now.AddDays(365);
        CookiesHelper.AddCookie(Cookie);
        return true;
        ////下次自动登录
        ////if (CheckBox1.Checked == true)
        ////{


        ////if (!Convert.ToBoolean(hfIsEnableCookie.Value))
        ////{
        ////    // Response.Write("<script language='javascript'>alert('提示！您的浏览器不接受cookie,将影响一些功能的正常使用,请将浏览器cookie启用！')</script>");
        ////    msg.InnerHtml = "【友情提示】：您的浏览器不接受cookie,将影响一些功能的正常使用,请将浏览器cookie启用！";
        ////    return false;
        ////}
        ////else
        ////{

        //    HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
        //    if (Cookie == null)
        //    {
        //        Cookie = new HttpCookie("UserInfo");
        //        Cookie.Values.Add("userName",Server.UrlEncode(name));
        //        Cookie.Values.Add("usertPwd",Server.UrlEncode(pwd));
        //        //设置Cookie过期时间
        //        Cookie.Expires = DateTime.Now.AddMonths(12);//DateTime.Now.AddDays(365);
        //        CookiesHelper.AddCookie(Cookie);

        //    }
        //    else if (!Cookie.Values["userName"].Equals(name) || !Cookie.Values["usertPwd"].Equals(pwd))
        //    {
        //        CookiesHelper.SetCookie("UserInfo", "userName", Server.UrlEncode(name),DateTime.Now.AddMonths(12));
        //        CookiesHelper.SetCookie("UserInfo", "usertPwd", Server.UrlEncode(pwd), DateTime.Now.AddMonths(12));
        //    }
        //    return true;
        ////}

        ////}
        ////else
        ////{
        ////    CookiesHelper.RemoveCookie("UserInfo");
        ////    HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
        ////    int a = Request.Cookies.Count;
        ////    return true;
        ////}

    }
    /// <summary>
    /// 用户登录后 添加进去
    /// </summary>
    /// <param name="usercode"></param>
    private void addUserOnline(string usercode)
    {
        bool boControlPoint = new ConfigBLL().GetValueByKey("ISControlPoint").Equals("1");
        if (boControlPoint)
        {
            new Bll.OnlineBLL().AddUser(usercode);
        }
    }
}
