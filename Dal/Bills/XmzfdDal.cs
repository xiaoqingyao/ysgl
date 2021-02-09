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
    public class XmzfdDal
    {
        public void InsertXmzfdDal(Bill_xmzfd xmzfd, SqlTransaction tran)
        {
            string sql = @"insert into bill_xmzfd(billcode,sj,zfDept,zfxm,zynr,sm,cbr,ms,zfje) values (@billcode,@sj,@zfDept,@zfxm,@zynr,@sm,@cbr,@ms,@zfje)";

            SqlParameter[] parameters = {
					new SqlParameter("@billcode", SqlNull(xmzfd.Billcode)),
					new SqlParameter("@sj", SqlNull(xmzfd.Sj)),
					new SqlParameter("@zfDept", SqlNull(xmzfd.ZfDept)),
					new SqlParameter("@zfxm", SqlNull(xmzfd.Zfxm)),
					new SqlParameter("@zynr", SqlNull(xmzfd.Zynr)),
					new SqlParameter("@sm", SqlNull(xmzfd.Sm)),
					new SqlParameter("@cbr", SqlNull(xmzfd.Cbr)),
					new SqlParameter("@ms", SqlNull(xmzfd.Ms)),
					new SqlParameter("@zfje", SqlNull(xmzfd.Zfje))};


            DataHelper.ExcuteNonQuery(sql,tran, parameters, false);
        }

        public void InsertXmzfdDal(Bill_xmzfd xmzfd, Bill_Main main)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal mainDal = new MainDal();
                    if (GetXmzfdByCode(xmzfd.Billcode) != null)
                    {
                        DeleteXmzfd(xmzfd.Billcode, tran);
                    }
                    mainDal.InsertMain(main,tran);
                    InsertXmzfdDal(xmzfd, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public void DeleteXmzfd(string billcode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteXmzfd(billcode, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public void DeleteXmzfd(string billCode, SqlTransaction tran)
        {
            MainDal mainDal = new MainDal();
            mainDal.DeleteMain(billCode, tran);
            string sql = @"delete bill_xmzfd where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public Bill_xmzfd GetXmzfdByCode(string billCode)
        {
            string sql = "select * from bill_xmzfd where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            SqlDataReader dr = DataHelper.GetDataReader(sql, sps);
            Bill_xmzfd xmzfd = new Bill_xmzfd();
            if (dr.Read())
            {
                xmzfd.Billcode = Convert.ToString(dr["billcode"]);
                xmzfd.Sj = Convert.ToDateTime(dr["sj"]);
                xmzfd.ZfDept = Convert.ToString(dr["zfDept"]);
                xmzfd.Zfxm = Convert.ToString(dr["zfxm"]);
                xmzfd.Zynr = Convert.ToString(dr["zynr"]);
                xmzfd.Sm = Convert.ToString(dr["sm"]);
                xmzfd.Cbr = Convert.ToString(dr["cbr"]);
                xmzfd.Ms = Convert.ToString(dr["ms"]);
                xmzfd.Zfje = Convert.ToDecimal(dr["zfje"]);
                return xmzfd;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据传递的参数组建查询条件查询项目支付报销单
        /// </summary>
        /// <param name="deptcode">部门</param>
        /// <param name="usercode">人员</param>
        /// <param name="xmcode">项目</param>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="je">金额</param>
        /// <returns>返回datatable，无条件时参数为“”</returns>
        public DataTable GetXmzfsqd(string billcode,string deptcode, string usercode, string xmcode, string kssj, string jssj, string je)
        {
            string sql = " select a.billcode,a.billname,a.flowid,a.stepid,a.billuser,d.username,a.billdate,a.billdept,c.deptname,a.billje,b.zfxm,e.xmname,b.zynr,b.sm from bill_main a left join bill_xmzfd b on a.billcode=b.billcode  left join bill_departments c on a.billdept=c.deptCode left join bill_users d on a.billuser=d.userCode left join bill_xm e on b.zfxm=e.xmCode  ";
            IList<SqlParameter> list = new List<SqlParameter>();
            sql = sql + " where a.flowid=@flowid  ";
            SqlParameter flowid = new SqlParameter("@flowid", "xmzf");
            list.Add(flowid);
            if (billcode != "")
            {
                sql = sql + " and a.billcode=@billcode ";
                SqlParameter code = new SqlParameter("@billcode", billcode);
                list.Add(code);
            }
            if (deptcode != "")
            {
                sql = sql + " and a.billdept=@deptcode ";
                SqlParameter dep = new SqlParameter("@deptcode", deptcode);
                list.Add(dep);
            }
            if (usercode != "")
            {
                sql = sql + " and a.billuser=@usercode ";
                SqlParameter user = new SqlParameter("@usercode", usercode);
                list.Add(user);
            }
            if (xmcode != "")
            {
                sql = sql + " and b.zfxm=@xmcode ";
                SqlParameter xm = new SqlParameter("@xmcode", xmcode);
                list.Add(xm);
            }
            if (kssj != "")
            {
                sql = sql + " and convert(varchar(10),a.billdate,121)>=@kssj ";
                SqlParameter ks = new SqlParameter("@kssj", kssj);
                list.Add(ks);
            }
            if (jssj != "")
            {
                sql = sql + " and convert(varchar(10),a.billdate,121)<=@jssj ";
                SqlParameter js = new SqlParameter("@jssj", jssj);
                list.Add(js);
            }
            if (je != "")
            {
                sql = sql + " and a.billje=@je ";
                SqlParameter j = new SqlParameter("@je", je);
                list.Add(j);
            }
            sql = sql+" order by billdate desc";
            SqlParameter[] sps = new SqlParameter[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                sps[i] = list[i];
            }

            return DataHelper.GetDataTable(sql, sps, false);
        }

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}
