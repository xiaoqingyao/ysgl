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
using System.Collections.Generic;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;

public partial class BillTravelApply_travelApplyAudit : System.Web.UI.Page
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
        string pageNav = "";
        string sql = GetSql();
        DataTable dt = PubMethod.GetPageData(sql, "travelApplyAudit.aspx", out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }
    private string GetSql()
    {
        string flowid = "ccsq";
        string usercode = Session["userCode"].ToString();
        //IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request);

        string strStatus = "1";//this.rdoStatusNow.Checked ? "1" : "2";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, flowid, strStatus);

        StringBuilder sb = new StringBuilder();

        if (list.Count > 0)
        {
            sb.Append("select  convert(varchar(10),a.billdate,121) as billDate,a.billcode,a.flowID , (select '['+userCode+']'+userName from bill_users where userCode=a.billUser) as billUser ,a.billJe, isnull((select '['+deptcode+']'+deptName from bill_departments where deptcode=a.billDept),a.billdept) as billdept, Row_number()over(order by a.billdate desc,a.billName desc) as crow ,b.travelDate,b.arrdess,b.reasion,b.needAmount,b.travelplan   from bill_main a ,(select distinct arrdess,reasion,travelDate,needamount,travelplan,maincode from bill_travelApplication) b where a.billCode=b.maincode and  a.flowid='" + flowid + "' and a.billcode in(");
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