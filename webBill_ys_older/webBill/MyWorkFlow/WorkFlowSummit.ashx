<%@ WebHandler Language="C#" Class="WorkFlowSummit" %>

using System;
using System.Web;
using Models;
using Bll.UserProperty;

public class WorkFlowSummit : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        if (context.Session["userCode"] == null || context.Session["userCode"].ToString().Trim() == "")
        {
            context.Response.Redirect("../../webBill.aspx");
            return;
        }

        string billcode = context.Request.Params["billcode"];
        string isdz=context.Request.Params["isdz"];
        string flowid = context.Request.Params["flowid"];
        string dept = context.Request.Params["dept"];
        string billtype = "";
        if (string.IsNullOrEmpty(context.Request.Params["billtype"]))
        {
            billtype = flowid;
        }
        else
        {
            billtype = context.Request.Params["billtype"];
        }
        //判断是否可以审批
        //if (flowid == "ys")
        //{
        //    //Bill_Main main = (new BillManager()).GetMainByCode(billcode);

        //    //string cwtb = "1";

        //    //if (new Bll.UserProperty.SysManager().GetsysConfigBynd(main.BillName.Substring(0, 4))["ystbfs"] == "0")  //如果是分解填报就不用判断财务填报了 2012.11.29 sq
        //    //{
        //    //    cwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billcode + "' and yskm in (select yskmcode from bill_yskm where tblx='02')");
        //    //}
        //    //string dwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billcode + "' and yskm in (select yskmcode from bill_yskm where tblx='01')");
        //    //string cwtbkm = server.GetCellValue("select count(1) from bill_yskm_dept where deptCode='" + main.BillDept + "' and yskmcode in (select yskmcode from bill_yskm where tblx='02')");
        //    //if (cwtb == "0" && cwtbkm != "0")
        //    //{
        //    //    context.Response.ContentType = "text/plain";
        //    //    context.Response.Write("-1");
        //    //    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少财务填报部分,不能提交！');", true);
        //    //    return;
        //    //}
        //    //if (dwtb == "0")
        //    //{
        //    //    context.Response.ContentType = "text/plain";
        //    //    context.Response.Write("-2");
        //    //    //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少部门填报部分,不能提交！');", true);
        //    //    return;
        //    //}
        //}
        //
        string usercode = "";

        string[] arrbillcode = billcode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < arrbillcode.Length; i++)
        {
            string evebillcode = arrbillcode[i];
            if (flowid.Equals("gkbx") || (isdz=="1"&&(flowid=="ybbx"||flowid=="yksq_dz"))||flowid=="tfsq")
            {
                usercode = server.GetCellValue(" select top 1 billuser from bill_main where billname='" + evebillcode + "'");
                //如果没有传入dept 那就按照bill_main中的billdept
                if (string.IsNullOrEmpty(dept))
                {
                    dept = server.GetCellValue("select billdept from bill_main where billname='" + evebillcode + "'");
                }
            }
            else
            {
                usercode = server.GetCellValue(" select billuser from bill_main where billcode='" + evebillcode + "'");
                //如果没有传入dept 那就按照bill_main中的billdept
                if (string.IsNullOrEmpty(dept))
                {
                    dept = server.GetCellValue("select billdept from bill_main where billcode='" + evebillcode + "'");
                }
            }
            
            
           WorkFlowLibrary.WorkFlowBll.WorkFlowManager bll= new WorkFlowLibrary.WorkFlowBll.WorkFlowManager() ;
            try
            {
                //1.判断配置项是否根据部门进行设置

                WorkFlowLibrary.WorkFlowModel.WorkFlowRecord wfr = bll.CreateWFRecordNew(evebillcode, flowid, usercode, dept);
                if (wfr.RecordList.Count == 0)
                {
                    bll.UpdateBillToEnd(billcode);
                }
                else
                {
                    WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager mgr = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
                    mgr.InsertRecord(wfr);
                }
            }
            catch (Exception e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-3");
                return;
            }
        }
        WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager remsg = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
        string ret = remsg.WFState(arrbillcode[arrbillcode.Length - 1]);
        context.Response.ContentType = "text/plain";
        context.Response.Write(ret);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}