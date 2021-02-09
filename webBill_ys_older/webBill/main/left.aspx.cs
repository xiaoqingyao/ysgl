using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Ajax;

public partial class main_left : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        Ajax.Utility.RegisterTypeForAjax(typeof(main_left));
        
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
    }
    [Ajax.AjaxMethod()]
    public string getMenuName(string menuID)
    {
        return server.GetCellValue("select menuName from bill_sysmenu where menuid='" + menuID + "'");
    }

    /// <summary>
    /// 获取权限导航菜单
    /// </summary>
    /// <returns></returns>
    [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
    public string getNavMenu()
    {
        List<string> arr = new List<string>();
        DataSet temp = new DataSet();
        DataSet temp2 = new DataSet();
        if (Session["userCode"].ToString().Trim() == "admin")
        {
            temp = server.GetDataSet("select * from bill_sysMenu where  menuid in ('01','06','07') order by menuOrder");
            temp2 = server.GetDataSet("select * from bill_sysMenu where  menuid in ('0101','0102','0103','0104','0602','0603','0604','0605','0608','0609','0610','0611','0701')");
        }
        else
        {
            temp = server.GetDataSet("select * from bill_sysMenu where menuID='06' or menuID in (select distinct(left(objectID,2)) from bill_userRight where rightType='1' and userCode='" + Session["userCode"].ToString().Trim() + "') order by menuOrder");
           
            temp2 = server.GetDataSet("select * from bill_sysMenu where (menuID='0608' or menuID in (select objectID from bill_userRight where rightType='1' and userCode='" + Session["userCode"].ToString().Trim() + "')) and len(menuid)=4 order by menuOrder");

            if (temp2.Tables[0].Rows.Count==1)
            {
                temp = server.GetDataSet("select * from bill_sysMenu where menuID='06' or menuID in (select distinct(left(objectID,2)) from bill_userRight where rightType='3' and userCode='" + Session["userGroup"].ToString().Trim() + "') order by menuOrder");

                temp2 = server.GetDataSet("select * from bill_sysMenu where (menuID='0608' or menuID in (select objectID from bill_userRight where rightType='3' and userCode='" + Session["userGroup"].ToString().Trim() + "')) and len(menuid)=4 order by menuOrder");

            }
        }
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            if (temp.Tables[0].Rows[i]["menuID"].ToString().Trim().Length == 2)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("title:'" + temp.Tables[0].Rows[i]["menuName"].ToString().Trim() + "',xtype: 'panel', autoScroll: true");
                sb.Append(",html:'<table>");
                for (int j = 0; j <= temp2.Tables[0].Rows.Count - 1; j++)
                {
                    if (temp2.Tables[0].Rows[j]["menuID"].ToString().Trim().Substring(0, 2) == temp.Tables[0].Rows[i]["menuID"].ToString().Trim())
                    {
                        sb.Append("<tr><td height=2px></td></tr>");
                        sb.Append("<tr><td width=25px align=right><img width=20px src=../Resources/Images/菜单标记.jpg /></td><td width=2px></td>");

                        string menuurl = temp2.Tables[0].Rows[j]["menuUrl"].ToString().Trim();
                        if (menuurl.IndexOf('?') > 0)
                        {
                            sb.Append("<td align=left><a href=" + temp2.Tables[0].Rows[j]["menuUrl"].ToString().Trim() + "&menuID=" + temp2.Tables[0].Rows[j]["menuID"].ToString().Trim() + " target=main>" + temp2.Tables[0].Rows[j]["menuName"].ToString().Trim() + "</a></td></tr>");
                        }
                        else
                        {
                            sb.Append("<td align=left><a href=" + temp2.Tables[0].Rows[j]["menuUrl"].ToString().Trim() + "?menuID=" + temp2.Tables[0].Rows[j]["menuID"].ToString().Trim() + " target=main>" + temp2.Tables[0].Rows[j]["menuName"].ToString().Trim() + "</a></td></tr>");
                        }
                        
                        sb.Append("<tr><td height=2px></td></tr>");
                    }
                }
                sb.Append("</table>'");
                arr.Add("{" + sb.ToString() + "}");
            }
        }
        StringBuilder returnSb = new StringBuilder();
        for (int i = 0; i <= arr.Count - 1; i++)
        {
            if (i == arr.Count - 1)
            {
                returnSb.Append(arr[i].ToString().Trim());
            }
            else
            {
                returnSb.Append(arr[i].ToString().Trim() + ",");
            }
        }

        return "[" + returnSb.ToString() + "]";
    }
}
