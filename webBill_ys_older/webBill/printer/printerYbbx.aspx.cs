using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_printer_printerYbbx : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();

        DataSet result = server.GetDataSet("select (select yskmbm+'、'+yskmmc from bill_yskm where yskmcode=fykm) as fykm,je,ms from bill_ybbxmxb_fykm where billCode='" + billCode + "'");

        DataSet temp = server.GetDataSet("select (select username from bill_users where usercode=b.bxr) as bxr,(select username from bill_users where usercode=a.billuser) as billuser,a.billdept as dept,convert(varchar(10),a.billdate,121) as billdate,(select dicname from bill_datadic where dictype='02' and diccode=b.bxmxlx) as bxmxlx,b.bxzy,b.bxsm from bill_main a,bill_ybbxmxb b where a.billCode=b.billCode and a.billCode='" + billCode + "'");

        for (int i = 0; i <= result.Tables[0].Rows.Count - 1; i++)
        {
            result.Tables[0].Rows[i]["ms"] = result.Tables[0].Rows[i]["ms"].ToString().Trim().Replace("<font color=red>", "").Replace("</font>", "");
        }

        DataSet tempRecord = server.GetDataSet("select (select username from bill_users where usercode=checkuser) as checkuser,convert(varchar(10),checkdate,121) as checkdate,checkbz from bill_workflowrecord where billCode='" + billCode + "' and flowid='ybbx' and looptimes=(select max(looptimes) from bill_workflowrecord where billCode='" + billCode + "' and flowid='ybbx') order by checkdate");

        string shgc1 = "";
        string shgc2 = "";
        string shgc3 = "";
        string shgc4 = "";
        string shgc5 = "";
        string shgc6 = "";
        string shgc7 = "";
        string shgc8 = "";
        string shgc9 = "";
        for (int i = 0; i <= tempRecord.Tables[0].Rows.Count - 1; i++)
        {
            if (i == 0) shgc1 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 1) shgc2 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 2) shgc3 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 3) shgc4 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 4) shgc5 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 5) shgc6 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 6) shgc7 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 7) shgc8 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();
            if (i == 8) shgc9 = "审核人：" + tempRecord.Tables[0].Rows[i]["checkuser"].ToString().Trim() + " 审核日期：" + tempRecord.Tables[0].Rows[i]["checkdate"].ToString().Trim() + " 审核意见：" + tempRecord.Tables[0].Rows[i]["checkbz"].ToString().Trim();

        }
            CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryYbbx.rpt"));
        CrystalReportSource1.ReportDocument.SetDataSource(result.Tables[0]);
        CrystalReportSource1.ReportDocument.SetParameterValue("jbr", temp.Tables[0].Rows[0]["bxr"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxr", temp.Tables[0].Rows[0]["billUser"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bm", (new billCoding()).getDeptLevel2Name(temp.Tables[0].Rows[0]["dept"].ToString().Trim()));
        CrystalReportSource1.ReportDocument.SetParameterValue("sqrq", temp.Tables[0].Rows[0]["billDate"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxmxlx", temp.Tables[0].Rows[0]["bxmxlx"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxzy", temp.Tables[0].Rows[0]["bxzy"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxsm", temp.Tables[0].Rows[0]["bxsm"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc1", shgc1);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc2", shgc2);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc3", shgc3);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc4", shgc4);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc5", shgc5);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc6", shgc6);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc7", shgc7);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc8", shgc8);
        CrystalReportSource1.ReportDocument.SetParameterValue("shgc9", shgc9);
        CrystalReportSource1.DataBind();

        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();
    }
}
