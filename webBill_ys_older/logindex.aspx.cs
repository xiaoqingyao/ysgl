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
using System.Windows.Forms;
using Bll;
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowModel;
using WorkFlowLibrary.WorkFlowDal;
using sqlHelper;
using System.Xml;

public partial class logindex : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    string strcon = "";
    private string strcp = "";
    public string Strcp
    {
        get { return strcp; }
        set { strcp = value; }
    }
    private string strobjectname = "";
    public string Strobjectname
    {
        get { return strobjectname; }
        set { strobjectname = value; }
    }
    public string ShowYanZhengMa = "none";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["errorNum"] == null)
        {
            Session["errorNum"] = 0;
        }

        if (!IsPostBack)
        {
            // bdzt();
            //绑定账套
            databind();
            //this.txtUserCode.Value = "22217";
        }
        //bdzt();
        //string strsql2 = "select * from t_Config where akey='Companyname'";
        //DataTable dtcmpname = server.RunQueryCmdToTable(strsql2);
        //if (dtcmpname.Rows.Count > 0 && dtcmpname != null)
        //{
        //    if (dtcmpname.Rows[0]["meaning"].ToString() != null)
        //    {
        //        strcp = dtcmpname.Rows[0]["meaning"].ToString();
        //    }
        //    else
        //    {
        //        
        //    }
        //}

    }

    public void bdzt()
    {

        //IList<ConfigModel> configlist = XmlHelper.GetConfigAllzt();
        //this.zt_drlist.DataSource = configlist;
        //zt_drlist.DataTextField = "Typename";
        //zt_drlist.DataValueField = "Typecode";
        //zt_drlist.DataBind();
    }

    public void databind()
    {





        string image1 = ConfigurationManager.AppSettings["image1"];
        string image2 = ConfigurationManager.AppSettings["image2"];
        string image3 = ConfigurationManager.AppSettings["image3"];
        string name = ConfigurationManager.AppSettings["CustomTitle"];



        Bll.ConfigBLL bllConfig = new Bll.ConfigBLL();
        DataTable dt;
        //  企业简称 
        dt = bllConfig.GetDtByKey("CompanyName");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            name = Convert.ToString(dt.Rows[0]["avalue"]);
        }
        //登录界面图片1
        dt = bllConfig.GetDtByKey("LoginImg1");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            image1 = Convert.ToString(dt.Rows[0]["avalue"]);
        }
        //登录界面图片2
        dt = bllConfig.GetDtByKey("LoginImg2");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            image2 = Convert.ToString(dt.Rows[0]["avalue"]);
        }

        //登录界面图片3
        dt = bllConfig.GetDtByKey("LoginImg3");
        if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["avalue"])))
        {
            image3 = Convert.ToString(dt.Rows[0]["avalue"]);
        }
        string xtname = ConfigurationManager.AppSettings["Strobjectname"];
        if (image1 != "")
        {
            img01.Src = image1;
        }
        if (image2 != "")
        {
            img02.Src = image2;
        }
        if (image3 != "")
        {
            img03.Src = image3;
        }
        if (name != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", " document.title = '" + name + "-预算管理与报销系统" + "'", true);
            strcp = name;
        }
        if (xtname != null)
        {
            strobjectname = xtname;
        }
        //通过验证码判断是否应该让验证码显示
        int icount = Convert.ToInt32(Session["errorNum"]);
        if (icount >= 3)
        {
            ShowYanZhengMa = "block";
        }

    }




    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {



        server = new sqlHelper.sqlHelper();
        bool checkFlg = true;//验证通过标记
        //如果出错次数大于3 让其输入验证码
        if (Session["errorNum"] != null && Convert.ToInt32(Session["errorNum"]) >= 3)
        {
            if (Session["VNum"] == null)
            {
                checkFlg = false;
                Response.Redirect("logindex.aspx");
            }
            else if (this.txtCheckCode.Text.Trim() == "")
            {
                checkFlg = false;
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请输入验证码！');", true);
                txtCheckCode.Focus();
            }
            else if (this.txtCheckCode.Text.Trim() != Session["VNum"].ToString().Trim())
            {
                checkFlg = false;
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('验证码错误！');", true);
                txtCheckCode.Focus();
            }
        }
        //if (this.txtCheckCode.Text.Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请输入验证码！');", true);
        //    txtCheckCode.Focus();
        //    return;
        //}
        //if (Session["VNum"] == null)
        //{
        //    Response.Redirect("logindex.aspx");
        //    Response.End();
        //}
        //if (this.txtCheckCode.Text.Trim() != Session["VNum"].ToString().Trim())
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('验证码错误！');", true);
        //    txtCheckCode.Focus();
        //    return;
        //}
        string struser = this.txtUserCode.Value.ToString().Trim();
        #region 检测点数
        //int irel = checkPoint();
        //if (irel == 2)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('对不起，当前登录人数已经超过了服务器设定的最大数量！');", true);
        //    return;
        //}
        ////else if (irel == 1)
        ////{
        ////    if (checkUserHasOnline(struser))
        ////    {
        ////        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('对不起，该用户已经登录。如果是异常退出，请30秒后再次登录。');", true);
        ////        return;
        ////    }
        ////}
        //else if (irel == 3)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请插入加密狗。');", true);
        //    return;
        //}
        //else { }
        #endregion
        string sql = "select * from bill_users where userCode=@userCode and userPwd=@userPwd and userStatus='1'";
        //string sql = "select * from bill_users where userCode=@userCode  and userStatus='1'";



        SqlParameter[] par = new SqlParameter[] { 
            new SqlParameter("@userCode",SqlDbType.VarChar,20),
            new SqlParameter("@userPwd",SqlDbType.VarChar,32)
        };
        par[0].Value = struser;
        par[1].Value = this.txtUserPwd.Value.ToString().Trim();

        DataSet temp = server.GetDataSet(sql, par);
        if (temp.Tables[0].Rows.Count == 0)
        {
            checkFlg = false;
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('登录失败！');", true);
        }
        //如果没有被验证通过 则不再继续
        if (!checkFlg)
        {
            int icount = Convert.ToInt32(Session["errorNum"]) + 1;
            Session["errorNum"] = icount;
            if (icount >= 3)
            {
                ShowYanZhengMa = "block";
            }
            return;
        }
        //自动结束已到期的预算填报信息   暂时不控制
        //server.ExecuteNonQuery("update bill_ysgc set status='2' where jzsj<'" + System.DateTime.Now.ToShortDateString() + "'");
        string strusercode = temp.Tables[0].Rows[0]["userCode"].ToString().Trim();
        Session["userCode"] = strusercode;
        Session["userGroup"] = temp.Tables[0].Rows[0]["userGroup"].ToString().Trim();
        Session["userName"] = temp.Tables[0].Rows[0]["userName"].ToString().Trim();
        Session["isSystem"] = temp.Tables[0].Rows[0]["isSystem"].ToString().Trim();

        /*
        ClientScript.RegisterStartupScript(this.GetType(), "", "userLoginSucess();", true);
         */
        #region 控制点数
        addUserOnline(strusercode);
        #endregion

        Response.Redirect("webBill/main/mainFrame.aspx");

    }

    /// <summary>
    /// 检测点数
    /// </summary>
    /// <returns></returns>
    private int checkPoint()
    {
        //是否控制点
        bool boControlPoint = new ConfigBLL().GetValueByKey("ISControlPoint").Equals("1");
        if (!boControlPoint)
        {
            return 1;
        }
        //点数
        int ilinecount = new Bll.OnlineBLL().GetOnlineCount();//在线点数
        int iMaxCount = new Bll.OnlineBLL().GetMaxOnlineCount();//最大点数
        if (iMaxCount == 0)
        {
            return 3;//没有狗  因为不可能注册点数为0的狗
        }
        if (iMaxCount <= ilinecount)
        {
            return 2;
        }
        else
        {
            return 1;//成功
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
    /// <summary>
    /// 检测是否已经在线
    /// </summary>
    /// <param name="usercode"></param>
    /// <returns></returns>
    private bool checkUserHasOnline(string usercode)
    {
        bool boControlPoint = new ConfigBLL().GetValueByKey("ISControlPoint").Equals("1");
        if (!boControlPoint)
        {
            return false;
        }
        return new Bll.OnlineBLL().IsExit(usercode);
    }

}
