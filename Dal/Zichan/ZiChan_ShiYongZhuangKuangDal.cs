using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Models;
using System.Data.SqlClient;

namespace Dal.Zichan
{
    public class ZiChan_ShiYongZhuangKuangDal
    {
        string sql = "select ZhuangKuangCode,Note6,Note7,Note8,Note9,Note10,ZhuangKuangName,ParentCode,BeiZhu,Note1,Note2,Note3,Note4,Note5,Row_Number()over(order by ZhuangKuangCode) as crow from ZiChan_ShiYongZhuangKuang";
        string sqlCont = "select count(*) from ZiChan_ShiYongZhuangKuang";

        public bool Exists(string ZhuangKuangCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" ZhuangKuangCode = @ZhuangKuangCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZhuangKuangCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZhuangKuangCode;

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

        public int Add(ZiChan_ShiYongZhuangKuang model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.ZhuangKuangCode, tran);
                    int intRow = Add(model, tran);
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
        private DataTable returnDt(string tempsql, SqlParameter[] sps, bool isPro)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, isPro);
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable ReturnAllDt()
        {
            return returnDt(sql, null, false);
        }
        /// <summary>
        /// 返回其所有的子节点
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<ZiChan_ShiYongZhuangKuang> GetAllChildren(string strCode)
        {
            string strsql = sql + " where ParentCode=@parentCode";
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@parentCode", strCode) };
            return this.ListMaker(strsql, arrSp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int updatemodel(ZiChan_ShiYongZhuangKuang model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ZiChan_ShiYongZhuangKuang set ZhuangKuangName=@ZhuangKuangName,ParentCode=@ParentCode,
BeiZhu=@BeiZhu,Note1=@Note1,Note2=@Note2,Note3=@Note3,Note4=@Note4,Note5=@Note5,Note6=@Note6,Note7=@Note7,Note8=@Note8,Note9=@Note9,Note10=@Note10
 where ZhuangKuangCode=@ZhuangKuangCode");

            SqlParameter[] parameters = {
			            new SqlParameter("@ZhuangKuangCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZhuangKuangName", SqlDbType.NVarChar,50) ,                
                        new SqlParameter("@ParentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BeiZhu", SqlDbType.NVarChar,100) ,   
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) 
              
            };

            parameters[0].Value = SqlNull(model.ZhuangKuangCode);

            parameters[1].Value = SqlNull(model.ZhuangKuangName);
            parameters[2].Value = SqlNull(model.ParentCode);

            parameters[3].Value = SqlNull(model.BeiZhu);
            parameters[4].Value = SqlNull(model.Note1);
            parameters[5].Value = SqlNull(model.Note2);

            parameters[6].Value = SqlNull(model.Note3);

            parameters[7].Value = SqlNull(model.Note4);

            parameters[8].Value = SqlNull(model.Note5);

            parameters[9].Value = SqlNull(model.Note6);

            parameters[10].Value = SqlNull(model.Note7);
            parameters[11].Value = SqlNull(model.Note8);

            parameters[12].Value = SqlNull(model.Note9);

            parameters[13].Value = SqlNull(model.Note10);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);

        }

      
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string ZhuangKuangCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int intRow = Delete(ZhuangKuangCode, tran);
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
        public int Add(ZiChan_ShiYongZhuangKuang model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_ShiYongZhuangKuang(");
            strSql.Append("ZhuangKuangCode,Note6,Note7,Note8,Note9,Note10,ZhuangKuangName,ParentCode,BeiZhu,Note1,Note2,Note3,Note4,Note5");
            strSql.Append(") values (");
            strSql.Append("@ZhuangKuangCode,@Note6,@Note7,@Note8,@Note9,@Note10,@ZhuangKuangName,@ParentCode,@BeiZhu,@Note1,@Note2,@Note3,@Note4,@Note5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@ZhuangKuangCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZhuangKuangName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ParentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BeiZhu", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.ZhuangKuangCode);

            parameters[1].Value = SqlNull(model.Note6);

            parameters[2].Value = SqlNull(model.Note7);

            parameters[3].Value = SqlNull(model.Note8);

            parameters[4].Value = SqlNull(model.Note9);

            parameters[5].Value = SqlNull(model.Note10);

            parameters[6].Value = SqlNull(model.ZhuangKuangName);

            parameters[7].Value = SqlNull(model.ParentCode);

            parameters[8].Value = SqlNull(model.BeiZhu);

            parameters[9].Value = SqlNull(model.Note1);

            parameters[10].Value = SqlNull(model.Note2);

            parameters[11].Value = SqlNull(model.Note3);

            parameters[12].Value = SqlNull(model.Note4);

            parameters[13].Value = SqlNull(model.Note5);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string ZhuangKuangCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_ShiYongZhuangKuang ");
            strSql.Append(" where ZhuangKuangCode=@ZhuangKuangCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZhuangKuangCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZhuangKuangCode;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_ShiYongZhuangKuang> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_ShiYongZhuangKuang> list = new List<ZiChan_ShiYongZhuangKuang>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_ShiYongZhuangKuang model = new ZiChan_ShiYongZhuangKuang();
                model.ZhuangKuangCode = dr["ZhuangKuangCode"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.ZhuangKuangName = dr["ZhuangKuangName"].ToString();
                model.ParentCode = dr["ParentCode"].ToString();
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
        public ZiChan_ShiYongZhuangKuang GetModel(string ZhuangKuangCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where ZhuangKuangCode=@ZhuangKuangCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZhuangKuangCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZhuangKuangCode;


            ZiChan_ShiYongZhuangKuang model = new ZiChan_ShiYongZhuangKuang();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.ZhuangKuangCode = dr["ZhuangKuangCode"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.ZhuangKuangName = dr["ZhuangKuangName"].ToString();
                    model.ParentCode = dr["ParentCode"].ToString();
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
        public IList<ZiChan_ShiYongZhuangKuang> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_ShiYongZhuangKuang> GetAllList(int beg, int end)
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
