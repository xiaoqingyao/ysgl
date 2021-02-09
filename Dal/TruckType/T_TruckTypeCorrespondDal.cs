using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.TruckType
{
    /// <summary>
    /// 车辆型号对应
    /// </summary>
    public  class T_TruckTypeCorrespondDal
    {
        string sql = "select Note7,Note8,Note9,Note10,truckTypeCode,factTruckType,Note1,Note2,Note3,Note4,Note5,Note6,Row_Number()over(order by list_id) as crow from T_TruckTypeCorrespond";
        string sqlCont = "select count(*) from T_TruckTypeCorrespond";

        public bool Exists(long list_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" list_id = @list_id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.BigInt)
			};
            parameters[0].Value = list_id;

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

        public int Add(T_TruckTypeCorrespond model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    int row = Add(model, tran);
                    tran.Commit();
                    return row;
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
        public int Delete(long list_id)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int row = Delete(list_id, tran);
                    tran.Commit();
                    return row;
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
        public int Add(T_TruckTypeCorrespond model, SqlTransaction tran)
        {
            Delete(model.list_id, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_TruckTypeCorrespond(");
            strSql.Append("Note7,Note8,Note9,Note10,truckTypeCode,factTruckType,Note1,Note2,Note3,Note4,Note5,Note6");
            strSql.Append(") values (");
            strSql.Append("@Note7,@Note8,@Note9,@Note10,@truckTypeCode,@factTruckType,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@truckTypeCode", SqlDbType.NVarChar,200) ,            
                        new SqlParameter("@factTruckType", SqlDbType.NVarChar,200) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.Note7);

            parameters[1].Value = SqlNull(model.Note8);

            parameters[2].Value = SqlNull(model.Note9);

            parameters[3].Value = SqlNull(model.Note10);

            parameters[4].Value = SqlNull(model.truckTypeCode);

            parameters[5].Value = SqlNull(model.factTruckType);

            parameters[6].Value = SqlNull(model.Note1);

            parameters[7].Value = SqlNull(model.Note2);

            parameters[8].Value = SqlNull(model.Note3);

            parameters[9].Value = SqlNull(model.Note4);

            parameters[10].Value = SqlNull(model.Note5);

            parameters[11].Value = SqlNull(model.Note6);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        public DataTable GetAlltable(T_TruckTypeCorrespond model)
        {
            StringBuilder sbQuery = new StringBuilder();

            List<SqlParameter> lstParams = new List<SqlParameter>();
            string strsql = @"select a.*,(select '['+typeCode+']'+typeName from dbo.T_truckType where typeCode=a.truckTypeCode )as truckTypeName
 from T_TruckTypeCorrespond a where 1=1";
            if (model.truckTypeCode != null && model.truckTypeCode.Trim() != "")
            {
                sbQuery.Append(" and a.truckTypeCode in(select typeCode from T_truckType where parentCode='" + model.truckTypeCode + " ')");
                sbQuery.Append(" or a.truckTypeCode ='" + model.truckTypeCode + "'");
            }
            //显示所有未对应的记录
            if (model.IsShowNoCorrespond!=""&&model.IsShowNoCorrespond!=null)
            {
                sbQuery.Append(" and isnull(truckTypeCode,'')=''");
            }

            strsql += sbQuery.ToString();

            return DataHelper.GetDataTable(strsql, null, false);
        }



        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(long list_id, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_TruckTypeCorrespond ");
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.BigInt)
			};
            parameters[0].Value = list_id;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_TruckTypeCorrespond> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_TruckTypeCorrespond> list = new List<T_TruckTypeCorrespond>();
            foreach (DataRow dr in dt.Rows)
            {
                T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
                if (!DBNull.Value.Equals(dr["list_id"]))
                {
                    model.list_id = long.Parse(dr["list_id"].ToString());
                }
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.truckTypeCode = dr["truckTypeCode"].ToString();
                model.factTruckType = dr["factTruckType"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_TruckTypeCorrespond GetModel(long list_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.BigInt)
			};
            parameters[0].Value = list_id;


            T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["list_id"]))
                    {
                        model.list_id = long.Parse(dr["list_id"].ToString());
                    }
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.truckTypeCode = dr["truckTypeCode"].ToString();
                    model.factTruckType = dr["factTruckType"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();

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
        public IList<T_TruckTypeCorrespond> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_TruckTypeCorrespond> GetAllList(int beg, int end)
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

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }


    }
}
