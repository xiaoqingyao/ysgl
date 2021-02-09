<%@ WebHandler Language="C#" Class="DeleteBill" %>

using System;
using System.Web;
using Bll.UserProperty;

public class DeleteBill : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string billtype = context.Request["type"];
            string billCode = context.Request["billCode"];
         
            BillManager bmgr = new BillManager();
            switch (billtype)
            {
                case "ybbx":
                    bmgr.DeleteYbbx(billCode);
                    break;
                case "gkbx":
                    bmgr.DeleteYbbxsByName(billCode);
                    break;
                case "qtbx":
                    bmgr.DeleteYbbx(billCode);
                    break;
                case "lscg":
                    bmgr.DeleteLscg(billCode);
                    break;
                case "cwtb":
                    bmgr.DeleteCwtb(billCode);
                    break;
                case "zjys":
                    bmgr.DeleteZjys(billCode);
                    break;
                case "ystb":
                    bmgr.DeleteYstb(billCode);
                    break;
                case "cgzjjh":
                    bmgr.DeleteCgzjjh(billCode);
                    break;
                case "cgzjfk":
                    bmgr.DeleteCgzjfk(billCode);
                    break;
                case "cgsp":
                    bmgr.DeleteCgsp(billCode);
                    break;
                case "ystz":
                    bmgr.DeleteYstz(billCode);
                    break;
                case "message":
                    DeskManager deskMgr = new DeskManager();
                    deskMgr.Delete(billCode);
                    break;
                case "xmzf":
                    XmzfManger xmzf = new XmzfManger();
                    xmzf.DeleteXmzfd(billCode);
                    break;
                case "ccsq"://出差申请单
                    bmgr.DeleteTravelApplicationBill(billCode);
                    break;
                case "ccbg"://出差报告单
                    bmgr.DeleteTravelReportBill(billCode);
                    break;
                case "tzcfld"://特种车申请表
                    bmgr.DeleSpecialRebatesApp(billCode);
                    break;
                case "cksj"://车款上缴单
                    bmgr.DeleCarmoney(billCode);
                    break;
                case "bgsq"://报告申请单
                    bmgr.Delebgsqd(billCode);
                    break;
                case "jksq"://借款申请单
                    bmgr.Delejksqpo(billCode);
                    break;
                case "kmystz"://科目预算调整单
                    bmgr.DeleteKmystz(billCode);
                    break;
                case "wxsq"://维修申请
                    bmgr.DelteWeiXiuSqBill(billCode);
                    break;
                case "zcczd"://资产处置
                    bmgr.DeleteZiChanChuZhi(billCode);
                    break;
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }
        catch
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