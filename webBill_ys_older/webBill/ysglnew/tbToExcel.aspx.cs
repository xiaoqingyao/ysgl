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
using System.Collections.Generic;
using Models;
using System.Data.OleDb;

public partial class webBill_ysglnew_tbToExcel : System.Web.UI.Page
{
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    string stryskmtype = "02";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string deptcode = Request.QueryString["deptCode"].ToString();
            string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";  // 02是财务填报  ""是部门填报
            string nd = Request.QueryString["nd"].ToString();
            IList<YsgcTb> ysMainTable = new List<YsgcTb>();
            if (!string.IsNullOrEmpty(Request["yskmtype"]))
            {
                stryskmtype = Request["yskmtype"].ToString();
                ysMainTable = bll.GetMainTable(deptcode, nd, stryskmtype, tblx, new string[] { "1", "5" },"","");
            }
            else
            {
                ysMainTable = bll.GetMainTable(deptcode, nd,"" , tblx, new string[] { "1", "5" },"","");
            }


            //string deptcode = Request.QueryString["deptCode"].ToString();
            //string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";  // 02是财务填报  ""是部门填报
            //string nd = Request.QueryString["nd"].ToString();
            //IList<YsgcTb> ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx, new string[] { "1", "5" });
            //IList<YsgcTb> ysMainTable = new List<YsgcTb>();
           
