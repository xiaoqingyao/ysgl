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

public partial class Index : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            Response.Redirect("Login.aspx");
        }
        if (!IsPostBack)
        {
            span_ybbxd_unCheck.InnerText = Convert.ToString(GetUnCheckData());
            span_ybbxd_unSubmit.InnerText = Convert.ToString(GetYbbxData());
        }

    }

    /// <summary>
    /// 获取当前用户待审核的一般报销单
    /// </summary>
    private int GetUnCheckData()
    {

        return GetData();
    }

    /// <summary>
    /// 获取当前用户未提交的一般报销单
    /// </summary>
    /// <returns></returns>
    private int GetYbbxData()
    {
        string djlx = "ybbx";
        string sql = @"select  billName,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,
(select xmmc from bill_ysgc where gcbh=billName) as billName2,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,
billdate,billje ,Row_Number()over(order by billName desc,billdate desc) as crow from bill_main where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "')) and flowID='" + djlx + "'";
        string strsqlcount = "select isnull(count(*),0) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        return int.Parse(server.GetCellValue(strsqlcount));

    }

    private int GetData()
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        string request = Request.QueryString["flowid"];
        if (string.IsNullOrEmpty(request))
        {
        request="ybbx";
        }
        string flowid = request;
        string usercode = Session["userCode"].ToString();
       
        string strStatus = "1";
        IList<string> list = new WorkFlowRecordManager().GetAppBill(usercode, request, strStatus);

        return list.Count;

    }

}
