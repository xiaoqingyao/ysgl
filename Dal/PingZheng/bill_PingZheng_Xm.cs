using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.PingZheng
{
    //bill_pingzheng_xm
    public partial class bill_pingzheng_xm
    {
        string sql = "select Note3,Note4,Note5,Note6,Note7,Note8,Note9,xmCode,xmName,parentCode,parentName,isDefault,(case isDefault when '0' then '否' when '1' then '是' end) as isDefaultShow,Status,Note1,Note2,list_id,Row_Number()over(order by list_id) as crow from bill_pingzheng_xm";
        string sqlCont = "select count(*) from bill_pingzheng_xm";

        public bool Exists(int list_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" list_id = @list_id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Int,4)
			};
            parameters[0].Value = list_id;

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

        public int Add(bill_pingzheng_xmModel model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int iRel = 0;
                try
                {
                    Delete(model.list_id, tran);
                    iRel = Add(model, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    iRel = -1; ;
                }
                return iRel;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int list_id)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                int iRel = 0;
                try
                {
                    iRel = Delete(list_id, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    iRel = -1;
                    throw;
                }
                return iRel;
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(bill_pingzheng_xmModel model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_pingzheng_xm(");
            strSql.Append("Note3,Note4,Note5,Note6,Note7,Note8,Note9,xmCode,xmName,parentCode,parentName,isDefault,Status,Note1,Note2");
            strSql.Append(") values (");
            strSql.Append("@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@xmCode,@xmName,@parentCode,@parentName,@isDefault,@Status,@Note1,@Note2");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@xmCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@xmName", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@parentCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@parentName", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@isDefault", SqlDbType.Char,1) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.Note3);

            parameters[1].Value = SqlNull(model.Note4);

            parameters[2].Value = SqlNull(model.Note5);

            parameters[3].Value = SqlNull(model.Note6);

            parameters[4].Value = SqlNull(model.Note7);

            parameters[5].Value = SqlNull(model.Note8);

            parameters[6].Value = SqlNull(model.Note9);

            parameters[7].Value = SqlNull(model.xmCode);

            parameters[8].Value = SqlNull(model.xmName);

            parameters[9].Value = SqlNull(model.parentCode);

            parameters[10].Value = SqlNull(model.parentName);

            parameters[11].Value = SqlNull(model.isDefault);

            parameters[12].Value = SqlNull(model.Status);

            parameters[13].Value = SqlNull(model.Note1);

            parameters[14].Value = SqlNull(model.Note2);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int list_id, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_pingzheng_xm ");
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Int,4)
			};
            parameters[0].Value = list_id;
            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<bill_pingzheng_xmModel> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill_pingzheng_xmModel> list = new List<bill_pingzheng_xmModel>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_pingzheng_xmModel model = new bill_pingzheng_xmModel();
                if (!DBNull.Value.Equals(dr["list_id"]))
                {
                    model.list_id = int.Parse(dr["list_id"].ToString());
                }
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.xmCode = dr["xmCode"].ToString();
                model.xmName = dr["xmName"].ToString();
                model.parentCode = dr["parentCode"].ToString();
                model.parentName = dr["parentName"].ToString();
                model.isDefault = dr["isDefault"].ToString();
                model.Status = dr["Status"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public bill_pingzheng_xmModel GetModel(int list_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Int,4)
			};
            parameters[0].Value = list_id;


            bill_pingzheng_xmModel model = new bill_pingzheng_xmModel();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["list_id"]))
                    {
                        model.list_id = int.Parse(dr["list_id"].ToString());
                    }
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.xmCode = dr["xmCode"].ToString();
                    model.xmName = dr["xmName"].ToString();
                    model.parentCode = dr["parentCode"].ToString();
                    model.parentName = dr["parentName"].ToString();
                    model.isDefault = dr["isDefault"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public bill_pingzheng_xmModel GetModelByName(string strName)
        {
            string strsql = "select top 1 Note3,Note4,Note5,Note6,Note7,Note8,Note9,xmCode,xmName,parentCode,parentName,isDefault,(case isDefault when '0' then '否' when '1' then '是' end) as isDefaultShow,Status,Note1,Note2,list_id,Row_Number()over(order by list_id) as crow from bill_pingzheng_xm";
            StringBuilder strSql = new StringBuilder();
            strSql.Append(strsql);
            strSql.Append(" where xmName=@xmName");
            SqlParameter[] parameters = {
					new SqlParameter("@xmName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = strName;


            bill_pingzheng_xmModel model = new bill_pingzheng_xmModel();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["list_id"]))
                    {
                        model.list_id = int.Parse(dr["list_id"].ToString());
                    }
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.xmCode = dr["xmCode"].ToString();
                    model.xmName = dr["xmName"].ToString();
                    model.parentCode = dr["parentCode"].ToString();
                    model.parentName = dr["parentName"].ToString();
                    model.isDefault = dr["isDefault"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();

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
        public IList<bill_pingzheng_xmModel> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill_pingzheng_xmModel> GetAllList(int beg, int end)
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



        public DataTable GelAllTable()
        {
            return DataHelper.GetDataTable(sql, null, false);
        }

        public bill_pingzheng_xmModel GetChildByName(string strName)
        {
            string s = "select top 1 Note3,Note4,Note5,Note6,Note7,Note8,Note9,xmCode,xmName,parentCode,parentName,isDefault,(case isDefault when '0' then '否' when '1' then '是' end) as isDefaultShow,Status,Note1,Note2,list_id,Row_Number()over(order by list_id) as crow from bill_pingzheng_xm";
            StringBuilder strSql = new StringBuilder();
            strSql.Append(s);
            strSql.Append(" where parentName=@parentName order by isDefault desc");
            SqlParameter[] parameters = {
					new SqlParameter("@parentName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = strName;


            bill_pingzheng_xmModel model = new bill_pingzheng_xmModel();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["list_id"]))
                    {
                        model.list_id = int.Parse(dr["list_id"].ToString());
                    }
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.xmCode = dr["xmCode"].ToString();
                    model.xmName = dr["xmName"].ToString();
                    model.parentCode = dr["parentCode"].ToString();
                    model.parentName = dr["parentName"].ToString();
                    model.isDefault = dr["isDefault"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<bill_pingzheng_xmModel> GetAllParent()
        {
            string str = "select Note3,Note4,Note5,Note6,Note7,Note8,Note9,xmCode,xmName,parentCode,parentName,isDefault,(case isDefault when '0' then '否' when '1' then '是' end) as isDefaultShow,Status,Note1,Note2,list_id,Row_Number()over(order by list_id) as crow from bill_pingzheng_xm where parentCode='0' order by isDefault desc";
            return ListMaker(str, null);
        }

        public IList<bill_pingzheng_xmModel> GetChildsByName(string strName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where parentName=@parentName order by isDefault desc");
            SqlParameter[] parameters = {
					new SqlParameter("@parentName", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = strName;

            return ListMaker(strSql.ToString(), parameters);
        }


        public bool ExistsNext(string parentcode)
        {
            try
            {

            string sql = "select count(*) from bill_pingzheng_xm where parentcode=@parentcode";
            SqlParameter[] parameters = {
					new SqlParameter("@parentcode", SqlDbType.VarChar,50)
			};
            parameters[0].Value = parentcode;

            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(sql, parameters, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

    }
}
