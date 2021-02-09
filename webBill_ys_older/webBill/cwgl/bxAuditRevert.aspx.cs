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
using Dal;
using System.Collections.Generic;

public partial class webBill_cwgl_bxAuditRevert : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            bindData();
        }
    }

    private void bindData()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 100);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>



        List<SqlParameter> listSp = new List<SqlParameter>();
        string strCondition = "";
        if (this.txtBillCode.Text.ToString().Trim() != "")
        {
            strCondition += " and (m.billName like @billName)";
            listSp.Add(new SqlParameter("@billName", "%" + txtBillCode.Text.Trim() + "%"));
        }
        icount = 0;
        DataTable dtrel = GetData(arrpage[0], arrpage[1], listSp, strCondition, out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;
        //----------给gridview赋值

        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
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
    private DataTable GetData(int pagefrm, int pageto, List<SqlParameter> paramter, string strCondition, out int count)
    {
        // by a.billDate
        string sql = @"select Row_Number()over(order by billdate desc) as crow, m.*,x.bxzy,x.bxsm,(select username from bill_users where usercode=m.billuser) as billuserName,
(select flowName from mainworkflow where flowid=m.flowid) as billTypeName from bill_main m,bill_ybbxmxb x where 1=1 and  m.billCode=x.billCode  and flowID in ('qtbx','ybbx','gkbx')";

        sql += strCondition;
        sql += " and x.sfgf='0' and stepID='end' ";
        //获取条数
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount, paramter.ToArray()));


        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by billdate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);

        return server.GetDataTable(strsqlframe, paramter.ToArray());
    }




    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        bindData();
    }
    protected void btn_Select_Click(object sender, EventArgs e)
    {
        bindData();
    }
    protected void btn_Rever_Click(object sender, EventArgs e)
    {
        string strCode = this.hdCode.Value;
        if (strCode.Equals(""))
        {
            return;
        }
        //new sqlHelper.sqlHelper().ExecuteProc("exec pro_shqx ",strCode);
        int iRel = DataHelper.ExcuteNonQuery("pro_shqx", new SqlParameter[] { new SqlParameter("@billcode", strCode) }, true);
        if (iRel > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('操作成功！')", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('操作失败！')", true);
        }
        bindData();
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        bindData();
    }
}
