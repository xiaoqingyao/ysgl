<%@ WebHandler Language="C#" Class="WorkFlowReplace" %>

using System;
using System.Web;
using WorkFlowLibrary.WorkFlowBll;

public class WorkFlowReplace : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        //删除审批流
        string billCode = context.Request["billCode"];
        object objflowid = context.Request["flowid"];
        string strflowid = "";
        if (objflowid == null)
        {
            strflowid = "";
        }
        else
        {
            strflowid = objflowid.ToString();
        }

        if (strflowid.Equals("gkbx"))
        {
            
            try
            {
                    WorkFlowRecordManager wfmgr = new WorkFlowRecordManager();
                    wfmgr.ReplaceForGk(billCode);

            }
            catch (Exception)
            {

                context.Response.ContentType = "text/plain";
                context.Response.Write("-1");
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");

        }
        else
        {
            WorkFlowRecordManager wfmgr = new WorkFlowRecordManager();
            if (wfmgr.Replace(billCode))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("1");
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-1");
            }
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