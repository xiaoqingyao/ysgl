using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowBll;

public partial class BillYs_JsAuditList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_self');", true);
            Response.Redirect("../Login.aspx");
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        ltrTitle.Text = GetTitleByFlowid(Request["flowid"]);
        string pageNav = "";
        string sql = GetSql();

        DataTable dt = PubMethod.GetPageData(sql, "BillYs_JsAuditList.aspx?flowid=jfsq", out pageNav);

        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }

    private string GetTitleByFlowid(string flowid)
    {
        string text = server.GetCellValue("select flowName from mainworkflow where flowId='" + flowid + "'");
        if (!string.IsNullOrEmpty(text) && text.IndexOf("审批") == -1)
        {
            text += "审批";
        }
        return text;
    }
    private string GetSql()
    {
        string flowid = Request["flowid"];
        string usercode = Session["userCode"].ToString();
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, flowid, strStatus);

        StringBuilder sb = new StringBuilder();

        if (list.Count > 0)
        {
            sb.Append(@" select  *,(select top 1 (xmmc) from bill_ysgc a,bill_ysmxb  b      where a.gcbh=b.gcbh and b.billcode=bill_main.billcode  ) as ysgc 
 ,(select '['+deptCode+']'+deptName from dbo.bill_departments where deptCode=bill_main.billDept) as showdept
              ,  Row_number()over(order by billdate desc,billName desc) as crow from bill_main  where  flowid='" + flowid + "' and  billcode in(");

            foreach (string billcode in list)
            {
                sb.Append("'");
                sb.Append(billcode);
                sb.Append("',");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(") ");
            return sb.ToString();
        }
        else
        {
            return null;
        }

    }
    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            HiddenField hf = e.Item.FindControl("hfCode") as HiddenField;
            string code = hf.Value.Trim();
            if (!string.IsNullOrEmpty(code))
            {
                DataTable dt = server.GetDataTable(@" 	 select  billcode,(select left(xmmc,8) from bill_ysgc  where gcbh=f.gcbh) as ysgc ,yskm as yskmCode
                    ,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.yskm ) as yskmMc,isnull(ysje ,0)  as je from bill_ysmxb f where billCode='" + code + "'", null);
                if (dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(SetKm(dt, "预算过程"));
                    Label lb = e.Item.FindControl("lbmx") as Label;
                    lb.Text = sb.ToString();
                }
            }
        }
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
        sb.Append("<table class='tab-hs ItemTable'>");
        // sb.Append("<tr><th class='tdOdd'>预算科目</th><th>金额</th></tr>");
        for (int j = 0; j < ddt.Rows.Count; j++)
        {
            sb.Append("<tr><td >" + Convert.ToString(ddt.Rows[j]["yskmMc"]) + ":&nbsp;&nbsp;￥" + Convert.ToDecimal(ddt.Rows[j]["je"]).ToString("N02") + "</td></tr>");
        }
        sb.Append("</table>");
        sb.Append("</div>");

        return sb.ToString();
    }

}