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

public partial class webBill_select_xmFrame : System.Web.UI.Page
{
    public string deptCode;
    protected void Page_Load(object sender, EventArgs e)
    {
        deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
    }
}
