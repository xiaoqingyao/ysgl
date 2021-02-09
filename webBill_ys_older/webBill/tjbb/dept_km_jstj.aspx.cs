using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;

public partial class webBill_tjbb_dept_km_jstj : System.Web.UI.Page
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
        }
    }
    private void Bind()
    {

        DateTime begDate = Convert.ToDateTime(txt_beg.Text);
        DateTime endDate = Convert.ToDateTime(txt_end.Text);
        SqlParameter[] sps = { 
                                 new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj", Convert.ToDateTime(endDate.ToString("yyyy-MM-dd")+ " 23:59:59")),
                                 new SqlParameter("@deptCode",""),
                                 new SqlParameter("@fykmcode","")
                             };
        DataSet ds = server.ExecuteProcedure("[pro_bill_jstj_yskm_dept]", sps);
        GridViewCreate(ds.Tables[0]);

        GridView1.DataSource = ds;
        GridView1.DataBind();
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
        GridView1.Columns.Clear();
        int width = 300;
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;
            bf.HeaderStyle.Width = 80;
            bf.ItemStyle.Width = 80;
            bf.DataFormatString = "{0:N2}";
            bf.ItemStyle.CssClass = "myGridItemRight";
            if (dc.ColumnName.Equals("科目名称"))
            {
                bf.HeaderStyle.Width = 150;
                bf.ItemStyle.Width = 150;
                bf.ItemStyle.CssClass = "myGridItem";
            }
            if (dc.ColumnName.Length > 5)
            {
                bf.HeaderStyle.Width = 120;
                bf.ItemStyle.Width = 120;
            }
            GridView1.Columns.Add(bf);
            width += 80;
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
    protected void GridView1_rowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e == null)
        {
            return;
        }
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[0].Style.Add("text-align", "right");
        }
    }
}
