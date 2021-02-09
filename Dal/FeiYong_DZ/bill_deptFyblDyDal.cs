using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dal.FeiYong_DZ
{
    /// <summary>
    /// 
    /// </summary>
    public class bill_deptFyblDyDal
    {
        string sql = "select list_id,cdefine6,ddefine7,ddefine8,ddefine9,ddefine10,deptCode,deptName,fjbl,cdefine1,cdefine2,cdefine3,cdefine4,cdefine5,Row_Number()over(order by list_id desc) as crow from bill_deptFyblDy";
        string sqlCont = "select count(*) from bill_deptFyblDy";

        public bool Exists(string deptCode,string nd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" deptCode = @deptCode and cdefine1=@nd  ");
            SqlParameter[] parameters = {
					new SqlParameter("@deptCode", SqlDbType.VarChar,50),
                    new SqlParameter("@nd",SqlDbType.VarChar,50)
			};
            parameters[0].Value = deptCode;
            parameters[1].Value = nd;
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

        public int Add(bill_deptFyblDy model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.deptCode,model.cdefine1, tran);
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
        public int Delete(string deptcode,string nd)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(deptcode,nd, tran);
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
        public int Add(bill_deptFyblDy model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_deptFyblDy(");
            strSql.Append("cdefine6,ddefine7,ddefine8,ddefine9,ddefine10,deptCode,deptName,fjbl,cdefine1,cdefine2,cdefine3,cdefine4,cdefine5");
            strSql.Append(") values (");
            strSql.Append("@cdefine6,@ddefine7,@ddefine8,@ddefine9,@ddefine10,@deptCode,@deptName,@fjbl,@cdefine1,@cdefine2,@cdefine3,@cdefine4,@cdefine5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@cdefine6", SqlDbType.VarChar,300) ,            
                        new SqlParameter("@ddefine7", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ddefine8", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ddefine9", SqlDbType.DateTime) ,            
                        new SqlParameter("@ddefine10", SqlDbType.DateTime) ,            
                        new SqlParameter("@deptCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@deptName", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@fjbl", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@cdefine1", SqlDbType.VarChar,10) ,            
                        new SqlParameter("@cdefine2", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@cdefine3", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@cdefine4", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@cdefine5", SqlDbType.VarChar,100)             
              
            };

            parameters[0].Value = SqlNull(model.cdefine6);

            parameters[1].Value = SqlNull(model.ddefine7);

            parameters[2].Value = SqlNull(model.ddefine8);

            parameters[3].Value = SqlNull(model.ddefine9);

            parameters[4].Value = SqlNull(model.ddefine10);

            parameters[5].Value = SqlNull(model.deptCode);

            parameters[6].Value = SqlNull(model.deptName);

            parameters[7].Value = SqlNull(model.fjbl);

            parameters[8].Value = SqlNull(model.cdefine1);

            parameters[9].Value = SqlNull(model.cdefine2);

            parameters[10].Value = SqlNull(model.cdefine3);

            parameters[11].Value = SqlNull(model.cdefine4);

            parameters[12].Value = SqlNull(model.cdefine5);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string deptcode,string nd, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_deptFyblDy ");
            strSql.Append(" where deptCode=@deptCode and cdefine1=@nd ");
            SqlParameter[] parameters = {
					new SqlParameter("@deptCode", SqlDbType.VarChar,50),
                    new SqlParameter("@nd",SqlDbType.VarChar,50)
			};
            parameters[0].Value = deptcode;
            parameters[1].Value = nd;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<bill_deptFyblDy> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_deptFyblDy> list = new List<bill_deptFyblDy>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_deptFyblDy model = new bill_deptFyblDy();
                if (!DBNull.Value.Equals(dr["list_id"]))
                {
                    model.list_id = int.Parse(dr["list_id"].ToString());
                }
                model.cdefine6 = dr["cdefine6"].ToString();
                if (!DBNull.Value.Equals(dr["ddefine7"]))
                {
                    model.ddefine7 = decimal.Parse(dr["ddefine7"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine8"]))
                {
                    model.ddefine8 = decimal.Parse(dr["ddefine8"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine9"]))
                {
                    model.ddefine9 = DateTime.Parse(dr["ddefine9"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ddefine10"]))
                {
                    model.ddefine10 = DateTime.Parse(dr["ddefine10"].ToString());
                }
                model.deptCode = dr["deptCode"].ToString();
                model.deptName = dr["deptName"].ToString();
                if (!DBNull.Value.Equals(dr["fjbl"]))
                {
                    model.fjbl = decimal.Parse(dr["fjbl"].ToString());
                }
                model.cdefine1 = dr["cdefine1"].ToString();
                model.cdefine2 = dr["cdefine2"].ToString();
                model.cdefine3 = dr["cdefine3"].ToString();
                model.cdefine4 = dr["cdefine4"].ToString();
                model.cdefine5 = dr["cdefine5"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public bill_deptFyblDy GetModel(int list_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Int,4)
			};
            parameters[0].Value = list_id;


            bill_deptFyblDy model = new bill_deptFyblDy();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["list_id"]))
                    {
                        model.list_id = int.Parse(dr["list_id"].ToString());
                    }
                    model.cdefine6 = dr["cdefine6"].ToString();
                    if (!DBNull.Value.Equals(dr["ddefine7"]))
                    {
                        model.ddefine7 = decimal.Parse(dr["ddefine7"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine8"]))
                    {
                        model.ddefine8 = decimal.Parse(dr["ddefine8"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine9"]))
                    {
                        model.ddefine9 = DateTime.Parse(dr["ddefine9"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ddefine10"]))
                    {
                        model.ddefine10 = DateTime.Parse(dr["ddefine10"].ToString());
                    }
                    model.deptCode = dr["deptCode"].ToString();
                    model.deptName = dr["deptName"].ToString();
                    if (!DBNull.Value.Equals(dr["fjbl"]))
                    {
                        model.fjbl = decimal.Parse(dr["fjbl"].ToString());
                    }
                    model.cdefine1 = dr["cdefine1"].ToString();
                    model.cdefine2 = dr["cdefine2"].ToString();
                    model.cdefine3 = dr["cdefine3"].ToString();
                    model.cdefine4 = dr["cdefine4"].ToString();
                    model.cdefine5 = dr["cdefine5"].ToString();

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
        public IList<bill_deptFyblDy> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_deptFyblDy> GetAllList(int beg, int end)
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
        public IList<bill_deptFyblDy> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
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
        public IList<bill_deptFyblDy> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
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
        public IList<bill_deptFyblDy> GetAllList(List<SqlParameter> paramter, string sqls)
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
