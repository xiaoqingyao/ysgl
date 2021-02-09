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
using System.Collections.Generic;
using Bll.Bills;
using System.IO;
using System.Text;
using WorkFlowLibrary.WorkFlowBll;
using Bll.UserProperty;

public partial class webBill_search_UniLoanList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    //Bll.Sepecial.RemittanceBll Remitbill = new Bll.Sepecial.RemittanceBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            if (!IsPostBack)
            {

                if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "hk")
                {
                    lblMsg.Visible = true;
                    DrCJstatus.SelectedValue = "4";
                    DrSPstatus.SelectedValue = "end";
                    Button2.Visible = false;
                    btn_print.Visible = false;
                }
                this.BindDataGrid();
            }
        }

    }

    public void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 80);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();

    }


    public DataTable GetData(int pagefrm, int pageto, out int count)
    {

        #region GetSearchModel
        Models.T_LoanList model = new Models.T_LoanList();
        //借款日期从
        if (txtLoanDateFrm.Text != null && txtLoanDateFrm.Text != "")
        {
            model.LoanDate = txtLoanDateFrm.Text;
        }
        //借款日期末
        if (txtLoanDateTo.Text != null && txtLoanDateTo.Text != "")
        {
            model.NOTE20 = txtLoanDateTo.Text;
        }
        //申请单号
        if (txtOrderCode.Text != null && txtOrderCode.Text != "")
        {
            model.Listid = txtOrderCode.Text;
        }
        //经办日期从
        if (txtRepsonDateTo.Text != null && txtRepsonDateTo.Text != "")
        {
            model.ResponsibleDate = txtRepsonDateFrom.Text;
        }
        //经办日期止
        if (txtRepsonDateFrom.Text != null && txtRepsonDateFrom.Text != "")
        {
            model.NOTE19 = txtRepsonDateTo.Text;
        }
        //冲减状态
        if (DrCJstatus.SelectedValue.ToString() != "")
        {
            model.Status = DrCJstatus.SelectedValue.ToString();
        }

        //审批状态
        if (DrSPstatus.SelectedValue.ToString() != "")
        {
            model.NOTE3 = DrSPstatus.SelectedValue.ToString();
        }

        if (!string.IsNullOrEmpty(Request["deptCode"]))
        {
            model.LoanDeptCode = Request["deptCode"].ToString();
        }

        #endregion
        string sql = loanbll.GetSql(model, "jksq");
        string hkStr = "";

        if (string.IsNullOrEmpty(Request["all"]) || Request["all"] != "1")
        {
            hkStr = " and (t.jbr='" + Session["userCode"].ToString() + "' or t.jkr='" + Session["userCode"].ToString() + "')";
        }
        if (!string.IsNullOrEmpty(Request["dept"]))
        {
            hkStr = " and billdept like '" + Request["dept"] + "%'";
        }

        string strsqlcount = "select count(*) from ( {0} ) t where 1=1 " + hkStr;
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}" + hkStr;
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 80);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        DataTableToExcel(dtrel, this.myGrid, null);
    }
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
    public delegate void MyDelegate(DataGrid gv);

    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }


    protected void Button6_Click1(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }



    }
    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            string zt = e.Item.Cells[8].Text;
            if (zt == "审批通过")
            {
                return;
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[8].Text = state;
            }
        }
    }
}
