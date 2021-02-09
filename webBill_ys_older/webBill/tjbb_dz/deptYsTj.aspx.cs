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
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.TextBox1.Text = (System.DateTime.Now.Year + 1).ToString() + "-01-01";

            //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
            this.TextBox2.Text = (System.DateTime.Now.Year + 1).ToString() + System.DateTime.Now.ToString("-MM-dd");
            //1.判断是否预算到末级部门
            string ismj = configdal.GetValueByKey("deptjc");
            string strsql = "";
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            if (!string.IsNullOrEmpty(ismj) && ismj == "Y")//如果是预算到末级
            {
                strsql = @"select * from bill_departments where sjDeptCode!=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')and sjdeptcode!='' and deptcode in (" + deptCodes + ") order by deptcode";
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
            if (deptCodes.IndexOf("000001") > -1)
            {
                this.drpDept.Items.Insert(0, new ListItem("所有单位", ""));
            }
            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                strdjdy = Request["dydj"].ToString();
            }
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
        //if (DateTime.Parse(this.TextBox1.Text.ToString().Trim()).Year.ToString() != DateTime.Parse(this.TextBox2.Text.ToString().Trim()).Year.ToString())
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('起止日期必须是同一年度时间！');", true);
        //    return;
        //}
        string kssj = DateTime.Parse(this.bgintime.SelectedValue.Trim()).ToShortDateString();//DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = DateTime.Parse(this.endtime.SelectedValue.Trim()).ToShortDateString();// DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();

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





        //Response.Write(hf_dept.Value);
        string url = "";
        if (CheckBox1.Checked)//按科目汇总
        {
            url = "deptYsTjResult.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&type=" + type + "&dydj=" + strdjdy;
        }
        else
        {
            url = "fykmYsTjResult.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&ishz=0&dydj=" + strdjdy;
        }

        //Response.Write(url);
        Response.Redirect(url);
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
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drpyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        gettime();
    }
}