using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dept_km_jd_ystj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    double[] arreveColumnAmount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet tmep = server.GetDataSet("select * from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");

            for (int i = 0; i <= tmep.Tables[0].Rows.Count - 1; i++)
            {
                ListItem li = new ListItem();
                li.Text = "[" + tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + tmep.Tables[0].Rows[i]["deptName"].ToString().Trim();
                li.Value = tmep.Tables[0].Rows[i]["deptCode"].ToString().Trim();
                this.drpDept.Items.Add(li);
            }
            this.drpDept.Items.Add(new ListItem("汇总所有部门", ""));
            DataTable dtnd = server.GetDataTable("select distinct nian from bill_ysgc order by nian desc ", null);
            this.ddlNd.DataSource = dtnd;
            this.ddlNd.DataTextField = "nian";
            this.ddlNd.DataValueField = "nian";
            this.ddlNd.DataBind();
        }
    }

    protected void btn_find_Click(object sender, EventArgs e)
    {
        Bind();
    }
    private void Bind()
    {
        string strdeptcode = this.drpDept.SelectedValue;
        string strnd = this.ddlNd.SelectedValue;
        SqlParameter[] sps = { 
                             new SqlParameter("@deptcode", strdeptcode), new SqlParameter("@nd", strnd) 
                             };
        DataSet dtrel = server.ExecuteProcedure("pro_bill_ystj_yskm_dept_yf", sps);
        arreveColumnAmount = new double[dtrel.Tables[0].Columns.Count];
        GridViewCreate(dtrel.Tables[0]);
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
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
            if (dc.ColumnName.Equals("年度已分配预算") || dc.ColumnName.Equals("年度未分配预算") || dc.ColumnName.Equals("科目名称") || dc.ColumnName.Equals("部门名称"))
            {
                bf.HeaderStyle.Width = 100;
                bf.ItemStyle.Width = 100;
                bf.ItemStyle.CssClass = "myGridItem";
                width += 100;
            }
            else if (dc.ColumnName.Equals("年度预算") || dc.ColumnName.Equals("部门编号"))
            {
                bf.HeaderStyle.Width = 80;
                bf.ItemStyle.Width = 80;
                bf.ItemStyle.CssClass = "myGridItem";
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
        string strHeaderStr = "部门编号#部门名称#科目编号#科目名称#年预算情况 年度预算,年度已分配预算,年度决算,年度决算执行率,年度未分配预算";
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
                        if ((i - 6) % 3 == 0 && i > 6)//预算
                        {
                            string strcolumname = dt.Columns[i].ColumnName;
                            string strhd1 = strcolumname.Substring(0, strcolumname.Length - 2);
                            strHeaderStr += string.Format("#{0} {1},{2},{3}", strhd1, "预算", "决算", "执行率");
                        }
                    }
                    DynamicTHeaderHepler dHelper = new DynamicTHeaderHepler();
                    dHelper.SplitTableHeader(e.Row, strHeaderStr);
                }
            }
        }
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.Header)
        {
            for (int i = 4; i < arreveColumnAmount.Length; i++)
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
            e.Row.Cells[3].Text = "合计：";
            e.Row.Cells[0].Style.Add("text-align", "right");
            for (int i = 4; i < arreveColumnAmount.Length; i++)
            {
                e.Row.Cells[i].Text = arreveColumnAmount[i].ToString("N02");
                e.Row.Cells[i].Style.Add("text-align", "right");
            }
        }
    }
}