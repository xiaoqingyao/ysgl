<%@ WebHandler Language="C#" Class="helpHandler" %>

using System;
using System.Web;

public class helpHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        object objshowname = context.Request["menuname"];
        if (objshowname == null || objshowname.ToString().Equals(""))
        {
            context.Response.ContentType = "text/HTML";
            context.Response.Write("");
            return;
        }
        string strmenuname = HttpUtility.UrlDecode(objshowname.ToString());
        string strrel = new Bll.sysMenuHelpBLL().GetContentByShowName(strmenuname);
        context.Response.ContentType = "text/HTML";
        context.Response.Write(strrel);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}