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
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.TextBox1.Text = (System.DateTime.Now.Year + 1).ToString() + "-01-01";
            this.TextBox2.Text = (System.DateTime.Now.Year + 1).ToString() + System.DateTime.Now.ToString("-MM-dd");
            //1.判断是否预算到末级
            string ismj = configdal.GetValueByKey("deptjc");
            string strsql = "";
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            if (!string.IsNullOrEmpty(ismj) && ismj == "Y")//如果是预算到末级
            {
                strsql = @"select * from bill_departments where sjDeptCode!=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')and sjdeptcode!=''  and deptcode in (" + deptCodes + ")  order by deptcode";
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
            if (deptCodes.IndexOf("000001") > -1)
            {
                this.DropDownList1.Items.Insert(0, new ListItem("所有单位", ""));
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

    public void gettime() {

        if (!string.IsNullOrEmpty(drpyear.SelectedValue))
        {
            string ysgcyuesql = @"select CONVERT(varchar(10), kssj, 121) as kssj,CONVERT(varchar(10), jzsj, 121) as jzsj,a.xmmc from bill_ysgc  a  where yue!='' and nian='" + drpyear.SelectedValue + "'";

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
        //string strselect = getSelectStr();
        //if (strselect.Equals(""))
        //{
        //    return;
        //}
        //Response.Redirect("fyqkListTable_new.aspx" + strselect);

        if (string.IsNullOrEmpty(this.bgintime.SelectedValue.Trim()) || string.IsNullOrEmpty(this.endtime.SelectedValue.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开始月份或截止月份不能为空！');", true);
            return;
        }

        string kssj = this.bgintime.SelectedValue.Trim();//DateTime.Parse(this.TextBox1.Text.ToString().Trim()).ToShortDateString();
        string jzsj = this.endtime.SelectedValue.Trim();// DateTime.Parse(this.TextBox2.Text.ToString().Trim()).ToShortDateString();

        string dept = "";

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
        Response.Redirect("fyqkListTable_new.aspx?kssj=" + kssj + "&jzsj=" + jzsj + "&deptCode=" + dept + "&type=" + type + "&dydj=" + strdydj);
    }

    protected void drpyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        gettime();
    }
}