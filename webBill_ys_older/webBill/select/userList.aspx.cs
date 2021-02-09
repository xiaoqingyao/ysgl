using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class user_userList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strFlg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objFlg = Request["Flg"];
            if (objFlg!=null)
            {
                strFlg = objFlg.ToString();
            }
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }

    #region 绑定结果集
    
    public void BindDataGrid()
    {
        string sql = "select usercode,username,(select groupname from bill_usergroup where groupid=usergroup) as usergroup ,case userstatus when 1 then '正常' else '禁用' end as userstatus,";
        sql += " (select deptname from bill_departments where deptcode =userdept ) as userdept,userpwd  from bill_users where 1=1 ";
        if (Page.Request.QueryString["deptCode"].ToString().Trim() == "")
        {
            if (!strFlg.Equals("All"))
            {
                string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
                sql += " and userDept in (" + deptCodes + ")";
            }
        }
        else
        {
            string deptCodes = (new Departments()).GetNextLevelDepartments(Page.Request.QueryString["deptCode"].ToString().Trim(), "", this.chkNextLevel.Checked);
            sql += " and userDept in (" + deptCodes + ")";
        }
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
    #endregion

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

    #region 确定    
    protected void btn_select_Click(object sender, EventArgs e)
    {
        string userCode = "";
        int selectCount = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                userCode += "[" + myGrid.Items[i].Cells[1].Text + "]" + myGrid.Items[i].Cells[2].Text;
                userCode+="|&|";
                selectCount += 1;
            }
        }
        if (selectCount == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择人员！');</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + userCode.Substring(0,userCode.Length-3) + "\";self.close();", true);
        }
    }
    #endregion

    #region 取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Page.RegisterStartupScript("", "<script>window.close();</script>");
    }
    #endregion

    #region 选择
    private void getSelect()
    {
        string returnStr = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count-1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if(chk.Checked)
            {
                returnStr = "[" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "]" + this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                count += 1;
            }
        }
        if (count > 1)
        {
            Page.RegisterStartupScript("", "<script language=javascript>alert('只能选择一条数据!');</script>"); 
        }
        else if (count == 0)
        {
            Page.RegisterStartupScript("", "<script language=javascript>alert('必须选择一条数据!');</script>"); 
        }
        else
        {
            Page.RegisterStartupScript("", "<script language=javascript>window.returnValue=" + returnStr + ";window.close();</script>");
        }
        //return returnStr;
    }
    #endregion

    protected void myGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string str = "[" + e.Row.Cells[1].Text + "]" + e.Row.Cells[2].Text;
            e.Row.Attributes.Add("ondblclick", "javascript:selected('" + str + "');");
            e.Row.Attributes.Add("onmousedown", "javascript:setsel('" + str + "');");
        }
    }
}
