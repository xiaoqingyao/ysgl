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

public partial class webBill_cgzj_sp_cgzjbxList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("cgzjfk");
                this.BindDataGrid();
            }
        }
    }

    void BindDataGrid()
    {
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //string selfDeptCode = (new Departments()).GetZgDepartments(Session["userCode"].ToString().Trim());

        string billCodes = (new workFlowLibrary.workFlow()).getRightStepBills("cgzjfk", Session["userGroup"].ToString().Trim(), Session["userCode"].ToString().Trim(), deptCodes);

        string sql = "select * from( select '' as bxzy,stepid as stepID_ID,billDept,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='cgzjfk' and bill_workFlowStep.stepid=bill_main.stepid ) end) as stepID,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,billdate,billje from bill_main";
        sql += " where billCode in (" + billCodes + ") and flowID='cgzjfk')t";

        /*
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (billCode like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
         */
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(TextBox2.Text))
        {
            sb.Append(" and (billdept in(select deptcode from bill_departments where deptname like '%" + TextBox2.Text + "%'))");
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
        }

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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button6_Click(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待修改的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('bxDetail.aspx?type=look&billCode=" + billGuid + "');", true);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
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
            (new workFlowLibrary.workFlow()).checkBills("cgzjfk", billCode, Session["userCode"].ToString().Trim(), System.DateTime.Now.ToString(), shyj, true);
            this.BindDataGrid();
        }
    }
    protected void Button3_Click1(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个单据！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待查看的单据！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('cgzjDetail.aspx?type=look&par=" + billGuid + "');", true);
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
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
            (new workFlowLibrary.workFlow()).checkBills("cgzjfk", billCode, Session["userCode"].ToString().Trim(), System.DateTime.Now.ToString(), shyj, false);
            this.BindDataGrid();
        }
    }
    protected void Button5_Click(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待查看的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=cgzjfk&billCode=" + billCode + "');", true);
        }
    }
    protected void Button6_Click1(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待打印的报销单！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../printer/printerYbbx.aspx?billCode=" + billCode + "');", true);
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header)
        {
            e.Item.Cells[5].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[5].Text.ToString().Trim());
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('search_bxshFrame.aspx','_blank');", true);
    }
    protected void btn_summit_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}