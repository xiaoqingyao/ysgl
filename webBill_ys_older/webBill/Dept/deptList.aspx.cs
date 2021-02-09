using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Models;
using Bll.UserProperty;
using Dal;

public partial class Dept_deptList : BasePage
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
                hf_dept.Value = Request.QueryString["deptCode"].ToString().Trim();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["deptCode"]).Trim()))
                {
                    string deptname = server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                    if (!string.IsNullOrEmpty(deptname))
                    {
                        this.Label1.Text = "当前部门：[" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "]" + deptname;
                    }

                }

                this.BindDataGrid();
            }
        }
    }

    // 绑定
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


        IList<Bill_Departments> list;
        string deptcode = Request.QueryString["deptCode"].ToString().Trim();
        if (string.IsNullOrEmpty(deptcode))
        {
            SysManager sysMgr = new SysManager();
            list = GetAllDept(arrpage[0], arrpage[1], out icount);
        }
        else
        {
            //修改是否包含下级 Edit by Lvcc
            DepartmentManager deptMgr = new DepartmentManager(deptcode);
            if (deptMgr.dept == null)
            {
                list = new List<Bill_Departments>();
            }
            else if (this.chkNextLevel.Checked)
            {
                list = deptMgr.GetAllChild(arrpage[0], arrpage[1], out icount);
            }
            else
            {
                //不包含下级
                list = deptMgr.GetListWithOutChild(deptcode, arrpage[0], arrpage[1], out icount);
            }

        }

        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;//==0?1:icount;
        //----------给gridview赋值
        this.myGrid.DataSource = list;
        this.myGrid.DataBind();


    }
    public IList<Bill_Departments> GetAllDept(int pagefrm, int pageto, out int count)
    {
        return GetAllDept1(pagefrm, pageto, out count);
    }
    public IList<Bill_Departments> GetAllDept1(int pagefrm, int pageto, out int count)
    {
        string sql = "select deptCode,deptName,sjDeptCode,deptStatus,IsSell,deptJianma,forU8id,Row_Number()over(order by deptCode) as crow from bill_departments where 1=1 ";
        string strsqlcount = "select count(*) from bill_departments where 1=1 ";//deptStatus !='D'

        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} order  by t.deptCode asc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return ListMaker(strsqlframe, null);
    }
    public IList<Bill_Departments> ListMaker(string tempsql, SqlParameter[] sps)
    {
        DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
        IList<Bill_Departments> list = new List<Bill_Departments>();
        foreach (DataRow dr in dt.Rows)
        {
            Bill_Departments dept = new Bill_Departments();
            dept.DeptCode = Convert.ToString(dr["deptCode"]);
            dept.DeptName = Convert.ToString(dr["DeptName"]);
            dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
            dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
            dept.isSell = Convert.ToString(dr["IsSell"]);
            dept.deptJianma = Convert.ToString(dr["deptJianma"]);
            dept.forU8id = Convert.ToString(dr["forU8id"]);
            list.Add(dept);
        }
        return list;
    }


    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #region 包含下级
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    #endregion



    #region 删除
    protected void btn_dele_Click(object sender, EventArgs e)
    {
        string str_deptcode = "";
        int sel_count = 0;
        //for (int i = 0; i < myGrid.Items.Count; i++)
        //{
        //    str_deptcode = hf_user.Value;
        //    //CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
        //    //if (str_deptcode!= null)
        //    //{
        //        str_deptcode = myGrid.Items[i].Cells[1].Text;
        //        sel_count += 1;
        //    //}
        //}

        //if (sel_count == 0)
        //{
        //    Page.RegisterStartupScript("", "<script>window.alert('请选择要删除的部门！');</script>");
        //}
        //else if (sel_count > 1)
        //{
        //    Page.RegisterStartupScript("", "<script>window.alert('每次只允许删除一个部门！');</script>");
        //}
        //else
        //{
        string deptCodes = hf_user.Value;
        //(new Departments()).GetNextLevelDepartments(str_deptcode, "", true);//获取所有下级单位
        deptCodes = "'" + deptCodes.Replace(",", "','") + "'";
        if (server.GetCellValue("select count(1) from bill_departments where sjdeptcode in (" + deptCodes + ")") != "0"
            || server.GetCellValue("select count(1) from bill_main where billDept in (" + deptCodes + ")") != "0"
            || server.GetCellValue("select count(1) from bill_users where userDept in (" + deptCodes + ")") != "0"
            )
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门处于使用状态,禁止删除！');", true);
            return;
        }

        if (server.ExecuteNonQuery("delete from bill_departments where deptCode in (" + deptCodes + ")") == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
            this.BindDataGrid();
        }
        //}
    }
    #endregion



    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            System.Web.UI.HtmlControls.HtmlInputButton btnSelectZg = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSetZg");
            System.Web.UI.HtmlControls.HtmlInputButton btnSelectLd = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSetLd");
            if (btnSelectZg == null)
            { }
            else
            {
                btnSelectZg.Attributes.Add("onclick", "openSelectZg('" + e.Item.Cells[0].Text.ToString().Trim() + "');");
                btnSelectLd.Attributes.Add("onclick", "openSelectLd('" + e.Item.Cells[0].Text.ToString().Trim() + "');");
            }

            if (Convert.ToString(e.Item.Cells[6].Text) == "1")
            {
                e.Item.Cells[6].Text = "启用";
            }
            else if (Convert.ToString(e.Item.Cells[6].Text) == "D" || Convert.ToString(e.Item.Cells[6].Text) == "0")
            {
                e.Item.Cells[6].Text = "禁用";
            }
        }
    }
}
