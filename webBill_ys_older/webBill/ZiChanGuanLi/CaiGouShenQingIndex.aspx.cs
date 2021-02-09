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
using Bll.Zichan;
using WorkFlowLibrary.WorkFlowBll;
using Models;
using System.IO;

public partial class ZiChan_ZiChanGuanLi_CaiGouShenQingIndex : System.Web.UI.Page
{
    Bll.Bills.BillMainBLL mainbll = new Bll.Bills.BillMainBLL();
    ZiChan_CaiGouShenQingBll bll = new ZiChan_CaiGouShenQingBll();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeptAll());
            if (!IsPostBack)
            {
                Bind(true);

            }
        }
    }

    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    private void Bind(bool ispock)
    {
        DataTable dt = getdatable();
        myGrid.DataSource = dt;
        myGrid.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataTable dtExport = new DataTable();
        dtExport = getdatable();
        DataTableToExcel(dtExport, this.myGrid, null);
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
    //查询
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        HiddenField1.Value = "1";
        Bind(true);
    }
    protected void PaginationToGV1_DataBinding(object sender, EventArgs e)
    {
        Bind(false);
    }
    protected int[] ComputeRow(int pageIndex)
    {
        int[] ret = new int[2];
        int pagRows = Convert.ToInt32(ConfigurationManager.AppSettings["ItemNumPerPage"]);
        ret[0] = (pageIndex - 1) * pagRows;
        ret[1] = pageIndex * pagRows;
        return ret;
    }
    //删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string bh = HiddenBh.Value;
        Bill_Main mainmodel = new Bill_Main();
        mainmodel.BillCode = bh;
        if (bh != "")
        {
            int row = bll.Delete(mainmodel,bh);
            if (row > 0)
            {
                Bind(true);
            }

        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[4].Text;
            if (zt == "end")
            {
                e.Item.Cells[4].Text = "审批通过";
            }
            else
            {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[4].Text = state;
            }
          
        }
    }
    //分页
    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        Bind(false);
    }


    private DataTable getdatable() 
    {
        string sql = @"   select billcode , 
                         (select '['+deptCode+']'+ deptName from dbo.bill_departments where deptCode=billdept) as billdept , billdate,
                         (select '['+userCode+']'+ userName from bill_users where userCode=billuser)   as billuser,
                         stepid,Row_Number()over(order by billCode desc) as crow 
                         from Bill_Main 
                         where 1=1 and flowid='zccgsq' ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and billdept in (" + deptCodes + ") ";

        if (txb_sqrqbegin.Text != "" && txb_sqrqbegin.Text != null)
        {
            sql += " and billdate >= '" + txb_sqrqbegin.Text + "' ";
        }
        if (txb_sqrqend.Text != "" && txb_sqrqend.Text != null)
        {
            sql += "  and billdate <= '" + txb_sqrqend.Text + "'  ";
        }
        if (txtbh.Text != "" && txtbh.Text != null)
        {
            sql += " and billcode like '%" + txtbh.Text + "%'  ";
        }

        string strdeptcode = this.txtdept.Text;
        try
        {
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            txtdept.Text = "";
            strdeptcode = "";
        }
        if (!strdeptcode.Equals(""))
        {
            sql += " and billdept in ('" + strdeptcode + "')  ";
        }




        //获取分页行数
        string countsql = "select count(*) from (" + sql + ")t";
        PaginationToGV1.RowsCount = server.ExecuteScalar(countsql).ToString();
        PaginationToGV1.PageIndex = "1";


        int[] ArrRows = ComputeRow(Convert.ToInt32(PaginationToGV1.PageIndex == "" ? "1" : PaginationToGV1.PageIndex));

        string cxsql = " select * from ( " + sql + ")t where t.crow >= " + ArrRows[0] + " and t.crow < " + ArrRows[1];


        DataTable dtExport = new DataTable();
        dtExport = server.RunQueryCmdToTable(cxsql);
        return dtExport;
    
    }
}
