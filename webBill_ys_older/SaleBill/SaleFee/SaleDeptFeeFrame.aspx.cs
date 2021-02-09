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

public partial class SaleBill_SaleFee_SaleDeptFeeFrame : System.Web.UI.Page
{
    string strdatefrom = "";
    string strdateto = "";
    string strdeptcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["datefrom"] != null)
            {
                strdatefrom = Request["datefrom"].ToString();
            }
            if (Request["dateto"] != null)
            {
                strdateto = Request["dateto"].ToString();
            }
            if (Request["deptcode"] != null)
            {
                strdeptcode = Request["deptcode"].ToString();
            }
            this.left.Attributes.Add("src", "SaleDeptFeeLeft.aspx?deptcode=" + strdeptcode + "&datefrom=" + strdatefrom + "&dateto=" + strdateto);
            this.list.Attributes.Add("src", "SaleDeptFeelist.aspx?deptcode=" + strdeptcode + "&datefrom=" + strdatefrom + "&dateto=" + strdateto);
        }
       
    }
}
