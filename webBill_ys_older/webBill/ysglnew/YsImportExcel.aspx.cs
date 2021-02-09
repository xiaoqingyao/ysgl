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
using Models;
using System.Collections.Generic;
using System.IO;
using Dal.UserProperty;
using System.Data.SqlClient;
using Dal;

public partial class webBill_ysgl_YsImportExcel : System.Web.UI.Page
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
            string strSql = "select * from [" + tableName + "]";
            OleDbDataAdapter mycommand = new OleDbDataAdapter(strSql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds, "[" + tableName + "]");
            dt = ds.Tables[0];
        }
        #endregion
        bool check = true;
        if (dt.Columns[0].ColumnName != "'部门编号" && dt.Columns[0].ColumnName != "部门编号")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'部门名称" && dt.Columns[1].ColumnName != "部门名称")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'科目编号" && dt.Columns[2].ColumnName != "科目编号")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'科目名称" && dt.Columns[3].ColumnName != "科目名称")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'1月" && dt.Columns[4].ColumnName != "1月")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "'2月" && dt.Columns[5].ColumnName != "2月")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "'3月" && dt.Columns[6].ColumnName != "3月")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "'4月" && dt.Columns[7].ColumnName != "4月")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "'5月" && dt.Columns[8].ColumnName != "5月")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "'6月" && dt.Columns[9].ColumnName != "6月")
        {
            check = false;
        }
        else if (dt.Columns[10].ColumnName != "'7月" && dt.Columns[10].ColumnName != "7月")
        {
            check = false;
        }
        else if (dt.Columns[11].ColumnName != "'8月" && dt.Columns[11].ColumnName != "8月")
        {
            check = false;
        }
        else if (dt.Columns[12].ColumnName != "'9月" && dt.Columns[12].ColumnName != "9月")
        {
            check = false;
        }
        else if (dt.Columns[13].ColumnName != "'10月" && dt.Columns[13].ColumnName != "10月")
        {
            check = false;
        }
        else if (dt.Columns[14].ColumnName != "'11月" && dt.Columns[14].ColumnName != "11月")
        {
            check = false;
        }
        else if (dt.Columns[15].ColumnName != "'12月" && dt.Columns[15].ColumnName != "12月")
        {
            check = false;
        }
        else if (dt.Columns[16].ColumnName != "'合计" && dt.Columns[16].ColumnName != "合计")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "deptcode";
        dt.Columns[1].ColumnName = "deptname";
        dt.Columns[2].ColumnName = "yskmcode";
        dt.Columns[3].ColumnName = "yskmmc";
        dt.Columns[4].ColumnName = "yi";
        dt.Columns[5].ColumnName = "er";
        dt.Columns[6].ColumnName = "san";
        dt.Columns[7].ColumnName = "si";
        dt.Columns[8].ColumnName = "wu";
        dt.Columns[9].ColumnName = "liu";
        dt.Columns[10].ColumnName = "qi";
        dt.Columns[11].ColumnName = "ba";
        dt.Columns[12].ColumnName = "jiu";
        dt.Columns[13].ColumnName = "shi";
        dt.Columns[14].ColumnName = "shiyi";
        dt.Columns[15].ColumnName = "shier";
        dt.Columns[16].ColumnName = "nian";

        File.Delete(nowPath);

        int icount = dt.Rows.Count;

        if (icount > 0)
        {
            string strid = new GuidHelper().getNewGuid();
            string insql = " insert into bill_ys_import (id,deptcode,deptname,yskmcode,yskmmc,yi,er,san,si,wu,liu,qi,ba,jiu,shi,shiyi,shier,nian)values(@id,@deptcode,@deptname,@yskkmcode,@yskmmc,@yi,@er,@san,@si,@wu,@liu,@qi,@ba,@jiu,@shi,@shiyi,@shier,@nian)";
            using (SqlConnection conn = new SqlConnection(Dal.DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    for (int i = 0; i < icount; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        SqlParameter[] inparamter = {
                                                    new SqlParameter("@id",strid),
                                                        new SqlParameter("@deptcode",dr["deptcode"]),
                                                          new SqlParameter("@deptname",dr["deptname"]),
                                                          new SqlParameter("@yskkmcode",dr["yskmcode"]),
                                                          new SqlParameter("@yskmmc",dr["yskmmc"]),
                                                          new SqlParameter("@yi",dr["yi"]),
                                                          new SqlParameter("@er",dr["er"]),
                                                          new SqlParameter("@san",dr["san"]),
                                                          new SqlParameter("@si",dr["si"]),
                                                          new SqlParameter("@wu",dr["wu"]),
                                                          new SqlParameter("@liu",dr["liu"]),
                                                          new SqlParameter("@qi",dr["qi"]),
                                                          new SqlParameter("@ba",dr["ba"]),
                                                          new SqlParameter("@jiu",dr["jiu"]),
                                                          new SqlParameter("@shi",dr["shi"]),
                                                          new SqlParameter("@shiyi",dr["shiyi"]),
                                                          new SqlParameter("@shier",dr["shier"]),
                                                          new SqlParameter("@nian",dr["nian"]) };
                        DataHelper.ExcuteNonQuery(insql, tran, inparamter, false);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算数导入失败！');", true);
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
