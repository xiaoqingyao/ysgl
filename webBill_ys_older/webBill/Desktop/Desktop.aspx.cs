using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Desktop_Desktop : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                this.BindInfo();
            }
        }
    }

    public void BindInfo()
    {
        #region 待审核单据列表
        string billCodes = "";
        string sql = "";
        DataSet temp = new DataSet();
        int index = 1;

        StringBuilder sb = new StringBuilder();
        sb.Append("<table style=\"width: 100%\">");

        string[] arrBillType = new string[] { "ys", "yszj", "ystz", "lscg", "cgsp", "ybbx" ,"qtbx","xmys","cgzjjh"};
        string[] arrBillTypeName = new string[] { "预算", "预算追加", "预算调整", "报告申请", "采购审批", "一般报销","其他报销","项目预算","采购资金计划" };
        //string[] arrBillTypeUrl = new string[] { "../ysgl/ysshList.aspx", "../ysgl/yszjshList.aspx", "../ysgl/ystzSh.aspx", "../fysq/sp_lscgList.aspx", "../fysq/sp_cgspList.aspx", "../bxgl/bxshFrame.aspx", "../bxgl/qtbxshFrame.aspx", "../xmsz/ysshList.aspx","../cgzj/sp_cgzjList.aspx" };
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //string selfDeptCode = (new Departments()).GetZgDepartments(Session["userCode"].ToString().Trim());

        for (int i = 0; i <= arrBillType.Length - 1; i++)
        {
            //billCodes = (new workFlowLibrary.workFlow()).getRightStepBills(arrBillType[i], Session["userGroup"].ToString().Trim(), Session["userCode"].ToString().Trim(), deptCodes);
            /*
            sql = "select 1 from bill_main where flowid='" + arrBillType[i] + "' and billCode in (" + billCodes + ")";
            temp = server.GetDataSet(sql);
            */
            sql = @"select count(*) from workflowrecord where recordid in
                    (select recordid from dbo.workflowrecords where rdstate=1 and checkuser='"+Session["userCode"].ToString().Trim()+@"')
                    and billtype='"+arrBillType[i]+"'";
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
                sb.Append("                         <a href=\"../MyWorkFlow/BillMainToApprove.aspx?flowid="+arrBillType[i]+"\" target=_self>【" + (index++) + "】<font color=red>待审核" + arrBillTypeName[i] + "：");
                //sb.Append("                         " + temp.Tables[0].Rows.Count.ToString() + "</font></a>");
                sb.Append("                         " + temp.Tables[0].Rows[0][0].ToString() + "</font></a>");
            }
            sb.Append("                     </td>");
            sb.Append("                  </tr>");
            sb.Append("              </table>");
            sb.Append("          </td>");
            sb.Append("     </tr>");
        }
        sb.Append("</table>");
        this.info.InnerHtml = sb.ToString();
        #endregion

        #region 单位预算填报情况

        temp = server.GetDataSet("select * from bill_userright where (userCode='" + Session["userCode"].ToString().Trim() + "' and objectID='0303' and righttype='1') or (usercode=(select userGroup from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') and objectID='0303' and righttype='3')");
        if (temp.Tables[0].Rows.Count == 0)
        {
            this.ystbTitle.Style["display"] = "none";
            this.ystbInfoList.Style["display"] = "none";
        }
        else
        {
            sb = new StringBuilder();
            sb.Append("<table style=\"width: 100%\">");

            temp = server.GetDataSet("select * from bill_ysgc where jzsj>=getdate() and gcbh not in (select gcbh from bill_ysmxb where ysDept=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and ystype='1') and status='1'");


            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                sb.Append("     <tr>");
                sb.Append("         <td>");
                sb.Append("             <table class=\"myGrid\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 100%\">");
                sb.Append("                 <tr class=\"myGridItem\">");
                sb.Append("                     <td>");

                sb.Append("                         <a href=../ysgl/ystbFrame.aspx target=_self>【" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "】<font color=red>" + temp.Tables[0].Rows[i]["xmmc"].ToString().Trim() + "</font></a>");

                sb.Append("                     </td>");
                sb.Append("                  </tr>");
                sb.Append("              </table>");
                sb.Append("          </td>");
                sb.Append("     </tr>");
            }
            sb.Append("</table>");
            this.ystbInfoList.InnerHtml = sb.ToString();
        }
        #endregion

        #region yqtsInfo
        sb = new StringBuilder();
        sb.Append("<table style=\"width: 100%\">");

        temp = server.GetDataSet("select top 5 * from bill_msg order by date desc");

        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            sb.Append("     <tr>");
            sb.Append("         <td>");
            sb.Append("             <table class=\"myGrid\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width: 100%\">");
            sb.Append("                 <tr class=\"myGridItem\">");
            sb.Append("                     <td>");

            sb.Append("                         <a onclick=\"openMsg(" + temp.Tables[0].Rows[i]["id"].ToString().Trim() + ");\" href=# target=_self title=\"" + temp.Tables[0].Rows[i]["title"].ToString().Trim() + "\">" + (temp.Tables[0].Rows[i]["title"].ToString().Trim().Length > 33 ? temp.Tables[0].Rows[i]["title"].ToString().Trim().Substring(0, 31) + "..." : temp.Tables[0].Rows[i]["title"].ToString().Trim()) + "</a>");

            sb.Append("                     </td>");
            sb.Append("                  </tr>");
            sb.Append("              </table>");
            sb.Append("          </td>");
            sb.Append("     </tr>");
        }
        sb.Append("</table>");
        this.yqtsInfo.InnerHtml = sb.ToString();
        #endregion

        #region 技术支持显示

        string strsql = "select parVal from bill_syspar where parname='技术支持'";
        this.jszcInfo.InnerHtml = server.GetCellValue(strsql);

        #endregion
    }
    protected void btn_ed_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "editBill('DeskMsgDetails.aspx');", true);
    }
}