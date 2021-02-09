<%@ WebHandler Language="C#" Class="NoteSave" %>

using System;
using System.Web;
using Models;
using Bll.UserProperty;
using System.Text;

public class NoteSave : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            string userCode = Convert.ToString(context.Session["userCode"]);

            context.Request.ContentEncoding = Encoding.UTF8;
            
            string date = context.Request.Params["tempDate"];
            string text = context.Request.Params["text"];
            Bill_NotePad note = new Bill_NotePad();
            note.UserCode = userCode;
            note.NoteType = "js";
            note.NoteDate = Convert.ToDateTime(date);
            note.Context = text;
            NotePad noteMgr = new NotePad(note);
            noteMgr.Edit();
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}