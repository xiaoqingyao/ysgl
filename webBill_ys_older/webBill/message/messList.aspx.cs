using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class message_messList : BasePage
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
        string sql = "select Row_Number()over(order by date desc) as crow ,id,title,(select username from bill_users where usercode = writer) as writer,contents,CONVERT(varchar(100),date, 23) as date ,isnull(readtimes,0) as readtimes,mstype,endtime from bill_msg where 1=1";

        try
        {
            if (Page.Request.QueryString["type"].ToString().Trim() == "lookall")
            {
                Button1.Visible = false;
                Button2.Visible = false;
                Button3.Visible = false;
            }
        }
        catch
        {
            sql += " and writer ='" + Session["userCode"].ToString() + "' ";
        }
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (title like '%" + this.TextBox1.Text.ToString().Trim() + "%' or writer like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
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
    //添加
    protected void Button1_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('messDetails.aspx?type=add');", true);
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
            Page.RegisterStartupScript("", "<script>window.alert('请选择要修改的友情提示！');</script>");
        }
        else if (selectCount > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('只能选择一条友情提示！');</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('messDetails.aspx?type=edit&mCode=" + kmCodes + "');", true);
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待删除的友情提示！');", true);
        }
        else if (selectCount > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能删除一条友情提示！');", true);
        }
        else
        {
            List<string> list = new List<string>();
            list.Add("delete from bill_msg where id = '" + strCwkmCode + "' and writer='" + Session["userCode"].ToString() + "' "); 


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
    protected void btnLook_Click(object sender, EventArgs e)
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
            Page.RegisterStartupScript("", "<script>window.alert('请选择要查看的友情提示！');</script>");
        }
        else if (selectCount > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('只能选择一条友情提示！');</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('message.aspx?id=" + kmCodes + "');", true);
        }
    }
}
