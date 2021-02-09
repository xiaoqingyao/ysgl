using Dal;
using Dal.Bills;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_zrn_dz_deptYsTjResult_zrn : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();
    string strflowid = "ybbx";//相应的flowid 默认一般报销
    string strdydj = "02";//对应单据 默认02 费用
    string strismj = "";//是否预算到末级
    string strkssj = "";
    string strjssj = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(Request["dydj"]))//对应单据
        {
            strdydj = Request["dydj"].ToString();
            MainDal maindal = new MainDal();
            strflowid = maindal.getJSFlowId(strdydj);

        }

        this.TextBox1.Focus();
        if (!IsPostBack)
        {
            string dept = Page.Request.QueryString["deptCode"].ToString().Trim();

            string strshowname = "单位名称:";
            DataTable deptdt = new DataTable();

            if (dept == "")
            {
                strshowname += "所有单位,";
            }
            else
            {

                dept = dept.Replace('|', ',');
                string[] array = dept.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(array[i]))
                    {
                        strshowname += server.GetCellValue("select '['+deptcode+']'+deptname as showname from bill_departments where deptcode ='" + array[i] + "'") + ",";
                    }
                }
            }

            if (!string.IsNullOrEmpty(strshowname))
            {
                strshowname = strshowname.Substring(0, strshowname.Length - 1);
            }
            this.Label1.Text = "开始时间：" + Page.Request.QueryString["kssj"].ToString().Trim() + "  截止时间：" + Page.Request.QueryString["jzsj"].ToString().Trim() + "  " + strshowname;



            this.bindData();
        }
    }
    public void bindData()
    {
        string dept = Request.QueryString["deptCode"].ToString();

        string nd = Request["nd"].ToString();

        dept = dept.Replace('|', ',');
        string type = Request.QueryString["type"].ToString();

        string ksyf = Page.Request.QueryString["ksyf"].ToString().Trim();
        string jzyf = Page.Request.QueryString["jzyf"].ToString().Trim();
        strkssj = Page.Request.QueryString["kssj"].ToString().Trim();
        strjssj = Page.Request.QueryString["jzsj"].ToString();

        strismj = configdal.GetValueByKey("deptjc");//获取配置项是否预算到末级


        string strsql = "exec [pro_bill_bxtj_dept_zrn] '" + nd + "', '" + ksyf + "','" + jzyf + "','" + dept + "','" + type + "','" + strdydj + "','" + strismj + "','" + strkssj + "','" + strjssj + "'";
      //  Response.Write(strsql);
     //   return;
        //DataSet temp = server.GetDataSet("exec pro_bill_bxtj_dept2 '" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + DateTime.Parse( DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString()).AddDays(1).ToShortDateString() + "','" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        DataSet temp = server.GetDataSet(strsql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("deptYsTj_zrn.aspx");
    }
}