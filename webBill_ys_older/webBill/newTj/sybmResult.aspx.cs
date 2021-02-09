using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Models;
using Bll.UserProperty;

public partial class webBill_newTj_sybmFrame : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }

        if (!IsPostBack)
        {
            string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
            if (dept == "")
            { dept = "统计单位：所有单位"; }
            else
            {
                SysManager mgr = new SysManager();
                dept = "统计单位：" + mgr.GetDeptByCode(dept).DeptName.ToString().Trim();
            }
            this.Label1.Text = "开始时间:" + Page.Request.QueryString["kssj"].ToString().Trim();
            this.Label2.Text = "截止时间:" + Page.Request.QueryString["jzsj"].ToString().Trim();
            this.Label3.Text = dept;
            this.bindData();
        }
    }

    public void bindData()
    {
        DateTime begDate = Convert.ToDateTime(Page.Request.QueryString["kssj"].ToString().Trim());
        DateTime endDate = Convert.ToDateTime(DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString());
        string deptcode = Page.Request.QueryString["deptCode"].ToString().Trim();

        QueryManger qm = new QueryManger();
        DataTable temp = qm.SearchSybmReport(begDate, endDate, deptcode, "");
        GridViewCreate(temp);
        this.GridView1.DataSource = temp;
        this.GridView1.DataBind();
    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("sybmFrame.aspx");
    }



    private void GridViewCreate(DataTable dt)
    {
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;

            bf.HeaderStyle.CssClass = "myGridHeader NoBreak";
            if (dc.ColumnName != "费用科目")
            {
                bf.DataFormatString = "{0:N2}";
                bf.ItemStyle.CssClass = "myGridItemRight";
            }
            else
            {
                bf.ItemStyle.CssClass = "NoBreak";
            }
            GridView1.Columns.Add(bf);
        }
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
        //Response.ClearContent();

        //Response.Charset = "utf-8";
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        //Response.AddHeader("content-disposition", "attachment; filename=XXExcelFile.xls");

        //Response.ContentType = "application/excel";

        //StringWriter sw = new StringWriter();

        //HtmlTextWriter htw = new HtmlTextWriter(sw);

        //GridView1.RenderControl(htw);

        //Response.Write(sw.ToString());

        //Response.End();
        DateTime begDate = Convert.ToDateTime(Page.Request.QueryString["kssj"].ToString().Trim());
        DateTime endDate = Convert.ToDateTime(DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString());
        string deptcode = Page.Request.QueryString["deptCode"].ToString().Trim();

        QueryManger qm = new QueryManger();
        DataTable temp = qm.SearchSybmReport(begDate, endDate, deptcode, "");
        DataTableToExcel(temp, this.GridView1, null);
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        // Confirms that an HtmlForm control is rendered for
    }
    //声明委托
    public delegate void MyDelegate(GridView gv);

    protected void DataTableToExcel(object dtData, GridView stylegv, MyDelegate rowbound)
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

            GridView gvExport = new GridView();


            gvExport.AutoGenerateColumns = false;
            BoundField bndColumn = new BoundField();
            for (int j = 0; j < stylegv.HeaderRow.Cells.Count - 1; j++)
            {
                bndColumn = new BoundField();
                if (stylegv.Columns[j] is BoundField)
                {
                    bndColumn.DataField = ((BoundField)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundField)stylegv.Columns[j]).HeaderText.ToString();

                    //添加一列
                    gvExport.Columns.Add(bndColumn);
                }
            }
            gvExport.DataSource = dtData;
            //gvExport.DataSource = dtData.DefaultView;
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