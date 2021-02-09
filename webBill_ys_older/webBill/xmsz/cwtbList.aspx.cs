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

public partial class webBill_ysgl_cwtbList : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("xmys");
                this.BindDataGrid();
            }
        }
    }
    void BindDataGrid()
    {
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //此处有bug:在从bill_ysmxb查询数据时，没有增加当前单位的权限参数
        string sql = "select (select '['+xmcode+']'+xmname from bill_xm where xmcode=bill_main.billname2) as ysxm,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select deptname from bill_departments where deptcode=billdept) as billDept,(select username from bill_users where usercode=billuser) as billUser,billDate,stepid as stepID_ID,(case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='xmys' and bill_workFlowStep.stepid=bill_main.stepid ) end) as stepID from bill_main where billCode in (select billCode from bill_ysmxb where yskm in (select yskmcode from bill_yskm where tblx='02'))";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            this.Button1.Enabled = false;
            sql += " and billDept in (" + deptCodes + ") ";
        }
        else {
            sql += " and billDept='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'";
        }
        sql += " and billType='x'";
        DataSet temp = server.GetDataSet(sql);


        #region 计算分页相关数据
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
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("cwtbSelectYsgc.aspx?deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
    }
    protected void Button2_Click(object sender, EventArgs e)
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

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            if (stepID_ID != "-1" && stepID_ID != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('所选预算信息已进入审核流程,禁止修改！');", true);
            }
            else {
                Response.Redirect("cwtbEdit.aspx?billCode=" + billCode+"&deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
            }
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
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

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            if (stepID_ID != "-1" && stepID_ID != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('所选预算信息已进入审核流程,禁止删除！');", true);
            }
            else
            {
                string sql = "delete from bill_ysmxb where billCode='"+billCode+"' and yskm in (select yskmcode from bill_yskm where tblx='02')";
                if (server.ExecuteNonQuery(sql) == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
                }
                else {
                    server.ExecuteNonQuery("update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billcode='" + billCode + "') where billCode='" + billCode + "'");
                    this.BindDataGrid();
                }
            }
        }
    }
    protected void Button8_Click(object sender, EventArgs e)
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

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            Response.Redirect("cwtbDetail.aspx?from=cwtbList&billCode=" + billCode + "&deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
        }
    }
    protected void Button4_Click(object sender, EventArgs e)
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

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            if (stepID_ID != "-1" && stepID_ID != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算已进入审核流程,无须再次提交！');", true);
            }
            else
            {
                string cwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billCode + "' and yskm in (select yskmcode from bill_yskm where tblx='02')");
                string dwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billCode + "' and yskm in (select yskmcode from bill_yskm where tblx='01')");
                if (cwtb == "0")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少财务填报部分,不能提交！');", true);
                    return;
                } if (dwtb == "0")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少部门填报部分,不能提交！');", true);
                    return;
                }

                if (server.ExecuteNonQuery("update bill_main set stepID='begin',loopTimes=loopTimes+1 where flowID='xmys' and billCode='" + billCode + "'") == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交成功！');", true);
                    this.BindDataGrid();
                }
            }
        }
    }
    protected void Button9_Click(object sender, EventArgs e)
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

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            if (stepID_ID == "-1" || stepID_ID == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算未提交！');", true);
                return;
            }
            if (stepID_ID != "begin")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算已进入审核流程,不能撤销提交！');", true);
            }
            else {
                if (server.ExecuteNonQuery("update bill_main set stepID='-1',loopTimes=loopTimes+1 where flowID='xmys' and stepID='begin' and billCode='" +billCode + "'") == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('撤销提交失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('撤销提交成功！');", true);
                    this.BindDataGrid();
                }
            }
        }
    }
    protected void Button7_Click(object sender, EventArgs e)
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
            ClientScript.RegisterStartupScript(this.GetType(), "", "openLookSpStep('../../workflow/steplook.aspx?billType=xmys&billCode=" + billCode + "');", true);
        }
    }
    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
}
