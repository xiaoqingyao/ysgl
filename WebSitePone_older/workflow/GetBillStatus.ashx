<%@ WebHandler Language="C#" Class="GetBillStatus" %>

using System;
using System.Web;
using System.Text;
using WorkFlowLibrary.WorkFlowDal;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using Bll.UserProperty;
using System.Linq;
using System.Collections.Generic;

public class GetBillStatus : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string billcode = context.Request["billCode"];
        StringBuilder ret = new StringBuilder();
        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        WorkFlowRecord recode = bll.GetWFRecordByBill(billcode);
        if (recode.RecordList.Count < 1)
        {
            ret.Append("<span>未提交</span>");
        }
        else
        {

            int maxid = (from s in recode.RecordList
                         select s.StepId)
                          .Max();

            ret.Append("<table style='color:black;'>");
            ret.Append("<tr><th>序号</th><th>人员</th><th style='margin:0px;padding:0px;'>状态</th><th>审核时间</th></tr>");
            IList<WorkFlowRecords> list = (from l in recode.RecordList where l.RdState != 0 select l).ToList<WorkFlowRecords>();
            for (int i = 0; i < list.Count; i++)
            {
                WorkFlowRecords records = recode.RecordList[i];
                UserMessage umgr = new UserMessage(records.CheckUser);

                ret.Append("<tr>");
                ret.Append("<td>" + records.StepId + "</td>");
                if (umgr == null)
                {
                    ret.Append("<td>" + records.CheckUser + "</td>");
                }
                else
                {
                    ret.Append("<td>[" + umgr.Users.UserCode + "]" + umgr.Users.UserName + "</td>");
                }
                ret.Append("<td>" + GetState(Convert.ToInt32(records.RdState)) + "</td>");
                if (records.RdState == 2)
                    ret.Append("<td>" + Convert.ToDateTime(records.CheckDate).ToString("yyyy-MM-dd") + "</td>");
                else
                    ret.Append("<td>未审核</td>");
                ret.Append("</tr>");
            }
            ret.Append("</table>");
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(ret.ToString());
    }


    private string GetState(int preState)
    {
        string result = "11";
        if (preState == 0)
        {
            result = "等待";
        }
        else if (preState == 1)
        {
            result = "审批中";
        }
        else if (preState == 2)
        {
            result = "通过";
        }
        else if (preState == 3)
        {
            result = "否决";
        }
        return result;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}