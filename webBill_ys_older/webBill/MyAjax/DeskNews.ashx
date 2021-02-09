<%@ WebHandler Language="C#" Class="DeskNews" %>

using System;
using System.Web;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Web.Script.Serialization;

public class DeskNews : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        DeskManager deskMgr = new DeskManager();
        IList<TitleMessage> title = deskMgr.GetNews(0, 12);
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