using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class yskm_yskmDz : System.Web.UI.Page
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
                this.bindData();
            }
        }
    }

    public void bindData()
    {
        string yskmCode = Page.Request.QueryString["yskmCode"].ToString().Trim();
        DataSet temp = server.GetDataSet("select * frm bill_yskm where yskmCode='" + yskmCode + "'");
        string gjfs = temp.Tables[0].Rows[0]["gjfs"].ToString().Trim();
        if (gjfs == "0" || gjfs == "1" || gjfs == "2")//自报自销 或 指定单位
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('yskmDept.aspx?yskmCode=" + yskmCode + "','detail');", true);
        }
        else//部分对应
        { 
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('yskmDeptList.aspx?yskmCode=" + yskmCode + "','detail');", true);
        }
    }
}
