using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_makebxd_weicaiyaopin_ckd : System.Web.UI.Page
{
    sqlHelper.sqlHelper sqlhelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            this.txtDateF.Text = DateTime.Now.ToString("yyyy-MM") + "-01";
            this.txtDateT.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
    protected void btnSelect_OnClick(object sender, EventArgs e)
    {
        string strdtF = this.txtDateF.Text.Trim();
        string strdtT = this.txtDateT.Text.Trim();
        this.GridView1.DataSource = sqlhelper.GetDataTable("select * from V_Slyy_CKD where 单据日期>='" + strdtF + "' and 单据日期 <='" + strdtT + "'", null);
        this.GridView1.DataBind();
    }
    protected void btnMakBxd_OnClick(object sender, EventArgs e)
    {
        string strdtF = this.txtDateF.Text.Trim();
        string strdtT = this.txtDateT.Text.Trim();
        string usercode = Session["usercode"].ToString();
        if (strdtF.Equals("") || strdtT.Equals(""))
        {
            Response.Write("日期条件必须都不能为空"); return;
        }
        //执行整理数据的存储过程
        string rel = sqlhelper.ExecuteScalar("exec ckd_MakeSrd '" + strdtF + "','" + strdtT + "','" + usercode + "'").ToString();
        if (rel.IndexOf("error") > -1)
        {
            string strErrorMsg = rel.Substring(6);
            Response.Write(strErrorMsg);
            return;
        }
        rel = sqlhelper.ExecuteScalar("exec pro_makebxd '" + rel + "','ckd'").ToString();
        Response.Write("单据生成成功，对应单据号为：" + rel);
    }
}
