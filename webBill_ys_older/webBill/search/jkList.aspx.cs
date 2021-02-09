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
using Bll.Bills;
using Bll.UserProperty;
using Models;

public partial class webBill_search_jkList : BasePage
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
        Models.T_LoanList model = getmodel();//new Models.T_LoanList();

        string sql = loanbll.GetSql(model, "jksq");


        DataTable dtsql = server.GetDataTable(sql, null);
        for (int j = 0; j < dtsql.Rows.Count; j++)
        {
            if (!string.IsNullOrEmpty(dtsql.Rows[j]["billJe"].ToString()))
            {
                arreveColumnJkje += decimal.Parse(dtsql.Rows[j]["billJe"].ToString());
            }
            if (!string.IsNullOrEmpty(dtsql.Rows[j]["Note3"].ToString()))
            {
                arreveColumnYcjje += decimal.Parse(dtsql.Rows[j]["Note3"].ToString());
            }
            if (!string.IsNullOrEmpty(dtsql.Rows[j]["wcjmoney"].ToString()))
            {
                arreveColumnWcjje += decimal.Parse(dtsql.Rows[j]["wcjmoney"].ToString());
            }
        }
        lbl_jkje.Text = "借款总金额：" + arreveColumnJkje.ToString();
        lbl_yhje.Text = "已冲减总金额：" + arreveColumnYcjje.ToString();
        lbl_whje.Text = "未冲减总金额：" + arreveColumnWcjje.ToString();

    }
    public DataTable GetData(int pagefrm, int pageto, out int count)
    {

        #region GetSearchModel
        Models.T_LoanList model = getmodel();//new Models.T_LoanList();


        #endregion
        string sql = loanbll.GetSql(model, "jksq");
        string hkStr = "";

        string strsqlcount = "select count(*) from ( {0} ) t where 1=1 " + hkStr;
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} order by billdate desc " + hkStr;
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }


    protected T_LoanList getmodel()
    {
        Models.T_LoanList model = new Models.T_LoanList();
        //借款日期从
        if (txtLoanDateFrm.Text != null && txtLoanDateFrm.Text != "")
        {
            model.LoanDate = txtLoanDateFrm.Text;
        }
        //借款日期末
        if (txtLoanDateTo.Text != null && txtLoanDateTo.Text != "")
        {
            model.NOTE20 = txtLoanDateTo.Text;
        }
        //申请单号
        if (txtOrderCode.Text != null && txtOrderCode.Text != "")
        {
            model.Listid = txtOrderCode.Text;
        }
        //经办日期从
        if (txtRepsonDateTo.Text != null && txtRepsonDateTo.Text != "")
        {
            model.ResponsibleDate = txtRepsonDateFrom.Text;
        }
        //经办日期止
        if (txtRepsonDateFrom.Text != null && txtRepsonDateFrom.Text != "")
        {
            model.NOTE19 = txtRepsonDateTo.Text;
        }
        //经办人
        if (txtRepsonCode.Text != null && txtRepsonCode.Text != "")
        {
            string strReponCode = this.txtRepsonCode.Text.Trim();
            strReponCode = strReponCode.Substring(1, strReponCode.IndexOf("]") - 1).Trim();
            model.LoanCode = strReponCode;
        }
        //借款人
        if (txtloannamecode.Text != null && txtloannamecode.Text != "")
        {
            string strloanCode = txtloannamecode.Text.Trim();
            strloanCode = strloanCode.Substring(1, strloanCode.IndexOf("]") - 1).Trim();
            model.ResponsibleCode = strloanCode;
        }
        if (!string.IsNullOrEmpty(Request["deptCode"]))
        {
            model.LoanDeptCode = Request["deptCode"];
        }

        //冲减状态
        if (DrCJstatus.SelectedValue.ToString() != "")
        {
            model.Status = DrCJstatus.SelectedValue.ToString();
        }

        //审批状态
        if (DrSPstatus.SelectedValue.ToString() != "")
        {
            model.NOTE3 = DrSPstatus.SelectedValue.ToString();
        }
        //超期状态 
        model.NOTE21 = this.ddlStatus.SelectedValue;

        //超期天数
        int i = 0;
        if (int.TryParse(this.txtTianShu.Text.Trim(), out i))
        {
            model.NOTE22 = this.txtTianShu.Text.Trim();
        }
        else
        {
            model.NOTE22 = "0";
        }
        return model;

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
            string zt = e.Item.Cells[8].Text;
            if (zt != "审批通过")
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[8].Text = state;
            }

            if (!string.IsNullOrEmpty(e.Item.Cells[5].Text))
            {
                jkje += decimal.Parse(e.Item.Cells[5].Text);
            }
            if (!string.IsNullOrEmpty(e.Item.Cells[9].Text))
            {
                ycjje += decimal.Parse(e.Item.Cells[9].Text);
            }
            if (!string.IsNullOrEmpty(e.Item.Cells[10].Text))
            {
                wcjje += decimal.Parse(e.Item.Cells[10].Text);
            }
            //string dateTime = e.Item.Cells[3].Text;//借款日期
            //string jksj = e.Item.Cells[4].Text;//借款天数
            //string jszt = e.Item.Cells[6].Text;//借款状态
            //if (jszt != "结算完毕")//如果借款状态是结算完毕则不去添加背景颜色判断
            //{
            //    int diffDay = GetDiff(dateTime, jksj);
            //    if (diffDay <= 0 && diffDay >= -3)
            //    {
            //        e.Item.CssClass = "linqi";
            //    }
            //    else if (diffDay > 0)
            //    {
            //        e.Item.CssClass = "chaoqi";

            //    }
            //}
            string chaoqidays = e.Item.Cells[14].Text;
            string status = e.Item.Cells[6].Text;

            if (status.Equals("结算完毕"))
            {
                string chaoqidays_wanbi = e.Item.Cells[15].Text;
                if (int.Parse(chaoqidays_wanbi) > 0)
                {
                    e.Item.CssClass = "chaoqiwanbi";
                }
            }
            else
            {
                int idays = 0;
                if (int.TryParse(chaoqidays, out idays))
                {
                    if (idays >= 0 && idays <= 3)
                    {
                        e.Item.CssClass = "linqi";
                    }
                    else if (idays < 0)
                    {

                        e.Item.CssClass = "chaoqi";

                    }
                }
            }
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].Text = "合计";
            e.Item.Cells[0].Style.Add("text-align", "right");
            e.Item.Cells[5].Text = jkje.ToString();
            e.Item.Cells[9].Text = ycjje.ToString();
            e.Item.Cells[10].Text = wcjje.ToString();
            e.Item.Cells[5].Style.Add("text-align", "right");
            e.Item.Cells[9].Style.Add("text-align", "right");
            e.Item.Cells[10].Style.Add("text-align", "right");


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
