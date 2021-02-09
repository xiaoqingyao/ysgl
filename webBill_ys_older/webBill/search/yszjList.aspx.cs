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
using System.Data.SqlClient;
using System.Text;
using Bll;
using System.Collections.Generic;

public partial class webBill_search_yszjList : BasePage
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
            //this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
            //this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
            object objDeptCode = Request["deptCode"];
            if (objDeptCode != null)
            {
                strDeptCode = objDeptCode.ToString();
            }
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
            ClientScript.RegisterArrayDeclaration("arrFyKm", GetYskmByDep(strDeptCode));
            object objDept = Request["deptCode"];
            if (objDept != null)
            {
                string strdep = objDept.ToString();
                string strSql = "select '['+deptCode+']'+deptName from bill_departments where deptCode='" + strdep + "'";
                object objRel = server.ExecuteScalar(strSql);
                this.lblDept.Text = objRel == null ? "" : "当前部门：" + objRel.ToString();
            }
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("subjectTags", GetYSSubjectAll(strDeptCode));
    }

    void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 150);
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
        string sql = @"select Row_Number()over(order by main.billname desc) as crow ,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept) as billDept
,main.stepid,main.billCode,main.billName as billNameCode
, (case when ISNULL((select xmmc from bill_ysgc where gcbh=main.billName),'')='' then main.billName else (select xmmc from bill_ysgc where gcbh=main.billName)  end)as billName
,(select username from bill_users where usercode=main.billuser) as billUser
,main.billdate,main.billje
from bill_main main where main.flowID='yszj'";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and main.billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and main.billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),main.billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),main.billdate,121)<='" + strDateTo + "'";
        }
        //报销人
        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and main.billUser='" + strBxr + "'";
        }

        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and main.billUser='" + strBxr + "'";
        //}
        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and main.billcode in (select billcode from bill_ysmxb where yskm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and main.billcode in (select billcode from bill_ysmxb where yskm = '" + strSubject + "') ";
        //}
        //报销单号
        string strBillCode = this.txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and main.billName like '%" + strBillCode + "%' ";
        }
        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {
                sql += " and billcode not in (select billcode from workflowrecord)";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='D' ";
            }
            else if (strStatus.Equals("1"))
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='1')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='1'";
            }
            else
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='2')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='2'";
            }
        }
        //单据来源
        string strBillFrm = this.ddlBillFrm.SelectedValue;
        if (!strBillFrm.Equals(""))
        {
            if (strBillFrm.Equals("1"))//录入请求 and isnull(main.billUser,'')!=''
            {
                sql += " and isnull(main.billUser,'')!=''";
            }
            else
            {//系统生成
                sql += " and isnull(main.billUser,'')=''";
            }
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
                select Row_Number()over(order by main.billname desc) as crow ,main.stepid,main.billCode,main.billName as billNameCode,
                (select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept) as billDept,
                (case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' 
                when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='yszj' and bill_workFlowStep.stepid=main.stepid ) end) as status,
                (select xmmc from bill_ysgc where gcbh=main.billName) as billName,(select username from bill_users where usercode=main.billuser) as billUser
                ,main.billdate,main.billje from bill_main main where main.flowID='yszj' ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and main.billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and main.billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),main.billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),main.billdate,121)<='" + strDateTo + "'";
        }
        //报销人
        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and main.billUser='" + strBxr + "'";
        }

        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and main.billUser='" + strBxr + "'";
        //}
        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and main.billcode in (select billcode from bill_ysmxb where yskm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and main.billcode in (select billcode from bill_ysmxb where yskm = '" + strSubject + "') ";
        //}
        //报销单号
        string strBillCode = this.txtBillCode.Text.Trim();
        if (!string.IsNullOrEmpty(strBillCode))
        {
            sql += " and main.billName like '%" + strBillCode + "%' ";
        }
        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {
                sql += " and billcode not in (select billcode from workflowrecord)";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='D' ";
            }
            else if (strStatus.Equals("1"))
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='1')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='1'";
            }
            else
            {
                sql += " and billcode in (select billcode from workflowrecord where rdState='2')";
                //sql += " and isnull((select rdState from workflowrecord where billcode=billCode),'D')='2'";
            }
        }
        //单据来源
        string strBillFrm = this.ddlBillFrm.SelectedValue;
        if (!strBillFrm.Equals(""))
        {
            if (strBillFrm.Equals("1"))//录入请求 and isnull(main.billUser,'')!=''
            {
                sql += " and isnull(main.billUser,'')!=''";
            }
            else
            {//系统生成
                sql += " and isnull(main.billUser,'')=''";
            }
        }




        return sql;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];
        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("billName", "预算过程");
        dic.Add("billUser", "制单人");
        dic.Add("billDate", "制单日期");
        dic.Add("billJe", "追加金额");
        dic.Add("status", "完成审核");
        dic.Add("billDept", "部门");
        new ExcelHelper().ExpExcel(dtExport, "yszjlist", dic);

        //  DataTableToExcel(dtExport, this.myGrid, null);
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    /// <summary>
    /// 查看详细信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button3_Click(object sender, EventArgs e)
    {
        string billCode = "";
      
        int count = 0;
        string gcbh = "";
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

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个预算追加单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查看的预算追加单！');", true);
        }
        else
        {
            if (gcbh.Substring(0,3)=="pl_")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/yszjAdd_bykm.aspx?type=look&billCode=" + billCode + "');", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/yszjEdit.aspx?type=look&gcbh=" + gcbh + "&billCode=" + billCode + "');", true);

            }

        }
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button4_Click(object sender, EventArgs e)
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
                string billcode = e.Item.Cells[1].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[7].Text = state;
            }

        }
    }
    /// <summary>
    /// 根据科目统计
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_CX_Click(object sender, EventArgs e)
    {
        if (strDeptCode != "" && strDeptCode != null)
        {

            if (txtKm.Text != "" && txtKm.Text != null)
            {
                string strkmcode = txtKm.Text;
                strkmcode = strkmcode.Substring(1, strkmcode.IndexOf("]") - 1);

                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/yszjkmtj.aspx?deptcode=" + strDeptCode + "&kmcode=" + strkmcode + "');", true);
            }
        }
        else
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查询部门！');", true);
        }

        // this.BindDataGrid();
    }
    private string GetYskmByDep(string depcode)
    {
        string sql = " select '['+yskmCode+']'+yskmMc as yskm from Bill_Yskm where yskmcode in(select yskmcode from bill_yskm_dept where deptcode=@deptcode)";
        SqlParameter[] sp = { new SqlParameter("@deptcode", depcode) };
        DataTable dtRel = server.GetDataTable(sql, sp);
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
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
    private string GetYSSubjectAll(string strdept)
    {
        DataSet ds = server.GetDataSet("select '['+yskmCode+']'+yskmMc as kemu from  bill_yskm");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kemu"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
}
