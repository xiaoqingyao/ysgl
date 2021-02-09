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
using WorkFlowLibrary.WorkFlowBll;
using System.Text;
using Models;
using Bll.UserProperty;
using System.Collections.Generic;
using System.IO;
using Bll;

public partial class webBill_search_bxList : BasePage
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
                //this.txtDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.txtDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.Button1.Attributes.Add("onclick", "javascript:selectry('../select/userFrame.aspx');return false;");

                this.BindDataGrid();
            }
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("subjectTags", GetYSSubjectAll());
        ClientScript.RegisterArrayDeclaration("gkDEPTTags", GetGKDeptAll());
    }

    void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
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



        //string sql = getSelectSql();
        //DataSet temp = server.GetDataSet(sql);



        //if (temp.Tables[0].Rows.Count == 0)
        //{
        //    temp = null;
        //}
        //this.myGrid.DataSource = temp;
        //this.myGrid.DataBind();
    }

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        //,case isnull((select top 1  rdState from workflowrecord  where billCode=bill_main.billCode),0) 
        //when '3' then '驳回'  when '1' then '审核中' when '2' then '审核通过'   else '未提交' end as 审核

        string sql = @"select Row_Number()over(order by bill_main.billdate desc) as crow , bill_main.billCode,billname as 单据编号
        ,bill_main.stepid as stepid ,userName as 报销人,
            bill_main.billdate as 报销日期,deptName as 报销单位,bxzy 摘要,bxsm 报销说明,'['+fykm+']'+yskmMc as 科目,je as 金额,(select deptName from bill_departments where deptCode = gkdept )as gkdept 
            from bill_main left join bill_ybbxmxb on bill_main.billcode=bill_ybbxmxb.billcode
            left join bill_ybbxmxb_fykm on bill_main.billcode=bill_ybbxmxb_fykm.billcode
            left join bill_users on userCode=bill_main.billuser
            left join bill_departments on bill_departments.deptcode=bill_main.billdept
            left join bill_yskm on bill_yskm.yskmcode=fykm 
            left join workflowrecord on bill_main.billCode=workflowrecord.billCode
            where bill_main.flowID in ('ybbx','qtbx')";
