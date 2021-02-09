using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;

namespace Dal.Bills
{
    public class bill_lydsDal
    {
        string sql = "select guid,note6,note7,note8,note9,note0,myGuid,fykm,je,note1,note2,note3,note4,note5,Row_Number()over(order by guid) as crow from bill_lyds";
        string sqlCont = "select count(*) from bill_lyds";

        public bool Exists(string guid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" guid = @guid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.VarChar,50)			};
            parameters[0].Value = guid;

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

        public int Add(bill_lyds model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.guid, tran);
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
        public int Delete(string guid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(guid, tran);
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
        public int Add(bill_lyds model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_lyds(");
            strSql.Append("guid,note6,note7,note8,note9,note0,myGuid,fykm,je,note1,note2,note3,note4,note5");
            strSql.Append(") values (");
            strSql.Append("@guid,@note6,@note7,@note8,@note9,@note0,@myGuid,@fykm,@je,@note1,@note2,@note3,@note4,@note5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@guid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note6", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note7", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note8", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note9", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note0", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@myGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@fykm", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@je", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@note1", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note2", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note3", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note4", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note5", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.guid);

            parameters[1].Value = SqlNull(model.note6);

            parameters[2].Value = SqlNull(model.note7);

            parameters[3].Value = SqlNull(model.note8);

            parameters[4].Value = SqlNull(model.note9);

            parameters[5].Value = SqlNull(model.note0);

            parameters[6].Value = SqlNull(model.myGuid);

            parameters[7].Value = SqlNull(model.fykm);

            parameters[8].Value = SqlNull(model.je);

            parameters[9].Value = SqlNull(model.note1);

            parameters[10].Value = SqlNull(model.note2);

            parameters[11].Value = SqlNull(model.note3);

            parameters[12].Value = SqlNull(model.note4);

            parameters[13].Value = SqlNull(model.note5);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string guid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_lyds ");
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.VarChar,50)			};
            parameters[0].Value = guid;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<bill_lyds> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_lyds> list = new List<bill_lyds>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_lyds model = new bill_lyds();
                model.guid = dr["guid"].ToString();
                model.note6 = dr["note6"].ToString();
                model.note7 = dr["note7"].ToString();
                model.note8 = dr["note8"].ToString();
                model.note9 = dr["note9"].ToString();
                model.note0 = dr["note0"].ToString();
                model.myGuid = dr["myGuid"].ToString();
                model.fykm = dr["fykm"].ToString();
                if (!DBNull.Value.Equals(dr["je"]))
                {
                    model.je = decimal.Parse(dr["je"].ToString());
                }
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public bill_lyds GetModel(string guid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.VarChar,50)			};
            parameters[0].Value = guid;


            bill_lyds model = new bill_lyds();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.guid = dr["guid"].ToString();
                    model.note6 = dr["note6"].ToString();
                    model.note7 = dr["note7"].ToString();
                    model.note8 = dr["note8"].ToString();
                    model.note9 = dr["note9"].ToString();
                    model.note0 = dr["note0"].ToString();
                    model.myGuid = dr["myGuid"].ToString();
                    model.fykm = dr["fykm"].ToString();
                    if (!DBNull.Value.Equals(dr["je"]))
                    {
                        model.je = decimal.Parse(dr["je"].ToString());
                    }
                    model.note1 = dr["note1"].ToString();
                    model.note2 = dr["note2"].ToString();
                    model.note3 = dr["note3"].ToString();
                    model.note4 = dr["note4"].ToString();
                    model.note5 = dr["note5"].ToString();

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
        public IList<bill_lyds> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_lyds> GetAllList(int beg, int end)
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
        public IList<bill_lyds> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
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
        public IList<bill_lyds> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
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
        public IList<bill_lyds> GetAllList(List<SqlParameter> paramter, string sqls)
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

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_lyds> GetItemByMain(string guid)
        {
            StringBuilder strSql = new StringBuilder();
          
            strSql.Append(sql);
            strSql.Append(" where guid=@guid ");
            SqlParameter[] parameters = {
					new SqlParameter("@guid", SqlDbType.VarChar,50)			};
            parameters[0].Value = guid;
            
            return ListMaker(strSql.ToString(), null);
        }
        

    }
}
