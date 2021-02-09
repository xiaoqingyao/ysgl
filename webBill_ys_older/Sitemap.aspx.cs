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

public partial class Sitemap : System.Web.UI.Page
{
    Bll.ConfigBLL bllConfig = new Bll.ConfigBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        //404提示信息    ErrorMsg
        DataTable dt = bllConfig.GetDtByKey("ErrorMsg");
        if (dt.Rows.Count > 0)
        {
            lierrormsg.Text = Convert.ToString(dt.Rows[0]["avalue"]);
        }
    }
}
