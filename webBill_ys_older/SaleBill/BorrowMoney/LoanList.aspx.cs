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
using Bll.Sepecial;
using Bll.Bills;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class SaleBill_BorrowMoney_LoanList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    //Bll.Sepecial.RemittanceBll Remitbill = new Bll.Sepecial.RemittanceBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            if (!IsPostBack)
            {

                this.txtLoanDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtLoanDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtRepsonDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtRepsonDateFrom.Attributes.Add("onfocus", "javascript:setday(this);");
                this.DrCJstatus.SelectedValue = "0";
                this.BindDataGrid();
            }
        }

    }

    void BindDataGrid()
    {

        //Models.T_LoanList model = new Models.T_LoanList();
        ////借款日期从
        //if (txtLoanDateFrm.Text != null && txtLoanDateFrm.Text != "")
        //{
        //    model.LoanDate = txtLoanDateFrm.Text;
        //}
        ////借款日期末
        //if (txtLoanDateTo.Text != null && txtLoanDateTo.Text != "")
        //{
        //    model.NOTE20 = txtLoanDateTo.Text;
        //}
        ////申请单号
        //if (txtOrderCode.Text != null && txtOrderCode.Text != "")
        //{
        //    model.Listid = txtOrderCode.Text;
        //}
        ////经办日期从
        //if (txtRepsonDateTo.Text != null && txtRepsonDateTo.Text != "")
        //{
        //    model.ResponsibleDate = txtRepsonDateFrom.Text;
        //}
        ////经办日期止
        //if (txtRepsonDateFrom.Text != null && txtRepsonDateFrom.Text != "")
        //{
        //    model.NOTE19 = txtRepsonDateTo.Text;
        //}
        ////经办人
        //if (txtRepsonCode.Text != null && txtRepsonCode.Text != "")
        //{
        //    string strReponCode = this.txtRepsonCode.Text.Trim();
        //    strReponCode = strReponCode.Substring(1, strReponCode.IndexOf("]") - 1).Trim();
        //    model.ResponsibleCode = strReponCode;
        //}
        ////借款人
        //if (txtloannamecode.Text != null && txtloannamecode.Text != "")
        //{
        //    string strloanCode = txtloannamecode.Text.Trim();
        //    strloanCode = strloanCode.Substring(1, strloanCode.IndexOf("]") - 1).Trim();
        //    model.LoanCode = strloanCode;
        //}
        ////缴款单位
        //if (txtLoanDeptCode.Text != null && txtLoanDeptCode.Text != "")
        //{
        //    string deptcode = txtLoanDeptCode.Text.Trim();
        //    deptcode = deptcode.Substring(1, deptcode.IndexOf("]") - 1).Trim();
        //    model.LoanDeptCode = deptcode;
        //}
        ////冲减状态
        //if (DrCJstatus.SelectedValue.ToString() != "")
        //{
        //    model.Status = DrCJstatus.SelectedValue.ToString();
        //}

        ////审批状态
        //if (DrSPstatus.SelectedValue.ToString() != "")
        //{
        //    model.NOTE3 = DrSPstatus.SelectedValue.ToString();
        //}
        //DataTable temp = loanbll.GetAllListBySql1(model, "yzsq");




        //if (temp.Rows.Count == 0)
        //{
        //    temp = null;
        //}


        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 80);
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


        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
    }
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select  a.billCode,a.billName,( case a.stepID when '-1' then '未提交' when 'end' then '审批通过'  end) as stepID,a.billDate,a.billDept,a.billJe,
(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName,
(select  '['+usercode+']'+ username from bill_users where usercode=
(select LoanCode from T_LoanList where Listid=a.billCode ) ) as loanName,
(select   '['+deptCode+']'+ deptName from bill_departments
 where deptCode=(select LoanDeptCode from T_LoanList where Listid=a.billCode ) ) as loanDeptName,
(select LoanDate from T_LoanList where Listid=a.billCode )as loandate,
(select ResponsibleDate from T_LoanList where Listid=a.billCode )as Respondate,
(case (select Status from T_LoanList where Listid=a.billCode) 
when '1' then '借款' when '2' then '结算完毕' when '3' then '冲减中'  end) as loanStatus,
(select convert(decimal(18,2),isnull(Note3,0)) from T_LoanList where Listid=a.billCode) as Note3,
(a.billJe-(select convert(decimal(18,2),isnull(Note3,0)) from T_LoanList where Listid=a.billCode))as wcjmoney,
( case (select SettleType from T_LoanList where Listid=a.billCode) 
when '0' then '现金' when '1' then '单据冲减'  end) as loanType,
(select CJCode from T_LoanList where Listid=a.billCode) as cjcode,
(select '['+usercode+']'+ username from bill_users where usercode=a.billuser) as jbname,Row_Number()over(order by  a.billDate desc) as crow
from  bill_main a   where  a.flowid='yzsq' ";

        List<SqlParameter> listSp = new List<SqlParameter>();

        //借款日期从
        if (txtLoanDateFrm.Text != null && txtLoanDateFrm.Text != "")
        {
            sql += " and a.billDate >=@begtime  ";
            listSp.Add(new SqlParameter("@begtime", txtLoanDateFrm.Text));

        }
        //借款日期末
        if (txtLoanDateTo.Text != null && txtLoanDateTo.Text != "")
        {
            sql += " and a.billDate <=@endtime  ";
            listSp.Add(new SqlParameter("@endtime", txtLoanDateTo.Text));
        }
        //申请单号
        if (txtOrderCode.Text != null && txtOrderCode.Text != "")
        {
            //model.Listid = txtOrderCode.Text;
            sql += " and a.billCode like @billCode  ";
            listSp.Add(new SqlParameter("@billCode", "%" + txtOrderCode.Text + "%"));
        }
        //经办日期从
        if (txtRepsonDateTo.Text != null && txtRepsonDateTo.Text != "")
        {
            sql += "   and (select ResponsibleDate from T_LoanList where Listid=a.billCode )<=@begResponsibleDate";

            listSp.Add(new SqlParameter("@begResponsibleDate", txtRepsonDateTo.Text));
        }
        //经办日期止
        if (txtRepsonDateFrom.Text != null && txtRepsonDateFrom.Text != "")
        {
            sql += "   and (select ResponsibleDate from T_LoanList where Listid=a.billCode )>=@endResponsibleDate";

            listSp.Add(new SqlParameter("@endResponsibleDate", txtRepsonDateFrom.Text));
        }
        //经办人
        if (txtRepsonCode.Text != null && txtRepsonCode.Text != "")
        {
            sql += "   and (select usercode from bill_users where usercode=a.billuser)=@jbname";

            string strReponCode = this.txtRepsonCode.Text.Trim();
            strReponCode = strReponCode.Substring(1, strReponCode.IndexOf("]") - 1).Trim();
            listSp.Add(new SqlParameter("@jbname", strReponCode));

        }
        //借款人
        if (txtloannamecode.Text != null && txtloannamecode.Text != "")
        {

            sql += "   and (select LoanCode from T_LoanList where Listid=a.billCode )=@loanName";

            string strloanCode = txtloannamecode.Text.Trim();
            strloanCode = strloanCode.Substring(1, strloanCode.IndexOf("]") - 1).Trim();
            listSp.Add(new SqlParameter("@loanName", strloanCode));
        }
        //缴款单位
        if (txtLoanDeptCode.Text != null && txtLoanDeptCode.Text != "")
        {
            sql += "   and (select deptCode from bill_departments where deptCode=a.billDept  )=@deptName";
            string deptcode = txtLoanDeptCode.Text.Trim();
            deptcode = deptcode.Substring(1, deptcode.IndexOf("]") - 1).Trim();
            listSp.Add(new SqlParameter("@deptName", deptcode));
        }
        //冲减状态
        if (DrCJstatus.SelectedValue.ToString() != "")
        {
            sql += "   and ( select Status from T_LoanList where Listid=a.billCode )=@Status";
            listSp.Add(new SqlParameter("@Status", DrCJstatus.SelectedValue.ToString()));
        }

        //审批状态
        if (DrSPstatus.SelectedValue.ToString() != "")
        {
            sql += " and a.stepID =@stepID  ";
            listSp.Add(new SqlParameter("@stepID", DrSPstatus.SelectedValue.ToString()));

        }

        //获取条数
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount, listSp.ToArray()));

        //返回查询结果
        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by a.billDate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, listSp.ToArray());

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 80);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
         DataTable dtExport = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;
        DataTableToExcel(dtExport, this.myGrid, null);
    }

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
    public delegate void MyDelegate(DataGrid gv);
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

    protected void Button6_Click1(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
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

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
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
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[7].Text;
            if (zt == "审批通过")
            {
                return;
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
    ///// <summary>
    ///// 结算
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btn_js_Click(object sender, EventArgs e)
    //{
    //    string strjszt = this.HiddenField1.Value.Trim();
    //    string billCode = hd_billCode.Value.ToString().Trim();
    //    if (strjszt == "" || billCode == "")
    //    {

    //        this.BindDataGrid();
    //        return;
    //    }
    //    if (strjszt=="结算")
    //    {

    //        this.BindDataGrid();
    //        return;
    //    }
    //    else
    //    {

    //        string strsqljs = "update T_LoanList set Status='2' where Listid='" + billCode + "'";
    //        int row = server.ExecuteNonQuery(strsqljs);
    //        if (row > 0)
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('已结算！');", true);
    //            this.BindDataGrid();

    //        }

    //    }

    //}
}