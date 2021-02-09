using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.ReportApplication
{
    /// <summary>
    /// 报告申请单
    /// </summary>
    public class ReportApplicationDal
    {
        string sql = "select ReportAppCode,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,NOTE7,NOTE8,NOTE9,NOTE0,ReportName,ReportNameCode,ReportDeptCode,ReportDeptName,ReportDate,ReportExplain,ReportRemark,NOTE1,Row_Number()over(order by ReportAppCode) as crow from T_ReportApplication";
        string sqlCont = "select count(*) from T_ReportApplication";

        public bool Exists(string ReportAppCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" ReportAppCode = @ReportAppCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@ReportAppCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ReportAppCode;

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

        public int Add(T_ReportApplication model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                  int row=Add(model, tran);
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
        public int Delete(string ReportAppCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                   int row= Delete(ReportAppCode, tran);
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
        public int Add(T_ReportApplication model, SqlTransaction tran)
        {
            Delete(model.ReportAppCode, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_ReportApplication(");
            strSql.Append("ReportAppCode,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,NOTE7,NOTE8,NOTE9,NOTE0,ReportName,ReportNameCode,ReportDeptCode,ReportDeptName,ReportDate,ReportExplain,ReportRemark,NOTE1");
            strSql.Append(") values (");
            strSql.Append("@ReportAppCode,@NOTE2,@NOTE3,@NOTE4,@NOTE5,@NOTE6,@NOTE7,@NOTE8,@NOTE9,@NOTE0,@ReportName,@ReportNameCode,@ReportDeptCode,@ReportDeptName,@ReportDate,@ReportExplain,@ReportRemark,@NOTE1");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@ReportAppCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE0", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportNameCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportDeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportDeptName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportExplain", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ReportRemark", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE1", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.ReportAppCode);

            parameters[1].Value = SqlNull(model.NOTE2);

            parameters[2].Value = SqlNull(model.NOTE3);

            parameters[3].Value = SqlNull(model.NOTE4);

            parameters[4].Value = SqlNull(model.NOTE5);

            parameters[5].Value = SqlNull(model.NOTE6);

            parameters[6].Value = SqlNull(model.NOTE7);

            parameters[7].Value = SqlNull(model.NOTE8);

            parameters[8].Value = SqlNull(model.NOTE9);

            parameters[9].Value = SqlNull(model.NOTE0);

            parameters[10].Value = SqlNull(model.ReportName);

            parameters[11].Value = SqlNull(model.ReportNameCode);

            parameters[12].Value = SqlNull(model.ReportDeptCode);

            parameters[13].Value = SqlNull(model.ReportDeptName);

            parameters[14].Value = SqlNull(model.ReportDate);

            parameters[15].Value = SqlNull(model.ReportExplain);

            parameters[16].Value = SqlNull(model.ReportRemark);

            parameters[17].Value = SqlNull(model.NOTE1);


          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string ReportAppCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_ReportApplication ");
            strSql.Append(" where ReportAppCode=@ReportAppCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ReportAppCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ReportAppCode;

          
          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_ReportApplication> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_ReportApplication> list = new List<T_ReportApplication>();
            foreach (DataRow dr in dt.Rows)
            {
                T_ReportApplication model = new T_ReportApplication();
                model.ReportAppCode = dr["ReportAppCode"].ToString();
                model.NOTE2 = dr["NOTE2"].ToString();
                model.NOTE3 = dr["NOTE3"].ToString();
                model.NOTE4 = dr["NOTE4"].ToString();
                model.NOTE5 = dr["NOTE5"].ToString();
                model.NOTE6 = dr["NOTE6"].ToString();
                model.NOTE7 = dr["NOTE7"].ToString();
                model.NOTE8 = dr["NOTE8"].ToString();
                model.NOTE9 = dr["NOTE9"].ToString();
                model.NOTE0 = dr["NOTE0"].ToString();
                model.ReportName = dr["ReportName"].ToString();
                model.ReportNameCode = dr["ReportNameCode"].ToString();
                model.ReportDeptCode = dr["ReportDeptCode"].ToString();
                model.ReportDeptName = dr["ReportDeptName"].ToString();
                model.ReportDate = dr["ReportDate"].ToString();
                model.ReportExplain = dr["ReportExplain"].ToString();
                model.ReportRemark = dr["ReportRemark"].ToString();
                model.NOTE1 = dr["NOTE1"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_ReportApplication GetModel(string ReportAppCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where ReportAppCode=@ReportAppCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ReportAppCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ReportAppCode;


            T_ReportApplication model = new T_ReportApplication();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.ReportAppCode = dr["ReportAppCode"].ToString();
                    model.NOTE2 = dr["NOTE2"].ToString();
                    model.NOTE3 = dr["NOTE3"].ToString();
                    model.NOTE4 = dr["NOTE4"].ToString();
                    model.NOTE5 = dr["NOTE5"].ToString();
                    model.NOTE6 = dr["NOTE6"].ToString();
                    model.NOTE7 = dr["NOTE7"].ToString();
                    model.NOTE8 = dr["NOTE8"].ToString();
                    model.NOTE9 = dr["NOTE9"].ToString();
                    model.NOTE0 = dr["NOTE0"].ToString();
                    model.ReportName = dr["ReportName"].ToString();
                    model.ReportNameCode = dr["ReportNameCode"].ToString();
                    model.ReportDeptCode = dr["ReportDeptCode"].ToString();
                    model.ReportDeptName = dr["ReportDeptName"].ToString();
                    model.ReportDate = dr["ReportDate"].ToString();
                    model.ReportExplain = dr["ReportExplain"].ToString();
                    model.ReportRemark = dr["ReportRemark"].ToString();
                    model.NOTE1 = dr["NOTE1"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public DataTable getallmode(T_ReportApplication model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select a.* ,('['+ReportNameCode+']'+ReportName)as username,('['+ReportDeptCode+']'+ReportDeptName)as deptnames from dbo.T_ReportApplication a where 1=1");

            //////申请单号
            //if (model.Billcode != null && model.Billcode != "")
            //{
            //    strSql.Append(" and a.Billcode like '%" + model.Billcode + "%' ");
            //}
            strSql.Append(" order by ReportDate desc");

            return DataHelper.GetDataTable(strSql.ToString(), null, false);
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
        public IList<T_ReportApplication> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_ReportApplication> GetAllList(int beg, int end)
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
