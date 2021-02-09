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

public partial class webBill_cwkm_ExcelInport : System.Web.UI.Page
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
            showMessage("请先选择上传的文件！",false,"");
            return;
        }
        if (strfileName.Split('.')[1] != "xls")
        {
            showMessage("上传文件必须是xls格式！",false,"");
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
        string constr = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source =" + nowPath + ";Extended Properties=Excel 8.0";
        DataTable dt = new DataTable();
        using (OleDbConnection conn=new OleDbConnection(constr))
        {
            conn.Open();
            //取得excel表中的字段名和表名
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strSql = "select * from ["+tableName+"]";
            OleDbDataAdapter mycommand = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds,"["+tableName+"]");
            dt=ds.Tables[0];
        }
        #endregion
        bool check = true;
        if (dt.Columns[0].ColumnName!="科目编码"||dt.Columns.Count!=8)
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "科目名称"){
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "显示名称"){
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "科目类型")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "方向")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "级次")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "辅助核算")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "是否封存")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName="kmbm";
        dt.Columns[1].ColumnName="kmmc";
        dt.Columns[2].ColumnName="xsmc";
        dt.Columns[3].ColumnName="kmlx";
        dt.Columns[4].ColumnName="fx";
        dt.Columns[5].ColumnName="jc";
        dt.Columns[6].ColumnName="fzhs";
        dt.Columns[7].ColumnName="sffc";

        System.Collections.Generic.List<string> listSql = new System.Collections.Generic.List<string>();

        listSql.Add("delete from bill_cwkm");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            listSql.Add(string.Format(@"insert into bill_cwkm(cwkmCode,cwkmBm,cwkmMc,XianShiMc,Type,Fangxiang,JiCi,FuZhuHeSuan,ShiFouFengCun) values
                ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", dt.Rows[i]["kmbm"].ToString(), dt.Rows[i]["kmbm"].ToString(), dt.Rows[i]["kmmc"].ToString(), dt.Rows[i]["xsmc"].ToString(), dt.Rows[i]["kmlx"].ToString(), dt.Rows[i]["fx"].ToString(), dt.Rows[i]["jc"].ToString(), dt.Rows[i]["fzhs"].ToString(), dt.Rows[i]["sffc"].ToString()));
        }
        listSql.Add("delete from bill_cwkm where isnull(cwkmCode,'')=''");
        if (server.ExecuteNonQuerysArray(listSql) >= 0)
        {
            showMessage("导入数据成功，请关闭该功能标签重新打开以显示最新数据！", true, "");
        }
        else {
            showMessage("导入失败！",false,"");
        }
        
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
