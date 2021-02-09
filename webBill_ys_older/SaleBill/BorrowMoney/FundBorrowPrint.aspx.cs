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
using Bll.Bills;
using Models;
using System.Collections.Generic;
using System.Text;
using Dal.SysDictionary;
using Bll;

public partial class SaleBill_BorrowMoney_FundBorrowPrint : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();


    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();

            object objCode = Request["Code"];
            if (objCode != null)
            {
                strBillCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                this.BindModel();
            }
            if (!string.IsNullOrEmpty(Request["from"]) && Request["from"] == "sh")
            {
                lbhdje.Visible = false;
            }
        }
    }



    /// <summary>
    /// 获取model
    /// </summary>
    public void BindModel()
    {
        T_LoanList modeljk = loanbll.GetModel(strBillCode);
        if (modeljk != null)
        {
            lbcompany.Text = server.GetCellValue(" select deptName from bill_departments where deptcode='000001'");
            lbjkcode.Text = modeljk.Listid;
            lbloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.ResponsibleCode + "'");
            lbjklb.Text = server.GetCellValue("select dicname  from bill_datadic where dictype='20' and diccode='" + modeljk.NOTE6 + "' ");
            lbdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            lbjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            lbaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            lbjksy.Text = modeljk.LoanExplain;
            lbjkts.Text = modeljk.NOTE4;
            lbhdje.Text = lbmoney.Text = Convert.ToDecimal(modeljk.LoanMoney).ToString("N02");
            lbBz.Text = modeljk.NOTE5;
            //if (!string.IsNullOrEmpty(modeljk.NOTE7))
            //{
            //    this.tbody.InnerHtml = GetFjdj(modeljk.NOTE7);
            //}
            if (!string.IsNullOrEmpty(modeljk.NOTE7))
            {
                //显示采购审批单相关信息
                string mainsql = "select cgbh,sj,cgdept,cglb,sm,(select '['+usercode+']'+username from bill_users where usercode=cbr) as cbr,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,gys,khh,zh from bill_cgsp where cgbh='" + modeljk.NOTE7 + "'";
                DataTable dtmain = server.GetDataTable(mainsql, null);
                if (dtmain != null && dtmain.Rows.Count > 0)
                {
                    this.trCgsp.Visible = true;
                    this.lblspdh.InnerText = dtmain.Rows[0]["cgbh"].ToString();
                    this.lbldept.InnerText = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + dtmain.Rows[0]["cgdept"].ToString().Trim() + "'");
                    this.lblcbr.InnerText = dtmain.Rows[0]["cbr"].ToString();
                    this.lblgys.InnerText = dtmain.Rows[0]["gys"].ToString();
                    this.lblkhh.InnerText = dtmain.Rows[0]["khh"].ToString();
                    this.lblzh.InnerText = dtmain.Rows[0]["zh"].ToString();
         

                    DataTable dtrel = server.GetDataTable("select * from bill_cgsp_mxb where cgbh='" + modeljk.NOTE7 + "' order by cgIndex", null);
                    StringBuilder sbhtml = new StringBuilder();
                    for (int i = 0; i < dtrel.Rows.Count; i++)
                    {
                        DataRow dr = dtrel.Rows[i];
                        sbhtml.Append("<tr>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["mc"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["gg"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["sl"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["dj"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["zj"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("<td>");
                        sbhtml.Append(dr["bz"]);
                        sbhtml.Append("</td>");
                        sbhtml.Append("</tr>");
                    }
                    this.tbCgmx.InnerHtml = sbhtml.ToString();
                }
            }
        }
    }
    private string GetFjdj(string djs)
    {

        StringBuilder fysqSb = new StringBuilder();
        string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
        string sql2 = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.yjfy as cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";
        string sqlCCSQ = @"select(select dicname from bill_datadic where diccode=b.typecode and dictype='06') 
                as cclb,a.billDate,(select deptName from bill_departments where deptCode=a.billDept)
                 as Dept,(select userName from bill_users where userCode=a.billUser)
                 as  billUser ,a.billJe,'审批通过' as spzt,b.reasion,a.billCode
               from bill_main a,bill_travelApplication b where a.billCode=b.maincode";

        string[] arr = djs.Split(',');
        for (int i = 1; i <= arr.Length; i++)
        {
            if (arr[i - 1].Substring(0, 4) == "ccsq")//出差申请
            {
                DataSet ds = server.GetDataSet(sqlCCSQ + " and a.billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["Dept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billUser"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(new PublicServiceBLL().cutDt(ds.Tables[0].Rows[0]["billDate"].ToString()));
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cclb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billJe"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["reasion"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
            else if (arr[i - 1].Substring(0, 2) == "cg")
            {
                DataSet ds = server.GetDataSet(sql + "and billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>&nbsp;");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(new PublicServiceBLL().cutDt((ds.Tables[0].Rows[0]["sj"].ToString())));
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
            else if (arr[i - 1].Substring(0, 2) == "ls")
            {
                DataSet ds = server.GetDataSet(sql2 + "and billcode='" + arr[i - 1] + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fysqSb.Append("<tr>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("<td>");
                    fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                    fysqSb.Append("</td>");
                    fysqSb.Append("</tr>");
                }
            }
        }

        return fysqSb.ToString();
    }

}
