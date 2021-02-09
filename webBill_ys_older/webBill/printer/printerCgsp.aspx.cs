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

public partial class webBill_printer_printerCgsp : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();

        DataSet result = server.GetDataSet("select convert(varchar(10),sj,121) as sprq,cgbh as spdh,cgdept as bm,(select dicname from bill_datadic where dictype='03' and diccode=cglb) as sqlb,sm as cgyy,(select username from bill_users where usercode=cbr) as cbr,'' as xj_dw_1,'' as xj_dw_2,'' as xj_dw_3,'' as xj_qk_1,'' as xj_qk_2,'' as xj_qk_3,gys,khh,zh  from bill_cgsp where cgbh='" + billCode + "'");
        for (int i = 0; i <= result.Tables[0].Rows.Count - 1; i++)
        {
            result.Tables[0].Rows[i]["bm"] = (new billCoding()).getDeptLevel2Name(result.Tables[0].Rows[i]["bm"].ToString().Trim());
        }

        DataSet temp = server.GetDataSet("select * from bill_cgsp_xjb where cgbh='" + billCode + "'");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            result.Tables[0].Rows[0]["xj_dw_" + (i + 1)] = temp.Tables[0].Rows[i]["xjdw"].ToString().Trim();
            result.Tables[0].Rows[0]["xj_qk_" + (i + 1)] = temp.Tables[0].Rows[i]["xjqk"].ToString().Trim();
        }
        string sprq = result.Tables[0].Rows[0]["sprq"].ToString().Trim();
        string spdh = result.Tables[0].Rows[0]["spdh"].ToString().Trim();
        string bm = result.Tables[0].Rows[0]["bm"].ToString().Trim();
        string sqlb = result.Tables[0].Rows[0]["sqlb"].ToString().Trim();
        string cgyy = result.Tables[0].Rows[0]["cgyy"].ToString().Trim();
        string cbr = result.Tables[0].Rows[0]["cbr"].ToString().Trim();
        string xj_dw_1 = result.Tables[0].Rows[0]["xj_dw_1"].ToString().Trim();
        string xj_dw_2 = result.Tables[0].Rows[0]["xj_dw_2"].ToString().Trim();
        string xj_dw_3 = result.Tables[0].Rows[0]["xj_dw_3"].ToString().Trim();
        string xj_qk_1 = result.Tables[0].Rows[0]["xj_qk_1"].ToString().Trim();
        string xj_qk_2 = result.Tables[0].Rows[0]["xj_qk_2"].ToString().Trim();
        string xj_qk_3 = result.Tables[0].Rows[0]["xj_qk_3"].ToString().Trim();
        string gys = result.Tables[0].Rows[0]["gys"].ToString().Trim();
        string khh = result.Tables[0].Rows[0]["khh"].ToString().Trim();
        string zh = result.Tables[0].Rows[0]["zh"].ToString().Trim();

        temp = server.GetDataSet("select mc,gg,sl,dj,zj,bz from bill_cgsp_mxb where cgbh='" + billCode + "'");

        CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryCgsp.rpt"));
        CrystalReportSource1.ReportDocument.SetDataSource(temp.Tables[0]);

        CrystalReportSource1.ReportDocument.SetParameterValue("sprq", sprq);
        CrystalReportSource1.ReportDocument.SetParameterValue("spdh", spdh);
        CrystalReportSource1.ReportDocument.SetParameterValue("bm", bm);
        CrystalReportSource1.ReportDocument.SetParameterValue("sqlb", sqlb);
        CrystalReportSource1.ReportDocument.SetParameterValue("cgyy", cgyy);
        CrystalReportSource1.ReportDocument.SetParameterValue("cbr", cbr);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_dw_1", xj_dw_1);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_dw_2", xj_dw_2);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_dw_3", xj_dw_3);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_qk_1", xj_qk_1);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_qk_2", xj_qk_2);
        CrystalReportSource1.ReportDocument.SetParameterValue("xj_qk_3", xj_qk_3);
        CrystalReportSource1.ReportDocument.SetParameterValue("gys", gys);
        CrystalReportSource1.ReportDocument.SetParameterValue("khh", khh);
        CrystalReportSource1.ReportDocument.SetParameterValue("zh", zh);
        CrystalReportSource1.DataBind();

        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();
    }
}
