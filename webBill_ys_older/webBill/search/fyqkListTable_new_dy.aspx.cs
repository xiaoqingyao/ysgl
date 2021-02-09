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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class webBill_search_fyqkListTable_new_dy : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }
    private void PageInit(object sender, EventArgs e) { bind(); }
    private void bind()
    {
        TableLogOnInfo loginfo = new TableLogOnInfo();
        loginfo.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
        loginfo.ConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
        loginfo.ConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];
        loginfo.ConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];
        ReportDocument ReportDoc = new ReportDocument();
        ReportDoc.Load(Server.MapPath("fyqkListTable_new_dy.rpt"));
        for (int i = 0; i < ReportDoc.Database.Tables.Count; i++)
        {
            ReportDoc.Database.Tables[i].ApplyLogOnInfo(loginfo);
        }

        CrystalReportViewer1.ParameterFieldInfo.Clear();
        ParameterField parameterFieldkssj = new ParameterField();
        ParameterDiscreteValue discreteValkssj = new ParameterDiscreteValue();
        ParameterFields paramFields = new ParameterFields();
        parameterFieldkssj.ParameterFieldName = "@kssj";
        discreteValkssj.Value = Request["kssj"];
        parameterFieldkssj.CurrentValues.Add(discreteValkssj);
        paramFields.Add(parameterFieldkssj);

        ParameterField paramFieldjzsj = new ParameterField();
        ParameterDiscreteValue discreteValjzsj = new ParameterDiscreteValue();
        paramFieldjzsj.ParameterFieldName = "@jzsj";
        discreteValjzsj.Value = Request["jzsj"];
        paramFieldjzsj.CurrentValues.Add(discreteValjzsj);
        paramFields.Add(paramFieldjzsj);

        ParameterField paramFielddeptcode = new ParameterField();
        ParameterDiscreteValue discreteValdeptcode = new ParameterDiscreteValue();
        paramFielddeptcode.ParameterFieldName = "@deptcode";
        discreteValdeptcode.Value = Request["deptcode"];
        paramFielddeptcode.CurrentValues.Add(discreteValdeptcode);
        paramFields.Add(paramFielddeptcode);

        ParameterField paramFieldishz = new ParameterField();
        ParameterDiscreteValue discreteValishz = new ParameterDiscreteValue();
        paramFieldishz.ParameterFieldName = "@djzt";
        discreteValishz.Value = Request["type"];
        paramFieldishz.CurrentValues.Add(discreteValishz);
        paramFields.Add(paramFieldishz);

        CrystalReportViewer1.ReportSource = ReportDoc;
        CrystalReportViewer1.ParameterFieldInfo = paramFields;
        CrystalReportViewer1.DataBind();

    }
}
