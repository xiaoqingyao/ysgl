using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_zrn_dz_deptYsTj_zrn : System.Web.UI.Page
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
            //1.判断是否预算到末级部门
            string ismj = configdal.GetValueByKey("deptjc");
            string strsql = "";
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            if (!string.IsNullOrEmpty(ismj) && ismj == "Y")//如果是预算到末级
            {
                strsql = @"select * from bill_departments where sjDeptCode!=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')and sjdeptcode!='' and deptcode in (" + deptCodes + ") order by deptcode";
            }
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
                drpyear.DataTextField = "nian";
                drpyear.DataBind();
            }

            //if (!string.IsNullOrEmpty(drpyear.SelectedValue))
            //{
            //    string ysgcyuesql = " select * from bill_ysgc  where yue!='' and nian='" + drpyear.SelectedValue + "'";

            //    DataTable dtyue = new DataTable();
            //    dtyue = server.GetDataTable(ysgcyuesql, null);
            //    bgintime.DataSource = dtyue;
            //    bgintime.DataTextField = "xmmc";
            //    bgintime.DataValueField = "kssj";
            //    bgintime.DataBind();

            //    ysgcyuesql += " order by gcbh desc";
            //    dtyue = server.GetDataTable(ysgcyuesql, null);
            //    endtime.DataSource = dtyue;
            //    endtime.DataTextField = "xmmc";
            //    endtime.DataValueField = "jzsj";
            //    endtime.DataBind();

            //}
            //else
            //{
            //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先选择财年！');", true);
            //    return;

            //}
        }

    }
    /// <summary>
    /// 生成报表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string nd = drpyear.SelectedValue;
        string ksyf = this.bgintime.SelectedValue.ToString();
        string jzyf = this.endtime.SelectedValue.ToString();
        string dept = "";
        string kssj = "";
        string jzsj = "";

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
        string url = "";


        if (!string.IsNullOrEmpty(ksyf) && !string.IsNullOrEmpty(nd))
        {
            kssj = nd + "-" + ksyf + "-01";
        }


        //能被4整除且又能不能被100整除 是闰年 能直接被400整除也是闰年

        int intnd = int.Parse(nd);
        if ((intnd % 4 == 0 && intnd % 100 != 0) || intnd % 400 == 0)//闰年
        {
            if (jzyf == "02")//闰年2月29
            {
                jzsj = nd + "-" + jzyf + "-29";
            }

        }
        else
        {
            if (jzyf == "02")//闰年2月29
            {
                jzsj = nd + "-" + jzyf + "-28";
            }
        }

        if (jzyf == "01" || jzyf == "03" || jzyf == "05" || jzyf == "07" || jzyf == "08" || jzyf == "10" || jzyf == "12")
        {
            jzsj = nd + "-" + jzyf + "-31";
        }
        if (jzyf == "04" || jzyf == "06" || jzyf == "09" || jzyf == "11")
        {
            jzsj = nd + "-" + jzyf + "-30";
        }
       
        if (CheckBox1.Checked)//按科目汇总
        {
            url = "deptYsTjResult_zrn.aspx?nd=" + nd + "&ksyf=" + ksyf + "&jzyf=" + jzyf + "&deptCode=" + dept + "&type=" + type + "&dydj=" + strdjdy + "&kssj=" + kssj + "&jzsj=" + jzsj;
        }
        else
        {
            url = "fykmYsTjResult_zrn.aspx?nd=" + nd + "&ksyf=" + ksyf + "&jzyf=" + jzyf + "&deptCode=" + dept + "&ishz=0&dydj=" + strdjdy + "&kssj=" + kssj + "&jzsj=" + jzsj;
        }

        //Response.Write(url);
        Response.Redirect(url);

    }
}