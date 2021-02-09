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

public partial class webBill_yskm_selectCwkmList : System.Web.UI.Page
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
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        string sql = "select * from bill_cwkm where 1=1 ";
        string kmCode = Page.Request.QueryString["kmCode"].ToString().Trim();
        if (kmCode == "")
        {
        }
        else
        {
            if (this.chkNextLevel.Checked)
            {
                sql += " and left(cwkmCode,len('" + kmCode + "'))= '" + kmCode + "'";
            }
            else
            {
                sql += " and cwkmCode= '" + kmCode + "'";
            }
        }
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (cwkmcode like '%" + this.TextBox1.Text.ToString().Trim() + "%' or cwkmbm  like '%" + this.TextBox1.Text.ToString().Trim() + "%' or cwkmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        sql += " order by cwkmcode";
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

    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string kmCodes = "";
        string kmMc = "";
        int selectCount = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                kmCodes = myGrid.Items[i].Cells[1].Text;
                kmMc = myGrid.Items[i].Cells[3].Text;
                selectCount++;
            }
        }
        if (selectCount == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择财务科目！');</script>");
        }
        else if (selectCount > 1)
        {
            Page.RegisterStartupScript("", "<script>window.alert('只能选择一条科目！');</script>");
        }
        else
        {
            System.Collections.Generic.List<string> arr = new System.Collections.Generic.List<string>();
            arr.Add("delete from bill_yskm_dept where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "' and yskmCode='" + Page.Request.QueryString["yskmCode"].ToString().Trim() + "'");
            arr.Add("insert into bill_yskm_dept values('" + Page.Request.QueryString["deptCode"].ToString().Trim() + "','" + Page.Request.QueryString["yskmCode"].ToString().Trim() + "','" + kmCodes + "')");
            

            if (server.ExecuteNonQuerysArray(arr) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
            else
            {
                string rValue = "[" + kmCodes + "]" + kmMc;
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"" + rValue + "\";self.close();", true);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
}
