using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.SysDictionary;
using System.Data;
using System.Xml;
using System.IO;

namespace Bll
{
    /// <summary>
    /// 公共服务类
    /// Edit by Lvcc
    /// </summary>
    public class PublicServiceBLL
    {
        /// <summary>
        /// 将code[]中的值取出
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string SubCode(string code)
        {
            if (code == "")
            {
                return "";
            }
            else
            {
                try
                {
                    return code.Substring(1, code.IndexOf("]") - 1);
                }
                catch
                {
                    return code;
                }
            }
        }
        /// <summary>
        /// 如果时间精确到时分秒 将其装换成精确到日的
        /// </summary>
        /// <param name="strDt"></param>
        /// <returns></returns>
        public string cutDt(string strDt)
        {
            if (string.IsNullOrEmpty(strDt))
            {
                return "";
            }
            if (strDt.Length < 7)
            {
                return strDt;
            }
            else
            {
                return Convert.ToDateTime(strDt).ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 将时间装换成日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string cutDt(DateTime dt) {
            if (dt==null)
            {
                return "";
            }
            return Convert.ToDateTime(dt).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取数据表code
        /// </summary>
        /// <param name="card">单据号头</param>
        /// <param name="seed">单据日期20120101</param>
        /// <param name="type">类型0是读取1是修改</param>
        /// <param name="lshws">单号长度</param>
        /// <returns></returns>
        public string GetBillCode(string card, string seed, int type, int lshws)
        {
            DataDicDal dal = new DataDicDal();
            return dal.GetYbbxBillName(card, seed, type, lshws);
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

        /// <summary>
        /// 将xml字符串转为dataset
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
    }
}
