using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using WorkFlowLibrary.WorkFlowBll;
using Bll.UserProperty;
using System.Data.SqlClient;

public partial class webBill_fysq_cgspList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    my_fzl.bindClss bindCl = new my_fzl.bindClss();
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
                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("cgsp");
                BindDataGrid();
            }
        }
    }

    protected void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight,70);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        DataTable dtrel = GetData(arrpage[0], arrpage[1],out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;
        //----------给gridview赋值

        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        if (dtrel.Rows.Count == 0 || dtrel == null)
        {
            this.ucPager.Visible = false;
        }


    }
   
    /// <summary>
    /// 根据查询条件获取符合条件的条数
    /// </summary>
    /// <param name="pagefrm"></param>
    /// <param name="pageto"></param>
    /// <param name="paramter"></param>
    /// <param name="strCondition"></param>
    /// <returns></returns>
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select  Row_Number()over(order by sj desc) as crow,
        (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,
        b.sj,b.sm,b.cgze,a.billCode,cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,stepID,

        (select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=a.billCode) and rdstate='3') as mind,b.gys as gys
        from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and a.billDept in (" + deptCodes + ")";

        List<SqlParameter> listSp = new List<SqlParameter>();
        string strCondition = "";//sql
        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >=@begtime ";
            listSp.Add(new SqlParameter("@begtime", txb_sqrqbegin.Text));
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <=@endtime ";
            listSp.Add(new SqlParameter("@endtime", txb_sqrqend.Text));
        }

        //获取条数
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount, listSp.ToArray()));

        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by sj desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, listSp.ToArray());
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            e.Item.Cells[1].Text = sysMgr.GetDeptCodeName(e.Item.Cells[1].Text);
            string zt = e.Item.Cells[7].Text;
            if (zt == "end")
            {
                e.Item.Cells[7].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[7].Text = state;
            }
        }
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

}
