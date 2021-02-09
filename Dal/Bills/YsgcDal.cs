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
    public class YsgcDal
    {
        public void InsertYsgcDal(Bill_Ysgc ysgc)
        {
            string sql = @"insert into bill_ysgc(gcbh, xmmc, kssj, jzsj, status, fqr, fqsj, nian, yue, ysType) values (@gcbh, @xmmc, @kssj, @jzsj, @status, @fqr, @fqsj, @nian, @yue, @ysType)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@gcbh",SqlNull(ysgc.Gcbh)),
                                     new SqlParameter("@xmmc",SqlNull(ysgc.Xmmc)),
                                     new SqlParameter("@kssj",SqlNull(ysgc.Kssj)),
                                     new SqlParameter("@jzsj",SqlNull(ysgc.Jzsj)),
                                     new SqlParameter("@status",SqlNull(ysgc.Status)),
                                     new SqlParameter("@fqr",SqlNull(ysgc.Fqr)),
                                     new SqlParameter("@fqsj",SqlNull(ysgc.Fqsj)),
                                     new SqlParameter("@nian",SqlNull(ysgc.Nian)),
                                     new SqlParameter("@yue",SqlNull(ysgc.Yue)),
                                     new SqlParameter("@ysType",SqlNull(ysgc.YsType))                                                                       
                                 };
            DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        public IList<Bill_Ysgc> GetYsgcByYear(string year, string type)
        {
            string sql = "select * from bill_ysgc where nian=@year and ystype=@type";
            SqlParameter[] sps = { 
                                     new SqlParameter("@year",year),
                                     new SqlParameter("@type",type)
                                 };
            IList<Bill_Ysgc> list = new List<Bill_Ysgc>();
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysgc ysgc = new Bill_Ysgc();
                ysgc.Fqr = Convert.ToString(dr["Fqr"]);
                ysgc.Fqsj = Convert.ToString(dr["Fqsj"]);
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

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }

        public IList<Bill_Ysgc> GetYsgcByNian(string nd)
        {
            string cxsql = "select gcbh,ysType,xmmc,kssj,jzsj,status,fqr,fqsj,nian,yue,Row_Number()over(order by gcbh) as crow from bill_ysgc" + " where nian=@nian ";
            SqlParameter[] paramter = { new SqlParameter("@nian", nd) };
            return ListMaker(cxsql, paramter);
        }
        public IList<Bill_Ysgc> ListMaker(string tempsql, SqlParameter[] sps)
        {

            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Bill_Ysgc> list = new List<Bill_Ysgc>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysgc model = new Bill_Ysgc();
                model.Gcbh = dr["gcbh"].ToString();
                model.YsType = dr["ysType"].ToString();
                model.Xmmc = dr["xmmc"].ToString();
                if (!DBNull.Value.Equals(dr["kssj"]))
                {
                    model.Kssj = DateTime.Parse(dr["kssj"].ToString());
                }
                if (!DBNull.Value.Equals(dr["jzsj"]))
                {
                    model.Jzsj = DateTime.Parse(dr["jzsj"].ToString());
                }
                model.Status = dr["status"].ToString();
                model.Fqr = dr["fqr"].ToString();
                model.Fqsj = dr["fqsj"].ToString();
                model.Nian = dr["nian"].ToString();
                model.Yue = dr["yue"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="p">预算过程编号</param>
        /// <param name="deptcode">部门编号</param>
        ///<param name="xmbh">项目编号 用于处理大智问题</param> 
        /// <returns>true-不允许再次编辑  false -允许再次编辑</returns>
        public bool IsState(string p, string deptcode, string flowid, string xmbh)
        {
            string cxsql = "select status  from  dbo.bill_ysgc where gcbh=@gcbh";
            SqlParameter[] paramter = { new SqlParameter("@gcbh", p) };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    //如果已经提交审批了  返回true 代表不允许编辑
                    string cxsql1 = " select count(*) from workflowrecords where recordid=(select recordid from workflowrecord where billCode =(select billcode from bill_main where  flowid='" + flowid + "' and billname=@billname  and billdept=@billdept and isnull(note3,'')=@xmbh ))";
                    SqlParameter[] paramter1 = { new SqlParameter("@billdept", deptcode),
                                               new SqlParameter("@billname",p),
                                               new SqlParameter("@xmbh",xmbh)};
                    int s = Convert.ToInt32(DataHelper.ExecuteScalar(cxsql1, paramter1, false));
                    return s > 0;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true; ;
            }
        }

        public bool IsState(string p, string deptcode, string flowid, string strdydj, string xmbh)
        {
            string cxsql = "select status  from  dbo.bill_ysgc where gcbh=@gcbh";
            SqlParameter[] paramter = { new SqlParameter("@gcbh", p) };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {

                    string cxsql1 = @" select count(*) from workflowrecords
					  where recordid in(
										select recordid from workflowrecord 
					                                  where billCode  in(
																		select billcode from bill_main where flowid=@flowid and billdept=@billdept and billname=@billname and isnull(note3,'')=@xmbh
					                                                                     and billcode in(
																										select billcode from bill_ysmxb where ysdept=@billdept 
					                                                                                    and yskm in (select yskmcode from bill_yskm where dydj=@dydj)
					                                                                                     )
					                                                                                     					  
																		  )
					   
					             )
					 ";
                    //如果已经提交审批了  返回true 代表不允许编辑
                    //string cxsql1 = @" select count(*) from workflowrecords where recordid=(select recordid from workflowrecord where billCode =(select billcode from bill_main where  flowid='" + flowid + "' and billname=@billname  and billdept=@billdept and billtype=@dydj))";
                    SqlParameter[] paramter1 = { new SqlParameter("@billdept", deptcode),
                                               new SqlParameter("@billname",p),
                                               new SqlParameter("@dydj",strdydj),
                                               new SqlParameter("@flowid",flowid),
                                               new SqlParameter("@xmbh",xmbh)}
                                               ;
                    int s = Convert.ToInt32(DataHelper.ExecuteScalar(cxsql1, paramter1, false));
                    return s > 0;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true; ;
            }
        }

    }
}
