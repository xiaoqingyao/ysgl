using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bll.UserProperty;

public partial class yskm_yskmList : BasePage
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
                hf_km.Value = Request.QueryString["kmCode"];
                this.BindDataGrid();

            }
        }
    }

    public void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 110);
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
        string sql = @"select Row_Number()over(order by yskmcode) as crow , (case kmStatus when '1' then '正常' when '0' then '禁用' end) as status,yskmCode,yskmbm,replicate('　　',len(yskmCode)-2)+yskmmc as yskmmc,tbsm,
(case tblx when '01' then '单位填报' when '02' then '<font color=red>财务填报</font>' end) as tblx,(case kmlx when '0' then '可控费用' when '1' then '不可控费用' end) as kmlx,
(case isnull(gkfy,'0') when '0' then '否' when '1' then '是' end) as gkfy,(case isnull(xmhs,'0') when '0' then '否' when '1' then '是' end) as xmhs,
(case isnull(bmhs,'0') when '0' then '否' when '1' then '是' end) as bmhs,(case isnull(ryhs,'0') when '0' then '否' when '1' then '是' end) as ryhs  from bill_yskm where 1=1 ";
        if (this.CheckBox2.Checked == true)
        { }
        else
        {
            sql += " and kmStatus='1'";
        }

        string kmCode = Page.Request.QueryString["kmCode"].ToString().Trim();
        if (kmCode == "")
        {
        }
        else
        {
            if (this.chkNextLevel.Checked)
            {
                sql += " and left(yskmCode,len('" + kmCode + "'))= '" + kmCode + "'";
            }
            else
            {
                sql += " and yskmCode= '" + kmCode + "'";
            }
        }
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (yskmcode like '%" + this.TextBox1.Text.ToString().Trim() + "%' or yskmbm  like '%" + this.TextBox1.Text.ToString().Trim() + "%' or yskmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
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
        BindDataGrid();
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        string strCwkmCode = kmcode.Value.ToString().Trim();
        List<string> list = new List<string>();
        list.Add("delete from bill_yskm where left(yskmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");
        list.Add("delete from bill_yskm_dzb where left(yskmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");
        list.Add("delete from bill_yskm_dept where left(yskmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");
        //判断要删除的科目是否有预算或者使用
        YsManager ysm = new YsManager();
        if (ysm.CheckYskm(strCwkmCode) == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该科目已使用，不能删除，如果不再使用该科目，请禁用！');", true);
            return;
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
        }
        this.BindDataGrid();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button5_Click1(object sender, EventArgs e)
    {
        string kmCodes = kmcode.Value.ToString().Trim();

        System.Collections.Generic.List<string> list = new List<string>();

        list.Add("update bill_yskm set kmStatus='0' where yskmCode='" + kmCodes + "'");

        server.ExecuteNonQuerysArray(list);
        this.BindDataGrid();
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        string kmCodes = kmcode.Value.ToString().Trim();
        System.Collections.Generic.List<string> list = new List<string>();

        list.Add("update bill_yskm set kmStatus='1' where yskmCode='" + kmCodes + "'");

        server.ExecuteNonQuerysArray(list);
        this.BindDataGrid();
    }
    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
}