<%@ WebHandler Language="C#" Class="GetDeskUndo" %>

using System;
using System.Web;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;

public class GetDeskUndo : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
  
    public void ProcessRequest(HttpContext context)
    {
        StringBuilder sb = new StringBuilder();
        string userCode = Convert.ToString(context.Session["userCode"]);
        string[] arrBillType = new string[] { @"gkbx", "ys", "yszj", "ystz", "lscg", "cgsp", "ybbx", "qtbx", "xmys", "cgzjjh", "xmzf", "ccsq", "ccbg", "kpsq", "tsfl", "yzsq", "kmystz", "yzsq", "jksq", "srys", "zcys"
            , "chys", "wlys", "srd", "zcgz", "chly", "wlfk", "hksq", "yksq_dz", "yshz", "xmys", "xmyshz","jfsq","tfsq","yszjhz","zfzxsqd","nzfzxsqd","fzcgz","xyth" };
        string[] arrBillTypeName = new string[] { @"费用报销单", "费用预算", "预算追加", "预算调整", "报告申请", "采购审批", "借款申请", "其他报销", "项目预算", "采购资金计划", "项目支付申请"
            , "出差申请", "报告申请", "开票申请", "特殊返利申请", "借款申请单", "科目预算调整", "预支单", "借款单", "收入预算", "固定资产预算", "存货预算", "往来预算", "收入报告", "固定资产申购单", "存货领用"
            , "往来付款", "还款申请", "费用报销单", "预算汇总单", "项目预算填报", "项目预算汇总","经费申请单","退费申请单","预算追加汇总","跨区域转校转费申请单","区域内转校转费申请单","物品申购单","学员特惠信息表" };
        string[] dydj = new string[] { "01", "02", "03", "04", "05", "06" };

      
        WorkFlowRecordManager wfMgr = new WorkFlowRecordManager();
        for (int i = 0; i < arrBillType.Length; i++)
        {
            int  cont = wfMgr.GetAppraveCount(userCode, arrBillType[i]);
         
            
            if (cont > 0)
            {
                sb.Append("<li class=\"infoLi\" style=\"list-style-type:none \">");
                sb.Append("<span class=\"addTabs\" linkname=\"");
                sb.Append(arrBillTypeName[i]);

                sb.Append("审核");
                sb.Append("\" datalink=\"../MyWorkFlow/BillMainToApprove.aspx?flowid=");
                sb.Append(arrBillType[i]);
                if (arrBillType[i] == "srys" || arrBillType[i] == "srd")
                {
                    sb.Append("&yskmtype=01");
                }
                if (arrBillType[i] == "ys" || arrBillType[i] == "ybbx")
                {
                    sb.Append("&yskmtype=02");
                }
                if (arrBillType[i] == "yksq_dz")
                {
                    sb.Append("&yskmtype=06");
                }
                if (arrBillType[i] == "zcys" || arrBillType[i] == "zcgzbx")
                {
                    sb.Append("&yskmtype=03");
                }
                if (arrBillType[i] == "chys" || arrBillType[i] == "chly")
                {
                    sb.Append("&yskmtype=04");
                }
                if (arrBillType[i] == "wlys" || arrBillType[i] == "wlfk")
                {
                    sb.Append("&yskmtype=05");
                }
                if (new Bll.ConfigBLL().GetValueByKey("dz_syys_flg").Equals("1"))//判断如果是大智 加此参数
                {
                    sb.Append("&isdz=1");
                }
                sb.Append("\"><img src='../images/icon2_030.png'  width='12px' height='12px' />&nbsp;&nbsp;&nbsp;");
                sb.Append(arrBillTypeName[i]);
                sb.Append("有【<font color='red'>");
                sb.Append(cont.ToString());
                sb.Append("</font>】张单据待审核");
                sb.Append("</span>");
            }
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(sb.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}