using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysglnew_gkdept_zt_list : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Gridzt.Visible = true;
        string strdept = Request["deptcode"];
        string strnd = Request["nian"];
        string strsql = "  exec pro_gkzt'" + strdept + "','" + strnd + "' ";

        Response.Write(strsql);
        DataTable dt = server.GetDataTable(strsql, null);
        if (dt == null || dt.Rows.Count == 0)
        {
            string strdeptsql = @"select deptName from dbo.bill_departments where deptCode='" + strdept + "'";
            string strdeptname = server.GetCellValue(strdeptsql);
            lbl_masge.Text = strdeptname + "部门没有区域分校。";
        }
        else
        {
            Gridzt.DataSource = dt;
            Gridzt.DataBind();
        }
    }
    protected void Gridzt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string zt = e.Row.Cells[2].Text.Trim();
            if (zt.Equals("已生效"))
            {
                e.Row.Cells[2].Text = "<span style='color:green;font-weight:bold '>" + zt + "</span>";
            }
        }
    }
}