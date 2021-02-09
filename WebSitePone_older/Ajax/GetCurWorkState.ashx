<%@ WebHandler Language="C#" Class="GetCurWorkState" %>

using System;
using System.Web;
using WorkFlowLibrary.WorkFlowBll;

public class GetCurWorkState : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string code = context.Request.Params["code"];
        if (!string.IsNullOrEmpty(code))
        {
            WorkFlowRecordManager bll = new WorkFlowRecordManager();
            string state = bll.WFState(code);
            context.Response.Write(state);
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