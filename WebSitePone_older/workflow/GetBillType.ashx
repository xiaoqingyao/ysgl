<%@ WebHandler Language="C#" Class="GetBillType" %>

using System;
using System.Web;

public class GetBillType : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        string billcode = context.Request.Params["billcode"];
        string flowid = "";
        //根据传入的参数判断是否存在
        string strsql = @" select count(*) from workflowrecord where billcode='" + billcode + "'";
        string strcount = server.GetCellValue(strsql);
        if (strcount == "0")//根据billname 获取billcode 重新赋值
        {
            billcode = server.GetCellValue("select billcode from bill_main where billname='" + billcode + "'");

        }

        flowid = server.GetCellValue("select flowid from workflowrecord where billcode='" + billcode + "'");
        string strisdz = "";
        if (!string.IsNullOrEmpty(context.Request["isdz"]))
        {
            strisdz = context.Request["isdz"].ToString();
        }
        string ret = "";
        if (strisdz == "1")
        {
            switch (flowid)
            {
                case "ybbx"://一般报销
                case "yksq_dz"://用款申请
                case "sr":
                case "gdzcgz":
                case "chly"://存货领用单
                case "srd"://收入报告单
                case "wlfk"://往来付款单
                case "zcgzbx"://资产购置报销
                    ret = "../bxd/ybbxView.aspx?type=audit&billCode=" + billcode + "&isdz=" + strisdz;
                    break;
                case "lscg"://临时采购
                    ret = "../BillBgsq/bgsqView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "ccsq"://出差申请
                    ret = "../BillTravelApply/travelApplyView.aspx?type=audit&billCode=" + billcode;
                    break;

                case "cgsp"://采购审批
                    ret = "../fysq/cgspDetail.aspx?type=audit&cgbh=" + billcode;
                    break;

                case "yszj":
                    ret = "../BillYszj/YszjAuditView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "ystz":
                    ret = "../BillYstz/YstzAuditView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "tfsq":
                    ret = "../bxd/ybbxView_tf.aspx?type=audit&billCode=" + billcode + "&isdz=" + strisdz;
                    break;
                case "ys":
                case "srys":
                case "zcys":
                case "chys":
                case "wlys":
                    ret = "../BillYs/YsAuditView.aspx?type=audit&billCode=" + billcode;
                    ////ret = "../ysgl/cwtbDetail.aspx?from=lookDialog&billCode=" + billcode;
                    ////break;
                    //Models.Bill_Main main = new Dal.Bills.MainDal().GetMainByCode(billcode);
                    //string strdeptcode = main.BillDept;
                    //string strnd = main.BillName.Substring(0, 4);
                    //ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + strdeptcode + "&nd=" + strnd + "&type=ystb&look=look";
                    break;
                case "cgzjjh":
                    ret = "../cgzj/cgzjDetail.aspx?type=look&par=" + billcode;
                    break;

                case "xmzf":
                    ret = "../xmzf/xmzfsqDetail.aspx?type=look&billCode=" + billcode;
                    break;
                case "qtbx"://其它报销
                    ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode;
                    break;

                case "ccbg"://出差报告
                    ret = "../fysq/travelReportDetail.aspx?Ctrl=look&Code=" + billcode;
                    break;
                case "tsfl"://特殊返利申请单
                    ret = "../../SaleBill/Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "kpsq"://开票申请
                    ret = "../../SaleBill/kpsq/KpsqDetails.aspx?type=audit&&bh=" + billcode;
                    break;
                case "cksj"://车款上缴明细表
                    ret = "../../SaleBill/RemitTance/RemitTanceDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "jksq"://借款申请
                    ret = "../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "zccgsq"://资产采购申请单

                    ret = "../ZiChanGuanLi/CaiGouShenQingDetail.aspx?type=audit&bh=" + billcode;
                    break;
                case "zcczd"://资产处置单
                    ret = "../ZiChanGuanLi/ChuZhiDanDetail.aspx?type=audit&bh=" + billcode;
                    break;
                case "wxsq"://维修申请
                    ret = "../ZiChanGuanLi/WeiXiuShenQingDetail.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "jfsq"://经费申请
                    ret = "../BillJfsq/JfspView.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "gkbx"://归口报销

                    string billname = server.GetCellValue("select top 1 billName from bill_main where billcode='" + billcode + "'");
                    ret = "../bxgl/bxDetailForGK.aspx?type=look&billCode=" + billname;
                    break;
            }
        }
        else
        {



            switch (flowid)
            {
                case "ybbx"://一般报销
                case "yksq_dz"://用款申请
                case "sr":
                case "gdzcgz":
                case "chly"://存货领用单
                case "srd"://收入报告单
                case "wlfk"://往来付款单
                case "zcgzbx"://资产购置报销
                    ret = "../bxd/ybbxView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "lscg"://临时采购
                    ret = "../BillBgsq/bgsqView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "ccsq"://出差申请
                    ret = "../BillTravelApply/travelApplyView.aspx?type=audit&billCode=" + billcode;
                    break;

                case "cgsp"://采购审批
                    ret = "../fysq/cgspDetail.aspx?type=audit&cgbh=" + billcode;
                    break;

                case "yszj":
                    ret = "../BillYszj/YszjAuditView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "ystz":
                    ret = "../BillYstz/YstzAuditView.aspx?type=audit&billCode=" + billcode;
                    break;
                case "ys":
                case "srys":
                case "zcys":
                case "chys":
                case "wlys":
                    ret = "../BillYs/YsAuditView.aspx?type=audit&billCode=" + billcode;
                    ////ret = "../ysgl/cwtbDetail.aspx?from=lookDialog&billCode=" + billcode;
                    ////break;
                    //Models.Bill_Main main = new Dal.Bills.MainDal().GetMainByCode(billcode);
                    //string strdeptcode = main.BillDept;
                    //string strnd = main.BillName.Substring(0, 4);
                    //ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + strdeptcode + "&nd=" + strnd + "&type=ystb&look=look";
                    break;
                case "cgzjjh":
                    ret = "../cgzj/cgzjDetail.aspx?type=look&par=" + billcode;
                    break;
                case "tfsq":
                    ret = "../bxd/ybbxView_tf.aspx?type=audit&billCode=" + billcode + "&isdz=" + strisdz;
                    break;
                case "xmzf":
                    ret = "../xmzf/xmzfsqDetail.aspx?type=look&billCode=" + billcode;
                    break;
                case "qtbx"://其它报销
                    ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode;
                    break;

                case "ccbg"://出差报告
                    ret = "../fysq/travelReportDetail.aspx?Ctrl=look&Code=" + billcode;
                    break;
                case "tsfl"://特殊返利申请单
                    ret = "../../SaleBill/Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "kpsq"://开票申请
                    ret = "../../SaleBill/kpsq/KpsqDetails.aspx?type=audit&&bh=" + billcode;
                    break;
                case "cksj"://车款上缴明细表
                    ret = "../../SaleBill/RemitTance/RemitTanceDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "jksq"://借款申请
                    ret = "../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "zccgsq"://资产采购申请单

                    ret = "../ZiChanGuanLi/CaiGouShenQingDetail.aspx?type=audit&bh=" + billcode;
                    break;
                case "zcczd"://资产处置单
                    ret = "../ZiChanGuanLi/ChuZhiDanDetail.aspx?type=audit&bh=" + billcode;
                    break;
                case "wxsq"://维修申请
                    ret = "../ZiChanGuanLi/WeiXiuShenQingDetail.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "jfsq"://经费申请
                    ret = "../BillJfsq/JfspView.aspx?Ctrl=View&Code=" + billcode;
                    break;
                case "gkbx"://归口报销
                    string billname = server.GetCellValue("select top 1 billName from bill_main where billcode='" + billcode + "'");
                    ret = "../bxgl/bxDetailForGK.aspx?type=look&billCode=" + billname;
                    break;
            }
        }
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