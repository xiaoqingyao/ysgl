using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.Zichan
{
    /// <summary>
    /// 资产类别
    /// </summary>
    public class ZiChan_LeibieDal
    {
        string sql = "select LeibieCode,Note4,Note5,Note6,Note7,Note8,Note9,Note10,LeibieName,ShiYongQiXian,JiLiangDanWei,ParentCode,BeiZhu,Note1,Note2,Note3,Row_Number()over(order by LeibieCode) as crow from ZiChan_Leibie";
        string sqlCont = "select count(*) from ZiChan_Leibie";

        public bool Exists(string LeibieCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" LeibieCode = @LeibieCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeibieCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = LeibieCode;

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


        public int  Add(ZiChan_Leibie model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    //Delete(model.LeibieCode, tran);
                    int IntRow= Add(model, tran);
                    tran.Commit();
                    return IntRow;
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
        public int Delete(string LeibieCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                   int rowint= Delete(LeibieCode, tran);
                    tran.Commit();
                    return rowint;
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
        public int Add(ZiChan_Leibie model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_Leibie(");
            strSql.Append("LeibieCode,Note4,Note5,Note6,Note7,Note8,Note9,Note10,LeibieName,ShiYongQiXian,JiLiangDanWei,ParentCode,BeiZhu,Note1,Note2,Note3");
            strSql.Append(") values (");
            strSql.Append("@LeibieCode,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@LeibieName,@ShiYongQiXian,@JiLiangDanWei,@ParentCode,@BeiZhu,@Note1,@Note2,@Note3");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@LeibieCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LeibieName", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@ShiYongQiXian", SqlDbType.Int,4) ,            
                        new SqlParameter("@JiLiangDanWei", SqlDbType.NVarChar,10) ,            
                        new SqlParameter("@ParentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BeiZhu", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.LeibieCode);

            parameters[1].Value = SqlNull(model.Note4);

            parameters[2].Value = SqlNull(model.Note5);

            parameters[3].Value = SqlNull(model.Note6);

            parameters[4].Value = SqlNull(model.Note7);

            parameters[5].Value = SqlNull(model.Note8);

            parameters[6].Value = SqlNull(model.Note9);

            parameters[7].Value = SqlNull(model.Note10);

            parameters[8].Value = SqlNull(model.LeibieName);

            parameters[9].Value = SqlNull(model.ShiYongQiXian);

            parameters[10].Value = SqlNull(model.JiLiangDanWei);

            parameters[11].Value = SqlNull(model.ParentCode);

            parameters[12].Value = SqlNull(model.BeiZhu);

            parameters[13].Value = SqlNull(model.Note1);

            parameters[14].Value = SqlNull(model.Note2);

            parameters[15].Value = SqlNull(model.Note3);


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string LeibieCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_Leibie ");
            strSql.Append(" where LeibieCode=@LeibieCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeibieCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = LeibieCode;


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_Leibie> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_Leibie> list = new List<ZiChan_Leibie>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_Leibie model = new ZiChan_Leibie();
                model.LeibieCode = dr["LeibieCode"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.LeibieName = dr["LeibieName"].ToString();
                if (!DBNull.Value.Equals(dr["ShiYongQiXian"]))
                {
                    model.ShiYongQiXian = int.Parse(dr["ShiYongQiXian"].ToString());
                }
                model.JiLiangDanWei = dr["JiLiangDanWei"].ToString();
                model.ParentCode = dr["ParentCode"].ToString();
                model.BeiZhu = dr["BeiZhu"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_Leibie GetModel(string LeibieCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where LeibieCode=@LeibieCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@LeibieCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = LeibieCode;


            ZiChan_Leibie model = new ZiChan_Leibie();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.LeibieCode = dr["LeibieCode"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.LeibieName = dr["LeibieName"].ToString();
                    if (!DBNull.Value.Equals(dr["ShiYongQiXian"]))
                    {
                        model.ShiYongQiXian = int.Parse(dr["ShiYongQiXian"].ToString());
                    }
                    model.JiLiangDanWei = dr["JiLiangDanWei"].ToString();
                    model.ParentCode = dr["ParentCode"].ToString();
                    model.BeiZhu = dr["BeiZhu"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();

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
        public IList<ZiChan_Leibie> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_Leibie> GetAllList(int beg, int end)
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
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public int updatemodel(ZiChan_Leibie model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"update ZiChan_Leibie set LeibieName=@LeibieName,ShiYongQiXian=@ShiYongQiXian,JiLiangDanWei=@JiLiangDanWei,ParentCode=@ParentCode,
BeiZhu=@BeiZhu,Note1=@Note1,Note2=@Note2,Note3=@Note3,Note4=@Note4,Note5=@Note5,Note6=@Note6,Note7=@Note7,Note8=@Note8,Note9=@Note9,Note10=@Note10
 where LeibieCode=@LeibieCode");

            SqlParameter[] parameters = {
			            new SqlParameter("@LeibieCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LeibieName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShiYongQiXian", SqlDbType.Int,4) ,            
                        new SqlParameter("@JiLiangDanWei", SqlDbType.NVarChar,10) ,            
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

            parameters[0].Value = SqlNull(model.LeibieCode);

            parameters[1].Value = SqlNull(model.LeibieName);

            parameters[2].Value = SqlNull(model.ShiYongQiXian);

            parameters[3].Value = SqlNull(model.JiLiangDanWei);

            parameters[4].Value = SqlNull(model.ParentCode);

            parameters[5].Value = SqlNull(model.BeiZhu);
            parameters[6].Value = SqlNull(model.Note1);
            parameters[7].Value = SqlNull(model.Note2);

            parameters[8].Value = SqlNull(model.Note3);

            parameters[9].Value = SqlNull(model.Note4);

            parameters[10].Value = SqlNull(model.Note5);

            parameters[11].Value = SqlNull(model.Note6);

            parameters[12].Value = SqlNull(model.Note7);
            parameters[13].Value = SqlNull(model.Note8);

            parameters[14].Value = SqlNull(model.Note9);

            parameters[15].Value = SqlNull(model.Note10);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);

        }


        /// <summary>
        /// 返回其所有的子节点
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<ZiChan_Leibie> GetAllChildren(string strCode)
        {
            string strsql = sql + " where ParentCode=@parentCode";
            SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@parentCode", strCode) };
            return this.ListMaker(strsql, arrSp);
        }
        /// <summary>
        /// 修改父节点是否是子节点的状态
        /// </summary>
        /// <returns></returns>
        //public int changeParentStatus(string strCode)
        //{
        //    string strUpSql = "update ZiChan_Leibie set IsLastNode='0' where typeCode=@parentCode";
        //    SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@parentCode", strCode) };
        //    return DataHelper.ExcuteNonQuery(strUpSql, arrSp, false);
        //}

    }
}
