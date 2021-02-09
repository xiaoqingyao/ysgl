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
using WorkFlowLibrary.WorkFlowBll;
using Bll.UserProperty;
using Dal.Bills;
using System.Collections.Generic;


public partial class webBill_bxgl_bxglFrame_Dz : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdydj = "02";
    string StrType = "";
    string strflowid = "ybbx";
    MainDal miandal = new MainDal();
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
            if (objDjlx != null)
            {
                djmxlx = objDjlx.ToString();
            }
            //如果对应单据为01（收入） 则显示导入按钮
            if (Request["dydj"] != null && Request["dydj"].ToString() != "")
            {
                strdydj = Request["dydj"].ToString();
                strflowid = miandal.getJSFlowId(strdydj);
                if (Request["dydj"].ToString() == "01")
                {
                    btn_Srbg.Visible = true;
                    btn_print.Visible = false;
                }
                else
                {
                    btn_Srbg.Visible = false;
                }
                this.btn_importEcxel.Visible = false;
            }

            if (!IsPostBack)
            {
                if (Request["type"] != null && Request["type"].ToString() != "")
                {
                    StrType = Request["type"].ToString();
                }
                //this.txtLoanDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                //this.txtLoanDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
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
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();




        //通过一般报销是否自动审核的配置项来控制删除是否好用 如果是自动审核的  允许删除审核成功了的单子 edit by lvcc 20130124
        this.hdYbbxNeedAudit.Value = boYbbxNeedAudit ? "1" : "0";
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
        hidflowid.Value = strflid;
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        string sql = @"select Row_Number()over(order by billdate desc,billName desc) as crow ,sum(je) as billJe,billname,flowid,stepid,billuser,billdate,isgk,(select bxsm from bill_ybbxmxb 
where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxsm,
(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select top 1 billdept from bill_main where billname=main.billname) as billdept
,(select deptname from bill_departments where deptcode =(select top 1 billdept from bill_main where billname=main.billname)) as billdeptname,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName
from bill_main main inner join bill_ybbxmxb_fykm
fykm on main.billcode=fykm.billcode  where flowid='" + strflid + "' and (billUser='" + Session["userCode"].ToString().Trim() + "' or main.billcode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "') or  billDept in (" + deptCodes + "))";
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
        }
        if (this.txtloannamecode.Text.Trim() != "")
        {
            string strusercode = this.txtloannamecode.Text.Trim();
            strusercode = strusercode.Substring(1, strusercode.IndexOf("]") - 1);
            sql += " and billuser='" + strusercode + "'";
        }
        if (this.txtLoanDeptCode.Text.Trim() != "")
        {
            string strdeptcode = this.txtLoanDeptCode.Text.Trim();
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
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
        //审批状态
        if (!this.ddlstatus.SelectedValue.Equals(""))
        {
            sql += " and stepid='" + this.ddlstatus.SelectedValue + "'";
        }
        //求和
        sql += " group by billname,flowid,stepid,billuser,billdate,isgk ";

        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));



        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        //Response.Write(strsqlframe);
        //count = 0;
        //return new DataTable();
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

        if (!string.IsNullOrEmpty(strdydj))
        {
            if (e.Item.ItemType == ListItemType.Header)
            {
                if (strdydj == "01")//收入
                {
                    e.Item.Cells[3].Text = "报告人";
                    e.Item.Cells[4].Text = "报告申请日期";


                }
                if (strdydj == "02")//用款申请单
                {
                    e.Item.Cells[3].Text = "借款人";
                    e.Item.Cells[4].Text = "申请日期";
                }
                if (strdydj == "04")//存货领用单
                {
                    e.Item.Cells[3].Text = "领用人";
                    e.Item.Cells[4].Text = "领用申请日期";

                }
                if (strdydj == "03")//固定资产购置付款单
                {
                    e.Item.Cells[3].Text = "购置人";
                    e.Item.Cells[4].Text = "购置付款日期";
                }
                if (strdydj == "05")//往来付款单
                {
                    e.Item.Cells[3].Text = "付款人";
                    e.Item.Cells[4].Text = "往来付款日期";
                }
                e.Item.Cells[5].Text = "部门";
            }

            if (strdydj == "01")//收入
            {

                e.Item.Cells[9].Style.Add("display", "none");//.Visible = false;

            }
        }
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strbillcode = e.Item.Cells[2].Text.Trim();
            SysManager sysMgr = new SysManager();
            e.Item.Cells[5].Text = sysMgr.GetDeptCodeName(e.Item.Cells[5].Text);
            string billcode = e.Item.Cells[2].Text;
            string zt = e.Item.Cells[8].Text;
            string state = "";
            if (zt == "end")
            {
                e.Item.Cells[8].Text = "审批通过";
            }
            else
            {

                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                state = bll.WFState(billcode);
                if (state == "未提交")//大智单子转移的问题  过一阵子就可以删除
                {
                    string billcode2 = server.GetCellValue("select top 1 billcode from bill_main where billname='" + billcode + "'");
                    state = bll.WFState(billcode2);
                }
                e.Item.Cells[8].Text = state;
            }
            if (state.IndexOf("否决") > -1)
            {
                e.Item.Cells[9].Text = getStatus(billcode);
            }
        }
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
    private string getaudituser(string strbillcode)
    {
        DataTable dt = server.GetDataTable("select '['+usercode+']'+username as username from bill_users where usercode in(select checkuser from workflowrecord a,workflowrecords  b where a.recordid=b.recordid and billcode=@billcode)", new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@billcode", strbillcode) });
        string endstr = "";
        StringBuilder strendstr = new StringBuilder();
        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                strendstr.Append(dr["username"]);
                strendstr.Append(",");
            }
        }
        if (strendstr.Length > 1)
        {
            endstr = strendstr.ToString().Substring(0, strendstr.Length - 1).ToString();
        }
        return endstr;

    }
    private string getStatus(string billcode)
    {


        string sql = @" select top 1 mind 
                         from workflowrecords
                         where rdstate='3' and
 
                         recordid=(
			                          select w.recordid
				                        from workflowrecord w,bill_main m
				                        where (w.billCode = m.billCode or w.billCode = m.billName)
				                        and w.billCode='" + billcode + @"' 
				                        group by w.recordid
			                          ) ";
        //string sql = " select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where (billCode='" + billcode + "' or billcode=(select top 1 billcode from bill_main where billname='" + billcode + "'))) and rdstate='3' ";
        return server.GetCellValue(sql);
        //string sql = " select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where (billCode='" + billcode + "' or billcode=(select top 1 billcode from bill_main where billname='" + billcode + "'))) and rdstate='3' ";
        //return server.GetCellValue(sql);
    }
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        int count = 0;
        DataTable dt = GetData(0, 99999999, out count);
        Dictionary<string, string> dic = new Dictionary<String, String>();
        dic.Add("billname", "单据编号");
        dic.Add("billdeptname", "部门");
        dic.Add("billUserName", "制单人");
        dic.Add("billdate", "单据日期");
        dic.Add("billJe", "单据金额");
        dic.Add("isgk", "是否归口");
        dic.Add("bxsm", "摘要");
        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
       // int count = 0;
        string strsql = "select * from dz_yksq_bxd";

        DataTable dt = server.GetDataTable(strsql, null); 
        Dictionary<string, string> dic = new Dictionary<String, String>();
        //dic.Add("billname", "单据编号");
        //dic.Add("billdeptname", "部门");
        //dic.Add("billUserName", "制单人");
        //dic.Add("billdate", "单据日期");
        //dic.Add("billJe", "单据金额");
        //dic.Add("isgk", "是否归口");
        //dic.Add("bxsm", "摘要");
        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);
    }
}