using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Bll.UserProperty;
using Models;
using System.Data;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_main_MainNew : System.Web.UI.Page
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
                left.InnerHtml = LeftInnerMaker();
            } 
        }

    }
    private string LeftInnerMaker()
    {
        StringBuilder sb = new StringBuilder();
        SysManager sysMgr = new SysManager();
        string userCode = Convert.ToString(Session["userCode"]);
        string roleCode = Convert.ToString(Session["userGroup"]);
        //IList<Bill_SysMenu> userList = sysMgr.GetMenuByUserAll(userCode);
        UserMessage usMgr = new UserMessage(userCode);
        IList<Bill_SysMenu> userList = new List<Bill_SysMenu>();


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
}

