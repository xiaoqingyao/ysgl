using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_zcfzb_select : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();
    string strdydj = "02";
    public string title = "";
    private string flg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        object objFlg = Request["flg"];
        if (objFlg != null)
        {
            flg = objFlg.ToString();
        }
        switch (flg)
        {
            case "zcfzb": title = "资产负债表"; break;
            case "xjllb": title = "现金流量表"; break;
            case "lrb": title = "利润表"; break;
            default:
                break;
        }
        if (!IsPostBack)
        {
            string ysgcsql = "select * from bill_ysgc where yue='' order by nian desc";
            DataTable dtysgc = server.GetDataTable(ysgcsql, null);
            if (dtysgc != null)
            {
                drpyear.DataSource = dtysgc;
                drpyear.DataValueField = "nian";
                drpyear.DataTextField = "xmmc";
                drpyear.DataBind();
            }

            gettime();
        }
    }


    public void gettime()
    {
        if (!string.IsNullOrEmpty(drpyear.SelectedValue))
        {
            string ysgcyuesql = " select * from bill_ysgc  where yue!='' and nian='" + drpyear.SelectedValue + "'";

            DataTable dtyue = new DataTable();
            dtyue = server.GetDataTable(ysgcyuesql, null);
            bgintime.DataSource = dtyue;
            bgintime.DataTextField = "xmmc";
            bgintime.DataValueField = "kssj";
            bgintime.DataBind();
            ysgcyuesql += " order by gcbh desc";
            dtyue = server.GetDataTable(ysgcyuesql, null);
            endtime.DataSource = dtyue;
            endtime.DataTextField = "xmmc";
            endtime.DataValueField = "jzsj";
            endtime.DataBind();

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先选择财年！');", true);
            return;

        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.bgintime.SelectedValue.Trim()) || string.IsNullOrEmpty(this.endtime.SelectedValue.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开始月份或截止月份不能为空！');", true);
            return;
        }

        string kssj = DateTime.Parse(this.bgintime.SelectedValue.Trim()).ToShortDateString();//DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = DateTime.Parse(this.endtime.SelectedValue.Trim()).ToShortDateString();// DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();
        string url = "";
        switch (flg)
        {
            case "zcfzb": url = "zcfzb_result"; break;
            case "xjllb": url = "xjllb_result"; break;
            case "lrb": url = "lrb_result"; break;
            default:
                break;
        }
        string kssjtext = this.bgintime.SelectedItem.Text.Trim();
        string jzsjtext = this.endtime.SelectedItem.Text.Trim();
        string showdt = kssjtext + "-" + jzsjtext;
        Response.Redirect(url + ".aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&showdt=" + showdt);
    }

    protected void drpyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        gettime();
    }
}