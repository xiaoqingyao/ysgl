using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Dal.UserProperty;
using Models;
using System.Data;

namespace Dal.Bills
{
    public class YsglDal
    {
        /// <summary>
        /// 删除预算明细,02是财务填报
        /// </summary>
        /// <param name="billCode"></param>
        /// <param name="tblx"></param>
        public void DeleteYsmx(string billCode, string tblx)
        {
            string delMx = @"delete bill_ysmxb
                            where billCode=@billCode
                            and yskm in (select yskmcode from bill_yskm where tblx=@tblx)";
            SqlParameter[] billsps = { 
                                         new SqlParameter("@billCode", billCode),
                                         new SqlParameter("@tblx",tblx)
                                     };
            DataHelper.ExcuteNonQuery(delMx, billsps, false);
        }

        public void DeleteYsmx(string billCode, SqlTransaction tran)
        {
            string delMx = @"delete bill_ysmxb
                            where billCode=@billCode";
            SqlParameter[] billsps = { 
                                         new SqlParameter("@billCode", billCode)
                                     };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="billCode"></param>
        public void DeleteYsmx(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteYsmx(billCode, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(billCode, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 取得月预算金额
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueYsje(DateTime dt, string deptCode, string kmCode)
        {
            string yue = int.Parse(dt.Month.ToString()).ToString();
            string nian = int.Parse(dt.Year.ToString()).ToString();
            string type = "2";
            string yscode = GetYsgcCode(nian, yue, type);

            if (string.IsNullOrEmpty(yscode) || string.IsNullOrEmpty(deptCode) || string.IsNullOrEmpty(kmCode))
            {
                return 0;
            }
            return GetYueYsje(yscode, deptCode, kmCode);
        }
        /// <summary>
        /// 取得月预算金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueYsje(string gcbh, string deptCode, string kmCode)
        {
            string sql = @" select isnull(sum(ysje),0) from bill_ysmxb,bill_main 
                            where gcbh=@gcbh and ysdept=@ysdept and yskm=@yskm
                            and bill_main.billcode=bill_ysmxb.billcode and stepid='end'  ";// and flowid in('ys','xmys')
            SqlParameter[] sps = {
                                    new SqlParameter("@gcbh",gcbh),
                                    new SqlParameter("@ysdept",deptCode),
                                    new SqlParameter("@yskm",kmCode)
                                 };
            return Math.Round(Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false)), 2);
        }

//        public decimal GetYueYsje_dept(string gcbh, string deptCode)
//        {

//            string sql = @" select isnull(sum(ysje),0) from bill_ysmxb,bill_main 
//                            where gcbh=@gcbh
//                            and ysdept in(select dydeptcode from bill_sqjfbmdy where billdept=@ysdept)
//                            and bill_main.billcode=bill_ysmxb.billcode and stepid='end'
//                             and yskm in (select yskmcode from bill_yskm where dydj='02') and stepid='end'  ";// and flowid in('ys','xmys')
//            SqlParameter[] sps = {
//                                    new SqlParameter("@gcbh",gcbh),
//                                    new SqlParameter("@ysdept",deptCode)
//                                 };
//            return Math.Round(Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false)), 2);
//        }
        /// <summary>
        /// 取得没审批通过的金额
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueNotEndje(string gcbh, string deptCode, string kmCode)
        {
            string sql = @"select round(isnull(sum(ysje),0),2) from bill_ysmxb,bill_main 
                            where gcbh=@gcbh and ysdept=@ysdept and yskm=@yskm
                            and bill_main.billcode=bill_ysmxb.billcode and stepid<>'end' and ysje<0 ";
            SqlParameter[] sps = {
                                    new SqlParameter("@gcbh",gcbh),
                                    new SqlParameter("@ysdept",deptCode),
                                    new SqlParameter("@yskm",kmCode)
                                 };
            return Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false));
        }
        /// <summary>
        /// 取得花费金额（审批未通过的也认为已花费,废弃）
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public decimal GetYueHfje(DateTime dt, string deptCode, string kmCode)
        {

            string sql = @" select round(isnull(sum(je),0),2) from bill_ybbxmxb_fykm 
                            where billcode in (
	                            select billcode 
	                            from bill_ybbxmxb 
	                            where bxmxlx in (select isnull(diccode,'1') 
	                            from bill_datadic where cjys='1')
                            )  
                            and fykm=@kmCode
                            and billCode in (
                                  select billCode 
                                  from bill_main 
                                  where left(convert(varchar(8),billDate,112),6)=@yf
                                  and flowid='ybbx' 
	                              and gkdept=@gkdept
                            )";
            string yf = dt.ToString("yyyyMM");
            SqlParameter[] sps = {
                                    new SqlParameter("@kmCode",kmCode),
                                    new SqlParameter("@yf",yf),
                                    new SqlParameter("@gkdept",deptCode)
                                 };
            return Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false));
        }
        /// <summary>
        /// 取得花费金额  junpin  fashen 
        /// </summary>
        /// <param name="begDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="deptCode">部门编号</param>
        /// <param name="kmCode">科目编号</param>
        /// <returns>金额</returns>
        public decimal GetYueHfje(DateTime begDate, DateTime endDate, string deptCode, string kmCode)
        {
            DepartmentDal deptDal = new DepartmentDal();
            // and (flowid='ybbx' or flowid='gkbx')
            string sql = @" select round(isnull(sum(je),0),2) from bill_ybbxmxb_fykm 
                            where billcode in (
	                            select billcode 
	                            from bill_ybbxmxb 
	                            where bxmxlx in (select isnull(diccode,'1') 
	                            from bill_datadic where cjys='1')
                            )  
                            and fykm=@kmCode
                            and billCode in (
                                  select billCode 
                                  from bill_main 
                                  where convert(varchar(10),billDate,121)>=@begDate and convert(varchar(10),billDate,121)<@endDate 
                                 
	                              and gkdept=@gkdept
                            )";
            SqlParameter[] sps = {
                                    new SqlParameter("@kmCode",kmCode),
                                    new SqlParameter("@begDate",begDate.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@endDate",endDate.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@gkdept",deptCode)
                                 };
            return Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false));
        }
        /// <summary>
        /// 花费金额
        /// </summary>
        /// <param name="begDate"></param>
        /// <param name="endDate"></param>
        /// <param name="deptCode"></param>
        /// <param name="kmCode"></param>
        /// <param name="strflowid"></param>
        /// <returns></returns>
        public decimal GetYueHfje(DateTime begDate, DateTime endDate, string deptCode, string kmCode,string strflowid)
        {
            DepartmentDal deptDal = new DepartmentDal();
            string sql = @" select round(isnull(sum(je),0),2) from bill_ybbxmxb_fykm 
                            where billcode in (
	                            select billcode 
	                            from bill_ybbxmxb 
	                            where bxmxlx in (select isnull(diccode,'1') 
	                            from bill_datadic where cjys='1')
                            )  
                            and fykm=@kmCode
                            and billCode in (
                                  select billCode 
                                  from bill_main 
                                  where convert(varchar(10),billDate,121)>=@begDate and convert(varchar(10),billDate,121)<@endDate 
                                  and (flowid=@flowid)
	                              and gkdept=@gkdept
                            )";
            SqlParameter[] sps = {
                                    new SqlParameter("@kmCode",kmCode),
                                    new SqlParameter("@begDate",begDate.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@endDate",endDate.ToString("yyyy-MM-dd")),
                                    new SqlParameter("@gkdept",deptCode),
                                    new SqlParameter("@flowid",strflowid)

                                 };
            return Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false));
        }
        /// <summary>
        /// 取得预算过程编号
        /// </summary>
        /// <param name="nian"></param>
        /// <param name="yue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetYsgcCode(string nian, string yue, string type)
        {
            string sql = "select gcbh from bill_ysgc where nian=@nian and yue=@yue and ystype=@type";
            SqlParameter[] sps = {
                                    new SqlParameter("@nian",nian),
                                    new SqlParameter("@yue",yue),
                                    new SqlParameter("@type",type)
                                 };
            return Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public Bill_Ysgc GetYsgcByCode(string code)
        {
            string sql = "select * from bill_ysgc where gcbh=@gcbh";
            SqlParameter[] sps = { new SqlParameter("@gcbh", code) };
            SqlDataReader dr = DataHelper.GetDataReader(sql, sps);
            Bill_Ysgc ysgc = new Bill_Ysgc();
            if (dr.Read())
            {
                ysgc.Fqsj = Convert.ToString(dr["Fqsj"]);
                ysgc.Fqr = Convert.ToString(dr["Fqr"]);
                ysgc.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysgc.Jzsj = Convert.ToDateTime(dr["Jzsj"]);
                ysgc.Kssj = Convert.ToDateTime(dr["Kssj"]);
                ysgc.Nian = Convert.ToString(dr["Nian"]);
                ysgc.Status = Convert.ToString(dr["Status"]);
                ysgc.Xmmc = Convert.ToString(dr["Xmmc"]);
                ysgc.YsType = Convert.ToString(dr["YsType"]);
                ysgc.Yue = Convert.ToString(dr["Yue"]);
                return ysgc;
            }
            else
            {
                return null;
            }
        }

        public IList<Bill_Ysgc> GetYsgcByYear(string year, string type)
        {
            string sql = "select * from bill_ysgc where nian=@nian and ystype=@type";
            SqlParameter[] sps = {
                                    new SqlParameter("@nian",year),
                                    new SqlParameter("@type",type)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ysgc> list = new List<Bill_Ysgc>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysgc ysgc = new Bill_Ysgc();
                ysgc.Fqsj = Convert.ToString(dr["Fqsj"]);
                ysgc.Fqr = Convert.ToString(dr["Fqr"]);
                ysgc.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysgc.Jzsj = Convert.ToDateTime(dr["Jzsj"]);
                ysgc.Kssj = Convert.ToDateTime(dr["Kssj"]);
                ysgc.Nian = Convert.ToString(dr["Nian"]);
                ysgc.Status = Convert.ToString(dr["Status"]);
                ysgc.Xmmc = Convert.ToString(dr["Xmmc"]);
                ysgc.YsType = Convert.ToString(dr["YsType"]);
                ysgc.Yue = Convert.ToString(dr["Yue"]);
                list.Add(ysgc);
            }
            return list;
        }

        public IList<Bill_Ysgc> GetYsgcByType(string type)
        {
            string sql = "select * from bill_ysgc where ystype=@type";
            SqlParameter[] sps = {
                                    new SqlParameter("@type",type)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ysgc> list = new List<Bill_Ysgc>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysgc ysgc = new Bill_Ysgc();
                ysgc.Fqsj = Convert.ToString(dr["Fqsj"]);
                ysgc.Fqr = Convert.ToString(dr["Fqr"]);
                ysgc.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysgc.Jzsj = Convert.ToDateTime(dr["Jzsj"]);
                ysgc.Kssj = Convert.ToDateTime(dr["Kssj"]);
                ysgc.Nian = Convert.ToString(dr["Nian"]);
                ysgc.Status = Convert.ToString(dr["Status"]);
                ysgc.Xmmc = Convert.ToString(dr["Xmmc"]);
                ysgc.YsType = Convert.ToString(dr["YsType"]);
                ysgc.Yue = Convert.ToString(dr["Yue"]);
                list.Add(ysgc);
            }
            return list;
        }

        /// <summary>
        /// 根据科目编号，得到已经做过预算的预算过程
        /// </summary>
        /// <param name="kmcode"></param>
        /// <returns></returns>
        public Boolean CheckYskm(string kmcode)
        {
            string sql = "select distinct a.* from bill_ysgc a,bill_ysmxb b where a.gcbh=b.gcbh and left(b.yskm,len(@kmcode))=@kmcode";
            SqlParameter[] sps = {
                                    new SqlParameter("@kmcode",kmcode)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                sql = "select top 1 * from Bill_Ybbxmxb_Fykm where left(fykm,len(@kmcode))=@kmcode";
                dt = DataHelper.GetDataTable(sql, sps, false);
                if (dt.Rows.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据过程编号,部门编号获得各科目预算
        /// </summary>
        /// <param name="gcbh"></param>
        /// <param name="depCode"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> GetYsmxByDeptYue(string gcbh, string depCode)
        {
            string sql = @"select gcbh,yskm,round(sum(isnull(ysje,0)),2) as ysje,ysDept from bill_ysmxb a,bill_main b 
                           where gcbh=@gcbh and ysDept=@depCode and a.billcode=b.billcode and b.stepid='end'
                           group by gcbh,yskm,ysDept";
            SqlParameter[] sps = {
                                    new SqlParameter("@gcbh",gcbh),
                                    new SqlParameter("@depCode",depCode)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysmxb ysmx = new Bill_Ysmxb();
                ysmx.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysmx.YsDept = Convert.ToString(dr["YsDept"]);
                ysmx.Ysje = Convert.ToDecimal(dr["Ysje"]);
                ysmx.Yskm = Convert.ToString(dr["Yskm"]);
                list.Add(ysmx);
            }
            return list;
        }
        /// <summary>
        /// 根据科目编号获得各月预算
        /// </summary>
        /// <param name="kmCode"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> GetYsmxByKm(string kmCode, string dept, string gcbh)
        {
            string sql = @" select gcbh,yskm,ysdept,round(sum(isnull(ysje,0)),2) as ysje
                            from bill_ysmxb where ysdept=@dept and yskm=@kmCode and yskm in(" + gcbh + @") 
                            group by gcbh,yskm,ysdept order by gcbh ";
            SqlParameter[] sps = {
                                    new SqlParameter("@dept",dept),
                                    new SqlParameter("@kmCode",kmCode)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysmxb ysmx = new Bill_Ysmxb();
                ysmx.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysmx.YsDept = Convert.ToString(dr["YsDept"]);
                ysmx.Ysje = Convert.ToDecimal(dr["Ysje"]);
                ysmx.Yskm = Convert.ToString(dr["Yskm"]);
                list.Add(ysmx);
            }
            return list;
        }

        public IList<Bill_Ysmxb> GetYsmxByCode(string billCode)
        {
            string sql = @"select * from bill_ysmxb
                           where billCode=@billCode";
            SqlParameter[] sps = {
                                    new SqlParameter("@billCode",billCode)
                                 };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysmxb ysmx = new Bill_Ysmxb();
                ysmx.Gcbh = Convert.ToString(dr["Gcbh"]);
                ysmx.YsDept = Convert.ToString(dr["YsDept"]);
                ysmx.Ysje = Convert.ToDecimal(dr["Ysje"]);
                ysmx.Yskm = Convert.ToString(dr["Yskm"]);
                ysmx.BillCode = Convert.ToString(dr["BillCode"]);
                ysmx.YsType = Convert.ToString(dr["YsType"]);
                list.Add(ysmx);
            }
            return list;
        }

        public void InsertYsmx(IList<Bill_Ysmxb> list, SqlTransaction tran)
        {
            string sql = @"insert into bill_ysmxb(gcbh, billCode, yskm, ysje, ysDept, ysType,sm)values
                            (@gcbh, @billCode, @yskm, @ysje, @ysDept, @ysType,@sm)";
            foreach (Bill_Ysmxb mxb in list)
            {
                SqlParameter[] sps = { 
                                         new SqlParameter("@gcbh",mxb.Gcbh),
                                         new SqlParameter("@billCode",mxb.BillCode),
                                         new SqlParameter("@yskm",mxb.Yskm),
                                         new SqlParameter("@ysje",mxb.Ysje),
                                         new SqlParameter("@ysDept",mxb.YsDept),
                                         new SqlParameter("@ysType",mxb.YsType),
                                         new SqlParameter("@sm",mxb.Sm)
                                     };
                DataHelper.ExcuteNonQuery(sql, tran, sps, false);
            }
        }

        public void InsertYsmx(IList<Bill_Ysmxb> list, Bill_Main main)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteYsmx(main.BillCode);
                    InsertYsmx(list, tran);
                    MainDal md = new MainDal();
                    md.DeleteMain(main.BillCode);
                    md.InsertMain(main, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        //2012.04.06 mxl 去预算金额
        public decimal GetYsje(string ystype, string nian, string deptCode, string kmCode)
        {
            string sql = @" select round(isnull(sum(ysje),0),2) from bill_ysmxb,bill_ysgc 
                            where nian=@nian and bill_ysmxb.ystype = '1' 
                            and bill_ysgc.ysType = @ystype and ysdept=@ysdept and yskm=@yskm
                            and bill_ysgc.gcbh=bill_ysmxb.gcbh 
                            and bill_ysmxb.billcode not in(select billcode from dbo.workflowrecord where rdstate=3)";
            SqlParameter[] sps = {
                                    new SqlParameter("@ystype",ystype),
                                    new SqlParameter("@nian",nian),
                                    new SqlParameter("@ysdept",deptCode),
                                    new SqlParameter("@yskm",kmCode)
                                 };
            return Convert.ToDecimal(DataHelper.ExecuteScalar(sql, sps, false));
        }

    }
}
