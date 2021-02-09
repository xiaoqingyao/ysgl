using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_select_selectcwkmframe : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            (new cwkm()).BindCwkm(this.TreeView1.Nodes[0], "", "", "../Resources/Images/treeView/treeNode.gif");
            this.BindDataGrid();
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetCwkmAll());
    }
    public void BindDataGrid()
    {
        string sql = "select *,(case ShiFouFengCun when '1' then '是' when '0' then '否' end)as sffc from bill_cwkm where 1=1 ";
        if (this.TreeView1.SelectedNode == null)
        {
            return;
        }
        string kmCode = this.TreeView1.SelectedNode.Value;
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

    #region 包含下级
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    #endregion


    #region 确定
    protected void btn_select_Click(object sender, EventArgs e)
    {

        string str_deptcode = "";
        int sel_count = 0;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                str_deptcode = "[" + myGrid.Items[i].Cells[1].Text + "]" + myGrid.Items[i].Cells[3].Text;
                sel_count++;
            }
        }
        if (sel_count == 0)
        {
            string strxz = txtcwkm.Text.Trim();
            if (!string.IsNullOrEmpty(strxz))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + strxz + "\";self.close();", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "a", "<script>window.alert('请选择科目！');</script>", true);
            }

        }
        else if (sel_count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "a", "<script>window.alert('只能选择一个科目！');</script>", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + str_deptcode + "\";self.close();", true);
        }
    }
    #endregion

    #region 取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }
    #endregion

    private string GetCwkmAll()
    {
        string sql = "select '['+cwkmcode+']'+cwkmMc as cwkmName from bill_cwkm ";
        if (this.TreeView1.SelectedNode != null)
        {
            sql += " where cwkmcode like '%" + this.TreeView1.SelectedNode.Value + "%'";
        }
        DataSet ds = server.GetDataSet(sql);
        System.Text.StringBuilder arry = new System.Text.StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["cwkmName"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }

    }
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}