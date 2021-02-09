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

public partial class webBill_tjbb_shouru_ystj_result : System.Web.UI.Page
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
                dept = "统计单位：" + server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode='" + dept + "'");
            }
            this.Label1.Text = "开始时间：" + Page.Request.QueryString["kssj"].ToString().Trim() + "  截止时间：" + Page.Request.QueryString["jzsj"].ToString().Trim() + "  " + dept;
            this.bindData();
        }
    }
    public void bindData()
    {
        string dept = Request.QueryString["deptCode"].ToString();
        if (!dept.Equals(""))
        {
            dept = dept.Replace("|", "','");
            dept = "'" + dept + "'";
        }
        
        
        //DataSet temp = server.GetDataSet("exec pro_bill_bxtj_dept2 '" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + DateTime.Parse( DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString()).AddDays(1).ToShortDateString() + "','" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        //DataSet temp = server.GetDataSet("exec pro_bill_shourutongji_yskm '" + + "','" + + "'," + dept );



        SqlParameter[] arrSlPara ={
                                 new SqlParameter("@kssj", Page.Request.QueryString["kssj"].ToString().Trim()),
                                new SqlParameter("@jzsj",Convert.ToDateTime(Page.Request.QueryString["jzsj"]).ToString("yyyy-MM-dd") + " 23:59:59" ),
                                new SqlParameter("@deptcode",dept)
                                 };
        DataSet ds = server.ExecuteProcedure("pro_bill_shourutongji_yskm", arrSlPara);
        DataTable temp = ds.Tables[0];



        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //{
        //    if (e.Item.Cells[0].Text.ToString().Trim().IndexOf("合计") >= 0)
        //    { }
        //    else
        //    {
        //        string dept = Request.QueryString["deptCode"].ToString().Trim();
        //        string yskm = e.Item.Cells[10].Text.ToString().Trim();
        //        if (dept == "")
        //        {
        //            dept = e.Item.Cells[10].Text.ToString().Trim();
        //            yskm = "";
        //        }
        //        e.Item.Cells[0].Text = "<a href=# onclick=\"openDetail('" + DateTime.Parse(e.Item.Cells[11].Text.ToString().Trim()).ToShortDateString() + "','" + yskm + "','" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + Page.Request.QueryString["jzsj"].ToString().Trim() + "','" + dept + "','" + Page.Request.QueryString["type"].ToString().Trim() + "');\">" + e.Item.Cells[10].Text.ToString().Trim() + "</a>";
        //        e.Item.Cells[1].Text = "<a href=# onclick=\"openDetail('" + DateTime.Parse(e.Item.Cells[11].Text.ToString().Trim()).ToShortDateString() + "','" + yskm + "','" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + Page.Request.QueryString["jzsj"].ToString().Trim() + "','" + dept + "','" + Page.Request.QueryString["type"].ToString().Trim() + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        //    }
        //}
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("shouru_ystj_index.aspx");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
