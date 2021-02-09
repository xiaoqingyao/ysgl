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
    public class ZiChan_WeiXiuShenQingDAL
    {
        string sql = "select MainCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ZiChanCode,WeiXiuTypeCode,YuJiJinE,ShenQingRenCode,JingBanRenCode,ShuoMing,Row_Number()over(order by MainCode) as crow from ZiChan_WeiXiuShenQing";
        string sqlCont = "select count(*) from ZiChan_WeiXiuShenQing";

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

        public int Edit(ZiChan_WeiXiuShenQing model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    int iRel = Edit(model, tran);
                    tran.Commit();
                    return iRel;
                }
                catch
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }
        public int Add(ZiChan_WeiXiuShenQing model)
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
                    return -1;
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
                    int iRel = Delete(MainCode, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(MainCode, tran);
                    tran.Commit();
                    return iRel;
                }
                catch
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }

        public int Deletezb(string MainCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(MainCode, tran);
                    tran.Commit();
                    return iRel;
                }
                catch
                {
                    tran.Rollback();
                    return -1;
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        public int Edit(ZiChan_WeiXiuShenQing model, SqlTransaction tran)
        {
            Delete(model.MainCode, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_WeiXiuShenQing(");
            strSql.Append("MainCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ZiChanCode,WeiXiuTypeCode,YuJiJinE,ShenQingRenCode,JingBanRenCode,ShuoMing");
            strSql.Append(") values (");
            strSql.Append("@MainCode,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@ZiChanCode,@WeiXiuTypeCode,@YuJiJinE,@ShenQingRenCode,@JingBanRenCode,@ShuoMing");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MainCode", SqlDbType.NVarChar,50) ,            
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
                        new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuTypeCode", SqlDbType.VarChar,10) ,            
                        new SqlParameter("@YuJiJinE", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ShenQingRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@JingBanRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ShuoMing", SqlDbType.NVarChar,500)             
              
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

            parameters[11].Value = SqlNull(model.ZiChanCode);

            parameters[12].Value = SqlNull(model.WeiXiuTypeCode);

            parameters[13].Value = SqlNull(model.YuJiJinE);

            parameters[14].Value = SqlNull(model.ShenQingRenCode);

            parameters[15].Value = SqlNull(model.JingBanRenCode);

            parameters[16].Value = SqlNull(model.ShuoMing);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 添加
        /// </summary>
        public int Add(ZiChan_WeiXiuShenQing model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_WeiXiuShenQing(");
            strSql.Append("MainCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ZiChanCode,WeiXiuTypeCode,YuJiJinE,ShenQingRenCode,JingBanRenCode,ShuoMing");
            strSql.Append(") values (");
            strSql.Append("@MainCode,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@ZiChanCode,@WeiXiuTypeCode,@YuJiJinE,@ShenQingRenCode,@JingBanRenCode,@ShuoMing");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MainCode", SqlDbType.NVarChar,50) ,            
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
                        new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuTypeCode", SqlDbType.VarChar,10) ,            
                        new SqlParameter("@YuJiJinE", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ShenQingRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@JingBanRenCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ShuoMing", SqlDbType.NVarChar,500)             
              
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

            parameters[11].Value = SqlNull(model.ZiChanCode);

            parameters[12].Value = SqlNull(model.WeiXiuTypeCode);

            parameters[13].Value = SqlNull(model.YuJiJinE);

            parameters[14].Value = SqlNull(model.ShenQingRenCode);

            parameters[15].Value = SqlNull(model.JingBanRenCode);

            parameters[16].Value = SqlNull(model.ShuoMing);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string MainCode, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_WeiXiuShenQing ");
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = MainCode;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_WeiXiuShenQing> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_WeiXiuShenQing> list = new List<ZiChan_WeiXiuShenQing>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_WeiXiuShenQing model = new ZiChan_WeiXiuShenQing();
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
                model.ZiChanCode = dr["ZiChanCode"].ToString();
                model.WeiXiuTypeCode = dr["WeiXiuTypeCode"].ToString();
                if (!DBNull.Value.Equals(dr["YuJiJinE"]))
                {
                    model.YuJiJinE = decimal.Parse(dr["YuJiJinE"].ToString());
                }
                model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                model.JingBanRenCode = dr["JingBanRenCode"].ToString();
                model.ShuoMing = dr["ShuoMing"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_WeiXiuShenQing GetModel(string MainCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = MainCode;


            ZiChan_WeiXiuShenQing model = new ZiChan_WeiXiuShenQing();

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
                    model.ZiChanCode = dr["ZiChanCode"].ToString();
                    model.WeiXiuTypeCode = dr["WeiXiuTypeCode"].ToString();
                    if (!DBNull.Value.Equals(dr["YuJiJinE"]))
                    {
                        model.YuJiJinE = decimal.Parse(dr["YuJiJinE"].ToString());
                    }
                    model.ShenQingRenCode = dr["ShenQingRenCode"].ToString();
                    model.JingBanRenCode = dr["JingBanRenCode"].ToString();
                    model.ShuoMing = dr["ShuoMing"].ToString();

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
        public IList<ZiChan_WeiXiuShenQing> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_WeiXiuShenQing> GetAllList(int beg, int end)
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

        public IList<ZiChan_WeiXiuShenQing> GetListModel(string strCode)
        {
            string strMysql = sql + " where MainCode='"+strCode+"'";
            return this.ListMaker(strMysql, null);
        }
    }
}