            //临时文件    
            string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
            //使用OleDb连接  
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
            using (con)
            {
                con.Open();
                //创建Sheet   
                OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([科目编码] VarChar,[月份\\科目] VarChar,[1月份] VarChar,[2月份] VarChar,[3月份] VarChar,[4月份] VarChar,[5月份] VarChar,[6月份] VarChar,[7月份] VarChar,[8月份] VarChar,[9月份] VarChar,[10月份] VarChar,[11月份] VarChar,[12月份] VarChar,[年度总预算] VarChar)", con);
                cmdCreate.ExecuteNonQuery();
                //插入数据     
                for (int i = 0; i < ysMainTable.Count; i++)
                {
                    YsgcTb ysgc = ysMainTable[i];
                    OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@kmbh, @kmmc,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@year)", con);
                    cmd.Parameters.AddWithValue("@kmbh", ysgc.kmbh);
                    cmd.Parameters.AddWithValue("@kmmc", ysgc.km);
                    cmd.Parameters.AddWithValue("@1", ysgc.January == null ? "0" : ysgc.January);
                    cmd.Parameters.AddWithValue("@2", ysgc.February == null ? "0" : ysgc.February);
                    cmd.Parameters.AddWithValue("@3", ysgc.march == null ? "0" : ysgc.march);
                    cmd.Parameters.AddWithValue("@4", ysgc.April == null ? "0" : ysgc.April);
                    cmd.Parameters.AddWithValue("@5", ysgc.May == null ? "0" : ysgc.May);
                    cmd.Parameters.AddWithValue("@6", ysgc.June == null ? "0" : ysgc.June);
                    cmd.Parameters.AddWithValue("@7", ysgc.July == null ? "0" : ysgc.July);
                    cmd.Parameters.AddWithValue("@8", ysgc.August == null ? "0" : ysgc.August);
                    cmd.Parameters.AddWithValue("@9", ysgc.September == null ? "0" : ysgc.September);
                    cmd.Parameters.AddWithValue("@10", ysgc.October == null ? "0" : ysgc.October);
                    cmd.Parameters.AddWithValue("@11", ysgc.November == null ? "0" : ysgc.November);
                    cmd.Parameters.AddWithValue("@12", ysgc.December == null ? "0" : ysgc.December);
                    cmd.Parameters.AddWithValue("@year", ysgc.year == null ? "0" : ysgc.year);
                    cmd.ExecuteNonQuery();
                }
            }
            Response.ContentType = "application/ms-excel";
            Response.AppendHeader("Content-Disposition", "attachment;filename=info.xls");
            Response.BinaryWrite(File.ReadAllBytes(tempFile));
            Response.End();
            File.Delete(tempFile);
            //this.GridView1_toExcel.DataSource = ysMainTable;
            //this.GridView1_toExcel.DataBind();
        }
    }
    protected void btn_ExportExcel_Click(object sender, EventArgs e)
    {
        string deptcode = Request.QueryString["deptCode"].ToString();
        string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";  // 02是财务填报  ""是部门填报
        string nd = Request.QueryString["nd"].ToString();
        //IList<YsgcTb> ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx, new string[] { "1", "5" });
        IList<YsgcTb> ysMainTable = new List<YsgcTb>();
        if (!string.IsNullOrEmpty(Request["yskmtype"]))
        {
            stryskmtype = Request["yskmtype"].ToString();
            ysMainTable = bll.GetMainTable(deptcode, nd, stryskmtype, tblx, new string[] { "1", "5" }, "", "");
        }
        else
        {
            ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx, new string[] { "1", "5" }, "", "");
        }
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([科目编码] VarChar,[月份\\科目] VarChar,[1月份] VarChar,[2月份] VarChar,[3月份] VarChar,[4月份] VarChar,[5月份] VarChar,[6月份] VarChar,[7月份] VarChar,[8月份] VarChar,[9月份] VarChar,[10月份] VarChar,[11月份] VarChar,[12月份] VarChar,[年度总预算] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < ysMainTable.Count; i++)
            {
                YsgcTb ysgc = ysMainTable[i];
                OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@kmbh, @kmmc,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@year)", con);
                cmd.Parameters.AddWithValue("@kmbh", ysgc.kmbh);
                cmd.Parameters.AddWithValue("@kmmc", ysgc.km);
                cmd.Parameters.AddWithValue("@1", ysgc.January==null ? "0" : ysgc.January);
                cmd.Parameters.AddWithValue("@2", ysgc.February==null ? "0" : ysgc.February);
                cmd.Parameters.AddWithValue("@3", ysgc.march==null ? "0" : ysgc.march);
                cmd.Parameters.AddWithValue("@4", ysgc.April==null ? "0" : ysgc.April);
                cmd.Parameters.AddWithValue("@5", ysgc.May==null ? "0" : ysgc.May);
                cmd.Parameters.AddWithValue("@6", ysgc.June==null ? "0" : ysgc.June);
                cmd.Parameters.AddWithValue("@7", ysgc.July==null ? "0" : ysgc.July);
                cmd.Parameters.AddWithValue("@8", ysgc.August==null ? "0" : ysgc.August);
                cmd.Parameters.AddWithValue("@9", ysgc.September==null ? "0" : ysgc.September);
                cmd.Parameters.AddWithValue("@10", ysgc.October==null ? "0" : ysgc.October);
                cmd.Parameters.AddWithValue("@11", ysgc.November==null ? "0" : ysgc.November);
                cmd.Parameters.AddWithValue("@12", ysgc.December==null ? "0" : ysgc.December);
                cmd.Parameters.AddWithValue("@year", ysgc.year==null ? "0" : ysgc.year);
                cmd.ExecuteNonQuery();
            }
        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=info.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }
    //public override void VerifyRenderingInServerForm(Control control)
    //{

    //}
    //private void export2()
    //{
    //    Response.ClearContent();

    //    Response.Charset = "utf-8";
    //    Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");

    //    Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

    //    Response.ContentType = "application/ms-excel";

    //    StringWriter sw = new StringWriter();

    //    HtmlTextWriter htw = new HtmlTextWriter(sw);

    //    GridView1_toExcel.RenderControl(htw);

    //    Response.Write(sw.ToString());

    //    Response.End();
    //}
    private void ExportExcel()
    {
        string deptcode = Request.QueryString["deptCode"].ToString();
        string tblx = Request.QueryString["type"].ToString() == "ystb" ? "" : "02";  // 02是财务填报  ""是部门填报
        string nd = Request.QueryString["nd"].ToString();
        IList<YsgcTb> ysMainTable = new List<YsgcTb>();
        if (!string.IsNullOrEmpty(Request["yskmtype"]))
        {
            stryskmtype = Request["yskmtype"].ToString();
            ysMainTable = bll.GetMainTable(deptcode, nd, stryskmtype, tblx, new string[] { "1", "5" }, "", "");
        }
        else
        {
            ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx, new string[] { "1", "5" }, "", "");
        }
   //     IList<YsgcTb> ysMainTable = bll.GetMainTable(deptcode, nd, "", tblx, new string[] { "1", "5" });

        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([科目编码] VarChar,[月份\\科目] VarChar,[1月份] VarChar,[2月份] VarChar,[3月份] VarChar,[4月份] VarChar,[5月份] VarChar,[6月份] VarChar,[7月份] VarChar,[8月份] VarChar,[9月份] VarChar,[10月份] VarChar,[11月份] VarChar,[12月份] VarChar,[年度总预算] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < ysMainTable.Count; i++)
            {
                YsgcTb ysgc = ysMainTable[i];
                OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@kmbh, @kmmc,@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@year)", con);
                cmd.Parameters.AddWithValue("@kmbh", ysgc.kmbh);
                cmd.Parameters.AddWithValue("@kmmc", ysgc.km);
                cmd.Parameters.AddWithValue("@1", ysgc.January);
                cmd.Parameters.AddWithValue("@2", ysgc.February);
                cmd.Parameters.AddWithValue("@3", ysgc.march);
                cmd.Parameters.AddWithValue("@4", ysgc.April);
                cmd.Parameters.AddWithValue("@5", ysgc.May);
                cmd.Parameters.AddWithValue("@6", ysgc.June);
                cmd.Parameters.AddWithValue("@7", ysgc.July);
                cmd.Parameters.AddWithValue("@8", ysgc.August);
                cmd.Parameters.AddWithValue("@9", ysgc.September);
                cmd.Parameters.AddWithValue("@10", ysgc.October);
                cmd.Parameters.AddWithValue("@11", ysgc.November);
                cmd.Parameters.AddWithValue("@12", ysgc.December);
                cmd.Parameters.AddWithValue("@year", ysgc.year);
                cmd.ExecuteNonQuery();
            }
           
        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=info.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }

}
