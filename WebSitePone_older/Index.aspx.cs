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
using System.Data.SqlClient;

public partial class Index : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='Login.aspx'", true);
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindData();
        }
    }


    private void BindData()
    {
        string userCode = Session["userCode"].ToString();
        string strsql = @"select * from ph_sysmenu where menuid in(select distinct menuid from ph_menuRight where objectID='" + userCode + "' and rightType='1' or  rightType='2' and objectID = (select userGroup from bill_users where usercode='" + userCode + "')) and isnull(menuState,'0')!='0' order by convert(int, menuOrder )asc";
        DataTable dtMenu = server.GetDataTable(strsql, null);
        rptMenu.DataSource = dtMenu;
        rptMenu.DataBind();
        if (dtMenu.Rows.Count < 1)
        {
            msg.InnerText = "您暂时没有任何操作权限，请联系系统管理员为您分配权限！";
        }
    }



    protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        HiddenField hf = e.Item.FindControl("hfId") as HiddenField;
        string sql = server.GetCellValue("select isnull(getCountSql,'') from ph_sysmenu where menuid='" + hf.Value + "'");
        if (!string.IsNullOrEmpty(sql))
        {
            int count = Convert.ToInt32(server.GetCellValue(sql.Replace("@userCode", "'" + Session["userCode"].ToString() + "'")));
            HtmlContainerControl span = e.Item.FindControl("span_num") as HtmlContainerControl;
            span.InnerText = count.ToString();
        }
    }
}
