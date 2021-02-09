using Dal.Bills;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Dal.FeiYong_DZ
{
    public class dz_zcgzsqdDal
    {
        string sql = "select swbh,sybm,xyrq,gjjz,gzbz,sqjs,zje,sgbmfzr,sgbmrq,nqbyj,nqbrq,zydj,cwbyj,cwbrq,rzxzyj,rzxzrq,xzbyj,xzbrq,fj,note0,note1,note2,sqsy,note3,note4,note5,tsbz,bh,sqsj,wpmc,ggsl,yt,Row_Number()over(order by swbh desc) as crow from dz_zcgzsqd";
        string sqlCont = "select count(*) from dz_zcgzsqd";

        public bool Exists(string swbh)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" swbh = @swbh  ");
            SqlParameter[] parameters = {
					new SqlParameter("@swbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = swbh;

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

        public int Add(Bill_Main mainmodel, dz_zcgzsqd model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(mainmodel.BillCode);
                    maindal.InsertMain(mainmodel);
                    Delete(model.swbh, tran);
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
        public int Delete(string swbh)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(swbh, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(swbh, tran);
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
        public int Add(dz_zcgzsqd model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dz_zcgzsqd(");
            strSql.Append("swbh,sybm,xyrq,gjjz,gzbz,sqjs,zje,sgbmfzr,sgbmrq,nqbyj,nqbrq,zydj,cwbyj,cwbrq,rzxzyj,rzxzrq,xzbyj,xzbrq,fj,note0,note1,note2,sqsy,note3,note4,note5,tsbz,bh,sqsj,wpmc,ggsl,yt");
            strSql.Append(") values (");
            strSql.Append("@swbh,@sybm,@xyrq,@gjjz,@gzbz,@sqjs,@zje,@sgbmfzr,@sgbmrq,@nqbyj,@nqbrq,@zydj,@cwbyj,@cwbrq,@rzxzyj,@rzxzrq,@xzbyj,@xzbrq,@fj,@note0,@note1,@note2,@sqsy,@note3,@note4,@note5,@tsbz,@bh,@sqsj,@wpmc,@ggsl,@yt");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@swbh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sybm", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@xyrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@gjjz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@gzbz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqjs", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@zje", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@sgbmfzr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sgbmrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@nqbyj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@nqbrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@zydj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwbyj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwbrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@rzxzyj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@rzxzrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@xzbyj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@xzbrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@fj", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@note0", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note1", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note2", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqsy", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@note3", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note4", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note5", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@tsbz", SqlDbType.VarChar,500) ,            
                        new SqlParameter("@bh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqsj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@wpmc", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ggsl", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yt", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.swbh);

            parameters[1].Value = SqlNull(model.sybm);

            parameters[2].Value = SqlNull(model.xyrq);

            parameters[3].Value = SqlNull(model.gjjz);

            parameters[4].Value = SqlNull(model.gzbz);

            parameters[5].Value = SqlNull(model.sqjs);

            parameters[6].Value = SqlNull(model.zje);

            parameters[7].Value = SqlNull(model.sgbmfzr);

            parameters[8].Value = SqlNull(model.sgbmrq);

            parameters[9].Value = SqlNull(model.nqbyj);

            parameters[10].Value = SqlNull(model.nqbrq);

            parameters[11].Value = SqlNull(model.zydj);

            parameters[12].Value = SqlNull(model.cwbyj);

            parameters[13].Value = SqlNull(model.cwbrq);

            parameters[14].Value = SqlNull(model.rzxzyj);

            parameters[15].Value = SqlNull(model.rzxzrq);

            parameters[16].Value = SqlNull(model.xzbyj);

            parameters[17].Value = SqlNull(model.xzbrq);

            parameters[18].Value = SqlNull(model.fj);

            parameters[19].Value = SqlNull(model.note0);

            parameters[20].Value = SqlNull(model.note1);

            parameters[21].Value = SqlNull(model.note2);

            parameters[22].Value = SqlNull(model.sqsy);

            parameters[23].Value = SqlNull(model.note3);

            parameters[24].Value = SqlNull(model.note4);

            parameters[25].Value = SqlNull(model.note5);

            parameters[26].Value = SqlNull(model.tsbz);

            parameters[27].Value = SqlNull(model.bh);

            parameters[28].Value = SqlNull(model.sqsj);

            parameters[29].Value = SqlNull(model.wpmc);

            parameters[30].Value = SqlNull(model.ggsl);

            parameters[31].Value = SqlNull(model.yt);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string swbh, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dz_zcgzsqd ");
            strSql.Append(" where swbh=@swbh ");
            SqlParameter[] parameters = {
					new SqlParameter("@swbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = swbh;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<dz_zcgzsqd> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<dz_zcgzsqd> list = new List<dz_zcgzsqd>();
            foreach (DataRow dr in dt.Rows)
            {
                dz_zcgzsqd model = new dz_zcgzsqd();
                model.swbh = dr["swbh"].ToString();
                model.sybm = dr["sybm"].ToString();
                model.xyrq = dr["xyrq"].ToString();
                model.gjjz = dr["gjjz"].ToString();
                model.gzbz = dr["gzbz"].ToString();
                if (!DBNull.Value.Equals(dr["sqjs"]))
                {
                    model.sqjs = decimal.Parse(dr["sqjs"].ToString());
                }
                if (!DBNull.Value.Equals(dr["zje"]))
                {
                    model.zje = decimal.Parse(dr["zje"].ToString());
                }
                model.sgbmfzr = dr["sgbmfzr"].ToString();
                model.sgbmrq = dr["sgbmrq"].ToString();
                model.nqbyj = dr["nqbyj"].ToString();
                model.nqbrq = dr["nqbrq"].ToString();
                model.zydj = dr["zydj"].ToString();
                model.cwbyj = dr["cwbyj"].ToString();
                model.cwbrq = dr["cwbrq"].ToString();
                model.rzxzyj = dr["rzxzyj"].ToString();
                model.rzxzrq = dr["rzxzrq"].ToString();
                model.xzbyj = dr["xzbyj"].ToString();
                model.xzbrq = dr["xzbrq"].ToString();
                model.fj = dr["fj"].ToString();
                model.note0 = dr["note0"].ToString();
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.sqsy = dr["sqsy"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();
                model.tsbz = dr["tsbz"].ToString();
                model.bh = dr["bh"].ToString();
                model.sqsj = dr["sqsj"].ToString();
                model.wpmc = dr["wpmc"].ToString();
                model.ggsl = dr["ggsl"].ToString();
                model.yt = dr["yt"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public dz_zcgzsqd GetModel(string swbh)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where swbh=@swbh ");
            SqlParameter[] parameters = {
					new SqlParameter("@swbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = swbh;


            dz_zcgzsqd model = new dz_zcgzsqd();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.swbh = dr["swbh"].ToString();
                    model.sybm = dr["sybm"].ToString();
                    model.xyrq = dr["xyrq"].ToString();
                    model.gjjz = dr["gjjz"].ToString();
                    model.gzbz = dr["gzbz"].ToString();
                    if (!DBNull.Value.Equals(dr["sqjs"]))
                    {
                        model.sqjs = decimal.Parse(dr["sqjs"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["zje"]))
                    {
                        model.zje = decimal.Parse(dr["zje"].ToString());
                    }
                    model.sgbmfzr = dr["sgbmfzr"].ToString();
                    model.sgbmrq = dr["sgbmrq"].ToString();
                    model.nqbyj = dr["nqbyj"].ToString();
                    model.nqbrq = dr["nqbrq"].ToString();
                    model.zydj = dr["zydj"].ToString();
                    model.cwbyj = dr["cwbyj"].ToString();
                    model.cwbrq = dr["cwbrq"].ToString();
                    model.rzxzyj = dr["rzxzyj"].ToString();
                    model.rzxzrq = dr["rzxzrq"].ToString();
                    model.xzbyj = dr["xzbyj"].ToString();
                    model.xzbrq = dr["xzbrq"].ToString();
                    model.fj = dr["fj"].ToString();
                    model.note0 = dr["note0"].ToString();
                    model.note1 = dr["note1"].ToString();
                    model.note2 = dr["note2"].ToString();
                    model.sqsy = dr["sqsy"].ToString();
                    model.note3 = dr["note3"].ToString();
                    model.note4 = dr["note4"].ToString();
                    model.note5 = dr["note5"].ToString();
                    model.tsbz = dr["tsbz"].ToString();
                    model.bh = dr["bh"].ToString();
                    model.sqsj = dr["sqsj"].ToString();
                    model.wpmc = dr["wpmc"].ToString();
                    model.ggsl = dr["ggsl"].ToString();
                    model.yt = dr["yt"].ToString();

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
        public IList<dz_zcgzsqd> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<dz_zcgzsqd> GetAllList(int beg, int end)
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
        /// 根据查询条件分页
        /// </summary>
        public IList<dz_zcgzsqd> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }
        /// <summary>
        /// 根据查询条件分页并返回记录数 
        /// </summary>
        public IList<dz_zcgzsqd> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
        {
            totalCount = GetAllListCount(paramter, sqls);

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }


        /// <summary>
        /// 根据查询条件
        /// </summary>
        public IList<dz_zcgzsqd> GetAllList(List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(sql);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);

            if (paramter == null)
            {
                return ListMaker(strSql.ToString(), null);
            }
            else
            {
                return ListMaker(strSql.ToString(), paramter.ToArray());
            }
        }

        /// <summary>
        /// 根据查询条件获取行数
        /// </summary>
        public int GetAllListCount(List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append(sqlCont);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);

            if (paramter == null)
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), null, false));
            }
            else
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), paramter.ToArray(), false));
            }
        }

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
    }
}
