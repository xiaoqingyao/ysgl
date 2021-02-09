using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_sjzd_sjzdList : BasePage
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
                DataDicList();
            }
        }
    }

    protected void DataDicList()
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
        string str_sql = "select Row_Number()over(order by dicCode) as crow ,dictype,diccode,dicname,(case isnull(cdj,'0') when '0' then '否' when '1' then '报告单'  end) as cdj from bill_dataDic where 1=1 ";
        if (Request.QueryString["dicType"].ToString() != "")
        {
            str_sql += " and dictype='" + Request.QueryString["dicType"].ToString() + "'";
            this.Label1.Text = server.GetCellValue("select dicName from bill_dataDic where dicCode='" + Page.Request.QueryString["dicType"].ToString().Trim() + "' and dicType='00'");
        }
        else
        {
            str_sql += " and dicType='reliy'";
            this.Label1.Text = "请选择相应字典类型......";
            this.btn_add.Visible = false;
            this.btn_del.Visible = false;
            this.btn_edit.Visible = false;
        }
        //查询条件
        if (txb_where.Text.Trim() != "")
        {
            str_sql += " and dicname like '%" + txb_where.Text.Trim() + "%'";
        }
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, str_sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, str_sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }



    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        DataDicList();
    }

 
    #region 修改
	protected void btn_edit_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                diccode = this.myGrid.Items[i].Cells[2].Text.ToString().Trim(); 
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多条数据！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的数据！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sjzdDetail.aspx?type=edit&dictype=" + Request.QueryString["dicType"].ToString() + "&diccode=" + diccode + "');", true);             
        }
    } 
	#endregion 
    
    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0; 
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                diccode += "'"+this.myGrid.Items[i].Cells[2].Text.ToString().Trim()+"',";
                count += 1;
            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要删除的数据！');", true);
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);
            if (Page.Request.QueryString["dictype"].ToString().Trim() == "01")
            {
                DataSet temp = server.GetDataSet("select jkdjlx from bill_fysq where jkdjlx in (" + diccode + ")");
                if (temp.Tables[0].Rows.Count == 0)
                { }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('待删除的数据字典正在使用中！');", true);
                    return;
                }
            }
            else if (Page.Request.QueryString["dictype"].ToString().Trim() == "02")
            {
                DataSet temp = server.GetDataSet("select bxmxlx from bill_ybbxmxb where bxmxlx in (" + diccode + ")");
                if (temp.Tables[0].Rows.Count == 0)
                { }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('待删除的数据字典正在使用中！');", true);
                    return;
                }
            }
            System.Collections.Generic.List<string> list = new List<string>();

            list.Add("delete from bill_dataDic where diccode in (" + diccode + ") and dictype='" + Request.QueryString["dicType"].ToString() + "'");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                this.DataDicList();
            }
        }

    }
    #endregion

    #region 查询 
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        DataDicList();
    }
    #endregion

    protected void btn_add_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sjzdDetail.aspx?type=add&dictype=" + Request.QueryString["dicType"].ToString() + "&diccode=');", true);
    }
}
