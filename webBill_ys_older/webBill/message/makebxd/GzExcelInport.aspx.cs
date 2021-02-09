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
public partial class webBill_makebxd_GzExcelInport : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = new DataTable();
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
        string serverPath = Server.MapPath("~/Uploads/makebxd/");
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
        if (dt.Columns[0].ColumnName != "部门" || dt.Columns.Count != 30)
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "人数")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "岗位工资")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "薪级工资")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "护士10")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "护龄")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "住房补贴")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "基本工资")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "保健")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "独子")
        {
            check = false;
        }
        else if (dt.Columns[10].ColumnName != "院外")
        {
            check = false;
        }
        else if (dt.Columns[11].ColumnName != "生活护理费")
        {
            check = false;
        }
        else if (dt.Columns[12].ColumnName != "奖励性绩效工资")
        {
            check = false;
        }
        else if (dt.Columns[13].ColumnName != "基础性绩效工资")
        {
            check = false;
        }
        else if (dt.Columns[14].ColumnName != "电话费补助")
        {
            check = false;
        }
        else if (dt.Columns[15].ColumnName != "监察办案补贴")
        {
            check = false;
        }
        else if (dt.Columns[16].ColumnName != "返聘费")
        {
            check = false;
        }
        else if (dt.Columns[17].ColumnName != "人才津贴")
        {
            check = false;
        }
        else if (dt.Columns[18].ColumnName != "补工资")
        {
            check = false;
        }
        else if (dt.Columns[19].ColumnName != "应发合计")
        {
            check = false;
        }
        else if (dt.Columns[20].ColumnName != "房租")
        {
            check = false;
        }
        else if (dt.Columns[21].ColumnName != "扣基资")
        {
            check = false;
        }
        else if (dt.Columns[22].ColumnName != "公积金")
        {
            check = false;
        }
        else if (dt.Columns[23].ColumnName != "失业金")
        {
            check = false;
        }
        else if (dt.Columns[24].ColumnName != "养老保险")
        {
            check = false;
        }
        else if (dt.Columns[25].ColumnName != "医疗保险")
        {
            check = false;
        }
        else if (dt.Columns[26].ColumnName != "扣税")
        {
            check = false;
        }
        else if (dt.Columns[27].ColumnName != "合养保险")
        {
            check = false;
        }
        else if (dt.Columns[28].ColumnName != "合医保险")
        {
            check = false;
        }
        else if (dt.Columns[29].ColumnName != "实发合计")
        {
            check = false;
        }


        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "deptName";
        dt.Columns[1].ColumnName = "renshu";
        dt.Columns[2].ColumnName = "gwgz";
        dt.Columns[3].ColumnName = "xjgz";
        dt.Columns[4].ColumnName = "hs10";
        dt.Columns[5].ColumnName = "hl";
        dt.Columns[6].ColumnName = "zfbt";
        dt.Columns[7].ColumnName = "jbgz";
        dt.Columns[8].ColumnName = "bj";
        dt.Columns[9].ColumnName = "dz";
        dt.Columns[10].ColumnName = "yw";
        dt.Columns[11].ColumnName = "shhlf";
        dt.Columns[12].ColumnName = "jlxjxgz";
        dt.Columns[13].ColumnName = "jcxjxgz";
        dt.Columns[14].ColumnName = "dhfbz";
        dt.Columns[15].ColumnName = "dcbabt";
        dt.Columns[16].ColumnName = "fpf";
        dt.Columns[17].ColumnName = "rcjt";
        dt.Columns[18].ColumnName = "bgz";
        dt.Columns[19].ColumnName = "yfhj";
        dt.Columns[20].ColumnName = "fz";
        dt.Columns[21].ColumnName = "kjz";
        dt.Columns[22].ColumnName = "gjj";
        dt.Columns[23].ColumnName = "syj";
        dt.Columns[24].ColumnName = "ylbx";
        dt.Columns[25].ColumnName = "yilbx";
        dt.Columns[26].ColumnName = "ks";
        dt.Columns[27].ColumnName = "hybx";
        dt.Columns[28].ColumnName = "hyibx";
        dt.Columns[29].ColumnName = "sfhj";
        System.Collections.Generic.List<string> listSql = new System.Collections.Generic.List<string>();
        listSql.Add("delete from bill_gzExcel");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string strdeptname = dt.Rows[i]["deptName"].ToString();
            strdeptname = strdeptname.Trim();
            if (strdeptname.Equals("") || strdeptname.Equals("医疗") || strdeptname.Equals("医技") || strdeptname.Equals("药剂") || strdeptname.Equals("管理") || strdeptname.Equals("内退") || strdeptname.Equals("内退") || strdeptname.Equals("离退遗") || strdeptname.Equals("合计"))
            {
                continue;
            }
            listSql.Add(string.Format(@"insert into bill_gzExcel(部门,人数,岗位工资,薪级工资,护士10,护龄,住房补贴,基本工资,保健,独子,院外,生活护理费,奖励性绩效工资,基础性绩效工资,电话费补助,监察办案补贴,返聘费,人才津贴,补工资,应发合计,房租,扣基资,公积金,失业金,养老保险,医疗保险,扣税,合养保险,合医保险,实发合计) values
                ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}')", dt.Rows[i]["deptName"].ToString(), dt.Rows[i]["renshu"].ToString(), dt.Rows[i]["gwgz"].ToString(), dt.Rows[i]["xjgz"].ToString(), dt.Rows[i]["hs10"].ToString(), dt.Rows[i]["hl"].ToString(), dt.Rows[i]["zfbt"].ToString(), dt.Rows[i]["jbgz"].ToString(), dt.Rows[i]["bj"].ToString(), dt.Rows[i]["dz"].ToString(), dt.Rows[i]["yw"].ToString(), dt.Rows[i]["shhlf"].ToString(), dt.Rows[i]["jlxjxgz"].ToString(), dt.Rows[i]["jcxjxgz"].ToString(), dt.Rows[i]["dhfbz"].ToString(), dt.Rows[i]["dcbabt"].ToString(), dt.Rows[i]["fpf"].ToString(), dt.Rows[i]["rcjt"].ToString(), dt.Rows[i]["bgz"].ToString(), dt.Rows[i]["yfhj"].ToString(), dt.Rows[i]["fz"].ToString(), dt.Rows[i]["kjz"].ToString(), dt.Rows[i]["gjj"].ToString(), dt.Rows[i]["syj"].ToString(), dt.Rows[i]["ylbx"].ToString(), dt.Rows[i]["yilbx"].ToString(), dt.Rows[i]["ks"].ToString(), dt.Rows[i]["hybx"].ToString(), dt.Rows[i]["hyibx"].ToString(), dt.Rows[i]["sfhj"].ToString()));
        }
        //listSql.Add("update bill_gzExcel set ");

        if (server.ExecuteNonQuerysArray(listSql) >= 0 && listSql.Count > 2)
        {
            showMessage("导入数据成功！", true, "1");
        }
        else
        {
            showMessage("导入失败！", false, "0");
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