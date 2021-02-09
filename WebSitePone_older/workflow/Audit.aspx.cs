using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;

public partial class workflow_Audit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string[] djArr;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_self');", true);
            Response.Redirect("../Login.aspx");
        }
        Session["123456"] = "1234";
        string djlx = "ybbx,srd,zcgzbx,chly,wlfk,lscg,ccsq,yszj,ystz,ys,srys,tfsq,jfsq,zcys,chys,wlys,yksq_dz";//
        djArr = djlx.Split(',');
        GetremainUL();
    }
    private string GetTitleByFlowid(string flowid)
    {
        string text = server.GetCellValue("select flowName from mainworkflow where flowId='" + flowid + "'");
        if (!string.IsNullOrEmpty(text) && text.IndexOf("审批") == -1)
        {
            text += "审批";
        }
        return text;
    }

    private void GetremainUL()
    {
        int sum = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < djArr.Length; i++)
        {
            string url = GetUrl(djArr[i]);
            string text = GetTitleByFlowid(djArr[i]);
            int count = GetSql(djArr[i]);
            if (count > 0 && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(text))
            {
                sb.Append(LiMakert(url, text, count.ToString()));
            }
            sum += count;

        }
        if (sb.Length == 0)
        {
            remainUL.InnerText = "您暂时没有要审批的单据";
        }
        else
        {
            remainUL.InnerHtml = sb.ToString();
        }
    }


    private string GetUrl(string flowid)
    {
        if (!string.IsNullOrEmpty(flowid))
        {
            string strisdz = "";
            if (!string.IsNullOrEmpty(Request["isdz"]))
            {
                strisdz = Request["isdz"].ToString();
            }
            string url = "";
            switch (flowid)
            {
                case "ybbx"://一般报销 一般报销单
                case "yksq_dz":
                case "chly"://存货领用单
                case "srd"://收入报告单
                case "wlfk"://往来付款单
                case "zcgzbx"://资产购置报销
                case "tfsq"://退费申请
                    url = "ybbxAuditList.aspx?flowid=" + flowid + "&isdz=" + strisdz;
                    break;
                case "lscg"://  临时采购
                    url = "../BillBgsq/bgsqAdult.aspx";
                    break;
                case "ccsq":// 出差申请单
                    url = "../BillTravelApply/travelApplyAudit.aspx";
                    break;
                case "yszj"://预算追加
                    url = "../BillYszj/YszjAuditList.aspx";
                    break;

                case "ystz":// 预算调整

                    url = "../BillYstz/YstzAuditList.aspx";
                    break;
                case "ys":// 费用预算
                case "srys":// 收入预算                 
                case "zcys":// 资产预算                  
                case "chys":// 存货预算                   
                case "wlys":// 往来预算
                    url = "../BillYs/YsAuditList.aspx?flowid=" + flowid;
                    break;
                //case "tfsq"://退费申请
                //    url = "../bxd/ybbxList_tf.aspx?flowid=" + flowid + "&isdz=" + strisdz;
                //    break;
                case "jfsq":// 费用预算
                    url = "../BillJfsq/JsAuditList.aspx?flowid=" + flowid;
                    break;
                default:
                    break;
            }
            return url;
        }
        else
        {
            return "";
        }
    }
    private int GetSql(string type)
    {

        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
        }
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        string request = type;
        string flowid = request;
        if (flowid == "xmzf")
        {
            request = "ybbx";
        }
        string usercode = Session["userCode"].ToString();

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request, strStatus);


        StringBuilder sb = new StringBuilder();
        if (list.Count < 1)
        {

            if (flowid == "gkbx" || flowid == "tfsq" || (strisdz == "1" && (flowid == "ybbx" || flowid == "yksq_dz")))
            {
                sb.Append(@"select (select top 1 billcode from bill_main where billname=main.billName) as billcode, sum(billJe) as billJe
                ,convert(varchar(10),billdate,121) as tbilldate,billname as tbillName,flowid,stepid,billuser,billdate,isgk, billdept as gkdept
                ,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy
                ,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select top 1 billdept from bill_main where billname=main.billname) as billdept
                ,Row_number()over(order by billdate,billName desc) as crow from bill_main main where flowid='" + flowid + "' and 1=2");
            }
            else
            {
                sb.Append(@" select convert(varchar(10),billdate,121) as tbilldate,*,(case flowID when 'ys' then (select '['+gcbh+']'+xmmc from bill_ysgc
                                         where gcbh=bill_main.billName) else billName end) as tbillName ,Row_number()over(order by billdate,billName desc) as crow 
                            from bill_main 
                            where flowid='" + flowid + "' and 1=2 ");
            }
        }
        else
        {
            if (flowid == "gkbx" || flowid == "tfsq" || (strisdz == "1" && (flowid == "ybbx" || flowid == "yksq_dz")))
            {
                sb.Append(@"select (select top 1 billcode from bill_main where billname=main.billName) as billcode, sum(billJe) as billJe
                ,convert(varchar(10),billdate,121) as tbilldate,billname as tbillName,flowid,stepid,billuser,billdate,isgk,(billdept) as gkdept
                ,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy
                ,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select top 1 billdept from bill_main where billname=main.billname) as billdept
                ,Row_number()over(order by billdate,billName desc) as crow from bill_main main where flowid='" + flowid + "' and billname in(");
            }
            else
            {
                sb.Append(@"select convert(varchar(10),billdate,121) as tbilldate,*,(case flowID when 'ys' then (select '['+gcbh+']'+xmmc from bill_ysgc
                            where gcbh=bill_main.billName) else billName end) as tbillName,Row_number()over(order by billdate,billName desc) as crow
                            from bill_main where flowid='" + flowid + "' and billcode in(");
            }
            foreach (string billcode in list)
            {
                sb.Append("'");
                sb.Append(billcode);
                sb.Append("',");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") ");
        }

        if (flowid == "gkbx" || flowid == "tfsq" || (strisdz == "1" && (flowid == "ybbx" || flowid == "yksq_dz")))
        {
            sb.Append(" group by billname,flowid,stepid,billuser,billdate,isgk,billdept ");


        }


        string strsqlcount = "select count(*) from ( {0} ) t ";
        //string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} order by billName desc";



        strsqlcount = string.Format(strsqlcount, sb.ToString());
        //strsqlframe = string.Format(strsqlframe, sb.ToString(), pagefrm, pageto);


        return int.Parse(server.GetCellValue(strsqlcount));
        // return server.GetDataTable(strsqlframe, null);
    }

    private string LiMakert(string url, string text, string count)
    {
        return "  <li><a href='" + url + "' data-ajax='false'>" + text + "<span class='ui-li-count'>" + count + "</span></a></li>";
    }
}
