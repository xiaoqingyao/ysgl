using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Web.UI;
using System.Collections.Specialized;
using Dal;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public BasePage()
    {

        //
        //TODO: 在此处添加构造函数逻辑
        //
        PageInputValidate();
    }
    /// <summary>
    /// 用这个
    /// </summary>
    /// <param name="pageIndex">页序号</param>
    /// <param name="iEveCount">每页显示</param>
    /// <returns></returns>
    protected int[] ComputeRow(int pageIndex, int iEveCount)
    {
        int[] ret = new int[2];
        int pagRows = Convert.ToInt32(iEveCount);
        if (pageIndex <= 0)
        {
            pageIndex = 1;
        }
        ret[0] = (pageIndex - 1) * pagRows;
        ret[1] = pageIndex * pagRows;
        return ret;
    }

    
    
    /// <summary>
    /// 获得单据编号
    /// </summary>
    /// <param name="card">单据号头</param>
    /// <param name="seed">单据日期20120101</param>
    ///// <param name="type">类型0是读取1是修改</param>
    ///  <param name="lshws">长度</param>
    /// <returns></returns>
    public string GetBillCode(string card, string seed, int lshws)
    {
        if (seed == "")
        {
            seed = DateTime.Now.ToString("yyyyMMdd");
        }

        string sql = "pro_cardnumber";
        SqlParameter[] sps = { 
                                     new SqlParameter("@card",card),
                                     new SqlParameter("@seed",seed),
                                     new SqlParameter("@type",1),
                                     new SqlParameter("@lshws",lshws)
                                 };
        return Convert.ToString(Dal.DataHelper.ExecuteScalar(sql, sps, true));
    }

    
    /// <summary>
    /// 生成单据编号
    /// </summary>
    /// <param name="card">编号前缀</param>
    /// <param name="lshws">编号长度</param>
    /// <returns></returns>
    public string GetBillCode(string card, int lshws)
    {

        string seed = DateTime.Now.ToString("yyyyMMdd");
        string sql = "pro_cardnumber";
        SqlParameter[] sps = { 
                                     new SqlParameter("card",SqlDbType.VarChar),
                                     new SqlParameter("seed",SqlDbType.VarChar),
                                     new SqlParameter("lshws",SqlDbType.Int),
                                     new SqlParameter("rel",SqlDbType.VarChar,1000)
                                 };
        sps[0].Value = card;
        sps[1].Value = seed;
        sps[2].Value = lshws;
        sps[3].Direction = ParameterDirection.Output;
        Dal.DataHelper.ExcuteNonQuery(sql, sps, true);
        return Convert.ToString(sps[3].Value.ToString());
    }
    public string ClearNbsp(string str)
    {
        return str.Replace("&nbsp;", "");
    }

    
   

    /// <summary>
    /// 附件下载方法
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="path">文件路径</param>
    public void downLoad(string fileName, string path)
    {
        try
        {
            Response.BufferOutput = false;
            Response.Clear();
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));//防止中文名出现乱码
            Response.ContentType = "application/octstream";
            Response.CacheControl = "Private";
            Stream stm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            Response.AppendHeader("Content-length", stm.Length.ToString());
            BinaryReader br = new BinaryReader(stm);
            byte[] bytes;
            for (Int64 x = 0; x < (br.BaseStream.Length / 4096 + 1); x++)
            {
                bytes = br.ReadBytes(4096);
                Response.BinaryWrite(bytes);
                System.Threading.Thread.Sleep(5); //休息一下,防止耗用带宽太多。
            }
            stm.Close();
        }
        catch (Exception)
        {

            throw new Exception("下载失败！");
        }

    }

    //TEST测试
    //string PATH = Server.HtmlEncode(Request.PhysicalApplicationPath) + fjLink;//获取网站跟目录+文件路径
    //downLoad(fjExplain,PATH);//fjExplain,fjLink变量分别是文件名和路径

    /// <summary>
    /// 附件下载方法
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="path">文件路径</param>
    public void downLoad(string fileName, string path, HttpContext context)
    {
        try
        {
            //context.Response.Write("<script>" + path + "</script>");
            context.Response.BufferOutput = false;
            context.Response.Clear();
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));//防止中文名出现乱码
            context.Response.ContentType = "application/octstream";
            context.Response.CacheControl = "Private";
            Stream stm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            context.Response.AppendHeader("Content-length", stm.Length.ToString());
            BinaryReader br = new BinaryReader(stm);
            byte[] bytes;
            for (Int64 x = 0; x < (br.BaseStream.Length / 4096 + 1); x++)
            {
                bytes = br.ReadBytes(4096);
                context.Response.BinaryWrite(bytes);
                System.Threading.Thread.Sleep(5); //休息一下,防止耗用带宽太多。
            }
            stm.Close();
        }
        catch (Exception)
        {
            throw new Exception("下载失败！");


        }
        finally
        {
            context.Response.End();
        }
    }


    public string SubSting(string longStr)
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


    //字符验证
    public void PageInputValidate()
    {
        //try
        //{
        //    // NameValueCollection parms = HttpContext.Current.Request.Params;

        //    var parms = HttpContext.Current.Request.Params.AllKeys;
        //    string[] validateParms = { "'", "\"", "%" };


        //    foreach (var i in parms)
        //    {
        //        if (validateParms.Where(p => p.IndexOf(Request.Params[i]) > 0).Count() > 0)
        //        {
        //            Response.Write("<script>alert('您输入的有恶意字符，请核实并修改后再提交！');</script>");
        //        }
        //    }

        //}
        //catch (Exception e)
        //{

        //    throw e;
        //}

    }

    


  
}
