using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text;

/// <summary>
///PubMethod 的摘要说明
/// </summary>
public class PubMethod
{
    public PubMethod()
    {
    }

    /// <summary>
    /// 根据参数获取dataTable 并返回页面导航字符串
    /// </summary>
    /// <param name="sql">获取数据的sql</param>
    /// <param name="parms">sql参数（方便参数化查询）</param>
    /// <param name="url">url</param>
    /// <param name="pageNav">输出参数 页码导航</param>
    /// <returns>请求页面的数据，并返回页面导航</returns>
    public static DataTable GetPageData(string sql, string url, out string pageNav)
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        string pageStr = HttpContext.Current.Request["page"];
        int pageIndex = 0;
        if (!string.IsNullOrEmpty(pageStr))
        {
            pageIndex = Convert.ToInt32(pageStr);
        }

        DataTable dt;
        int count = 0;


        if (string.IsNullOrEmpty(sql))
        {
            pageNav = "";
            return null;
        }

        string conSql = "select count(*) from (" + sql + ") as t";
        string dtSql = "select * from ({0}) as t where t.crow>{1} and t.crow <={2}";

        dtSql = string.Format(dtSql, sql, pageIndex * 10, (pageIndex + 1) * 10);
        count = Convert.ToInt32(server.GetCellValue(conSql));
        dt = server.GetDataTable(dtSql, null);




        StringBuilder sb = new StringBuilder();
        int pageCount = count % 10 == 0 ? (count / 10) : (count / 10 + 1);
        int prevNum = pageIndex - 1;
        int nextNum = pageIndex + 1;
        sb.Append("<p id='page'>");
        if (prevNum >= 0)
        {
            if (url.IndexOf("?") == -1)
            {
                sb.Append("<a href='" + url + "?page=" + prevNum + "' class='n'>&lt;上一页</a>");
            }
            else
            {
                sb.Append("<a href='" + url + "&page=" + prevNum + "' class='n'>&lt;上一页</a>");
            }
        }
        if (pageCount > 1)
        {

            if (pageCount <= 7)
            {
                for (int i = 0; i < pageCount; i++)
                {
                    sb.Append("<a href='" + url + "'> <span class='pc'>" + (i + 1) + "</span></a> ");
                    if (i == pageIndex)
                    {
                        sb.Append("<strong><span class='pc'>" + (i + 1) + "</span></strong> ");
                    }
                }
            }
            else
            {
                if (pageIndex+1 < 5)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sb.Append("<a href='" + url + "'> <span class='pc'>" + (i + 1) + "</span></a> ");
                        if (i == pageIndex)
                        {
                            sb.Append("<strong><span class='pc'>" + (i + 1) + "</span></strong> ");
                        }
                    }
                    sb.Append("…<a href='" + url + "'> <span class='pc'>" + pageCount + "</span></a> ");
                }
                else if (pageIndex+1 > pageCount - 4)
                {
                    sb.Append("<a href='" + url + "'> <span class='pc'>" + 1 + "</span></a> …");
                    for (int i = pageCount - 5; i < pageCount; i++)
                    {
                        sb.Append("<a href='" + url + "'> <span class='pc'>" + (i + 1) + "</span></a> ");
                        if (i == pageIndex)
                        {
                            sb.Append("<strong><span class='pc'>" + (i + 1) + "</span></strong> ");
                        }
                    }
                }
                else
                {
                    sb.Append("<a href='" + url + "'> <span class='pc'>" + 1 + "</span></a> …");
                    for (int i = pageIndex - 2; i <= pageIndex + 2; i++)
                    {
                        sb.Append("<a href='" + url + "'> <span class='pc'>" + (i + 1) + "</span></a> ");
                        if (pageIndex == i)
                        {
                            sb.Append("<strong><span class='pc'>" + (i + 1) + "</span></strong> ");
                        }
                    }
                    sb.Append("…<a href='" + url + "'> <span class='pc'>" + pageCount + "</span></a> ");
                }

            }
        }

        if (nextNum <= pageCount - 1)
        {
            if (url.IndexOf("?") == -1)
            {
                sb.Append("<a href='" + url + "?page=" + nextNum + "' class='n'>下一页&gt;</a>");
            }
            else
            {
                sb.Append("<a href='" + url + "&page=" + nextNum + "' class='n'>下一页&gt;</a>");
            }
        }

        sb.Append("<span class='nums'>共" + count + "条</span>");
        sb.Append("</p>");
        pageNav = sb.ToString();
        return dt;

    }

    public static string SubString(string longStr)
    {

        try
        {
            string result = "";
            if (!string.IsNullOrEmpty(longStr) && longStr.Length > 1 && longStr.IndexOf("[") != -1 && longStr.IndexOf("]") != -1)
            {
                int i = longStr.LastIndexOf("]");
                result = longStr.Substring(1, i - 1);
            }
            else
            {
                result = longStr;
            }
            return result;
        }
        catch (Exception e)
        {

            throw e;
        }
    }



    /// <summary>
    /// 执行DataTable中的查询返回新的DataTable
    /// </summary>
    /// <param name="dt">源数据DataTable</param>
    /// <param name="condition">查询条件</param>
    /// <returns></returns>
    public static DataTable GetNewDataTable(DataTable dt, string condition)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone();
        DataRow[] dr = dt.Select(condition);
        for (int i = 0; i < dr.Length; i++)
        {
            newdt.ImportRow((DataRow)dr[i]);
        }
        return newdt;//返回的查询结果
    }

    /// <summary>
    /// 执行DataTable中的查询返回新的DataTable
    /// </summary>
    /// <param name="dt">源数据DataTable</param>
    /// <param name="condition">查询条件</param>
    /// <returns></returns>
    public static DataTable GetNewDataTable2(DataTable dt, string condition)
    {
        DataTable newdt = new DataTable();
        newdt = dt.Clone(); // 克隆dt 的结构，包括所有 dt 架构和约束,并无数据； 
        DataRow[] rows = dt.Select(condition); // 从dt 中查询符合条件的记录； 
        foreach (DataRow row in rows)  // 将查询的结果添加到dt中； 
        {
            newdt.Rows.Add(row.ItemArray);
        }
        return newdt;
    }

}
