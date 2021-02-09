using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using Dal.Bills;

namespace Dal.Zichan
{
    public class ZiChan_ChuZhiDanDal
    {
        string sql = "select MainCode,JingBanRen,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,ZiChanCode,Note10,BianDongType,Qian,Hou,ShuoMing,BianDongDate,ShenQingRenCode,ShenQingDate,Row_Number()over(order by MainCode) as crow from ZiChan_ChuZhiDan";
        string sqlCont = "select count(*) from ZiChan_ChuZhiDan";

        public bool Exists(string MainCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" MainCode = @MainCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = MainCode;

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

        public void Add(ZiChan_ChuZhiDan model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.MainCode, tran);
                    Add(model, tran);
                    tran.Commit();
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
        public int Delete(string MainCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(MainCode, tran);
                    int intRows = Delete(MainCode, tran);
                    tran.Commit();
                    return intRows;
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
        public void Add(ZiChan_ChuZhiDan model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_ChuZhiDan(");
            strSql.Append("MainCode,JingBanRen,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,ZiChanCode,Note10,BianDongType,Qian,Hou,ShuoMing,BianDongDate,ShenQingRenCode,ShenQingDate");
            strSql.Append(") values (");
            strSql.Append("@MainCode,@JingBanRen,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@ZiChanCode,@Note10,@BianDongType,@Qian,@Hou,@ShuoMing,@BianDongDate,@ShenQingRenCode,@ShenQingDate");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MainCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@JingBanRen", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BianDongType", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Qian", SqlDbType.NVarChar,300) ,            
                        new SqlParameter("@Hou", SqlDbType.NVarChar,300) ,            
                        new SqlParameter("@ShuoMing", SqlDbType.NVarChar,300) ,            
                        new SqlParameter("@BianDongDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShenQingRenCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShenQingDate", SqlDbType.NVarChar,30)             
              
            };

            parameters[0].Value = SqlNull(model.MainCode);

            parameters[1].Value = SqlNull(model.JingBanRen);

            parameters[2].Value = SqlNull(model.Note1);

            parameters[3].Value = SqlNull(model.Note2);

            parameters[4].Value = SqlNull(model.Note3);

            parameters[5].Value = SqlNull(model.Note4);

            parameters[6].Value = SqlNull(model.Note5);

            parameters[7].Value = SqlNull(model.Note6);

            parameters[8].Value = SqlNull(model.Note7);

            parameters[9].Value = SqlNull(model.Note8);

            parameters[10].Value = SqlNull(model.Note9);

            parameters[11].Value = SqlNull(model.ZiChanCode);

            parameters[12].Value = SqlNull(model.Note10);

            parameters[13].Value = SqlNull(model.BianDongType);

            parameters[14].Value = SqlNull(model.Qian);

            parameters[15].Value = SqlNull(model.Hou);

            parameters[16].Value = SqlNull(model.ShuoMing);

            parameters[17].Value = SqlNull(model.BianDongDate);

            parameters[18].Value = SqlNull(model.ShenQingRenCode);

            parameters[19].Value = SqlNull(model.ShenQingDate);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string MainCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_ChuZhiDan ");
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = MainCode;


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_ChuZhiDan> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_ChuZhiDan> list = new List<ZiChan_ChuZhiDan>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_ChuZhiDan model = new ZiChan_ChuZhiDan();
                model.MainCode = dr["MainCode"].ToString();
                model.JingBanRen = dr["JingBanRen"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.ZiChanCode = dr["ZiChanCode"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.BianDongType = dr["BianDongType"].ToString();
                model.Qian = dr["Qian"].ToString();
                model.Hou = dr["Hou"].ToString();
                model.ShuoMing = dr["ShuoMing"].ToString();
                model.BianDongDate = dr["BianDongDate"].ToString();
                model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                model.ShenQingDate = dr["ShenQingDate"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_ChuZhiDan GetModel(string MainCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = MainCode;


            ZiChan_ChuZhiDan model = new ZiChan_ChuZhiDan();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.MainCode = dr["MainCode"].ToString();
                    model.JingBanRen = dr["JingBanRen"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.ZiChanCode = dr["ZiChanCode"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.BianDongType = dr["BianDongType"].ToString();
                    model.Qian = dr["Qian"].ToString();
                    model.Hou = dr["Hou"].ToString();
                    model.ShuoMing = dr["ShuoMing"].ToString();
                    model.BianDongDate = dr["BianDongDate"].ToString();
                    model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                    model.ShenQingDate = dr["ShenQingDate"].ToString();

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
        public IList<ZiChan_ChuZhiDan> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_ChuZhiDan> GetAllList(int beg, int end)
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


        public bool Add(Bill_Main main, IList<ZiChan_ChuZhiDan> czLists)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.InsertMain(main, tran);
                    foreach (var i in czLists)
                    {
                        Add(i, tran);
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

        public IList<ZiChan_ChuZhiDan> GetListByCode(string bh)
        {
            string cxsql = sql + " where MainCode=@MainCode ";
            SqlParameter[] paramter = { new SqlParameter("@MainCode", bh) };
            return ListMaker(cxsql, paramter);
        }

        public bool Edit(Bill_Main main, ZiChan_ChuZhiDan sqmodel)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(main.BillCode, tran);
                    Delete(main.BillCode, tran);
                    maindal.InsertMain(main, tran);
                    Add(sqmodel, tran);
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
    }
}
