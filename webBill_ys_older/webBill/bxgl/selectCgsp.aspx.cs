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
using System.Collections.Generic;
using System.Text;

public partial class webBill_fysq_cgspList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    my_fzl.bindClss bindCl = new my_fzl.bindClss();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
            if (!IsPostBack)
            {
                this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("cgsp");
                BindDataGrid();
            }
        }
    }

    protected void BindDataGrid()
    {
        string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as cglb from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and a.billDept in (" + deptCodes + ") and a.stepID='end' and b.cgbh not in (select sqCode from bill_ybbx_fysq where status in ('1','2'))";//权限单位+审核通过+未报销过

        //借款单 过来已经冲减的  begin edit by zyl 2015-3-11
        if (Request["from"] == "jkd")
        {
            string codes = "";
            string sqlTemp = "  select item.note7 from bill_main main,  T_LoanList item where main.billcode =item.ListId and len(item.note7)>0 ";

            if (!string.IsNullOrEmpty(deptCodes))
                sqlTemp += " and main.billDept in(" + deptCodes + ") ";

            DataTable temp1 = server.GetDataTable(sqlTemp, null);
            for (int i = 0; i < temp1.Rows.Count; i++)
                codes += "'" + Convert.ToString(temp1.Rows[i]["note7"]) + "',";

            if (codes.Length > 1)
            {
                codes = codes.Substring(0, codes.Length - 1);

                sql += "  and a.billCode not in (" + codes + ")";
            }

        }
        //借款单 过来已经冲减的   end



        #region 查询条件

        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        if (this.txtcode.Text != "")
        {
            sql += " and a.billCode like '%" + txtcode.Text.Trim() + "%'";
        }
        sql += " order by b.sj desc";
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
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    //edit by lvcc 全改成单选无复选框的
    //protected void btn_edit_Click(object sender, EventArgs e)
    //{
    //    int count = 0;
    //    for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
    //    {
    //        CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
    //        if (chk.Checked == true)
    //        {
    //            count += 1;
    //        }
    //    }
    //    if (count < 1)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择采购审批单！');", true);
    //    }
    //    else
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        for (int i = 0; i < myGrid.Items.Count; i++)
    //        {
    //            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
    //            if (chk.Checked == true)
    //            {
    //                sb.Append("<tr>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[1].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[2].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[3].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[4].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[5].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[6].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[8].Text);
    //                sb.Append("</td>");
    //                sb.Append("<td>");
    //                sb.Append(myGrid.Items[i].Cells[9].Text);
    //                sb.Append("</td>");
    //                sb.Append("</tr>");
    //            }
    //        }
    //        string script = "<script>window.returnValue='" + sb.ToString() + "';window.close();</script>";
    //        ClientScript.RegisterStartupScript(this.GetType(), "", script);
    //    }
    //}

    //protected void Button2_Click(object sender, EventArgs e)
    //{
    //    string billCode = "";
    //    int count = 0;
    //    string stepID_ID = "";
    //    for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
    //    {
    //        CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
    //        if (chk.Checked == true)
    //        {
    //            billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
    //            count += 1;
    //        }
    //    }

    //    if (count > 1)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个采购审批单！');", true);
    //    }
    //    else if (count == 0)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的采购审批单！');", true);
    //    }
    //    else
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('cgspDetail.aspx?type=look&cgbh=" + billCode + "');", true);
    //    }
    //}
    //protected void Button3_Click(object sender, EventArgs e)
    //{
    //    ClientScript.RegisterStartupScript(this.GetType(),"","window.returnValue=\"\";self.close();",true);
    //}

}
