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
using System.Data.SqlClient;
using System.IO;
using Microsoft.Win32;

public partial class webBill : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.txtUserCode.Value = "22217";
            BankGroundBind();
        }
    }

    private void BankGroundBind()
    {
        //string BgSql = "select meaning from t_Config where akey='OldaspxBackground' ";
        //DataTable dt = server.GetDataTable(BgSql,null);
        //if (dt.Rows.Count > 0)
        //{
        //    mainTable.Style.Remove("background-image");
        //    mainTable.Style.Add("background-image", dt.Rows[0][0].ToString());
        //}
        //string Newaspx = " select meaning from t_Config where akey='NewAspxLogin'";
        //DataTable tzdt = server.GetDataTable(Newaspx,null);
        //if (tzdt.Rows.Count > 0)
        //{
        //    if (tzdt.Rows[0][0].ToString() == "have")
        //    {
        //        Response.Redirect("logindex.aspx");
        //    }
        //}
        string oldBackGround = ConfigurationManager.AppSettings["oldBackGround"];
        string NewAspxLogin = ConfigurationManager.AppSettings["NewAspxLogin"];
        if (NewAspxLogin == null || NewAspxLogin == "")
        {
            
        }
        else
        {
            Response.Redirect(NewAspxLogin);
        }
        if (oldBackGround == null || oldBackGround == "")
        {

        }
        else
        {
            mainTable.Style.Remove("background-image");
            mainTable.Style.Add("background-image", oldBackGround);
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        if (this.txtCheckCode.Text.Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请输入验证码！');", true);
            txtCheckCode.Focus();
            return;
        }
        if (Session["VNum"] == null)
        {
            Response.Redirect("webBill.aspx");
            Response.End();
        }
        if (this.txtCheckCode.Text.Trim() != Session["VNum"].ToString().Trim())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('验证码错误！');", true);
            txtCheckCode.Focus();
            return;
        }

        string sql = "select * from bill_users where userCode=@userCode and userPwd=@userPwd and userStatus='1'";
        //string sql = "select * from bill_users where userCode=@userCode  and userStatus='1'";



        SqlParameter[] par = new SqlParameter[] { 
            new SqlParameter("@userCode",SqlDbType.VarChar,20),
            new SqlParameter("@userPwd",SqlDbType.VarChar,32)
        };
        par[0].Value = this.txtUserCode.Value.ToString().Trim();
        par[1].Value = this.txtUserPwd.Value.ToString().Trim();

        DataSet temp = server.GetDataSet(sql, par);
        if (temp.Tables[0].Rows.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登录失败！');", true);
        }
        else
        {
            //自动结束已到期的预算填报信息
            server.ExecuteNonQuery("update bill_ysgc set status='2' where jzsj<'" + System.DateTime.Now.ToShortDateString() + "'");
            Session["userCode"] = temp.Tables[0].Rows[0]["userCode"].ToString().Trim();
            Session["userGroup"] = temp.Tables[0].Rows[0]["userGroup"].ToString().Trim();
            Session["userName"] = temp.Tables[0].Rows[0]["userName"].ToString().Trim();
            Session["isSystem"] = temp.Tables[0].Rows[0]["isSystem"].ToString().Trim();
            /*
            ClientScript.RegisterStartupScript(this.GetType(), "", "userLoginSucess();", true);
             */
            Response.Redirect("webBill/main/mainFrame.aspx");
        }
    }
}
