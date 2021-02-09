using Dal.Bills;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_deptys_cx : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdydj = "02";
    string ysflowid = "ys";
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //    return;
        //}

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["dydj"].ToString()))
            {
                strdydj = Request["dydj"].ToString();
            }
            MainDal maindal = new MainDal();
            ysflowid = maindal.getFlowId(strdydj);
            this.bindData();
        }
    }

    public void bindData()
    {
        string kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string jzsj = Page.Request.QueryString["jzsj"].ToString().Trim();
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string fykm = Page.Request.QueryString["fykm"].ToString().Trim();
        string strtype = Request["type"].ToString();
        string strsql = @"  select (select xmmc from bill_ysgc where gcbh=mxb.gcbh) as gcmc,
					  (select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=yskm) as kmmc,
					  ( select '['+deptcode+']'+deptname from bill_departments where deptcode=mxb.ysDept )as deptname, * from bill_ysmxb mxb,bill_main main  
					where mxb.billcode = main.billcode and flowid='" + ysflowid + "'  and main.stepid='end'	";
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
            strsql += "   and mxb.ysDept in(" + strdept + ")";
        }

        if (!string.IsNullOrEmpty(fykm))
        {
            strsql += " and yskm='" + fykm + "'";
        }
        else
        {
            strsql += "and yskm in (select yskmcode from bill_yskm where dydj='" + strdydj + "') ";
        }
        if (!string.IsNullOrEmpty(kssj) & !string.IsNullOrEmpty(jzsj))
        {
            strsql += "and   mxb.gcbh in (select gcbh from bill_ysgc where convert(char(10),kssj,121)>='" + kssj + "' and convert(char(10),jzsj,121)<='" + jzsj + "' and ystype='2' )";
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
    decimal decysje = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //合计行
        if (e.Item.ItemType != ListItemType.Header&&e.Item.ItemType!=ListItemType.Footer)
        {
            string strysje = e.Item.Cells[5].Text;
            if (!string.IsNullOrEmpty(strysje))
            {
                decysje += decimal.Parse(strysje);
            }
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].Text = "合计:";
            e.Item.Cells[5].Text = decysje.ToString("0.00");

        }
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