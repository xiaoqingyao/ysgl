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
using Bll.UserProperty;
using System.Text;
using System.Data.SqlClient;
using Bll;
using Dal.Bills;

public partial class webBill_bxgl_bxList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    MainDal miandal = new MainDal();
    string StrType = "";
    string strdydj = "02";
    string strflowid = "ybbx";
    string djmxlx = "";//单据明细类型  如果是空则报销明细类型读取所有的项目   如果传入01则只读取数据字典中为01的报销明细类型  在列表页只是作为一个参数传递的中转站
    //一般报销单是否需要审核 默认是1 需要 edit by Lvcc
    bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            //获取url参数 单据明细类型
            object objDjlx = Request["djmxlx"];
            if (objDjlx!=null)
            {
                djmxlx = objDjlx.ToString();
            }
            //如果对应单据为01（收入） 则显示导入按钮
            if (Request["dydj"] != null && Request["dydj"].ToString() != "")
            {
                strdydj = Request["dydj"].ToString();
                strflowid = miandal.getJSFlowId(strdydj);
               
             
            }
            if (!IsPostBack)
            {
                if (Request["type"] != null && Request["type"].ToString() != "")
                {
                    StrType = Request["type"].ToString();
                }
                this.bindData();
            }


        }
    }

    void bindData()
    {
        if (Request["type"] != null && Request["type"].ToString() != "")
        {
            StrType = Request["type"].ToString();
        }
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


        //通过一般报销是否自动审核的配置项来控制删除是否好用 如果是自动审核的  允许删除审核成功了的单子 edit by lvcc 20130124
        this.hdYbbxNeedAudit.Value = boYbbxNeedAudit ? "1" : "0";

        //注册
        object objregistermark_date = System.Configuration.ConfigurationManager.AppSettings["RegistDate"];
        DateTime dtReg;
        if (objregistermark_date != null)
        {
            dtReg = DateTime.Parse(objregistermark_date.ToString());
            DateTime strnowdate = DateTime.Now;
            if (strnowdate > dtReg)
            {
                Random dom = new Random();
                int idom = dom.Next(0, 10);
                if (idom % 3 == 0)
                {
                    TimeSpan aa = DateTime.Parse("2013-05-26") - DateTime.Now;
                    int iDays = aa.Days + 1;
                    if (iDays > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期，还有" + iDays + "天系统将锁定，请联系软件开发商！');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期,，请联系软件开发商！');", true);

                    }
                }
            }
        }
    }


    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string strflid = "ybbx";//决算flowid
        string djlx = "02";
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            djlx = Convert.ToString(Request["dydj"]);
        }
        if (!string.IsNullOrEmpty(miandal.getJSFlowId(djlx)))
        {
            strflid = miandal.getJSFlowId(djlx);
        }



        //if (!string.IsNullOrEmpty(Request["djlx"]))
        //{
        //    if (djlx == "02" && Request["djlx"] == "qkd")
        //    {
        //        strflid = "yksq";
        //    }
        //}
        hidflowid.Value = strflid;


        //根据对应单据查找对应的flowid


        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");

        string sql = @"select  billName,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy
                            ,stepid,billDept,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName
                            ,billdate,billje ,Row_Number()over(order by billName desc,billdate desc) as crow from bill_main 
                      where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "') or  billDept in (" + deptCodes + ") ) and flowID='" + strflid + "'";// and stepid !='end'
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
        }
       
        string strusercode = (new PublicServiceBLL()).SubSting(this.txtloannamecode.Text.Trim());
        if (!string.IsNullOrEmpty(strusercode))
        {
            sql += " and billuser='" + strusercode + "'";
        }

        string strdeptcode = (new PublicServiceBLL()).SubSting(this.txtLoanDeptCode.Text.Trim());
        if (!string.IsNullOrEmpty(strdeptcode))
        {
            sql += " and billDept='" + strdeptcode + "'";
        }
        if (txtLoanDateFrm.Text.Trim() != "")
        {
            sql += " and billdate>='" + this.txtLoanDateFrm.Text.Trim() + "'";
        }
        if (this.txtLoanDateTo.Text.Trim() != "")
        {
            sql += " and billdate<='" + this.txtLoanDateTo.Text.Trim() + "'";
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
        bindData();
    }



    protected void Button4_Click(object sender, EventArgs e)
    {
        this.bindData();
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        //if (Request["type"] != null && Request["type"].ToString() != "")
        //{
        //    StrType = Request["type"].ToString();
        //}
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            e.Item.Cells[4].Text = sysMgr.GetDeptCodeName(e.Item.Cells[4].Text);
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

            string isgk = e.Item.Cells[8].Text;
            if (isgk == "1")
            {
                e.Item.Cells[8].Text = "是";
            }
            else
            {
                e.Item.Cells[8].Text = "否";
            }

            string gkDep = e.Item.Cells[9].Text;
            if (gkDep != "&nbsp;" && !string.IsNullOrEmpty(gkDep))
            {
                e.Item.Cells[9].Text = sysMgr.GetDeptCodeName(gkDep);
            }
            string strbillcode = e.Item.Cells[0].Text.Trim();
            e.Item.Cells[10].Text = this.getreasion(strbillcode);
        }
        //string djlx = "ybbx";
        //if (!string.IsNullOrEmpty(Request["dydj"]))
        //{
        //    djlx = Convert.ToString(Request["dydj"]);
        //}
        if (!string.IsNullOrEmpty(strdydj))
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (strdydj == "01")//收入
                {
                    e.Item.Cells[2].Text = "报告人";
                    e.Item.Cells[3].Text = "报告申请日期";


                }
                if (strdydj == "02")//用款申请单
                {
                    e.Item.Cells[2].Text = "申请人";
                    e.Item.Cells[3].Text = "申请日期";
                }
                if (strdydj == "04")//存货领用单
                {
                    e.Item.Cells[2].Text = "领用人";
                    e.Item.Cells[3].Text = "领用申请日期";

                }
                if (strdydj == "03")//固定资产购置付款单
                {
                    e.Item.Cells[2].Text = "购置人";
                    e.Item.Cells[3].Text = "购置付款日期";
                }
                if (strdydj == "05")//往来付款单
                {
                    e.Item.Cells[2].Text = "付款人";
                    e.Item.Cells[3].Text = "往来付款日期";
                }
                e.Item.Cells[5].Text = "总额";
            }

            if (strdydj == "01")//收入
            {

                e.Item.Cells[8].Style.Add("display", "none");//.Visible = false;
                e.Item.Cells[9].Style.Add("display", "none");//.Visible = false;

            }
        }

    }
    private string getreasion(string billcode)
    {
        string strsql = @"select top 1 mind from workflowrecord a ,workflowrecords b where a.recordid=b.recordid and billCode=@billcode  and b.rdstate='3' and b.mind is not null";
        SqlParameter[] arr = new SqlParameter[] { new SqlParameter("@billcode", billcode) };
        return server.GetCellValue(strsql, arr);
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
}
