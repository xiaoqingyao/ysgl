using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.Zichan
{
    public class ZiChan_ZengJianFangShiDal
    {
        string sql = "select FangshiCode,Note6,Note7,Note8,Note9,Note10,Fangshiname,ZjType,BeiZhu,Note1,Note2,Note3,Note4,Note5,Row_Number()over(order by FangshiCode) as crow from ZiChan_ZengJianFangShi";
        string sqlCont = "select count(*) from ZiChan_ZengJianFangShi";

        public bool Exists(string FangshiCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" FangshiCode = @FangshiCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@FangshiCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = FangshiCode;

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

        public int Add(ZiChan_ZengJianFangShi model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.FangshiCode, tran);
                    int introw = Add(model, tran);
                    tran.Commit();
                    return introw;
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
        public int Delete(string FangshiCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                   int intRow= Delete(FangshiCode, tran);
                    tran.Commit();
                    return intRow;
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
        public int Add(ZiChan_ZengJianFangShi model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_ZengJianFangShi(");
            strSql.Append("FangshiCode,Note6,Note7,Note8,Note9,Note10,Fangshiname,ZjType,BeiZhu,Note1,Note2,Note3,Note4,Note5");
            strSql.Append(") values (");
            strSql.Append("@FangshiCode,@Note6,@Note7,@Note8,@Note9,@Note10,@Fangshiname,@ZjType,@BeiZhu,@Note1,@Note2,@Note3,@Note4,@Note5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@FangshiCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Fangshiname", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZjType", SqlDbType.Char,1) ,            
                        new SqlParameter("@BeiZhu", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.FangshiCode);

            parameters[1].Value = SqlNull(model.Note6);

            parameters[2].Value = SqlNull(model.Note7);

            parameters[3].Value = SqlNull(model.Note8);

            parameters[4].Value = SqlNull(model.Note9);

            parameters[5].Value = SqlNull(model.Note10);

            parameters[6].Value = SqlNull(model.Fangshiname);

            parameters[7].Value = SqlNull(model.ZjType);

            parameters[8].Value = SqlNull(model.BeiZhu);

            parameters[9].Value = SqlNull(model.Note1);

            parameters[10].Value = SqlNull(model.Note2);

            parameters[11].Value = SqlNull(model.Note3);

            parameters[12].Value = SqlNull(model.Note4);

            parameters[13].Value = SqlNull(model.Note5);


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }



        public DataTable GetAllListBySql1(ZiChan_ZengJianFangShi model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select a.*,(case when ZjType='0' then '减' when ZjType='1' then '增' end ) as zjfs from ZiChan_ZengJianFangShi a where 1=1");

            if (model.FangshiCode!=null&&model.FangshiCode!="")
            {
                 strSql.Append(" and a.FangshiCode like'%" + model.FangshiCode + "%'");
            }
            if (model.Fangshiname != null && model.Fangshiname != "")
            {
                strSql.Append(" and a.Fangshiname like'%" + model.Fangshiname + "%'");
            }
            if (model.ZjType != null && model.ZjType != "")
            {
                strSql.Append(" and a.ZjType like'%" + model.ZjType + "%'");
            }
          
            return DataHelper.GetDataTable(strSql.ToString(), null, false);

        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string FangshiCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_ZengJianFangShi ");
            strSql.Append(" where FangshiCode=@FangshiCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@FangshiCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = FangshiCode;


          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_ZengJianFangShi> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_ZengJianFangShi> list = new List<ZiChan_ZengJianFangShi>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_ZengJianFangShi model = new ZiChan_ZengJianFangShi();
                model.FangshiCode = dr["FangshiCode"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.Fangshiname = dr["Fangshiname"].ToString();
                model.ZjType = dr["ZjType"].ToString();
                model.BeiZhu = dr["BeiZhu"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_ZengJianFangShi GetModel(string FangshiCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where FangshiCode=@FangshiCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@FangshiCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = FangshiCode;


            ZiChan_ZengJianFangShi model = new ZiChan_ZengJianFangShi();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.FangshiCode = dr["FangshiCode"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.Fangshiname = dr["Fangshiname"].ToString();
                    model.ZjType = dr["ZjType"].ToString();
                    model.BeiZhu = dr["BeiZhu"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public bool Addlistmodel(IList<ZiChan_ZengJianFangShi> modellist)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in modellist)
                    {
                        int row = Add(i, tran);
                    }

                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
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
        public IList<ZiChan_ZengJianFangShi> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_ZengJianFangShi> GetAllList(int beg, int end)
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
