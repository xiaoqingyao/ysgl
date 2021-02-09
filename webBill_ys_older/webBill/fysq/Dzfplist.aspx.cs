using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_fysq_Dzfplist : BasePage
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


            BindDataGrid();

        }
       // ClientScript.RegisterArrayDeclaration("availableTagsdept", GetdetpAll());

    }
    /// <summary>
    /// 绑定GridView
    /// </summary>
    protected void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 85);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
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
    /// 
    /// </summary>
    /// <param name="pagefrm"></param>
    /// <param name="pageto"></param>
    /// <param name="paramter"></param>
    /// <param name="strCondition"></param>
    /// <returns></returns>
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        // by a.billDate
        string sql = @"select Row_Number()over(order by billDate desc) as crow,a.billDept as showdeptcode,a.billName,a.stepID,a.billDate,a.billJe,
                (select '['+userCode+']'+userName from bill_users where userCode=a.billUser) as billUser,b.* 
                from bill_main a ,bill_fpfj  b
                 where a.billCode=b.billCode
                 and  a.flowID='dzfp'  ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and a.billDept in (" + deptCodes + ")";

        #region 查询条件
        List<SqlParameter> listSp = new List<SqlParameter>();
        //申请开始日期
        if (!string.IsNullOrEmpty(txb_sqrqbegin.Text))
        {
            sql += " and  a.billDate >=@begtime  ";
            listSp.Add(new SqlParameter("@begtime", txb_sqrqbegin.Text));

        }
        //申请结束日期
        if (!string.IsNullOrEmpty(txb_sqrqend.Text))
        {
            sql += " and  a.billDate <=@endtime";
            listSp.Add(new SqlParameter("@endtime", txb_sqrqend.Text));
        }
        string strBillCode = txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and a.billCode like @billcode";
            listSp.Add(new SqlParameter("@billcode", "%" + strBillCode + "%"));
        }

        if (!string.IsNullOrEmpty(txtAppDept.Text.Trim()))
        {
            string strDept = txtAppDept.Text.Trim();
            strDept = strDept.Substring(1, strDept.IndexOf("]") - 1);
            sql += " and a.billDept = @billdeptcode";
            listSp.Add(new SqlParameter("@billdeptcode", strDept));
        }
        //附加单据状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {
            if (strStatus.Equals("1"))
            {
                sql += " and isnull(b.ReportCode,'') != ''";
            }
            else if (strStatus.Equals("0"))
            {
                sql += " and isnull(b.ReportCode,'') = ''";
            }
        }
        #endregion
        //获取条数
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount, listSp.ToArray()));


        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by billDate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, listSp.ToArray());
    }



    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        
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
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

}