using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class webBill_search_YsXmDeptReport : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime dtEnd = DateTime.Now;
            DateTime dtBeg = new DateTime(dtEnd.Year, dtEnd.Month, 1);
            txt_beg.Text = dtBeg.ToString("yyyy-MM-dd");
            txt_end.Text = dtEnd.ToString("yyyy-MM-dd");
            //GridView1.RowsDefaultCellStyle.WrapMode = False;

        }
    }


    private void Bind()
    {
        DateTime begDate = Convert.ToDateTime(txt_beg.Text);
        DateTime endDate = Convert.ToDateTime(txt_end.Text);
        SqlParameter[] sps = { 
                                 new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj",Convert.ToDateTime( endDate.ToString("yyyy-MM-dd")+ " 23:59:59")),
                                 new SqlParameter("@deptCode","")
                             };
        DataSet ds = server.ExecuteProcedure("pro_bill_xmtj_ys_bx", sps);
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    protected void btn_find_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        GridView1.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }


    //protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    //{
    //    string strbegtime = "";
    //    string strendtime = "";
    //    string strdeptcode = "";
    //    string strxmcode = "";


    //    if (txt_beg.Text.Trim() != "")//开始时间
    //    {
    //        strbegtime = txt_beg.Text.Trim();
    //    }
    //    if (txt_end.Text.Trim() != "")//结束时间
    //    {
    //        strendtime = txt_end.Text.Trim();
    //    }
    //    if (e.Item.Cells[0].Text.ToString().Trim() != "")
    //    {
    //        strdeptcode = e.Item.Cells[0].Text.ToString().Trim();
    //        strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
    //    }
    //    if (e.Item.Cells[1].Text.ToString().Trim() != "")
    //    {
    //        strxmcode = e.Item.Cells[1].Text.ToString().Trim();
    //    }

    //    e.Item.Cells[0].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strdeptcode + "');\">" + e.Item.Cells[0].Text.ToString().Trim() + "</a>";
    //    e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strxmcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";


    //}

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for
    }

    protected void myGrid_ItemDataBound(object sender, EventArgs e)
    {
        //    string strbegtime = "";
        //    string strendtime = "";
        //    string strdeptcode = "";
        //    string strxmcode = "";


        //    if (txt_beg.Text.Trim() != "")//开始时间
        //    {
        //        strbegtime = txt_beg.Text.Trim();
        //    }
        //    if (txt_end.Text.Trim() != "")//结束时间
        //    {
        //        strendtime = txt_end.Text.Trim();
        //    }
        //    if (e.Item.Cells[0].Text.ToString().Trim() != "")
        //    {
        //        strdeptcode = e.Item.Cells[0].Text.ToString().Trim();
        //        strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        //    }
        //    if (e.Item.Cells[1].Text.ToString().Trim() != "")
        //    {
        //        strxmcode = e.Item.Cells[1].Text.ToString().Trim();
        //    }

        //    e.Item.Cells[0].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strdeptcode + "');\">" + e.Item.Cells[0].Text.ToString().Trim() + "</a>";
        //    e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strxmcode + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        string strbegtime = "";
        string strendtime = "";
        string strdeptcode = "";
        string strxmcode = "";



        if (e.Row.Cells[0].Text.ToString().Trim() != "合计：" && e.Row.Cells[0].ToString().Trim() != "" && e.Row.Cells[0].Text.ToString().Trim() != "&nbsp;")
            {
                if (txt_beg.Text.Trim() != "")//开始时间
                {
                    strbegtime = txt_beg.Text.Trim();
                }
                if (txt_end.Text.Trim() != "")//结束时间
                {
                    strendtime = txt_end.Text.Trim();
                }
                if (e.Row.Cells[0].Text.ToString().Trim() != "" && e.Row.Cells[0].Text.ToString().Trim() != "&nbsp;")
                {
                    strdeptcode = e.Row.Cells[0].Text.ToString().Trim();
                    strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1).Trim();
                }
                if (e.Row.Cells[1].Text.ToString().Trim() != "")
                {
                    strxmcode = e.Row.Cells[1].Text.ToString().Trim();
                }

                e.Row.Cells[0].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strdeptcode + "','" + strxmcode + "');\">" + e.Row.Cells[0].Text.ToString().Trim() + "</a>";
                e.Row.Cells[1].Text = "<a href=# onclick=\"openDetail('" + strbegtime + "','" + strendtime + "','" + strdeptcode + "','" + strxmcode + "');\">" + e.Row.Cells[1].Text.ToString().Trim() + "</a>";

            }
            else
            {

            }

        

    }
}
