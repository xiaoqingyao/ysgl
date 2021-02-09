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
using System.Data.SqlClient;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using Bll.UserProperty;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class webBill_YbbxPrint_GkbxPrint : System.Web.UI.Page
{
    ReportDocument ReportDoc = new ReportDocument();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Bind();
    }
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        ReportDoc.Dispose();
    }
    private void Bind()
    {
        string billcode = Request["billName"];
        if (billcode == null)
        {
            return;
        }
        string strsql = "select  top 1 main.billcode,convert(varchar(10),billdate,20) as billdate,substring(convert(varchar(10),billdate,20),1,4) as billdate1,substring(convert(varchar(10),billdate,20),6,2) as billdate2,substring(convert(varchar(10),billdate,20),9,2) as billdate3,billname,pt=(select deptname from bill_departments where bill_departments.deptcode=(select userDept from bill_users where usercode=(select top 1 bxr from bill_ybbxmxb where billcode=main.billcode))),billuser=(select username from bill_users where bill_users.usercode=main.billuser),billtype=(select dicname from bill_dataDic where dictype='02' and bill_dataDic.diccode=(select bxmxlx from bill_ybbxmxb where bill_ybbxmxb.billcode=main.billcode )),bxsm=(select bxsm from bill_ybbxmxb where bill_ybbxmxb.billcode =(select top 1 billcode from bill_main where billname=main.billname)),bxzy=(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billcode =(select top 1 billcode from bill_main where billname=main.billname)) from bill_main main where billname='" + billcode + "' and flowid='gkbx'";
        DataTable tbHead = server.GetDataTable(strsql, null);
        string strbxzy = tbHead.Rows[0]["bxzy"].ToString();
        if (strbxzy.Length <= 25)
        {
            tbHead.Rows[0]["bxzy"] = "\r\n" + strbxzy;
        }

        string strhjsql = "select dbo.CapitalRMB(sum(isnull(je,0))+sum(isnull(se,0))) as hj,(sum(isnull(je,0))+sum(isnull(se,0))) as hjxiaoxie from  bill_ybbxmxb_fykm where billcode in (select billcode from bill_main where  billname='" + billcode + "') ";
        DataTable dthj = server.GetDataTable(strhjsql, new SqlParameter[] { new SqlParameter("@billname", billcode) });

        string strpro = "bill_pro_fykm_gkfybxd";
        DataTable dtmx = server.ExecuteProcedure(strpro, new SqlParameter[] { new SqlParameter("@billname", billcode) }).Tables[0];
        //判断有几行 如果不够6行  就凑够
        int dtmxrows = dtmx.Rows.Count;
        int addrow =8 - (dtmxrows % 8);
        for (int i = 0; i < addrow; i++)
        {
            DataRow dr = dtmx.NewRow();
            dtmx.Rows.Add(dr);
        }
        ReportDoc.Load(Server.MapPath("gkbxreport.rpt"));
        ReportDoc.Database.Tables[0].SetDataSource(tbHead);
        ReportDoc.Database.Tables[1].SetDataSource(dtmx);
        ReportDoc.Database.Tables[2].SetDataSource(dthj);

        string state = GetState(billcode);
        ReportDoc.SetParameterValue("wf", state);

        //获取报销人 
        string strbxrordept = server.GetCellValue("select top 1 bxr2 from bill_ybbxmxb where billcode in (select top 1 billcode from bill_main where billname = '" + billcode + "' )");
        if (strbxrordept.Length < 6)//如果长度没有6 就换行 为了显示好看
        {
            strbxrordept = "\r\n" + strbxrordept;
        }
        ReportDoc.SetParameterValue("bxr2", strbxrordept);

        decimal dehjxiaoxie = 0;
        decimal.TryParse(dthj.Rows[0]["hjxiaoxie"].ToString(), out dehjxiaoxie);
        ReportDoc.SetParameterValue("xiaoxie", dehjxiaoxie.ToString("N"));
        CrystalReportViewer1.ReportSource = ReportDoc;
        CrystalReportViewer1.DataBind();
        if (IsPostBack)
        {
            // 设置打印页边距 
            PageMargins margins;
            margins = ReportDoc.PrintOptions.PageMargins;
            margins.bottomMargin = 10;
            margins.leftMargin = 5220;
            margins.rightMargin = 0;
            margins.topMargin = 2580;
            ReportDoc.PrintOptions.ApplyPageMargins(margins);
            ReportDoc.PrintOptions.PaperSize = PaperSize.PaperEnvelope12;
            CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf;
            ReportDoc.PrintToPrinter(1, true, 1, 1);
        }
    }
    private string GetState(string billcode)
    {
        StringBuilder ret = new StringBuilder();
        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        WorkFlowRecord recode = bll.GetWFRecordByBill(billcode);
        if (recode.RecordList.Count < 1)
        {
            ret.Append("未提交");
        }
        else
        {
            int preStep = 1;
            int preState = 0;

            foreach (WorkFlowRecords records in recode.RecordList)
            {
                if (records.StepId != preStep)
                {
                    //状态(0,等待;1,正在执行;2,通过;3,废弃)
                    if (preState == 0)
                    {
                        ret.Append(",等待");
                    }
                    else if (preState == 1)
                    {
                        ret.Append(",正在执行");
                    }
                    else if (preState == 2)
                    {
                        ret.Append(",通过");
                    }
                    else if (preState == 3)
                    {
                        ret.Append(",否决");
                    }

                    ret.Append("-->");
                }
                UserMessage umgr = new UserMessage(records.CheckUser);
                ret.Append("[" + umgr.Users.UserCode + "]" + umgr.Users.UserName);
                preStep = records.StepId;
                preState = records.RdState;
            }

            if (preState == 0)
            {
                ret.Append(",等待");
            }
            else if (preState == 1)
            {
                ret.Append(",正在执行");
            }
            else if (preState == 2)
            {
                ret.Append(",通过");
            }
            else if (preState == 3)
            {
                ret.Append(",否决");
            }

            ret.Append("-->");

            ret.Append("结束");
        }
        return ret.ToString();
    }

}
