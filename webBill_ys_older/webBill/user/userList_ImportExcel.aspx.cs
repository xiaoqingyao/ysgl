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


public partial class webBill_user_userList_ImportExcel : System.Web.UI.Page
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
        string serverPath = Server.MapPath("~/Uploads/user/");
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
        if (dt.Columns[0].ColumnName != "'用户编号" && dt.Columns[0].ColumnName != "用户编号")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'姓名" && dt.Columns[1].ColumnName != "姓名")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'角色编号" && dt.Columns[2].ColumnName != "角色编号")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'状态" && dt.Columns[3].ColumnName != "状态")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'部门编号" && dt.Columns[4].ColumnName != "部门编号")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "'密码" && dt.Columns[5].ColumnName != "密码")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "'是否系统用户" && dt.Columns[6].ColumnName != "是否系统用户")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "'职位" && dt.Columns[7].ColumnName != "职位")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "userCode";
        dt.Columns[1].ColumnName = "userName";
        dt.Columns[2].ColumnName = "userGroup";
        dt.Columns[3].ColumnName = "userStatus";
        dt.Columns[4].ColumnName = "userDept";
        dt.Columns[5].ColumnName = "userPwd";
        dt.Columns[6].ColumnName = "isSystem";
        dt.Columns[7].ColumnName = "userPosition";
        string strReturnStr = "";
        IList<Bill_Users> list = new List<Bill_Users>();
        UsersDal userDal = new UsersDal();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string userCode = dt.Rows[i]["userCode"].ToString();
            if (userCode.Equals(""))
            {
                continue;
            }
            else if (userDal.Exists(userCode))
            {
                continue;
            }
            Bill_Users temp = new Bill_Users();
            temp.UserCode = Convert.ToString(dt.Rows[i]["userCode"]);
            temp.UserName = Convert.ToString(dt.Rows[i]["userName"]);
            temp.UserGroup = Convert.ToString(dt.Rows[i]["userGroup"]);
            temp.UserStatus = Convert.ToString(dt.Rows[i]["userStatus"]);
            temp.UserDept = Convert.ToString(dt.Rows[i]["userDept"]);
            temp.UserPwd = Convert.ToString(dt.Rows[i]["userPwd"]);
            temp.IsSystem = Convert.ToString(dt.Rows[i]["isSystem"]);
            temp.UserPosition = Convert.ToString(dt.Rows[i]["userPosition"]);
            list.Add(temp);
        }
        File.Delete(nowPath);
        if (list != null && list.Count > 0)
        {
            if (userDal.InsertList(list))
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

