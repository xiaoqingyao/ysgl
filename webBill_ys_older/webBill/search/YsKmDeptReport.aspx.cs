using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class webBill_search_YsKmDeptReport : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            DateTime dtEnd = DateTime.Now;
            DateTime dtBeg = new DateTime(dtEnd.Year, dtEnd.Month, 1);
            txt_beg.Text = dtBeg.ToString("yyyy-MM-dd");
            txt_end.Text = dtEnd.ToString("yyyy-MM-dd");
        }
    }


    private void Bind()
    {

        DateTime begDate = Convert.ToDateTime(txt_beg.Text);
        DateTime endDate = Convert.ToDateTime(txt_end.Text);
        SqlParameter[] sps = { 
                                 new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj", Convert.ToDateTime( endDate.ToString("yyyy-MM-dd")+ " 23:59:59")),
                                 new SqlParameter("@deptCode",""),
                                 new SqlParameter("@fykmcode","")
                             };
        DataSet ds = server.ExecuteProcedure("pro_bill_sytj_sybm", sps);
        GridViewCreate(ds.Tables[0]);

        GridView1.DataSource = ds;
        GridView1.DataBind();
        //GridView1.Columns[1].GetType();
    }
    protected void btn_find_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        Bind();
        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        Response.AddHeader("content-disposition", "attachment; filename=XXExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);
        
        GridView1.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for
    }



    private void GridViewCreate(DataTable dt)
    {
        int width = 300;
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;

            bf.HeaderStyle.CssClass = "myGridHeader";
            bf.ItemStyle.CssClass = "myGridItem";
            bf.ItemStyle.Wrap = true;
            bf.HeaderStyle.Wrap = true;
            bf.HeaderStyle.Width = 110;
            bf.ItemStyle.Width = 110;
            //if (dc.ColumnName != "费用科目")
            //{
            //    bf.DataFormatString = "{0:N2}";
            //    bf.ItemStyle.CssClass = "myGridItemRight";
            //    bf.ItemStyle.Width = bf.HeaderStyle.Width;
            //    bf.ItemStyle.Width = 120;
            //    bf.HeaderStyle.Width = 122;
            //}
            if (dc.ColumnName == "预算金额" || dc.ColumnName == "报销金额")
            {
                // bf.ItemStyle.Width = 120;
                //bf.HeaderStyle.Width = 130;
            }
            if (dc.ColumnName == "费用科目" )
            {
                bf.ItemStyle.Width = 150;
                bf.HeaderStyle.Width = 150;
            }
            else
            {
                //bf.ItemStyle.Width = 100;
                //bf.HeaderStyle.Width = 100;
              //  bf.ItemStyle.CssClass = "NoBreak";
            }
            GridView1.Columns.Add(bf);
            width += 110;
        }
        this.GridView1.Width = width;
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView1.UseAccessibleHeader = true;
        if (GridView1.Rows.Count > 0)
        {
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}
