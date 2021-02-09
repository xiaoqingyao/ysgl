<%@ WebHandler Language="C#" Class="GetDeskData" %>

using System;
using System.Web;
using System.Data;
using System.Text;

public class GetDeskData : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        StringBuilder sb = new StringBuilder();
        string userCode = context.Session["userCode"].ToString().Trim();
        string[] ret = BindInfo(userCode);
        sb.Append(ret[0]);
        sb.Append("|");
        sb.Append(ret[1]);
        context.Response.Write(sb);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public string[] BindInfo(string userCode)
    {
        string [] ret = new string [2]; 
        #region 待审核单据列表
        string sql = "";
        DataSet temp = new DataSet();
        int index = 1;

        StringBuilder sb = new StringBuilder();
        sb.Append("<table style=\"width: 100%\">");

        string[] arrBillType = new string[] { "ys","xmys","xmyshz", "yszj", "ystz", "lscg", "cgsp", "ybbx", "qtbx", "xmys", "cgzjjh","xmzf" };
        string[] arrBillTypeName = new string[] { "预算","项目预算","项目预算汇总", "预算追加", "预算调整", "报告申请", "采购审批", "一般报销", "其他报销", "项目预算", "采购资金计划","项目支付申请" };
        string deptCodes = (new Departments()).GetUserRightDepartments(userCode, "");

        for (int i = 0; i <= arrBillType.Length - 1; i++)
        {

            sql = @"select count(*) from workflowrecord where recordid in
                    (select recordid from dbo.workflowrecords where rdstate=1 and checkuser='" + userCode + @"')
                    and flowid='" + arrBillType[i] + "'";
            temp = server.GetDataSet(sql);

            sb.Append("     <tr>");
            sb.Append("         <td>");
            sb.Append("             <table class=\"myGrid\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 100%\">");
            sb.Append("                 <tr class=\"myGridItem\">");
            sb.Append("                     <td>");

            /*
            int infoCount = int.Parse(temp.Tables[0].Rows.Count.ToString());
             */
            int infoCount = Convert.ToInt32(temp.Tables[0].Rows[0][0]);
            if (infoCount == 0)
            {
                sb.Append("                         【" + (index++) + "】待审核" + arrBillTypeName[i] + "：");
                //sb.Append("                         " + temp.Tables[0].Rows.Count.ToString());
                sb.Append("                         " + temp.Tables[0].Rows[0][0].ToString());
            }
            else
            {
                //sb.Append("                         <a href=\"../MyWorkFlow/BillMainToApprove.aspx?flowid=" + arrBillType[i] + "\" target=_self>【" + (index++) + "】<font color=red>待审核" + arrBillTypeName[i] + "：");
                sb.Append("                         <span datalink=\"../MyWorkFlow/BillMainToApprove.aspx?flowid=" + arrBillType[i] + "\" class='addTabs' linkname='" + arrBillTypeName[i] + "审核" + "'>【" + (index++) + "】<font color=red>待审核" + arrBillTypeName[i] + "：");
                sb.Append("                         " + temp.Tables[0].Rows[0][0].ToString() + "</font></span>");
            }
            sb.Append("                     </td>");
            sb.Append("                  </tr>");
            sb.Append("              </table>");
            sb.Append("          </td>");
            sb.Append("     </tr>");
        }
        sb.Append("</table>");
        ret[0] = sb.ToString();
        #endregion

        #region 单位预算填报情况

        temp = server.GetDataSet("select * from bill_userright where (userCode='" + userCode + "' and objectID='0303' and righttype='1') or (usercode=(select userGroup from bill_users where usercode='" + userCode + "') and objectID='0303' and righttype='3')");
        if (temp.Tables[0].Rows.Count == 0)
        {
            ret[1] = "";
        }
        else
        {
            sb = new StringBuilder();
            sb.Append("<table style=\"width: 100%\">");

            temp = server.GetDataSet("select * from bill_ysgc where jzsj>=getdate() and gcbh not in (select gcbh from bill_ysmxb where ysDept=(select userDept from bill_users where userCode='" + userCode + "') and ystype='1') and status='1'");


            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                sb.Append("     <tr>");
                sb.Append("         <td>");
                sb.Append("             <table class=\"myGrid\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 100%\">");
                sb.Append("                 <tr class=\"myGridItem\">");
                sb.Append("                     <td>");

                //sb.Append("                         <a href=../ysgl/ystbFrame.aspx target=_self>【" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "】<font color=red>" + temp.Tables[0].Rows[i]["xmmc"].ToString().Trim() + "</font></a>");
                sb.Append("                         <span datalink='../ysgl/ystbFrame.aspx' linkname='预算填报' class='addTabs' >【" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "】<font color=red>" + temp.Tables[0].Rows[i]["xmmc"].ToString().Trim() + "</font></span>");

                sb.Append("                     </td>");
                sb.Append("                  </tr>");
                sb.Append("              </table>");
                sb.Append("          </td>");
                sb.Append("     </tr>");
            }
            sb.Append("</table>");
            ret[1] = sb.ToString();
        }
        #endregion

        return ret;
    }

}