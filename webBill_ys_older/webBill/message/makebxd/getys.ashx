<%@ WebHandler Language="C#" Class="getys" %>

using System;
using System.Web;

public class getys : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strdate = context.Request["date"];
        string stryskm = context.Request["yskm"];
        string strdeptcode = context.Request["dept"];
        DateTime dt;
        if (!DateTime.TryParse(strdate, out dt))
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
        }

        Bll.UserProperty.YsManager ysmgr = new Bll.UserProperty.YsManager();
        string config = (new Bll.UserProperty.SysManager()).GetsysConfigBynd(dt.Year.ToString())["MonthOrQuarter"];
        string gcbh= ysmgr.GetYsgcCode(dt);
        
        decimal hfje = ysmgr.GetYueHf(gcbh, strdeptcode, stryskm);
        decimal ysje = ysmgr.GetYueYs(gcbh, strdeptcode, stryskm);
        decimal syje = ysje - hfje;
        context.Response.ContentType = "text/plain";
        context.Response.Write(syje.ToString());

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}