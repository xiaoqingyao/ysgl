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
using System.IO;
using Models;
using Bll.UserProperty;

public partial class webBill_newTj_ysxmResult_mx : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
                if (dept == "")
                { dept = "统计单位：所有单位"; }
                else
                {
                    SysManager mgr = new SysManager();
                    dept = "统计单位：" + mgr.GetDeptByCode(dept).DeptName.ToString().Trim();
                }
                this.Label1.Text = "开始时间：" + Page.Request.QueryString["kssj"].ToString().Trim() + "  截止时间：" + Page.Request.QueryString["jzsj"].ToString().Trim() + "  " + dept; 
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        DateTime begDate = Convert.ToDateTime(Page.Request.QueryString["kssj"].ToString().Trim());
        DateTime endDate = Convert.ToDateTime(DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString());
        string deptcode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string xmcode = Page.Request.QueryString["xmCode"].ToString().Trim();
        string cxlx = Page.Request.QueryString["cxlx"].ToString().Trim();

        QueryManger qm = new QueryManger();
        DataTable dt = qm.SearchSyxmReportMx(begDate, endDate, deptcode, xmcode, cxlx);
        //string sql = "select (select '['+xmcode+']'+xmname from bill_xm where bill_xm.xmcode=bill_ybbxmxb_hsxm.xmcode) as xmbh,je as bxje,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=(select fykm from bill_ybbxmxb_fykm where bill_ybbxmxb_fykm.mxGuid=bill_ybbxmxb_hsxm.kmmxGuid)) as kmmc from bill_ybbxmxb_hsxm";
        //sql += " where xmCode='" + Page.Request.QueryString["xmCode"].ToString().Trim() + "' ";
        //if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
        //{
        //    sql += " and kmmxGuid in (select mxguid from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billdept='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "' and stepid='end' and flowid in ('ybbx','qtbx')))";
        //}
        //else
        //{
        //    sql += " and kmmxGuid in (select mxguid from bill_ybbxmxb_fykm where billCode in (select billcode from bill_main where billdate>=cast('" + Page.Request.QueryString["kssj"].ToString().Trim() + "' as datetime) and billdate<=cast('" + Page.Request.QueryString["jzsj"].ToString().Trim() + "' as datetime) and stepid='end' and flowid in ('ybbx','qtbx')))";
        //}
        //DataSet temp = server.GetDataSet(sql);
        this.myGrid.DataSource = dt;
        this.myGrid.DataBind();
    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Write("<script> history.go(-2) </script>");
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
    }
}
