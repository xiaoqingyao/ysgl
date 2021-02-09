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

public partial class webBill_fysq_cgspDetailDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),"","self.close();",true);
    }
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        double sl = 0;
        double dj = 0;
        double zj = 0;
        try
        {
            sl = double.Parse(this.TextBox3.Text.ToString().Trim());
            dj = double.Parse(this.TextBox4.Text.ToString().Trim());
            zj = sl * dj;
        }
        catch {
            ClientScript.RegisterStartupScript(this.GetType(),"","alert('数量或单价输入错误！');",true);
            return;
        }

        string sql = "insert into bill_cgsp_mxb(cgbh,mc,gg,sl,dj,zj,bz,cgbhGuid) values('','" + this.TextBox1.Text.ToString().Trim() + "','" + this.TextBox2.Text.ToString().Trim() + "'," + sl.ToString() + "," + dj.ToString() + "," + zj.ToString() + ",'" + this.TextBox5.Text.ToString().Trim() + "','" + Page.Request.QueryString["billCode"].ToString().Trim() + "')";
        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
        }
    }
}
