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

public partial class message_messageList : System.Web.UI.Page
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
        string sql = "select id, title, contents, writer, convert(varchar(10),date,121) as date, readTimes, mstype, notifierid,notifiername,endtime,Accessories,Row_Number()over(order by date desc) crow  from bill_msg where writer='" + Session["userCode"].ToString() + "'";
        DataTable dt = PubMethod.GetPageData(sql, "messageList.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }





    protected void DeleteMsg(object sender, EventArgs e)
    {
        string sql = "delete from bill_Msg where id=" + hfid.Value;
        int row = server.ExecuteNonQuery(sql);
        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');window.location.href='messageList.aspx';", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
        }

    }

}
