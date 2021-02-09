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

public partial class webBill_bxgl_fykm_sm_show : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        divshow.InnerHtml = getTable();
    }
    private string getTable()
    {
        string strend = "<table class='baseTable' width='99%' style='margin-left:5px;'>{0}</table>";
        string streve = "";
        DataTable dt = server.GetDataTable("select * from bill_datadic where dictype='19'", null);
        if (dt == null || dt.Rows.Count <= 0)
        {
            streve = "<tr><td>您尚未编辑内容。</td></tr>";
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                streve += "<tr>";
                streve += "<td>费用科目：" + dr["dicCode"] + "</br>";
                streve += "报销说明：" + dr["dicName"] + "</td>";
                streve += "</tr>";
            }
        }
        strend = string.Format(strend, streve);
        return strend;
    }
}