//        string sql = @"select Row_Number()over(order by bill_main.billdate desc) as crow , bill_main.billCode,billname as 单据编号,case stepid when 'end' then '审核通过' else '审核中' end as 审核,userName as 报销人,
//            bill_main.billdate as 报销日期,deptName as 报销单位,bxzy 摘要,bxsm 报销说明,'['+fykm+']'+yskmMc as 科目,je as 金额,(select deptName from bill_departments where deptCode = gkdept )as gkdept 
//            from bill_main left join bill_ybbxmxb on bill_main.billcode=bill_ybbxmxb.billcode
//            left join bill_ybbxmxb_fykm on bill_main.billcode=bill_ybbxmxb_fykm.billcode
//            left join bill_users on userCode=bill_main.billuser
//            left join bill_departments on bill_departments.deptcode=bill_main.billdept
//            left join bill_yskm on bill_yskm.yskmcode=fykm 
//            where flowID in ('ybbx','qtbx')";
        //单位
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and bill_main.billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and bill_main.billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),bill_main.billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),bill_main.billdate,121)<='" + strDateTo + "'";
        }
        //报销人
        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and bill_main.billUser='" + strBxr + "'";
        }
        

        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
             sql += " and bill_ybbxmxb_fykm.fykm='" + strSubject + "'";
            //sql += " and bill_main.billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
      

        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {

                //sql += " and bill_main.billcode not in (select billcode from workflowrecord)";
            }
            else if (strStatus.Equals("1"))
            {
                sql += "  and workflowrecord.rdState='1' and bill_main.stepid!='end'";
                //sql += " and bill_main.billcode in (select billcode from workflowrecord where rdState='1')";
            }
            else if(strStatus=="2")
            {
                sql += " and bill_main.stepid='end'";
                //sql += " and bill_main.billcode in (select billcode from workflowrecord where rdState='2')";
            }
        }
        string strGKDept = this.txtGKDept.Text.Trim();
        if (!strGKDept.Equals(""))
        {
            strGKDept = strGKDept.Substring(1, strGKDept.IndexOf("]") - 1);
            sql += " and bill_main.gkdept='" + strGKDept + "'";
        }
      
      
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
      //  Response.Write(strsqlframe);
        return server.GetDataTable(strsqlframe, null);
    }


    private string getSelectSql()
    {
        //,case isnull((select top 1  rdState from workflowrecord  where billCode=bill_main.billCode),0)
        //when '3' then '驳回'  when '1' then '审核中' when '2' then '审核通过'   else '未提交' end as 审核 , bill_main.stepid as stepid
        string sql = @"select bill_main.billCode,billname as 单据编号
            ,userName as 报销人,
            bill_main.billdate as 报销日期,deptName as 报销单位,bxzy 摘要,bxsm 报销说明,'['+fykm+']'+yskmMc as 科目,je as 金额,(select deptName from bill_departments where deptCode = gkdept )as gkdept 
            from bill_main left join bill_ybbxmxb on bill_main.billcode=bill_ybbxmxb.billcode
            left join bill_ybbxmxb_fykm on bill_main.billcode=bill_ybbxmxb_fykm.billcode
            left join bill_users on userCode=bill_main.billuser
            left join bill_departments on bill_departments.deptcode=bill_main.billdept
            left join bill_yskm on bill_yskm.yskmcode=fykm 
            left join workflowrecord on bill_main.billCode=workflowrecord.billCode
            where bill_main.flowID in ('ybbx','qtbx')";
        //单位
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and bill_main.billDept in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and bill_main.billDept in (" + deptCodes + ")";
        }
        //日期frm
        string strDateFrm = this.txtDateFrm.Text.Trim();
        if (!strDateFrm.Equals(""))
        {
            sql += " and convert(varchar(10),bill_main.billdate,121)>='" + strDateFrm + "'";
        }
        //日期to
        string strDateTo = this.txtDateTo.Text.Trim();
        if (!strDateTo.Equals(""))
        {
            sql += " and convert(varchar(10),bill_main.billdate,121)<='" + strDateTo + "'";
        }
        //报销人
        if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        {
            string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

            sql += " and bill_main.billUser='" + strBxr + "'";
        }
        //string strBxr = this.txtBxr.Text.Trim();
        //if (!strBxr.Equals(""))
        //{
        //    strBxr = strBxr.Substring(1, strBxr.IndexOf("]") - 1);
        //    sql += " and bill_main.billUser='" + strBxr + "'";
        //}

        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and bill_ybbxmxb_fykm.fykm='" + strSubject + "'";
            //sql += " and bill_main.billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        }
        //string strSubject = this.txtSubject.Text.Trim();
        //if (!strSubject.Equals(""))
        //{
        //    strSubject = strSubject.Substring(1, strSubject.IndexOf("]") - 1);
        //    sql += " and bill_main.billcode in (select billcode from bill_ybbxmxb_fykm where fykm = '" + strSubject + "') ";
        //}

        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!strStatus.Equals(""))
        {

            if (strStatus.Equals("-1"))
            {

                //sql += " and bill_main.billcode not in (select billcode from workflowrecord)";
            }
            else if (strStatus.Equals("2"))
            {
                
                sql += " and bill_main.stepid!='end'";
                //sql += " and bill_main.billcode in (select billcode from workflowrecord where rdState='1')";
            }
            else if(strStatus=="1")
            {   
            sql+=" and workflowrecord.rdState='1'";
               // sql += " and bill_main.stepid='end'";
                //sql += " and bill_main.billcode in (select billcode from workflowrecord where rdState='2')";
            }
        }
        string strGKDept = this.txtGKDept.Text.Trim();
        if (!strGKDept.Equals(""))
        {
            strGKDept = strGKDept.Substring(1, strGKDept.IndexOf("]") - 1);
            sql += " and bill_main.gkdept='" + strGKDept + "'";
        }
        sql += " order by bill_main.billdate desc";
        return sql;
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../bxgl/bxDetailFinal.aspx?type=look&billCode=" + billCode + "');", true);
    }

    protected void Button5_Click(object sender, EventArgs e)
    {

        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=ybbx&billCode=" + billCode + "');", true);
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button6_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../YbbxPrint/YbbxPrint.aspx?billCode=" + billCode + "');", true);
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string billcode = e.Item.Cells[0].Text;//报销单号
            string zt = e.Item.Cells[6].Text;//状态

            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[6].Text = state;
            }
        }
    }
    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExport_Click(object sender, EventArgs e)
    {
        string sql = getSelectSql();
        DataTable dtExport = new DataTable();
        dtExport = server.GetDataSet(sql).Tables[0];
        new ExcelHelper().ExpExcel(dtExport, "报销明细表", null);

        //this.DataTableToExcel(dtExport, this.myGrid, null);
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
    private string GetGKDeptAll()
    {
        string ret = "";
        SysManager smgr = new SysManager();
        IList<Bill_Departments> list = smgr.GetAllRootDept();
        StringBuilder sb = new StringBuilder();
        //sb.Append("[");
        foreach (Bill_Departments dept in list)
        {
            sb.Append("\"[" + dept.DeptCode + "]" + dept.DeptName + "\",");
        }
        sb.Remove(sb.Length - 1, 1);
        //sb.Append("]");
        ret = sb.ToString();
        return ret;
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}
