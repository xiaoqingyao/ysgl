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

public partial class webBill_ysgl_yszjkmtj : System.Web.UI.Page
{
    string strdeptcode = "";
    string strkmcode = "";

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            if (Request["deptcode"] != null && Request["deptcode"].ToString() != "")
            {
                strdeptcode = Request["deptcode"].ToString();
            }
            if (Request["kmcode"] != null && Request["kmcode"].ToString() != "")
            {
                strkmcode = Request["kmcode"].ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();
            }
        }
    }

    void bindData()
    {

        string deptGuid = @"select a.*,
(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.ysDept ) as deptName,
(select '['+yskmCode+']'+ ysKmMc from bill_yskm where yskmCode=a.yskm ) as yskmName
from bill_ysmxb a where ysType='2' and ysDept='" + strdeptcode + "' and yskm='" + strkmcode + "'";

        DataTable temp = server.RunQueryCmdToTable(deptGuid);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }


}

