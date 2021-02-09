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
using System.IO;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// 像费用使用部门统计一样来统计项目
/// Edit by Lvcc
/// </summary>
public partial class webBill_search_YsXmDeptReport2 : System.Web.UI.Page
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

    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_excel_Click(object sender, EventArgs e) {
        Bind();
        //DateTime begDate = Convert.ToDateTime(txt_beg.Text);
        //DateTime endDate = Convert.ToDateTime(txt_end.Text);

        //SqlParameter[] sps = { 
        //                     new SqlParameter("@kssj",begDate),
        //                         new SqlParameter("@jzsj",endDate),
        //                         new SqlParameter("@deptCode","")
        //                     };
        //DataSet ds = server.ExecuteProcedure("pro_bill_sytj_syxm", sps);
        //DataTable dtRel = ds.Tables[0];
        //DataTableToExcel(dtRel, this.GridView1, null);
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
        //base.VerifyRenderingInServerForm(control);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_find_Click(object sender, EventArgs e) {
        Bind();
    }

    /// <summary>
    /// gridview加载前
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_PreRender(object sender,EventArgs e) {
        //GridView1.UseAccessibleHeader = true;
        //if (GridView1.Rows.Count > 0)
        //{
        //    GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}
    }

    private void Bind() {
        DateTime begDate = Convert.ToDateTime(txt_beg.Text);
        DateTime endDate = Convert.ToDateTime(txt_end.Text);

        SqlParameter[] sps = { 
                             new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj",Convert.ToDateTime( endDate.ToString("yyyy-MM-dd")+ " 23:59:59")),
                                 new SqlParameter("@deptCode","")
                             };
        DataSet ds = server.ExecuteProcedure("pro_bill_sytj_syxm", sps);
        DataTable dtRel=ds.Tables[0];
        int iColumnCount=dtRel.Columns.Count;
        int iRowCount = dtRel.Rows.Count;
        DataRow dr = dtRel.NewRow();
        dr[0] = "合计：";
        for (int i = 1; i < iColumnCount; i++)
        {
            double fSum = 0;
            for (int j = 0; j < iRowCount; j++)
            {
                double fJe = 0;
                if (double.TryParse(dtRel.Rows[j][i].ToString(), out fJe))
                {
                    fSum += fJe;
                }
            }
            dr[i] = fSum;
        }
        dtRel.Rows.InsertAt(dr, 0);
        GridViewCreate(dtRel);
        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    private void GridViewCreate(DataTable dt) {
        this.GridView1.Columns.Clear();
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;

            bf.HeaderStyle.CssClass = "myGridHeader NoBreak";
            bf.ItemStyle.CssClass = "myGridItem NoBreak";
            if (dc.ColumnName == "项目名称")
            {
                bf.ItemStyle.Width = 180;
                bf.HeaderStyle.Width = 184;
            }
            else {
                //bf.ItemStyle.Width = 100;
                //bf.ItemStyle.Width = 100;
            }
            this.GridView1.Columns.Add(bf);
        }
    }

    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, GridView stylegv, MyDelegate rowbound)
    {
        if (dtData != null)
        {
            // 设置编码和附件格式
            Response.ContentType = "application/vnd.ms-excel";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.Charset = "utf-8";

            // 导出excel文件
            // IO用于导出并返回excel文件
            StringWriter strWriter = new StringWriter();
            HtmlTextWriter htmlWriter = new HtmlTextWriter(strWriter);

            DataGrid gvExport = new DataGrid();


            gvExport.AutoGenerateColumns = false;
            BoundColumn bndColumn = new BoundColumn();
            for (int j = 0; j < stylegv.Columns.Count - 1; j++)
            {
                bndColumn = new BoundColumn();
                if (stylegv.Columns[j] is BoundField)
                {
                    bndColumn.DataField = ((BoundField)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundField)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = dtData.DefaultView;
            gvExport.AllowPaging = false;
            gvExport.DataBind();
            if (rowbound != null)
            {
                rowbound(gvExport);
            }

            // 返回客户端
            gvExport.RenderControl(htmlWriter);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\">");
            Response.Write(strWriter.ToString());
            Response.Write("</body></html>");
            Response.End();
        }
    }

}
