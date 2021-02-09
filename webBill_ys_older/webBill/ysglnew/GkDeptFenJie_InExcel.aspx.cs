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
using System.IO;

public partial class webBill_ysglnew_GkDeptFenJie_InExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
            string tableName = schemaTable.Rows[1][2].ToString().Trim();
            string strSql = "select * from [" + tableName + "]";
            OleDbDataAdapter mycommand = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds, "[" + tableName + "]");
            dt = ds.Tables[0];
        }
        #endregion
        bool check = true;
        if (dt.Columns[0].ColumnName != "'年度" && dt.Columns[0].ColumnName != "年度")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'费用编号" && dt.Columns[1].ColumnName != "费用编号")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'部门编号" && dt.Columns[3].ColumnName != "部门编号")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "'1月份预算" && dt.Columns[7].ColumnName != "1月份预算")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "'2月份预算" && dt.Columns[8].ColumnName != "2月份预算")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "'3月份预算" && dt.Columns[9].ColumnName != "3月份预算")
        {
            check = false;
        }
        else if (dt.Columns[10].ColumnName != "'4月份预算" && dt.Columns[10].ColumnName != "4月份预算")
        {
            check = false;
        }
        else if (dt.Columns[11].ColumnName != "'5月份预算" && dt.Columns[11].ColumnName != "5月份预算")
        {
            check = false;
        }
        else if (dt.Columns[12].ColumnName != "'6月份预算" && dt.Columns[12].ColumnName != "6月份预算")
        {
            check = false;
        }
        else if (dt.Columns[13].ColumnName != "'7月份预算" && dt.Columns[13].ColumnName != "7月份预算")
        {
            check = false;
        }
        else if (dt.Columns[14].ColumnName != "'8月份预算" && dt.Columns[14].ColumnName != "8月份预算")
        {
            check = false;
        }
        else if (dt.Columns[15].ColumnName != "'9月份预算" && dt.Columns[15].ColumnName != "9月份预算")
        {
            check = false;
        }
        else if (dt.Columns[16].ColumnName != "'10月份预算" && dt.Columns[16].ColumnName != "10月份预算")
        {
            check = false;
        }
        else if (dt.Columns[17].ColumnName != "'11月份预算" && dt.Columns[17].ColumnName != "11月份预算")
        {
            check = false;
        }
        else if (dt.Columns[18].ColumnName != "'12月份预算" && dt.Columns[18].ColumnName != "12月份预算")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "nd";
        dt.Columns[1].ColumnName = "kmcode";
        dt.Columns[3].ColumnName = "deptcode";
        dt.Columns[7].ColumnName = "1";
        dt.Columns[8].ColumnName = "2";
        dt.Columns[9].ColumnName = "3";
        dt.Columns[10].ColumnName = "4";
        dt.Columns[11].ColumnName = "5";
        dt.Columns[12].ColumnName = "6";
        dt.Columns[13].ColumnName = "7";
        dt.Columns[14].ColumnName = "8";
        dt.Columns[15].ColumnName = "9";
        dt.Columns[16].ColumnName = "10";
        dt.Columns[17].ColumnName = "11";
        dt.Columns[18].ColumnName = "12";


        Session["nowfjjedt"] = dt;
        File.Delete(nowPath);
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
