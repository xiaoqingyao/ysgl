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

public partial class webBill_printer_printerqtbx : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();

        DataSet result = server.GetDataSet("select (select yskmbm+'、'+yskmmc from bill_yskm where yskmcode=fykm) as fykm,je,ms from bill_qtbxmxb_fykm where billCode='" + billCode + "'");

        DataSet temp = server.GetDataSet("select (select username from bill_users where usercode=b.bxr) as bxr,(select username from bill_users where usercode=a.billuser) as billuser,a.billdept as dept,convert(varchar(10),a.billdate,121) as billdate,(select dicname from bill_datadic where dictype='02' and diccode=b.bxmxlx) as bxmxlx,b.bxzy,b.bxsm from bill_main a,bill_qtbxmxb b where a.billCode=b.billCode and a.billCode='" + billCode + "'");

        for (int i = 0; i <= result.Tables[0].Rows.Count - 1; i++)
        {
            result.Tables[0].Rows[i]["ms"] = result.Tables[0].Rows[i]["ms"].ToString().Trim().Replace("<font color=red>", "").Replace("</font>", "");
        }

        CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryqtbx.rpt"));
        CrystalReportSource1.ReportDocument.SetDataSource(result.Tables[0]);
        CrystalReportSource1.ReportDocument.SetParameterValue("jbr", temp.Tables[0].Rows[0]["bxr"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxr", temp.Tables[0].Rows[0]["billUser"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bm", (new billCoding()).getDeptLevel2Name(temp.Tables[0].Rows[0]["dept"].ToString().Trim()));
        CrystalReportSource1.ReportDocument.SetParameterValue("sqrq", temp.Tables[0].Rows[0]["billDate"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxmxlx", temp.Tables[0].Rows[0]["bxmxlx"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxzy", temp.Tables[0].Rows[0]["bxzy"].ToString().Trim());
        CrystalReportSource1.ReportDocument.SetParameterValue("bxsm", temp.Tables[0].Rows[0]["bxsm"].ToString().Trim());
        CrystalReportSource1.DataBind();

        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();
    }
}
