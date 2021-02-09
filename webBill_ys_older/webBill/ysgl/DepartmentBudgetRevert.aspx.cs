using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;

/// <summary>
/// 部门预算驳回  将部门未使用的金额（不包括占用的）驳回到集团全年预算中
/// 该功能仅适用于使用预算分解的企业
/// Edit By Lvcc
/// </summary>
public partial class webBill_ysgl_DepartmentBudgetRevert : System.Web.UI.Page
{
    string userCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysMgr = new YsManager();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            userCode = Session["userCode"].ToString().Trim();
        }
        if (!IsPostBack)
        {
            BindControl();
            Bind();
        }
    }

    /// <summary>
    /// 页面空间初始化
    /// </summary>
    private void BindControl()
    {
        #region 绑定年份
        string strSelectNian = "select distinct nian from bill_ysgc order by nian desc";
        this.ddlDateYear.DataSource = server.GetDataTable(strSelectNian,null);
        this.ddlDateYear.DataTextField = "nian";
        this.ddlDateYear.DataValueField = "nian";
        this.ddlDateYear.DataBind();
        #endregion
        #region 绑定人员管理下的部门
        ddlDept.Items.Clear();
        if (!userCode.Equals(""))
        {
            DataTable dtDept = new Departments().GetUserRightDepartmentsDT(userCode, "");
            this.ddlDept.DataSource = dtDept;
            this.ddlDept.DataTextField = "deptName";
            this.ddlDept.DataValueField = "deptCode";
            this.ddlDept.DataBind();
        }
        #endregion
    }
    /// <summary>
    /// 页面数据初始化绑定
    /// </summary>
    private void Bind()
    {
        //绑定gridview
        string strSql = "select (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=a.yskm) as yskmShowName,(select '['+gcbh+']'+xmmc from bill_ysgc where gcbh=a.gcbh) as gcbhShowName,gcbh,billCode,yskm,ysje,ysDept,ysType from bill_ysmxb a where ysDept=@deptCode and left(gcbh,4)=@Year and ysje>0";
        if (this.ddlDept.SelectedValue!=null&&this.ddlDateYear.SelectedValue!=null)
        {
            string strCurrentDept = this.ddlDept.SelectedValue.Trim();
            string strCurrentYear=this.ddlDateYear.SelectedValue.Trim();
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@deptCode", strCurrentDept), new SqlParameter("@Year", strCurrentYear) };
            DataTable dtRel = server.GetDataTable(strSql, arrSp);
            this.myGridView.DataSource = dtRel;
            this.myGridView.DataBind();
        }
    }

    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnbtnRevertClick(object sender, EventArgs e) {
        if (ddlDateYear.SelectedValue==null||this.ddlDept.SelectedValue==null){return;}
        string strYear = this.ddlDateYear.SelectedValue.Trim();
        string strDept = this.ddlDept.SelectedValue.Trim();
        int iRowsCount = this.myGridView.Rows.Count;
        List<Bill_Ysmxb> lstYbbxMxb=new List<Bill_Ysmxb>();
        for (int i = 0; i < iRowsCount; i++)
        {
            //获取
            string strCanReverAmount = this.myGridView.Rows[i].Cells[7].Text.Trim();
            decimal deCanRevertAmount = 0;
            if (!decimal.TryParse(strCanReverAmount,out deCanRevertAmount)){
                continue;
            }
            string strYskm = this.myGridView.Rows[i].Cells[1].Text.Trim();//科目编号
            string strGcbh= this.myGridView.Rows[i].Cells[0].Text.Trim();//过程编号
            string strBillCode=this.myGridView.Rows[i].Cells[8].Text.Trim();//主表编号
            if (strYskm.Equals("")||strGcbh.Equals(""))
            {
                continue;
            }
            //添加到list
            Bill_Ysmxb modelYsmxb = new Bill_Ysmxb();
            modelYsmxb.Yskm = strYskm;
            modelYsmxb.YsDept = strDept;
            modelYsmxb.Gcbh = strGcbh;
            modelYsmxb.Ysje = deCanRevertAmount;//用预算金额来保存可驳回的金额
            modelYsmxb.BillCode = strBillCode;
            lstYbbxMxb.Add(modelYsmxb);
        }
        if (lstYbbxMxb.Count<=0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('没有要驳回的记录！');", true);
            return;
        }
        string strErrorMsg = "";
        if (ysMgr.DeptBudgetRevert(strDept,strYear,userCode,out strErrorMsg) > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('驳回成功！');", true);
            Bind();
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('驳回失败，原因：" + strErrorMsg + "！');", true);
        }
    }
    /// <summary>
    /// gridview行绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnMyGridViewRowDataBound(object sender, GridViewRowEventArgs e) {
        if (e == null) { return; }
        if (e.Row.RowType == DataControlRowType.DataRow) {
            string strGcbh = e.Row.Cells[0].Text;
            string strKmbh = e.Row.Cells[1].Text;
            string strDeptCode = this.ddlDept.SelectedValue.Trim();
            decimal ysje = ysMgr.GetYueYs(strGcbh, strDeptCode, strKmbh);//预算金额
            decimal hfje = ysMgr.GetYueHf(strGcbh, strDeptCode, strKmbh);//花费金额
            decimal zyje = ysMgr.GetYueNotEndje(strGcbh, strDeptCode, strKmbh);//占用金额
            e.Row.Cells[4].Text = ysje.ToString("N02");
            e.Row.Cells[5].Text = hfje.ToString("N02");
            e.Row.Cells[6].Text = (-zyje).ToString("N02");
            e.Row.Cells[7].Text = (ysje - hfje + zyje).ToString();
        }
    }

    /// <summary>
    /// 年份SelectIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnDDlDateYearSelectIndexChanged(object sender, EventArgs e) {
        Bind();
    }
    /// <summary>
    /// 部门SelectIndexChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void OnDDlDeptSelectIndexChanged(object sender, EventArgs e) {
        Bind();
    }
}
