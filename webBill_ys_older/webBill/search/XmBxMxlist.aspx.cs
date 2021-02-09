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

public partial class webBill_search_XmBxMxlist : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strbegtime;
    string strendtime;
    string strdeptcode;
    string strxmcode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["begtime"]!=null&&Request["begtime"].ToString()!="")
            {
                strbegtime = Request["begtime"].ToString();
            }
            if (Request["endtime"]!=null&&Request["endtime"].ToString()!="")
            {
                strendtime = Request["endtime"].ToString();
            }
            if (Request["deptcode"]!=null&&Request["deptcode"].ToString()!="")
            {
                strdeptcode = Request["deptcode"].ToString();
            }
            if (Request["xmcode"]!=null&&Request["xmcode"].ToString()!="")
            {
                strxmcode = Request["xmcode"].ToString();
            }
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid() 
    {
        string strdeptcodename = "";
        string strxmcodename = "";
        string strsql = @"select a.*,bill_main.gkdept,bill_main.billDate,bill_main.billName,(select top 1 '['+xmCode+']'+xmName from bill_xm where xmCode=a.xmCode )as xmname,(case flowid when 'ybbx' then '一般报销' when 'qtbx' then '其它报销' else '未知' end) as flowidname,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_main.gkdept)as deptcodename
,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=a.fykm ) as fykmName
 from (select bill_ybbxmxb_hsxm.xmCode,bill_ybbxmxb_hsxm.je,bill_ybbxmxb_fykm.billCode,bill_ybbxmxb_fykm.fykm
from bill_ybbxmxb_hsxm,bill_ybbxmxb_fykm where bill_ybbxmxb_hsxm.kmmxGuid=bill_ybbxmxb_fykm.mxGuid) a
 left join bill_main on a.billCode=bill_main.billCode  where  bill_main.stepID='end' ";
        if (strbegtime!="")
        {
            strsql += " and  convert(varchar(10),billdate,20)>='"+strbegtime+"'";
            this.lbmasge.Text += "开始时间："+strbegtime;
        }
        if (strendtime!="")
        {
            strsql += " and convert(varchar(10),billdate,20)<='"+strendtime+"'";
            this.lbmasge.Text += "; 结束时间："+strendtime;
        }
        if (strdeptcode!="")
        {
            strsql += " and gkdept in (select deptcode from bill_departments where deptcode ='"+strdeptcode+"')";
            strdeptcodename = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode='"+strdeptcode+"'");
            this.lbmasge.Text += "; 部门："+strdeptcodename;

        }
        if (strxmcode!="")
        {
            strsql += " and xmCode='"+strxmcode+"'";
            strxmcodename = server.GetCellValue("select top 1 '['+xmCode+']'+xmName from bill_xm where xmCode='"+strxmcode+"'");
            this.lbmasge.Text += "; 项目："+strxmcodename;

        }
        DataSet temp = server.GetDataSet(strsql);
        this.GridView1.DataSource = temp;
        this.GridView1.DataBind();
    }
    double dall = 0;
    protected void GridView1_RowDataBound(object sender,GridViewRowEventArgs e) {
        if (e.Row.RowType==DataControlRowType.DataRow)
        {
            string strBillCode = e.Row.Cells[3].Text.Trim();
            string strBillName = e.Row.Cells[2].Text.Trim();
            string strUrl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strBillCode;
            e.Row.Cells[2].Text = "<a href=# onclick=\"openDetail('" + strUrl + "')\">" + strBillName + "</a>";
            double deve = double.Parse(e.Row.Cells[1].Text.Trim());
            dall = dall + deve;
        }else if(e.Row.RowType==DataControlRowType.Footer){
            e.Row.Cells[1].Text = dall.ToString("N");
            e.Row.Cells[0].Text = "合计：";
        }
    }
}
