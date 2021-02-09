using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Dept_deptList : System.Web.UI.Page
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
        string sql = "select deptcode,deptname,(select b.deptname from bill_departments b where b.deptcode=a.sjdeptcode) as sjdeptcode,(case deptstatus when '1' then '正常' when '0' then '禁用' end) as deptstatus from bill_departments a where 1=1 ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
            sql += " and deptCode in (" + deptCodes + ")";
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and deptCode in (" + deptCodes + ")";
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

    #region 包含下级
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    #endregion


    #region 确定
    protected void btn_select_Click(object sender, EventArgs e)
    {
        List<string> sqls = new List<string>();
        bool selectLev3 = true;
        int sel_count = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                string sj2 = server.GetCellValue("select sjdeptcode from bill_departments where deptcode='" + myGrid.Items[i].Cells[1].Text + "'");
                string sj1 = server.GetCellValue("select sjdeptcode from bill_departments where deptcode='" + sj2 + "'");
                if (sj1 != "000001")
                {
                    selectLev3 = false;
                }

                string guid2 = (new GuidHelper()).getNewGuid();
                sqls.Add("insert into bill_ybbxmxb_fykm_dept select '" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + guid2 + "',deptcode,0,0 from bill_departments where deptcode='" + myGrid.Items[i].Cells[1].Text + "' and deptcode not in (select deptcode from bill_ybbxmxb_fykm_dept where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "')");
                sel_count++;
            }
        }
        if (selectLev3 == false)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择三级部门！');", true);
            return;
        }   
        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择部门！');</script>");
        }
        else
        {
            if (server.ExecuteNonQuerysArray(sqls) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(),"","alert('保存失败！');",true);
                return;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
            }
        }
    }
    #endregion

    #region 取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
    #endregion
}