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


public partial class webBill_Dept_DeptImportExcel : System.Web.UI.Page
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
        string serverPath = Server.MapPath("~/Uploads/dept/");
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
        else if (dt.Columns[2].ColumnName != "'上级部门编号" && dt.Columns[2].ColumnName != "上级部门编号")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'部门状态" && dt.Columns[3].ColumnName != "部门状态")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'是否销售部" && dt.Columns[4].ColumnName != "是否销售部")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "'助记简码" && dt.Columns[5].ColumnName != "助记简码")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "'对应U8编号" && dt.Columns[6].ColumnName != "对应U8编号")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "deptCode";
        dt.Columns[1].ColumnName = "deptName";
        dt.Columns[2].ColumnName = "sjDeptCode";
        dt.Columns[3].ColumnName = "deptStatus";
        dt.Columns[4].ColumnName = "IsSell";
        dt.Columns[5].ColumnName = "deptJianma";
        dt.Columns[6].ColumnName = "foru8id";

        string strReturnStr = "";
        IList<Bill_Departments> deptList=new List<Bill_Departments>();
        DepartmentDal bllDept = new DepartmentDal();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string deptcode = dt.Rows[i]["deptCode"].ToString();
            if (deptcode.Equals(""))
            {
                continue;
            }
            else if (bllDept.Exists(deptcode))
            {
                continue;
            }
            else if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["sjDeptCode"])))
            {
                continue;
            }
            Bill_Departments temp = new Bill_Departments();
            temp.DeptCode =Convert.ToString(dt.Rows[i]["deptCode"]);
            temp.DeptName = Convert.ToString(dt.Rows[i]["deptName"]);
            temp.SjDeptCode = Convert.ToString(dt.Rows[i]["sjDeptCode"]);
            temp.DeptStatus = Convert.ToString(dt.Rows[i]["deptStatus"]);
            temp.isSell = Convert.ToString(dt.Rows[i]["IsSell"]);
            temp.deptJianma = Convert.ToString(dt.Rows[i]["deptJianma"]);
            temp.forU8id = Convert.ToString(dt.Rows[i]["foru8id"]);
            deptList.Add(temp);
        }
        File.Delete(nowPath);
        if (deptList != null && deptList.Count > 0)
        {
            if (bllDept.InsertList(deptList))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('导入成功！');window.returnValue='ok';self.close();", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('导入失败！');", true);
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
