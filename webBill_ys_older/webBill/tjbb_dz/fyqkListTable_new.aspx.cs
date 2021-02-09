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
using System.Data.SqlClient;
using Dal.Bills;
using Dal;
using System.Drawing;

public partial class webBill_tjbb_dz_fyqkListTable_new : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();
    string strflowid = "ybbx";//相应的flowid 默认一般报销
    string strdydj = "02";//对应单据 默认02 费用
    string strismj = "";//是否预算到末级
    string kssj = "";
    string jzsj = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }


        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["dydj"]))//对应单据
            {
                strdydj = Request["dydj"].ToString();
                MainDal maindal = new MainDal();
                strflowid = maindal.getJSFlowId(strdydj);

            }

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
        kssj = Request.QueryString["kssj"].ToString();
        jzsj = Request.QueryString["jzsj"].ToString();
        string dept = Request.QueryString["deptCode"].ToString();
        string type = Request.QueryString["type"].ToString();
        strismj = configdal.GetValueByKey("deptjc");//获取配置项是否预算到末级
        dept = dept.Replace('|', ',');
        if (string.IsNullOrEmpty(dept))
        {
            dept = "";
        }
        //SqlParameter[] sps = { 
        //                         new SqlParameter("@kssj",Convert.ToDateTime(kssj)),
        //                         new SqlParameter("@jzsj",Convert.ToDateTime(jzsj)),
        //                         new SqlParameter("@deptcode",dept),
        //                         new SqlParameter("@djzt",type),
        //                         new SqlParameter("@dydj",strdydj),
        //                         new SqlParameter("@ismj",strismj)
        //                     };


        ////判断是不是财年
        //string iscn = configdal.GetValueByKey("CYLX");
        //if (!string.IsNullOrEmpty(iscn) && iscn == "Y")
        //{
        //    string strksny = DateTime.Parse(kssj).ToString("yyyy-MM");
        //    string strzrnkssj = server.GetCellValue("select beg_time from dbo.bill_Cnpz where year_moth ='" + strksny + "' ");


        //    if (!string.IsNullOrEmpty(strzrnkssj))
        //    {
        //        kssj = strzrnkssj;
        //    }
        //    string strjzny = DateTime.Parse(jzsj).ToString("yyyy-MM");
        //    string strzrnjzsj = server.GetCellValue("select end_time from dbo.bill_Cnpz where year_moth ='" + strjzny + "' ");
        //    if (!string.IsNullOrEmpty(strzrnjzsj))
        //    {
        //        jzsj = strzrnjzsj;
        //    }
        //}

        string strsql=@"exec bill_pro_fyqkb_new_dz '" + kssj + "','" + jzsj + "','" + dept + "','" + type + "','" + strdydj + "','" + strismj + "'";

        Response.Write(strsql);
       // return;
        DataSet ds = server.GetDataSet(strsql);
       
        ////string strsql = @"exec bill_pro_fyqkb_new @kssj,@jzsj,@deptcode,@djzt,@dydj,@ismj";
        //DataSet ds = server.GetDataSet(strsql, sps);
        this.myGrid.DataSource = ds;
        this.myGrid.DataBind();

    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        Response.Redirect("fyqkFrame.aspx?dydj=" + strdydj);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.HeaderEncoding = System.Text.Encoding.UTF8;

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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {


        string yskmname = e.Item.Cells[1].Text.Trim();
        if (yskmname.IndexOf("合计")!=-1)
        {
            e.Item.CssClass = "heji";
        }
        //string dept = Request.QueryString["deptCode"].ToString().Trim();
        //string stryskm = "";
        //if (!string.IsNullOrEmpty(dept))
        //{
        //    stryskm = e.Item.Cells[0].Text.Trim();

        //}
        //else
        //{
        //    dept = e.Item.Cells[0].Text.Trim();
        //}
        ////预算
        //e.Item.Cells[2].Text = "<a href=# onclick=\"openDetail('Zxqktz.aspx','" + kssj + "','" + stryskm + "','" + kssj + "','" + jzsj + "','" + dept + "','" + strflowid + "','" + strdydj + "','ys');\">" + e.Item.Cells[2].Text.ToString().Trim() + "</a>";
        ////报销
        //e.Item.Cells[3].Text = "<a href=# onclick=\"openDetail('../tjbb/fykmYsTj_Yskm.aspx','" + kssj + "','" + stryskm + "','" + kssj + "','" + jzsj + "','" + dept + "','" + strflowid + "','" + strdydj + "','bx');\">" + e.Item.Cells[3].Text.ToString().Trim() + "</a>";

        ////追加
        //e.Item.Cells[4].Text = "<a href=# onclick=\"openDetail('ZxqkZj.aspx','" + kssj + "','" + stryskm + "','" + kssj + "','" + jzsj + "','" + dept + "','" + strflowid + "','" + strdydj + "','zj');\">" + e.Item.Cells[4].Text.ToString().Trim() + "</a>";
        ////调整出

        //e.Item.Cells[5].Text = "<a href=# onclick=\"openDetail('Zxqktz .aspx','" + kssj + "','" + stryskm + "','" + kssj + "','" + jzsj + "','" + dept + "','" + strflowid + "','" + strdydj + "','tc');\">" + e.Item.Cells[5].Text.ToString().Trim() + "</a>";
        ////调整入
        //e.Item.Cells[6].Text = "<a href=# onclick=\"openDetail('Zxqktz .aspx','" + kssj + "','" + stryskm + "','" + kssj + "','" + jzsj + "','" + dept + "','" + strflowid + "','" + strdydj + "','tr');\">" + e.Item.Cells[6].Text.ToString().Trim() + "</a>";

    }
}
