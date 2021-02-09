﻿using System;
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
public partial class webBill_ysgl_ysnzjList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    string strCsDeptCode;
    string strCsdeptcode;
    string strCsdeptname;
    string strDeptCodes;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["deptcode"] != null)
            {
                strCsDeptCode = Request["deptcode"].ToString();
            }
            string usercode = Session["userCode"].ToString().Trim();
            strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            if (isTopDept("y", usercode))
            {
                if (Request["deptcode"] != null)
                {
                    strCsDeptCode = Request["deptcode"].ToString().Trim();
                    strCsdeptcode = server.GetCellValue("select deptcode from bill_departments where deptcode='" + strCsDeptCode + "'");
                    strCsdeptname = server.GetCellValue("select deptName from bill_departments where deptcode='" + strCsDeptCode + "'");

                    dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in (" + strCsdeptcode + ")", null);

                }
                else
                {
                    //获取当前用户所在的部门编号及其部门名称
                    strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                    strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                    dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strNowDeptCode + "')", null);

                }


            }
            else
            {
                //上级部门
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            }
            if (!IsPostBack)
            {

                #region 绑定人员管理下的部门
                //if (!strNowDeptCode.Equals(""))
                //{
                //获取人员管理下的部门
                if (strDeptCodes != "")
                {
                    if (dtuserRightDept.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                        {
                            ListItem li = new ListItem();
                            li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                            li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                            this.LaDept.Items.Add(li);
                        }
                    }
                    if (strCsdeptcode != "" && strCsdeptcode != null)
                    {
                        this.LaDept.Items.Insert(0, new ListItem("[" + strCsdeptcode + "]" + strCsdeptname, strCsdeptcode));
                        this.LaDept.SelectedIndex = 0;


                    }
                    else
                    {
                        this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                        this.LaDept.SelectedIndex = 0;
                    }
                }
                //}
                #endregion
                string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
                DataTable selectdt = server.GetDataTable(selectndsql, null);
                drpSelectNd.DataSource = selectdt;
                drpSelectNd.DataTextField = "xmmc";
                drpSelectNd.DataValueField = "nian";
                drpSelectNd.DataBind();

                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("ysnzj");
                //string gcbh = Page.Request.QueryString["gcbh"].ToString().Trim();
                //DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");
                //this.Label1.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();
                this.BindDataGrid();
            }
        }
    }

    void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
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
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string deptGuid = this.LaDept.SelectedValue.ToString().Trim();
        string sql = "select stepid,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,billdate,billje,(select mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind ,Row_Number()over( order by billDate desc) as crow  from bill_main where flowID='ysnzj' and  billDept='" + deptGuid + "'";

        if (drpSelectNd.SelectedValue!=null)
        {
            sql += " and left(billname,4)='" + drpSelectNd.SelectedValue + "'";
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strdeptcode = this.LaDept.SelectedValue.ToString().Trim();
        Response.Redirect("yszjFrame.aspx?Ctrl=ysnzj&deptcode=" + strdeptcode + "&page=ysnzj");
    }


    //protected void Button5_Click(object sender, EventArgs e)
    //{
    //    string strdeptcode = this.LaDept.SelectedValue.ToString().Trim();
    //    Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode);
    //}
    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }



    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            string billcode = e.Item.Cells[0].Text;
            WorkFlowRecordManager bll = new WorkFlowRecordManager();

            if (e.Item.Cells[5].Text == "end")
            {
                e.Item.Cells[5].Text = "审批通过";
            }
            else
            {
                string state = bll.WFState(billcode);
                e.Item.Cells[5].Text = state;
            }
        }
    }
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ysnzjEdit.aspx?type=edit&billCode=" + choosebill.Value);
    }
    /// <summary>
    /// 查询是不是二级单位
    /// </summary>
    /// <param name="strus">是人员CODE？y:n</param>
    /// <param name="usercode">人员CODE</param>
    /// <returns></returns>
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}