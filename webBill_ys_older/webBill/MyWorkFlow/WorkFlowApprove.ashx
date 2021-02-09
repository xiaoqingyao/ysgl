<%@ WebHandler Language="C#" Class="WorkFlowApprove" %>

using System;
using System.Web;

public class WorkFlowApprove : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        if (context.Session["userCode"] == null || context.Session["userCode"].ToString().Trim() == "")
        {
            context.Response.Redirect("../../webBill.aspx");
            return;
        }
        string action = context.Request.Params["action"];
        string billcode = context.Request.Params["billcode"];//获取的格式应为billcode*mind,billcode*mind
        if (string.IsNullOrEmpty(billcode))
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
        }
        string[] arrbillcodemind = billcode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < arrbillcodemind.Length; i++)
        {
            string mind = "";

            //
            //if (arrbillcodemind[i].Split('*').Length>2)
            //{
                string[] strs = arrbillcodemind[i].Split('*');

                mind = strs[1].ToString();//arrbillcodemind[i].Split('*')[1].ToString();
                mind = HttpUtility.UrlDecode(mind);
            //} 


                string evebillcode = strs[0].ToString();//arrbillcodemind[i].Split('*')[0].ToString();
            //在此加一个标记  判断改编号是否在workflowrecord表中存在  如果不存在 则通过他去bill_main找对应的code
                if (new sqlHelper.sqlHelper().GetCellValue("select count(*) from workflowrecord where billcode='"+evebillcode+"'",null)=="0")
                {
                    evebillcode = new sqlHelper.sqlHelper().GetCellValue("select billcode from bill_main where billname='" + evebillcode + "'");
                }
          
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
        //string mind = context.Request.Params["mind"];



    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}