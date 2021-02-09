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

public partial class webBill_tjbb_dept_km_ystj_dy : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        bind();
    }
    private void Page_Init(object sender, EventArgs e)
    {
        bind();
    }
    private void bind()
    {

        TableLogOnInfo loginfo = new TableLogOnInfo();
        loginfo.ConnectionInfo.ServerName = ConfigurationManager.AppSettings["ServerName"];
        loginfo.ConnectionInfo.UserID = ConfigurationManager.AppSettings["UserID"];
        loginfo.ConnectionInfo.Password = ConfigurationManager.AppSettings["Password"];
        loginfo.ConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["DatabaseName"];


        //loginfo.ConnectionInfo.ServerName = ".";
        //loginfo.ConnectionInfo.UserID = "sa";
        //loginfo.ConnectionInfo.Password = "123";
        //loginfo.ConnectionInfo.DatabaseName = "yssoft_slyy";


        ReportDocument ReportDoc = new ReportDocument();
        ReportDoc.Load(Server.MapPath("dept_km_ystj_dy.rpt"));
        for (int i = 0; i < ReportDoc.Database.Tables.Count; i++)
        {
            ReportDoc.Database.Tables[i].ApplyLogOnInfo(loginfo);
        }

        CrystalReportViewer1.ParameterFieldInfo.Clear();
        ParameterField paramFieldkssj = new ParameterField();
        ParameterDiscreteValue discreteValkssj = new ParameterDiscreteValue();
        ParameterFields paramFields = new ParameterFields();
        //ParameterRangeValue rangeVal = new ParameterRangeValue(); 参数范围

        string strnd=Request["nd"].ToString();
        paramFieldkssj.ParameterFieldName = "@kssj";
        discreteValkssj.Value = strnd+"-01-01";
        paramFieldkssj.CurrentValues.Add(discreteValkssj);
        paramFields.Add(paramFieldkssj);

        ParameterField paramFieldjzsj = new ParameterField();
        ParameterDiscreteValue discreteValjzsj = new ParameterDiscreteValue();
        paramFieldjzsj.ParameterFieldName = "@jzsj";
        discreteValjzsj.Value = strnd+"-12-31 23:59:59";
        paramFieldjzsj.CurrentValues.Add(discreteValjzsj);
        paramFields.Add(paramFieldjzsj);

        ParameterField paramFielddeptcode = new ParameterField();
        ParameterDiscreteValue discreteValdeptcode = new ParameterDiscreteValue();
        paramFielddeptcode.ParameterFieldName = "@deptcode";
        discreteValdeptcode.Value = "";
        paramFielddeptcode.CurrentValues.Add(discreteValdeptcode);
        paramFields.Add(paramFielddeptcode);

        ParameterField paramFieldfykmcode = new ParameterField();
        ParameterDiscreteValue discreteValfykmcode = new ParameterDiscreteValue();
        paramFieldfykmcode.ParameterFieldName = "@fykmcode";
        discreteValfykmcode.Value = "";
        paramFieldfykmcode.CurrentValues.Add(discreteValfykmcode);
        paramFields.Add(paramFieldfykmcode);

        //标题
        string strTitle = "";
        string strCompany = ConfigurationManager.AppSettings["CustomTitle"];
        strTitle = string.Format("{0}{1}年度", strCompany, strnd);
        //ReportDoc.SetParameterValue("Title", strTitle);

        ParameterField paramFieldTitle = new ParameterField();
        ParameterDiscreteValue discreteValTitle = new ParameterDiscreteValue();
        paramFieldTitle.ParameterFieldName = "Title";
        discreteValTitle.Value = strTitle;
        paramFieldTitle.CurrentValues.Add(discreteValTitle);
        paramFields.Add(paramFieldTitle);

        CrystalReportViewer1.ReportSource = ReportDoc;//这句一定要在设置ParameterFieldInfo前面
        CrystalReportViewer1.ParameterFieldInfo = paramFields;
        CrystalReportViewer1.DataBind();
    }
}
