using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowBll;

public partial class BillYstz_YstzAuditList : System.Web.UI.Page
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
        string pageNav = "";
        string sql = GetSql();

        DataTable dt = PubMethod.GetPageData(sql, "YstzAuditList.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }
    private string GetSql()
    {

        string flowid = "ystz";

        string usercode = Session["userCode"].ToString();
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, flowid, strStatus);

        StringBuilder sb = new StringBuilder();

        if (list.Count > 0)
        {
            sb.Append(" select   convert(char(10), billDate,121) as showdate,*,isnull((select '['+deptCode+']'+deptname from bill_departments where deptCode=billDept),billDept)as showDept,(select top 1 (xmmc) from bill_ysgc a,bill_ysmxb  b      where a.gcbh=b.gcbh and b.billcode=bill_main.billcode  ) as ysgc ,  Row_number()over(order by billdate desc,billName desc) as crow from bill_main  where  flowid='" + flowid + "' and  billcode in(");

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
                DataTable dt = server.GetDataTable(" 	 select  billcode,(select xmmc from bill_ysgc  where gcbh=f.gcbh) as ysgc ,yskm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.yskm ) as yskmMc,isnull(ysje ,0)  as je,sm from bill_ysmxb f where billCode='" + code + "'", null);
                if (dt.Rows.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable ddt = PubMethod.GetNewDataTable(dt,"je>0");
                    //sb.Append(SetKm(null, "调整入"));
                    sb.Append( SetKm( ddt,"调整入"));
                    ddt = PubMethod.GetNewDataTable(dt, "je<0");
                   // sb.Append(SetKm(null, "调整出"));     
                    sb.Append(SetKm(ddt, "调整出"));                  
                    Label lb = e.Item.FindControl("lbmx") as Label;
                    lb.Text = sb.ToString();
                }
            }
        }
    }

    private string SetKm(DataTable ddt,string h)
    {
        if (ddt == null || ddt.Rows.Count == 0)
            return "";
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='div-yskm'>");
        sb.Append("<table class='tab-yskm'>");
        sb.Append("<tr><td class=''>" + h + "：</td></tr>");//:" +ddt.Rows[0]["ysgc"]+ "
        sb.Append("</table>");
        sb.Append("</div>");


        sb.Append("<div>");
        //sb.Append("<h5>预算科目</h5>");
        sb.Append("<table class='tab-hs' style='color:black;font-size:14px;' >");
        // sb.Append("<tr><th class='tdOdd'>预算科目</th><th>金额</th></tr>");
        for (int j = 0; j < ddt.Rows.Count; j++)
        {
            sb.Append("<tr><td > 预算过程:" + ddt.Rows[j]["ysgc"].ToString() + "</td></tr>");
            sb.Append("<tr><td > 预算科目:" + Convert.ToString(ddt.Rows[j]["yskmMc"]) + "</td></tr>");
            sb.Append("<tr><td >预算金额：￥" + Convert.ToDecimal(ddt.Rows[j]["je"]).ToString("N02") + "</td></tr>");
            sb.Append("<tr><td >说明：" + Convert.ToString(ddt.Rows[j]["sm"]) + "</td></tr>");
            sb.Append("<tr><td ><hr/></td></tr>");

        }
        sb.Append("</table>");
        sb.Append("</div>");

        return sb.ToString();
    }

}
