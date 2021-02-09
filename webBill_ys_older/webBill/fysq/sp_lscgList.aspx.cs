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
using System.Text;

public partial class webBill_fysq_sp_lscgList : System.Web.UI.Page
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
            /*
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");
             */
            if (!IsPostBack)
            {
                this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("lscg");
                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='03' order by dicCode");
                //this.ddl_sqlx.DataTextField = "dicName";
                //this.ddl_sqlx.DataValueField = "dicCode";
                //this.ddl_sqlx.DataSource = temp;
                //this.ddl_sqlx.DataBind();

                ListItem li = new ListItem();
                li.Text = "-=全部=-";
                li.Value = "00";
                /*
                this.ddl_sqlx.Items.Add(li);
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li2 = new ListItem(temp.Tables[0].Rows[i]["dicName"].ToString().Trim(), temp.Tables[0].Rows[i]["dicCode"].ToString().Trim());
                    this.ddl_sqlx.Items.Add(li2);
                }
                */
                BindDataGrid();
            }
        }
    }


    protected void BindDataGrid()
    {
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //string selfDeptCode = (new Departments()).GetZgDepartments(Session["userCode"].ToString().Trim());

        string billCodes = (new workFlowLibrary.workFlow()).getRightStepBills("lscg", Session["userGroup"].ToString().Trim(),Session["userCode"].ToString().Trim(),deptCodes);

        string sql = "select * from( select b.sm as sm,(select dicname from bill_datadic where diccode=cglb and dictype='03') as cglb,cgDept,a.billCode,(select userName from bill_users where userCode=b.cbr) as billUser,a.billje,a.billDate,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID,(select dicname from bill_dataDic where dictype='03' and diccode =b.cglb) as jkdjlx from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";

        sql += " and a.billCode in (" + billCodes + "))t ";

        #region 查询条件
        //申请开始日期
        /*
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >=cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <=cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        //申请类型
        if (ddl_sqlx.SelectedIndex != 0)
        {
            sql += " and b.cglb='" + ddl_sqlx.SelectedValue + "'";
        }
        
        */
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            sb.Append(" and (cgdept in(select deptcode from bill_departments where deptname like '%" + TextBox2.Text + "%'))");
        }
        if (!string.IsNullOrEmpty(TextBox3.Text))
        {
            sb.Append(" and (billuser like '%" + TextBox3.Text + "%')");
        }
        if (!string.IsNullOrEmpty(TextBox4.Text))
        {
            sb.Append(" and (billje =" + TextBox4.Text + ")");
        }
        if (!string.IsNullOrEmpty(TextBox5.Text))
        {
            sb.Append(" and (convert(varchar(10),billdate,121)='" + TextBox5.Text + "')");
        }
        if (sb.Length > 0)
        {
            sql += " where " + sb.ToString().Substring(4, sb.Length - 4);
            //sql += sb.ToString();
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
    protected void btn_edit_Click(object sender, EventArgs e)
    {

        string billCode = "";
        int count = 0;
        string shyj = "";
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[8].Text.ToString().Trim();
                shyj = ((TextBox)this.myGrid.Items[i].FindControl("TextBox1")).Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待审核的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能审核一项！');", true);
        }
        else
        {
            (new workFlowLibrary.workFlow()).checkBills("lscg", billCode, Session["userCode"].ToString().Trim(), System.DateTime.Now.ToString(), shyj, true);
            this.BindDataGrid();
        }
    }
    protected void btn_tj_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string stepID_ID = "";
        string shyj = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[8].Text.ToString().Trim();
                shyj = ((TextBox)this.myGrid.Items[i].FindControl("TextBox1")).Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待审核的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能审核一项！');", true);
        }
        else
        {
            (new workFlowLibrary.workFlow()).checkBills("lscg", billCode, Session["userCode"].ToString().Trim(), System.DateTime.Now.ToString(), shyj, false);
            this.BindDataGrid();
        }
    }
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查看的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能查看一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookShgc('../../workFlow/stepLook.aspx?billType=lscg&billCode=" + billCode + "');", true);
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个申请单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择查看的申请单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('lscgDetail.aspx?type=look&cgbh=" + billGuid + "');", true);
            //Response.Redirect("ystbDetail.aspx?gcbh=" + billGuid );
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
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
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();

            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待打印的报告申请单！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../printer/printerLscg.aspx?billCode=" + billCode + "');", true);
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header)
        {
            e.Item.Cells[3].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[3].Text.ToString().Trim());
        }
    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('search_lscgList.aspx','_blank');", true);
    }

    protected void btn_summit_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}
