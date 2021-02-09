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

public partial class webBill_tjbb_YSBalance : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    double[] arreveColumnAmount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDDL();
            Bind();
        }
    }

    private void Bind()
    {
        if (!string.IsNullOrEmpty(ddlNd.SelectedValue))
        {
            SqlParameter[] sps = { 
                                 new SqlParameter("@nd",this.ddlNd.SelectedValue)
                             };
            DataSet ds = server.ExecuteProcedure("[pro_tj_pinghengbiao]", sps);
            GridViewCreate(ds.Tables[0]);
            arreveColumnAmount = new double[ds.Tables[0].Columns.Count];
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }

    }
    private void BindDDL()
    {
        string sql = "select distinct nian from bill_ysgc order by nian desc ";
        ddlNd.DataSource = server.GetDataSet(sql);
        ddlNd.DataTextField = "nian";
        ddlNd.DataValueField = "nian";
        ddlNd.DataBind();
        ddlNd.SelectedValue = System.DateTime.Now.Year.ToString();
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
        int width = 1200;
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;
            bf.HeaderStyle.Width = 50;
            bf.ItemStyle.Width = 50;
            bf.DataFormatString = "{0:N2}";
            bf.ItemStyle.CssClass = "myGridItemRight";
            if (dc.ColumnName.Equals("flg"))
            {
                bf.HeaderStyle.Width = 70;
                bf.ItemStyle.Width = 70;
                bf.HeaderText = "收/支";
                bf.ItemStyle.CssClass = "myGridItemCenter";
            }
            if (dc.ColumnName.Length > 5)
            {
                bf.HeaderStyle.Width = 120;
                bf.ItemStyle.Width = 120;
            }
            GridView1.Columns.Add(bf);
            width += 50;
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
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Header)
        {

            for (int i = 1; i < arreveColumnAmount.Length; i++)
            {
                string stramount = e.Row.Cells[i].Text.Trim();
                double damount = 0;
                if (e.Row.Cells[0].Text.Trim() == "收入：")
                {

                    if (double.TryParse(stramount, out damount))
                    {
                        arreveColumnAmount[i] += damount;
                    }
                }
                else
                {
                    if (double.TryParse(stramount, out damount))
                    {
                        arreveColumnAmount[i] -= damount;
                    }
                }
                e.Row.Cells[0].Style.Add("text-align", "right");
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "利润：";
            e.Row.Cells[0].Style.Add("text-align", "right");
            for (int i = 1; i < arreveColumnAmount.Length; i++)
            {
                e.Row.Cells[i].Text = arreveColumnAmount[i].ToString("N02");
                e.Row.Cells[i].CssClass = "myGridItemRight";
                if (Convert.ToDecimal(arreveColumnAmount[i]) < 0)
                {
                    e.Row.Cells[i].Style.Add("color", "Red");
                }
            }
        }
    }

    protected void GridView1_Created(object sender, GridViewRowEventArgs e)
    {
        string strHeader = "收/支";
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView gv = sender as GridView;
            if (gv != null)
            {
                DataSet ds = gv.DataSource as DataSet;
                if (ds != null)
                {
                    DataTable dt = ds.Tables[0];
                    int icolumnCount = dt.Columns.Count;
                    for (int i = 1; i < icolumnCount; i++)
                    {
                        if (i % 2 == 1)//预算
                        {
                            string strcolumname = dt.Columns[i].ColumnName;
                            string strhd1 = strcolumname.Substring(0,strcolumname.Length - 2);
                            strHeader += string.Format("#{0} {1},{2}", strhd1, "预算", "实际");
                        }
                    }
                    DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                    dHelper.SplitTableHeader(e.Row, strHeader);
                }
            }
        }
    }


}
