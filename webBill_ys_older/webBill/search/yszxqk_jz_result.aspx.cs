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

public partial class webBill_search_yszxqk_jz_result : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    double[] arreveColumnAmount=new double[13];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }

    }

    private void Bind()
    {
        string nd = Request["nd"];
        string yd = Request["yd"];
        string dept = Request["deptcode"];
        SqlParameter[] sps = { 
                             new SqlParameter("@nd", nd), new SqlParameter("@yd", yd) 
                            , new SqlParameter("@depts", dept) 
                             };
        DataSet dtrel = server.ExecuteProcedure("pro_bill_ystj_yskm_dept_jiezhi", sps);
        GridViewCreate(dtrel.Tables[0]);
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for
    }
    private void GridViewCreate(DataTable tables)
    {
        GridView1.Columns.Clear();
        int width = 600;
        foreach (DataColumn dc in tables.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;
            bf.HeaderStyle.Width = 60;
            bf.ItemStyle.Width = 60;
            bf.DataFormatString = "{0:N2}";
            bf.ItemStyle.CssClass = "myGridItemRight";
            if (dc.ColumnName.Equals("年度预算"))
            {
                bf.HeaderStyle.Width = 80;
                bf.ItemStyle.Width = 80;
                bf.ItemStyle.CssClass = "myGridItemRight";
                width += 80;
            }
            else
            {
                width += 60;
            }

            GridView1.Columns.Add(bf);
        }
        GridView1.Width = width;
    }
    protected void GridView1_PreRender(object sender, EventArgs e)
    {
        GridView1.UseAccessibleHeader = true;
        if (GridView1.Rows.Count > 0)
        {
            GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void GridView1_OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        string strHeaderStr = "部门编号#部门名称#科目编号#科目名称#年预算情况 年度预算,年度决算,年度决算执行率,年度累计执行率#当月 当月预算,当月决算,当月执行率#累计 累计预算,累计决算,累计执行率";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
            dHelper.SplitTableHeader(e.Row, strHeaderStr);
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Header)
        {
            for (int i = 4; i < 13; i++)
            {
                string stramount = e.Row.Cells[i].Text.Trim();
                double damount = 0;
                if (double.TryParse(stramount, out damount))
                {
                    arreveColumnAmount[i] += damount;
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[0].Style.Add("text-align", "right");
            for (int i = 4; i < 13; i++)
            {
                e.Row.Cells[i].Text = arreveColumnAmount[i].ToString("N02");
                e.Row.Cells[i].Style.Add("text-align", "right");
            }
        }
    }
}