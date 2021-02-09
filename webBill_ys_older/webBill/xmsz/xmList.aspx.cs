using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Dal.Bills;

public partial class Dept_deptList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strxkfx = "";
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

                //if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["deptCode"]).Trim()))
                if (Request["deptCode"] != null)
                {

                    this.Label1.Text = "当前部门：[" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "]" + server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                }
                if (string.IsNullOrEmpty(Request["deptCode"]))//(Page.Request.QueryString["deptCode"].ToString().Trim() == "")
                {
                    this.btn_add.Enabled = false;
                    //this.btn_edit.Enabled = false;
                    btn_Copy.Disabled = true;
                }
                if (!string.IsNullOrEmpty(Request["xkfx"]))
                {
                    strxkfx = Request["xkfx"].ToString();

                }
                if ((!string.IsNullOrEmpty(strxkfx) && strxkfx == "1") || (!string.IsNullOrEmpty(Request["xmys"]) && Request["xmys"].ToString() == "1") || (!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"] == "1") || (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"] == "1"))
                {
                    this.btn_add.Visible = false;
                    this.btn_Copy.Visible = false;
                    this.btn_dele.Visible = false;
                    this.btn_edit.Visible = false;
                    if ((!string.IsNullOrEmpty(strxkfx) && strxkfx == "1"))
                    {
                        btn_ystb.Visible = true;
                        btn_xmyshz.Visible = false;
                    }
                    else
                    {
                        btn_ystb.Visible = false;
                        btn_xmyshz.Visible = true;
                    }

                }
                else
                {
                    btn_ystb.Visible = false;
                }

                if ((!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"] == "1") || (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"] == "1"))
                {
                    btn_xmyshz.Visible = false;
                    btn_ystb.Visible = true;
                    this.btn_ystb.Text = "预算追加";
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
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 150);
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
        string sql = @"select Row_Number()over(order by xmcode asc) as crow ,(case isnull(xmStatus,'1') when '1' then '正常' when '0' then '停用' end) as xmStatus
                                ,xmcode,xmname,sjxm,(select top 1 xmname from bill_xm b where b.xmcode=a.sjxm) as sjxmname,xmdept
                                ,(select '['+deptcode+']'+deptname from bill_departments where deptcode=a.xmdept) as xmdeptname 
                        from bill_xm a where 1=1 ";
        if ((!string.IsNullOrEmpty(Request["xkfx"]) && Request["xkfx"].ToString() == "1") || (!string.IsNullOrEmpty(Request["xmys"]) && Request["xmys"].ToString() == "1") || (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"].ToString() == "1"))
        {
            //string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            //sql += " and xmdept in (" + deptCodes + ")";
        }
        else
        {

            if (Request["deptCode"] == null || Request["deptCode"].ToString() == "")//(Page.Request.QueryString["deptCode"].ToString().Trim() == "")
            {
                string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
                sql += " and xmdept in (" + deptCodes + ")";
            }
            else
            {
                if (string.IsNullOrEmpty(Request["xmzj"]))
                {
                    string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
                    sql += " and xmdept in (" + deptCodes + ")";
                }
               
            }
        }

        if (txb_where.Text != "")
        {
            sql += " and (xmname like '%" + txb_where.Text + "%' or xmname like '%" + txb_where.Text + "%')";
        }
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        //Response.Write(strsqlframe);
        return server.GetDataTable(strsqlframe, null);
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

    #region 修改
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string str_deptcode = "";
        int sel_count = 0;
        string dept = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                str_deptcode = myGrid.Items[i].Cells[1].Text;
                dept = myGrid.Items[i].Cells[5].Text;
                if (dept.Length > 1)
                {
                    dept = dept.Substring(1, dept.LastIndexOf(']') - 1);
                }
                sel_count++;
            }
        }

        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择要修改的项目！');</script>");
        }
        else if (sel_count > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('只能选择一条项目修改！');</script>");
        }
        else if (!string.IsNullOrEmpty(Page.Request.QueryString["deptCode"].ToString().Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('xmDetails.aspx?type=edit&xmCode=" + str_deptcode + "&deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim() + "');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('xmDetails.aspx?type=edit&xmCode=" + str_deptcode + "&deptCode=" + dept + "');", true);
        }

    }
    #endregion

    #region 删除
    protected void btn_dele_Click(object sender, EventArgs e)
    {
        string str_deptcode = "";
        int sel_count = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                str_deptcode = myGrid.Items[i].Cells[1].Text;
                sel_count += 1;
            }
        }
        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择要删除的项目！');</script>");
        }
        else if (sel_count > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('每次只允许删除一个项目！');</script>");
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(str_deptcode, "", true);//获取所有下级单位

            //返回值是true 就不删除 否则删除
            XmDeptNdDal xmdept = new XmDeptNdDal();
            if (xmdept.IsExsitXmCode(deptCodes))//是
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('项目有预算，禁止删除！');", true);
                return;
            }

            //    deptCodes = "'" + deptCodes.Replace(",", "','") + "'";
            if (server.GetCellValue("select count(1) from bill_xm where sjxm in (" + deptCodes + ")") != "0"
                //|| server.GetCellValue("select count(1) from bill_main where billDept in (" + deptCodes + ")") != "0"
                //|| server.GetCellValue("select count(1) from bill_users where userDept in (" + deptCodes + ")") != "0"
                )
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('项目处于使用状态,禁止删除！');", true);
                return;
            }





            //Response.Write("<script>alert(" + deptCodes + ");</script>");
            if (server.ExecuteNonQuery("delete from bill_xm where xmcode in (" + deptCodes + ")") == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                this.BindDataGrid();
            }
        }
    }
    #endregion

    #region 添加
    protected void btn_add_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('xmDetails.aspx?type=add&deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim() + "');", true);
    }
    #endregion

    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion
    /// <summary>
    /// 填报
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_ystb_Click(object sender, EventArgs e)
    {
        if ((!string.IsNullOrEmpty(Request["xkfx"]) && Request["xkfx"] == "1") || (!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"] == "1") || (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"] == "1"))
        {
            string deptcode = Request["deptCode"].ToString();
            string str_xmCode = "";
            int sel_count = 0;
            string dept = "";
            for (int i = 0; i < myGrid.Items.Count; i++)
            {
                CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
                if (cbox.Checked == true)
                {
                    str_xmCode = myGrid.Items[i].Cells[1].Text;
                    dept = myGrid.Items[i].Cells[5].Text;
                    if (dept.Length > 1)
                    {
                        dept = dept.Substring(1, dept.LastIndexOf(']') - 1);
                    }
                    sel_count++;
                }
            }
            if (sel_count == 0)
            {
                Page.RegisterStartupScript("", "<script>window.alert('请选择项目！');</script>");
            }
            else if (sel_count > 1)
            {
                Page.RegisterStartupScript("", "<script>window.alert('只能选择一条项目！');</script>");
            }
            else
            {
                if (!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"] == "1")
                {
                    Response.Redirect("../ysgl/yszjFrame.aspx?deptcode=" + deptcode + "&page=xmyszj&isdz=1&xmzj=" + Request["xmzj"].ToString()+"&xmcode=" + str_xmCode);
                }
                else if (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"] == "1")
                {
                   Response.Redirect("../ysgl/yszjFrame.aspx?deptcode=" + deptcode + "&page=xmyszj&isdz=1&ishz=1&isxm=" + Request["isxm"].ToString()+"&xmcode=" + str_xmCode);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request["xkfx"]) && Request["xkfx"] == "1")
                    {
                        string nd = Request["nd"].ToString();

                        string yskmtype = Request["yskmtype"].ToString();
                        string tbtype = Request["tbtype"].ToString();
                        string limittotal = Request["limittotal"].ToString();
                        string jecheckflg = Request["jecheckflg"];
                        string isdz = Request["isdz"].ToString();//是否是大智学校的标记
                        string xkfx = Request["xkfx"];//新开分校填报
                        Response.Redirect("../ysglnew/cwtbDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=ystb" + "&yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg + "&isdz=" + isdz + "&xkfx=" + xkfx + "&xmcode=" + str_xmCode);

                    }

                }
            }
        }
    }
    /// <summary>
    /// 汇总
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_xmyshz_Click(object sender, EventArgs e)
    {
        // deptCode=0101&nd=2016&tbtype=&xmys=1
        string deptcode = Request["deptCode"].ToString();
        string tbtype = Request["tbtype"].ToString();

        string xmys = Request["xmys"].ToString();//是否是项目预算
        string nd = Request["nd"].ToString();

        string str_xmCode = "";
        int sel_count = 0;
        string dept = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                str_xmCode += myGrid.Items[i].Cells[1].Text + ",";
                dept = myGrid.Items[i].Cells[5].Text;

                sel_count++;
            }
        }
        //if (!string.IsNullOrEmpty(str_xmCode))
        //{
        //    str_xmCode = str_xmCode.Substring(0,str_xmCode.Length-1);
        //}

        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择项目！');</script>");
        }
        else
        {
            Response.Redirect("../ysglnew/ystbHzDetail.aspx?deptCode=" + deptcode + "&nd=" + nd + "&tbtype=" + tbtype + "&xmcode=" + str_xmCode);
        }
    }
}
