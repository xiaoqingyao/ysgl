using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

/// <summary>
///ystzServiece 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]

public class ystzServiece : System.Web.Services.WebService
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public ystzServiece()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string GetYsbl()
    {
        DataSet ds = server.GetDataSet("select * from bill_ysbl");
        if (ds.Tables[0].Rows.Count <= 0)
        {
            StringBuilder insert = new StringBuilder();

            for (int i = 1; i <= 12; i++)
            {
                insert.Append(" insert into bill_ysbl(yf,bl) values ");
                insert.Append(" (");
                insert.Append("'" + Convert.ToString(i) + "',");
                insert.Append("0");
                insert.Append(") ");
            }
            server.ExecuteNonQuery(insert.ToString());
        }
        
        ds = server.GetDataSet("select * from bill_ysbl order by yf");
        try
        {
            return Serialize(ds.Tables[0]);
        }
        catch(Exception e)
        {
            return e.Message;
        }
    }

    [WebMethod]
    public int UpdateYYsje(string gcbh, string bmbh, string kmbh, decimal je)
    {
        try
        {
            SqlParameter[] sps = {
                                 new SqlParameter("@cllb","1"),
                                 new SqlParameter("@gcbh",gcbh),
                                 new SqlParameter("@bmbh",bmbh),
                                 new SqlParameter("@yskm",kmbh),
                                 new SqlParameter("@xgje",je),
                             };
            server.ExecuteProc("bill_pro_ystz", sps);
            return 1;
        }
        catch
        {
            return -1;
        }
    }

    [WebMethod]
    public int UpdateYsbl(string json,string bmbh,string kmbh,decimal je)
    {
        string[] monthArray = json.Split('|');

        decimal test=0;

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (string mon in monthArray)
        {
            string[]temp= mon.Split(',');
            string yf = temp[0].Split(':')[1];
            decimal bl = Convert.ToDecimal(temp[1].Split(':')[1]);
            test += bl ;
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add(yf, bl);
            list.Add(result);
        }
        if (test != 1)
        {
            return -1;
        }

        StringBuilder sql = new StringBuilder();
        sql.Append("delete bill_ysbl");
        foreach (Dictionary<string, object> dic in list)
        {
            sql.Append(" insert into bill_ysbl(yf,bl) values ");
            sql.Append(" (");
            foreach(string s in dic.Keys)
            {
                sql.Append("'" + s + "'," + dic[s]);
            }
            sql.Append(") ");
        }
        server.ExecuteNonQuery(sql.ToString());

        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        SqlParameter[] sps = {
                                 new SqlParameter("@cllb","0"),
                                 new SqlParameter("@gcbh",date),
                                 new SqlParameter("@bmbh",bmbh),
                                 new SqlParameter("@yskm",kmbh),
                                 new SqlParameter("@xgje",je),
                             };
        server.ExecuteProc("bill_pro_ystz", sps);
        return 1;
    }

    private static string Serialize(DataTable dt)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DataRow dr in dt.Rows)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                result.Add(dc.ColumnName, dr[dc].ToString());
            }
            list.Add(result);
        }
        JavaScriptSerializer seria = new JavaScriptSerializer();
        string json = seria.Serialize(list);
        return json;
    }



}

