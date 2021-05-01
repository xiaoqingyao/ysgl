<%@ WebHandler Language="C#" Class="WF_GetBillStatus" %>

using System;
using System.Web;
using WorkFlowLibrary.WorkFlowBll;

public class WF_GetBillStatus : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        string billName = context.Request["billname"];
        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        string state = bll.WFState(billName);
        if (state == "未提交")
        {
            string billcode2 = server.GetCellValue("select top 1 billcode from bill_main where billname='" + billName + "'");
            state = bll.WFState(billName);
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(state);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}