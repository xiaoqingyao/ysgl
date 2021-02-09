using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

public partial class webBill_xsreport_srlr : System.Web.UI.Page
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
                this.TBnd.Text = DateTime.Now.Year.ToString();

                bindData();
            }
        }
    }

    private void bindData()
    {
        this.Label1.Text = this.TBnd.Text;
        StringBuilder sb = new StringBuilder();
        sb.Append("select * from xs_srb where left(ny,4)='" + this.TBnd.Text.ToString().Trim() + "'");
        DataSet ds = server.GetDataSet(sb.ToString());
        List<string> list = new List<string>();

        if (ds.Tables[0].Rows.Count < 1)
        {
            for (int i = 1; i <= 12; i++)
            {
                list.Add("insert into xs_srb(ny,srys,srsj,scys,scsj) values('" + this.TBnd.Text.ToString() + i.ToString().PadLeft(2,'0') + "',0,0,0,0)");
            }
            server.ExecuteNonQuerysArray(list);
            ds = server.GetDataSet(sb.ToString());
        }

        this.Lbsrys1.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["srys"]).ToString("F2");
        this.Lbsrsj1.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["srsj"]).ToString("F2");
        this.Lbclys1.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["scys"]).ToString("F2");
        this.Lbclsj1.Text = Convert.ToDecimal(ds.Tables[0].Rows[0]["scsj"]).ToString("F2");

        this.Lbsrys2.Text = Convert.ToDecimal(ds.Tables[0].Rows[1]["srys"]).ToString("F2");
        this.Lbsrsj2.Text = Convert.ToDecimal(ds.Tables[0].Rows[1]["srsj"]).ToString("F2");
        this.Lbclys2.Text = Convert.ToDecimal(ds.Tables[0].Rows[1]["scys"]).ToString("F2");
        this.Lbclsj2.Text = Convert.ToDecimal(ds.Tables[0].Rows[1]["scsj"]).ToString("F2");

        this.Lbsrys3.Text = Convert.ToDecimal(ds.Tables[0].Rows[2]["srys"]).ToString("F2");
        this.Lbsrsj3.Text = Convert.ToDecimal(ds.Tables[0].Rows[2]["srsj"]).ToString("F2");
        this.Lbclys3.Text = Convert.ToDecimal(ds.Tables[0].Rows[2]["scys"]).ToString("F2");
        this.Lbclsj3.Text = Convert.ToDecimal(ds.Tables[0].Rows[2]["scsj"]).ToString("F2");

        this.Lbsrys4.Text = Convert.ToDecimal(ds.Tables[0].Rows[3]["srys"]).ToString("F2");
        this.Lbsrsj4.Text = Convert.ToDecimal(ds.Tables[0].Rows[3]["srsj"]).ToString("F2");
        this.Lbclys4.Text = Convert.ToDecimal(ds.Tables[0].Rows[3]["scys"]).ToString("F2");
        this.Lbclsj4.Text = Convert.ToDecimal(ds.Tables[0].Rows[3]["scsj"]).ToString("F2");

        this.Lbsrys5.Text = Convert.ToDecimal(ds.Tables[0].Rows[4]["srys"]).ToString("F2");
        this.Lbsrsj5.Text = Convert.ToDecimal(ds.Tables[0].Rows[4]["srsj"]).ToString("F2");
        this.Lbclys5.Text = Convert.ToDecimal(ds.Tables[0].Rows[4]["scys"]).ToString("F2");
        this.Lbclsj5.Text = Convert.ToDecimal(ds.Tables[0].Rows[4]["scsj"]).ToString("F2");

        this.Lbsrys6.Text = Convert.ToDecimal(ds.Tables[0].Rows[5]["srys"]).ToString("F2");
        this.Lbsrsj6.Text = Convert.ToDecimal(ds.Tables[0].Rows[5]["srsj"]).ToString("F2");
        this.Lbclys6.Text = Convert.ToDecimal(ds.Tables[0].Rows[5]["scys"]).ToString("F2");
        this.Lbclsj6.Text = Convert.ToDecimal(ds.Tables[0].Rows[5]["scsj"]).ToString("F2");

        this.Lbsrys7.Text = Convert.ToDecimal(ds.Tables[0].Rows[6]["srys"]).ToString("F2");
        this.Lbsrsj7.Text = Convert.ToDecimal(ds.Tables[0].Rows[6]["srsj"]).ToString("F2");
        this.Lbclys7.Text = Convert.ToDecimal(ds.Tables[0].Rows[6]["scys"]).ToString("F2");
        this.Lbclsj7.Text = Convert.ToDecimal(ds.Tables[0].Rows[6]["scsj"]).ToString("F2");

        this.Lbsrys8.Text = Convert.ToDecimal(ds.Tables[0].Rows[7]["srys"]).ToString("F2");
        this.Lbsrsj8.Text = Convert.ToDecimal(ds.Tables[0].Rows[7]["srsj"]).ToString("F2");
        this.Lbclys8.Text = Convert.ToDecimal(ds.Tables[0].Rows[7]["scys"]).ToString("F2");
        this.Lbclsj8.Text = Convert.ToDecimal(ds.Tables[0].Rows[7]["scsj"]).ToString("F2");

        this.Lbsrys9.Text = Convert.ToDecimal(ds.Tables[0].Rows[8]["srys"]).ToString("F2");
        this.Lbsrsj9.Text = Convert.ToDecimal(ds.Tables[0].Rows[8]["srsj"]).ToString("F2");
        this.Lbclys9.Text = Convert.ToDecimal(ds.Tables[0].Rows[8]["scys"]).ToString("F2");
        this.Lbclsj9.Text = Convert.ToDecimal(ds.Tables[0].Rows[8]["scsj"]).ToString("F2");

        this.Lbsrys10.Text = Convert.ToDecimal(ds.Tables[0].Rows[9]["srys"]).ToString("F2");
        this.Lbsrsj10.Text = Convert.ToDecimal(ds.Tables[0].Rows[9]["srsj"]).ToString("F2");
        this.Lbclys10.Text = Convert.ToDecimal(ds.Tables[0].Rows[9]["scys"]).ToString("F2");
        this.Lbclsj10.Text = Convert.ToDecimal(ds.Tables[0].Rows[9]["scsj"]).ToString("F2");

        this.Lbsrys11.Text = Convert.ToDecimal(ds.Tables[0].Rows[10]["srys"]).ToString("F2");
        this.Lbsrsj11.Text = Convert.ToDecimal(ds.Tables[0].Rows[10]["srsj"]).ToString("F2");
        this.Lbclys11.Text = Convert.ToDecimal(ds.Tables[0].Rows[10]["scys"]).ToString("F2");
        this.Lbclsj11.Text = Convert.ToDecimal(ds.Tables[0].Rows[10]["scsj"]).ToString("F2");

        this.Lbsrys12.Text = Convert.ToDecimal(ds.Tables[0].Rows[11]["srys"]).ToString("F2");
        this.Lbsrsj12.Text = Convert.ToDecimal(ds.Tables[0].Rows[11]["srsj"]).ToString("F2");
        this.Lbclys12.Text = Convert.ToDecimal(ds.Tables[0].Rows[11]["scys"]).ToString("F2");
        this.Lbclsj12.Text = Convert.ToDecimal(ds.Tables[0].Rows[11]["scsj"]).ToString("F2");

    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string strsrys = "0";
        string strsrsj = "0";
        string strclys = "0";
        string strclsj = "0";
        List<string> list = new List<string>();

        strsrys = this.Lbsrys1.Text.Trim();
        strsrsj = this.Lbsrsj1.Text.Trim();
        strclys = this.Lbclys1.Text.Trim();
        strclsj = this.Lbclsj1.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "01" + "'");

        strsrys = this.Lbsrys2.Text.Trim();
        strsrsj = this.Lbsrsj2.Text.Trim();
        strclys = this.Lbclys2.Text.Trim();
        strclsj = this.Lbclsj2.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "02" + "'");


        strsrys = this.Lbsrys3.Text.Trim();
        strsrsj = this.Lbsrsj3.Text.Trim();
        strclys = this.Lbclys3.Text.Trim();
        strclsj = this.Lbclsj3.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "03" + "'");

        strsrys = this.Lbsrys4.Text.Trim();
        strsrsj = this.Lbsrsj4.Text.Trim();
        strclys = this.Lbclys4.Text.Trim();
        strclsj = this.Lbclsj4.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "04" + "'");


        strsrys = this.Lbsrys5.Text.Trim();
        strsrsj = this.Lbsrsj5.Text.Trim();
        strclys = this.Lbclys5.Text.Trim();
        strclsj = this.Lbclsj5.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "05" + "'");


        strsrys = this.Lbsrys6.Text.Trim();
        strsrsj = this.Lbsrsj6.Text.Trim();
        strclys = this.Lbclys6.Text.Trim();
        strclsj = this.Lbclsj6.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "06" + "'");

        strsrys = this.Lbsrys7.Text.Trim();
        strsrsj = this.Lbsrsj7.Text.Trim();
        strclys = this.Lbclys7.Text.Trim();
        strclsj = this.Lbclsj7.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "07" + "'");
        
        strsrys = this.Lbsrys8.Text.Trim();
        strsrsj = this.Lbsrsj8.Text.Trim();
        strclys = this.Lbclys8.Text.Trim();
        strclsj = this.Lbclsj8.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "08" + "'");

        strsrys = this.Lbsrys9.Text.Trim();
        strsrsj = this.Lbsrsj9.Text.Trim();
        strclys = this.Lbclys9.Text.Trim();
        strclsj = this.Lbclsj9.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "09" + "'");

        strsrys = this.Lbsrys10.Text.Trim();
        strsrsj = this.Lbsrsj10.Text.Trim();
        strclys = this.Lbclys10.Text.Trim();
        strclsj = this.Lbclsj10.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "10" + "'");

        strsrys = this.Lbsrys11.Text.Trim();
        strsrsj = this.Lbsrsj11.Text.Trim();
        strclys = this.Lbclys11.Text.Trim();
        strclsj = this.Lbclsj11.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "11" + "'");

        strsrys = this.Lbsrys12.Text.Trim();
        strsrsj = this.Lbsrsj12.Text.Trim();
        strclys = this.Lbclys12.Text.Trim();
        strclsj = this.Lbclsj12.Text.Trim();
        list.Add("update xs_srb set srys=" + strsrys + ",srsj=" + strsrsj + ",scys=" + strclys + ",scsj=" + strclsj + " where ny='" + this.TBnd.Text.Trim() + "12" + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        
    }
    
}
