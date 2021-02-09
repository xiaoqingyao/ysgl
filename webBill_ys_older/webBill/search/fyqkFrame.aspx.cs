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

public partial class webBill_search_fyqkList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();
    string strdydj = "02";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

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
                this.DropDownList1.Items.Add(li);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //string strselect = getSelectStr();
        //if (strselect.Equals(""))
        //{
        //    return;
        //}
        //Response.Redirect("fyqkListTable_new.aspx" + strselect);

        if (string.IsNullOrEmpty(this.TextBox1.Text.ToString().Trim())|| string.IsNullOrEmpty(this.TextBox2.Text.ToString().Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开始时间或截止时间不能为空！');", true);
            return;
        }

        if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() != DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString())
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
            return;
        }
        string kssj = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToString("yyyy-MM-dd");
        string jzsj = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToString("yyyy-MM-dd"); 

        string dept = "";
        /*
        if (this.DropDownList1.SelectedIndex == 0)
        {
            dept = "";
        }
        else
        {
            dept = this.DropDownList1.SelectedItem.Value;
        }
         */
        string[] temp = hf_dept.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        }
        string type = DropDownList2.SelectedValue;
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdydj = Request["dydj"].ToString();
        }

        //让客户选择财年时间 系统根据财年时间转换成自然年  
        //select beg_time from dbo.bill_Cnpz where year_moth ='2016-01' 
        // select end_time from dbo.bill_Cnpz where  year_moth ='2016-03' fyqkListTable_new




        Response.Redirect("fyqkListTable_new.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&type=" + type + "&dydj=" + strdydj);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string strselect = getSelectStr();
        if (strselect.Equals(""))
        {
            return;
        }
        Response.Redirect("fyqkListTable_new_dy.aspx" + strselect);
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
        /*
        if (this.DropDownList1.SelectedIndex == 0)
        {
            dept = "";
        }
        else
        {
            dept = this.DropDownList1.SelectedItem.Value;
        }
         */


        //让客户选择财年时间 系统根据财年时间转换成自然年  
        //select beg_time from dbo.bill_Cnpz where year_moth ='2016-01' 
        // select end_time from dbo.bill_Cnpz where  year_moth ='2016-03'
        //判断是不是财年
        string iscn = configdal.GetValueByKey("CYLX");
        if (!string.IsNullOrEmpty(iscn) && iscn == "Y")
        {
            string strksny = DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToString("yyyy-MM");
            string strzrnkssj = server.GetCellValue("select beg_time from dbo.bill_Cnpz where year_moth ='" + strksny + "' ");


            if (!string.IsNullOrEmpty(strzrnkssj))
            {
                kssj = strzrnkssj;
            }
            string strjzny = DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToString("yyyy-MM");
            string strzrnjzsj = server.GetCellValue("select end_time from dbo.bill_Cnpz where year_moth ='" + strjzny + "' ");
            if (!string.IsNullOrEmpty(strzrnjzsj))
            {
                jzsj = strzrnjzsj;
            }
        }
        string[] temp = hf_dept.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        }
        string type = DropDownList2.SelectedValue;
        return "?kssj=" + kssj + "&jzsj=" + jzsj + "&deptcode=" + dept + "&type=" + type;
    }
}