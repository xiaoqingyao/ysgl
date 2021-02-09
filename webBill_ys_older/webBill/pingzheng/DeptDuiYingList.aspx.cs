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

public partial class webBill_pingzheng_DeptDuiYingRight : System.Web.UI.Page
{
    string strDeptName = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objDeptName = Request["deptName"];
            if (objDeptName!=null)
            {
                strDeptName = objDeptName.ToString();
                hdCurrentDeptName.Value = strDeptName;
            }
            if (!IsPostBack)
            {
                bindDate();
            }
        }
    }
    private void bindDate() {
        string strsql = "";
        if (string.IsNullOrEmpty(strDeptName))
        {
            strsql = "select * from bill_pingzheng_bumenduiying";
        }
        else {
            strsql = "select * from bill_pingzheng_bumenduiying where Note1='"+strDeptName+"'";
        }
        DataTable dt = server.GetDataTable(strsql, null);
        this.gridView.DataSource = dt;
        this.gridView.DataBind();
    }
}
