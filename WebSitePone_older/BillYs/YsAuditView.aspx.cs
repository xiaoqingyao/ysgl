using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class BillYs_YsAuditView : System.Web.UI.Page
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
    private string GetTitleByFlowid(string flowid)
    {
        string text = server.GetCellValue("select flowName from mainworkflow where flowId='" + flowid + "'");
        if (!string.IsNullOrEmpty(text) && text.IndexOf("审批") != -1)
        {
            text = text.Replace("审批","");
        }
        if (!string.IsNullOrEmpty(text) && text.IndexOf("详细") == -1)
        {
            text += "详细";
        }
        return text;
    }
    private void BindData()
    {
        string code = Convert.ToString(Request["billCode"]);
        if (!string.IsNullOrEmpty(code))
        {
            ltrTitle.Text = GetTitleByFlowid(Request["flowid"]);
            DataTable dt = server.GetDataTable("select (select top 1 (xmmc) from bill_ysgc a,bill_ysmxb  b      where a.gcbh=b.gcbh and b.billcode=bill_main.billcode  ) as ysgc, billCode,billName,convert(varchar(10),billDate,121) as billDate,isnull((select '['+userCode+']'+userName from bill_users where usercode=billUser),billUser) as billuser,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=billDept),billDept)as billDept,billje from bill_main where  billCode=@code", new SqlParameter[] { new SqlParameter("@code", code) });
            if (dt.Rows.Count > 0)
            {
                lbBillCode.Text = Convert.ToString(dt.Rows[0]["ysgc"]);
                lbBillData.Text = Convert.ToString(dt.Rows[0]["billDate"]);
                lbBillUser.Text = Convert.ToString(dt.Rows[0]["billUser"]);
                lbBillDept.Text = Convert.ToString(dt.Rows[0]["billDept"]);

                lbBillje.Text = Convert.ToDecimal(dt.Rows[0]["billje"]).ToString("N02");
                lbMx.Text = GetYskmStr(code);
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

    private string GetYskmStr(string code)
    {
        string result = "";
        if (!string.IsNullOrEmpty(code))
        {
            DataTable dt = server.GetDataTable(" 	 select  billcode,(select left(xmmc,8) from bill_ysgc  where gcbh=f.gcbh) as ysgc ,yskm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.yskm ) as yskmMc,isnull(ysje ,0)  as je from bill_ysmxb f where billCode='" + code + "'", null);
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(SetKm(dt, "预算过程"));
                result = sb.ToString();
            }
        }
        return result;
    }

    private string SetKm(DataTable ddt, string h)
    {
        if (ddt == null || ddt.Rows.Count == 0)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='div-yskm'>");
        sb.Append("<table class='tab-yskm'>");
        sb.Append("<tr><td class=''>" + h + ":" + ddt.Rows[0]["ysgc"] + "</td></tr>");
        sb.Append("</table>");
        sb.Append("</div>");


        sb.Append("<div class='div-hs'>");
        sb.Append("<h5>科目</h5>");
        sb.Append("<table class='tab-hs' style='color:black;font-size:14px;' >");
        // sb.Append("<tr><th class='tdOdd'>预算科目</th><th>金额</th></tr>");
        for (int j = 0; j < ddt.Rows.Count; j++)
        {
            sb.Append("<tr><td >" + Convert.ToString(ddt.Rows[j]["yskmMc"]) + ":&nbsp;&nbsp;" + Convert.ToDecimal(ddt.Rows[j]["je"]).ToString("N02") + "￥</td></tr>");
        }
        sb.Append("</table>");
        sb.Append("</div>");

        return sb.ToString();
    }
}
