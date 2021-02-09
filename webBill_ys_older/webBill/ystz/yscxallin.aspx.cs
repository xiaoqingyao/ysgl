using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

public partial class webBill_ystz_yscxallin : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                this.TextBox1.Text = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
                this.DropDownList1.Text = "01";
                this.DropDownList2.Text = DateTime.Now.ToString("yyyy-MM-dd").Substring(5, 2); ;
                //this.BindDataGrid();
            }
        }


        
    }
    void BindDataGrid()
    {
        SqlParameter[] sp = {new SqlParameter("@nd",this.TextBox1.Text.ToString().Trim()),
                             new SqlParameter("@ksyf",this.DropDownList1.Text.ToString().Trim()),
                             new SqlParameter("@jzyf",this.DropDownList2.Text.ToString().Trim())
                            };
        DataSet ds = server.ExecuteProcedure("pro_bill_zhcx", sp);

        GridView1.Columns.Clear();//移除控件中所有项

        GridViewCreate(ds.Tables[0]);//重新创建控件项

        GridView1.DataSource = ds;
        GridView1.DataBind();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);



        GridView1.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }

   


    protected void Button2_Click(object sender, EventArgs e)
    {

        this.BindDataGrid();
    }


    private void GridViewCreate(DataTable dt)
    {
        foreach (DataColumn dc in dt.Columns)
        {
            BoundField bf = new BoundField();
            bf.DataField = dc.ColumnName;
            bf.HeaderText = dc.ColumnName;
            if (dc.ColumnName != "费用科目")
            {
                bf.DataFormatString = "{0:N2}";
                bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
            }
            
            GridView1.Columns.Add(bf);
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
        {

            //设置gridview头和体不自动换行
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].CssClass = "NoBreak";
            }
        }
    }
}
