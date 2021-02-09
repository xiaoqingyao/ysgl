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

public partial class webBill_cwgl_cbzxList : System.Web.UI.Page
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
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        string sql = "select * from bill_cbzx where 1=1 ";
        if (txb_where.Text != "")
        {
            sql += " and (zxCode like '%" + txb_where.Text + "%' or zxName like '%" + txb_where.Text + "%')";
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

    #region 修改
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string strGroupID = "";
        int selectCount = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                strGroupID = myGrid.Items[i].Cells[1].Text;
                selectCount += 1;
            }
        }
        if (selectCount == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要修改的成本中心！');", true);
        }
        else if (selectCount > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('只能选择一条成本中心修改！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('cbzxDetail.aspx?type=edit&zxCode=" + strGroupID + "');", true);
        }
    }

    #endregion

    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string strGroupID = "";
        int sel_count = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                strGroupID += "'" + myGrid.Items[i].Cells[1].Text + "',";
                sel_count += 1;
            }
        }
        if (sel_count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待删除的成本中心！');", true);
        }
        else
        {
            strGroupID = strGroupID.Substring(0, strGroupID.Length - 1);
            if (server.GetCellValue("select count(1) from bill_ybbxmxb_fykm_ft where cbzx in (" + strGroupID + ")") != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('成本中心处于使用状态,禁止删除！');", true);
                return;
            }


            if (server.ExecuteNonQuery("delete from bill_cbzx where zxCode in (" + strGroupID + ")") == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！'); ", true);
            }

        }
        this.BindDataGrid();
    }

    #endregion

    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion
}