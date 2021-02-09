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
using Bll;

public partial class webBill_tjbb_fykmYsTj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigBLL config = new ConfigBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";

            //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
            this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            //1.判断是否预算到末级
            string ismj = config.GetValueByKey("deptjc");
            DataSet tmep = new DataSet();
            if (!string.IsNullOrEmpty(ismj) && ismj == "Y")//如果预算到末级
            {
                tmep = server.GetDataSet("select * from bill_departments where sjDeptCode!=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') and sjdeptcode!=''");

            }
            else
            {
                tmep = server.GetDataSet("select * from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");

            }
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
        string strselect = getSelectStr();
        if (strselect.Equals(""))
        {
            return;
        }
        Response.Redirect("fykmYsTjResult.aspx" + strselect);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string strselect = getSelectStr();
        if (strselect.Equals(""))
        {
            return;
        }
        Response.Redirect("fykmYsTjResult_dy.aspx" + strselect);
    }
    /// <summary>
    /// 获取传参的字符串
    /// </summary>
    /// <returns></returns>
    private string getSelectStr()
    {
        if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() != DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
            return "";
        }
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToString("yyyy-MM-dd");
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToString("yyyy-MM-dd");

        string dept = "";
        if (this.drpDept.SelectedIndex == 0)
        {
            dept = "";
        }
        else
        {
            dept = this.drpDept.SelectedItem.Value;
        }
        string isKmhz = "";//是否科目汇总
        if (ckbIsKmhz.Checked == true)
        {
            isKmhz = "1";
        }
        else
        {
            isKmhz = "0";
        }
        return "?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&ishz=" + isKmhz;
    }
}