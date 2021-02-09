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

public partial class WorkFlow_updateStepMode : System.Web.UI.Page
{
    public string strJson;
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "update bill_workflowgroup set flowmode='" + Page.Request.QueryString["stepMode"] + "' where flowID='" + Page.Request.QueryString["flowID"] + "' and stepID='" + Page.Request.QueryString["stepID"] + "'";
        if (server.ExecuteNonQuery(sql) == -1)
        {
            strJson = "false";
        }
        else {
            strJson= "true";
        }
    }
}