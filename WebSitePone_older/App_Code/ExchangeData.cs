using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Reflection;


    /// <summary>
    /// 数据源格式
    /// </summary>
    public  class ExchangeData
    {
        /// <summary>
        /// DataTable解析成json
        /// </summary>
        public static string DataTableToJSON(DataTable dt)
        {

            if (dt == null)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sb.Append("{");
                for (int s = 0; s < dt.Columns.Count; s++)
                {
                    sb.Append(dt.Columns[s].ColumnName);
                    sb.Append(":'");
                    sb.Append(dt.Rows[i][s].ToString());
                    sb.Append("'");
                    if (s != dt.Columns.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("}");
                if (i != dt.Rows.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        /// <summary>
        /// Obj解析成json
        /// </summary>
        public string ObjectToJSON<T>(T data)
        {
            throw new Exception("未完成的方法");
            //try
            //{
            //    string jsoning = "";
            //    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(data.GetType());
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        serializer.WriteObject(ms, data);
            //        jsoning = Encoding.UTF8.GetString(ms.ToArray());
            //    }


            //    //替换Json的Date字符串    
            //    string p = @"///Date/((/d+)/+/d+/)///"; /*////Date/((([/+/-]/d+)|(/d+))[/+/-]/d+/)////*/
            //    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            //    Regex reg = new Regex(p);
            //    jsoning = reg.Replace(jsoning, matchEvaluator);
            //    return jsoning;

            //}
            //catch
            //{
            //    return null;
            //}
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// </summary>    
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>
        /// 将dataTable转化为集合
        /// </summary>
        /// <param name="dt">DataTable要转化的数据</param>
        /// <param name="colName">加入到集合的列名</param>
        /// <returns>字符串集合</returns>
        public static IList<string> DataTableToList(DataTable dt,string colName)
        {
            IList<string> ret=new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret.Add( Convert.ToString(dt.Rows[i][colName]));
            }
            return ret;
        }
    }
