using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class cwkm_cwkmList :BasePage
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
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 70);
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
        string sql = "select *,(case ShiFouFengCun when '1' then '是' when '0' then '否' end)as sffc ,Row_Number()over(order by cwkmcode) as crow from bill_cwkm where 1=1 ";
        string kmCode = Page.Request.QueryString["kmCode"].ToString().Trim();
        if (kmCode == "")
        {
        }
        else
        {
            if (this.chkNextLevel.Checked)
            {
                sql += " and left(cwkmCode,len('" + kmCode + "'))= '" + kmCode + "'";
            }
            else
            {
                sql += " and cwkmCode= '" + kmCode + "'";
            }
        }
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (cwkmcode like '%" + this.TextBox1.Text.ToString().Trim() + "%' or cwkmbm  like '%" + this.TextBox1.Text.ToString().Trim() + "%' or cwkmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%' or hsxm1 like '%" + this.TextBox1.Text.ToString().Trim() + "%' or hsxm2 like '%" + this.TextBox1.Text.ToString().Trim() + "%' or hsxm3 like '%" + this.TextBox1.Text.ToString().Trim() + "%' or hsxm4 like '%" + this.TextBox1.Text.ToString().Trim() + "%' or hsxm5 like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('cwkmDetails.aspx?type=add&pCode=" + Page.Request.QueryString["kmCode"].ToString().Trim() + "');", true);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string kmCodes = "";
        int selectCount = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                kmCodes = myGrid.Items[i].Cells[1].Text;
                selectCount++;
            }
        }
        if (selectCount == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择要修改的科目！');</script>");
        }
        else if (selectCount > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('只能选择一条科目！');</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('cwkmDetails.aspx?type=edit&kmCode=" + kmCodes + "&pCode=" + Page.Request.QueryString["kmCode"].ToString().Trim() + "');", true);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string strCwkmCode = "";
        int selectCount = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                strCwkmCode = myGrid.Items[i].Cells[1].Text;
                selectCount += 1;
            }
        }
        if (selectCount == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待删除的科目！');", true);
        }
        else if (selectCount > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能删除一项科目！');", true);
        }
        else
        {
            List<string> list = new List<string>();
            list.Add("delete from bill_cwkm where left(cwkmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");
            list.Add("delete from bill_yskm_dzb where left(cwkmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");
            list.Add("delete from bill_yskm_dept where left(cwkmCode,len('" + strCwkmCode + "'))='" + strCwkmCode + "' ");


            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！'); ", true);
            }

        }
        this.BindDataGrid();
    }
}
