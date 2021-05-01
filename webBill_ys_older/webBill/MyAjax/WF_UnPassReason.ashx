<%@ WebHandler Language="C#" Class="WF_UnPassReason" %>

using System;
using System.Web;
using WorkFlowLibrary.WorkFlowBll;

public class WF_UnPassReason : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
             string billName = context.Request["billname"];
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

              string sql = @" select top 1 mind 
                         from workflowrecords
                         where rdstate='3' and
 
                         recordid=(
			                          select w.recordid
				                        from workflowrecord w,bill_main m
				                        where (w.billCode = m.billCode or w.billCode = m.billName)
				                        and w.billCode='" + billName + @"' 
				                        group by w.recordid
			                          ) ";
        string reason = server.GetCellValue(sql);
        context.Response.ContentType = "text/plain";
        context.Response.Write(reason);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}