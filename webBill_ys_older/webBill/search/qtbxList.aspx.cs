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
using System.Text;
using System.IO;
using Bll;
using System.Collections.Generic;

public partial class webBill_search_qtbxList : BasePage
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
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("subjectTags", GetYSSubjectAll());
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
        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid1.DataSource = dtrel;
        this.myGrid1.DataBind();
        //string sql = getSelectSql();
        //DataSet temp = server.GetDataSet(sql);


        //if (temp.Tables[0].Rows.Count == 0)
        //{
        //    temp = null;
        //}
        //this.myGrid1.DataSource = temp;
        //this.myGrid1.DataBind();
    }


    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select  Row_Number()over(order by billdate desc) as crow ,(select bxzy from bill_qtbxmxb where bill_qtbxmxb.billCode=bill_main.billCode) as bxzy,stepid as stepID_ID,
                        (select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept) as billDept,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' 
                        when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='qtbx' and bill_workFlowStep.stepid=bill_main.stepid ) end) as stepID,
                        billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,billdate,billje,
                        (select (case sfgf when '0' then '未给付' when '1' then '已给付' end) as sfgf from bill_ybbxmxb where billcode=bill_main.billcode) as sfgf,
                        (select (select username from bill_users where usercode=gfr) as gfr from bill_ybbxmxb where billcode=bill_main.billcode) as gfr,
                        (select gfsj from bill_ybbxmxb where billcode=bill_main.billcode) as gfsj,(select cxyy from bill_ybbxmxb where billcode=bill_main.billcode) as cxyy,
                        (select (select username from bill_users where usercode=cxr) as cxr from bill_ybbxmxb where billcode=bill_main.billcode) as cxr,
                        (select cxsj from bill_ybbxmxb where billcode=bill_main.billcode) as cxsj  from bill_main where flowID='qtbx'";


        //单位
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
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)<='" + strDateTo + "'";
        }
        //报销人

        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and billUser='" + strBxr + "'";
        }
        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and billUser='" + strBxr + "'";
        //}

        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        //}
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
     
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }

    private string getSelectSql()
    {
        string sql = @"select (select bxzy from bill_qtbxmxb where bill_qtbxmxb.billCode=bill_main.billCode) as bxzy,stepid as stepID_ID,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept) as billDept,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' 
when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='qtbx' and bill_workFlowStep.stepid=bill_main.stepid ) end) as stepID,
billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,billdate,billje,
(select (case sfgf when '0' then '未给付' when '1' then '已给付' end) as sfgf from bill_ybbxmxb where billcode=bill_main.billcode) as sfgf,
(select (select username from bill_users where usercode=gfr) as gfr from bill_ybbxmxb where billcode=bill_main.billcode) as gfr,
(select gfsj from bill_ybbxmxb where billcode=bill_main.billcode) as gfsj,(select cxyy from bill_ybbxmxb where billcode=bill_main.billcode) as cxyy,
(select (select username from bill_users where usercode=cxr) as cxr from bill_ybbxmxb where billcode=bill_main.billcode) as cxr,
(select cxsj from bill_ybbxmxb where billcode=bill_main.billcode) as cxsj  from bill_main where flowID='qtbx'";
       
        //单位
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
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),billdate,121)<='" + strDateTo + "'";
        }
        //报销人

        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and billUser='" + strBxr + "'";
        }
        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and billUser='" + strBxr + "'";
        //}

        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        //}
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
        sql += " order by billdate desc";
        return sql;
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid1.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid1.Items[i].Cells[1].Text.ToString().Trim();
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../bxgl/bxDetailFinal.aspx?type=look&billCode=" + billGuid + "');", true);//qtbxDetail
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];

        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("billCode", "报销单号");
        dic.Add("billUser", "报销人");
        dic.Add("billDate", "申请日期");
        dic.Add("billDept", "所属部门");
        dic.Add("billJe", "报销总额");
        dic.Add("stepID", "状态");
        dic.Add("bxzy", "摘要");
        dic.Add("sfgf", "是否给付");
        dic.Add("gfr", "给付人");
        dic.Add("gfsj", "给付时间");
        dic.Add("cxr", "撤销人");
        dic.Add("cxsj", "撤销时间");
        dic.Add("cxyy", "撤销原因");

        new ExcelHelper().ExpExcel(dtExport, "qtbx", dic);

        //this.DataTableToExcel(dtExport, this.myGrid1, null);
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
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        string shyj = "";
        for (int i = 0; i <= this.myGrid1.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid1.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid1.Items[i].Cells[8].Text.ToString().Trim();
                shyj = ((TextBox)this.myGrid1.Items[i].FindControl("TextBox1")).Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待审核的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能审核一项！');", true);
        }
        else
        {
            (new workFlowLibrary.workFlow()).checkBills("qtbx", billCode, Session["userCode"].ToString().Trim(), System.DateTime.Now.ToString(), shyj, false);
            this.BindDataGrid();
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
    {

        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid1.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid1.Items[i].Cells[1].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查看的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=qtbx&billCode=" + billCode + "');", true);
        }
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button6_Click1(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        bool isBegin = false;
        for (int i = 0; i <= this.myGrid1.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billCode = this.myGrid1.Items[i].Cells[1].Text.ToString().Trim();

            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待打印的报销单！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../printer/printerqtbx.aspx?billCode=" + billCode + "');", true);
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //if (e.Item.ItemType != ListItemType.Header)
        //{
        //    e.Item.Cells[4].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[4].Text.ToString().Trim());
        //}
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
    private string GetYSSubjectAll()
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