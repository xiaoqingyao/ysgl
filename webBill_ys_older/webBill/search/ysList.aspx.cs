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
using System.Text;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_search_ysList :BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strDeptCode = "";
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
                string strsql = "select distinct nian from bill_ysgc order by nian desc";
                DataTable dtrel = server.GetDataTable(strsql, null);
                this.ddlDate.DataSource = dtrel;
                this.ddlDate.DataTextField = "nian";
                this.ddlDate.DataValueField = "nian";
                this.ddlDate.DataBind();
                this.BindDataGrid();
            }

            ClientScript.RegisterArrayDeclaration("arrFyKm", GetYskmByDep());
            object objDept = Request["deptCode"];
            if (objDept != null)
            {
                string strdep = objDept.ToString();
                string strSql = "select '['+deptCode+']'+deptName from bill_departments where deptCode='" + strdep + "'";
                object objRel = server.ExecuteScalar(strSql);
                this.lblDept.Text = objRel == null ? "" : "当前部门：" + objRel.ToString();
            }
        }
    }

    void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight,150);
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
        string sql = "select Row_Number()over(order by billName desc,billdate desc) as crow,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='ys' and bill_workFlowStep.stepid=bill_main.stepid ) end) as stepID,billCode,billName as billNameCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,billdate,billje from bill_main where flowID='ys'";
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
        if (ddlDate.SelectedValue != null)
        {
            sql += " and left(billname,4)='" + ddlDate.SelectedValue + "'";
        }
        
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }



    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    
    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void btn_CX_Click(object sender, EventArgs e)
    {
        if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
        {
            strDeptCode = Page.Request.QueryString["deptCode"].ToString().Trim();

        }
        if (strDeptCode != "" && strDeptCode != null)
        {

            if (txtKm.Text != "" && txtKm.Text != null)
            {
                string strkmcode = txtKm.Text;
                strkmcode = strkmcode.Substring(1, strkmcode.IndexOf("]") - 1);

                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/yskmtjlist.aspx?deptcode=" + strDeptCode + "&kmcode=" + strkmcode + "');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查询部门！');", true);
        }
        // this.BindDataGrid();
    }
    


    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string gcbh = "";
        string shyj = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                gcbh = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查看的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能查看一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/ystbDetailLook.aspx?gcbh=" + gcbh + "&billCode=" + billCode + "');", true);
        }
    }
    private string GetYskmByDep()
    {

        string sql = " select '['+yskmCode+']'+yskmMc as yskm from Bill_Yskm where  1=1";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() != "")
        {
            strDeptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
            sql += " and yskmcode in(select yskmcode from bill_yskm_dept where deptCode ='" + strDeptCode + "')";
        }



        DataTable dtRel = server.RunQueryCmdToTable(sql);
        StringBuilder sb = new StringBuilder();
        foreach (DataRow dr in dtRel.Rows)
        {
            sb.Append("'");
            sb.Append(Convert.ToString(dr["yskm"]));
            sb.Append("',");
        }
        if (sb.Length > 0)
        {
            return sb.ToString().Substring(0, sb.Length - 1);
        }
        else
        {
            return "";
        }

    }

    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            string billcode = e.Item.Cells[1].Text;
            WorkFlowRecordManager bll = new WorkFlowRecordManager();

            if (e.Item.Cells[7].Text == "end")
            {
                e.Item.Cells[7].Text = "审批通过";
            }
            else
            {
                string state = bll.WFState(billcode);
                e.Item.Cells[7].Text = state;
            }
        }
    }
}
