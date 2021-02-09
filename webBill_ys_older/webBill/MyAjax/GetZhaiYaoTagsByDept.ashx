<%@ WebHandler Language="C#" Class="GetZhaiYaoTagsByDept" %>

using System;
using System.Web;

public class GetZhaiYaoTagsByDept : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        string strcode = context.Request["deptcode"];
        System.Data.DataSet ds = server.GetDataSet("select dicname from bill_datadic where dictype='01'  ");
        System.Text.StringBuilder arry = new System.Text.StringBuilder();
        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        context.Response.ContentType = "text/plain";
        context.Response.Write(script);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}