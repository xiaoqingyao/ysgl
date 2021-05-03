<%@ WebHandler Language="C#" Class="Proposal" %>

using System;
using System.Web;


public class Proposal : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    //我要吐槽
    public void ProcessRequest(HttpContext context)
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        prop pl;
        try
        {
            byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
            string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
            //反序列化json需要framework3.5sp1
            using (System.IO.StringReader sr = new System.IO.StringReader(jsonStr))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                pl = (prop)serializer.Deserialize(new Newtonsoft.Json.JsonTextReader(sr), typeof(prop));
            }
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            //反序列化失败
            context.Response.Write("-1");
            return;
        }

        try
        {
            if (server.ExecuteNonQuery("insert into bill_proposal(usercode,title,tp,des,createdate) values('"+context.Session["userCode"].ToString()+"','" + pl.title + "','" + pl.type + "','" + pl.desc + "','"+DateTime.Now.ToLongTimeString()+"')") == -1)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-0");
            }
            else
            {
                //ysMgr.InsertYsmx(list, main);
                context.Response.ContentType = "text/plain";
                context.Response.Write("1");
            }
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-2");
        }
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    class prop
    {
        public int type { get; set; }
        public string title { get; set; }
        public string desc { get; set; }

    }
}