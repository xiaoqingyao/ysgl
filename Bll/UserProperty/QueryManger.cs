using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Query;
using System.Data;
using System.Data.SqlClient;

namespace Bll.UserProperty
{
    public class QueryManger
    {
        QueryDal qd = new QueryDal();

        /// <summary>
        /// 归口费用使用部门统计分析
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <param name="kmCode">统计科目</param>
        /// <returns>返回DataTable,返回列数不定</returns>
        public DataTable SearchSybmReport(DateTime kssj, DateTime jssj, string deptCode, string kmCode)
        {
            DataTable ds = qd.SearchSybmReport(kssj, jssj, deptCode, kmCode);
            return ds;
        }
        /// <summary>
        /// 归口费用使用部门统计分析明细
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <param name="kmCode">统计科目</param>
        /// <returns>返回DataTable</returns>
        public DataTable SearchSybmReportMx(DateTime kssj, DateTime jssj, string deptcode, string kmcode)
        {
            DataTable ds = qd.SearchSybmReportMx(kssj, jssj, deptcode, kmcode);
            return ds;
        }

        /// <summary>
        /// 项目费用报销统计分析
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <returns>返回DataTable</returns>
        public DataTable SearchSyxmReport(DateTime kssj, DateTime jssj, string deptCode)
        {
            DataTable ds = qd.SearchSyxmReport(kssj, jssj, deptCode);
            return ds;
        }
        /// <summary>
        /// 项目费用报销统计分析明细
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <param name="kmCode">统计项目</param>
        /// <returns>返回DataTable</returns>
        public DataTable SearchSyxmReportMx(DateTime kssj, DateTime jssj, string deptcode, string xmcode, string cxlx)
        {
            DataTable ds = qd.SearchSyxmReportMx(kssj, jssj, deptcode, xmcode, cxlx);
            return ds;
        }

        /// <summary>
        /// 月结
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="userCode">用户编号</param>
        /// <param name="guid">guid</param>
        public void ProYj(string month, string userCode, string guid)
        {
            qd.ProYj(month, userCode, guid);
        }

        /// <summary>
        /// 检查是否能够月结，可以返回""
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string CheckYj(string month)
        {
            return qd.CheckYj(month);
        }
        /// <summary>
        /// 取消月结
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string qxyj(string month)
        {
            return qd.qxyj(month);
        }
        /// <summary>
        /// 获得月结表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYsTable(string stryf)
        {
            return qd.GetYsTable(stryf);
        }
    }
}
