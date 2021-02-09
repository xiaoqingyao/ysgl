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

public partial class WorkFlow_stepLook : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.showData();
        }
    }

    void showData()
    {
        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();
        string flowid = Page.Request.QueryString["billType"].ToString().Trim();

        string sql = "select (select username from bill_users where usercode=checkuser) as checkuser,checkdate,checkbz,(case result when 'true' then '审核通过' when 'false' then '审核退回' end) as result from bill_workflowrecord";
        sql += " where flowid='" + flowid + "' and billCode='" + billCode + "' order by checkdate";

        this.myGrid.DataSource = (new sqlHelper.sqlHelper()).GetDataSet(sql);
        this.myGrid.DataBind();

    }
}
