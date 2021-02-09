using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using System.Data;

public partial class webBill_main_Main : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            left.InnerHtml = LeftInnerMaker();
            this.BindInfo();
        }
    }

    private string LeftInnerMaker()
    {
        StringBuilder sb = new StringBuilder();
        SysManager sysMgr = new SysManager();
        string userCode = Convert.ToString(Session["userCode"]);
        string roleCode=Convert.ToString(Session["userGroup"]);
        //IList<Bill_SysMenu> userList = sysMgr.GetMenuByUserAll(userCode);
        UserMessage usMgr = new UserMessage(userCode);
        IList<Bill_SysMenu> userList=new List<Bill_SysMenu>();
        //edit by lvcc 因为未登录 usMgr.GetMenu();会报异常
        try
        {
            userList = usMgr.GetMenu();
        }
        catch (Exception)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        IList<Bill_SysMenu> rootList = sysMgr.GetMenuRoot();
        foreach (Bill_SysMenu rootMenu in rootList)
        {
            var tempList = from linqtemp in userList
                           where linqtemp.MenuId.Substring(0, 2) == rootMenu.MenuId
                           orderby linqtemp.MenuOrder
                           select linqtemp;
               
            if (tempList.Count() > 0)
            {
                sb.Append("<h3><a>");
                sb.Append(rootMenu.ShowName);
                sb.Append("</a></h3>");
                sb.Append("<div><ul>");

                foreach (Bill_SysMenu userMenu in tempList)
                {
                    sb.Append("<li>");
                    sb.Append("<img src=\"../Resources/Images/菜单标记.JPG\" alt=\"\"/>");
                    sb.Append("<span datalink='");
                    sb.Append(userMenu.MenuUrl);
                    sb.Append("' linkname='");
                    sb.Append(userMenu.ShowName);
                    sb.Append("' class='addTabs' >");
                    sb.Append(userMenu.ShowName);
                    sb.Append("</span>");
                    sb.Append("</li>");
                }
                sb.Append("</ul></div>");
            }
        }
        return sb.ToString();
    }

    public void BindInfo()
    {
        #region 待审核单据列表
        string sql = "";
        DataSet temp = new DataSet();
        int index = 1;

        StringBuilder sb = new StringBuilder();
        sb.Append("<table style=\"width: 100%\">");

        string[] arrBillType = new string[] { "ys", "yszj", "ystz", "lscg", "cgsp", "ybbx", "qtbx", "xmys", "cgzjjh","xmzf","ccsq" };
        string[] arrBillTypeName = new string[] { "预算", "预算追加", "预算调整", "报告申请", "采购审批", "一般报销", "其他报销", "项目预算", "采购资金计划","项目支付申请","出差申请" };
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");

        for (int i = 0; i <= arrBillType.Length - 1; i++)
        {

            sql = @"select count(*) from workflowrecord where recordid in
                    (select recordid from dbo.workflowrecords where rdstate=1 and checkuser='" + Session["userCode"].ToString().Trim() + @"')
                    and flowid='" + arrBillType[i] + "' ";
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
                sb.Append("                         <span datalink=\"../MyWorkFlow/BillMainToApprove.aspx?flowid=" + arrBillType[i] + "\" class='addTabs' linkname='" + arrBillTypeName[i]+"审核" + "'>【" + (index++) + "】<font color=red>待审核" + arrBillTypeName[i] + "：");
                sb.Append("                         " + temp.Tables[0].Rows[0][0].ToString() + "</font></span>");
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

                //sb.Append("                         <a href=../ysgl/ystbFrame.aspx target=_self>【" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "】<font color=red>" + temp.Tables[0].Rows[i]["xmmc"].ToString().Trim() + "</font></a>");
                sb.Append("                         <span datalink='../ysgl/ystbAdd.aspx?gcbh=" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "' linkname='预算填报' class='addTabs' >【" + temp.Tables[0].Rows[i]["gcbh"].ToString().Trim() + "】<font color=red>" + temp.Tables[0].Rows[i]["xmmc"].ToString().Trim() + "</font></span>");

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
}
