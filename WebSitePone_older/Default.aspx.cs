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

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            trValidate.Visible = false;
            Session["userCode"] = null;
            HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
            if (Cookie != null)
            {
                this.txtUserCode.Value = Cookie.Values["userName"];
                this.txtUserPwd.Value = Cookie.Values["usertPwd"];
                this.CheckBox1.Checked = true;
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
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
                LoginCheck();
            }
        }
        else
        {
            LoginCheck();
        }

    }

    private void LoginCheck()
    {
        string sql = "select * from bill_users where userCode=@userCode and userPwd=@userPwd and userStatus='1'";
        SqlParameter[] par = new SqlParameter[] { 
            new SqlParameter("@userCode",SqlDbType.VarChar,20),
            new SqlParameter("@userPwd",SqlDbType.VarChar,32)
        };
        par[0].Value = txtUserCode.Value;
        par[1].Value = txtUserPwd.Value;
        DataTable temp = new DataTable();
        temp = DataHelper.GetDataTable(sql, par, false);//server.GetDataSet(sql, par);
        if (temp.Rows.Count == 0)
        {
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
            addUserOnline(strusercode);
            #endregion
            if (SetCookie(txtUserCode.Value, txtUserPwd.Value))
                Response.Redirect("Index.aspx");
        }
    }

    private bool SetCookie(string name, string pwd)
    {       
        //下次自动登录
        if (CheckBox1.Checked == true)
        {


            if (!Convert.ToBoolean(hfIsEnableCookie.Value))
            {
                // Response.Write("<script language='javascript'>alert('提示！您的浏览器不接受cookie,将影响一些功能的正常使用,请将浏览器cookie启用！')</script>");
                msg.InnerHtml = "【友情提示】：您的浏览器不接受cookie,将影响一些功能的正常使用,请将浏览器cookie启用！";
                return false;
            }
            else
            {

                HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
                if (Cookie == null)
                {
                    Cookie = new HttpCookie("UserInfo");
                    Cookie.Values.Add("userName", name);
                    Cookie.Values.Add("usertPwd", pwd);
                    //设置Cookie过期时间
                    Cookie.Expires = DateTime.Now.AddMonths(1);//DateTime.Now.AddDays(365);
                    CookiesHelper.AddCookie(Cookie);

                }
                else if (!Cookie.Values["userName"].Equals(name) || !Cookie.Values["usertPwd"].Equals(pwd))
                {
                    CookiesHelper.SetCookie("UserInfo", "userName", name);
                    CookiesHelper.SetCookie("UserInfo", "usertPwd", pwd);
                }
                return true;
            }

        }
        else
        {
            CookiesHelper.RemoveCookie("UserInfo");
            HttpCookie Cookie = CookiesHelper.GetCookie("UserInfo");
            int a = Request.Cookies.Count;
            return true;
        }

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
