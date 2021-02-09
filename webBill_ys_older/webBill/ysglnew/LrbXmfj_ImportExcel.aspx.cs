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
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using Dal;

public partial class webBill_ysglnew_LrbXmfj_ImportExcel : System.Web.UI.Page
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
        string serverPath = Server.MapPath("~/Uploads/ysgl/");
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
            string strSql = "select * from [" + tableName + "$]";//
            OleDbDataAdapter mycommand = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds, "[" + tableName + "]");
            dt = ds.Tables[0];
        }
        #endregion
        bool check = true;
        if (dt.Columns[0].ColumnName != "'序号" && dt.Columns[0].ColumnName != "序号")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'年度" && dt.Columns[1].ColumnName != "年度")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'利润项目" && dt.Columns[2].ColumnName != "利润项目")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'预算科目" && dt.Columns[3].ColumnName != "预算科目")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'预算控制金额" && dt.Columns[4].ColumnName != "预算控制金额")
        {
            check = false;
        }
       
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "xu";
        dt.Columns[1].ColumnName = "nd";
        dt.Columns[2].ColumnName = "xm";
        dt.Columns[3].ColumnName = "yskm";
        dt.Columns[4].ColumnName = "je";

        File.Delete(nowPath);

        int icount = dt.Rows.Count;

        if (icount > 0)
        {
            string strid = new GuidHelper().getNewGuid();
            string insql = " insert into bill_ys_xmfjlrb (procode,kmcode,budgetmoney,annual) values(@procode,@kmcode,@budgetmoney,@annual)";
            using (SqlConnection conn = new SqlConnection(Dal.DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    //删除年度的分解信息
                    DataHelper.ExcuteNonQuery("delete from bill_ys_xmfjlrb where annual='"+dt.Rows[0]["nd"].ToString()+"'", null, false);
                    for (int i = 0; i < icount; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        //分解金额
                        decimal deje = 0;
                        if (!decimal.TryParse(dr["je"].ToString().Equals("")?"0":dr["je"].ToString(), out deje)) {
                            throw new Exception("第"+(i+1)+"行，金额输入不正确");
                        }
                        //预算科目
                        string stryskm = dr["yskm"].ToString();
                        stryskm = stryskm.Substring(1, stryskm.IndexOf(']') - 1);
                        //预算项目档案
                        string strxm = dr["xm"].ToString();
                        strxm = strxm.Substring(1, strxm.IndexOf(']') -1);

                        SqlParameter[] inparamter = {
                                                        new SqlParameter("@procode",strxm),
                                                        new SqlParameter("@kmcode",stryskm),
                                                        new SqlParameter("@budgetmoney",deje),
                                                        new SqlParameter("@annual",dr["nd"])
                                                    };
                        DataHelper.ExcuteNonQuery(insql, tran, inparamter, false);
                    }
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算数导入失败,"+ex.Message+"');", true);
                    throw;
                }
                Session["ysdt"] = strid;
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('导入成功！');window.returnValue='ok';self.close();", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('导入0条记录！');window.returnValue='ok';self.close();", true);
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
