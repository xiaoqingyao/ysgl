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
using System.Data.SqlClient;
using Bll;
using Bll.UserProperty;

public partial class bxd_ybbxView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        ltrTitle.Text = GetTitleByFlowid(Request["flowid"]);
        string code = Convert.ToString(Request["billCode"]);
        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
        }
        if (!string.IsNullOrEmpty(code))
        {
            DataTable dt = new DataTable();
            if (strisdz == "1")
            {
                dt = server.GetDataTable("select a.billCode,a.billName,convert(varchar(10),a.billDate,121) as billDate,isnull((select '['+userCode+']'+userName from bill_users where usercode=a.billUser),a.billUser) as billuser,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.billDept),a.billDept)as billDept,(case a.isgk when '1' then '是'  else '否' end ) as isgk, isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.gkdept),a.gkdept)as gkdept,isnull((select dicName from bill_dataDic where dicType='02' and dicCode=b.bxmxlx),b.bxmxlx) as bxmxlx,b.bxzy,b.bxsm,a.billje from bill_main  a,bill_ybbxmxb b where a.billCode=b.billCode and a.billname=@code", new SqlParameter[] { new SqlParameter("@code", code) });

            }
            else
            {
                dt = server.GetDataTable("select a.billCode,a.billName,convert(varchar(10),a.billDate,121) as billDate,isnull((select '['+userCode+']'+userName from bill_users where usercode=a.billUser),a.billUser) as billuser,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.billDept),a.billDept)as billDept,(case a.isgk when '1' then '是'  else '否' end ) as isgk, isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.gkdept),a.gkdept)as gkdept,isnull((select dicName from bill_dataDic where dicType='02' and dicCode=b.bxmxlx),b.bxmxlx) as bxmxlx,b.bxzy,b.bxsm,a.billje from bill_main  a,bill_ybbxmxb b where a.billCode=b.billCode and a.billCode=@code", new SqlParameter[] { new SqlParameter("@code", code) });

            }
            if (dt.Rows.Count > 0)
            {
                lbBillCode.Text = Convert.ToString(dt.Rows[0]["billName"]);
                lbBillData.Text = Convert.ToString(dt.Rows[0]["billDate"]);
                lbBillUser.Text = Convert.ToString(dt.Rows[0]["billUser"]);
                lbBillDept.Text = Convert.ToString(dt.Rows[0]["billDept"]);
                lbIsgk.Text = Convert.ToString(dt.Rows[0]["isgk"]);
                if (lbIsgk.Text == "否")
                {
                    tdgk.Visible = false;
                }
                lbGkdept.Text = Convert.ToString(dt.Rows[0]["gkdept"]);
                lbBillType.Text = Convert.ToString(dt.Rows[0]["bxmxlx"]);
                lbBxzy.Text = Convert.ToString(dt.Rows[0]["bxzy"]);
                lbBxsm.Text = Convert.ToString(dt.Rows[0]["bxsm"]);
                lbBillje.Text = Convert.ToDecimal(dt.Rows[0]["billje"]).ToString("N02");
                lbMx.Text = GetYskmStr(code);
            }
            //显示附件
            string fujian = "";
            if (strisdz == "1")
            {
                string strfjsql = @"select fujian from bill_ybbxmxb where billCode in (select billcode from bill_main where billname='" + code + "')";
                fujian = server.GetCellValue(strfjsql);

            }
            else
            {
                fujian = server.GetCellValue("select fujian from bill_ybbxmxb where billCode ='" + Request["billCode"] + "'");

            }
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                for (int i = 0; i < arrname.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(arrname[i]))
                    {
                        {
                            this.lalFuJian.Text += "<a href='../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a>";
                        }
                    }
                }
            }
            else
            {
                this.lalFuJian.Text = "无";
            }
        }

        string type = Request["type"];
        if (!string.IsNullOrEmpty(type))
        {
            if (type == "View")
            {
                aduittr.Visible = false;
                btn_audit.Visible = false;
                btn_cancel.Visible = false;
                //判断是否已提交
                DataTable dt = server.GetDataTable("select * from workflowrecord where billCode='" + code + "'", null);
                if (dt.Rows.Count > 0)
                {
                    btn_submit.Visible = false;
                    btn_delete.Visible = false;
                    if (dt.Rows[0]["rdState"].ToString() == "3")
                    {
                        btn_revoke.Visible = true;
                    }
                }


            }
            if (type == "audit")
            {
                btn_submit.Visible = false;
                btn_delete.Visible = false;
                aduittr.Visible = true;
            }

        }

    }
    private string GetTitleByFlowid(string flowid)
    {
        string text = server.GetCellValue("select flowName from mainworkflow where flowId='" + flowid + "'");
        if (!string.IsNullOrEmpty(text) && text.IndexOf("审批") != -1)
        {
            text = text.Replace("审批", "");
        }
        if (!string.IsNullOrEmpty(text) && text.IndexOf("详细") == -1)
        {
            text += "详细";
        }
        return text;
    }
    private string GetYskmStr(string code)
    {
        string strisdz = "";
        YsManager ysmgr = new YsManager();
        string strdydj = "02";
        string result = "";
        string strsql = "";

        if (!string.IsNullOrEmpty(code))
        {
            if (!string.IsNullOrEmpty(Request["isdz"]))
            {
                strisdz = Request["isdz"].ToString();
            }
            if (strisdz == "1")
            {
                //根据传参billname 获取billcode
                // strbillcode = server.GetCellValue("select billcode from bill_main where billname='" + code + "'");
                strsql = @"select  main.gkdept,mxGuid ,fykm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.fykm ) as yskmMc,isnull(je ,0)  as je,f.ms from bill_ybbxmxb_fykm f,bill_main main where main.billcode=f.billcode and  main.billCode in(select billcode from bill_main where billname='" + code + "')";
            }
            else
            {
                strsql = @"select  main.gkdept,mxGuid ,fykm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.fykm ) as yskmMc,isnull(je ,0)  as je,f.ms from bill_ybbxmxb_fykm f,bill_main main where main.billcode=f.billcode and billCode='" + code + "'";
            }
            DataTable dt = server.GetDataTable(strsql, null);
            if (dt.Rows.Count > 0)
            {
                string billDate = lbBillData.Text;
                string deptCode = lbBillDept.Text;
                //预算金额

                deptCode = string.IsNullOrEmpty(deptCode) ? "" : deptCode.Split(']')[0].Trim('[');


                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string kmCode = string.IsNullOrEmpty(dt.Rows[i]["yskmMc"].ToString()) ? "" : dt.Rows[i]["yskmMc"].ToString().Split(']')[0].Trim('[');
                    string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(billDate));

                    if (!string.IsNullOrEmpty(Request["flowid"]))
                    {
                        if (Request["flowid"].ToString() == "ybbx")
                        {
                            strdydj = "02";
                        }

                    }
                    deptCode = dt.Rows[i]["gkdept"].ToString(); ;
                    decimal ysje = ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);//预算金额

                    decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                    //是否启用销售提成模块
                    bool hasSaleRebate = new ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");
                    decimal syje_dongtai = ysje - hfje;
                    decimal syje = 0;
                    if (!decimal.TryParse(dt.Rows[i]["ms"].ToString(), out syje) || syje == 0)//大智是用的单据剩余金额来的  不是每次求最新的剩余预算
                    {
                        syje = syje_dongtai;
                    }


                    if (syje < 0)
                    {
                        syje = 0;
                    }

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
                        sb.Append("<table class='tab-hs ItemTable'  style='color:black;font-size:14px;'>");
                        // sb.Append("<tr><th class='tdOdd'>部门</th><th>核算金额</th></tr>");
                        for (int j = 0; j < temp.Rows.Count; j++)
                        {
                            sb.Append("<tr><td >" + Convert.ToString(temp.Rows[j]["Dept"]) + ":&nbsp;&nbsp;￥" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "</td></tr>");
                        }
                        sb.Append("</table>");
                        sb.Append("</div>");
                    }

                    string strdeptsql = @"select distinct  '['+b.xmCode+']'+xmName as xmCode,isnull(je,0) as je from bill_ybbxmxb_hsxm a,bill_xm b where a.xmcode=b.xmcode and kmmxGuid='" + Convert.ToString(dt.Rows[i]["mxGuid"]) + "'";
                    temp = server.GetDataTable(strdeptsql, null);
                    if (temp.Rows.Count > 0)
                    {
                        sb.Append("<div class='div-hs'>");
                        sb.Append("<h5>核算项目</h5>");
                        sb.Append("<table class='tab-hs' style='color:black;font-size:14px;'>");
                        // sb.Append("<tr><th class='tdOdd'>项目</th><th>核算金额</th></tr>");
                        for (int j = 0; j < temp.Rows.Count; j++)
                        {
                            sb.Append("<tr><td >" + Convert.ToString(temp.Rows[j]["xmCode"]) + ":&nbsp;&nbsp;￥" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "</td></tr>");
                        }
                        sb.Append("</table>");
                        sb.Append("</div>");
                    }
                    result = sb.ToString();
                }
            }
        }
        return result;
    }
}
