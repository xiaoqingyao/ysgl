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

public partial class message_messageGetList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        string pageNav = "";
        string dtime = DateTime.Now.ToString("yyyy-MM-dd");
        string userCode = Convert.ToString(Session["userCode"]);
        string username = Convert.ToString(Session["userName"]);
        string sql = "select Row_Number()over(order by date desc) as crow ,id,title,(select username from bill_users where usercode = writer) as writer,contents,CONVERT(varchar(100),date, 23) as date ,isnull(readtimes,0) as readtimes,mstype,endtime from bill_msg where endtime>='" + dtime + "' and  (notifiername like'%" + userCode + "%' or notifiername like'%" + username + "%') and mstype='通知' or (mstype='新闻'  and endtime>='"+dtime+"' )";
        DataTable dt = PubMethod.GetPageData(sql, "messageGetList.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }

}
