using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;
using Dal.Bills;

namespace Dal.Zichan
{
    public class CaiGouShenQingDal
    {
        string sql = "select MainCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ShenQingRenCode,JingBanRenCode,ZaiChanName,ShenQingDate,CaiGouShuoMing,JiaGe,ShuLiang,GuiGe,Row_Number()over(order by MainCode) as crow from ZiChan_CaiGouShenQing";
        string sqlCont = "select count(*) from ZiChan_CaiGouShenQing";

        public bool Exists(string MainCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" MainCode = @MainCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
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
        public IList<ZiChan_CaiGouShenQing> GetListByCode(string bh)
        {
            string cxsql = sql + " where MainCode=@MainCode ";
            SqlParameter[] paramter = { new SqlParameter("@MainCode", bh) };
            return ListMaker(cxsql, paramter);
        }

        public bool Add(Bill_Main main, IList<ZiChan_CaiGouShenQing> cgsqLists)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.InsertMain(main, tran);
                    foreach (var i in cgsqLists)
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

        public bool Edit(Bill_Main main, ZiChan_CaiGouShenQing sqmodel)
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
                    Add(sqmodel,tran);
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
        public void Add(ZiChan_CaiGouShenQing model)
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
        public int Delete(Bill_Main main,string MainCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                  
                   MainDal maindal = new MainDal();
                    maindal.DeleteMain(main.BillCode, tran);
                   int intRow = Delete(MainCode, tran);
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
        public void Add(ZiChan_CaiGouShenQing model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_CaiGouShenQing(");
            strSql.Append("MainCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ShenQingRenCode,JingBanRenCode,ZaiChanName,ShenQingDate,CaiGouShuoMing,JiaGe,ShuLiang,GuiGe");
            strSql.Append(") values (");
            strSql.Append("@MainCode,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@ShenQingRenCode,@JingBanRenCode,@ZaiChanName,@ShenQingDate,@CaiGouShuoMing,@JiaGe,@ShuLiang,@GuiGe");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MainCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShenQingRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@JingBanRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ZaiChanName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShenQingDate", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@CaiGouShuoMing", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@JiaGe", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ShuLiang", SqlDbType.Int,4) ,            
                        new SqlParameter("@GuiGe", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.MainCode);

            parameters[1].Value = SqlNull(model.Note1);

            parameters[2].Value = SqlNull(model.Note2);

            parameters[3].Value = SqlNull(model.Note3);

            parameters[4].Value = SqlNull(model.Note4);

            parameters[5].Value = SqlNull(model.Note5);

            parameters[6].Value = SqlNull(model.Note6);

            parameters[7].Value = SqlNull(model.Note7);

            parameters[8].Value = SqlNull(model.Note8);

            parameters[9].Value = SqlNull(model.Note9);

            parameters[10].Value = SqlNull(model.Note10);

            parameters[11].Value = SqlNull(model.ShenQingRenCode);

            parameters[12].Value = SqlNull(model.JingBanRenCode);

            parameters[13].Value = SqlNull(model.ZaiChanName);

            parameters[14].Value = SqlNull(model.ShenQingDate);

            parameters[15].Value = SqlNull(model.CaiGouShuoMing);

            parameters[16].Value = SqlNull(model.JiaGe);

            parameters[17].Value = SqlNull(model.ShuLiang);

            parameters[18].Value = SqlNull(model.GuiGe);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string MainCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_CaiGouShenQing ");
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = MainCode;


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_CaiGouShenQing> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_CaiGouShenQing> list = new List<ZiChan_CaiGouShenQing>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_CaiGouShenQing model = new ZiChan_CaiGouShenQing();
                model.MainCode = dr["MainCode"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                model.JingBanRenCode = dr["JingBanRenCode"].ToString();
                model.ZaiChanName = dr["ZaiChanName"].ToString();
                model.ShenQingDate = dr["ShenQingDate"].ToString();
                model.CaiGouShuoMing = dr["CaiGouShuoMing"].ToString();
                if (!DBNull.Value.Equals(dr["JiaGe"]))
                {
                    model.JiaGe = decimal.Parse(dr["JiaGe"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ShuLiang"]))
                {
                    model.ShuLiang = int.Parse(dr["ShuLiang"].ToString());
                }
                model.GuiGe = dr["GuiGe"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_CaiGouShenQing GetModel(string MainCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = MainCode;


            ZiChan_CaiGouShenQing model = new ZiChan_CaiGouShenQing();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.MainCode = dr["MainCode"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                    model.JingBanRenCode = dr["JingBanRenCode"].ToString();
                    model.ZaiChanName = dr["ZaiChanName"].ToString();
                    model.ShenQingDate = dr["ShenQingDate"].ToString();
                    model.CaiGouShuoMing = dr["CaiGouShuoMing"].ToString();
                    if (!DBNull.Value.Equals(dr["JiaGe"]))
                    {
                        model.JiaGe = decimal.Parse(dr["JiaGe"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ShuLiang"]))
                    {
                        model.ShuLiang = int.Parse(dr["ShuLiang"].ToString());
                    }
                    model.GuiGe = dr["GuiGe"].ToString();

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
        public IList<ZiChan_CaiGouShenQing> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_CaiGouShenQing> GetAllList(int beg, int end)
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
