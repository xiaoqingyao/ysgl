<%@ WebHandler Language="C#" Class="GetIp" %>

using System;
using System.Web;

public class GetIp : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        context.Response.ContentType = "application/json";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        string ReturnCode = string.Empty;
        string callback = string.Empty;

        if (!string.IsNullOrEmpty(context.Request["callbackparam"]))
        {
            callback = context.Request["callbackparam"];//跨域-必有项。这个是跨域请求的回调，同前台Ajax的配置项jsonp同名（默认也是callback）。
        }
        string type = context.Request["type"];
        string ip = "112.230.234.33:4321";// "192.168.1.158"; //server.GetCellValue("");
        ReturnCode = "{\"Ip\":\""+ip+"\"}";
        //context.Response.Write(ReturnCode);
        context.Response.Write(callback + "(" + ReturnCode + ")");
        context.ApplicationInstance.CompleteRequest();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}