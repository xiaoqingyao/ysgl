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
using System.Collections.Generic;
using Ajax;

public partial class webBill_ysgl_cwtbDetail : System.Web.UI.Page
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
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_ysgl_cwtbDetail));
            if (!IsPostBack)
            {
                if (Page.Request.QueryString["from"].ToString().Trim() == "lookDialog")
                {
                    this.Button2.Text = "关 闭";
                }

                this.lblBillCode.Text = Page.Request.QueryString["billCode"].ToString().Trim();
                DataSet temp = server.GetDataSet("select * from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
                this.Label1.Text = "预算过程：" + server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "'") + " 填报单位：" + server.GetCellValue("select deptname from bill_departments where deptcode='" + temp.Tables[0].Rows[0]["billDept"].ToString().Trim() + "'");

                this.bindData();
            }
        }
    }

    void bindData()
    {
        DataSet temp = server.GetDataSet("select * from bill_main where billCode='" + Page.Request.QueryString["billCode"].ToString().Trim() + "'");
        //去年对应过程编号
        DataSet gcInfo = server.GetDataSet("select * from bill_ysgc where gcbh='" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "'");
        string qnGcbh = server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and yue='" + gcInfo.Tables[0].Rows[0]["yue"].ToString() + "' and ystype='" + gcInfo.Tables[0].Rows[0]["ystype"].ToString() + "'");
        DataSet temp1 = server.GetDataSet("exec bill_pro_cwtb_yskm_look '" + temp.Tables[0].Rows[0]["billDept"].ToString().Trim() + "','" + temp.Tables[0].Rows[0]["billName"].ToString().Trim() + "','" + qnGcbh + "'");

        (new ysHistory()).bindHistory(this.myGrid, temp1, temp.Tables[0].Rows[0]["billDept"].ToString().Trim(), temp.Tables[0].Rows[0]["billName"].ToString().Trim()); 
        
        this.myGrid.DataSource = temp1;
        this.myGrid.DataBind();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

        if (Page.Request.QueryString["from"].ToString().Trim() == "lookDialog")
        {
            ClientScript.RegisterStartupScript(this.GetType(),"","self.close();",true);
        }
        else
        {
            Response.Redirect(Page.Request.QueryString["from"].ToString().Trim() + ".aspx?deptCode=" + Page.Request.QueryString["deptCode"].ToString().Trim());
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            DataSet temp = server.GetDataSet("select * from bill_ysmxb_smfj where billCode='" + this.lblBillCode.Text.ToString().Trim() + "' and yskm='" + e.Item.Cells[1].Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count == 0)
            { }
            else
            {
               e.Item.Cells[7].Text= temp.Tables[0].Rows[0]["sm"].ToString().Trim();

                if (temp.Tables[0].Rows[0]["fj"].ToString().Trim() == "")
                {
                }
                else
                {
                    string tempStr = "&nbsp;<a href=\"files/" + temp.Tables[0].Rows[0]["fj"].ToString().Trim() + "\" target=_blank>下 载</a>&nbsp;";

                    e.Item.Cells[8].Text = tempStr;
                }
            }
        }
    }

}
