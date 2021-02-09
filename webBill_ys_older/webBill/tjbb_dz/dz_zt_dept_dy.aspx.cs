using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_dz_zt_dept_dy : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindZhangTao();
            BindDataGrid();
        }
    }
    private void bindZhangTao()
    {

        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");

        string strselectsql = @"select dsname as db_data,iYear,
                                cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                                * from [{0}].UFTSystem.dbo.EAP_Account where iYear>='2014' order by iYear desc";
        strselectsql = string.Format(strselectsql, strlinkdbname);

        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "companyname";
        this.ddlZhangTao.DataValueField = "db_data";
        this.ddlZhangTao.DataBind();
        //ddlZhangTao.Items.Insert(0, (new ListItem("-选择帐套-", "")));
        //ddlZhangTao.Items.Insert(1, (new ListItem("集团财务", "001")));
        //ddlZhangTao.Items.Insert(2, (new ListItem("济南财务", "002")));
    }
    protected void OnddlZhangTao_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    public void BindDataGrid()
    {

        string sql = " select * from bill_departments where 1=1 and deptcode!='000001' order by deptcode ";

        DataSet dt = server.GetDataSet(sql);
        this.GridView1.DataSource = dt.Tables[0];
        this.GridView1.DataBind();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlZhangTao.SelectedValue))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            return;
        }
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from dz_zt_dept where zth='" + ddlZhangTao.SelectedValue + "' ");

        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cbox = (CheckBox)row.FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                if (row.Cells[1].Text.Length > 2)
                {
                    list.Add("insert into dz_zt_dept(zth,deptcode) values('" + ddlZhangTao.SelectedValue + "','" + row.Cells[1].Text + "')");
                }
            }
        }


        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {
            string ysdeptcode = e.Row.Cells[1].Text;
            string ufcode = server.GetCellValue("select deptcode from dz_zt_dept where zth='" + ddlZhangTao.SelectedValue + "' and deptcode='" + ysdeptcode + "'");
            if (!string.IsNullOrEmpty(ufcode))
            {
                CheckBox cbox = (CheckBox)e.Row.FindControl("CheckBox1");
                cbox.Checked = true;
            }

            if (e.Row.Cells[1].Text.Length == 2)
            {
                string text = "&nbsp &nbsp " + e.Row.Cells[2].Text;
                e.Row.Cells[2].Text = text;
                e.Row.Cells[2].Font.Bold = true;
            }


        }
    }

}