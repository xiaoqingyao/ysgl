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

public partial class SaleBill_SaleFee_Saleselect : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            //strDeptCodes = strDeptCodes.Substring(1, strDeptCodes.Length-1);
            //strDeptCodes.Replace("','", ",");
            //string[] arrStrRel = strDeptCodes.Split(new string[] { "" }, StringSplitOptions.None);
            //int arrCount = arrStrRel.Length;
            if (strDeptCodes!="")
            {
                DataTable dtRel = server.GetDataTable("select deptCode,deptName from bill_departments where IsSell='Y' and sjdeptCode='000001' and deptCode in (" + strDeptCodes + ")", null);
                for (int i = 0; i < dtRel.Rows.Count ; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = "[" + dtRel.Rows[i]["deptCode"].ToString().Trim() + "]" + dtRel.Rows[i]["deptName"].ToString().Trim();
                    li.Value = dtRel.Rows[i]["deptCode"].ToString().Trim();
                    this.drpDept.Items.Add(li);
                }
            }
            DateTime dt = DateTime.Now;
            this.txtDateFrm.Text = dt.Date.Year.ToString() + "-" + dt.Date.Month.ToString("00") + "-" + "01";
            this.txtDateTo.Text = dt.ToString("yyyy-MM-dd");
        }
        this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
        this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string strdatefrom = this.txtDateFrm.Text;
        string strdateto = this.txtDateTo.Text;

        string dept = "";
        string[] temp = hf_dept.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        }
        else {
            int iCount = this.drpDept.Items.Count;
            for (int i = 0; i < iCount; i++)
            {
                dept += drpDept.Items[i].Value;
                dept += "|";
            }
            dept.Substring(0, dept.Length - 1);
        }
        dept=dept.Replace("|", "','");
        dept = "'" + dept + "'";
        Response.Redirect("SaleFeeList.aspx?datefrom=" + strdatefrom + "&dateto=" + strdateto + "&deptCode=" + dept);
    }
}
