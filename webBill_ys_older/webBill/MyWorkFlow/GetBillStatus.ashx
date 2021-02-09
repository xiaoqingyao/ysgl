<%@ WebHandler Language="C#" Class="GetBillStatus" %>

using System;
using System.Web;
using System.Text;
using WorkFlowLibrary.WorkFlowDal;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Linq;

public class GetBillStatus : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        string billcode = context.Request["billCode"];
        //在此加一个标记  判断改编号是否在workflowrecord表中存在  如果不存在 则通过他去bill_main找对应的code
        if (new sqlHelper.sqlHelper().GetCellValue("select count(*) from workflowrecord where billcode='" + billcode + "'", null) == "0")
        {
            billcode = new sqlHelper.sqlHelper().GetCellValue("select billcode from bill_main where billname='" + billcode + "'");
        }
        string xkfx = "";
        if (context.Request["xkfx"] != null && context.Request["xkfx"].ToString() != "")
        {
            xkfx = context.Request["xkfx"].ToString();
        }
        StringBuilder ret = new StringBuilder();
        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        WorkFlowRecord recode = bll.GetWFRecordByBill(billcode);
        string stepid = server.GetCellValue("select stepID  from bill_main where billCode='" + billcode + "'");
        if (recode.RecordList.Count < 1 && stepid != "end")
        {
            ret.Append("<span>未提交</span>");
        }
        else if (stepid == "end" && recode.RecordList.Count == 0)
        {
            ret.Append("<span>审批通过</span>");
        }
        else
        {
            /*
            ret.Append("<span>");
            IList<WorkFlowRecords> recordsList = recode.RecordList;
            int maxStep = (from lin in recordsList
                           select lin.StepId).Max();
            for (int i = 1; i <= maxStep; i++)
            {
                var temp = from lin2 in recordsList
                           where lin2.StepId == i
                           select lin2;
                if (temp.First().CheckType == "2")
                {
                    
                }
                else
                {
                    
                }
            }
             */
            ret.Append("<span>");
            int preStep = 1;
            int preState = 0;

            foreach (WorkFlowRecords records in recode.RecordList)
            {
                if (records.StepId != preStep)
                {
                    //状态(0,等待;1,正在执行;2,通过;3,废弃)
                    if (preState == 0)
                    {
                        ret.Append(",等待");
                    }
                    else if (preState == 1)
                    {
                        ret.Append(",正在执行");
                    }
                    else if (preState == 2)
                    {
                        ret.Append(",通过");
                    }
                    else if (preState == 3)
                    {
                        ret.Append(",否决");
                    }

                    ret.Append("-->");
                }
                UserMessage umgr = new UserMessage(records.CheckUser);
                if (umgr.Users == null)
                {
                    ret.Append("-->" + records.CheckUser);
                }
                else
                {
                    ret.Append("[" + umgr.Users.UserCode + "]" + umgr.Users.UserName);
                }
                preStep = records.StepId;
                preState = records.RdState;
            }

            if (preState == 0)
            {
                ret.Append(",等待");
            }
            else if (preState == 1)
            {
                ret.Append(",正在执行");
            }
            else if (preState == 2)
            {
                ret.Append(",通过");
            }
            else if (preState == 3)
            {
                ret.Append(",否决");
            }

            ret.Append("-->");

            ret.Append("结束");
            ret.Append("</span>");
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(ret.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}