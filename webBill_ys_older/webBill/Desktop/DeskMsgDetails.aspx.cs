using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_Desktop_DeskMsgDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    protected void BindData()
    {
        string strsql = "select * from bill_DeskMsg where  musercode='" + Session["userCode"].ToString().Trim() + "'";
        DataSet ds = server.GetDataSet(strsql);
        if(ds.Tables[0].Rows.Count==1)
        {
            txtMes.Text = ds.Tables[0].Rows[0]["mmesg"].ToString();
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string strsql = "update bill_DeskMsg set mmesg='" + txtMes.Text + "' where musercode='" + Session["userCode"].ToString().Trim() + "'";
        if (server.ExecuteNonQuery(strsql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    { 
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }
}
