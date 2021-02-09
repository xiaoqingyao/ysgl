using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;

namespace Dal.Bills
{
   public  class T_BillingApplicationDal
    {
       string sql = @"select Code,AttachmentUrl,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,BillMainCode,Note10,TruckCode,SaleDeptCode,AppDate,
    SysPersionCode,SysDateTime,Explain,IsJC,InvoiceCode,BillingDate,BIllingSysTime,DealersName,IsSpApp,Row_Number()over(order by Code) as crow from T_BillingApplication";
        string sqlCont = "select count(*) from T_BillingApplication";

        public bool Exists(string Code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" Code = @Code  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;

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

        public void Add(T_BillingApplication model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.Code, tran);
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
        public int Delete(string Code)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(Code, tran);
                   int row= Delete(Code, tran);
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
        public void Add(T_BillingApplication model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_BillingApplication(");
            strSql.Append("Code,AttachmentUrl,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,BillMainCode,Note10,TruckCode,SaleDeptCode,AppDate,SysPersionCode,SysDateTime,Explain,IsJC,InvoiceCode,BillingDate,BIllingSysTime,DealersName,IsSpApp");
            strSql.Append(") values (");
            strSql.Append("@Code,@AttachmentUrl,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@BillMainCode,@Note10,@TruckCode,@SaleDeptCode,@AppDate,@SysPersionCode,@SysDateTime,@Explain,@IsJC,@InvoiceCode,@BillingDate,@BIllingSysTime,@DealersName,@IsSpApp");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Code", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@AttachmentUrl", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BillMainCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SaleDeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@AppDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SysPersionCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SysDateTime", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Explain", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@IsJC", SqlDbType.Char,1),  
                        new SqlParameter("@InvoiceCode",SqlDbType.NVarChar,50),
                        new SqlParameter("@BillingDate",SqlDbType.Char,10),
                        new SqlParameter("@BIllingSysTime",SqlDbType.NVarChar,30),
                        new SqlParameter("@DealersName",SqlDbType.NVarChar,50),
                        new SqlParameter("@IsSpApp",SqlDbType.Char,1),
              
            };

            parameters[0].Value = SqlNull(model.Code);

            parameters[1].Value = SqlNull(model.AttachmentUrl);

            parameters[2].Value = SqlNull(model.Note1);

            parameters[3].Value = SqlNull(model.Note2);

            parameters[4].Value = SqlNull(model.Note3);

            parameters[5].Value = SqlNull(model.Note4);

            parameters[6].Value = SqlNull(model.Note5);

            parameters[7].Value = SqlNull(model.Note6);

            parameters[8].Value = SqlNull(model.Note7);

            parameters[9].Value = SqlNull(model.Note8);

            parameters[10].Value = SqlNull(model.Note9);

            parameters[11].Value = SqlNull(model.BillMainCode);

            parameters[12].Value = SqlNull(model.Note10);

            parameters[13].Value = SqlNull(model.TruckCode);

            parameters[14].Value = SqlNull(model.SaleDeptCode);

            parameters[15].Value = SqlNull(model.AppDate);

            parameters[16].Value = SqlNull(model.SysPersionCode);

            parameters[17].Value = SqlNull(model.SysDateTime);

            parameters[18].Value = SqlNull(model.Explain);

            parameters[19].Value = SqlNull(model.IsJC);

            parameters[20].Value = SqlNull(model.InvoiceCode);

            parameters[21].Value = SqlNull(model.BillingDate);

            parameters[22].Value = SqlNull(model.BIllingSysTime);

            parameters[23].Value = SqlNull(model.DealersName);

            parameters[24].Value = SqlNull(model.IsSpApp);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Code, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_BillingApplication ");
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }
        public int DeletebyCarCode(string strCarCode, string strOrderCode) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_BillingApplication ");
            strSql.Append(" where TruckCode=@TruckCode and Note2=@Note2");
            SqlParameter[] parameters = {
					new SqlParameter("@TruckCode", SqlDbType.NVarChar,50),
                                        new SqlParameter("@Note2",SqlDbType.NVarChar,50)};
            parameters[0].Value = strCarCode;
            parameters[1].Value = strOrderCode;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);
        }

