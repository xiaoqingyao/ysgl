using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.Bills
{
   public class bill_yksqDal
    {
        string sql = "select billCode,cdecfine3,cdecfine4,ddefine0,ddefine1,ddefine3,ddefine4,ddefine5,note1,note2,note3,jbr,note4,note5,note6,note7,note8,note9,note0,billDept,yt,je,rkCodes,cdecfine0,cdecfine1,cdecfine2,Row_Number()over(order by billCode) as crow from bill_yksq";
        string sqlCont = "select count(*) from bill_yksq";

        public bool Exists(string billCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" billCode = @billCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = billCode;

            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), parameters, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Add(bill_yksq model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.billCode, tran);
                    int iRel = Add(model, tran);
                    tran.Commit();
                    return iRel;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(billCode, tran);
                    tran.Commit();
                    return iRel;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(bill_yksq model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_yksq(");
            strSql.Append("billCode,cdecfine3,cdecfine4,ddefine0,ddefine1,ddefine3,ddefine4,ddefine5,note1,note2,note3,jbr,note4,note5,note6,note7,note8,note9,note0,billDept,yt,je,rkCodes,cdecfine0,cdecfine1,cdecfine2");
            strSql.Append(") values (");
            strSql.Append("@billCode,@cdecfine3,@cdecfine4,@ddefine0,@ddefine1,@ddefine3,@ddefine4,@ddefine5,@note1,@note2,@note3,@jbr,@note4,@note5,@note6,@note7,@note8,@note9,@note0,@billDept,@yt,@je,@rkCodes,@cdecfine0,@cdecfine1,@cdecfine2");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@billCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cdecfine3", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@cdecfine4", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ddefine0", SqlDbType.DateTime) ,            
                        new SqlParameter("@ddefine1", SqlDbType.DateTime) ,            
                        new SqlParameter("@ddefine3", SqlDbType.DateTime) ,            
                        new SqlParameter("@ddefine4", SqlDbType.DateTime) ,            
                        new SqlParameter("@ddefine5", SqlDbType.DateTime) ,            
                        new SqlParameter("@note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@jbr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@note0", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@billDept", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yt", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@je", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@rkCodes", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@cdecfine0", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@cdecfine1", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@cdecfine2", SqlDbType.Decimal,9)             
              
            };

            parameters[0].Value = SqlNull(model.billCode);

            parameters[1].Value = SqlNull(model.cdecfine3);

            parameters[2].Value = SqlNull(model.cdecfine4);

            parameters[3].Value = SqlNull(model.ddefine0);

            parameters[4].Value = SqlNull(model.ddefine1);

            parameters[5].Value = SqlNull(model.ddefine3);

            parameters[6].Value = SqlNull(model.ddefine4);

            parameters[7].Value = SqlNull(model.ddefine5);

            parameters[8].Value = SqlNull(model.note1);

            parameters[9].Value = SqlNull(model.note2);

            parameters[10].Value = SqlNull(model.note3);

            parameters[11].Value = SqlNull(model.jbr);

            parameters[12].Value = SqlNull(model.note4);

            parameters[13].Value = SqlNull(model.note5);

            parameters[14].Value = SqlNull(model.note6);

            parameters[15].Value = SqlNull(model.note7);

            parameters[16].Value = SqlNull(model.note8);

            parameters[17].Value = SqlNull(model.note9);

            parameters[18].Value = SqlNull(model.note0);

            parameters[19].Value = SqlNull(model.billDept);

            parameters[20].Value = SqlNull(model.yt);

            parameters[21].Value = SqlNull(model.je);

            parameters[22].Value = SqlNull(model.rkCodes);

            parameters[23].Value = SqlNull(model.cdecfine0);

            parameters[24].Value = SqlNull(model.cdecfine1);

            parameters[25].Value = SqlNull(model.cdecfine2);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string billCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_yksq ");
            strSql.Append(" where billCode=@billCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = billCode;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<bill_yksq> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_yksq> list = new List<bill_yksq>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_yksq model = new bill_yksq();
                model.billCode = dr["billCode"].ToString();
                if (!DBNull.Value.Equals(dr["cdecfine3"]))
                {
                    model.cdecfine3 = decimal.Parse(dr["cdecfine3"].ToString());
                }
                if (!DBNull.Value.Equals(dr["cdecfine4"]))
                {
                    model.cdecfine4 = decimal.Parse(dr["cdecfine4"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine0"]))
                {
                    model.ddefine0 = DateTime.Parse(dr["ddefine0"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine1"]))
                {
                    model.ddefine1 = DateTime.Parse(dr["ddefine1"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine3"]))
                {
                    model.ddefine3 = DateTime.Parse(dr["ddefine3"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine4"]))
                {
                    model.ddefine4 = DateTime.Parse(dr["ddefine4"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine5"]))
                {
                    model.ddefine5 = DateTime.Parse(dr["ddefine5"].ToString());
                }
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.note3 = dr["note3"].ToString();
                model.jbr = dr["jbr"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();
                model.note6 = dr["note6"].ToString();
                model.note7 = dr["note7"].ToString();
                model.note8 = dr["note8"].ToString();
                model.note9 = dr["note9"].ToString();
                model.note0 = dr["note0"].ToString();
                model.billDept = dr["billDept"].ToString();
                model.yt = dr["yt"].ToString();
                if (!DBNull.Value.Equals(dr["je"]))
                {
                    model.je = decimal.Parse(dr["je"].ToString());
                }
                model.rkCodes = dr["rkCodes"].ToString();
                if (!DBNull.Value.Equals(dr["cdecfine0"]))
                {
                    model.cdecfine0 = decimal.Parse(dr["cdecfine0"].ToString());
                }
                if (!DBNull.Value.Equals(dr["cdecfine1"]))
                {
                    model.cdecfine1 = decimal.Parse(dr["cdecfine1"].ToString());
                }
                if (!DBNull.Value.Equals(dr["cdecfine2"]))
                {
                    model.cdecfine2 = decimal.Parse(dr["cdecfine2"].ToString());
                }

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public bill_yksq GetModel(string billCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where billCode=@billCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = billCode;


            bill_yksq model = new bill_yksq();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.billCode = dr["billCode"].ToString();
                    if (!DBNull.Value.Equals(dr["cdecfine3"]))
                    {
                        model.cdecfine3 = decimal.Parse(dr["cdecfine3"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["cdecfine4"]))
                    {
                        model.cdecfine4 = decimal.Parse(dr["cdecfine4"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine0"]))
                    {
                        model.ddefine0 = DateTime.Parse(dr["ddefine0"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine1"]))
                    {
                        model.ddefine1 = DateTime.Parse(dr["ddefine1"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine3"]))
                    {
                        model.ddefine3 = DateTime.Parse(dr["ddefine3"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine4"]))
                    {
                        model.ddefine4 = DateTime.Parse(dr["ddefine4"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine5"]))
                    {
                        model.ddefine5 = DateTime.Parse(dr["ddefine5"].ToString());
                    }
                    model.note1 = dr["note1"].ToString();
                    model.note2 = dr["note2"].ToString();
                    model.note3 = dr["note3"].ToString();
                    model.jbr = dr["jbr"].ToString();
                    model.note4 = dr["note4"].ToString();
                    model.note5 = dr["note5"].ToString();
                    model.note6 = dr["note6"].ToString();
                    model.note7 = dr["note7"].ToString();
                    model.note8 = dr["note8"].ToString();
                    model.note9 = dr["note9"].ToString();
                    model.note0 = dr["note0"].ToString();
                    model.billDept = dr["billDept"].ToString();
                    model.yt = dr["yt"].ToString();
                    if (!DBNull.Value.Equals(dr["je"]))
                    {
                        model.je = decimal.Parse(dr["je"].ToString());
                    }
                    model.rkCodes = dr["rkCodes"].ToString();
                    if (!DBNull.Value.Equals(dr["cdecfine0"]))
                    {
                        model.cdecfine0 = decimal.Parse(dr["cdecfine0"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["cdecfine1"]))
                    {
                        model.cdecfine1 = decimal.Parse(dr["cdecfine1"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["cdecfine2"]))
                    {
                        model.cdecfine2 = decimal.Parse(dr["cdecfine2"].ToString());
                    }

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得行数
        /// </summary>
        public int GetAllCount()
        {
            return Convert.ToInt32(DataHelper.ExecuteScalar(sqlCont, null, false));
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_yksq> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_yksq> GetAllList(int beg, int end)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sql);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            return ListMaker(strSql.ToString(), null);
        }




        /// <summary>
        /// 根据查询条件分页
        /// </summary>
        public IList<bill_yksq> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }
        /// <summary>
        /// 根据查询条件分页并返回记录数 
        /// </summary>
        public IList<bill_yksq> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
        {
            totalCount = GetAllListCount(paramter, sqls);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }


        /// <summary>
        /// 根据查询条件
        /// </summary>
        public IList<bill_yksq> GetAllList(List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);

            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }

        /// <summary>
        /// 根据查询条件获取行数
        /// </summary>
        public int GetAllListCount(List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(sqlCont);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);

            if (paramter == null)
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), null, false));
            }
            else
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), paramter.ToArray(), false));
            }
        }

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}
