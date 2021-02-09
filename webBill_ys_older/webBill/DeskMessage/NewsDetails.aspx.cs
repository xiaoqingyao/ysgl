using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Bll.UserProperty;
using System.Text;

public partial class webBill_DeskMessage_NewsDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string action = Request.QueryString["action"];
            if (action == "edit")
            {
                BindData();
            }
        }
    }

    protected void btn_return_Click(object sender, EventArgs e)
    {
        string type = Request.QueryString["type"];
        if (type == "message")
        {
            Response.Redirect("MessageList.aspx");
        }
        else
        {
            Response.Redirect("NewsList.aspx");
        }
        
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string type = Request.QueryString["type"];
        

        TitleMessage news = new TitleMessage();
        news.Context = FCKeditor1.Value;
        news.Title = txt_title.Text;
        news.UserCode = Convert.ToString(Session["userCode"]);        
        news.MessageDate = DateTime.Now;
        news.Memo = txt_memo.Text;
        news.Upfile = hf_upfile.Value;

        string url = "";

        IList<MessageReader> userlist = new List<MessageReader>();
        if (type == "message")
        {
            string code = hf_code.Value;
            if (string.IsNullOrEmpty(code))
            {
                code = new SysManager().GetYbbxBillName("MSG", DateTime.Now.ToString("yyyyMMdd"), 1);
            }
            news.Code = code;
            string userstr = txt_tzr.Text;
            if (!string.IsNullOrEmpty(userstr))
            {
                string[] temps = userstr.Split(':');
                for (int i = 0; i < temps.Length; i++)
                {
                    MessageReader reader = new MessageReader();
                    reader.Usercode = temps[i].Split('[')[0];
                    reader.IsRead = 0;
                    reader.Code = news.Code;
                    userlist.Add(reader);
                }
            }
            news.MsgType = "2";//通知
            url = "MessageList.aspx";
        }
        else
        {
            string code = hf_code.Value;
            if (string.IsNullOrEmpty(code))
            {
                code = new SysManager().GetYbbxBillName("NEWS", DateTime.Now.ToString("yyyyMMdd"), 1);
            }
            news.Code = code;
            news.MsgType = "1";//新闻
            url = "NewsList.aspx";
        }
        news.Userlist = userlist;

        DeskManager deskMgr = new DeskManager();
        deskMgr.Add(news);
        Response.Redirect(url);
    }

    private void BindData()
    {
        
        DeskManager deskMgr = new DeskManager();
        string code = Request.QueryString["code"];
        hf_code.Value = code;
        TitleMessage news = deskMgr.GetNewsByCode(code);
        txt_memo.Text = news.Memo;
        txt_title.Text = news.Title;
        FCKeditor1.Value = news.Context;

        StringBuilder sb = new StringBuilder();
        foreach (MessageReader reader in news.Userlist)
        {
            string usercode = reader.Usercode;
            UserMessage user = new UserMessage(usercode);
            sb.Append(usercode);
            sb.Append("[");
            sb.Append(user.Users.UserName);
            sb.Append("]");
            sb.Append(":");
        }
        if (sb.Length > 0)
        {
            sb.Remove(sb.Length - 1, 1);
            this.txt_tzr.Text = sb.ToString();
        }
    }
}
