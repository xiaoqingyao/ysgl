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
using WorkFlowLibrary.WorkFlowBll;
using System.Collections.Generic;

public partial class ysgl_yszjList : BasePage
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
            string strsfmj = new Bll.ConfigBLL().GetValueByKey("deptjc");//是否预算到末级部门
            string addWhere = " and sjdeptCode='000001' ";
            if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")
            {
                addWhere = "";
            }
            if (Request["deptcode"] != null)
            {
                strCsDeptCode = Request["deptcode"].ToString().Trim();
                strCsdeptcode = server.GetCellValue("select deptcode from bill_departments where deptcode='" + strCsDeptCode + "'");
                strCsdeptname = server.GetCellValue("select deptName from bill_departments where deptcode='" + strCsDeptCode + "'");

                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strCsdeptcode + "')" + addWhere, null);
            }
            //如果是三级部门并且不预算到三级部门
            if (!isTopDept("y", usercode) && strsfmj != "Y")
            {
                //获取上级部门
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            }
            else
            {
                //获取当前用户所在的部门编号及其部门名称
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");

            }
            string strrightsql = "select deptCode,deptName from bill_departments where   deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strNowDeptCode + "')" + addWhere + " order by deptcode";
            dtuserRightDept = server.GetDataTable(strrightsql, null);
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
                        this.LaDept.Items.Insert(0, new ListItem("-全部-", ""));
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

                string strndsql = @"select * from bill_ysgc where isnull(yue,'')='' order by nian desc";
                DataTable dtnd = server.GetDataTable(strndsql, null);
                if (dtnd != null)
                {
                    drpnd.DataValueField = "nian";
                    drpnd.DataTextField = "xmmc";

                }

                drpnd.DataSource = dtnd;
                drpnd.DataBind();
                this.drpnd.Items.Insert(1, new ListItem("-全部-", ""));
                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("yszj");
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
    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        //string deptGuid = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());

        string deptGuid = this.LaDept.SelectedValue.ToString().Trim();

        string sql = @"select (select '['+xmCode+']'+xmName from bill_xm where xmCode=note3 ) as xm, stepid,billCode
                    ,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser
                    ,billdate,billje,  (select top 1 mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord 
                    where billCode=bill_main.billCode) and rdstate='3') as mind,(select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept)as billDeptname
                                    ,Row_Number()over( order by billDate desc) as crow  from bill_main where flowID='yszj' and charindex('pl_', billName,1)=0";

        if (!string.IsNullOrEmpty(deptGuid))
        {
            sql += " and  billDept='" + deptGuid + "'";
        }
        string strnd = drpnd.SelectedValue;
        if (!string.IsNullOrEmpty(strnd))
        {
            sql += " and LEFT(convert(char(10),billDate,121),4)='" + strnd + "'";
        }
        if (!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"].ToString() == "1")
        {
            sql += " and isnull(note3,'')!=''";
        }
        else
        {
            sql += " and isnull(note3,'')=''";
        }
        //if (!string.IsNullOrEmpty(ddlstatus.SelectedValue))
        //{
        //    sql += " and stepid='" + this.ddlstatus.SelectedValue + "'";
        //}
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

    //protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDataGrid();
    //}

    protected void Button1_Click(object sender, EventArgs e)
    {
        string strdeptcode = this.LaDept.SelectedValue.ToString().Trim();
        string strisdz = "";
        string strurl = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
            if (!string.IsNullOrEmpty(Request["xmzj"]) && Request["xmzj"].ToString() == "1")
            {
                strurl = "../xmsz/xmList.aspx?deptCode=" + strdeptcode + "&xmzj=" + Request["xmzj"];
            }
            else
            {
                strurl = "yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&isdz=" + strisdz;
            }
        }
        else
        {
            strurl = "yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj&isdz=" + strisdz;
        }
        Response.Redirect(strurl);
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
        if (string.IsNullOrEmpty(Request["xmzj"]))
        {
            e.Item.Cells[7].CssClass = "hiddenbill";
        }
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
        Response.Redirect("yszjEdit.aspx?type=edit&billCode=" + choosebill.Value);
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
    protected void btn_Export_Click(object sender, EventArgs e)
    {
        int count = 0;
        DataTable dt = GetData(0, 99999999, out count);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strzt = dt.Rows[i]["stepid"].ToString();
            string billcode = dt.Rows[i]["billCode"].ToString();
            WorkFlowRecordManager bll = new WorkFlowRecordManager();

            if (strzt == "end")
            {
                dt.Rows[i]["stepid"] = "审批通过";
            }
            else
            {
                string state = bll.WFState(billcode);
                dt.Rows[i]["stepid"] = state;
            }
        }

        Dictionary<string, string> dic = new Dictionary<String, String>();
        dic.Add("billCode", "单据编号");
        dic.Add("billName", "预算过程");
        dic.Add("billDeptname", "填报单位");
        dic.Add("billUser", "制单人");
        dic.Add("billDate", "单据日期");
        dic.Add("billJe", "单据金额");
        dic.Add("stepid", "审批状态");
        dic.Add("mind", "驳回理由");
        dic.Add("xm", "项目");
        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);
    }
    /// <summary>
    /// 导出明细
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_exclemx_Click(object sender, EventArgs e)
    {
        //string billcode = choosebill.Value;
        string deptGuid = LaDept.SelectedValue;
        string sql = @" select  billdate ,billdept ,billJe  ,mxb.sm as shuoming,(select deptname from bill_departments where deptCode=main.billDept) as billdeptname
                     ,(select username from bill_users where userCode=main.billUser) as billusername ,(select deptname from bill_departments where deptCode=mxb.ysDept) as ysdeptname,mxb.yskm
                     ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=mxb.yskm) as billyskmname,ysje as zhuijiaje,              
                      LEFT(convert(char(10),main.billDate,121),4)as sj ,stepID,
                      (case when stepID='end' then '审批通过' else '未通过' end) as zt
                      from bill_main main,bill_ysmxb mxb
                      where main.billCode=mxb.billCode and main.flowID='yszj' and ysType='2' ";
        if (!string.IsNullOrEmpty(deptGuid))
        {
            sql += " and main.billDept='" + deptGuid + "'";
        }
        string strnd = drpnd.SelectedValue;
        if (!string.IsNullOrEmpty(strnd))
        {
            sql += " and LEFT(convert(char(10),main.billDate,121),4)='" + strnd + "'";
        }
        if (!string.IsNullOrEmpty(choosebill.Value))
        {
            sql += " and main.billcode='" + choosebill.Value.Trim() + "'";
        }
        sql += " order by main.billDept,billdate  ";
        DataTable temp = server.GetDataTable(sql, null);


        Dictionary<string, string> dic = new Dictionary<String, String>();
        dic.Add("billdate", "单据时间");
        dic.Add("billusername", "制单人");
        dic.Add("billdeptname", "制单部门");
        dic.Add("ysdeptname", "追加部门");
        dic.Add("zt", "审核状态");
        dic.Add("billyskmname", "科目名称");
        dic.Add("billJe", "单据金额");
        dic.Add("zhuijiaje", "追加金额");
        dic.Add("shuoming", "说明");

        new ExcelHelper().ExpExcel(temp, "ExportFile", dic);
    }
}