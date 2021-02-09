using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bll.SaleBill
{
    /// <summary>
    /// 视图的bll
    /// </summary>
    public class ViewBLL
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        /// <summary>
        /// 根据车架号获取车辆类型
        /// </summary>
        /// <returns></returns>
        public string GetTruckTypeByCode(string StrCarcode)
        {
            string strsql = @"select top 1 cxh as cartypename  from V_TruckMsg where cjh='" + StrCarcode + "'";
            object obj = server.ExecuteScalar(strsql);
            return obj == null ? "" : obj.ToString();
            //DataSet ds = server.GetDataSet(strsql);
            //StringBuilder arry = new StringBuilder();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{
            //    arry.Append("'");
            //    arry.Append(Convert.ToString(dr["cartypename"]));
            //    arry.Append("',");
            //}
            //if (arry.Length > 1)
            //{
            //    string script = arry.ToString().Substring(0, arry.Length - 1);
            //    return script;
            //}
            //else
            //{
            //    return "";
            //}
        }
        public DataTable getDt(string strCarcode) 
        {
            string strsql = @"select * from V_TruckMsg where cjh='" + strCarcode + "'";
            return server.RunQueryCmdToTable(strsql);
        }
        /// <summary>
        /// 通过订单号获取dt
        /// </summary>
        /// <returns></returns>
        public DataTable GetDtBySaleCode(string strSaleCode) {
            string strSql = @"select * from V_TruckMsg where ddh='" + strSaleCode + "'";
            return server.RunQueryCmdToTable(strSql);
        }
    }

}
