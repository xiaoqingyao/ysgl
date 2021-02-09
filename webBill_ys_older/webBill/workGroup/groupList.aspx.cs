using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class workGroup_groupList : BasePage
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
        string sql = "select Row_Number()over(order by groupid) as crow ,groupid,groupname,(case isnull(gType,'0') when '0' then '用户添加' when '1' then '系统定义' end) as gTypeName,gType from bill_userGroup where 1=1 ";
        if (txb_where.Text != "")
        {
            sql += " and (groupid like '%" + txb_where.Text + "%' or groupname like '%" + txb_where.Text + "%')";
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


    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string strGroupID = "";
        int sel_count = 0;
        string tType = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                strGroupID += "'" + myGrid.Items[i].Cells[1].Text + "',";
                tType = myGrid.Items[i].Cells[4].Text;
                sel_count += 1;
            }
        }
        if (sel_count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(),"","alert('请选择待删除的角色！');",true);
        }
        else if (sel_count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('只能选择一条角色删除！');", true);
        }
        else
        {
            if (tType == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('系统定义角色不能删除！');", true);
            }
            else
            {
                strGroupID = strGroupID.Substring(0, strGroupID.Length - 1);
                if (server.GetCellValue("select count(1) from bill_workflowgroup where wkGroup in (" + strGroupID + ")") != "0"
                    || server.GetCellValue("select count(1) from bill_users where usergroup in (" + strGroupID + ")") != "0"
                    )
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('角色处于使用状态,禁止删除！');", true);
                    return;
                }


                if (server.ExecuteNonQuery("delete from bill_userGroup where groupid in (" + strGroupID + ")") == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！'); ", true);
                }
            }
        }
        this.BindDataGrid();
    }

    #endregion

    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    } 
    #endregion
}