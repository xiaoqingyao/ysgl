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

public partial class webBill_xtsz_parFrame : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.bindData();
        }
    }

    public void bindData()
    {
        this.TextBox1.Text = server.GetCellValue("select parVal from bill_syspar where parname='年度预算开始时间'");
        this.TextBox2.Text = server.GetCellValue("select parVal from bill_syspar where parname='年度预算截止时间'");


        this.TextBox3.Text = server.GetCellValue("select parVal from bill_syspar where parname='第1季度预算开始时间'");
        this.TextBox4.Text = server.GetCellValue("select parVal from bill_syspar where parname='第1季度预算截止时间'");
        this.TextBox5.Text = server.GetCellValue("select parVal from bill_syspar where parname='第2季度预算开始时间'");
        this.TextBox6.Text = server.GetCellValue("select parVal from bill_syspar where parname='第2季度预算截止时间'");
        this.TextBox7.Text = server.GetCellValue("select parVal from bill_syspar where parname='第3季度预算开始时间'");
        this.TextBox8.Text = server.GetCellValue("select parVal from bill_syspar where parname='第3季度预算截止时间'");
        this.TextBox9.Text = server.GetCellValue("select parVal from bill_syspar where parname='第4季度预算开始时间'");
        this.TextBox10.Text = server.GetCellValue("select parVal from bill_syspar where parname='第4季度预算截止时间'");


        this.TextBox11.Text = server.GetCellValue("select parVal from bill_syspar where parname='1月份预算开始时间'");
        this.TextBox12.Text = server.GetCellValue("select parVal from bill_syspar where parname='1月份预算截止时间'");
        this.TextBox13.Text = server.GetCellValue("select parVal from bill_syspar where parname='2月份预算开始时间'");
        this.TextBox14.Text = server.GetCellValue("select parVal from bill_syspar where parname='2月份预算截止时间'");
        this.TextBox15.Text = server.GetCellValue("select parVal from bill_syspar where parname='3月份预算开始时间'");
        this.TextBox16.Text = server.GetCellValue("select parVal from bill_syspar where parname='3月份预算截止时间'");
        this.TextBox17.Text = server.GetCellValue("select parVal from bill_syspar where parname='4月份预算开始时间'");
        this.TextBox18.Text = server.GetCellValue("select parVal from bill_syspar where parname='4月份预算截止时间'");
        this.TextBox19.Text = server.GetCellValue("select parVal from bill_syspar where parname='5月份预算开始时间'");
        this.TextBox20.Text = server.GetCellValue("select parVal from bill_syspar where parname='5月份预算截止时间'");
        this.TextBox21.Text = server.GetCellValue("select parVal from bill_syspar where parname='6月份预算开始时间'");
        this.TextBox22.Text = server.GetCellValue("select parVal from bill_syspar where parname='6月份预算截止时间'");
        this.TextBox23.Text = server.GetCellValue("select parVal from bill_syspar where parname='7月份预算开始时间'");
        this.TextBox24.Text = server.GetCellValue("select parVal from bill_syspar where parname='7月份预算截止时间'");
        this.TextBox25.Text = server.GetCellValue("select parVal from bill_syspar where parname='8月份预算开始时间'");
        this.TextBox26.Text = server.GetCellValue("select parVal from bill_syspar where parname='8月份预算截止时间'");
        this.TextBox27.Text = server.GetCellValue("select parVal from bill_syspar where parname='9月份预算开始时间'");
        this.TextBox28.Text = server.GetCellValue("select parVal from bill_syspar where parname='9月份预算截止时间'");
        this.TextBox29.Text = server.GetCellValue("select parVal from bill_syspar where parname='10月份预算开始时间'");
        this.TextBox30.Text = server.GetCellValue("select parVal from bill_syspar where parname='10月份预算截止时间'");
        this.TextBox31.Text = server.GetCellValue("select parVal from bill_syspar where parname='11月份预算开始时间'");
        this.TextBox32.Text = server.GetCellValue("select parVal from bill_syspar where parname='11月份预算截止时间'");
        this.TextBox33.Text = server.GetCellValue("select parVal from bill_syspar where parname='12月份预算开始时间'");
        this.TextBox34.Text = server.GetCellValue("select parVal from bill_syspar where parname='12月份预算截止时间'");


        this.TextBox35.Text = server.GetCellValue("select parVal from bill_syspar where parname='技术支持'");
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

        list.Add("update bill_syspar set parVal='" + this.TextBox1.Text.ToString().Trim() + "' where parname='年度预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox2.Text.ToString().Trim() + "' where parname='年度预算截止时间'");


        list.Add("update bill_syspar set parVal='" + this.TextBox3.Text.ToString().Trim() + "' where parname='第1季度预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox4.Text.ToString().Trim() + "' where parname='第1季度预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox5.Text.ToString().Trim() + "' where parname='第2季度预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox6.Text.ToString().Trim() + "' where parname='第2季度预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox7.Text.ToString().Trim() + "' where parname='第3季度预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox8.Text.ToString().Trim() + "' where parname='第3季度预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox9.Text.ToString().Trim() + "' where parname='第4季度预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox10.Text.ToString().Trim() + "' where parname='第4季度预算截止时间'");


        list.Add("update bill_syspar set parVal='" + this.TextBox11.Text.ToString().Trim() + "' where parname='1月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox12.Text.ToString().Trim() + "' where parname='1月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox13.Text.ToString().Trim() + "' where parname='2月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox14.Text.ToString().Trim() + "' where parname='2月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox15.Text.ToString().Trim() + "' where parname='3月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox16.Text.ToString().Trim() + "' where parname='3月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox17.Text.ToString().Trim() + "' where parname='4月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox18.Text.ToString().Trim() + "' where parname='4月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox19.Text.ToString().Trim() + "' where parname='5月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox20.Text.ToString().Trim() + "' where parname='5月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox21.Text.ToString().Trim() + "' where parname='6月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox22.Text.ToString().Trim() + "' where parname='6月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox23.Text.ToString().Trim() + "' where parname='7月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox24.Text.ToString().Trim() + "' where parname='7月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox25.Text.ToString().Trim() + "' where parname='8月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox26.Text.ToString().Trim() + "' where parname='8月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox27.Text.ToString().Trim() + "' where parname='9月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox28.Text.ToString().Trim() + "' where parname='9月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox29.Text.ToString().Trim() + "' where parname='10月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox30.Text.ToString().Trim() + "' where parname='10月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox31.Text.ToString().Trim() + "' where parname='11月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox32.Text.ToString().Trim() + "' where parname='11月份预算截止时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox33.Text.ToString().Trim() + "' where parname='12月份预算开始时间'");
        list.Add("update bill_syspar set parVal='" + this.TextBox34.Text.ToString().Trim() + "' where parname='12月份预算截止时间'");

      string strjs= System.Text.RegularExpressions.Regex.Replace(this.TextBox35.Text.ToString().Trim(), @"<[^>]*>", "");
      list.Add("update bill_syspar set parVal='" + strjs + "' where parname='技术支持'");
     

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
    }
}
