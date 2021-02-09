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
using System.Text;
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowBll;
using Bll;
using Bll.UserProperty;
using Models;

public partial class bxd_ybbxAuditList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysmgr = new YsManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_self');", true);
            Response.Redirect("../Login.aspx");
        }
        if (!IsPostBack)
        {
            BindData();
        }
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
    private void BindData()
    {
        ltrTitle.Text = GetTitleByFlowid(Request["flowid"]);

        string pageNav = "";
        string sql = GetSql();
        DataTable dt = PubMethod.GetPageData(sql, "ybbxAuditList.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }
    private string GetSql()
    {

        string request = Request.QueryString["flowid"];
        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
        }
        if (string.IsNullOrEmpty(request))
        {
            request = "ybbx";
        }
        string flowid = request;
        if (flowid == "xmzf")
        {
            request = "ybbx";
        }

        string usercode = Session["userCode"].ToString();
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request, strStatus);

        StringBuilder sb = new StringBuilder();

        if (list.Count > 0)
        {
            if (strisdz=="1")
            {


//                string sql = @"select Row_Number()over(order by billdate desc,billName desc) as crow ,sum(je) as billJe,billname,flowid,stepid,billuser,billdate,isgk,(select bxzy from bill_ybbxmxb 
//where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy,
//(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select top 1 billdept from bill_main where billname=main.billname) as billdept,
//(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName
//from bill_main main inner join bill_ybbxmxb_fykm
//fykm on main.billcode=fykm.billcode  where flowid='" + flowid + "' and (billUser='" + Session["userCode"].ToString().Trim() + "' or main.billcode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "') )";


                sb.Append(@"select (case a.flowID when 'ys' then (select '['+gcbh+']'+xmmc from bill_ysgc where gcbh=a.billName) else a.billName end) as tbillName
                     , convert(varchar(10),a.billdate,121) as tbilldate,sum(billJe) as billJe ,a.billName,a.flowID , (select '['+userCode+']'+userName from bill_users 
                        where userCode=a.billUser) as billUser 
                        ,(select bxsm from bill_ybbxmxb where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=a.billName)) as bxsm

                     , isnull((select '['+deptcode+']'+deptName from bill_departments where deptcode=a.billDept),a.billdept) as billdept, a.isgk
                   , Row_number()over(order by a.billdate ,a.billName ) as crow,  (select case when len(bxzy)>20 then substring(bxzy,0,20)+'...'  else bxzy end from bill_ybbxmxb 
                    where billCode =(select top 1 billcode from bill_main where billname=a.billname)) as bxzy from bill_main a,bill_ybbxmxb b 
                     where a.billCode=b.billcode and  a.flowid='" + flowid + "' and a.billname in(");

            }
            else
            {
                sb.Append("select (case a.flowID when 'ys' then (select '['+gcbh+']'+xmmc from bill_ysgc where gcbh=a.billName) else a.billName end) as tbillName, convert(varchar(10),a.billdate,121) as tbilldate,a.billcode,a.billName,a.flowID , (select '['+userCode+']'+userName from bill_users where userCode=a.billUser) as billUser ,a.billJe, isnull((select '['+deptcode+']'+deptName from bill_departments where deptcode=a.billDept),a.billdept) as billdept, a.isgk, Row_number()over(order by a.billdate ,a.billName ) as crow,  (select case when len(bxzy)>20 then substring(bxzy,0,20)+'...'  else bxzy end from bill_ybbxmxb where billCode =a.billCode) as bxzy from bill_main a,bill_ybbxmxb b where a.billCode=b.billcode and  a.flowid='" + flowid + "' and a.billcode in(");

            }

            foreach (string billcode in list)
            {
                sb.Append("'");
                sb.Append(billcode);
                sb.Append("',");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") ");
            if (strisdz == "1")
            {
                sb.Append("  group by billname,flowid,stepid,billuser,billdate,isgk,billdept");
            }
            return sb.ToString();
        }
        else
        {
            return null;
        }

    }
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            HiddenField hf = e.Item.FindControl("hfCode") as HiddenField;
            HiddenField billdate = e.Item.FindControl("hiddatetime") as HiddenField;
            HiddenField hiddeptCode = e.Item.FindControl("hidbillDept") as HiddenField;
            string billDate = billdate.Value;
            string deptCode = hiddeptCode.Value;
            string code = hf.Value.Trim();
            string strdydj = "02";
            if (!string.IsNullOrEmpty(code))
            {
                //select  mxGuid ,fykm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.fykm ) as yskmMc
                        //,isnull(je ,0)  as je from bill_ybbxmxb_fykm f where billCode='" + code + "'
                string strkmsql = @"select  mxGuid ,fykm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.fykm ) as yskmMc
                        ,isnull(je ,0)  as je from bill_ybbxmxb_fykm f where billCode in (select billcode from bill_main where billname='" + code + "')";
                DataTable dt = server.GetDataTable(strkmsql, null);
            

                if (dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {


                        //预算金额

                        deptCode = string.IsNullOrEmpty(deptCode) ? "" : deptCode.Split(']')[0].Trim('[');
                        string kmCode = string.IsNullOrEmpty(dt.Rows[i]["yskmMc"].ToString()) ? "" : dt.Rows[i]["yskmMc"].ToString().Split(']')[0].Trim('[');
                        string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(billDate));

                        if (!string.IsNullOrEmpty(Request["flowid"]))
                        {
                            if (Request["flowid"].ToString()=="ybbx")
                            {
                                 strdydj = "02";
                            }
                           
                        }
                        decimal ysje = ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);//预算金额

                        decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                        //是否启用销售提成模块
                        bool hasSaleRebate = new ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");
                        decimal syje = ysje - hfje;
                        sb.Append("<div class='div-yskm'>");
                        sb.Append("<table class='tab-yskm'>");
                        sb.Append("<tr><td class=''>" + Convert.ToString(dt.Rows[i]["yskmMc"]) + "预算金额：￥(" + Convert.ToDecimal(ysje).ToString("N02") + ") 剩余金额：￥(" + Convert.ToDecimal(syje).ToString("N02") + ")" + "报销金额：￥(" + Convert.ToDecimal(dt.Rows[i]["je"]).ToString("N02") + ")</td></tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        DataTable temp = server.GetDataTable("select (select '['+deptCode+']'+deptName  from bill_departments where deptCode=a.deptCode) as Dept,isnull(je,0) as je  from bill_ybbxmxb_fykm_dept a where kmmxGuid ='" + Convert.ToString(dt.Rows[i]["mxGuid"]) + "'", null);
                        if (temp.Rows.Count > 0)
                        {
                            sb.Append("<div class='div-hs'>");
                            sb.Append("<h5>核算部门</h5>");
                            sb.Append("<table class='tab-hs ItemTable'>");
                            // sb.Append("<tr><th class='tdOdd'>部门</th><th>核算金额</th></tr>");
                            for (int j = 0; j < temp.Rows.Count; j++)
                            {
                                sb.Append("<tr><td >" + Convert.ToString(temp.Rows[j]["Dept"]) + ":&nbsp;&nbsp;￥" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("</div>");
                        }

                        temp = server.GetDataTable("select distinct  '['+b.xmCode+']'+xmName as xmCode,isnull(je,0) as je from bill_ybbxmxb_hsxm a,bill_xm b where a.xmcode=b.xmcode and kmmxGuid='" + Convert.ToString(dt.Rows[i]["mxGuid"]) + "'", null);
                        if (temp.Rows.Count > 0)
                        {
                            sb.Append("<div class='div-hs'>");
                            sb.Append("<h5>核算项目</h5>");
                            sb.Append("<table class='tab-hs'>");
                            // sb.Append("<tr><th class='tdOdd'>项目</th><th>核算金额</th></tr>");
                            for (int j = 0; j < temp.Rows.Count; j++)
                            {
                                sb.Append("<tr><td >" + Convert.ToString(temp.Rows[j]["xmCode"]) + ":&nbsp;&nbsp;￥" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("</div>");
                        }
                    }
                    Label lb = e.Item.FindControl("lbmx") as Label;
                    lb.Text = sb.ToString();
                }
            }
        }
    }
}
