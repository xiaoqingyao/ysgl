<%@ WebHandler Language="C#" Class="GetMsg" %>

using System;
using System.Web;

public class GetMsg : IHttpHandler {


    public void ProcessRequest(HttpContext context)
    {
        Bll.UserProperty.Msage mg = new Bll.UserProperty.Msage();

        System.Collections.Generic.IList<Models.Bill_Msg> mglist = mg.GetNews();
        System.Collections.Generic.IList<TempSeria> list = new System.Collections.Generic.List<TempSeria>(); 
        foreach (Models.Bill_Msg model in mglist)
        {
            TempSeria temp = new TempSeria();
            temp.sdate = model.Endtime.ToString();
            temp.title = model.Title;
            temp.code = model.ID.ToString();
            list.Add(temp);
        }
        System.Web.Script.Serialization.JavaScriptSerializer seria = new System.Web.Script.Serialization.JavaScriptSerializer();
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}