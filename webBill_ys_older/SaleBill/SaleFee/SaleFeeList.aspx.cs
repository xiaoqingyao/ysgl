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
using System.Text;
using System.IO;
using System.Data.SqlClient;

public partial class SaleBill_SaleFee_SaleFeeList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdatefrom = "";
    string strdateto = "";
    string strDeptCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (Request["datefrom"] != null)
        {
            strdatefrom = Request["datefrom"].ToString();
            this.lblfee.Text = "开始日期：" + strdatefrom;
        }
        if (Request["dateto"] != null)
        {
            strdateto = Request["dateto"].ToString();
            this.lblfee.Text += "截止日期：" + strdateto;
        }
        if (Request["deptCode"]!=null)
        {
            strDeptCode = Request["deptCode"].ToString();
        }
        if (!IsPostBack)
        {
            this.BindDataGrid();
        }
    }

    public void BindDataGrid()
    {

      //  string strsql = getSelectSql();
//        DataTable temp = server.RunQueryCmdToTable(strsql);

        SqlParameter[] arrSlPara ={
                                 new SqlParameter("@dateFrm",strdatefrom),
                                new SqlParameter("@dateTo",strdateto),
                                new SqlParameter("@deptCode",strDeptCode)
                                 };
        DataSet ds=server.ExecuteProcedure("SaleBill_SaleDeptInOutTongji", arrSlPara);
        DataTable temp = ds.Tables[0];

        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();

        this.myGrid.DataSource = temp;
        myGrid.DataBind();
    }

    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strDeptCode = e.Item.Cells[0].Text.ToString();
            e.Item.Cells[0].Text = "<a href=# onclick=\"openDetaildept('" + strDeptCode + "','" + strdatefrom + "','" + strdateto + "');\">" + e.Item.Cells[0].Text.ToString().Trim() + "</a>";
            e.Item.Cells[1].Text = "<a href=# onclick=\"openDetaildept('" + strDeptCode + "','" + strdatefrom + "','" + strdateto + "');\">" + e.Item.Cells[1].Text.ToString().Trim() + "</a>";
        }
    }

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion




    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Saleselect.aspx");
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //SqlParameter[] arrSlPara ={
        //                         new SqlParameter("@dateFrm",strdatefrom),
        //                        new SqlParameter("@dateTo",strdatefrom),
        //                        new SqlParameter("@deptCode",strDeptCode)
        //                         };
        //DataSet ds = server.ExecuteProcedure("SaleBill_SaleDeptInOutTongji", arrSlPara);
        //DataTable temp = ds.Tables[0];
        SqlParameter[] arrSlPara ={
                                 new SqlParameter("@dateFrm",strdatefrom),
                                new SqlParameter("@dateTo",strdatefrom),
                                new SqlParameter("@deptCode",strDeptCode)
                                 };
        DataSet ds = server.ExecuteProcedure("SaleBill_SaleDeptInOutTongji", arrSlPara);
        DataTable temp = ds.Tables[0];
        DataTableToExcel(temp, this.myGrid, null);
    }

    private string getSelectSql()
    {
        string sql = @"exec dbo.SaleBill_SaleDeptInOutTongji '" + strdatefrom + "','" + strdateto + "','"+strDeptCode+"'";
        return sql;
    }
    public delegate void MyDelegate(DataGrid gv);
    protected void DataTableToExcel(DataTable dtData, DataGrid stylegv, MyDelegate rowbound)
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
                if (stylegv.Columns[j] is BoundColumn)
                {
                    bndColumn.DataField = ((BoundColumn)stylegv.Columns[j]).DataField.ToString();
                    bndColumn.HeaderText = ((BoundColumn)stylegv.Columns[j]).HeaderText.ToString();

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
