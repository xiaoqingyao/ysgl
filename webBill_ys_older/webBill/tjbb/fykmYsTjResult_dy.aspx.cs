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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class webBill_tjbb_fykmYsTjResult_dy : System.Web.UI.Page
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
        string strFile = "fykmYsTjResult_dy.rpt";
        if (Request["ishz"].ToString().Equals("1"))
        {
            strFile = "fykmYsTjResult_dy_hz.rpt";
        }
        ReportDoc.Load(Server.MapPath(strFile));
        for (int i = 0; i < ReportDoc.Database.Tables.Count; i++)
        {
            ReportDoc.Database.Tables[i].ApplyLogOnInfo(loginfo);
        }
        CrystalReportViewer1.ParameterFieldInfo.Clear();
        ParameterFields paramFields = new ParameterFields();
        //开始时间
        ParameterField parameterFieldkssj = new ParameterField();
        ParameterDiscreteValue discreteValkssj = new ParameterDiscreteValue();
        
        parameterFieldkssj.ParameterFieldName = "@kssj";
        discreteValkssj.Value = Request["kssj"];
        parameterFieldkssj.CurrentValues.Add(discreteValkssj);
        paramFields.Add(parameterFieldkssj);
        //截止时间
        ParameterField paramFieldjzsj = new ParameterField();
        ParameterDiscreteValue discreteValjzsj = new ParameterDiscreteValue();
        paramFieldjzsj.ParameterFieldName = "@jzsj";
        discreteValjzsj.Value = Request["jzsj"];
        paramFieldjzsj.CurrentValues.Add(discreteValjzsj);
        paramFields.Add(paramFieldjzsj);
        //部门编号
        ParameterField paramFielddeptcode = new ParameterField();
        ParameterDiscreteValue discreteValdeptcode = new ParameterDiscreteValue();
        paramFielddeptcode.ParameterFieldName = "@deptcode";
        discreteValdeptcode.Value = Request["deptcode"];
        paramFielddeptcode.CurrentValues.Add(discreteValdeptcode);
        paramFields.Add(paramFielddeptcode);

        //是否汇总
        ParameterField paramFieldishz = new ParameterField();
        ParameterDiscreteValue discreteValishz = new ParameterDiscreteValue();
        paramFieldishz.ParameterFieldName = "@ishz";
        discreteValishz.Value = Request["ishz"];
        paramFieldishz.CurrentValues.Add(discreteValishz);
        paramFields.Add(paramFieldishz);

        //标题
        string strTitle = "";
        string strCompany = ConfigurationManager.AppSettings["CustomTitle"];
        string strYear = Request["jzsj"].ToString().Substring(0, 4);
        strTitle = string.Format("{0}{1}年度", strCompany, strYear);
        //ReportDoc.SetParameterValue("Title", strTitle);

        ParameterField paramFieldTitle = new ParameterField();
        ParameterDiscreteValue discreteValTitle = new ParameterDiscreteValue();
        paramFieldTitle.ParameterFieldName = "Title";
        discreteValTitle.Value = strTitle;
        paramFieldTitle.CurrentValues.Add(discreteValTitle);
        paramFields.Add(paramFieldTitle);

        CrystalReportViewer1.ReportSource = ReportDoc;
        CrystalReportViewer1.ParameterFieldInfo = paramFields;
        CrystalReportViewer1.DataBind();

    }
}
