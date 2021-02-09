using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace Dal.SaleProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlItemDal
    {
        string sql = "select Code,Note2,Note3,Note4,Note5,CName,ControlCodeFirst,Status,ControlNameFirst,ControlCodeSecond,ControlNameSecond,Months,Remark,Note1,Row_Number()over(order by Code) as crow from T_ControlItem";
        string sqlCont = "select count(*) from T_ControlItem";

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

        public void Add(Models.T_ControlItemMode model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

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
        public void Delete(string Code)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(Code, tran);
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
        /// 
        /// </summary>
        /// <param name="saleprocemode"></param>
        /// <returns></returns>
        public int Updetesalep(Models.T_ControlItemMode saleprocemode)
        {
            string sql = "update T_ControlItem set CName=@CName,ControlCodeFirst=@ControlCodeFirst,ControlNameFirst=@ControlNameFirst,ControlCodeSecond=@ControlCodeSecond,ControlNameSecond=@ControlNameSecond,Status=@Status,Months=@Months,Remark=@Remark  where  Code=@Code";
            SqlParameter[] sps = {   
                                     new SqlParameter("@CName",saleprocemode.CName),
                                     new SqlParameter("@ControlCodeFirst",saleprocemode.ControlCodeFirst),
                                     new SqlParameter("@ControlNameFirst",saleprocemode.ControlNameFirst),
                                     new SqlParameter("@ControlCodeSecond",saleprocemode.ControlCodeSecond),
                                     new SqlParameter("@ControlNameSecond",saleprocemode.ControlNameSecond),
                                     new SqlParameter("@Status",saleprocemode.Status),
                                     new SqlParameter("@Months",saleprocemode.Months),
                                     new SqlParameter("@Remark",saleprocemode.Remark),
                                     new SqlParameter("@Code",saleprocemode.Code)
                                 };
            return DataHelper.ExcuteNonQuery(sql, sps, false);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Models.T_ControlItemMode model, SqlTransaction tran)
        {
            
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_ControlItem(");
            strSql.Append("Code,Note2,Note3,Note4,Note5,CName,ControlCodeFirst,ControlNameFirst,ControlCodeSecond,ControlNameSecond,Months,Remark,Note1,Status");
            strSql.Append(") values (");
            strSql.Append("@Code,@Note2,@Note3,@Note4,@Note5,@CName,@ControlCodeFirst,@ControlNameFirst,@ControlCodeSecond,@ControlNameSecond,@Months,@Remark,@Note1,@Status");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Code", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@CName", SqlDbType.NVarChar,50) , 
                        new SqlParameter("@ControlCodeFirst", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlNameFirst", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlCodeSecond", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlNameSecond", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Months", SqlDbType.VarChar,5) ,            
                        new SqlParameter("@Remark", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50),
                       new SqlParameter("@Status",SqlDbType.Char,1)

              
            };

            parameters[0].Value = SqlNull(model.Code);

            parameters[1].Value = SqlNull(model.Note2);

            parameters[2].Value = SqlNull(model.Note3);

            parameters[3].Value = SqlNull(model.Note4);

            parameters[4].Value = SqlNull(model.Note5);

            parameters[5].Value = SqlNull(model.CName);

            parameters[6].Value = SqlNull(model.ControlCodeFirst);

            parameters[7].Value = SqlNull(model.ControlNameFirst);

            parameters[8].Value = SqlNull(model.ControlCodeSecond);

            parameters[9].Value = SqlNull(model.ControlNameSecond);

            parameters[10].Value = SqlNull(model.Months);

            parameters[11].Value = SqlNull(model.Remark);

            parameters[12].Value = SqlNull(model.Note1);

            parameters[13].Value = SqlNull(model.Status);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string Code, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_ControlItem ");
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList< T_ControlItemMode> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Models.T_ControlItemMode> list = new List<Models.T_ControlItemMode>();
            foreach (DataRow dr in dt.Rows)
            {
                T_ControlItemMode model = new T_ControlItemMode();
                model.Code = dr["Code"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.CName = dr["CName"].ToString();
                model.ControlCodeFirst = dr["ControlCodeFirst"].ToString();
                model.ControlNameFirst = dr["ControlNameFirst"].ToString();
                model.ControlCodeSecond = dr["ControlCodeSecond"].ToString();
                model.ControlNameSecond = dr["ControlNameSecond"].ToString();
                model.Months = dr["Months"].ToString();
                model.Remark = dr["Remark"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Status = dr["Status"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Models.T_ControlItemMode GetModel(string Code)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            T_ControlItemMode model = new T_ControlItemMode();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Code = dr["Code"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.CName = dr["CName"].ToString();
                    model.ControlCodeFirst = dr["ControlCodeFirst"].ToString();
                    model.ControlNameFirst = dr["ControlNameFirst"].ToString();
                    model.ControlCodeSecond = dr["ControlCodeSecond"].ToString();
                    model.ControlNameSecond = dr["ControlNameSecond"].ToString();
                    model.Months = dr["Months"].ToString();
                    model.Remark = dr["Remark"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Status = dr["Status"].ToString();

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
        public IList<Models.T_ControlItemMode> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<Models.T_ControlItemMode> GetAllList(int beg, int end)
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
