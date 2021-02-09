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
using WorkFlowLibrary.WorkFlowBll;
using System.Collections.Generic;

public partial class webBill_search_ystzList : BasePage
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
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 120);
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

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select Row_Number()over(order by billDate desc) as crow ,(select xmmc from bill_ysgc
            where gcbh=(select top 1 gcbh from bill_ysmxb where billCode=bill_main.billCode and ysje<=0)) as fromGc
            ,(select xmmc from bill_ysgc where gcbh=(select top 1 gcbh from bill_ysmxb where billCode=bill_main.billCode and ysje>=0)) 
            as toGc,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid as stepID,billCode
            ,(select username from bill_users where usercode=billuser) as billUser,billdate,billje from bill_main where flowID='ystz' ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and billDept in (" + deptCodes + ")";
        }

        //日期frm
        string strDateFrm = this.txb_sqrqbegin.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txb_sqrqend.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10), billdate,121)<='" + strDateTo + "'";
        }

        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }



    private string getSelectSql()
    {
        string sql = @"
           select Row_Number()over(order by billDate desc) as crow ,(select xmmc from bill_ysgc
            where gcbh=(select top 1 gcbh from bill_ysmxb where billCode=main.billCode and ysje<=0)) as fromGc
            ,(select xmmc from bill_ysgc where gcbh=(select top 1 gcbh from bill_ysmxb where billCode=main.billCode and ysje>=0)) 
            as toGc,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' 
           when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='ystz' and bill_workFlowStep.stepid=main.stepid ) end) as status,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid as stepID,billCode
            ,(select username from bill_users where usercode=billuser) as billUser,billdate,billje from bill_main main where flowID='ystz' ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and billDept in (" + deptCodes + ")";
        }

        //日期frm
        string strDateFrm = this.txb_sqrqbegin.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txb_sqrqend.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10), billdate,121)<='" + strDateTo + "'";
        }


        return sql;
    }
    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("billDept", "填报单位");
        dic.Add("billUser", "制单人");
        dic.Add("billDate", "制单日期");
        dic.Add("fromGc", "源预算过程");
        dic.Add("toGc", "目标预算过程");
        dic.Add("billje", "金额");
        dic.Add("status", "完成审核");
        new ExcelHelper().ExpExcel(dtExport, "ystzlist", dic);

        //  DataTableToExcel(dtExport, this.myGrid, null);
    }


    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    /*
    protected void Button8_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[8].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个预算调整单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的预算调整单！');", true);
        }
        else
        {
            Response.Redirect("../ysgl/ystzDetailLook.aspx?type=search&deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim() + "&billCode=" + billCode);

        }
    }
     */
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
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
}