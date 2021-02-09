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

public partial class BillBgsq_bgsqView : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["billCode"]))
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
        string code = Convert.ToString(Request["billCode"]);
        DataTable dt = server.GetDataTable("select * from bill_lscg where cgbh='"+code+"'", null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lb_cgbh.Text = ObjectToStr(dr["cgbh"]);
            lb_sj.Text = Convert.ToDateTime(ObjectToStr(dr["sj"])).ToString("yyyy-MM-dd");
            lb_cbr.Text = server.GetCellValue("select  '['+usercode+']'+userName from bill_users  where usercode='" + ObjectToStr(dr["cbr"]) + "'");
            lb_yjfy.Text = Convert.ToDecimal(ObjectToStr(dr["yjfy"])).ToString("N02"); 
            lb_dept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + ObjectToStr(dr["cgDept"]) + "'");
            lb_cglb.Text = ObjectToStr(dr["cglb"]); 
            lb_zynr.Text = ObjectToStr(dr["zynr"]);
            lb_sm.Text = ObjectToStr(dr["sm"]);
        }

        string type = Request["type"];
        if (!string.IsNullOrEmpty(type))
        {
            if (type == "View")
            {
                aduittr.Visible = false;
                btn_audit.Visible = false;
                btn_cancel.Visible = false;
                //判断是否已提交
                DataTable dt1 = server.GetDataTable("select * from workflowrecord where billCode='" + code + "'", null);
                if (dt1.Rows.Count > 0)
                {
                    btn_submit.Visible = false;
                    btn_delete.Visible = false;
                    if (dt1.Rows[0]["rdState"].ToString() == "3")
                    {
                        btn_revoke.Visible = true;
                    }
                }


            }
            if (type == "audit")
            {
                btn_submit.Visible = false;
                btn_delete.Visible = false;
                aduittr.Visible = true;
            }

        }
    }

    private string ObjectToStr(object obj)
    {
        if (obj == null || Convert.ToString(obj) == string.Empty)
        {
            return "";
        }
        else
        {
            return obj.ToString();
        }
    }
}
