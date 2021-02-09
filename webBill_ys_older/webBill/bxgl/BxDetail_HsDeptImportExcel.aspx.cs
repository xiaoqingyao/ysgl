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

public partial class webBill_bxgl_BxDetail_HsDeptImportExcel : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strkmcodename = "";//科目名称
    string strkmcode = "";//科目编号
    protected void Page_Load(object sender, EventArgs e)
    {
        object objkmcode = Request["kmcode"];
        if (objkmcode != null)
        {
            strkmcodename = objkmcode.ToString();
            strkmcode = strkmcodename.Substring(1, strkmcodename.IndexOf("]") - 1);
        }
        else { showMessage("参数不完整。", true, ""); return; }
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
        if (dt.Columns[1].ColumnName != "'部门编号" && dt.Columns[1].ColumnName != "部门编号")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'部门名称" && dt.Columns[2].ColumnName != "部门名称")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'报销金额" && dt.Columns[3].ColumnName != "报销金额")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "kmmc";
        dt.Columns[1].ColumnName = "deptcode";
        dt.Columns[2].ColumnName = "deptname";
        dt.Columns[3].ColumnName = "je";





        string strReturnStr = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //判断部门是否禁用

            string strdeptcode = dt.Rows[i]["deptcode"].ToString();
            string sql = "select deptStatus from bill_departments where deptCode='" + strdeptcode + "'";

            string status = server.GetCellValue(sql);
            if (status != "1")
            {
                showMessage(strdeptcode + "部门已经禁用，不允许导入，请检查表格中部门是否有效。", true, ""); return;
            }

            string strkm = dt.Rows[i]["kmmc"].ToString();
            if (strkm.Equals(""))
            {
                continue;
            }
            if (strkm != strkmcodename && (("'" + strkmcodename) != strkm))
            {
                showMessage("科目名称不正确，请检查导入excel的第一列。", true, ""); return;
            }
            decimal deje = 0;
            string strje = dt.Rows[i]["je"].ToString();
            if (decimal.TryParse(strje, out deje))
            {
                strReturnStr += string.Format("<li><span>[{0}]{1}</span><input type='text' value='{2}' /></li>", dt.Rows[i]["deptcode"], dt.Rows[i]["deptname"], strje);
            }
        }
        File.Delete(nowPath);
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + strReturnStr + "\";self.close();", true);
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

    ////protected void aa_click(object sender, EventArgs e) {
    ////    export();
    ////}
    ////private void export()
    ////{
    ////    DataTable dtRel = new sqlHelper.sqlHelper().GetDataTable("select depart.deptcode,depart.deptname,(select '['+yskmbm+']'+yskmmc from bill_yskm where yskmbm=yskmdept.yskmcode) as yskmmc from bill_departments depart inner join bill_yskm_dept yskmdept on depart.deptcode=yskmdept.deptcode where yskmcode=@yskmcode", new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@yskmcode", strkmcode) });
    ////    //临时文件    
    ////    string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());

    ////    //使用OleDb连接  
    ////    OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
    ////    using (con)
    ////    {
    ////        con.Open();
    ////        //创建Sheet   
    ////        OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([费用名称] VarChar,[部门编号] VarChar,[部门名称] VarChar,[报销金额] VarChar)", con);
    ////        cmdCreate.ExecuteNonQuery();
    ////        //插入数据     
    ////        for (int i = 0; i < dtRel.Rows.Count; i++)
    ////        {
    ////            DataRow dr = dtRel.Rows[i];
    ////            string stryskmmc = dr["yskmmc"].ToString();
    ////            string strdeptcode = dr["deptcode"].ToString();
    ////            string strdeptname = dr["deptname"].ToString();

    ////            using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@yskmmc,@deptcode,@deptname,@je)", con))
    ////            {
    ////                cmd.Parameters.AddWithValue("@yskmmc", stryskmmc);
    ////                cmd.Parameters.AddWithValue("@deptcode", strdeptcode);
    ////                cmd.Parameters.AddWithValue("@deptname", strdeptname);
    ////                cmd.Parameters.AddWithValue("@je", "");
    ////                cmd.ExecuteNonQuery();
    ////            }
    ////        }
    ////    }
    ////    if (con.State == ConnectionState.Closed)
    ////    {
    ////        Response.ContentType = "application/ms-excel";
    ////        Response.AppendHeader("Content-Disposition", "attachment;filename=sybm.xls");
    ////        Response.BinaryWrite(File.ReadAllBytes(tempFile));
    ////        Response.End();
    ////        File.Delete(tempFile);
    ////    }
    ////}
}