        public IList<T_BillingApplication> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_BillingApplication> list = new List<T_BillingApplication>();
            foreach (DataRow dr in dt.Rows)
            {
                T_BillingApplication model = new T_BillingApplication();
                model.Code = dr["Code"].ToString();
                model.AttachmentUrl = dr["AttachmentUrl"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.BillMainCode = dr["BillMainCode"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.TruckCode = dr["TruckCode"].ToString();
                model.SaleDeptCode = dr["SaleDeptCode"].ToString();
                model.AppDate = dr["AppDate"].ToString();
                model.SysPersionCode = dr["SysPersionCode"].ToString();
                model.SysDateTime = dr["SysDateTime"].ToString();
                model.Explain = dr["Explain"].ToString();
                model.IsJC = dr["IsJC"].ToString();
                model.InvoiceCode = dr["InvoiceCode"].ToString();
                model.BillingDate = dr["BillingDate"].ToString();
                model.BIllingSysTime = dr["BIllingSysTime"].ToString();
                model.DealersName = dr["DealersName"].ToString();
                model.IsSpApp = dr["IsSpApp"].ToString();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_BillingApplication GetModel(string Code)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            T_BillingApplication model = new T_BillingApplication();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Code = dr["Code"].ToString();
                    model.AttachmentUrl = dr["AttachmentUrl"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.BillMainCode = dr["BillMainCode"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.TruckCode = dr["TruckCode"].ToString();
                    model.SaleDeptCode = dr["SaleDeptCode"].ToString();
                    model.AppDate = dr["AppDate"].ToString();
                    model.SysPersionCode = dr["SysPersionCode"].ToString();
                    model.SysDateTime = dr["SysDateTime"].ToString();
                    model.Explain = dr["Explain"].ToString();
                    model.IsJC = dr["IsJC"].ToString();
                    model.InvoiceCode = dr["InvoiceCode"].ToString();
                    model.BillingDate = dr["BillingDate"].ToString();
                    model.BIllingSysTime = dr["BIllingSysTime"].ToString();
                    model.DealersName = dr["DealersName"].ToString();
                    model.IsSpApp = dr["IsSpApp"].ToString();
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
        public IList<T_BillingApplication> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_BillingApplication> GetAllList(int beg, int end)
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

        public bool Add(Bill_Main main, IList<T_BillingApplication> kpsqLists)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.InsertMain(main,tran);
                    foreach (var i in kpsqLists)
                    {
                        Add(i,tran);
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

        public IList<T_BillingApplication> GetListByCode(string bh)
        {
            string cxsql = sql + " where code=@code ";
            SqlParameter[] paramter = { new SqlParameter("@code",bh) };
            return ListMaker(cxsql,paramter);
        }

        public bool Edit(Bill_Main main, IList<T_BillingApplication> kpsqLists)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(main.BillCode,tran);
                    Delete(main.BillCode,tran);
                    maindal.InsertMain(main, tran);
                    foreach (var i in kpsqLists)
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
        public T_BillingApplication GetModelByTruckCode(string strTruckCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where TruckCode=@TruckCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@TruckCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = strTruckCode;


            T_BillingApplication model = new T_BillingApplication();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Code = dr["Code"].ToString();
                    model.AttachmentUrl = dr["AttachmentUrl"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.BillMainCode = dr["BillMainCode"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.TruckCode = dr["TruckCode"].ToString();
                    model.SaleDeptCode = dr["SaleDeptCode"].ToString();
                    model.AppDate = dr["AppDate"].ToString();
                    model.SysPersionCode = dr["SysPersionCode"].ToString();
                    model.SysDateTime = dr["SysDateTime"].ToString();
                    model.Explain = dr["Explain"].ToString();
                    model.IsJC = dr["IsJC"].ToString();
                    model.InvoiceCode = dr["InvoiceCode"].ToString();
                    model.BillingDate = dr["BillingDate"].ToString();
                    model.BIllingSysTime = dr["BIllingSysTime"].ToString();
                    model.DealersName = dr["DealersName"].ToString();
                    model.IsSpApp = dr["IsSpApp"].ToString();
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
