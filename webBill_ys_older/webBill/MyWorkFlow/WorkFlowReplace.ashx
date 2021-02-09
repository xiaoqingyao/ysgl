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
        
        //检查单据状态，检查是否设置审批流，如果没有设置审批流，直接设置成未提交
        string stepid = new sqlHelper.sqlHelper().GetCellValue(" select stepid from workflowstep where flowid='" + objflowid + "' and stepid=1  ");
        string dzzt = new sqlHelper.sqlHelper().GetCellValue(" select 1 from bill_main where billCode='" + billCode + "' and stepID='end' ");
        if (stepid != "1" && dzzt=="1")
        {
            string strsqlup = @"update bill_main set stepid='-1' where billcode='" + billCode + "'";
            int introw = new sqlHelper.sqlHelper().ExecuteNonQuery(strsqlup);

            if (introw > 0)
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
        else
        { 
            string[] arrbillcode = billCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrbillcode.Length; i++)
            {

                if (strflowid.Equals("gkbx"))
                {

                    try
                    {
                        string evebillcode = arrbillcode[i];
                        //在此加一个标记  判断改编号是否在workflowrecord表中存在  如果不存在 则通过他去bill_main找对应的code
                        if (new sqlHelper.sqlHelper().GetCellValue("select count(*) from workflowrecord where billcode='" + evebillcode + "'", null) == "0")
                        {
                            evebillcode = new sqlHelper.sqlHelper().GetCellValue("select billcode from bill_main where billname='" + evebillcode + "'");
                        }
                        WorkFlowRecordManager wfmgr = new WorkFlowRecordManager();


                        if (wfmgr.ReplaceForGk(evebillcode))
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
                    catch (Exception)
                    {

                        context.Response.ContentType = "text/plain";
                        context.Response.Write("-1");
                    }
                    //context.Response.ContentType = "text/plain";
                    //context.Response.Write("1");

                }
                else
                {
                    WorkFlowRecordManager wfmgr = new WorkFlowRecordManager();
                    if (wfmgr.Replace(arrbillcode[i]))
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