<%@ WebHandler Language="C#" Class="GetBillType" %>

using System;
using System.Web;

public class GetBillType : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Dal.ConfigDal configdal = new Dal.ConfigDal();

    public void ProcessRequest(HttpContext context)
    {
        string billcode = context.Request.Params["billcode"];
        string isdz = context.Request.Params["isdz"];
        string flowid = server.GetCellValue("select flowid from bill_main where billcode='" + billcode + "'");
        if (isdz == "1")
        {
            flowid = context.Request["flowid"];
        }



        string deptcode = string.Empty;//部门编号
        string nd = string.Empty;//单据所属年度
        string yskmtype = string.Empty;//预算科目类型  01 收入类 02 费用类 03资产 04存货 05往来  在数据字典中配置
        string tbtype = string.Empty; ;//填报方式  zxer 自下而上  zsex自上而下
        string limittotal = string.Empty; ;//如果是自下而上填报  是否控制填报总金额
        string jecheckflg = string.Empty; ;//如果是自上而下填报  保存时检查方式 0分解金额必须等于总金额  1分解金额小于等于总金额
        string xmcode = context.Request.Params["xmcode"];//新开分校 项目编号
        string strbillname = "";

        Models.Bill_Main main = new Models.Bill_Main();
        if (flowid == "gkbx" || flowid == "tfsq" || (isdz == "1" && (flowid == "ybbx" || flowid == "yksq_dz")))
        {//如果是大智的用款申请和费用报销
            strbillname = billcode;
        }
        else
        {
             main = new Dal.Bills.MainDal().GetMainByCode(billcode);
            strbillname = main.BillName;
        }

        //如果是五大类预算 获取对应的部门和年度
        if (flowid.Equals("ys") || flowid.Equals("yshz") || flowid.Equals("xmys") || flowid.Equals("xmyshz") || flowid.Equals("srys") || flowid.Equals("zcys") || flowid.Equals("chys") || flowid.Equals("wlys"))
        {

            deptcode = main.BillDept;
            nd = main.BillName.Substring(0, 4);


            object objtbtype = context.Request.Params["tbtype"];
            if (objtbtype != null)
            {
                tbtype = objtbtype.ToString();
            }
            object objlimittotal = context.Request.Params["limittotal"];
            if (objlimittotal != null)
            {
                limittotal = objlimittotal.ToString();
            }
            object objjecheckflg = context.Request.Params["jecheckflg"];
            if (objjecheckflg != null)
            {
                jecheckflg = objjecheckflg.ToString();
            }
        }
        object objyskmtype = context.Request.Params["yskmtype"];
        if (objyskmtype != null)
        {
            yskmtype = objyskmtype.ToString();
        }
        string ystzshurl = "YstzDetailNew.aspx";
        string url = configdal.GetValueByKey("ystzshUrl");
        if (!string.IsNullOrEmpty(url))
        {
            ystzshurl = url;
        }


        string ret = "";
        switch (flowid)
        {
            case "ybbx"://一般报销
                if (isdz == "1")
                {
                    ret = "../bxgl/bxDetailForDz.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype;
                }
                else
                {
                    ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype;
                }
                //ret = flowid == "ybbx" ? ret : (ret += "&dydj=" + yskmtype);
                break;
            case "tfsq":
                ret = "../bxgl/jkDetailForDz.aspx?type=audit&billCode=" + billcode;
                break;
            case "srd": ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype; break;//收入单
            case "zcgzbx"://资产购置报销
            case "chly": ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype; break;//存货领用
            case "wlfk": ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype; break;//往来付款
            case "yksq_dz":
                if (isdz == "1")
                {
                    ret = "../bxgl/bxDetailForDz.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype;
                }
                else
                {
                    ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&dydj=" + yskmtype;
                }
                //ret = flowid == "ybbx" ? ret : (ret += "&dydj=" + yskmtype);
                break;
            case "cgsp"://采购审批
                ret = "../fysq/cgspDetail.aspx?type=audit&cgbh=" + billcode;
                break;
            case "lscg"://临时采购
                ret = "../fysq/lscgDetail.aspx?type=audit&cgbh=" + billcode;
                break;
            case "yszj":
                if (strbillname.Substring(0, 3) == "pl_")
                {

                    ret = "../ysgl/yszjAdd_bykm.aspx?type=look&billCode=" + billcode;
                }
                else
                {
                    ret = "../ysgl/yszjEdit.aspx?type=look&billCode=" + billcode;
                }

                break;
            case "ys"://费用预算
                //ret = "../ysgl/cwtbDetail.aspx?from=lookDialog&billCode=" + billcode;
                //break;
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal;
                break;
            case "xmys"://新开分校的项目预算
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&xmcode=" + xmcode + "&xkfx=1";
                break;
            case "yshz"://预算汇总
                ret = "../ysglnew/ystbHzDetail.aspx?ctrl=audit&billcode=" + billcode;
                break;
            case "xmyshz"://新开分校的项目预算 ystbHzDetail.aspx?ctrl=view&billcode=" + billcode + "&xmcode=" + xmcode
                ret = "../ysglnew/ystbHzDetail.aspx?ctrl=audit&billcode=" + billcode + "&xmcode=" + xmcode;
                break;
            case "srys"://收入预算
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal;
                break;
            case "zcys"://资产预算
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal;
                break;
            case "chys"://存货预算
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal;
                break;
            case "wlys"://往来预算
                ret = "../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb&look=look&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal;
                break;
            case "cgzjjh":
                ret = "../cgzj/cgzjDetail.aspx?type=look&par=" + billcode;
                break;
            case "ystz"://预算调整
                ret = "../ysgl/" + ystzshurl + "?type=audit&billCode=" + billcode;
                break;
            case "xmzf":
                ret = "../xmzf/xmzfsqDetail.aspx?type=look&billCode=" + billcode;
                break;
            case "qtbx"://其它报销
                ret = "../bxgl/bxDetailFinal.aspx?type=audit&billCode=" + billcode + "&djdy=01";
                break;
            case "ccsq"://出差申请
                ret = "../fysq/travelApplicationDetails2.aspx?Ctrl=look&Code=" + billcode;
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
            case "yzsq"://借款申请
                ret = "../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=View&Code=" + billcode;
                break;
            case "zccgsq"://资产采购申请单
                ret = "../ZiChanGuanLi/CaiGouShenQingDetail.aspx?type=audit&&bh=" + billcode;
                break;
            case "zcczd"://资产处置单
                ret = "../ZiChanGuanLi/ChuZhiDanDetail.aspx?type=audit&&bh=" + billcode + "&flg=g";
                break;
            case "fzcczd"://资产处置单
                ret = "../ZiChanGuanLi/ChuZhiDanDetail.aspx?type=audit&&bh=" + billcode + "&flg=f";
                break;
            case "wxsq"://维修申请
                ret = "../ZiChanGuanLi/WeiXiuShenQingDetail.aspx?Ctrl=View&&Code=" + billcode;
                break;
            case "gkbx"://归口报销
                string billname = server.GetCellValue("select top 1 billName from bill_main where billcode='" + billcode + "'");
                ret = "../bxgl/bxDetailForGK.aspx?type=look&billCode=" + billname;
                break;
            case "jksq"://借款申请
                ret = "../../SaleBill/BorrowMoney/FundBorrowDetail.aspx?Ctrl=audit&Code=" + billcode;
                break;
            case "hksq"://汇款申请
                ret = "../../SaleBill/BorrowMoney/FundHKDetail.aspx?Ctrl=look&Code=" + billcode;
                break;
            case "kmystz":
                ret = "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + billcode;
                break;
            case "zcgz"://资产购置申请单
                ret = "../fysq/ZcgzsqDetail.aspx?Ctrl=audit&Code=" + billcode;
                break;
            case "ysnzj"://资产购置申请单
                ret = "../ysgl/ysnzjEdit.aspx?type=audit&billCode=" + billcode;
                break;
            case "jfsq"://经费申请
                ret = "../bxgl/ZijinShenqingDetails.aspx?ctrl=audit&billCode=" + billcode;
                break;
            case "yszjhz"://预算追加汇总
                ret = "../ysgl/yszjAddhz.aspx?type=audit&billCode=" + billcode;
                break;

            case "zfzxsqd"://跨区域转校转费申请单
                ret = "../bxgl/zfzxsqd_dzDetails.aspx?ctrl=audit&billcode=" + billcode + "&flg=k";
                break;
            case "nzfzxsqd"://区域内转校转费申请单
                ret = "../bxgl/zfzxsqd_dzDetails.aspx?ctrl=audit&billcode=" + billcode + "&flg=n";
                break;

            case "xyth"://学员特惠关系表
                ret = "../fysq/gxxythxxDetail_dz.aspx?Ctrl=audit&billcode=" + billcode;
                break;

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