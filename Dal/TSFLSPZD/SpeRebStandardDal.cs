using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace Dal.TSFLSPZD
{
     public class SpeRebStandardDal
    {
         string sql = "select MarkerCode,Status,Type,SysUserCode,SysTime,Note1,Note2,Note3,Note4,Note5,Note6,AppBillCode,Note7,Note8,Note9,Note10,TruckCode,TruckTypeCode,DeptCode,SaleFeeTypeCode,SaleProcessCode,ControlItemCode,Fee,Row_Number()over(order by NID) as crow from T_SpecialRebatesStandard";
        string sqlCont = "select count(*) from T_SpecialRebatesStandard";

        public bool Exists(long NID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" NID = @NID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.BigInt)
			};
            parameters[0].Value = NID;

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

      

        public int Add(T_SpecialRebatesStandardmodel model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                  int row= Add(model, tran);
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
        public int Delete(string strsqcode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int row = Delete(strsqcode, tran);
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
        public int Add(T_SpecialRebatesStandardmodel model, SqlTransaction tran)
        {
            //Delete(model.NID, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_SpecialRebatesStandard(");
            strSql.Append("MarkerCode,Status,SysUserCode,SysTime,Note1,Note2,Note3,Note4,Note5,Note6,AppBillCode,Note7,Note8,Note9,Note10,TruckCode,TruckTypeCode,DeptCode,SaleFeeTypeCode,SaleProcessCode,ControlItemCode,Fee,[Type]");
            strSql.Append(") values (");
            strSql.Append("@MarkerCode,@Status,@SysUserCode,@SysTime,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@AppBillCode,@Note7,@Note8,@Note9,@Note10,@TruckCode,@TruckTypeCode,@DeptCode,@SaleFeeTypeCode,@SaleProcessCode,@ControlItemCode,@Fee,@Type");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MarkerCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Status", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SysUserCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SysTime", SqlDbType.NVarChar,20) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@AppBillCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckTypeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@DeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SaleFeeTypeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SaleProcessCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlItemCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Fee", SqlDbType.Decimal,9),
                        new SqlParameter("@Type",SqlDbType.Char,1)
              
            };

            parameters[0].Value = SqlNull(model.MarkerCode);

            parameters[1].Value = SqlNull(model.Status);

            parameters[2].Value = SqlNull(model.SysUserCode);

            parameters[3].Value = SqlNull(model.SysTime);

            parameters[4].Value = SqlNull(model.Note1);

            parameters[5].Value = SqlNull(model.Note2);

            parameters[6].Value = SqlNull(model.Note3);

            parameters[7].Value = SqlNull(model.Note4);

            parameters[8].Value = SqlNull(model.Note5);

            parameters[9].Value = SqlNull(model.Note6);

            parameters[10].Value = SqlNull(model.AppBillCode);

            parameters[11].Value = SqlNull(model.Note7);

            parameters[12].Value = SqlNull(model.Note8);

            parameters[13].Value = SqlNull(model.Note9);

            parameters[14].Value = SqlNull(model.Note10);

            parameters[15].Value = SqlNull(model.TruckCode);

            parameters[16].Value = SqlNull(model.TruckTypeCode);

            parameters[17].Value = SqlNull(model.DeptCode);

            parameters[18].Value = SqlNull(model.SaleFeeTypeCode);

            parameters[19].Value = SqlNull(model.SaleProcessCode);

            parameters[20].Value = SqlNull(model.ControlItemCode);

            parameters[21].Value = SqlNull(model.Fee);
            parameters[22].Value = SqlNull(model.Type);

          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }
         /// <summary>
         /// 调用存储过程返回值
         /// </summary>
         /// <param name="strappcode"></param>
         /// <param name="strdeptcode"></param>
         /// <param name="strcarcode"></param>
         /// <returns></returns>
        public DataTable getexpro(string strdeptcode, string strappcode , string strcarcode)
        {

            return DataHelper.GetDataTable("exec Salebill_getDeptFeeType '" + strdeptcode + "', '" + strappcode + "', '" + strcarcode + "'", null, false);

        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string strsqcode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SpecialRebatesStandard ");
            strSql.Append(" where AppBillCode=@strsqcode ");
            SqlParameter[] parameters = {
					new SqlParameter("@strsqcode", strsqcode),
			};
           return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_SpecialRebatesStandardmodel> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_SpecialRebatesStandardmodel> list = new List<T_SpecialRebatesStandardmodel>();
            foreach (DataRow dr in dt.Rows)
            {
                T_SpecialRebatesStandardmodel model = new T_SpecialRebatesStandardmodel();
                if (!DBNull.Value.Equals(dr["NID"]))
                {
                    model.NID = long.Parse(dr["NID"].ToString());
                }
                model.MarkerCode = dr["MarkerCode"].ToString();
                model.Status = dr["Status"].ToString();
                model.Type = dr["Type"].ToString();
                model.SysUserCode = dr["SysUserCode"].ToString();
                model.SysTime = dr["SysTime"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.AppBillCode = dr["AppBillCode"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.TruckCode = dr["TruckCode"].ToString();
                model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                model.DeptCode = dr["DeptCode"].ToString();
                model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();
                model.SaleProcessCode = dr["SaleProcessCode"].ToString();
                model.ControlItemCode = dr["ControlItemCode"].ToString();
                if (!DBNull.Value.Equals(dr["Fee"]))
                {
                    model.Fee = decimal.Parse(dr["Fee"].ToString());
                }

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_SpecialRebatesStandardmodel GetModel(long NID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where NID=@NID");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.BigInt)
			};
            parameters[0].Value = NID;


            T_SpecialRebatesStandardmodel model = new T_SpecialRebatesStandardmodel();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["NID"]))
                    {
                        model.NID = long.Parse(dr["NID"].ToString());
                    }
                    model.MarkerCode = dr["MarkerCode"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.Type = dr["Type"].ToString();
                    model.SysUserCode = dr["SysUserCode"].ToString();
                    model.SysTime = dr["SysTime"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.AppBillCode = dr["AppBillCode"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.TruckCode = dr["TruckCode"].ToString();
                    model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                    model.DeptCode = dr["DeptCode"].ToString();
                    model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();
                    model.SaleProcessCode = dr["SaleProcessCode"].ToString();
                    model.ControlItemCode = dr["ControlItemCode"].ToString();
                    if (!DBNull.Value.Equals(dr["Fee"]))
                    {
                        model.Fee = decimal.Parse(dr["Fee"].ToString());
                    }

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
        public IList<T_SpecialRebatesStandardmodel> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_SpecialRebatesStandardmodel> GetAllList(int beg, int end)
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
