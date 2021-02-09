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
using System.IO;

public partial class webBill_tjbb_Ysqk_fykm : System.Web.UI.Page
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
        if (Page.Request.QueryString["kssj"] == null || Page.Request.QueryString["kssj"].ToString().Trim().Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开始时间不能为空！');", true);
            return;
        }
        string kssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string jzsj = Page.Request.QueryString["jzsj"].ToString().Trim();
      
        if (kssj.Substring(0,4)!=jzsj.Substring(0,4))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('开始时间与截止时间必须在同一年内！');", true);
            return;
        }
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string fykm = Page.Request.QueryString["fykm"].ToString().Trim();

        DataSet temp = server.GetDataSet("exec pro_bill_ystj_yskm '" + kssj + "','" + jzsj + "','" + deptCode + "','" + fykm + "'");
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e) {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strDate = e.Item.Cells[7].Text.Trim();
            if (strDate.Equals("1900年1月1日"))
            {
                e.Item.Cells[7].Text = "";
            }
            string strystype = e.Item.Cells[0].Text.Trim();//预算类型
            string strbillcode = e.Item.Cells[1].Text.Trim();//单据编号
            string strdeptcode = e.Item.Cells[2].Text.Trim();//单位编号
            string strysgc = e.Item.Cells[8].Text.Trim();//预算过程
            strystype = strystype.Replace("&nbsp;", "");
            strbillcode = strbillcode.Replace("&nbsp;", "");
            strdeptcode = strdeptcode.Replace("&nbsp;", "");
            strysgc = strysgc.Replace("&nbsp;", "");
            string strurl = this.GetUrl(strbillcode, strysgc, strdeptcode, strystype);
            if (!strurl.Equals(""))
            {
                e.Item.Cells[8].Text = "<a href=# onclick=\"openDetail('" + strurl + "');\">" + strysgc + "</a>";
            }
            
        }
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
    /// <summary>
    /// 通过单据编号和类型 获取查看页面的url
    /// </summary>
    /// <param name="strbillcode"></param>
    /// <param name="strType"></param>
    /// <returns></returns>
    private string GetUrl(string strbillcode,string ysgc,string strdeptcode,string strType) {
        string strkssj = Page.Request.QueryString["kssj"].ToString().Trim();
        string strnd = strkssj.Substring(0, 4);
        switch (strType)
        {
            case "1": return "../ysglnew/cwtbDetail.aspx?deptCode=" + strdeptcode + "&nd=" + strnd + "&type=ystb";//预算
            case "2": return "../ysgl/yszjEdit.aspx?type=look&billCode=" + strbillcode;//追加
            case "3": return "../ysgl/YstzDetailNew.aspx?type=look&billCode=" + strbillcode + "&deptcode=" + strdeptcode;//预算调整
            case "4": return "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + strbillcode + "&deptcode=" + strdeptcode;//科目之间预算调整
            case "5": return "../ysgl/ysnzjEdit.aspx?type=look&gcbh=" + ysgc + "&billCode=" + strbillcode + "&deptCode=" + strdeptcode;//预算内追加
            default: return "";
        }
    }
}
