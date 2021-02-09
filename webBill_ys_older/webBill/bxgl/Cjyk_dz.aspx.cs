using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_Cjyk_dz : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                return;
            }
            hidusercode.Value = Session["userCode"].ToString().Trim();
            binddata();
        }

    }
    private void binddata()
    {
        this.myGrid.DataSource = getdt();
        this.myGrid.DataBind();
    }

    public DataTable getdt()
    {
        string sql = @" exec [dz_cjyk]";
      
        return server.GetDataTable(sql, null);
    }
}