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
using WorkFlowLibrary.WorkFlowBll;

public partial class BillTravelApply_travelApplyList : System.Web.UI.Page
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
            initControl();
            Bind();
        }
    }

    /// <summary>
    /// 初始化控件
    /// </summary>
    private void initControl()
    {
        string strtype = "1";
        //获取上一状态
        object objtype = Request["tp"];
        if (objtype != null && !string.IsNullOrEmpty(objtype.ToString()))
        {
            strtype = objtype.ToString();
        }
        this.ddlTime.SelectedValue = strtype;
    }

    private void Bind()
    {
        string pageNav = "";

        string sql = @"select flowID, billCode,billName,isnull((select '['+usercode+']'+userName from bill_users where usercode=bill_main.billUser),billUser) as billuser,isnull((select '['+deptCode+']'+deptName from bill_departments where deptcode=bill_main.billDept),billDept) as billDept,bill_main.billDept as deptCode,(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,convert(varchar(10),billDate,121) as billDate,billje ,Row_Number()over(order by billCode desc,billdate desc) as crow, b.travelDate,b.arrdess,b.reasion,b.needAmount,b.travelplan   from bill_main ,(select distinct arrdess,reasion,travelDate,needamount,travelplan,maincode from bill_travelApplication) b where billCode=mainCode and billUser='" + Session["userCode"].ToString() + "' and flowID='ccsq'";
        int year = System.DateTime.Now.Year;
        int month = System.DateTime.Now.Month;
        DateTime end = System.DateTime.Now;
        DateTime beg = Convert.ToDateTime(year + "-" + month + "-01");
        if (ddlTime.SelectedValue == "2")
        {
            beg = beg.AddMonths(-1);
            end = beg;
        }
        else if (ddlTime.SelectedValue == "3")
        {
            beg = beg.AddMonths(-2);
        }
        if (ddlTime.SelectedValue != "4")
        {
            sql += " and billDate >='" + beg.ToString("yyyy-MM-dd") + "' and billDate <='" + end.ToString("yyyy-MM-dd") + "'";
        }
        //时间区间
        string strDateType = ddlTime.SelectedValue.Trim();
        string strURL = string.Format("travelApplyList.aspx?tp={0}", strDateType);
        DataTable dt = PubMethod.GetPageData(sql, strURL, out pageNav);
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lbPageNav.InnerHtml = pageNav;
    }

    protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            HiddenField hf = e.Item.FindControl("hfCode") as HiddenField;
            string code = hf.Value.Trim();
            if (!string.IsNullOrEmpty(code))
            {
                StringBuilder sb = new StringBuilder();
                string status = "";
                DataTable workflow = server.GetDataTable("select * from workflowrecord where billCode='" + code + "'", null);

                if (workflow.Rows.Count == 0)
                    sb.Append("<div class='checkStatus' ><lable>审批状态：未提交</lable></div>");
                else
                {
                    status = workflow.Rows[0]["rdState"].ToString();
                    if (status == "1")
                    {
                        WorkFlowRecordManager bll = new WorkFlowRecordManager();
                        string state = bll.WFState(code);
                        sb.Append("<div class='checkStatus' ><lable>审批状态：" + state + "</lable></div>");
                    }
                    else if (status == "2")
                        sb.Append("<div class='checkStatus'><lable>审批状态：审核通过</lable></div>");
                    else if (status == "3")
                    {
                        string flowid = server.GetCellValue("select flowid from bill_main where billCode='" + code + "'");
                        sb.Append("<div class='checkStatus'><lable>审批状态：驳回</lable></div>");
                        sb.Append("<div  style='text-align:center'><a data-role='button' data-inline='true' data-theme='d' onclick=\"RevokeCheck(this,'" + flowid + "','" + code + "')\"><img src='../images/metro/Editor.png' />审核撤销</a></div>");
                    }
                }

                Label lb = e.Item.FindControl("lbmx") as Label;
                lb.Text = sb.ToString();
                HtmlContainerControl hcc = e.Item.FindControl("optionDiv") as HtmlContainerControl;
                if (status == "3")
                {
                    hcc.Attributes.CssStyle.Add("display", "none");
                }
                else if (!string.IsNullOrEmpty(status))
                {
                    hcc.Visible = false;
                }
            }
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }

}
