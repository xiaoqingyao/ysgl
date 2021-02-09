using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Models;
using Bll.UserProperty;

public partial class user_userList : BasePage
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
                hf_dept.Value = Page.Request.QueryString["deptCode"].ToString().Trim();

                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["deptCode"]).Trim()))
                {
                    this.Label1.Text = "当前部门：[" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "]" + server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                }
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
        string sql = "select Row_Number()over(order by usercode asc) as crow , usercode,username,( select DicName from bill_DataDic where dicType='05' and diccode=bill_users.userPosition) as userPosition,(select groupname from bill_usergroup where groupid=usergroup) as usergroup ,case userstatus when 1 then '正常' else '禁用' end as userstatus,";
        sql += " (select deptname from bill_departments where deptcode =userdept ) as userdept,userpwd  from bill_users  where userCode<>'admin' ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and userDept in (" + deptCodes + ")";
        }
        else
        {
            string strdeptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
            DepartmentManager mgr = new DepartmentManager(strdeptCode);
            if (this.chkNextLevel.Checked)
            {
                string deptCodes = mgr.GetAllChildToString();
                deptCodes = "'" + deptCodes.Replace(",", "','") + "'";
                sql += " and userDept in (" + deptCodes + ")";
            }
            else
            {
                //不包含下级
                sql += " and userDept='" + strdeptCode + "'";
            }
        }
        if (txb_where.Text != "")
        {
            sql += " and (usercode like'%" + txb_where.Text + "%' or username like '%" + txb_where.Text + "%')";
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





    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string userCodes = hf_user.Value;
        UserMessage mgr = new UserMessage(userCodes);
        mgr.DeleteUser();
        this.BindDataGrid();
    }
    #endregion

    #region 查询
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion

    protected void btn_resetPwd_click(object sender, EventArgs e)
    {
        string userCode = hf_user.Value;
        int ret = server.ExecuteNonQuery("update bill_users set userPwd=userCode where userCode='" + userCode + "' ");
        if (ret > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('密码初始化为用户编号，请及时修改密码');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('初始化失败,请与开发商联系！');", true);
        }

    }

}