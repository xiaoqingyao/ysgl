<%@ WebHandler Language="C#" Class="GetNote" %>

using System;
using System.Web;
using Models;
using System.Collections.Generic;
using Bll.UserProperty;
using System.Web.Script.Serialization;

public class GetNote : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        string date = context.Request.Params["tempDate"];

        if (string.IsNullOrEmpty(date))
        {
            date = DateTime.Now.ToString("yyyy-MM-dd");
        }
        
        string userCode = Convert.ToString(context.Session["userCode"]);
        DateTime dt = Convert.ToDateTime(date);
        
        IList<Bill_NotePad> noteList = new SysManager().GetNoteByUserDate(userCode, dt);
        IList<string> jsList = new List<string>();
        foreach (Bill_NotePad note in noteList)
        {
            jsList.Add(note.Context);
        }
        if (jsList.Count < 1)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("");
        }
        else
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string ret = jserializer.Serialize(jsList);
            context.Response.ContentType = "text/plain";
            context.Response.Write(ret);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}