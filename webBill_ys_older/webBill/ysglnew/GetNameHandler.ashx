<%@ WebHandler Language="C#" Class="GetNameHandler" %>

using System;
using System.Web;

public class GetNameHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string strtype = context.Request["type"];
        string strcode = context.Request["code"];
        if (!string.IsNullOrEmpty(strtype) && !string.IsNullOrEmpty(strcode))
        {
            string strname = "";
            switch (strtype)
            {
                case "dept": strname = getdeptname(strcode); break;
                case "yskm": strname = getyskmname(strcode); break;
                default:
                    break;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(strname);
        }
        else
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("传入参数不完整。");
        }

    }
    private string getdeptname(string strcode)
    {
        Bll.UserProperty.SysManager manager = new Bll.UserProperty.SysManager();
        Models.Bill_Departments dept = manager.GetDeptByCode(strcode);
        if (dept == null)
        {
            return "-1";
        }
        else
        {
            return dept.DeptName;
        }
    }
    private string getyskmname(string strcode)
    {
        Models.Bill_Yskm yskm = new Bll.UserProperty.SysManager().GetYskmByCode(strcode);
        if (yskm == null)
        {
            return "-1";
        }
        else
        {
            return yskm.YskmMc;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}