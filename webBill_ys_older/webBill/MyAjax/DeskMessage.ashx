<%@ WebHandler Language="C#" Class="DeskMessage" %>

using System;
using System.Web;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class DeskMessage : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest (HttpContext context) {
        DeskManager deskMgr = new DeskManager();
        string userCode = Convert.ToString(context.Session["userCode"]);
        IList<TitleMessage> title = deskMgr.GetMessageByReader(userCode, 0, 6);
        IList<TempSeria> list = new List<TempSeria>();
        foreach (TitleMessage model in title)
        {
            TempSeria temp = new TempSeria();
            temp.sdate = model.MessageDate.ToString("yyyy-MM-dd");
            temp.title = model.Title;
            temp.code = model.Code;
            list.Add(temp);
        }
        JavaScriptSerializer seria = new JavaScriptSerializer();
        string json = seria.Serialize(list);
        context.Response.ContentType = "text/plain";
        context.Response.Write(json);
    }

    private class TempSeria
    {
        public string code { get; set; }
        public string sdate { get; set; }
        public string title { get; set; }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}