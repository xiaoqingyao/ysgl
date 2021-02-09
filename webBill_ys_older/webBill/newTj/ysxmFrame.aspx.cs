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

public partial class webBill_newTj_ysxmFrame : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";

            //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
            this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            DataSet tmep = server.GetDataSet("select * from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");
            for (int i = 0; i <= tmep.Tables[0].Rows.Count - 1; i++)
            {
                ListItem li = new ListItem();
                li.Text = "[" + tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + tmep.Tables[0].Rows[i]["deptName"].ToString().Trim();
                li.Value = tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim();
                this.drpDept.Items.Add(li);
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() !=  DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString() )
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
            return;
        }
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();

        string dept = "";
        if (this.drpDept.SelectedIndex == 0)
        {
            dept = "";
        }
        else
        {
            dept = this.drpDept.SelectedItem.Value;
        }

        Response.Redirect("ysxmResult.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept);
    }
}