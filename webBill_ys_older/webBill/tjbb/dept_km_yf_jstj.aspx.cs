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

public partial class webBill_tjbb_dept_km_yf_jstj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
        DataSet dtrel = server.ExecuteProcedure("pro_bill_jstj_yskm_dept_yf", sps);
        GridViewCreate(dtrel.Tables[0]);
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
    }
    private void GridViewCreate(DataTable tables)
    {
        GridView1.Columns.Clear();
        int width = 300;
        foreach (DataColumn dc in tables.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;
            bf.HeaderStyle.Width = 50;
            bf.ItemStyle.Width = 50;
            bf.DataFormatString = "{0:N2}";
            bf.ItemStyle.CssClass = "myGridItemRight";
            if (dc.ColumnName.Equals("年度已分配预算") || dc.ColumnName.Equals("年度未分配预算") || dc.ColumnName.Equals("科目名称"))
            {
                bf.HeaderStyle.Width = 80;
                bf.ItemStyle.Width = 80;
                bf.ItemStyle.CssClass = "myGridItem";
                width += 80;
            }
            else {
                width += 50; 
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
}
