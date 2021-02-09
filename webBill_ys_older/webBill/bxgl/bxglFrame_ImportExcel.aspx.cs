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
using Bll;
using Bll.UserProperty;


public partial class webBill_bxgl_bxglFrame_ImportExcel : System.Web.UI.Page
{


    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigBLL configbll = new ConfigBLL();
    DataTable dtuserRightDept = new DataTable();
    DepartmentDal deptDal = new DepartmentDal();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            BindYsgc();
            txt_billDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                switch (Request["dydj"])
                {
                    case "01": this.Title = "收入报告单EXCEL导入"; break;
                    case "02": this.Title = "报销单EXCEL导入"; break;
                    case "03": this.Title = "固定资产购置单EXCEL导入"; break;
                    case "04": this.Title = "存货领用单EXCEL导入"; break;
                    case "05": this.Title = "往来付款单EXCEL导入"; break;
                    default:
                        break;
                }
            }
        }

        DataTable dtdept = deptDal.getUsercodeName(Session["userCode"].ToString().Trim());
        strNowDeptCode = dtdept.Rows[0]["deptcode"].ToString();
        strNowDeptName = dtdept.Rows[0]["deptName"].ToString();
        string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");

        dtuserRightDept = deptDal.getRigtusers(strDeptCodes, strNowDeptCode);

        #region 绑定人员管理下的部门
        if (!strNowDeptCode.Equals(""))
        {
            //获取人员管理下的部门
            if (strDeptCodes != "")
            {
                for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
                {
                    ListItem li = new ListItem();
                    li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
                    li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
                    this.ddl_dept.Items.Add(li);
                }
            }
            this.ddl_dept.Items.Insert(0, new ListItem("[" + strNowDeptCode + "]" + strNowDeptName, strNowDeptCode));

            this.ddl_dept.SelectedIndex = 1;
        }

        #endregion

    }


    private void BindYsgc()
    {
        //string nd=System.DateTime.Now.Year.ToString();
        //string yue=System.DateTime.Now.Month.ToString();
        //DataTable dt = server.GetDataTable("select gcbh,xmmc from bill_ysgc  where ysType='2'  and nian='"+nd+"' ",null);
        //ddlNd.DataSource=dt;
        //ddlNd.DataTextField="xmmc";
        //ddlNd.DataValueField="gcbh";
        //ddlNd.DataBind();
        //ddlNd.SelectedValue = server.GetCellValue("select gcbh from bill_ysgc where ysType='2' and  nian='" + nd + "' and yue='" + yue + "'",null);    

    }
    /// <summary>
    /// 制单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_zd_Click(object sender, EventArgs e)
    {
        //单据唯一键
        string strbillcode = new GuidHelper().getNewGuid();
        List<string> lstsql = new List<string>();
        for (int i = 0; i < Repeater1.Items.Count; i++)
        {
            string strsql = "";
            string strzdrcode = Session["userCode"].ToString().Trim();
            string strzddept = "";
            string strhsje = "";
            string strgkdept = "";
            string strbxzy = "";
            string stryskmcode = "";
            string strhsdeptcode = "";
            string strdate = "";

            HiddenField hf = Repeater1.Items[i].FindControl("hfDeptCode") as HiddenField;
            strzddept = hf.Value;
            strgkdept = hf.Value;
            strhsdeptcode = hf.Value;

            hf = Repeater1.Items[i].FindControl("hfFykmCode") as HiddenField;
            stryskmcode = hf.Value;


            hf = Repeater1.Items[i].FindControl("hfHsje") as HiddenField;
            strhsje = hf.Value;
            if (string.IsNullOrEmpty(strhsje))
            {
                continue;
            }
            YsManager ysmgr = new YsManager();
            string strysgc = ysmgr.GetYsgcCode(DateTime.Parse(txt_billDate.Text));


            string strmouth = server.GetCellValue("select yue from bill_ysgc where gcbh='" + strysgc + "'");
            if (string.IsNullOrEmpty(strysgc) || string.IsNullOrEmpty(strmouth))
            {
                showMessage("没有对应的预算过程，请开启预算过程。", false, "");
                return;
            }

            strdate = DateTime.Now.ToString("yy-MM-dd");//System.DateTime.Now.Year.ToString() + "-" + strmouth + "-01";

            strbxzy = server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + strysgc + "'") + "收入报告导入";
            strsql = @"insert into lsbxd_main(billcode,flowid,billUser,billDate,billDept,je,se,isgk,gkdept,bxzy,bxsm,fykmcode,sydept,bxlx)
                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')";
            strsql = string.Format(strsql, strbillcode, "srd", strzdrcode, strdate, strzddept, strhsje, "0", "1", strgkdept, strbxzy, "", stryskmcode, strhsdeptcode, "01");
            lstsql.Add(strsql);
        }
        if (lstsql.Count > 0)
        {
            int irels = server.ExecuteNonQuerysArray(lstsql);
            if (irels >= 1)
            {
                string strbillname = server.GetCellValue(" exec [pro_makebxd] '" + strbillcode + "','srd'");
                server.ExecuteNonQuery("delete lsbxd_main  where billcode='" + strbillcode + "'", null);

                showMessage("保存成功", true, "1");
              //  ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('收入报告单生成成功，单号为：" + strbillname + "');self.close();", true);


            }
        }
    }
    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        string serverPath = Server.MapPath("~/Uploads/srbg/");
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
        if (dt.Columns[0].ColumnName != "'部门编号" && dt.Columns[0].ColumnName != "部门编号")
        {
            check = false;
        }
        else if (dt.Columns[1].ColumnName != "'部门名称" && dt.Columns[1].ColumnName != "部门名称")
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "'费用科目编号" && dt.Columns[2].ColumnName != "费用科目编号")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "'费用科目名称" && dt.Columns[3].ColumnName != "费用科目名称")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "'决算金额" && dt.Columns[4].ColumnName != "决算金额")
        {
            check = false;
        }
        else if (dt.Columns[5].ColumnName != "'核算部门编号" && dt.Columns[5].ColumnName != "核算部门编号")
        {
            check = false;
        }
        else if (dt.Columns[6].ColumnName != "'核算部门名称" && dt.Columns[6].ColumnName != "核算部门名称")
        {
            check = false;
        }
        else if (dt.Columns[7].ColumnName != "'核算金额" && dt.Columns[7].ColumnName != "核算金额")
        {
            check = false;
        }
        else if (dt.Columns[8].ColumnName != "'报销摘要" && dt.Columns[8].ColumnName != "报销摘要")
        {
            check = false;
        }
        else if (dt.Columns[9].ColumnName != "'报销说明" && dt.Columns[9].ColumnName != "报销说明")
        {
            check = false;
        }
        if (!check)
        {
            showMessage("上传EXCEL格式不合法！", false, "");
            return;
        }
        dt.Columns[0].ColumnName = "deptCode";  //部门编号
        dt.Columns[1].ColumnName = "deptName";  //部门名称
        dt.Columns[2].ColumnName = "fykmCode";  //费用科目编号
        dt.Columns[3].ColumnName = "fykmName";  //费用科目名称
        dt.Columns[4].ColumnName = "bxje";      //报销金额
        dt.Columns[5].ColumnName = "hsDeptCode";//核算部门编号
        dt.Columns[6].ColumnName = "hsDeptName";//核算部门名称
        dt.Columns[7].ColumnName = "hsje";      //核算金额
        dt.Columns[8].ColumnName = "bxzy";      //报销摘要
        dt.Columns[9].ColumnName = "bxsm";      //报销说明

        //验证数据的合法性beg
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (!IsExist("dept", Convert.ToString(dt.Rows[i]["deptCode"])))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('第" + (i + 1) + "行部门编号" + dt.Rows[i]["deptCode"] + "不存在，导入失败！');", true);
                return;
            }
            if (!IsExist("fykm", Convert.ToString(dt.Rows[i]["fykmCode"])))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('第" + (i + 1) + "行费用科目" + dt.Rows[i]["fykmCode"] + "不不存在，导入失败！');", true);
                return;
            }

            if (IsNum(Convert.ToString(dt.Rows[i]["bxje"])) || IsNum(Convert.ToString(dt.Rows[i]["hsje"])))
            {
                continue;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('第" + (i + 1) + "行报销金额和核算金额都不能为空且必须是数字，导入失败！');", true);
                //return;
            }
        }
        //验证数据合法性 end
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        File.Delete(nowPath);
    }





    /// <summary>
    /// 判断输入的是否是数字
    /// </summary>
    /// <param name="num">要验证的字符串</param>
    /// <returns>有效 true 其他 返回false</returns>
    private bool IsNum(string num)
    {
        string msg = "";
        double result;
        if (!double.TryParse(num, out result))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 验证输入的部门和费用科目必须是有效的数据
    /// </summary>
    /// <param name="type">要验证的类型</param>
    /// <param name="code">主键</param>
    /// <returns></returns>
    private bool IsExist(string type, string code)
    {
        string sql = "";
        if (type == "dept")
        {
            sql = "select count(*) from bill_departments where deptCode='" + code + "'";
        }
        else if (type == "fykm")
        {
            sql = "select count(*) from bill_yskm where yskmCode='" + code + "'";
        }

        sql = server.GetCellValue(sql);
        if (Convert.ToUInt32(sql) > 0)
        {
            return true;
        }
        else
        {
            return false;
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
