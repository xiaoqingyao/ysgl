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

public partial class webBill_newTj_ysxmFrame : System.Web.UI.Page
{
    //sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
        DataTable temp = qm.SearchSyxmReport(begDate, endDate, deptcode);
        
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ysxmFrame.aspx");
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {/*
            if (e.Item.Cells[0].Text.ToString().Trim().IndexOf("合计") >= 0)
            { }
            else
            {
                
                DataSet temp = server.GetDataSet("select billDept,billName,billCode from bill_main where flowid='xmys' and billName2='" + e.Item.Cells[0].Text.ToString().Trim() + "'");
                if (temp.Tables[0].Rows.Count == 1)
                {
                    e.Item.Cells[2].Text = "<a href=# onclick=\"openDetail1('" + temp.Tables[0].Rows[0]["billDept"].ToString().Trim() + "','" + temp.Tables[0].Rows[0]["billCode"].ToString().Trim() + "');\">" + e.Item.Cells[2].Text.ToString().Trim() + "</a>";
                    e.Item.Cells[3].Text = "<a href=# onclick=\"openDetail2('" + e.Item.Cells[0].Text.ToString().Trim() + "','" + Page.Request.QueryString["kssj"].ToString().Trim() + "','" + Page.Request.QueryString["jzsj"].ToString().Trim() + "','" + Page.Request.QueryString["deptCode"].ToString().Trim() + "');\">" + e.Item.Cells[3].Text.ToString().Trim() + "</a>";
                }
                else
                {
                    e.Item.Cells[2].Text = "<a href=# onclick=\"alert('无数据！');\">" + e.Item.Cells[2].Text.ToString().Trim() + "</a>";
                    e.Item.Cells[3].Text = "<a href=# onclick=\"alert('无数据！');\">" + e.Item.Cells[3].Text.ToString().Trim() + "</a>";
                }
                
            } */
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        Response.AddHeader("content-disposition", "attachment; filename=XXExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }
}
