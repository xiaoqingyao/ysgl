using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;

namespace Dal.newysgl
{
    public class Bmfj
    {
        string sql = "select procode,deptcode,kmcode,je,by1,by2,by3,Attachment,Row_Number()over(order by procode) as crow from bill_ys_xmfjbm";
        public string GetJebyfjb(string yskm, string annual)
        {
            string cxsql = "select sum(budgetmoney) from dbo.bill_ys_xmfjlrb where annual=@annual and kmcode=@kmcode";
            SqlParameter[] paramter = { new SqlParameter("@annual",annual),
                                       new SqlParameter("@kmcode",yskm)};
            return Convert.ToDecimal(DataHelper.ExecuteScalar(cxsql, paramter, false)).ToString("0.00");
        }

        public bool AddBmfj(IList<bill_ys_xmfjbm> bm)
        {
            string delsql = "delete bill_ys_xmfjbm where procode=@procode and kmcode=@kmcode and deptcode=@deptcode";

            string insql = "insert into bill_ys_xmfjbm(procode,deptcode,kmcode,je,by1,by2,by3,attachment) values (@procode,@deptcode,@kmcode,@je,@by1,@by2,@by3,@attachment)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    //删除
                    foreach (var i in bm)
                    {
                        SqlParameter[] delparamter = { new SqlParameter("@procode", i.procode), new SqlParameter("@kmcode", i.kmcode), new SqlParameter("@deptcode", i.deptcode) };
                        DataHelper.ExcuteNonQuery(delsql, tran, delparamter, false);
                    }
                    //插入
                    foreach (var i in bm)
                    {
                        SqlParameter[] intparamter = { new SqlParameter("@procode",i.procode),
                                                       new SqlParameter("@deptcode",i.deptcode),
                                                      new SqlParameter("@kmcode",i.kmcode),
                                                      new SqlParameter("@je",i.je),
                                                      new SqlParameter("@by1",i.by1),
                                                      new SqlParameter("@by2",i.by2),
                                                      new SqlParameter("@by3",i.by3),
                                                      new SqlParameter("@attachment",i.Attachment)
                                                     };
                        DataHelper.ExcuteNonQuery(insql, tran, intparamter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public IList<bill_ys_xmfjbm> GetBmfjtb(string nd, string yskmcode)
        {
            string cxsql = sql + " where left(procode,4)=@nd and kmcode=@kmcode ";
            SqlParameter[] paramter = { new SqlParameter("@nd",nd),
                                       new SqlParameter("@kmcode",yskmcode)};
            return ListMaker(cxsql, paramter);

        }

        public IList<bill_ys_xmfjbm> GetData(string nd, string yskmcode, string strdeptcode)
        {
            string cxsql = sql + " where left(procode,4)=@nd and kmcode=@kmcode and deptcode=@deptcode";
            SqlParameter[] paramter = { new SqlParameter("@nd",nd),
                                       new SqlParameter("@kmcode",yskmcode),
                                      new SqlParameter("@deptcode",strdeptcode)
                                      };
            return ListMaker(cxsql, paramter);

        }

        public IList<bill_ys_xmfjbm> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_ys_xmfjbm> list = new List<bill_ys_xmfjbm>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_ys_xmfjbm model = new bill_ys_xmfjbm();
                model.procode = dr["procode"].ToString();
                model.deptcode = dr["deptcode"].ToString();
                model.kmcode = dr["kmcode"].ToString();
                if (!DBNull.Value.Equals(dr["je"]))
                {
                    model.je = decimal.Parse(dr["je"].ToString());
                }
                model.by1 = dr["by1"].ToString();
                model.by2 = dr["by2"].ToString();
                model.by3 = dr["by3"].ToString();
                model.Attachment = dr["Attachment"].ToString();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 通过部门编号年度获取为该部门分解的科目
        /// </summary>
        /// <param name="deptcode"></param>
        /// <param name="nd"></param>
        /// <param name="yskmtype"></param>
        /// <returns></returns>
        public DataTable GetListByDept(string deptcode, string nd, string yskmtype)
        {
            string cxsql = @"select procode=(select proname from bill_ys_benefitpro where bill_ys_benefitpro.procode = bill_ys_xmfjbm.procode ), procode as xmbh, kmcode as kmbh, by1 as jyje, by2 as sm,
	                        deptcode,(select '['+deptCode+']'+deptName from bill_departments where deptCode=bill_ys_xmfjbm.deptcode) as deptName,kmcode=(select '['+yskmcode+']'+yskmmc from bill_yskm where bill_yskm.yskmcode=bill_ys_xmfjbm.kmcode), je,by1,by2,by3,Attachment,(case by3 when '1' then '预算确认' when '2' then '部门确认' when '3' then '部门异议' else '未知' end) as status,Row_Number()over(order by procode) as crow 
                            from bill_ys_xmfjbm where 1=1";
            if (!string.IsNullOrEmpty(deptcode))
            {
                cxsql += " and deptcode in ('" + deptcode + "') ";
            }
            cxsql += " and left(procode,4) =@nd and by3 in ('0','1','2','3') order by kmcode";

            string appendsql = string.Empty;
            if (!string.IsNullOrEmpty(appendsql))
            {
                appendsql = " and kmcode in (select yskmcode from bill_yskm where dydj='" + yskmtype + "') ";
            }
            SqlParameter[] paramter = { new SqlParameter("@nd", nd) };
            return DataHelper.GetDataTable(cxsql, paramter, false);
        }

        public string GetDeptStateByUserCode(string deptcode, string nd)
        {
            string cxsql = @"select top 1 isnull(by3,'0') from  dbo.bill_ys_xmfjbm where deptcode=@deptcode    and left(procode,4) =@nd and by3 in( '2','1','3')   ";
            SqlParameter[] paramter = { new SqlParameter("@deptcode", deptcode), new SqlParameter("@nd", nd) };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count == 0)
            {
                return "未确认";
            }
            else
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    return "预算确认";
                }
                if (dt.Rows[0][0].ToString() == "3")
                {
                    return "部门异议";
                }
                else
                {
                    return "部门确认";
                }
            }

        }
        /// <summary>
        /// 部门分解金额确认
        /// </summary>
        /// <param name="bmlist"></param>
        /// <returns></returns>
        public bool InsertBmFjApprove(IList<bill_ys_xmfjbm> bmlist)
        {
            string upsql = "update bill_ys_xmfjbm set by3 = '2',by1=@shenbaoje where procode=@procode and kmcode=@kmcode and deptcode=@deptcode";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    foreach (var i in bmlist)
                    {
                        SqlParameter[] paramter = {
                                                       new SqlParameter("@shenbaoje", i.by1),
                                                      new SqlParameter("@procode", i.procode),
                                                    new SqlParameter("@kmcode",i.kmcode),
                                                   new SqlParameter("@deptcode",i.deptcode)};
                        DataHelper.ExcuteNonQuery(upsql, tran, paramter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }

        }

        public bool SaveBmFjApprove(IList<bill_ys_xmfjbm> bmlist)
        {
            string upsql = "update bill_ys_xmfjbm set by1=@by1, by2=@by2,by3=@by3 where procode=@procode and kmcode=@kmcode and deptcode=@deptcode";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    foreach (var i in bmlist)
                    {
                        SqlParameter[] paramter = { new SqlParameter("@procode", i.procode),
                                                    new SqlParameter("@kmcode",i.kmcode),
                                                   new SqlParameter("@by1",i.by1),
                                                   new SqlParameter("@by2",i.by2),
                                                   new SqlParameter("@by3",i.by3),
                                                   new SqlParameter("@deptcode",i.deptcode)};
                        DataHelper.ExcuteNonQuery(upsql, tran, paramter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }
        /// <summary>
        /// 预算确认
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public bool AddBmfjQr(IList<bill_ys_xmfjbm> bm)
        {
            string upsql = "update bill_ys_xmfjbm set by3='1' where procode=@procode and deptcode=@deptcode and kmcode=@kmcode";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    foreach (var i in bm)
                    {
                        SqlParameter[] paramter = { new SqlParameter("@procode", i.procode),
                                                    new SqlParameter("@kmcode",i.kmcode),
                                                   new SqlParameter("@deptcode",i.deptcode)};
                        DataHelper.ExcuteNonQuery(upsql, tran, paramter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public string IsFjtb(string nd, string deptcode)
        {
            string cxsql = @"select * from dbo.bill_ys_xmfjbm
                             where left(procode,4) = @nd
                                   and deptcode=@deptcode
                                   and by3 <> '0'";
            SqlParameter[] paramter = { new SqlParameter("@deptcode",deptcode),
                                        new SqlParameter("@nd",nd)};
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            DataRow[] NotCheckRow = dt.Select(" by3 = '1' ");  //如果财务确认 部门没确认 提示一下
            if (NotCheckRow.Count() > 0)
            {
                return "1"; //部门没有确认
            }
            else if (dt.Rows.Count == 0)
            {
                return "2"; //没有预算参数
            }
            else
            {
                return "0"; //有预算参数 并且部门已经全部确认
            }

        }
    }
}
