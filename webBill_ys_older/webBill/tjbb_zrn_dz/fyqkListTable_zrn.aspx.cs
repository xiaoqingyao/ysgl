using Dal;
using Dal.Bills;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_zrn_dz_fyqkListTable_zrn : System.Web.UI.Page
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
        string dept = Request.QueryString["deptCode"].ToString();

        string nd = Request["nd"].ToString();

        dept = dept.Replace('|', ',');
        string type = Request.QueryString["type"].ToString();

        string ksyf = Page.Request.QueryString["ksyf"].ToString().Trim();
        string jzyf = Page.Request.QueryString["jzyf"].ToString().Trim();
        kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        jzsj = Page.Request.QueryString["jzsj"].ToString();

        strismj = configdal.GetValueByKey("deptjc");//获取配置项是否预算到末级

        if (string.IsNullOrEmpty(dept))
        {
            dept = "";
        }
      

        string strsql = @"exec bill_pro_fyqkb_zrn_dz '" + nd + "','" + ksyf + "','" + jzyf + "','" + dept + "','" + type + "','" + strdydj + "','" + strismj + "','" + kssj + "','" + jzsj + "'";

        Response.Write(strsql);
        //return;
        DataSet ds = server.GetDataSet(strsql);

    
        this.myGrid.DataSource = ds;
        this.myGrid.DataBind();

    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        Response.Redirect("fyqkFrame_zrn.aspx?dydj=" + strdydj);
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
        if (yskmname.IndexOf("合计") != -1)
        {
            e.Item.CssClass = "heji";
        }
       

    }
}