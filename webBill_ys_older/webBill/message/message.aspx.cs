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

public partial class webBill_message_message : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Page.Request.QueryString["id"].ToString().Trim();

            server.ExecuteNonQuery("update bill_msg set readTimes=readTimes+1 where id=" + id);

            DataSet temp = server.GetDataSet("select title,contents,(select username from bill_users where usercode = writer)as writer,Accessories,CONVERT(varchar(100),date, 23) as date  ,readtimes from bill_msg where id=" + id.ToString());

            this.Label1.Text = temp.Tables[0].Rows[0]["title"].ToString().Trim();

            this.Label2.Text = temp.Tables[0].Rows[0]["writer"].ToString().Trim();

            try
            {
                this.Label3.Text = DateTime.Parse(temp.Tables[0].Rows[0]["date"].ToString().Trim()).ToShortDateString();
            }
            catch { }

            this.Label4.Text = temp.Tables[0].Rows[0]["readTimes"].ToString().Trim();

            this.FCKeditor1.Value = temp.Tables[0].Rows[0]["contents"].ToString().Trim();
            if (temp.Tables[0].Rows[0]["Accessories"].ToString() != null && temp.Tables[0].Rows[0]["Accessories"].ToString() != "")
            {
                //this.TextBox3.Text = remitemode[0].Accessories.ToString();
                string strfilename = this.HiddenField2.Value.ToString();
                string strAppTemp = string.Format("<a href=\"../../" + temp.Tables[0].Rows[0]["Accessories"].ToString() + " \" target='_blank'>点击查看附件</a>");
                this.lbfj.Text = strAppTemp;

            }
            else
            {
                this.lbfj.Text = "无附件";
            }
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(),"","self.close();",true);
    }
}
