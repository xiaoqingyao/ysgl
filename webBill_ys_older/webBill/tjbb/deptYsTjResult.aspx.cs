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
using System.IO;
using Dal.Bills;
using Dal;

public partial class webBill_tjbb_deptYsTjResult : System.Web.UI.Page
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
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }

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
        dept = dept.Replace('|', ',');
        string type = Request.QueryString["type"].ToString();

        strkssj = Request["kssj"].ToString().Trim();
        strjssj = Request["jzsj"].ToString();

        //////判断是不是财年
        //string iscn = configdal.GetValueByKey("CYLX");
        //if (!string.IsNullOrEmpty(iscn) && iscn == "Y")
        //{
        //    string strksny = DateTime.Parse(strkssj).ToString("yyyy-MM");
        //    string strzrnkssj = server.GetCellValue("select beg_time from dbo.bill_Cnpz where year_moth ='" + strksny + "' ");


        //    if (!string.IsNullOrEmpty(strzrnkssj))
        //    {
        //        strkssj = strzrnkssj;
        //    }
        //    else
        //    {
        //        strkssj = "2099-12-01";
        //    }
        //    string strjzny = DateTime.Parse(strjssj).ToString("yyyy-MM");
        //    string strzrnjzsj = server.GetCellValue("select end_time from dbo.bill_Cnpz where year_moth ='" + strjzny + "' ");
        //    if (!string.IsNullOrEmpty(strzrnjzsj))
        //    {
        //        strjssj = strzrnjzsj;
        //    }
        //    else
        //    {
        //        strjssj = "2099-12-31"; ;
        //    }
        //}


        strismj = configdal.GetValueByKey("deptjc");//获取配置项是否预算到末级


        //1.根据配置项 是否是财年  
        //如果是 进行时间转换  把选择的财年时间转成自然时间
        //如果不是 不用转了
        string strsql = " exec pro_bill_bxtj_dept2 '" + strkssj + "','" + Convert.ToDateTime(strjssj).ToString("yyyy-MM-dd") + " 23:59:59" + "','" + dept + "','" + type + "'";//,'" + strdydj + "','" + strismj + "'
        Response.Write(strsql);
       // return;
        //DataSet temp = server.GetDataSet("exec pro_bill_bxtj_dept2 '" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + DateTime.Parse( DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString()).AddDays(1).ToShortDateString() + "','" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        DataSet temp = server.GetDataSet(strsql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (e.Item.Cells[0].Text.ToString().Trim().IndexOf("合计") >= 0)
            { }
            else
            {
                string dept = Request.QueryString["deptCode"].ToString().Trim();
                string yskm = e.Item.Cells[10].Text.ToString().Trim();
                if (dept == "")
                {
                    dept = e.Item.Cells[10].Text.ToString().Trim();
                    yskm = "";
                }
                //报销单
              //  e.Item.Cells[4].Text = "<a href=# onclick=\"openDetail('deptYsTj_Dept.aspx','" + DateTime.Parse(e.Item.Cells[11].Text.ToString().Trim()).ToShortDateString() + "','" + yskm + "','" + strkssj + "','" + strjssj + "','" + dept + "','" + Page.Request.QueryString["type"].ToString().Trim() + "','" + strflowid + "','" + strdydj + "','');\">" + e.Item.Cells[4].Text.ToString().Trim() + "</a>";
               //本期预算

                //e.Item.Cells[3].Text = "<a href=# onclick=\"openDetail('deptys_cx.aspx','" + strkssj + "','" + yskm + "','" + strkssj + "','" + strjssj + "','" + dept + "','" + Page.Request.QueryString["type"].ToString().Trim() + "','" + strflowid + "','" + strdydj + "','ys');\">" + e.Item.Cells[3].Text.ToString().Trim() + "</a>";


            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdydj = Request["dydj"].ToString();
        }
        Response.Redirect("deptYsTj.aspx?dydj=" + strdydj);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {



        Response.ClearContent();
        Response.Buffer = true; //完成整个响应后再发送
        Response.Charset = "utf-8";//设置输出流的字符集-中文

        Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");//设置输出流的字符集
        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
