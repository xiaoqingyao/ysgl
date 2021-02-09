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
using System.Text;
using System.Data.SqlClient;
using System.IO;
using Bll.Bills;
using Dal.Bills;

public partial class webBill_cwgl_PingZhengList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string djlx = string.Empty;//对应的预算类型
    string flowid = string.Empty;//单据类型对应的flowid
    MainDal mainbill = new MainDal();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["djlx"] != null)
            {
                djlx = Request["djlx"].ToString();
                //根据对应单据获取flowid
                flowid = mainbill.getJSFlowId(djlx);// server.GetCellValue("select note2 from bill_datadic where dictype='18' and diccode=(select note1 from bill_datadic where dictype='07' and diccode='" + djlx + "')");
                //如果传入了flowid，原来的就不允许选择  直接显示全部解开
                this.ddlBillType.SelectedValue = "";
                this.ddlBillType.Enabled = false;
            }
            if (!IsPostBack)
            {
                DateTime now = DateTime.Now;
                int year2 = 0;
                int month2 = 0;
                if (now.Month == 12)
                {
                    year2 = now.Year + 1;
                    month2 = 1;
                }
                else
                {
                    year2 = now.Year;
                    month2 = now.Month + 1;
                }
                DateTime d1 = new DateTime(now.Year, now.Month, 1);
                DateTime d2 = new DateTime(year2, month2, 1);
                this.txtLoanDateFrm.Text = d1.ToString("yyyy-MM-dd");
                this.txtLoanDateTo.Text = d2.ToString("yyyy-MM-dd");
                //记录凭证制作页面的地址
                hdpingzhengdetailurl.Value = new Bll.ConfigBLL().GetValueByKey("pingzhengdetailurl");
                //读取配置项  单据类型
                hdbxdtype.Value = new Bll.ConfigBLL().GetValueByKey("fybxd");
                this.BindDataGrid();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
        }
    }

    private void BindDataGrid()
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
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();


    }


    private DataTable GetData(int pagefrm, int pageto, out int count)
    {

        string sql = GetSql();
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }

    private string GetSql()
    {

        bool bousegkfj = new Bll.ConfigBLL().GetValueByKey("UseGKFJ").Equals("1");//启用归口分解
        object obj = ConfigurationManager.AppSettings["makepingzhengafterjunxing"];
        bool bocontroljunxingend = obj == null ? false : obj.ToString().Equals("yes");//是否陈俊兴审核后才能做凭证
        string strmainsqlwhere = "";
        #region strmainsqlwhere
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        strmainsqlwhere += " and billDept in (" + deptCodes + ") ";// order by bill_main.billDate desc
        // if (!bousegkfj)//控制 如果不启用归口分解 那默认能做凭证的就只能是归口报销的
        // {
        string strddlBillType = this.ddlBillType.SelectedValue.Trim();
        if (!strddlBillType.Equals(""))
        {
            if (strddlBillType.Equals("1"))
            {
                strmainsqlwhere += " and main.flowID in ('ybbx','gkbx')";
            }
            else
            {
                strmainsqlwhere += " and main.flowID='qtbx'";
            }
        }
        // }
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            strmainsqlwhere += " and (main.billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
        }
        if (this.txtloannamecode.Text.Trim() != "")
        {
            string strusercode = this.txtloannamecode.Text.Trim();
            strusercode = strusercode.Substring(1, strusercode.IndexOf("]") - 1);
            strmainsqlwhere += " and main.billuser='" + strusercode + "'";
        }
        if (this.txtLoanDeptCode.Text.Trim() != "")
        {
            string strdeptcode = this.txtLoanDeptCode.Text.Trim();
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
            strmainsqlwhere += " and main.billDept='" + strdeptcode + "'";
        }
        if (txtLoanDateFrm.Text.Trim() != "")
        {
            strmainsqlwhere += " and main.billdate>='" + this.txtLoanDateFrm.Text.Trim() + "'";
        }
        if (this.txtLoanDateTo.Text.Trim() != "")
        {
            strmainsqlwhere += " and main.billdate<='" + this.txtLoanDateTo.Text.Trim() + "'";
        }
        if (!bocontroljunxingend)
        {
            //审批状态
            string strstepid = this.ddlStep.SelectedValue;
            if (!string.IsNullOrEmpty(strstepid))
            {
                if (strstepid.Equals("1"))
                {
                    strmainsqlwhere += " and main.stepID='end'";
                }
                else
                {
                    strmainsqlwhere += " and main.stepID='-1'";
                }
            }
        }
        //flowid 可以包含 ybbx srd chly 等各类决算单
        if (!string.IsNullOrEmpty(flowid))
        {
            strmainsqlwhere += " and flowid='" + flowid + "'";
        }
        #endregion
        string strybbxmxbsqlwhere = "";
        #region strybbxmxbsqlwhere
        //凭证状态
        if (this.ddlStatus.SelectedValue != "")
        {//已完成
            if (this.ddlStatus.SelectedValue.Equals("1"))
            {
                strybbxmxbsqlwhere += " and isnull(bill_ybbxmxb.pzcode,'')!=''";
            }
            else
            { //未完成
                strybbxmxbsqlwhere += " and isnull(bill_ybbxmxb.pzcode,'')=''";
            }
        }
        //支付方式
        if (this.ddlPayType.SelectedValue != "")
        {
            strybbxmxbsqlwhere += " and isnull(bill_ybbxmxb.guazhang,'0')=" + ddlPayType.SelectedValue + "";
        }
        //凭证号
        string strPingZhengCode = txtPingZhengCode.Text.Trim();
        if (!string.IsNullOrEmpty(strPingZhengCode))
        {
            strybbxmxbsqlwhere += " and isnull(bill_ybbxmxb.pzcode,'')='" + strPingZhengCode + "'";
        }
        #endregion

        string strje = txtje.Text.Trim();
        decimal deje = 0;
        if (decimal.TryParse(strje, out deje))
        {
            strybbxmxbsqlwhere = " and b.billJe=" + strje + "";
        }

        string sql = "";
        if (bousegkfj)//使用归口分解（市立医院）
        {
            sql = @"
                    select Row_Number()over(order by b.billname desc) as crow ,
	                     b.*,bill_ybbxmxb.bxzy,bill_ybbxmxb.pzcode,CONVERT(varchar(10), bill_ybbxmxb.pzdate, 23) as pzdate
	                    ,(case isnull(bill_ybbxmxb.guazhang,'0') when '1' then '挂账' when '0' then '出纳支付' else '出纳支付' end) as guazhang
	                    ,(case isnull(bill_ybbxmxb.sfgf,'0') when '0' then '否' when '1' then '是' end) as sfgf
	                    ,(select dicName from bill_dataDic where dicType='10' and dicCode=bill_ybbxmxb.zhangtao) as zhangtao 
	                    from
		                     (select stepid,(select '['+deptcode+']'+deptname from bill_departments where deptcode=billdept) as billdept,
		                     (select top 1 billcode from bill_main where billname=a.billname) as billcode,billname,
		                     (select '['+usercode+']'+username from bill_users where usercode=billuser) as billuser,
                             convert(varchar(10),billdate,120) as tbilldate,billje
			                    from (
			                    select sum(billJe) as billJe,billname,flowid,stepid,billuser,billdate,isgk,
			                    (select top 1 billdept from bill_main where billname=main.billname) as billdept
			                    from bill_main main where 1=1 {0} 
			                    group by billname,flowid,stepid,billuser,billdate,isgk )
			                    a)
	                     b inner join bill_ybbxmxb on b.billcode=bill_ybbxmxb.billcode where 1=1  
                ";
            sql = string.Format(sql, strmainsqlwhere);
            sql += strybbxmxbsqlwhere;
        }
        else
        {
            if (bocontroljunxingend)//陈俊星审核通过后就可以做凭证
            {
                sql = @"select Row_Number()over(order by main.billname desc) as crow , main.*,
                    (case isnull(guazhang,'0') when '1' then '挂账' when '0' then '出纳支付' else '出纳支付' end) as guazhang,
                    (case isnull(sfgf,'0') when '0' then '否' when '1' then '是' end) as sfgf,
                    (select dicName from bill_dataDic where dicType='10' and dicCode=bill_ybbxmxb.zhangtao) as zhangtao
                    ,bxzy,pzcode,CONVERT(varchar(10), pzdate, 23) as pzdate
                     from (select main.stepid,main.flowid,(select deptName from bill_departments where deptCode=main.billDept) as billDept,
                      main.billCode,main.billName,(select username from bill_users where usercode=main.billuser) as billUser,
                        convert(varchar(10),billdate,121) as tbilldate,billje
                         from bill_main main 
                        inner join workflowrecord record on main.billcode=main.billcode
                        inner join workflowrecords records on record.recordid=records.recordid where 
                        checkuser='24536' and records.rdstate='2' {0}) main ,bill_ybbxmxb 
                                         where bill_ybbxmxb.billCode=main.billCode ";
                sql = string.Format(sql, strmainsqlwhere);
                sql += strybbxmxbsqlwhere;
            }
            else
            {
                sql = "select  Row_Number()over( order by main.billname desc) as crow , stepid,(select deptName from bill_departments where deptCode=billDept) as billDept,main.billCode,billName,(select username from bill_users where usercode=billuser) as billUser,convert(varchar(10),billdate,121) as tbilldate,billje,bxzy,pzcode,CONVERT(varchar(10), pzdate, 23) as pzdate,(case isnull(guazhang,'0') when '1' then '挂账' when '0' then '出纳支付' else '出纳支付' end) as guazhang,(case isnull(sfgf,'0') when '0' then '否' when '1' then '是' end) as sfgf,(select dicName from bill_dataDic where dicType='10' and dicCode=bill_ybbxmxb.zhangtao) as zhangtao from bill_main main,bill_ybbxmxb  where bill_ybbxmxb.billCode=main.billCode {0}";
                sql = string.Format(sql, strmainsqlwhere + strybbxmxbsqlwhere);
            }

        }
        return sql;
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[8].Text;
            if (zt == "end")
            {
                e.Item.Cells[8].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[1].Text;
                WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager bll = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[8].Text = state;
            }
            string strpzdate = e.Item.Cells[10].Text.Trim();
            if (strpzdate.Equals("1900-01-01"))
            {
                e.Item.Cells[10].Text = "";
            }
        }
    }
    protected void btn_Select_click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }



    /// <summary>
    /// 补录凭证
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_BuLu_Click(object sender, EventArgs e)
    {
        int iGridViewRows = this.myGrid.Items.Count;
        string strcode = "";
        int iSelectRows = 0;
        for (int i = 0; i < iGridViewRows; i++)
        {
            CheckBox cb = this.myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            if (cb == null)
            {
                continue;
            }
            if (cb.Checked)
            {
                iSelectRows++;
                strcode = this.myGrid.Items[i].Cells[1].Text.Trim();
            }
        }
        if (iSelectRows != 1)
        {
            showMessage("请选择一条记录！", false, "");
            return;
        }
        ClientScript.RegisterStartupScript(this.GetType(), "bl", "openDetail2('PingZhengBuLu.aspx?BillCode=" + strcode + "')", true);
    }

    protected void btn_Mian_Click(object sender, EventArgs e)
    {
        int iGridViewRows = this.myGrid.Items.Count;
        string strcode = "";
        int iSelectRows = 0;
        for (int i = 0; i < iGridViewRows; i++)
        {
            CheckBox cb = this.myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            if (cb == null)
            {
                continue;
            }
            if (cb.Checked)
            {
                iSelectRows++;
                string code = this.myGrid.Items[i].Cells[1].Text.Trim().Replace("&nbsp;", "");
                strcode += "'" + code + "',";
            }
        }
        if (iSelectRows < 1)
        {
            showMessage("请选择一条记录！", false, "");
            return;
        }
        if (strcode.Length > 1)
        {
            strcode = strcode.Substring(0, strcode.Length - 1);
        }
        string strsql = "update bill_ybbxmxb set pzcode='000',pzdate=getdate(),zhangtao='mian' where billcode in (" + strcode + ")";
        int iRel = server.ExecuteNonQuery(strsql);
        if (iRel > 0)
        {
            showMessage("报销单免除做凭证操作成功。", false, "");
            this.BindDataGrid();
        }

    }

    protected void btn_GuaZhang_Click(object sender, EventArgs e)
    {
        int iGridViewRows = this.myGrid.Items.Count;
        int iSelectRows = 0;
        string strcode = "";
        string strStatus = "出纳支付";
        for (int i = 0; i < iGridViewRows; i++)
        {
            CheckBox cb = this.myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            if (cb == null)
            {
                continue;
            }
            if (cb.Checked)
            {
                iSelectRows++;
                strcode = this.myGrid.Items[i].Cells[1].Text.Trim();
                strStatus = this.myGrid.Items[i].Cells[12].Text.Trim();
            }
        }
        if (iSelectRows != 1)
        {
            showMessage("请选择一条记录！", false, "");
            return;
        }
        string strStatus2 = strStatus.Equals("出纳支付") ? "1" : "0";
        if (strStatus2.Equals("1"))//挂账
        {
            string strsqlselect = "select sfgf from bill_ybbxmxb where billCode='" + strcode + "'";
            object objSfgf = server.ExecuteScalar(strsqlselect);
            string strSfgf = "1";
            if (objSfgf != null)
            {
                strSfgf = objSfgf.ToString();
            }
            if (strSfgf.Equals("1"))
            {
                showMessage("该报销单已给付,不允许修改状态！", false, ""); return;
            }
        }
        string endSql = "update bill_ybbxmxb set guazhang='" + strStatus2 + "' where billCode='" + strcode + "'";
        if (server.ExecuteNonQuery(endSql) > 0)
        {
            showMessage("操作成功！", false, "");
            BindDataGrid();
        }
        else
        {
            showMessage("操作失败！", false, "");
        }
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
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



    protected void Button1_Click(object sender, EventArgs e)
    {

        string sql = GetSql();
        DataTable dt = server.GetDataTable(sql, null);


        //删除列 
        // dt.Columns.Remove("crow");

        ////调整列顺序 ，列排序从0开始 
        //dt.Columns["billCode"].SetOrdinal(0);

        //修改列标题名称 
        //dt.Columns["billCode"].ColumnName = "billCode";

        //DataColumn priceColumn = new DataColumn();
        ////该列的数据类型
        //priceColumn.DataType = System.Type.GetType("System.String");

        ////该列得名称
        //priceColumn.ColumnName = "price";

        ////该列得默认值
        //priceColumn.DefaultValue = "test";

        //dt.Columns.Add(priceColumn);




        string hiddens = "crow";
        string files = "billCode,billName,billUser,tbilldate,billDept,billJe,bxzy,stepid,pzcode,pzdate,zhangtao,guazhang,sfgf";
        string names = "单据唯一码,单据编号,报销人,申请日期,所属部门,报销总额,摘要,状态,凭证号,凭证日期,帐套,支付方式,是否给付";


        for (int i = 0; i < dt.Rows.Count; i++)
        {


            string zt = dt.Rows[i]["stepid"].ToString();
            if (zt == "end")
            {
                dt.Rows[i]["stepid"] = "审批通过";
            }
            else
            {
                string billcode = dt.Rows[i]["billcode"].ToString();
                WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager bll = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                dt.Rows[i]["stepid"] = state;
            }
            string strpzdate = dt.Rows[i]["pzdate"].ToString();
            if (strpzdate.Equals("1900-01-01"))
            {
                dt.Rows[i]["pzdate"] = "";
            }

        }

        DataGridToExcel.DataTable2Excel(dt, hiddens, files, names);
        // DataGridToExcel.CreateExcel(dt, hiddens, files, names);
        // DataGridToExcel.DataTableToExcel(dt,"mu.xls", hiddens, files, names);
    }


}
