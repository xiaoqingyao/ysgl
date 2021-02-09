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
using Dal;

public partial class webBill_tjbb_deptYsTj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    string strdjdy = "02";
    ConfigDal configdal = new ConfigDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";

            //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
            this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            //1.判断是否预算到末级
            string ismj = configdal.GetValueByKey("deptjc");
            string strsql = "";
            if (!string.IsNullOrEmpty(ismj) && ismj == "Y")//如果是预算到末级
            {
                strsql = @"select * from bill_departments where sjDeptCode!=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')and sjdeptcode!=''";
            }
            else
            {
                strsql = "select * from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')";
            }
            //
            DataSet tmep = server.GetDataSet(strsql);
            for (int i = 0; i <= tmep.Tables[0].Rows.Count - 1; i++)
            {
                ListItem li = new ListItem();
                li.Text = "[" + tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + tmep.Tables[0].Rows[i]["deptName"].ToString().Trim();
                li.Value = tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim();
                this.drpDept.Items.Add(li);
            }
            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                strdjdy = Request["dydj"].ToString();
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() != DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString())
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
        //    return;
        //}
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToString("yyyy-MM-dd");
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToString("yyyy-MM-dd");

        string dept = "";

        string[] temp = hf_dept.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        }
        string type = DropDownList2.SelectedValue;
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdjdy = Request["dydj"].ToString();
        }


        //让客户选择财年时间 系统根据财年时间转换成自然年  
        //select beg_time from dbo.bill_Cnpz where year_moth ='2016-01' 
        // select end_time from dbo.bill_Cnpz where  year_moth ='2016-03'

      
       

        Response.Redirect("deptYsTjResult.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&type=" + type + "&dydj=" + strdjdy);
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string strselect = getSelectStr();
        if (strselect.Equals(""))
        {
            return;
        }
        Response.Redirect("deptYsTjResult_dy.aspx" + strselect);
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
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();

        string dept = "";

        string[] temp = hf_dept.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        }
        string type = DropDownList2.SelectedValue;
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdjdy = Request["dydj"].ToString();
        }
        return "?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&djzt=" + type + "&dydj=" + strdjdy;
    }
}