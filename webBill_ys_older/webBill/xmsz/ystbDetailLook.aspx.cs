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

public partial class webBill_ysgl_ystbDetailLook : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
            if (!IsPostBack)
            {
                string gcbh = Page.Request.QueryString["gcbh"].ToString().Trim();
                DataSet temp = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");
                this.Label1.Text = temp.Tables[0].Rows[0]["xmmc"].ToString().Trim();

                this.bindData();
            }
        //}
    }

    void bindData()
    {
        string deptGuid = server.GetCellValue("select ysdept from bill_ysmxb where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        DataSet temp = server.GetDataSet("exec bill_pro_ysmxb_dept '" + Page.Request.QueryString["gcbh"].ToString().Trim() + "','" + deptGuid + "'");

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

        temp = server.GetDataSet("select * from bill_main where billName='" + Page.Request.QueryString["gcbh"].ToString().Trim() + "' and billDept='" + deptGuid + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            
        }
        else
        {
            this.lblBillCode.Text = temp.Tables[0].Rows[0]["billCode"].ToString().Trim();
            string stepID = temp.Tables[0].Rows[0]["stepID"].ToString().Trim();
            if (stepID == "-1")//未提交
            {
                this.Label2.Text = "预算信息未提交！";
            }
            else if (stepID == "begin")
            {
                this.Label2.Text = "预算信息已提交！";
            }
            else if (stepID == "end")
            {
                this.Label2.Text = "预算信息已审核通过！";
            }
            else
            {
                this.Label2.Text = "预算信息审核中！";
            }
        }
    }
}
