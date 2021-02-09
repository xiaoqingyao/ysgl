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
using Bll.UserProperty;
using WorkFlowLibrary.WorkFlowBll;
using Bll;
using System.Collections.Generic;

public partial class webBill_ysgl_ystz : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    public string DetailURL = "YstzDetailNew.aspx";//默认的预算调整详细页URL
    string strNowDeptCode = "";
    string strNowDeptName = "";
    bool boYstzNeedAudit = new Bll.ConfigBLL().GetValueByKey("YstzNeedAudit").Equals("0") ? false : true;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            string usercode = Session["userCode"].ToString().Trim();
            string strsfmj = new ConfigBLL().GetValueByKey("deptjc");// server.GetCellValue("select avalue from dbo.t_Config where akey=''");
            if (!isTopDept("y", usercode) && strsfmj == "Y")
            {
                //获取当前用户所在的部门编号及其部门名称
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            }
            else
            {
                //上级部门
                strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                strNowDeptName = server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            }
            string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
            if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y")
            {
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strNowDeptCode + "') order by deptCode", null);
            }
            else
            {
                dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ") and deptCode not in ('" + strNowDeptCode + "') order by deptCode ", null);
            }

            if (!IsPostBack)
            {
                #region 绑定人员管理下的部门
                if (!strNowDeptCode.Equals(""))
                {
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
                        this.LaDept.Items.Insert(0, new ListItem("--全部--", ""));
                        this.LaDept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));
                        this.LaDept.SelectedIndex = 0;
                    }
                }

                #endregion
                this.BindDataGrid();
            }
            this.hd_YstzNeedAudit.Value = boYstzNeedAudit ? "1" : "0";
            //获取url配置项
            string ystzUrl = new Bll.ConfigBLL().GetValueByKey("ystzUrl");
            if (!string.IsNullOrEmpty(DetailURL))
            {
                DetailURL = ystzUrl;
            }
        }
    }
    protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
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
        string deptGuid = this.LaDept.Text.Trim();
        string strselectsql = "";
        if (!string.IsNullOrEmpty(deptGuid))
        {
            strselectsql += " and  billDept='" + deptGuid + "'";
        }
        if (!string.IsNullOrEmpty(ddlstatus.SelectedValue))
        {
            strselectsql += " and stepid='" + this.ddlstatus.SelectedValue + "'";
        }
        string sql = @"select billJe ,billName2,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid,billCode
                ,(select username from bill_users where usercode=billuser) as billUser,billdate 
                ,( select xmmc from bill_ysgc 	where convert(char(10),kssj,121)<=convert(char(10),billdate,121) and  convert(char(10),jzsj,121)>=convert(char(10),billdate,121) and ystype!='0') as gcmc 
                ,(select top 1   mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind
            ,(select dicname from bill_dataDic where dictype='18' and diccode=isnull(bill_main.dydj,'02')) as dydjname
            ,Row_Number()over( order by billDate desc) as crow from bill_main where flowID='ystz'" + strselectsql;

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

    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    /// <summary>
    /// 导出表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        dic.Add("gcmc", "目标预算过程");
        dic.Add("billDept", "填报单位");
        dic.Add("billUser", "制单人");
        dic.Add("billDate", "单据日期");
        dic.Add("billJe", "调整金额");
        dic.Add("stepid", "审批状态");
        dic.Add("billName2", "摘要");
        dic.Add("dydjname", "调整类型");
        dic.Add("mind", "驳回理由");
        new ExcelHelper().ExpExcel(dt, "ExportFile", dic);
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[6].Text = state;
            }
        }
    }
    protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}