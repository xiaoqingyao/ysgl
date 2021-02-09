using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public partial class fysq_sqList : System.Web.UI.Page
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
                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='01' order by dicCode");
                //this.ddl_sqlx.DataTextField = "dicName";
                //this.ddl_sqlx.DataValueField = "dicCode";
                //this.ddl_sqlx.DataSource = temp;
                //this.ddl_sqlx.DataBind();

                ListItem li = new ListItem();
                li.Text = "-=全部=-";
                li.Value = "00";
                this.ddl_sqlx.Items.Add(li);
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li2 = new ListItem(temp.Tables[0].Rows[i]["dicName"].ToString().Trim(), temp.Tables[0].Rows[i]["dicCode"].ToString().Trim());
                    this.ddl_sqlx.Items.Add(li2);
                }

                BindDataGrid();
            }
        }
    }


    protected void BindDataGrid()
    {
        string sql = "select a.billCode,(select userName from bill_users where userCode=a.billUser) as billUser,a.billje,(select userName from bill_users where userCode=b.jbr) as  jbr,b.hjje,a.billDate,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='fysq' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='01' and diccode =b.jkdjlx) as jkdjlx,sqzy,sqbz from bill_main a,bill_fysq b where a.flowid='fysq' and a.billCode=b.billCode ";

        sql += " and (a.billUser='" + Session["userCode"].ToString().Trim() + "' or b.jbr='" + Session["userCode"].ToString().Trim() + "') ";

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
        //申请类型
        if (ddl_sqlx.SelectedIndex!=0)
        {
            sql += " and b.jkdjlx='" + ddl_sqlx.SelectedValue + "'";
        }
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

    protected void btn_del_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        bool isBegin = false;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billCode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                string stepID_ID = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
                if (stepID_ID == "-1" || stepID_ID == "0")
                { }
                else
                {
                    isBegin = true;
                }
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要删除的单据！');", true);
        }
        else
        {
            if (isBegin == false)
            {
                billCode = billCode.Substring(0, billCode.Length - 1);
                System.Collections.Generic.List<string> list = new List<string>();

                list.Add("delete from bill_main where billcode in (" + billCode + ") and flowid='fysq'");
                list.Add("delete from bill_fysq where billcode in (" + billCode + ")");
                list.Add("delete from bill_fysq_mxb where billcode in (" + billCode + ")");
                list.Add("delete from bill_fysq_fjb where billcode in (" + billCode + ")");
                list.Add("delete from bill_workflowRecord where billCode in (" + billCode + ") and flowid='fysq'");

                if (server.ExecuteNonQuerysArray(list) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                    this.BindDataGrid();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的费用申请单已进入审核流程,不能删除！');", true);
            }
        }
    }

    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个费用申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的费用申请单！');", true);
        }
        else
        {
            if (stepID_ID == "-1" || stepID_ID == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sqDetail.aspx?type=edit&billCode=" + billCode + "');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的费用申请单已进入审核流程,不能修改！');", true);
            }
        }
    }

    protected void btn_tj_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        bool isBegin = false;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billCode += "'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "',";
                string stepID_ID = this.myGrid.Items[i].Cells[10].Text.ToString().Trim();
                if (stepID_ID == "-1" || stepID_ID == "0")
                { }
                else
                {
                    isBegin = true;
                }
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待提交的费用申请单！');", true);
        }
        else
        {
            if (isBegin == false)
            {
                billCode = billCode.Substring(0, billCode.Length - 1);
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                list.Add("update bill_main set stepID='begin',loopTimes=loopTimes+1 where billCode in (" + billCode + ") and flowid='fysq'");

                if (server.ExecuteNonQuerysArray(list) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交成功！');", true);
                    this.BindDataGrid();
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择的费用申请单已进入审核流程,不能再次提交！');", true);
            }
        }
    }

    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个费用申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的费用申请单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sqDetail.aspx?type=look&billCode=" + billCode + "');", true);
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个费用申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的费用申请单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openSplc('../../workFlow/stepLook.aspx?billType=fysq&billCode=" + billCode + "');", true);
        }
    }
}
