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

public partial class webBill_tjbb_YSDatesDeptTongji : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";
            this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
           BindDept();
        }
    }
    private void BindDept()
    {
        DataSet temp = server.GetDataSet("select deptCode,('['+deptCode+']'+deptName) as deptName  from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");
        drpDept.DataSource = temp;
        drpDept.DataTextField = "deptName";
        drpDept.DataValueField = "deptCode";
        drpDept.DataBind();
        drpDept.Items.Insert(0, new ListItem("所有单位", ""));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() != DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
            return;
        }
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();

        string dept = drpDept.SelectedValue;



        Response.Redirect("YSDatesDeptTongjiResult.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept);
    }
}