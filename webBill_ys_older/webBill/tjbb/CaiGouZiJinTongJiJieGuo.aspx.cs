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

public partial class webBill_tjbb_CaiGouZiJinTongJiJieGuo : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdatefrom = "";
    string strdateto = "";
    string strgysbh = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../webBill.aspx','_top');", true);
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
        if (Request["gysbh"]!=null)
        {
            strgysbh = Request["gysbh"].ToString();
        }
        if (!IsPostBack)
        {
            this.BindDataGrid();
        }
    }

    private void BindDataGrid(){
        string strSql = @"select gysbh,'['+gysbh+']'+gysmc as gysmc,sum(fkje) as totalje from bill_main main,bill_cgzjfk item
                where main.billCode=item.billcode  and main.billDate>=cast('"+strdatefrom+"' as datetime) and main.billDate<=cast('"+strdateto+"' as datetime) and item.gysbh in ("+strgysbh+") group by gysbh,gysmc";
        DataTable dtRel = server.GetDataTable(strSql,null);
        this.myGrid.DataSource = dtRel;
        this.myGrid.DataBind();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();

//        string strSql = @"select gysbh,'['+gysbh+']'+gysmc as gysmc,sum(fkje) as totalje from bill_main main,bill_cgzjfk item
//                where main.billCode=item.billcode  and main.billDate>=cast('" + strdatefrom + "' as datetime) and main.billDate<=cast('" + strdateto + "' as datetime) and item.gysbh in (" + strgysbh + ") group by gysbh,gysmc";
//        DataTable dtRel = server.GetDataTable(strSql, null);
 
//        DataTableToExcel(dtRel, this.myGrid, null);
        
    }
    public override void VerifyRenderingInServerForm(Control control)
    {

    }

    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e) {
        Response.Redirect("CaiGouZiJinTongJi.aspx");
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e==null)
        {
            return;
        }
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            e.Item.Cells[1].Text = "<a href=# style=\"color:Blue\" onclick=\"openDetail('../cgzj/cgzjbxList.aspx?gysbh=" + e.Item.Cells[0].Text + "');\">" + e.Item.Cells[1].Text + "</a>";
        }
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
