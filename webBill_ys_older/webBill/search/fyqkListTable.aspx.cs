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

public partial class webBill_search_fyqkListTable : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string title = "";
        string tjzq = Page.Request.QueryString["tjzq"].ToString().Trim();
        string nian = Page.Request.QueryString["nian"].ToString().Trim();
        string jd = Page.Request.QueryString["jd"].ToString().Trim();
        string yue = Page.Request.QueryString["yue"].ToString().Trim();
        string dept = Page.Request.QueryString["dept"].ToString().Trim();
        string ysgcbh = "";

        string month1 = "";
        string month2 = "";
        string month3 = "";

        if (tjzq == "1")//年
        {
            ysgcbh = server.GetCellValue("select gcbh from bill_ysgc where ystype='0' and nian='" + nian + "'");
            title += "统计区间：" + nian + "年度预算";
        }
        else if (tjzq == "2")
        {
            ysgcbh = server.GetCellValue("select gcbh from bill_ysgc where ystype='1' and nian='" + nian + "' and yue='" + jd + "'");
            if (jd == "一")
            {
                month1 = nian + "01";
                month2 = nian + "02";
                month3 = nian + "03";
            }
            else if (jd == "二")
            {
                month1 = nian + "04";
                month2 = nian + "05";
                month3 = nian + "06";
            }
            else if (jd == "三")
            {
                month1 = nian + "07";
                month2 = nian + "08";
                month3 = nian + "09";
            }
            else if (jd == "四")
            {
                month1 = nian + "10";
                month2 = nian + "11";
                month3 = nian + "12";
            }
            title += "统计区间：第" + jd + "季度预算";
        }
        else if (tjzq == "3")
        {
            ysgcbh = server.GetCellValue("select gcbh from bill_ysgc where ystype='2' and nian='" + nian + "' and yue='" + yue + "'");
            month1 = nian + yue.ToString().PadLeft(2, '0');
            title += "统计区间：" + month1 + "月份预算";
        }
        if (dept == "")
        {
            title += "        统计单位：所有";
        }
        else
        {
            title += "        统计单位：" + server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode='" + dept + "'");
        }

        DataSet result = server.GetDataSet("exec bill_pro_fyqkb '" + dept + "','" + tjzq + "','" + ysgcbh + "','" + nian + "','" + month1 + "','" + month2 + "','" + month3 + "'");
        if (dept == "" )
        {
            CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryTable2.rpt"));
        }
        //else if (dept == "" && bxss == "image")
        //{
        //    CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryImage2.rpt"));
        //}
        else if (dept != "" )
        {
            CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryTable.rpt"));
        }
        //else if (dept != "" && bxss == "image")
        //{
        //    CrystalReportSource1.ReportDocument.Load(Server.MapPath("cryTable.rpt"));
        //}
        CrystalReportSource1.ReportDocument.SetDataSource(result.Tables[0]);
        CrystalReportSource1.ReportDocument.SetParameterValue("title", title);
        CrystalReportSource1.DataBind();

        CrystalReportViewer1.ReportSource = CrystalReportSource1;
        CrystalReportViewer1.DataBind();
    }
}
