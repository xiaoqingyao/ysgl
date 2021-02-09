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

public partial class webBill_search_cgspList : BasePage
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
                this.txb_sqrqbegin.Text = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-01";
               // this.txb_sqrqend.Text = System.DateTime.Now.ToShortDateString();
                this.txb_sqrqend.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='03' order by dicCode");
                //this.ddl_sqlx.DataTextField = "dicName";
                //this.ddl_sqlx.DataValueField = "dicCode";
                //this.ddl_sqlx.DataSource = temp;
                //this.ddl_sqlx.DataBind();

                ListItem li = new ListItem();
                li.Text = "--全部--";
                li.Value = "00";
                this.ddl_sqlx.Items.Add(li);
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li2 = new ListItem(temp.Tables[0].Rows[i]["dicName"].ToString().Trim(), temp.Tables[0].Rows[i]["dicCode"].ToString().Trim());
                    this.ddl_sqlx.Items.Add(li2);
                }

                BindDataGrid();
            }
        }
    }


    protected void BindDataGrid()
    {


        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 130);
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
        string sql = @"select Row_Number()over(order by billdate desc) as crow,(select dicname from bill_datadic where diccode=b.cglb and dictype='03') 
                               as cglb,b.sj,b.sm,b.cgze,a.billCode,cgDept,(select userName from bill_users where userCode=a.billUser) as  billUser,stepid as stepID_ID,
                              (case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' 
                             else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='cgsp' and bill_workFlowStep.stepid=a.stepid ) end) 
                             as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as cglb2 ,b.gys
                     from bill_main a,bill_cgsp b
                     where a.flowid='cgsp' and a.billCode=b.cgbh ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetSearchDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and a.billDept in (" + deptCodes + ") ";
            //报告申请单据所在单位也可查看对应生成的采购审批单
            //sql += " or b.cgbh in (select billCode from bill_cgsp_lscg where lscgCode in (select cgbh from bill_lscg where cgDept in (" + deptCodes + "))))";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and a.billDept in (" + deptCodes + ") ";
            //报告申请单据所在单位也可查看对应生成的采购审批单
            //sql += " or b.cgbh in (select billCode from bill_cgsp_lscg where lscgCode in (select cgbh from bill_lscg where cgDept in (" + deptCodes + "))))";
        }


        #region 查询条件
        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >=cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <=cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        //申请类型
        if (ddl_sqlx.SelectedIndex != 0)
        {
            sql += " and b.cglb='" + ddl_sqlx.SelectedValue + "'";
        }
        #endregion

 
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择查看的申请单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('../fysq/cgspDetail.aspx?type=look&cgbh=" + billGuid + "');", true);
            //Response.Redirect("ystbDetail.aspx?gcbh=" + billGuid );
        }
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        bool isBegin = false;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();

            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待打印的采购审批单！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../printer/printerCgsp.aspx?billCode=" + billCode + "');", true);
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header)
        {
            e.Item.Cells[2].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[2].Text.ToString().Trim());
        }
    }
}
