using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_search_Zxqktz : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }

        if (!IsPostBack)
        {
            this.bindData();
        }
    }

    public void bindData()
    {
        string kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string jzsj = DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString();
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string fykm = Page.Request.QueryString["fykm"].ToString().Trim();
        string strdydj = Request["dydj"].ToString();
        string strtype = Request["type"].ToString();
        string strsql = @"select a.gcbh as Gcbh ,(select xmmc from bill_ysgc where gcbh=a.gcbh) as GcMc,a.yskm as Yskm,a.billcode,ysdept,ystype
			 	,convert(char(10),b.billdate,121) as billdate,(select '['+deptcode+']'+deptname from bill_departments where deptcode=b.billdept) as dept
			 	,(select yskmmc from bill_yskm where yskmcode=a.yskm ) as YskmMc,a.ysje as Ysje from   Bill_Ysmxb a ,bill_main b
               where a.billcode=b.billcode   ";
        if (!string.IsNullOrEmpty(strtype))
        {
            if (strtype == "tc")
            {
                strsql += " and a.ysje<0  and ystype='3' and dydj='" + strdydj + "'";
            }
            else if (strtype == "tr")
            {
                strsql += " and a.ysje>0 and ystype='3' and dydj='" + strdydj + "'";
            }
            else if (strtype=="ys")
            {
                strsql += " and ystype='1' and dydj is null";
            }

        }

        if (!string.IsNullOrEmpty(deptCode))
        {
            string[] a = deptCode.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string strdept = "";
            if (a.Count() > 0)
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    strdept += "'";
                    strdept += a[i];
                    strdept += "',";

                }
            }

            strdept = strdept.Substring(0, strdept.Length - 1);
            strsql += " and ysdept in(" + strdept + ")";
        }

        if (!string.IsNullOrEmpty(fykm))
        {
            strsql += " and yskm='" + fykm + "'";
        }
        if (!string.IsNullOrEmpty(kssj) & !string.IsNullOrEmpty(jzsj))
        {
            strsql += "and   a.gcbh in (select gcbh from bill_ysgc where convert(char(10),kssj,121)>='" + kssj + "' and convert(char(10),jzsj,121)<='" + jzsj + "')";
            //strsql += " and convert(char(10),b.billdate,121)>='" + kssj + "'";
        }
        //if (!string.IsNullOrEmpty(jzsj))
        //{
        //    strsql += " and convert(char(10),b.billdate,121)<='" + jzsj + "'";
        //}
        DataSet temp = server.GetDataSet(strsql); //server.GetDataSet("exec pro_bill_bxtj_yskm_detail2 '" + kssj + "','" + jzsj + "','" + deptCode + "','" + fykm + "'");
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

    }
    /// <summary>
    /// 导出 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

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