using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class webBill_Dept_selectTianJianDept : System.Web.UI.Page
{
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
                initData();
            }
        }
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        initData();
    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string deptcode = string.Empty;
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                deptcode += myGrid.Items[i].Cells[1].Text.Trim() + ",";
            }
        }
        if (deptcode.Length > 1)
        {
            deptcode = deptcode.Substring(0, deptcode.Length - 1);
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + deptcode + "\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请勾选对应的部门');", true);
        }

    }

    /// <summary>
    /// 初始化数据表
    /// </summary>
    private void initData()
    {
        DataSet ds = new ShouRuHelper().getData("101", "");
        int dtcount = ds.Tables.Count;
        DataTable dt = ds.Tables[dtcount - 1];
        DataTable dtend = dt.Clone();
        string strdept = this.txtDept.Text.Trim();//查询条件 部门名称
        if (!string.IsNullOrEmpty(strdept))
        {
            DataRow[] drs = dt.Select(" DeptName like '%" + strdept + "%'");
            foreach (var item in drs)
            {
                dtend.ImportRow(item);
            }
            this.myGrid.DataSource = dtend;
            this.myGrid.DataBind();
        }
        else
        {
            if (dt != null)
            {
                this.myGrid.DataSource = dt;
                this.myGrid.DataBind();
            }
        }
    }
}
