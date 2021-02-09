using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;


public partial class webBill_xsreport_zlcbb : System.Web.UI.Page
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
                this.TextBox1.Text = System.DateTime.Now.Year.ToString() + "-01-01";

                //this.TextBox2.Text = System.DateTime.Now.ToShortDateString();
                this.TextBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.LbRq.Text = System.DateTime.Now.ToShortDateString();
                bindData();
            }
        }
    }
    void bindData()
    {
        string begDate = this.TextBox1.Text.Trim();
        string endDate = this.TextBox2.Text.Trim();
        SqlParameter[] sps = { 
                                 new SqlParameter("@kssj",begDate),
                                 new SqlParameter("@jzsj",endDate)
                             };
        DataSet ds = server.ExecuteProcedure("bill_pro_report_zlcbb_xs", sps);
        this.Lbwbspdy.Text = Convert.ToString(ds.Tables[0].Rows[0][4]);
        this.LbYfgzdy.Text = Convert.ToString(ds.Tables[0].Rows[1][4]);
        this.Lbjdjydy.Text = Convert.ToString(ds.Tables[0].Rows[2][4]);
        this.Lbjdgzdy.Text = Convert.ToString(ds.Tables[0].Rows[3][4]);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        bindData();
    }
}