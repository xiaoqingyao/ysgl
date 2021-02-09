<%@ WebHandler Language="C#" Class="WorkFlowApprove" %>

using System;
using System.Web;

public class WorkFlowApprove : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {

        if (context.Session["userCode"] == null || context.Session["userCode"].ToString().Trim() == "")
        {
            context.Response.Redirect("../Login.aspx");
            return;
        }
        string action = context.Request.Params["action"];
        string billcode = context.Request.Params["billcode"];//获取的格式应为billcode*mind,billcode*mind

        if (string.IsNullOrEmpty(billcode))
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
        }

        string mind = context.Request.Params["mind"];
        mind = HttpUtility.UrlDecode(mind);
          string strisdz = "";
        if (!string.IsNullOrEmpty(context.Request["isdz"]))
        {
            strisdz = context.Request["isdz"].ToString();
        }
        if (strisdz=="1")
        {
            //根据传入的参数判断是否存在
            string strsql = @" select count(*) from workflowrecord where billcode='" + billcode + "'";
            string strcount = server.GetCellValue(strsql);
            if (strcount == "0")//根据billname 获取billcode 重新赋值
            {
                billcode = server.GetCellValue("select billcode from bill_main where billname='"+billcode+"'");
            }
        }
        string evebillcode = billcode;//arrbillcodemind[i].Split('*')[0].ToString();
      
       
        try
        {
            WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager mgr = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
            string usercode = Convert.ToString(context.Session["userCode"]);

            if (action == "approve")
            {
                mgr.Next(evebillcode, usercode, mind);
            }
            else if (action == "disagree")
            {
                mgr.DisAgree(evebillcode, usercode, mind);
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }
        catch (Exception e)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
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