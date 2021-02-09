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
using System.Text;
using System.Data.SqlClient;
using Bll;
using Bll.UserProperty;

public partial class BillJfsq_JfspView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        ltrTitle.Text = GetTitleByFlowid(Request["flowid"]);
        string code = Convert.ToString(Request["Code"]);
        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
        }
        if (!string.IsNullOrEmpty(code))
        {
            DataTable dt = new DataTable();
            //if (strisdz == "1")
            //{
               // dt = server.GetDataTable("select a.billCode,a.billName,convert(varchar(10),a.billDate,121) as billDate,isnull((select '['+userCode+']'+userName from bill_users where usercode=a.billUser),a.billUser) as billuser,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.billDept),a.billDept)as billDept,(case a.isgk when '1' then '是'  else '否' end ) as isgk, isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.gkdept),a.gkdept)as gkdept,isnull((select dicName from bill_dataDic where dicType='02' and dicCode=b.bxmxlx),b.bxmxlx) as bxmxlx,b.bxzy,b.bxsm,a.billje,b.note0 as xyxx,b.note1 as bxfy,b.Bxrzh from bill_main  a,bill_ybbxmxb b where a.billCode=b.billCode and a.billname=@code", new SqlParameter[] { new SqlParameter("@code", code) });

            //}
            //else
            //{
                dt = server.GetDataTable("select a.billCode,a.billName,a.BillName2,a.Note2,convert(varchar(10),a.billDate,121) as billDate,isnull((select '['+userCode+']'+userName from bill_users where usercode=a.billUser),a.billUser) as billuser,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.billDept),a.billDept)as billDept,(case a.isgk when '1' then '是'  else '否' end ) as isgk, isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=a.gkdept),a.gkdept)as gkdept,a.billje from bill_main  a  where  a.billCode=@code", new SqlParameter[] { new SqlParameter("@code", code) });

            //}
            if (dt.Rows.Count > 0)
            {
                lbBillCode.Text = Convert.ToString(dt.Rows[0]["billName"]);
                lbBillData.Text = Convert.ToString(dt.Rows[0]["billDate"]);
                lbBillUser.Text = Convert.ToString(dt.Rows[0]["billuser"]);
                lbBillDept.Text = Convert.ToString(dt.Rows[0]["billDept"]);
                lbBillje.Text = Convert.ToDecimal(dt.Rows[0]["billje"]).ToString("N02");
                txtSm.Text = Convert.ToString(dt.Rows[0]["BillName2"]);
                lbBxsm.Text = Convert.ToString(dt.Rows[0]["Note2"]);
            }
            //显示附件

            string fujian = "";
            //if (strisdz == "1")
            //{
            fujian = server.GetCellValue("select top 1 note3 from bill_main where billcode='" + code + "'");

            // string strfjsql = @"select fujian from bill_ybbxmxb where billCode in (select billcode from bill_main where billname='" + code + "')";
            //  fujian = server.GetCellValue(strfjsql);

            //}
            //else
            //{
            //fujian = server.GetCellValue("select fujian from bill_ybbxmxb where billCode ='" + Request["billCode"] + "'");

            // }
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                for (int i = 0; i < arrname.Count(); i++)
                {
                    if (!string.IsNullOrEmpty(arrname[i]))
                    {
                        {
                            this.lalFuJian.Text += "<a href='../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a>";
                        }
                    }
                }
            }
            else
            {
                this.lalFuJian.Text = "无";
            }
        }

        string type = Request["type"];
        if (!string.IsNullOrEmpty(type))
        {
            if (type == "View")
            {
                aduittr.Visible = false;
                btn_audit.Visible = false;
                btn_cancel.Visible = false;
                //判断是否已提交
                DataTable dt = server.GetDataTable("select * from workflowrecord where billCode='" + code + "'", null);
                if (dt.Rows.Count > 0)
                {
                    btn_submit.Visible = false;
                    btn_delete.Visible = false;
                    if (dt.Rows[0]["rdState"].ToString() == "3")
                    {
                        btn_revoke.Visible = true;
                    }
                }


            }
            if (type == "audit")
            {
                btn_submit.Visible = false;
                btn_delete.Visible = false;
                aduittr.Visible = true;
            }

        }

    }
    private string GetTitleByFlowid(string flowid)
    {
        string text = server.GetCellValue("select flowName from mainworkflow where flowId='" + flowid + "'");
        if (!string.IsNullOrEmpty(text) && text.IndexOf("审批") != -1)
        {
            text = text.Replace("审批", "");
        }
        if (!string.IsNullOrEmpty(text) && text.IndexOf("详细") == -1)
        {
            text += "详细";
        }
        return text;
    }

}
