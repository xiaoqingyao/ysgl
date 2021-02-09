using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_QyNdtj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DrpSelectBid();
            Bind();

        }
    }

    private void Bind()
    {

        string strkssj = bgintime.SelectedValue.ToString().Trim();
        string strjzsj = endtime.SelectedValue.ToString().Trim();
        string sql = "exec dz_ys_gkysqk '" + strkssj + "','" + strjzsj + "'";


        ////SqlParameter[] sps =  { 
        ////                       new SqlParameter("@nd",strnd)
        ////                   };

      
        ////DataSet ds = server.ExecuteProcedure("dz_ys_gkysqk", sps);//
        Response.Write(sql);
        DataTable dt = server.GetDataTable(sql, null);
        //Response.Write(dt.Rows.Count);

        DataGrid1.DataSource = dt;
        DataGrid1.DataBind();
    }
    private void DrpSelectBid()
    {
        string selectndsql = @" select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
        DataTable selectdt = server.GetDataTable(selectndsql, null);
        drpSelectNd.DataSource = selectdt;
        drpSelectNd.DataTextField = "xmmc";
        drpSelectNd.DataValueField = "nian";
        drpSelectNd.DataBind();
        if (!string.IsNullOrEmpty(drpSelectNd.SelectedValue))
        {
            string ysgcyuesql = " select * from bill_ysgc  where yue!='' and nian='" + drpSelectNd.SelectedValue + "'";

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


   
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        string strkssj = bgintime.SelectedValue.ToString().Trim();
        string strjzsj = endtime.SelectedValue.ToString().Trim();
        string sql = "exec dz_ys_gkysqk '" + strkssj + "','" + strjzsj + "'";
        DataTable dt = server.GetDataTable(sql, null);
        new ExcelHelper().ExpExcel(dt, "职能部门占用预算表", null);
      //  Response.ClearContent();
        //Response.Charset = "utf-8";
        //Response.ContentEncoding = System.Text.Encoding.UTF8;
        //Response.HeaderEncoding = System.Text.Encoding.UTF8;

        //Response.AddHeader("content-disposition", "attachment; filename=znbzzysb.xls");

        //Response.ContentType = "application/excel";

        //StringWriter sw = new StringWriter();

        //HtmlTextWriter htw = new HtmlTextWriter(sw);

        //GridView1.RenderControl(htw);

        //Response.Write(sw.ToString());

        //Response.End();
    }



    protected void Button1_Click(object sender, EventArgs e)
    {
        this.Bind();
    }
    protected void GridView1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
      //  string yskmname = e.Item.Cells[1].Text.Trim();

      //  e.Item.Cells[0].CssClass = "locked";

       
    }
}