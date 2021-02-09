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

public partial class webBill_printer_printerLscg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();

        DataSet result = (new sqlHelper.sqlHelper()).GetDataSet("select convert(varchar(10),sj,121) as sqrq,cgbh as sqdh,cgdept as bm,(select dicname from bill_datadic where dictype='03' and diccode=cglb) as sqlb,zynr as sqnr,sm as sqsm,(select username from bill_users where usercode=cbr) as cbr,yjfy from bill_lscg where cgbh='" + billCode + "'");
        for (int i = 0; i <= result.Tables[0].Rows.Count - 1; i++)
        {
            result.Tables[0].Rows[i]["bm"] = (new billCoding()).getDeptLevel2Name(result.Tables[0].Rows[i]["bm"].ToString().Trim());
        }

        CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryLscg.rpt"));
        CrystalReportSource1.ReportDocument.SetDataSource(result.Tables[0]);
        //CrystalReportSource1.ReportDocument.SetParameterValue("bbTitle", bbtile);
        CrystalReportSource1.DataBind();

        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();
    }
}
