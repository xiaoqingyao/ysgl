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
    /// 卡车类型dal
    /// </summary>
    public class TruckTypeDal
    {
        string sql = "select typeCode,NOTE5,typeName,parentCode,status,IsLastNode,HigherPerPoint,RebatePoint,NOTE1,NOTE2,NOTE3,NOTE4,DeductionPoint,Row_Number()over(order by typeCode) as crow from T_truckType";
        string sqlCont = "select count(*) from T_truckType";

        public bool Exists(string typeCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" typeCode = @typeCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@typeCode",typeCode)			};

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

        public int Add(T_truckType model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

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
        public int Delete(string typeCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(typeCode, tran);
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
        public int Add(T_truckType model, SqlTransaction tran)
        {

            Delete(model.typeCode, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_truckType(");
            strSql.Append("typeCode,NOTE5,typeName,parentCode,status,IsLastNode,HigherPerPoint,RebatePoint,NOTE1,NOTE2,NOTE3,NOTE4,DeductionPoint");
            strSql.Append(") values (");
            strSql.Append("@typeCode,@NOTE5,@typeName,@parentCode,@status,@IsLastNode,@HigherPerPoint,@RebatePoint,@NOTE1,@NOTE2,@NOTE3,@NOTE4,@DeductionPoint");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@typeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@typeName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@parentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@status", SqlDbType.Char,1) ,            
                        new SqlParameter("@IsLastNode", SqlDbType.Char,1) ,   
                        new SqlParameter("@HigherPerPoint", SqlDbType.Float) ,
                        new SqlParameter("@RebatePoint", SqlDbType.Float) ,   
                        new SqlParameter("@NOTE1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE4", SqlDbType.NVarChar,50),
                        new SqlParameter("@DeductionPoint",SqlDbType.NVarChar,50)
              
            };

            parameters[0].Value = SqlNull(model.typeCode);

            parameters[1].Value = SqlNull(model.NOTE5);

            parameters[2].Value = SqlNull(model.typeName);

            parameters[3].Value = SqlNull(model.parentCode);

            parameters[4].Value = SqlNull(model.status);

            parameters[5].Value = SqlNull(model.IsLastNode);
            parameters[6].Value = SqlNull(model.HigherPerPoint);
            parameters[7].Value = SqlNull(model.RebatePoint);

            parameters[8].Value = SqlNull(model.NOTE1);

            parameters[9].Value = SqlNull(model.NOTE2);

            parameters[10].Value = SqlNull(model.NOTE3);

            parameters[11].Value = SqlNull(model.NOTE4);

            parameters[12].Value = SqlNull(model.DeductionPoint);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        /// <returns></returns>

        public int updatemodel(T_truckType model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update T_truckType set NOTE5=@NOTE5,typeName=@typeName,parentCode=@parentCode,status=@status,
IsLastNode=@IsLastNode,HigherPerPoint=@HigherPerPoint,RebatePoint=@RebatePoint,NOTE1=@NOTE1,NOTE2=@NOTE2,NOTE3=@NOTE3,NOTE4=@NOTE4,DeductionPoint=@DeductionPoint
 where typeCode=@typeCode");

            SqlParameter[] parameters = {
			            new SqlParameter("@typeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@typeName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@parentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@status", SqlDbType.Char,1) ,            
                        new SqlParameter("@IsLastNode", SqlDbType.Char,1) ,   
                        new SqlParameter("@HigherPerPoint", SqlDbType.Float) ,
                        new SqlParameter("@RebatePoint", SqlDbType.Float) ,   
                        new SqlParameter("@NOTE1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE4", SqlDbType.NVarChar,50),
                        new SqlParameter("@DeductionPoint",SqlDbType.NVarChar,50)
              
            };

            parameters[0].Value = SqlNull(model.typeCode);

            parameters[1].Value = SqlNull(model.NOTE5);

            parameters[2].Value = SqlNull(model.typeName);

            parameters[3].Value = SqlNull(model.parentCode);

            parameters[4].Value = SqlNull(model.status);

            parameters[5].Value = SqlNull(model.IsLastNode);
            parameters[6].Value = SqlNull(model.HigherPerPoint);
            parameters[7].Value = SqlNull(model.RebatePoint);

            parameters[8].Value = SqlNull(model.NOTE1);

            parameters[9].Value = SqlNull(model.NOTE2);

            parameters[10].Value = SqlNull(model.NOTE3);

            parameters[11].Value = SqlNull(model.NOTE4);

            parameters[12].Value = SqlNull(model.DeductionPoint);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string typeCode, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_truckType ");
            strSql.Append(" where typeCode=@typeCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@typeCode",typeCode)			};
            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
            return 1;
        }

        public DataTable ReturnAllDt()
        {
            string sqltrck = @"select typeCode,NOTE5,typeName,parentCode,status,IsLastNode,HigherPerPoint,RebatePoint,NOTE1,NOTE2,NOTE3,NOTE4,DeductionPoint,
(case IsLastNode when '1' then '是' when '0' then '否' end)as lastnode,
Row_Number()over(order by typeCode) as crow from T_truckType";

            return returnDt(sqltrck, null, false);
        }

        private DataTable returnDt(string tempsql, SqlParameter[] sps, bool isPro)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, isPro);
            return dt;
        }


        public IList<T_truckType> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = returnDt(tempsql, sps, false);
            IList<T_truckType> list = new List<T_truckType>();
            foreach (DataRow dr in dt.Rows)
            {
                T_truckType model = new T_truckType();
                model.typeCode = dr["typeCode"].ToString();
                model.NOTE5 = dr["NOTE5"].ToString();
                model.typeName = dr["typeName"].ToString();
                model.parentCode = dr["parentCode"].ToString();
                model.status = dr["status"].ToString();
                model.IsLastNode = dr["IsLastNode"].ToString();
                float fHigherPerPoint = 0;
                float.TryParse(dr["HigherPerPoint"].ToString(), out fHigherPerPoint);
                model.HigherPerPoint = fHigherPerPoint;
                float fRebatePoint = 0;
                float.TryParse(dr["RebatePoint"].ToString(), out fRebatePoint);
                model.RebatePoint = fRebatePoint;
                model.NOTE1 = dr["NOTE1"].ToString();
                model.NOTE2 = dr["NOTE2"].ToString();
                model.NOTE3 = dr["NOTE3"].ToString();
                model.NOTE4 = dr["NOTE4"].ToString();
                float fDeductionPoint = 0;
                float.TryParse(dr["DeductionPoint"].ToString(), out fDeductionPoint);
                model.DeductionPoint = fDeductionPoint;

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_truckType GetModel(string typeCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where typeCode=@typeCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@typeCode",typeCode)			};


            T_truckType model = new T_truckType();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.typeCode = dr["typeCode"].ToString();
                    model.NOTE5 = dr["NOTE5"].ToString();
                    model.typeName = dr["typeName"].ToString();
                    model.parentCode = dr["parentCode"].ToString();
                    model.status = dr["status"].ToString();
                    model.IsLastNode = dr["IsLastNode"].ToString();
                    float fHigherPerPoint = 0;
                    float.TryParse(dr["HigherPerPoint"].ToString(), out fHigherPerPoint);
                    model.HigherPerPoint = fHigherPerPoint;
                    float fRebatePoint = 0;
                    float.TryParse(dr["RebatePoint"].ToString(), out fRebatePoint);
                    model.RebatePoint = fRebatePoint;
                    model.NOTE1 = dr["NOTE1"].ToString();
                    model.NOTE2 = dr["NOTE2"].ToString();
                    model.NOTE3 = dr["NOTE3"].ToString();
                    model.NOTE4 = dr["NOTE4"].ToString();
                    float fDeductionPoint = 0;
                    float.TryParse(dr["DeductionPoint"].ToString(), out fDeductionPoint);
                    model.DeductionPoint = fDeductionPoint;

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
        public IList<T_truckType> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_truckType> GetAllList(int beg, int end)
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
        /// <summary>
        /// 返回其所有的子节点
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<T_truckType> GetAllChildren(string strCode)
        {
            string strsql = sql + " where parentCode=@parentCode";
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@parentCode", strCode) };
            return this.ListMaker(strsql, arrSp);
        }
        /// <summary>
        /// 修改父节点是否是子节点的状态
        /// </summary>
        /// <returns></returns>
        public int changeParentStatus(string strCode)
        {
            string strUpSql = "update T_truckType set IsLastNode='0' where typeCode=@parentCode";
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@parentCode", strCode) };
            return DataHelper.ExcuteNonQuery(strUpSql, arrSp, false);
        }
    }
}
