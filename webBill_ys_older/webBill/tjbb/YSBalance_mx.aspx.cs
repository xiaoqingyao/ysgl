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
using System.Data.SqlClient;

public partial class webBill_tjbb_YSBalance_mx : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    private void BindData()
    {
        myGrid.DataSource = getData("1");
        myGrid.DataBind();

    }

    protected void RdbMx_CheckedChanged(object sender, EventArgs e)
    {
        xianshichanged();
    }

    protected void RdbXmDangAn_CheckedChanged(object sender, EventArgs e)
    {
        xianshichanged();
    }

    private void xianshichanged()
    {
        if (this.RdbXmDangAn.Checked)
        {
            this.myGrid.Visible = false;
            this.DGxiangmu.Visible = true;
            this.DGxiangmu.DataSource = getData("0");
            this.DGxiangmu.DataBind();
        }
        else
        {
            this.DGxiangmu.Visible = false;
            this.myGrid.Visible = true;
            this.myGrid.DataSource = getData("1");
            this.myGrid.DataBind();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strMxOrXiangmu">明细还是项目  1明细 0 项目</param>
    /// <returns></returns>
    private DataSet getData(string strMxOrXiangmu)
    {
        string nd = Convert.ToString(Request["nd"]);
        string ysgc = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nd + "' and yue=" + Convert.ToString(Request["yf"]));
        string flag = Convert.ToString(Request["flag"]);
        string strfactflg = Convert.ToString(Request["fcflg"]);//实际or预算
        msg.InnerText = "【友情提示】：" + nd + "年" + Convert.ToString(Request["yf"]) + "月预算过程";
        DataSet dt = server.GetDataSet("exec pro_tj_pinghengbiao_mx '" + nd + "','" + ysgc + "','" + flag + "','" + strfactflg + "','" + strMxOrXiangmu + "'");
        return dt;
    }

    protected void btn_excel_Click(object sender, EventArgs e)
    {
        string strMxOrXiangmu = "";
        if (this.RdbXmDangAn.Checked)
        {
            strMxOrXiangmu = "0";

        }
        else
        {
            strMxOrXiangmu = "1";
        }

        string nd = Convert.ToString(Request["nd"]);
        string ysgc = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nd + "' and yue=" + Convert.ToString(Request["yf"]));
        string flag = Convert.ToString(Request["flag"]);
        string strfactflg = Convert.ToString(Request["fcflg"]);//实际or预算
        //  msg.InnerText = "【友情提示】：" + nd + "年" + Convert.ToString(Request["yf"]) + "月预算过程";
        DataTable dt = server.GetDataTable("exec pro_tj_pinghengbiao_mx '" + nd + "','" + ysgc + "','" + flag + "','" + strfactflg + "','" + strMxOrXiangmu + "'", null);


        new ExcelHelper().ExpExcel(dt, myGrid);//(dt, "file", null);

        //BindData();
        //Response.ClearContent();

        //Response.Charset = "utf-8";
        //Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");

        //Response.AddHeader("content-disposition", "attachment; filename=XXExcelFile.xls");

        //Response.ContentType = "application/excel";

        //StringWriter sw = new StringWriter();

        //HtmlTextWriter htw = new HtmlTextWriter(sw);

        //myGrid.RenderControl(htw);

        //Response.Write(sw.ToString());

        //Response.End();
    }
}
