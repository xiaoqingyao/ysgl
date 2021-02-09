using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.Bills
{
    public class bill_travelReportDal
    {
        string sql = "select MainCode,TravelProcess,WorkProcess,Result,Note1,Note2,Note3,Note4,Note5,AttachmentName,Attachment,Row_Number()over(order by MainCode) as crow from Bill_TravelReport";
        string sqlCont = "select count(*) from Bill_TravelReport";

        public bool Exists(string MainCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" MainCode = @MainCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode",MainCode)			};
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

        public int Add(Bill_TravelReport model)
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
        public int Delete(string MainCode)
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
                    throw;
                }
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Bill_TravelReport model, SqlTransaction tran)
        {
            Delete(model.MainCode, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Bill_TravelReport(");
            strSql.Append("MainCode,TravelProcess,WorkProcess,Result,Note1,Note2,Note3,Note4,Note5,Attachment,AttachmentName");
            strSql.Append(") values (");
            strSql.Append("@MainCode,@TravelProcess,@WorkProcess,@Result,@Note1,@Note2,@Note3,@Note4,@Note5,@Attachment,@AttachmentName");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@MainCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@TravelProcess", SqlDbType.Text) ,            
                        new SqlParameter("@WorkProcess", SqlDbType.Text) ,            
                        new SqlParameter("@Result", SqlDbType.Text) ,            
                        new SqlParameter("@Note1", SqlDbType.NChar,10) ,            
                        new SqlParameter("@Note2", SqlDbType.NChar,10) ,            
                        new SqlParameter("@Note3", SqlDbType.NChar,10) ,            
                        new SqlParameter("@Note4", SqlDbType.NChar,10) ,            
                        new SqlParameter("@Note5", SqlDbType.NChar,10) ,
                        new SqlParameter("@Attachment", SqlDbType.NVarChar,100) ,  
                        new SqlParameter("@AttachmentName", SqlDbType.NVarChar,100)   
              
            };

            parameters[0].Value = SqlNull(model.MainCode);

            parameters[1].Value = SqlNull(model.TravelProcess);

            parameters[2].Value = SqlNull(model.WorkProcess);

            parameters[3].Value = SqlNull(model.Result);

            parameters[4].Value = SqlNull(model.Note1);

            parameters[5].Value = SqlNull(model.Note2);

            parameters[6].Value = SqlNull(model.Note3);

            parameters[7].Value = SqlNull(model.Note4);

            parameters[8].Value = SqlNull(model.Note5);

            parameters[9].Value = SqlNull(model.Attachment);
            parameters[10].Value = SqlNull(model.AttachmentName);

            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string MainCode, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Bill_TravelReport ");
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = MainCode;

            string stresql = "update bill_travelApplication set ReportCode='' where ReportCode='" + MainCode + "'";
            DataHelper.ExcuteNonQuery(stresql, tran, null, false);

            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        public IList<Bill_TravelReport> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Bill_TravelReport> list = new List<Bill_TravelReport>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_TravelReport model = new Bill_TravelReport();
                model.MainCode = dr["MainCode"].ToString();
                model.TravelProcess = dr["TravelProcess"].ToString();
                model.WorkProcess = dr["WorkProcess"].ToString();
                model.Result = dr["Result"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Attachment = dr["Attachment"].ToString();
                model.AttachmentName = dr["AttachmentName"].ToString();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bill_TravelReport GetModel(string MainCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where MainCode=@MainCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@MainCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = MainCode;


            Bill_TravelReport model = new Bill_TravelReport();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.MainCode = dr["MainCode"].ToString();
                    model.TravelProcess = dr["TravelProcess"].ToString();
                    model.WorkProcess = dr["WorkProcess"].ToString();
                    model.Result = dr["Result"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Attachment = dr["Attachment"].ToString();
                    model.AttachmentName = dr["AttachmentName"].ToString();
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
        public IList<Bill_TravelReport> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<Bill_TravelReport> GetAllList(int beg, int end)
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
