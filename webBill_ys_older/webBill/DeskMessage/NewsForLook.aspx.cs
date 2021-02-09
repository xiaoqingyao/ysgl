using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;

public partial class webBill_DeskMessage_NewsForLook : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        DeskManager deskMgr = new DeskManager();
        string code = Request.QueryString["code"];
        string usercode = Convert.ToString(Session["userCode"]);
        deskMgr.UpReaded(usercode, code);
        TitleMessage news = deskMgr.GetNewsByCode(code);
        title.InnerHtml = news.Title;
        context.InnerHtml = news.Context;
        lb_fbr.Text = new UserMessage(news.UserCode).Users.UserName;
        if (news.MsgType == "1")
        {
            lb_type.Text = "新闻";
        }
        else
        {
            lb_type.Text = "通知";
        }
    }

}
