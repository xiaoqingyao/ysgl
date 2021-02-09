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
using Dal.SysDictionary;

public partial class webBill_yskm_yskmImportExcel : System.Web.UI.Page
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
        string serverPath = Server.MapPath("~/Uploads/yskm/");
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
        if (dt.Columns[0].ColumnName != "'科目编号" && dt.Columns[0].ColumnName != "科目编号")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'科目编码" && dt.Columns[1].ColumnName != "科目编码")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'科目名称" && dt.Columns[2].ColumnName != "科目名称")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'填报说明" && dt.Columns[3].ColumnName != "填报说明")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'填报类型" && dt.Columns[4].ColumnName != "填报类型")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "'科目状态" && dt.Columns[5].ColumnName != "科目状态")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "'科目类型" && dt.Columns[6].ColumnName != "科目类型")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "'归口费用" && dt.Columns[7].ColumnName != "归口费用")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "'项目核算" && dt.Columns[8].ColumnName != "项目核算")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "'部门核算" && dt.Columns[9].ColumnName != "部门核算")
        {
            check = false;
        }
        else if (dt.Columns[10].ColumnName != "'人员核算" && dt.Columns[10].ColumnName != "人员核算")
        {
            check = false;
        }
        else if (dt.Columns[11].ColumnName != "'科目主管" && dt.Columns[11].ColumnName != "科目主管")
        {
            check = false;
        }
        else if (dt.Columns[12].ColumnName != "'对应单据" && dt.Columns[12].ColumnName != "对应单据")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "yskmCode";
        dt.Columns[1].ColumnName = "yskmBm";
        dt.Columns[2].ColumnName = "yskmMc";
        dt.Columns[3].ColumnName = "tbsm";
        dt.Columns[4].ColumnName = "tblx";
        dt.Columns[5].ColumnName = "kmStatus";
        dt.Columns[6].ColumnName = "kmlx";
        dt.Columns[7].ColumnName = "gkfy";
        dt.Columns[8].ColumnName = "xmhs";
        dt.Columns[9].ColumnName = "bmhs";
        dt.Columns[10].ColumnName = "ryhs";
        dt.Columns[11].ColumnName = "kmzg";
        dt.Columns[12].ColumnName = "dydj"; 
        string strReturnStr = "";
        IList<Bill_Yskm> list = new List<Bill_Yskm>();

        YskmDal Dal = new YskmDal();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string yskmCode = dt.Rows[i]["yskmCode"].ToString();
            if (yskmCode.Equals(""))
            {
                continue;
            }
            else if (Dal.Exists(yskmCode))
            {
                continue;
            }
            Bill_Yskm temp = new Bill_Yskm();
            temp.YskmCode = Convert.ToString(dt.Rows[i]["yskmCode"]);
            temp.YskmBm = Convert.ToString(dt.Rows[i]["yskmBm"]);
            temp.YskmMc = Convert.ToString(dt.Rows[i]["yskmMc"]);
            temp.Tbsm = Convert.ToString(dt.Rows[i]["tbsm"]);
            temp.Tblx = Convert.ToString(dt.Rows[i]["tblx"]);
            temp.KmStatus = Convert.ToString(dt.Rows[i]["kmStatus"]);
            temp.KmLx = Convert.ToString(dt.Rows[i]["kmlx"]);
            temp.GkFy = Convert.ToString(dt.Rows[i]["gkfy"]);
            temp.XmHs = Convert.ToString(dt.Rows[i]["xmhs"]);
            temp.BmHs = Convert.ToString(dt.Rows[i]["bmhs"]);
            temp.RyHs = Convert.ToString(dt.Rows[i]["ryhs"]);
            temp.Kmzg = Convert.ToString(dt.Rows[i]["kmzg"]);
            temp.dydj = Convert.ToString(dt.Rows[i]["dydj"]);
            list.Add(temp);
        }
        File.Delete(nowPath);
        if (list != null && list.Count > 0)
        {
            if (Dal.InsertList(list))
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
