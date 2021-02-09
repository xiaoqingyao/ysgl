using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Bll.UserProperty;

public partial class webBill_user_UserToExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SysManager sysMgr = new SysManager();
            GridView1.DataSource = sysMgr.GetAllUser();
            GridView1.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.Charset = "utf-8";
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        GridView1.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            SysManager sysMgr = new SysManager();
            string sjdept = e.Row.Cells[2].Text;
            if (sjdept != "&nbsp;")
            {
                e.Row.Cells[2].Text = sysMgr.GetDeptCodeName(sjdept);
            }
            string group = e.Row.Cells[3].Text;
            if (group != "&nbsp;")
            {
                e.Row.Cells[3].Text = sysMgr.GetUserGroup(group);
            }
        }
    }
}

