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

public partial class webBill_bxgl_qtbxglFrame : BasePage
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
                
                this.bindData();
            }
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
    }



    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = "select Row_Number()over(order by billName desc) as crow,billName,isGk,gkDept,(select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName2,(select username from bill_users where usercode=billuser) as billUser,billdate,billje ,(select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind from bill_main where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "')) and flowID='qtbx'";
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (billCode like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
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
        }
        //if (e.Item.ItemType != ListItemType.Header)
        //{
        //    e.Item.Cells[4].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[4].Text.ToString().Trim());
        //}
    }
}
