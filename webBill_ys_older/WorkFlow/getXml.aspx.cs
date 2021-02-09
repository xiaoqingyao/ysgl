using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WorkFlow_getXml : System.Web.UI.Page
{
    public string strJson;
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        strJson = (new workFlowLibrary.getWkXml()).getXml(Page.Request.QueryString["flowID"].ToString().Trim());
    }
}
