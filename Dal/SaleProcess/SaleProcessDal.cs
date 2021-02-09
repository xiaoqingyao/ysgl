using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.SaleProcess
{
    /// <summary>
    /// T_SaleProcess
    /// </summary>
  public  class SaleProcessDal
    {

        string sql = "select Code,PName,Status,Note1,Note2,Note3,Note4,Note5,Row_Number()over(order by Code) as crow from T_SaleProcess";
        string sqlCont = "select count(*) from T_SaleProcess";
      /// <summary>
      /// 
      /// </summary>
      /// <param name="Code"></param>
      /// <returns></returns>
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="model"></param>
        public void  Add(SaleProcessMode model)
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
        /// 增加一条数据
        /// </summary>
        public void Add(SaleProcessMode model, SqlTransaction tran)
        {
           // Delete(model.Code, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_SaleProcess(");
            strSql.Append("Code,PName,Status,Note1,Note2,Note3,Note4,Note5");
            strSql.Append(") values (");
            strSql.Append("@Code,@PName,@Status,@Note1,@Note2,@Note3,@Note4,@Note5");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Code", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@PName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.Code);

            parameters[1].Value = SqlNull(model.PName);

            parameters[2].Value = SqlNull(model.Status);

            parameters[3].Value = SqlNull(model.Note1);

            parameters[4].Value = SqlNull(model.Note2);

            parameters[5].Value = SqlNull(model.Note3);

            parameters[6].Value = SqlNull(model.Note4);

            parameters[7].Value = SqlNull(model.Note5);


           DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

      /// <summary>
      /// 修改
      /// </summary>
      /// <param name="userslogmode"></param>
      /// <returns></returns>
        public int Updetesalep(Models.SaleProcessMode saleprocemode)
        {
            string sql = "update T_SaleProcess set PName=@PName,Status=@Status where  Code=@Code";
            SqlParameter[] sps = {   
                                     new SqlParameter("@PName",saleprocemode.PName),
                                     new SqlParameter("@Status",saleprocemode.Status),
                                     new SqlParameter("@Code",saleprocemode.Code)
                                 };
            return DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string Code, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SaleProcess ");
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<SaleProcessMode> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<SaleProcessMode> list = new List<SaleProcessMode>();
            foreach (DataRow dr in dt.Rows)
            {
                SaleProcessMode model = new SaleProcessMode();
                model.Code = dr["Code"].ToString();
                model.PName = dr["PName"].ToString();
                model.Status = dr["Status"].ToString();
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
        public SaleProcessMode GetModel(string Code)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            SaleProcessMode model = new SaleProcessMode();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Code = dr["Code"].ToString();
                    model.PName = dr["PName"].ToString();
                    model.Status = dr["Status"].ToString();
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
      /// 按条件查询
      /// </summary>
      /// <param name="strtext"></param>
      /// <returns></returns>
        public DataTable  GetAllDate(string strtext) 
        {
            string sqlall = "select (case Status when '1' then '正常' when '0' then '禁用' end) as status,Code,PName,Note1,Note2,Note3,Note4,Note5 from T_SaleProcess where 1=1 ";
            if (strtext!="")
	        {
        		             sqlall += " and (Code like'%" + strtext+ "%' or PName like '%" + strtext + "%')";
	        }
            return DataHelper.GetDataTable(sqlall,null,false);

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
        public IList<SaleProcessMode> GetAllList()
        {
            return ListMaker(sql, null);
        }
        public IList<SaleProcessMode> GetAllList1()
        {
            string strsql = "select Code,PName,Status,Note1,Note2,Note3,Note4,Note5,Row_Number()over(order by Code) as crow from T_SaleProcess where Status='1'";
            return ListMaker(strsql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<SaleProcessMode> GetAllList(int beg, int end)
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
      /// <summary>
      /// 
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}
