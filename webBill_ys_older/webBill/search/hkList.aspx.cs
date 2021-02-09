using Bll.Bills;
using Bll.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_search_hkList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    decimal arreveColumnJkje = 0;
    decimal arreveColumnYcjje = 0;
    decimal arreveColumnWcjje = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            if (!IsPostBack)
            {

                this.BindDataGrid();
            }


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

        string sql = @"select Row_Number()over(order by  a.billCode desc) as crow 
                        ,b.listid,a.billCode,a.billName,stepId,  convert(varchar(10),a.billDate,121) as billDate
                        ,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName,a.billDept
                        ,a.billJe,(select '['+usercode+']'+ username from bill_users where usercode=a.billuser) as billUser
                        ,(select isnull(note3,0) from T_LoanList where listid =b.loancode )as ycjmoney
                        , (select LoanMoney-convert( decimal(18,2),isnull(note3,0))
                        from T_LoanList where listid =b.loancode )as wcjmoney  ,loancode 
                        from bill_main a inner join T_ReturnNote b   on a.billCode=b.billCode   
                        and left(b.loancode,4)='jksq'  and ltype='2'";// and a.billUser='" + Session["userCode"].ToString() + "'

        string strsqlwhere = getwhere();
        if (!string.IsNullOrEmpty(strsqlwhere))
        {
            sql += strsqlwhere;
        }
        DataTable dtsql = server.GetDataTable(sql, null);
     

    }
    public DataTable GetData(int pagefrm, int pageto, out int count)
    {

        #region GetSearchModel
        #endregion
        string sql = @"select Row_Number()over(order by  a.billCode desc) as crow 
                        ,b.listid,a.billCode,a.billName,stepId,  convert(varchar(10),a.billDate,121) as billDate
                        ,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName,a.billDept
                        ,a.billJe,(select '['+usercode+']'+ username from bill_users where usercode=a.billuser) as billUser
                        ,(select isnull(note3,0) from T_LoanList where listid =b.loancode )as ycjmoney
                        , (select LoanMoney-convert( decimal(18,2),isnull(note3,0))
                        from T_LoanList where listid =b.loancode )as wcjmoney  ,loancode 
                        from bill_main a inner join T_ReturnNote b   on a.billCode=b.billCode   
                        and left(b.loancode,4)='jksq'  and ltype='2'";// and a.billUser='" + Session["userCode"].ToString() + "'
  

        string strsqlwhere = getwhere();
        if (!string.IsNullOrEmpty(strsqlwhere))
        {
            sql += strsqlwhere;
        }
        string hkStr = "";

        string strsqlcount = "select count(*) from ( {0} ) t where 1=1 " + hkStr;
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} order by billdate desc " + hkStr;
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }


    protected string getwhere()
    {
        string strwhere = " ";
        
        //部门
        string strdept = "";
        object objDept = Request["deptCode"];
        if (objDept != null)
        {
            strdept = Request["deptCode"].ToString();
            if (!string.IsNullOrEmpty(strdept))
            {
                strwhere += " and a.billDept='" + strdept + "'";
            }
        }
       
        
        //借款日期从
        string temp = txtLoanDateFrm.Text;
        if (!string.IsNullOrEmpty(temp))
        {
            strwhere += " and a.billDate>='" + temp + "'";
        }
        //借款日期末
        temp = txtLoanDateTo.Text;
        if (!string.IsNullOrEmpty(temp))
        {
            strwhere += " and a.billDate<='" + temp + "'";
        }
        //申请单号
        temp = txtcode.Text;
        if (!string.IsNullOrEmpty(temp))
        {
            strwhere += " and a.billCode  like '%" + temp + "%'";
        }
        //借款人
        temp = SubString(txtbilluser.Text.Trim());
        if (!string.IsNullOrEmpty(temp))
        {
            strwhere += " and a.billUser  like '%" + temp + "%'";
        }
        return strwhere;

    }

    protected void Button6_Click1(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }


    decimal jkje = 0;
    decimal ycjje = 0;
    decimal wcjje = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            string zt = e.Item.Cells[5].Text;
            if (zt == "end")
            {
                e.Item.Cells[5].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[5].Text = state;
            }
            string jkcode = e.Item.Cells[1].Text;
            string strurl = "../../SaleBill/BorrowMoney/FundBorrowDetail.aspx?Ctrl=look&Code=" + jkcode;
            e.Item.Cells[1].Text = "<a href=\"#\" style=\" color:blue; text-decoration:underline\" onclick=\"openDetail('" + strurl + "')\">" + jkcode + "</a>";
        }
    }

    private int GetDiff(string dateTime, string jksj)
    {
        if (!string.IsNullOrEmpty(dateTime) && !string.IsNullOrEmpty(jksj))
        {

            DateTime dt1 = DateTime.Now;
            DateTime dt2 = Convert.ToDateTime(dateTime).AddDays(Convert.ToInt32(jksj));
            TimeSpan ts = dt1 - dt2;
            int sub = ts.Days;     //sub就是两天相差的天数
            ////比较两个时间，如果比较的值小于0则dt1比dt2小，如果大于则dt1比dt2大，如果等于，则dt1等于dt2
            //if (DateTime.Compare(dt1, dt2) < 0)
            //{
            //sub=sub*(-1);
            //}
            return sub;
        }
        else
        {
            return -1;
        }

    }


    protected void ToExcel_click(object sender, EventArgs e)
    {
        DataGrid2Excel(myGrid);
    }
    public static void DataGrid2Excel(System.Web.UI.WebControls.DataGrid dgData)
    {
        // 当前对话 
        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件 
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dgData != null)
        {
            // 设置编码和附件格式 
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.UTF8;
            curContext.Response.Charset = "";

            // 导出excel文件 
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 返回客户端     
            dgData.RenderControl(htmlWriter);
            curContext.Response.Write(strWriter.ToString());
            curContext.Response.End();
        }
    }

    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
    }
}