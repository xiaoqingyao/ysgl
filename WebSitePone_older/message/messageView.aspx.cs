using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class message_messageView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["id"]))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Index.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindModel();
        }
    }

    private void BindModel()
    {
        server.ExecuteNonQuery("update bill_msg set readTimes=readtimes+1 where id=" + Request["id"]);
        DataTable dt = server.GetDataTable("select * from bill_msg where id=" + Request["id"], null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lb_tilte.Text = ObjectToStr(dr["title"]);
            lb_count.Text = ObjectToStr(dr["readTimes"]);
            lb_writer.Text = ObjectToStr(dr["writer"]);
            lb_type.Text=ObjectToStr(dr["mstype"]);
            lb_addTime.Text = Convert.ToDateTime(dr["date"]).ToString("yyyy-MM-dd");
            lb_endTime.Text = Convert.ToDateTime(dr["endtime"]).ToString("yyyy-MM-dd");
            lb_content.Text = ObjectToStr(dr["contents"]);
        }
    }

    private string ObjectToStr(object obj)
    {
        if (obj==null||Convert.ToString(obj)==string.Empty)
        {
            return "";
        }
        else
        {
            return obj.ToString();
        }
    }
}
