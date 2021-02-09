using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_ExportYskmExcel : System.Web.UI.Page
{
    string strdept = "";//对应部门
    string strdydj = "";//对应单据
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["dept"]) && Request["dept"].ToString() != "")
        {
            strdept = Request["dept"].ToString();
        }
        else
        {
            showMessage("参数不完整。", true, ""); return;
        }
        if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"].ToString() != "")
        {
            strdydj = Request["dydj"].ToString();
        }
        export();
    }

    private void export()
    {
        string strsql = "";
        if (!string.IsNullOrEmpty(strdept))
        {
            strsql = "select yskmcode,yskmmc from bill_yskm b where yskmcode in( select yskmcode from bill_yskm_dept where deptcode='" + strdept + "')";
        }
        if (!string.IsNullOrEmpty(strdydj))
        {
            strsql += "  and dydj='" + strdydj + "'";
        }
        strsql += "  and ( select count(*) from bill_yskm where yskmcode like b.yskmcode+'%' and len(yskmcode)>len(b.yskmcode) ) =0";
        DataTable dtRel = new sqlHelper.sqlHelper().GetDataTable(strsql, null);

        //临时文件

        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");

        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([部门编号] VarChar,[部门名称] VarChar,[费用科目编号] VarChar,[费用科目名称] VarChar,[决算金额] VarChar,[核算部门编号] VarChar,[核算部门名称] VarChar ,[核算金额] VarChar,[报销摘要] VarChar,[报销说明] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                DataRow dr = dtRel.Rows[i];
                string stryskmcode = dr["yskmcode"].ToString();
                string stryskmmc = dr["yskmmc"].ToString();
                string strdeptcode = strdept;

                string strdeptname = server.GetCellValue("select deptname from bill_departments where deptcode='" + strdeptcode + "'"); ;

                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@deptcode,@deptname,@yskmcode,@yskmmc,@bxje,@hsdeptcode,@hsdeptname,@hsje,@bxzy,@bxsm)", con))
                {
                    cmd.Parameters.AddWithValue("@deptcode", strdept);
                    cmd.Parameters.AddWithValue("@deptname", strdeptname);
                    cmd.Parameters.AddWithValue("@yskmcode", stryskmcode);
                    cmd.Parameters.AddWithValue("@yskmmc", stryskmmc);
                    cmd.Parameters.AddWithValue("@bxje", "");
                    cmd.Parameters.AddWithValue("@hsdeptcode", strdeptcode);
                    cmd.Parameters.AddWithValue("@hsdeptname", strdeptname);
                    cmd.Parameters.AddWithValue("@bxzy", "");
                    cmd.Parameters.AddWithValue("@bxsm", "");
                    cmd.Parameters.AddWithValue("@hsje", "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        if (con.State == ConnectionState.Closed)
        {
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=srbgd.xls");
            Response.BinaryWrite(File.ReadAllBytes(tempFile));
            Response.End();
            File.Delete(tempFile);
        }

    } /// <summary>
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