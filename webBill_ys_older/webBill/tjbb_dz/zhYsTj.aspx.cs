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

public partial class webBill_tjbb_dz_zhYsTj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    string strdjdy = "02";
    ConfigDal configdal = new ConfigDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.TextBox1.Text = (System.DateTime.Now.Year + 1).ToString() + "-01-01";

            //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
            //this.TextBox2.Text = (System.DateTime.Now.Year + 1).ToString() + System.DateTime.Now.ToString("-MM-dd");
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
    /// <summary>
    /// 
    /// </summary>
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

        string dept = "";

        string temp = hf_dept.Value;
        temp = temp.Substring(0, temp.Length - 1);
        if (!string.IsNullOrEmpty(temp))
        {
            dept = temp;
        }
        string tempkm = hf_yskm.Value;// DropDownList2.SelectedValue;
        string yskm = "";
        tempkm = tempkm.Substring(0, tempkm.Length - 1);
        if (!string.IsNullOrEmpty(tempkm))
        {
            yskm = tempkm;
        }
         //////判断是不是财年
         //   string iscn = configdal.GetValueByKey("CYLX");
         //   if (!string.IsNullOrEmpty(iscn) && iscn == "Y")
         //   {
         //       string strksny = DateTime.Parse(kssj).ToString("yyyy-MM");
         //       string strzrnkssj = server.GetCellValue("select beg_time from dbo.bill_Cnpz where year_moth ='" + strksny + "' ");


         //       if (!string.IsNullOrEmpty(strzrnkssj))
         //       {
         //           kssj = strzrnkssj;
         //       }
         //       else
         //       {
         //           kssj = "2099-12-01";
         //       }
         //       string strjzny = DateTime.Parse(jzsj).ToString("yyyy-MM");
         //       string strzrnjzsj = server.GetCellValue("select end_time from dbo.bill_Cnpz where year_moth ='" + strjzny + "' ");
         //       if (!string.IsNullOrEmpty(strzrnjzsj))
         //       {
         //           jzsj = strzrnjzsj;
         //       }
         //       else
         //       {
         //           jzsj = "2099-12-31"; ;
         //       }
         //   }
            string exesql = "delete bill_tcsql  ;";
            exesql += " insert bill_tcsql(kssj,jzsj,deptCode,yskm)values('"+kssj+"','"+jzsj+"','"+dept+"','"+yskm+"')";
            server.ExecuteNonQuery(exesql);
        Response.Redirect("touru_chanchu.aspx");
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
        string dept = lbl_dept.Text;
        //dept = dept.Substring(0,dept.Length-1);
        //string[] temp = hf_dept.Value.Split(';');
        string type = "";// DropDownList2.SelectedValue;
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdjdy = Request["dydj"].ToString();
        }
        return "?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&djzt=" + type + "&dydj=" + strdjdy;
    }
    protected void drpyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        gettime();
    }
}