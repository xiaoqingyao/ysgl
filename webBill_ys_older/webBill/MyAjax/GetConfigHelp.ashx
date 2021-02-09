<%@ WebHandler Language="C#" Class="GetConfigHelp" %>

using System;
using System.Web;

public class GetConfigHelp : IHttpHandler {

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string type = context.Request.Params["type"];
        if (!string.IsNullOrEmpty(type))
        {
           string returnVal=server.GetCellValue("select isnull(szsm,'') from T_config where akey='"+type+"'");
           if (!string.IsNullOrEmpty(returnVal))
           {
               context.Response.Write(returnVal);
           }
           else{
               context.Response.Write("暂时没有设置说明，如果不明白可以联系软件提供商！");
           }
        }
        else
        {
        context.Response.Write("");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}