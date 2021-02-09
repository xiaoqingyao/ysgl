using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.SysDictionary
{
    public class DataDicDal
    {
        public IList<Bill_DataDic> GetDicByType(string typecode)
        {
            string sql = " select * from bill_DataDic where dicType=@typeCode ";
            SqlParameter[] sps = { new SqlParameter("@typeCode", typecode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps,false);

            IList<Bill_DataDic> list = new List<Bill_DataDic>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_DataDic dic = new Bill_DataDic();
                dic.Cjys = Convert.ToString(dr["Cjys"]);
                dic.Cys = Convert.ToString(dr["Cys"]);
                dic.DicCode = Convert.ToString(dr["DicCode"]);
                dic.DicName = Convert.ToString(dr["DicName"]);
                dic.DicType = Convert.ToString(dr["DicType"]);
                dic.Cdj = Convert.ToString(dt.Rows[0]["Cdj"]);
                list.Add(dic);
            }
            return list;
        }

        public Bill_DataDic GetDicByTypeCode(string typecode,string code)
        {
            string sql = " select * from bill_DataDic where dicType='" + typecode + "' and diccode='" + code + "'";

            DataTable dt = DataHelper.GetDataTable(sql, null, false);

            if (dt.Rows.Count > 0)
            {
                Bill_DataDic dic = new Bill_DataDic();
                dic.Cjys = Convert.ToString(dt.Rows[0]["Cjys"]);
                dic.Cys = Convert.ToString(dt.Rows[0]["Cys"]);
                dic.DicCode = Convert.ToString(dt.Rows[0]["DicCode"]);
                dic.DicName = Convert.ToString(dt.Rows[0]["DicName"]);
                dic.DicType = Convert.ToString(dt.Rows[0]["DicType"]);
                dic.Cdj = Convert.ToString(dt.Rows[0]["Cdj"]);
                return dic;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得单据编号
        /// </summary>
        /// <param name="card">单据号头</param>
        /// <param name="seed">单据日期20120101</param>
        /// <param name="type">类型0是读取1是修改</param>
        /// <returns></returns>
        public string GetYbbxBillName(string card,string seed,int type)
        {
            string sql = "pro_cardnumber";
            SqlParameter[] sps = { 
                                     new SqlParameter("@card",card),
                                     new SqlParameter("@seed",seed),
                                     new SqlParameter("@type",type),
                                     new SqlParameter("@lshws",3)
                                 };
            return Convert.ToString(DataHelper.ExecuteScalar(sql, sps, true));
        }
        /// <summary>
        /// 获得单据编号
        /// </summary>
        /// <param name="card">单据号头</param>
        /// <param name="seed">单据日期20120101</param>
        /// <param name="type">类型0是读取1是修改</param>
        ///  <param name="lshws">长度</param>
        /// <returns></returns>
        public string GetYbbxBillName(string card, string seed, int type, int lshws)
        {
            string sql = "pro_cardnumber";
            SqlParameter[] sps = { 
                                     new SqlParameter("@card",card),
                                     new SqlParameter("@seed",seed),
                                     new SqlParameter("@type",type),
                                     new SqlParameter("@lshws",lshws)
                                 };
            return Convert.ToString(DataHelper.ExecuteScalar(sql, sps, true));
        }
    }
}
