using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using Bll.UserProperty;
using System.Data.SqlClient;

public partial class webBill_YbbxPrint_YbbxPrint : System.Web.UI.Page
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
        string billcode = Request.Params["billCode"];


        string sql = "select billcode,billdate,billname, pt=(select deptname from bill_departments where deptcode= (select sjdeptcode from bill_departments where deptcode=(select userDept from bill_users where bill_users.usercode=bill_main.billuser)))+'-->'+(select deptname from bill_departments where bill_departments.deptcode=(select userDept from bill_users where bill_users.usercode=bill_main.billuser)),deptto=(select deptname from bill_departments where deptcode= (select sjdeptcode from bill_departments where deptcode=(select userDept from bill_users where bill_users.usercode=bill_main.billuser))) , dept=(select deptname from bill_departments where bill_departments.deptcode=(select userDept from bill_users where bill_users.usercode=bill_main.billuser)),";
        sql += "billuser=(select username from bill_users where bill_users.usercode=bill_main.billuser), billtype=(select dicname from bill_dataDic where dictype='02' and bill_dataDic.diccode=(select bxmxlx from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode )), ";
        sql += "gkdept=(select deptname from bill_departments where bill_departments.deptcode=bill_main.gkdept),bxzy=(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode),";
        sql += "bxsm=(select bxsm from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode) from bill_main  WHERE billcode='" + billcode + "'";

        string sqll = "bill_pro_fykm_fybxd";

        SqlParameter billcodes = new SqlParameter("@billcode", SqlDbType.VarChar);
        billcodes.Value = billcode;
        SqlParameter[] sps = { billcodes };
        DataTable dt4 = server.ExecuteProcedure(sqll, sps).Tables[0];


        string sql2 = "select dbo.CapitalRMB(sum(je)+sum(se)) as hj from  bill_ybbxmxb_fykm where billcode='" + billcode + "'";
        DataTable dt5 = server.GetDataSet(sql2).Tables[0];


        DataTable dt = server.GetDataSet(sql).Tables[0];
        //CrystalReport1 cry = new CrystalReport1();
        ReportDocument ReportDoc = new ReportDocument();
        ReportDoc.Load(Server.MapPath("CrystalReport1.rpt"));
        ReportDoc.Database.Tables[0].SetDataSource(dt);
        ReportDoc.Database.Tables[1].SetDataSource(dt4);
        ReportDoc.Database.Tables[2].SetDataSource(dt5);

        string state = GetState(billcode);
        ReportDoc.SetParameterValue("wf", state);
        CrystalReportViewer1.ReportSource = ReportDoc;
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
    /*
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        Bind();
        //}
    }
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        
    }


    private void Bind()
    {
        string billCode = Request.Params["billCode"];
        string sql = "select billcode,billdate,billname,dept=(select deptname from bill_departments where bill_departments.deptcode=(select userDept from bill_users where bill_users.usercode=bill_main.billuser)),";
        sql += "billuser=(select username from bill_users where bill_users.usercode=bill_main.billuser), billtype=(select dicname from bill_dataDic where dictype='02' and bill_dataDic.diccode=(select bxmxlx from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode )), ";
        sql += "gkdept=(select deptname from bill_departments where bill_departments.deptcode=bill_main.gkdept),bxzy=(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode),";
        sql += "bxsm=(select bxsm from bill_ybbxmxb where bill_ybbxmxb.billcode=bill_main.billcode) from bill_main  WHERE billcode='" + billCode + "'";



        string sqlFykm = @"select  billCode, 
                        (select '['+yskmcode+']'+yskmmc from bill_yskm where bill_yskm.yskmcode=v_ybbx_fykm_dy.fykm)as fykm, 
                        je, mxGuid, status, ms, se,
                        qw, bw, sw, w, q, b, s, g, l1, l2
                        from v_ybbx_fykm_dy 
                        WHERE billcode='"+billCode+"'";

        string sqldept = @"SELECT     kmmxGuid, mxGuid,
                            (select deptname from bill_departments where bill_departments.deptcode= bill_ybbxmxb_fykm_dept.deptCode) as deptcode,
                             je, status
                            FROM         bill_ybbxmxb_fykm_dept
                            where kmmxGuid in(select mxGuid from bill_ybbxmxb_fykm where billcode='"+billCode+"')";
        string sqlXm = @"select  kmmxGuid, mxGuid, 
                            (select xmname from bill_xm where bill_xm.xmcode=bill_ybbxmxb_hsxm.xmcode) as xmCode, 
                            je 
                            from dbo.bill_ybbxmxb_hsxm 
                            where kmmxGuid in(
                            select mxGuid from bill_ybbxmxb_fykm where billcode='"+billCode+"')";

        DataTable dt = server.GetDataSet(sql).Tables[0];

        DataTable dt1 = server.GetDataSet(sqlFykm).Tables[0];
        DataTable dt2 = server.GetDataSet(sqldept).Tables[0];
        DataTable dt3 = server.GetDataSet(sqlXm).Tables[0];

        string state = GetState(billCode);

        ReportDocument reportDoc = new ReportDocument();
        reportDoc.Load(Server.MapPath("CrystalReport1.rpt"));
        reportDoc.Database.Tables[0].SetDataSource(dt);
        reportDoc.Database.Tables[1].SetDataSource(dt1);
        reportDoc.Database.Tables[2].SetDataSource(dt2);
        reportDoc.Database.Tables[3].SetDataSource(dt3);

        reportDoc.SetParameterValue("wf", state);

        CrystalReportViewer1.ReportSource = reportDoc;
    }


     */
}