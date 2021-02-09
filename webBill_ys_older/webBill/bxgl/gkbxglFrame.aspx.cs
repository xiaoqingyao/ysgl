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

public partial class webBill_bxgl_gkbxglFrame : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdydj = "02";
    //一般报销单是否需要审核 默认是1 需要 edit by Lvcc
    bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;
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
                this.bindData();
            }
        }
        object objdydj = Request["dydj"];
        if (objdydj != null)
        {
            strdydj = objdydj.ToString();
        }
    }

    void bindData()
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
        string sql = @"select Row_Number()over(order by billName desc,billdate desc) as crow ,sum(billJe) as billJe,billname,flowid,stepid,billuser,billdate,isgk,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=(select top 1 billcode from bill_main where billname=main.billName)) as bxzy,
(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select top 1 billdept from bill_main where billname=main.billname) as billdept,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName
,(select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=main.billname) and rdstate='3') as mind
from bill_main main inner join bill_ybbxmxb_fykm
fykm on main.billcode=fykm.billcode inner join bill_yskm yskm on fykm.fykm=yskm.yskmcode where flowid='gkbx' and billUser='" + Session["userCode"].ToString().Trim() + "'  and isnull(LoopTimes,0)=0 ";
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
        //控制费用单据
        sql += " and yskm.dydj='" + strdydj + "'";
        //求和
        sql += " group by billname,flowid,stepid,billuser,billdate,isgk ";

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
        string strbillcode = e.Item.Cells[2].Text.Trim();
        
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            e.Item.Cells[5].Text = sysMgr.GetDeptCodeName(e.Item.Cells[5].Text);
            string zt = e.Item.Cells[8].Text;
            if (zt == "end")
            {
                e.Item.Cells[8].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[2].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[8].Text = state;
            }

            string isgk = e.Item.Cells[9].Text;
            if (isgk == "1")
            {
                e.Item.Cells[9].Text = "是";
            }
            else
            {
                e.Item.Cells[9].Text = "否";
            }
            e.Item.Cells[10].Text = this.getaudituser(strbillcode);
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
}
