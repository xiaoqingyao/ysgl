using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_search_ZxqkZj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }

        if (!IsPostBack)
        {
            this.bindData();
        }
    }

    public void bindData()
    {
        string kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string jzsj = DateTime.Parse(Page.Request.QueryString["jzsj"].ToString().Trim()).AddDays(1).ToShortDateString();

        string strnd = kssj.Substring(0, 4);
        string strksyf = kssj.Substring(0, kssj.LastIndexOf('/'));
        strksyf = strksyf.Substring(strksyf.IndexOf('/') + 1);
        string strjzyf = jzsj.Substring(0, jzsj.LastIndexOf('/'));
        strjzyf = strjzyf.Substring(strjzyf.IndexOf('/') + 1);
        //    Response.Write(strjzyf);




        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string fykm = Page.Request.QueryString["fykm"].ToString().Trim();
        string strsql = @" select Row_Number()over(order by main.billname desc) as crow 
			,main.stepid,main.billCode,main.billName as billNameCode, main.billname2,
			(select '['+deptcode+']'+deptname from bill_departments where deptcode=main.billdept) as dept
			,(select xmmc from bill_ysgc where gcbh=main.billName) as billName
			,(select '['+usercode+']'+username from bill_users
			 where usercode=main.billuser) as billUser,convert(char(10),main.billdate,121) as billdate,main.billje from bill_main main where main.flowID='yszj' ";

        if (!string.IsNullOrEmpty(deptCode))
        {
            string[] a = deptCode.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string strdept = "";
            if (a.Count() > 0)
            {
                for (int i = 0; i < a.Count(); i++)
                {
                    strdept += "'";
                    strdept += a[i];
                    strdept += "',";

                }
            }
            strdept = strdept.Substring(0, strdept.Length - 1);
            strsql += " and main.billdept in(" + strdept + ")";
        }
        if (!string.IsNullOrEmpty(kssj) && (!string.IsNullOrEmpty(jzsj)))
        {
            strsql += @"  and main.billName in (select gcbh from bill_ysgc where yue>=" + strksyf + " and yue <=" + strjzyf + " and nian='" + strnd + "')";
        }
        //if (!string.IsNullOrEmpty(jzsj))
        //{
        //    strsql += " and convert(char(10),main.billdate,121)<='" + jzsj + "'";
        //}
        //Response.Write(strsql);
        DataSet temp = server.GetDataSet(strsql); //server.GetDataSet("exec pro_bill_bxtj_yskm_detail2 '" + kssj + "','" + jzsj + "','" + deptCode + "','" + fykm + "'");
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

    }
    /// <summary>
    /// 导出 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}