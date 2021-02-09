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
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class webBill_cwgl_Qtbxgf : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {

            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }

    void BindDataGrid()
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
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            strCondition += " and (bill_main.billCode like @billcode)";
            listSp.Add(new SqlParameter("@billcode", "%" + TextBox1.Text.Trim() + "%"));
        }
        icount = getcount();
        DataTable dtrel = GetData(arrpage[0], arrpage[1], listSp, strCondition);
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


    private int getcount()
    {
        int count = 0;

        string strsqlcount = @"select count(*) from bill_main,bill_qtbxmxb  where bill_qtbxmxb.billCode=bill_main.billCode";

        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");

        strsqlcount += " and billDept in (" + deptCodes + ") and flowID='qtbx' and stepID='end' and isnull(bill_qtbxmxb.sfgf,'0')='0'";


        #region
        List<SqlParameter> listSp = new List<SqlParameter>();


        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            strsqlcount += " and (bill_main.billCode like @billcode)";
            listSp.Add(new SqlParameter("@billcode", "%" + TextBox1.Text.Trim() + "%"));
        }



        #endregion

        return count = int.Parse(server.GetCellValue(strsqlcount, listSp.ToArray()));

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagefrm"></param>
    /// <param name="pageto"></param>
    /// <param name="paramter"></param>
    /// <param name="strCondition"></param>
    /// <returns></returns>
    private DataTable GetData(int pagefrm, int pageto, List<SqlParameter> paramter, string strCondition)
    {
        // by a.billDate
        string sql = @"select  Row_Number()over(order by billdate desc) as crow,stepid as stepID_ID,(select deptName from bill_departments where deptCode=billDept) as billDept,
(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else 
(select steptext from bill_workFlowStep where bill_workFlowStep.flowID='qtbx' and bill_workFlowStep.stepid=bill_main.stepid ) end) as
stepID,bill_main.billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser)
as billUser,billdate,billje from bill_main,bill_qtbxmxb  where bill_qtbxmxb.billCode=bill_main.billCode
 ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");

        sql += " and billDept in (" + deptCodes + ") and flowID='qtbx' and stepID='end' and isnull(bill_qtbxmxb.sfgf,'0')='0'";

        sql += strCondition;
        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by billdate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, paramter.ToArray());
    }



    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待修改的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('bxDetail.aspx?type=look&billCode=" + billGuid + "');", true);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
    
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待给付的报销的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能给付一个报销项！');", true);
        }
        else
        {
            if (server.ExecuteNonQuery("update bill_qtbxmxb set sfgf='1',gfr='" + Session["userCode"].ToString().Trim() + "',gfsj=getdate() where billCode='" + billCode + "'") == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('给付失败！');", true);
            }
            else
            {
                this.BindDataGrid();
            }
        }
    }
    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待查看的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../bxgl/qtbxDetail.aspx?type=look&billCode=" + billGuid + "');", true);
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待查看的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=qtbx&billCode=" + billGuid + "');", true);
        }
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

}