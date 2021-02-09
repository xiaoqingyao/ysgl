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
using Bll;
using System.IO;
using System.Text;
using Bll.UserProperty;
using System.Collections.Generic;
using Models;

public partial class webBill_search_kmystzCx : BasePage
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
    }

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
      
            
//        @"select * from (select  Row_Number()over(order by a.billname   desc) as crow,a.billcode,a.billname,
//        (select '['+deptname+']'+deptcode from bill_departments where deptcode=a.billdept)as deptname,a.billdept as billdept,
//        (select '['+username+']'+usercode from bill_users where usercode=a.billuser)as billuser,
//        (select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=b.yskm) as yskm,b.ysje as tzje,
//        convert(varchar(10),a.billDate,121) as billDate,
//        (select xmmc from bill_ysgc where gcbh=b.gcbh)as ysgc,
//        case isnull((select top 1 rdState from workflowrecord  where billCode=a.billCode),0) when '3' then '驳回'  when '1' then '审核中' when '2' then '审核通过'   else '未提交' end as  sh
//         from bill_main a ,bill_ysmxb b  where a.billcode=b.billcode and a. flowid='kmystz' ) t where 1=1 ";

        //if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        //{
        //    string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //    sql += " and  billDept in (" + deptCodes + ")";
        //}
        //else
        //{
        //    string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
        //    sql += " and billDept in (" + deptCodes + ")";
        //}
        ////日期frm
        //string strDateFrm = this.txtDateFrm.Text.Trim();
        //if (!strDateFrm.Equals(""))
        //{
        //    sql += " and convert(varchar(10),billdate,121)>='" + strDateFrm + "'";
        //}
        ////日期to
        //string strDateTo = this.txtDateTo.Text.Trim();
        //if (!strDateTo.Equals(""))
        //{
        //    sql += " and convert(varchar(10),billdate,121)<='" + strDateTo + "'";
        //}
        ////报销人
        //if (!string.IsNullOrEmpty(this.txtBxr.Text.Trim()))
        //{
        //    string strBxr = (new PublicServiceBLL()).SubSting(this.txtBxr.Text.Trim());

        //    sql += " and billUser like'%" + strBxr + "%'";
        //}


        ////科目
        //if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        //{
        //    string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
        //    sql += " and yskm  like '%" + strSubject + "%'";
        //}


        ////审核状态
        //string strStatus = this.ddlStatus.SelectedValue;
        //if (!string.IsNullOrEmpty(strStatus))
        //{
        //    sql += " and sh='" + this.ddlStatus.SelectedItem.Text + "' ";

        //}
        string sql = getSelectSql();
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);

        return server.GetDataTable(strsqlframe, null);


    }


    private string getSelectSql()
    {
        string sql = @"select * from (select  Row_Number()over(order by a.billname   desc) as crow,a.billcode,a.billname,
        (select '['+deptname+']'+deptcode from bill_departments where deptcode=a.billdept)as deptname,a.billdept as billdept,
        (select '['+username+']'+usercode from bill_users where usercode=a.billuser)as billuser,
        (select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=b.yskm) as yskm,b.ysje as tzje,
        convert(varchar(10),a.billDate,121) as billDate,
        (select xmmc from bill_ysgc where gcbh=b.gcbh)as ysgc,
        case isnull((select top 1 rdState from workflowrecord  where billCode=a.billCode),0) when '3' then '驳回'  when '1' then '审核中' when '2' then '审核通过'   else '未提交' end as  sh
         from bill_main a ,bill_ysmxb b  where a.billcode=b.billcode and a. flowid='kmystz' ) t where 1=1 ";

        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and  billDept in (" + deptCodes + ")";
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

            sql += " and billUser like'%" + strBxr + "%'";
        }


        //科目
        if (!string.IsNullOrEmpty(this.txtSubject.Text.Trim()))
        {
            string strSubject = (new PublicServiceBLL()).SubSting(this.txtSubject.Text.Trim());
            sql += " and yskm  like '%" + strSubject + "%'";
        }


        //审核状态
        string strStatus = this.ddlStatus.SelectedValue;
        if (!string.IsNullOrEmpty(strStatus))
        {
            sql += " and sh='" + this.ddlStatus.SelectedItem.Text + "' ";

        }
        return sql;
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value.ToString().Trim();
        string billDept = hd_billDept.Value.ToString().Trim();
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + billCode + "&deptcode=" + billDept + "');", true);
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
        Dictionary<string, string> dic = new Dictionary<String, String>();
        dic.Add("billname", "单据编号");
        dic.Add("billuser", "报销人");
        dic.Add("billdate", "制单日期");
        dic.Add("deptname", "所属部门");
        dic.Add("yskm", "科目");
        dic.Add("tzje", "调整金额");
        dic.Add("sh", "状态");
        new ExcelHelper().ExpExcel(dtExport, "ExportFile", dic);
       
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
