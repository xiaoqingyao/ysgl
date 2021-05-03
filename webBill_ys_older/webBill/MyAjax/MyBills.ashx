<%@ WebHandler Language="C#" Class="MyBills" %>

using System;
using System.Web;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;
using System.Data;

public class MyBills : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        StringBuilder sb = new StringBuilder();
        string userCode = Convert.ToString(context.Session["userCode"]);
        //我的单据绑定
        string strposql = @"select  top 10 billName,flowID,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,
(select xmmc from bill_ysgc where gcbh=billName) as billName,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,
(select top 1 djmc from bill_djlx where djbh=bill_main.flowid) as billType,
convert(varchar(10),billdate,20) as billdate ,billje from bill_main
where (billUser='" + context.Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + context.Session["userCode"].ToString().Trim() + "')) and stepid<>'end' and flowid not in('ys','xmys','xmyshz','zjys','srys','zcys','chys','wlys') order by billdate desc";

        DataTable temp = server.GetDataSet(strposql).Tables[0];
        foreach (DataRow dataRow in temp.Rows)
        {
            sb.Append("<tr>");
            sb.Append("<td style='width:90px'>" + dataRow["billType"].ToString() + "</td>");
            sb.Append("<td>" + dataRow["bxzy"].ToString() + "</td>");
            sb.Append("<td style='width:80px'>" + dataRow["billdate"].ToString() + "</td>");
            string stepid = dataRow["stepid"].ToString();
            string billname = dataRow["billname"].ToString();
            string flowID = dataRow["flowID"].ToString();
            string billCode = dataRow["billCode"].ToString();
            string state = "";
            WorkFlowRecordManager bll = new WorkFlowRecordManager();
            if (flowID.Equals("yszj") || flowID.Equals("ystz"))
            {
                state = bll.WFState(billCode);
            }
            else
            {
                state = bll.WFState(billname);
            }
            sb.Append("<td style='width:130px'>" + state + "</td>");
            sb.Append("</tr>");
        }
        if (sb.ToString().Length == 0)
        {
            sb.Append("您暂无待处理单据");
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(sb.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}