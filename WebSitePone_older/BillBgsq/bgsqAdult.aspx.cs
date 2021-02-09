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
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowBll;
using Bll;
using Bll.UserProperty;
using Models;
public partial class BillBgsq_bgsqAdult : System.Web.UI.Page
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
        DataTable dt = PubMethod.GetPageData(sql, "ybbxAuditList.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }
    private string GetSql()
    {
        string flowid = "lscg";
        string usercode = Session["userCode"].ToString();
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, flowid, strStatus);

        StringBuilder sb = new StringBuilder();

        if (list.Count > 0)
        {
            sb.Append("select  convert(varchar(10),a.billdate,121) as billDate,a.billcode,a.flowID , (select '['+userCode+']'+userName from bill_users where userCode=a.billUser) as billUser ,a.billJe, isnull((select '['+deptcode+']'+deptName from bill_departments where deptcode=a.billDept),a.billdept) as billdept, Row_number()over(order by a.billdate desc,a.billName desc) as crow,  (select case when len(Convert(varchar(2000),zynr))>20 then substring(zynr,0,20)+'...'  else zynr end from bill_lscg where cgbh =a.billCode) as zynr from bill_main a,bill_lscg b where a.billCode=b.cgbh and  a.flowid='" + flowid + "' and a.billcode in(");
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
        }
    }
}
