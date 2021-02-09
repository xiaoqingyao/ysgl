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
using Bll;
using Bll.UserProperty;

public partial class bxd_ybbxList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysmgr = new YsManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../Login.aspx','_self');", true);
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
        string djlx = "ybbx";
        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {

            strisdz =Request["isdz"].ToString();
        }
        if (strisdz=="1")
        {
            djlx = "yksq_dz";
        }
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            djlx = Convert.ToString(Request["dydj"]);
        }
        string sql = @"select Row_Number()over(order by billdate desc,billName desc) as crow ,sum(je) as billJe,billname,flowid,stepid,billuser,billdate,isgk,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy,
(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select top 1 billdept from bill_main where billname=main.billname) as billdept,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName
from bill_main main inner join bill_ybbxmxb_fykm
fykm on main.billcode=fykm.billcode  where flowid='" + djlx + "' and (billUser='" + Session["userCode"].ToString().Trim() + "' or main.billcode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "') )";//or  billDept in (" + deptCodes + ")
//        string sql = @"select flowID, billName,isnull((select '['+usercode+']'+userName from bill_users where usercode=bill_main.billUser),billUser) as billuser
//        ,isGk,gkDept,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,isnull((select '['+deptCode+']'+deptName 
//         from bill_departments where deptcode=bill_main.billDept),billDept) as billDept,bill_main.billDept as deptCode,billCode,(select xmmc from bill_ysgc where gcbh=billName)
//        as billName2,(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,convert(varchar(10),billDate,121) as billDate,billje 
//        ,Row_Number()over(order by billName desc,billdate desc) as crow from bill_main 
//        where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "')) and flowID='" + djlx + "'";
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
            sql += " and convert(varchar(10),billDate,121) >='" + beg.ToString("yyyy-MM-dd") + "' and convert(varchar(10),billDate,121) <='" + end.ToString("yyyy-MM-dd") + "'";
        }
        //时间区间
        string strDateType = ddlTime.SelectedValue.Trim();
        string strURL = string.Format("ybbxList.aspx?tp={0}", strDateType);
        //求和
        sql += " group by billname,flowid,stepid,billuser,billdate,isgk ";

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
            if (Request["isdz"].ToString()=="1")
            {
                hf= e.Item.FindControl("hfname") as HiddenField;
            }
            string code = hf.Value.Trim();


         
            HiddenField billdate = e.Item.FindControl("hiddatetime") as HiddenField;
            HiddenField hiddeptCode = e.Item.FindControl("hidbillDept") as HiddenField;
            string billDate = billdate.Value;
            string deptCode = hiddeptCode.Value;
         
         
            if (!string.IsNullOrEmpty(code))
            {


                string strsql = @"select  mxGuid ,fykm as yskmCode,(select  '['+yskmCode+']'+yskmMc as yskm  from bill_yskm where yskmCode=f.fykm ) as yskmMc,isnull(je ,0)  as je from bill_ybbxmxb_fykm f where 1=1";
                if (Request["isdz"].ToString()=="1")
                {
                    strsql += " and billCode in(select billcode from  bill_main where billname=('"+code+"'))";
                }
                else
                {
                    strsql += " and  billCode='" + code + "'";
                }
                
                DataTable dt = server.GetDataTable(strsql, null);
                if (dt.Rows.Count > 0)
                {

                    string strworksql = @"select * from workflowrecord where billCode='" + code + "'";
                  
                    DataTable workflow = server.GetDataTable(strworksql, null);
                   
                    
                    string status = "";
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        //预算金额
                        string strdydj = "02";
                        deptCode = string.IsNullOrEmpty(deptCode) ? "" : deptCode.Split(']')[0].Trim('[');
                        string kmCode = string.IsNullOrEmpty(dt.Rows[i]["yskmMc"].ToString()) ? "" : dt.Rows[i]["yskmMc"].ToString().Split(']')[0].Trim('[');
                        string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(billDate));

                        if (!string.IsNullOrEmpty(Request["flowid"]))
                        {
                            if (Request["flowid"].ToString() == "ybbx")
                            {
                                strdydj = "02";
                            }

                        }
                        decimal ysje = ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);//预算金额

                        decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                        //是否启用销售提成模块
                        bool hasSaleRebate = new ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");
                        decimal syje = ysje - hfje;
                        sb.Append("<div class='div-yskm'>");
                        sb.Append("<table class='tab-yskm'>");
                        //sb.Append("<tr><td class='tdOdd'>费用科目:</td><td>" + Convert.ToString(dt.Rows[i]["yskmMc"]) + "</td></tr>");
                        sb.Append("<tr><td>" + Convert.ToString(dt.Rows[i]["yskmMc"]) + "预算金额：￥(" + Convert.ToDecimal(ysje).ToString("N02") + ") 剩余金额：￥(" + Convert.ToDecimal(syje).ToString("N02") + ")" + "报销金额：￥(" + Convert.ToDecimal(dt.Rows[i]["je"]).ToString("N02") + ")</td></tr>");
                        sb.Append("</table>");
                        sb.Append("</div>");
                        DataTable temp = server.GetDataTable("select (select '['+deptCode+']'+deptName  from bill_departments where deptCode=a.deptCode) as Dept,isnull(je,0) as je  from bill_ybbxmxb_fykm_dept a where kmmxGuid ='" + Convert.ToString(dt.Rows[i]["mxGuid"]) + "'", null);
                        if (temp.Rows.Count > 0)
                        {
                            sb.Append("<div class='div-hs'>");
                            sb.Append("<h5>核算部门</h5>");
                            sb.Append("<table class='tab-hs ItemTable'>");
                            // sb.Append("<tr><th class='tdOdd'>部门</th><th>核算金额</th></tr>");
                            for (int j = 0; j < temp.Rows.Count; j++)
                            {
                                sb.Append("<tr><td class='tdOdd'>" + Convert.ToString(temp.Rows[j]["Dept"]) + ":&nbsp;&nbsp;" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "￥</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("</div>");
                        }

                        temp = server.GetDataTable("select distinct  '['+b.xmCode+']'+xmName as xmCode,isnull(je,0) as je from bill_ybbxmxb_hsxm a,bill_xm b where a.xmcode=b.xmcode and kmmxGuid='" + Convert.ToString(dt.Rows[i]["mxGuid"]) + "'", null);
                        if (temp.Rows.Count > 0)
                        {
                            sb.Append("<div class='div-hs'>");
                            sb.Append("<h5>核算项目</h5>");
                            sb.Append("<table class='tab-hs ItemTable'>");
                            // sb.Append("<tr><th class='tdOdd'>项目</th><th>核算金额</th></tr>");
                            for (int j = 0; j < temp.Rows.Count; j++)
                            {
                                sb.Append("<tr><td class='tdOdd'>" + Convert.ToString(temp.Rows[j]["xmCode"]) + ":&nbsp;&nbsp;" + Convert.ToDecimal(temp.Rows[j]["je"]).ToString("N02") + "￥</td></tr>");
                            }
                            sb.Append("</table>");
                            sb.Append("</div>");
                        }

                    }


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
    }
    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }
}
