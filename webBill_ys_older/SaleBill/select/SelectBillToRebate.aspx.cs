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
using System.Text;
using Bll.SaleBill;
using System.Web.UI.MobileControls;
using System.Collections.Generic;
using Models;

public partial class SaleBill_select_SelectBillToRebate : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strUserCode = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
            ClientScript.RegisterArrayDeclaration("deptAll", GetDeptAll());
            if (!IsPostBack)
            {
                ddlStatus.SelectedValue = "0";
                BindDataGrid();
            }
        }
    }

    /// <summary>
    /// 绑定列表页
    /// </summary>
    protected void BindDataGrid()
    {
        //        string sql = @"select a.billCode,a.billName,a.stepID,a.billDate,(select '['+deptCode+']'+deptName from dbo.bill_departments where 
        //                        a.billDept=deptCode) as deptName,b.* from bill_main a,
        //                        (select distinct TruckCode,code,SaleDeptCode,isJC,InvoiceCode,BillingDate,truckMsg.fph,truckMsg.fprq from T_BillingApplication billApp,V_TruckMsg truckMsg where billApp.TruckCode=truckMsg.cjh) b where a.billCode=b.code";
        string sql = @"select a.billCode,a.billName,a.stepID,a.billDate,(select '['+deptCode+']'+deptName from dbo.bill_departments where a.billDept=deptCode) as deptName,b.* from bill_main a,
                        (select TruckCode,code,SaleDeptCode,isJC,InvoiceCode,
BillingDate,truckMsg.fph,truckMsg.fprq from T_BillingApplication billApp, (select  c.fph,c.fprq,c.cjh from V_TruckMsg c , (select max(fprq) as fprq,cjh from V_TruckMsg group by cjh) d where c.cjh=d.cjh and c.fprq=d.fprq  ) truckMsg 
where  billApp.TruckCode=truckMsg.cjh  ) b where a.billCode=b.code ";
        string deptCodes = (new Departments()).GetUserRightDepartments(strUserCode, "");
        sql += " and a.billDept in (" + deptCodes + ") and a.stepID='end' ";//权限单位+审核通过

        #region 查询条件

        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >=cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <=cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        //申请单位
        if (!this.txtAppDept.Text.Trim().Equals(""))
        {
            string strAppDept = this.txtAppDept.Text.Trim();
            if (strAppDept.IndexOf("]") > -1)
            {
                strAppDept = strAppDept.Substring(1, strAppDept.IndexOf("]") - 1);
            }
            sql += " and a.billDept = '" + strAppDept + "'";
        }
        //单号
        if (this.txtBillCode.Text != "")
        {
            sql += " and a.billCode like '%" + this.txtBillCode.Text + "%' ";
        }
        //车架号
        if (txtCarCode.Text != "")
        {
            sql += " and TruckCode like '%" + txtCarCode.Text + "%'";
        }
        //状态
        if (ddlStatus.SelectedValue != "")
        {
            if (ddlStatus.SelectedValue.Equals("1"))
            {
                sql += " and isnull(BillingDate,'')!=''";
            }
            else
            {
                sql += " and isnull(BillingDate,'')=''";
            }

        }
        sql += " order by a.billCode desc";
        #endregion

        DataSet temp = server.GetDataSet(sql);
        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Tables[0].Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();
        #endregion
        if (temp.Tables[0].Rows.Count == 0)
        {
            temp = null;
        }

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void btn_unBilling_click(object sender, EventArgs e)
    {
        int iGridViewCount = this.myGrid.Items.Count;
        List<T_BillingApplication> lstTruck = new List<T_BillingApplication>();
        string strErrMsg = ""; int iCount = 0;
        for (int i = 0; i < iGridViewCount; i++)
        {
            CheckBox cb = this.myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            if (cb != null)
            {
                if (cb.Checked)
                {
                    string strdatetime = this.myGrid.Items[i].Cells[7].Text.Trim();
                    strdatetime = strdatetime.Equals("&nbsp;") ? "" : strdatetime;
                    if (strdatetime != "")
                    {
                        T_BillingApplication modelBillingApplication = new T_BillingApplication();
                        string strTruckCode = this.myGrid.Items[i].Cells[2].Text.Trim();
                        modelBillingApplication.TruckCode = strTruckCode;
                        string strCode = this.myGrid.Items[i].Cells[1].Text.Trim();
                        modelBillingApplication.Code = strCode;
                        lstTruck.Add(modelBillingApplication);
                    }
                }
            }
            iCount++;
        }
        if (lstTruck.Count == 0 && iCount == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要操作的记录！');", true);
        }
        else if (lstTruck.Count == 0 && iCount != 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择已经开票的记录！');", true);
        }
        else
        {
            try
            {
                int iRel = new T_SaleFeeAllocationNoteBLL().DoRevertRebate(lstTruck, Session["userCode"].ToString(), out strErrMsg);
                if (iRel <= 0)
                {
                    throw new Exception("退票失败，请联系管理员解决！");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('退票成功！');", true);
                    BindDataGrid();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('退票失败，原因：" + ex.Message + "！');", true);
            }
        }
    }
    /// <summary>
    /// 确认开票
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Billing_click(object sender, EventArgs e)
    {
        int iGridViewCount = this.myGrid.Items.Count;
        List<string> lstTruckCode = new List<string>();
        string strErrMsg = "";
        int iCount = 0;
        for (int i = 0; i < iGridViewCount; i++)
        {
            CheckBox cb = this.myGrid.Items[i].FindControl("CheckBox1") as CheckBox;
            if (cb != null)
            {
                if (cb.Checked)
                {
                    string strdatetime = this.myGrid.Items[i].Cells[7].Text.Trim();
                    strdatetime = strdatetime.Equals("&nbsp;") ? "" : strdatetime;
                    if (strdatetime == "")
                    {
                        string strTruckCode = this.myGrid.Items[i].Cells[2].Text.Trim();
                        lstTruckCode.Add(strTruckCode);
                    }

                }
            }
            iCount++;
        }
        if (lstTruckCode.Count == 0 && iCount == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要开票的记录！');", true);
        }
        else if (lstTruckCode.Count == 0 && iCount != 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择未开票的记录，勿重复开票！');", true);
        }
        else
        {
            try
            {
                int iRel = new T_SaleFeeAllocationNoteBLL().DoRebate(lstTruckCode, out strErrMsg);
                if (iRel <= 0)
                {
                    // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要开票的记录！');", true);
                    // BindDataGrid();
                    throw new Exception("已经全部开票！");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开票成功！');", true);
                    BindDataGrid();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开票失败，原因：" + ex.Message + "！');", true);
            }
        }
    }

    ///// <summary>
    ///// 确认开票
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btn_DoRebateNote_Click(object sender, EventArgs e) {
    //    string strBillCode = "";// this.hdBillCode.Value;
    //    if (strBillCode.Equals(""))
    //    {
    //        showMessage("请先选中行！", false, "");
    //    }
    //    else { 
    //        string strMsg="";
    //        try
    //        {
    //            int iRel = new T_SaleFeeAllocationNoteBLL().MakeRebateNote(strBillCode, out strMsg);
    //            if (iRel > 0)
    //            {
    //                showMessage("生成记录成功！", true, "");
    //            }
    //            else {
    //                throw new Exception("未知错误！");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            showMessage("生成记录失败，原因："+ex.Message,false,"");
    //        }
    //    }
    //}

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #region 私有
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }

    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptCode+']'+deptName as deptName from bill_departments where  IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        string script = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptName"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);

        }

        return script;
    }

    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion
    #endregion
}
