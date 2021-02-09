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

public partial class webBill_ysgl_ystbSelectYsgc : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
            this.Label1.Text = server.GetCellValue("select deptname from bill_departments where deptcode='" + (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim()) + "'") + "未填报预算过程";
            this.btn_selectXm.Attributes.Add("onclick", "javascript:selectdept('../select/xmFrame.aspx?deptCode=" + deptCode + "','txtSjxm');return false;");
            
            this.BindDataGrid();
        }
    }
    public void BindDataGrid()
    {
        string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        string sql = "select (case ysType when '0' then '年预算' when '1' then '季度预算' when '2' then '月预算' end) as ysType,nian,(case ysType when '0' then '' when '1' then '第'+yue+'季度' when '2' then yue+'月' end) as yue,gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status from bill_ysgc where ystype='0' ";
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        //找出未进行单位填报的预算过程
        sql += " and status='1' and kssj<='" + System.DateTime.Now.ToShortDateString() + "' and jzsj>='" + System.DateTime.Now.ToShortDateString() + "'";
        //sql += " and gcbh not in (select billName from bill_main where flowID='ys' and billDept='" + deptCode + "' and billName in (select gcbh from bill_ysmxb where ysdept='"+deptCode+"' and yskm in (select yskmcode from bill_yskm where tblx='01')))";

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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        string xm = this.txtSjxm.Value.ToString().Trim();
        if (xm == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待填报的项目！');", true);
            return;
        }
        else
        {
            xm = xm.Substring(1, xm.IndexOf("]") - 1);
        }
        

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
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待填报的预算过程！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待填报的预算过程！');", true);
        }
        else
        {
            if (server.GetDataSet("select * from bill_main where flowid='xmys' and billName='" + billGuid + "' and billdept='" + deptCode + "' and billname2='" + xm + "' and billcode in (select billcode from bill_ysmxb where ystype='x' and yskm in (select yskmcode from bill_yskm where tblx='01'))").Tables[0].Rows.Count == 0)
            {
                Response.Redirect("ystbAdd.aspx?gcbh=" + billGuid + "&xmCode=" + xm);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('选的单位的项目预算已存在选择预算过程的数据！');", true);
                return;
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystbFrame.aspx");
    }
}
