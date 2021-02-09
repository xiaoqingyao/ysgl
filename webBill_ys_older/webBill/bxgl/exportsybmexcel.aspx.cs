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
using System.IO;
using System.Data.OleDb;

public partial class webBill_bxgl_exportsybmexcel : System.Web.UI.Page
{
    string strkmcode = "";//科目编号
    protected void Page_Load(object sender, EventArgs e)
    {
        object objkmcode = Request["kmcode"];
        if (objkmcode != null)
        {
            strkmcode = objkmcode.ToString();
            strkmcode = strkmcode.Substring(1, strkmcode.IndexOf("]") - 1);
        }
        else { showMessage("参数不完整。", true, ""); return; }
        export();
    }

    private void export()
    {
        //select depart.deptcode,depart.deptname,(select '['+yskmbm+']'+yskmmc from bill_yskm where yskmbm=yskmdept.yskmcode) as yskmmc from bill_departments depart inner join bill_yskm_dept yskmdept on depart.deptcode=yskmdept.deptcode where yskmcode=@yskmcode
        DataTable dtRel = new sqlHelper.sqlHelper().GetDataTable("select depart.deptcode,depart.deptname,(select '['+yskmbm+']'+yskmmc from bill_yskm where yskmCode=@yskmcode) as yskmmc from bill_departments depart where isnull(deptStatus,'')='1' ", new System.Data.SqlClient.SqlParameter[] { new System.Data.SqlClient.SqlParameter("@yskmcode", strkmcode) });
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());

        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([费用名称] VarChar,[部门编号] VarChar,[部门名称] VarChar,[报销金额] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                DataRow dr = dtRel.Rows[i];
                string stryskmmc = dr["yskmmc"].ToString();
                string strdeptcode = dr["deptcode"].ToString();
                string strdeptname = dr["deptname"].ToString();

                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@yskmmc,@deptcode,@deptname,@je)", con))
                {
                    cmd.Parameters.AddWithValue("@yskmmc", stryskmmc);
                    cmd.Parameters.AddWithValue("@deptcode", strdeptcode);
                    cmd.Parameters.AddWithValue("@deptname", strdeptname);
                    cmd.Parameters.AddWithValue("@je", "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        if (con.State == ConnectionState.Closed)
        {
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=sybm.xls");
            Response.BinaryWrite(File.ReadAllBytes(tempFile));
            Response.End();
            File.Delete(tempFile);
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
