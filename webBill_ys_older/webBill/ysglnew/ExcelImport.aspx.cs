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
using System.Data.OleDb;

public partial class webBill_ysglnew_ExcelImport : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
            //this.DataGrid1.DataSource = dt;
            //this.DataGrid1.DataBind();
            //this.myg
            //myGridView1.DataBind();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string strfileName = FileUpload1.FileName;
        if (string.IsNullOrEmpty(strfileName))
        {
            showMessage("请先选择上传的文件！", false, "");
            return;
        }
        if (strfileName.Split('.')[1] != "xls")
        {
            showMessage("上传文件必须是xls格式！", false, "");
            return;
        }
        string serverPath = Server.MapPath("~/Uploads/Cwkm/");
        if (!System.IO.Directory.Exists(serverPath))
        {
            System.IO.Directory.CreateDirectory(serverPath);
        }
        string[] fileNames = System.IO.Directory.GetFiles(serverPath);
        string date = DateTime.Now.ToString("yyyyMMdd");
        int todayFile = (from fileName in fileNames where fileName.IndexOf(date) > 0 select fileName).Count();
        string newFileName = date + (todayFile + 1).ToString("0000");
        string nowPath = serverPath + "\\" + newFileName + ".xls";
        FileUpload1.SaveAs(nowPath);
        #region --------读取文件内容到服务器内存----------
        string constr = " Provider=Microsoft.Jet.Oledb.4.0; Data Source =" + nowPath + ";Extended Properties='Excel 8.0; HDR=Yes; IMEX=1;'";
        DataTable dt = new DataTable();
        using (OleDbConnection conn = new OleDbConnection(constr))
        {
            conn.Open();
            //取得excel表中的字段名和表名
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strSql = "select * from [" + tableName + "]";
            OleDbDataAdapter mycommand = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds, "[" + tableName + "]");
            dt = ds.Tables[0];
        }
        #endregion
        bool check = true;
        if (dt.Columns[0].ColumnName != "科目编码")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "月份\\科目" || dt.Columns.Count != 15)
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "1月份")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "2月份")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "3月份")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "4月份")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "5月份")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "6月份")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "7月份")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "8月份")
        {
            check = false;
        }
        else if (dt.Columns[10].ColumnName != "9月份")
        {
            check = false;
        }
        else if (dt.Columns[11].ColumnName != "10月份")
        {
            check = false;
        }
        else if (dt.Columns[12].ColumnName != "11月份")
        {
            check = false;
        }
        else if (dt.Columns[13].ColumnName != "12月份")
        {
            check = false;
        }
        else if (dt.Columns[14].ColumnName != "年度总预算")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "kmbh";
        dt.Columns[1].ColumnName = "km";
        dt.Columns[2].ColumnName = "January";
        dt.Columns[3].ColumnName = "February";
        dt.Columns[4].ColumnName = "march";
        dt.Columns[5].ColumnName = "April";
        dt.Columns[6].ColumnName = "May";
        dt.Columns[7].ColumnName = "June";
        dt.Columns[8].ColumnName = "July";
        dt.Columns[9].ColumnName = "August";
        dt.Columns[10].ColumnName = "September";
        dt.Columns[11].ColumnName = "October";
        dt.Columns[12].ColumnName = "November";
        dt.Columns[13].ColumnName = "December";
        dt.Columns[14].ColumnName = "year";

        dt.Columns.Add("JanuaryYsnZj");
        dt.Columns.Add("FebruaryYsnZj");
        dt.Columns.Add("marchYsnZj");
        dt.Columns.Add("AprilYsnZj");
        dt.Columns.Add("MayYsnZj");
        dt.Columns.Add("JuneYsnZj");
        dt.Columns.Add("JulyYsnZj");
        dt.Columns.Add("AugustYsnZj");
        dt.Columns.Add("SeptemberYsnZj");
        dt.Columns.Add("OctoberYsnZj");
        dt.Columns.Add("NovemberYsnZj");
        dt.Columns.Add("DecemberYsnZj");
        dt.Columns.Add("yearYsnZj");

        Session["nowdt"] = dt;
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"y\";self.close();", true);
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }
}
