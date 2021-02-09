using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Dal.Query
{
    public class QueryDal
    {
        /// <summary>
        /// 归口费用使用部门统计分析
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <param name="kmCode">统计科目</param>
        /// <returns>返回DataTable,返回列数不定</returns>
        public DataTable SearchSybmReport(DateTime kssj,DateTime jssj,string deptCode,string kmCode)
        {
            string sql = "pro_bill_sytj_sybm";
            SqlParameter[] sps = {
                                     new SqlParameter("@kssj",kssj),
                                     new SqlParameter("@jzsj",jssj),
                                     new SqlParameter("@deptCode",deptCode),
                                     new SqlParameter("@fykmcode",kmCode),
                                 };
            return DataHelper.GetDataTable(sql, sps, true);
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
            string sql = "pro_bill_sytj_sybm_mx";
            SqlParameter[] sps = {
                                     new SqlParameter("@kssj",kssj),
                                     new SqlParameter("@jzsj",jssj),
                                     new SqlParameter("@deptCode",deptcode),
                                     new SqlParameter("@fykmcode",kmcode),
                                 };
            return DataHelper.GetDataTable(sql, sps, true);
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
            string sql = "pro_bill_xmtj_ys_bx";
            SqlParameter[] sps = {
                                     new SqlParameter("@kssj",kssj),
                                     new SqlParameter("@jzsj",jssj),
                                     new SqlParameter("@deptCode",deptCode)
                                 };
            return DataHelper.GetDataTable(sql, sps, true);
        }

        /// <summary>
        /// 项目费用报销统计分析明细
        /// </summary>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="deptCode">统计部门</param>
        /// <param name="kmCode">统计项目</param>
        /// <returns>返回DataTable</returns>
        public DataTable SearchSyxmReportMx(DateTime kssj, DateTime jssj, string deptcode, string xmcode,string cxlx)
        {
            string sql ="";
            if (cxlx == "bx")
            {
                sql = " select g.deptcode+'['+g.deptname+']' as bm,a.xmcode+'['+e.xmname+']' as xm,b.fykm+'['+f.yskmmc+']' as km, a.je,h.usercode+'['+h.username+']' as ry,d.billdate,c.bxzy from bill_ybbxmxb_hsxm a left join bill_ybbxmxb_fykm b on a.kmmxguid=b.mxguid left join bill_ybbxmxb c on b.billcode=c.billcode left join bill_main d on d.billcode=c.billcode left join bill_xm e on a.xmcode=e.xmcode  left join bill_yskm f on b.fykm=f.yskmcode left join bill_departments g on d.gkdept=g.deptcode left join bill_users h on d.billuser=h.usercode where a.xmcode=@xmcode and d.gkdept=@deptcode   and  convert(varchar(10),d.billdate,121)>=convert(varchar(10),@kssj,121) and convert(varchar(10),d.billdate,121)<convert(varchar(10),@jssj,121)";
            }
            else
            {
                sql = " select a.zfdept+'['+c.deptname+']' as bm,a.zfxm+'['+b.xmname+']' as xm,'' as km, a.zfje as je,a.cbr+'['+d.username+']' as ry,a.sj as billdate,a.zynr as bxzy  from dbo.bill_xmzfd a left join bill_xm b on a.zfxm=b.xmcode left join bill_departments c on a.zfdept=c.deptcode left join bill_users d on a.cbr=d.usercode  where a.zfxm=@xmcode and a.zfdept=@deptcode   and  convert(varchar(10),a.sj,121)>=convert(varchar(10),@kssj,121) and convert(varchar(10),a.sj,121)<convert(varchar(10),@jssj,121)";
            }
            SqlParameter[] sps = { 
                                     new SqlParameter("@xmcode", xmcode),
                                     new SqlParameter("@deptcode", deptcode),
                                     new SqlParameter("@kssj",kssj.ToString("yyyy-MM-dd")),
                                     new SqlParameter("@jssj",jssj.ToString("yyyy-MM-dd"))
                                 };

            return DataHelper.GetDataTable(sql, sps, false);
        }
        
        /// <summary>
        /// 月结
        /// </summary>
        /// <param name="month">月份</param>
        /// <param name="userCode">用户编号</param>
        /// <param name="guid">guid</param>
        public void ProYj(string month,string userCode,string guid)
        {
            SqlParameter[] sps = { 
                                     new SqlParameter("@yf",month),
                                     new SqlParameter("@guid",guid),
                                     new SqlParameter("@userCode",userCode)                                     
                                 };
            DataHelper.ExcuteNonQuery("pro_yj", sps, true);
        }
        /// <summary>
        /// 检查是否能够月结，可以返回""
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string CheckYj(string month)
        {
            SqlParameter[] sps = {
                                     new SqlParameter("@yf",month)
                                 };
            int a =Convert.ToInt32(DataHelper.ExecuteScalar("select count(*) from dbo.bill_yj where yf=@yf and yjbj='1'",sps,false));
            if (a > 0)
            {
                return "该月已经做过月结";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 月结取消
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public string qxyj(string month)
        {
            if (CheckYj(month) != "")
            {
                string sql = "update bill_yj set yjbj='0' where yf='" + month + "'";
                DataHelper.ExecuteScalar(sql);
                return "";
            }
            else
            {
                return "该月份没有做过月结，不能取消月结";
            }
        }
        /// <summary>
        /// 获得月结表
        /// </summary>
        /// <returns></returns>
        public DataTable GetYsTable(string stryf)
        {
            string sql = "";
            if (stryf == "")
            {
                sql = "select yf,userCode,cguid,yjsj,case yjbj when '1' then '已月结 'else '未月结' end  as yjbj from bill_yj order by yf";
            }
            else {
                sql = "select yf,userCode,cguid,yjsj,case yjbj when '1' then '已月结 'else '未月结' end  as yjbj from bill_yj where left(yf,4)='" + stryf + "' order by yf";
            }
            return DataHelper.GetDataTable(sql, null, false);
        }
    }
}
