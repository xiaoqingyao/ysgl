using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;
using Dal.Bills;

namespace Dal.FeeApplication
{
    /// <summary>
    /// 出差申请单DAL
    /// </summary>
    public class bill_travelApplicationDal
    {
        string sql = "select maincode,MoreThanStandard,ReportCode,typecode,travelPersionCode,arrdess,travelDate,reasion,travelplan,needAmount,jiaotongfei,zhusufei,yewuzhaodaifei,huiyifei,yinshuafei,qitafei,Transport,sendDept,Row_Number()over(order by maincode) as crow from Bill_TravelApplication";
        string sqlCont = "select count(*) from Bill_TravelApplication";

        public bool Exists(string maincode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" maincode = @maincode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@maincode", SqlDbType.VarChar,50)			};
            parameters[0].Value = maincode;

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

        public int Add(Bill_TravelApplication model)
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
        public int Delete(string maincode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(maincode, tran);
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
        public int Add(Bill_TravelApplication model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Bill_TravelApplication(");
            strSql.Append("maincode,MoreThanStandard,typecode,travelPersionCode,arrdess,travelDate,reasion,travelplan,needAmount,Transport,ReportCode,jiaotongfei,zhusufei,yewuzhaodaifei,huiyifei,yinshuafei,qitafei,sendDept");
            strSql.Append(") values (");
            strSql.Append("@maincode,@MoreThanStandard,@typecode,@travelPersionCode,@arrdess,@travelDate,@reasion,@travelplan,@needAmount,@Transport,@ReportCode,@jiaotongfei,@zhusufei,@yewuzhaodaifei,@huiyifei,@yinshuafei,@qitafei,@sendDept");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@maincode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@MoreThanStandard", SqlDbType.Int,4) ,            
                        new SqlParameter("@typecode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@travelPersionCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@arrdess", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@travelDate", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@reasion", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@travelplan", SqlDbType.VarChar,100) ,            
                        new SqlParameter("@needAmount", SqlDbType.Int,4) ,            
                        new SqlParameter("@Transport", SqlDbType.VarChar,50) ,    
                        new SqlParameter("@ReportCode", SqlDbType.VarChar,50)  ,
                       new SqlParameter("@jiaotongfei", SqlDbType.Int,4) ,  
                       new SqlParameter("@zhusufei", SqlDbType.Int,4) ,  
                       new SqlParameter("@yewuzhaodaifei", SqlDbType.Int,4) ,  
                       new SqlParameter("@huiyifei", SqlDbType.Int,4) ,  
                       new SqlParameter("@yinshuafei", SqlDbType.Int,4) ,  
                       new SqlParameter("@qitafei", SqlDbType.Int,4) ,
                        new SqlParameter("@sendDept", SqlDbType.VarChar,50) 
              
            };

            parameters[0].Value = SqlNull(model.maincode);

            parameters[1].Value = SqlNull(model.MoreThanStandard);

            parameters[2].Value = SqlNull(model.typecode);

            parameters[3].Value = SqlNull(model.travelPersionCode);

            parameters[4].Value = SqlNull(model.arrdess);

            parameters[5].Value = SqlNull(model.travelDate);

            parameters[6].Value = SqlNull(model.reasion);

            parameters[7].Value = SqlNull(model.travelplan);

            parameters[8].Value = SqlNull(model.needAmount);

            parameters[9].Value = SqlNull(model.Transport);
            parameters[10].Value = SqlNull(model.ReportCode);
            parameters[11].Value = SqlNull(model.jiaotongfei);
            parameters[12].Value = SqlNull(model.zhusufei);
            parameters[13].Value = SqlNull(model.yewuzhaodaifei);
            parameters[14].Value = SqlNull(model.huiyifei);
            parameters[15].Value = SqlNull(model.yinshuafei);
            parameters[16].Value = SqlNull(model.qitafei);
             parameters[17].Value = SqlNull(model.sendDept);

            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string maincode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Bill_TravelApplication ");
            strSql.Append(" where maincode=@maincode ");
            SqlParameter[] parameters = {
					new SqlParameter("@maincode", SqlDbType.VarChar,50)			};
            parameters[0].Value = maincode;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<Bill_TravelApplication> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Bill_TravelApplication> list = new List<Bill_TravelApplication>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_TravelApplication model = new Bill_TravelApplication();
                model.maincode = dr["maincode"].ToString();
                if (!DBNull.Value.Equals(dr["MoreThanStandard"]))
                {
                    model.MoreThanStandard = int.Parse(dr["MoreThanStandard"].ToString());
                }
                model.typecode = dr["typecode"].ToString();
                model.travelPersionCode = dr["travelPersionCode"].ToString();
                model.arrdess = dr["arrdess"].ToString();
                model.ReportCode = dr["ReportCode"].ToString();
                if (!DBNull.Value.Equals(dr["travelDate"]))
                {
                    model.travelDate = dr["travelDate"].ToString();
                }
                model.reasion = dr["reasion"].ToString();
                model.sendDept = dr["sendDept"].ToString();
                model.travelplan = dr["travelplan"].ToString();
                if (!DBNull.Value.Equals(dr["needAmount"]))
                {
                    model.needAmount = int.Parse(dr["needAmount"].ToString());
                }
                model.Transport = dr["Transport"].ToString();
                model.jiaotongfei = int.Parse(dr["jiaotongfei"].ToString().Equals("") ? "0" : dr["jiaotongfei"].ToString());
                model.zhusufei = int.Parse(dr["zhusufei"].ToString().Equals("") ? "0" : dr["zhusufei"].ToString());
                model.yewuzhaodaifei = int.Parse(dr["yewuzhaodaifei"].ToString().Equals("") ? "0" : dr["yewuzhaodaifei"].ToString());
                model.huiyifei = int.Parse(dr["huiyifei"].ToString().Equals("") ? "0" : dr["huiyifei"].ToString());
                model.yinshuafei = int.Parse(dr["yinshuafei"].ToString().Equals("") ? "0" : dr["yinshuafei"].ToString());
                model.qitafei = int.Parse(dr["qitafei"].ToString().Equals("") ? "0" : dr["qitafei"].ToString());
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bill_TravelApplication GetModel(string maincode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where maincode=@maincode ");
            SqlParameter[] parameters = {
					new SqlParameter("@maincode", SqlDbType.VarChar,50)			};
            parameters[0].Value = maincode;


            Bill_TravelApplication model = new Bill_TravelApplication();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.maincode = dr["maincode"].ToString();
                    if (!DBNull.Value.Equals(dr["MoreThanStandard"]))
                    {
                        model.MoreThanStandard = int.Parse(dr["MoreThanStandard"].ToString());
                    }
                    model.typecode = dr["typecode"].ToString();
                    model.travelPersionCode = dr["travelPersionCode"].ToString();
                    model.arrdess = dr["arrdess"].ToString();
                    if (!DBNull.Value.Equals(dr["travelDate"]))
                    {
                        model.travelDate = dr["travelDate"].ToString();
                    }
                    model.reasion = dr["reasion"].ToString();
                    model.travelplan = dr["travelplan"].ToString();
                    if (!DBNull.Value.Equals(dr["needAmount"]))
                    {
                        model.needAmount = int.Parse(dr["needAmount"].ToString());
                    }

                    model.Transport = dr["Transport"].ToString();
                    model.ReportCode = dr["ReportCode"].ToString();
                    model.sendDept = dr["sendDept"].ToString();
                    model.Transport = dr["Transport"].ToString();
                    model.jiaotongfei = int.Parse(dr["jiaotongfei"].ToString());
                    model.zhusufei = int.Parse(dr["zhusufei"].ToString());
                    model.yewuzhaodaifei = int.Parse(dr["yewuzhaodaifei"].ToString());
                    model.huiyifei = int.Parse(dr["huiyifei"].ToString());
                    model.yinshuafei = int.Parse(dr["yinshuafei"].ToString());
                    model.qitafei = int.Parse(dr["qitafei"].ToString());
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
        public IList<Bill_TravelApplication> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<Bill_TravelApplication> GetAllList(int beg, int end)
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
        /// 通过主表code获取所有出差的人员的编号
        /// </summary>
        /// <returns></returns>
        public string[] GetAllTraUserCodeByMainCode(string mainCode)
        {
            string selSql = "select * from Bill_TravelApplication where maincode='" + mainCode + "'";
            DataTable dtRel = DataHelper.GetDataTable(selSql, null, false);
            int iRelRows = dtRel.Rows.Count;
            string[] arrRel = new string[iRelRows];
            for (int i = 0; i < iRelRows; i++)
            {
                arrRel[i] = dtRel.Rows[i]["travelPersionCode"].ToString();
            }
            return arrRel;
        }
    }
}
