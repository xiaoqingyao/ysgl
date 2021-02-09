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
    public class dz_yksqdDal
    {
        string sql = "select sqbh,skdw,khh,zh,bmfzr_yj,bmfzr_qz,bmfzr_rq,yfzkzy_yj,yfzkzy_qz,yfzkzy_rq,cwbfzr_yj,sqsj,cwbfzr_qz,cwbfzr_rq,cwxz_yj,cwxz_qz,cwxz_rq,dsz_yj,dsz_qz,dsz_rq,fj,note0,sqlx,note1,note2,note3,note4,note5,ykrq,sqr,kxyt,ykfs,kxje_dx,kxje_xx,Row_Number()over(order by sqbh desc) as crow from dz_yksqd";
        string sqlCont = "select count(*) from dz_yksqd";

        public bool Exists(string sqbh)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" sqbh = @sqbh  ");
            SqlParameter[] parameters = {
					new SqlParameter("@sqbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = sqbh;

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
        /// 添加
        /// </summary>
        /// <param name="mainModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(Bill_Main mainModel,dz_yksqd model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.sqbh, tran);
                    MainDal maindal = new MainDal();
                    maindal.DeleteMain(mainModel.BillCode);
                    maindal.InsertMain(mainModel);
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
        public int Delete(string sqbh)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int iRel = Delete(sqbh, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(sqbh, tran);
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
        public int Add(dz_yksqd model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into dz_yksqd(");
            strSql.Append("sqbh,skdw,khh,zh,bmfzr_yj,bmfzr_qz,bmfzr_rq,yfzkzy_yj,yfzkzy_qz,yfzkzy_rq,cwbfzr_yj,sqsj,cwbfzr_qz,cwbfzr_rq,cwxz_yj,cwxz_qz,cwxz_rq,dsz_yj,dsz_qz,dsz_rq,fj,note0,sqlx,note1,note2,note3,note4,note5,ykrq,sqr,kxyt,ykfs,kxje_dx,kxje_xx");
            strSql.Append(") values (");
            strSql.Append("@sqbh,@skdw,@khh,@zh,@bmfzr_yj,@bmfzr_qz,@bmfzr_rq,@yfzkzy_yj,@yfzkzy_qz,@yfzkzy_rq,@cwbfzr_yj,@sqsj,@cwbfzr_qz,@cwbfzr_rq,@cwxz_yj,@cwxz_qz,@cwxz_rq,@dsz_yj,@dsz_qz,@dsz_rq,@fj,@note0,@sqlx,@note1,@note2,@note3,@note4,@note5,@ykrq,@sqr,@kxyt,@ykfs,@kxje_dx,@kxje_xx");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@sqbh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@skdw", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@khh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@zh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bmfzr_yj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bmfzr_qz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bmfzr_rq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yfzkzy_yj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yfzkzy_qz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yfzkzy_rq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwbfzr_yj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqsj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwbfzr_qz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwbfzr_rq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwxz_yj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwxz_qz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cwxz_rq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@dsz_yj", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@dsz_qz", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@dsz_rq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@fj", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@note0", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqlx", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note1", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note2", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note3", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note4", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@note5", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ykrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sqr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@kxyt", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ykfs", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@kxje_dx", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@kxje_xx", SqlDbType.Decimal,9)             
              
            };

            parameters[0].Value = SqlNull(model.sqbh);

            parameters[1].Value = SqlNull(model.skdw);

            parameters[2].Value = SqlNull(model.khh);

            parameters[3].Value = SqlNull(model.zh);

            parameters[4].Value = SqlNull(model.bmfzr_yj);

            parameters[5].Value = SqlNull(model.bmfzr_qz);

            parameters[6].Value = SqlNull(model.bmfzr_rq);

            parameters[7].Value = SqlNull(model.yfzkzy_yj);

            parameters[8].Value = SqlNull(model.yfzkzy_qz);

            parameters[9].Value = SqlNull(model.yfzkzy_rq);

            parameters[10].Value = SqlNull(model.cwbfzr_yj);

            parameters[11].Value = SqlNull(model.sqsj);

            parameters[12].Value = SqlNull(model.cwbfzr_qz);

            parameters[13].Value = SqlNull(model.cwbfzr_rq);

            parameters[14].Value = SqlNull(model.cwxz_yj);

            parameters[15].Value = SqlNull(model.cwxz_qz);

            parameters[16].Value = SqlNull(model.cwxz_rq);

            parameters[17].Value = SqlNull(model.dsz_yj);

            parameters[18].Value = SqlNull(model.dsz_qz);

            parameters[19].Value = SqlNull(model.dsz_rq);

            parameters[20].Value = SqlNull(model.fj);

            parameters[21].Value = SqlNull(model.note0);

            parameters[22].Value = SqlNull(model.sqlx);

            parameters[23].Value = SqlNull(model.note1);

            parameters[24].Value = SqlNull(model.note2);

            parameters[25].Value = SqlNull(model.note3);

            parameters[26].Value = SqlNull(model.note4);

            parameters[27].Value = SqlNull(model.note5);

            parameters[28].Value = SqlNull(model.ykrq);

            parameters[29].Value = SqlNull(model.sqr);

            parameters[30].Value = SqlNull(model.kxyt);

            parameters[31].Value = SqlNull(model.ykfs);

            parameters[32].Value = SqlNull(model.kxje_dx);

            parameters[33].Value = SqlNull(model.kxje_xx);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string sqbh, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from dz_yksqd ");
            strSql.Append(" where sqbh=@sqbh ");
            SqlParameter[] parameters = {
					new SqlParameter("@sqbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = sqbh;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<dz_yksqd> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<dz_yksqd> list = new List<dz_yksqd>();
            foreach (DataRow dr in dt.Rows)
            {
                dz_yksqd model = new dz_yksqd();
                model.sqbh = dr["sqbh"].ToString();
                model.skdw = dr["skdw"].ToString();
                model.khh = dr["khh"].ToString();
                model.zh = dr["zh"].ToString();
                model.bmfzr_yj = dr["bmfzr_yj"].ToString();
                model.bmfzr_qz = dr["bmfzr_qz"].ToString();
                model.bmfzr_rq = dr["bmfzr_rq"].ToString();
                model.yfzkzy_yj = dr["yfzkzy_yj"].ToString();
                model.yfzkzy_qz = dr["yfzkzy_qz"].ToString();
                model.yfzkzy_rq = dr["yfzkzy_rq"].ToString();
                model.cwbfzr_yj = dr["cwbfzr_yj"].ToString();
                model.sqsj = dr["sqsj"].ToString();
                model.cwbfzr_qz = dr["cwbfzr_qz"].ToString();
                model.cwbfzr_rq = dr["cwbfzr_rq"].ToString();
                model.cwxz_yj = dr["cwxz_yj"].ToString();
                model.cwxz_qz = dr["cwxz_qz"].ToString();
                model.cwxz_rq = dr["cwxz_rq"].ToString();
                model.dsz_yj = dr["dsz_yj"].ToString();
                model.dsz_qz = dr["dsz_qz"].ToString();
                model.dsz_rq = dr["dsz_rq"].ToString();
                model.fj = dr["fj"].ToString();
                model.note0 = dr["note0"].ToString();
                model.sqlx = dr["sqlx"].ToString();
                model.note1 = dr["note1"].ToString();
                model.note2 = dr["note2"].ToString();
                model.note3 = dr["note3"].ToString();
                model.note4 = dr["note4"].ToString();
                model.note5 = dr["note5"].ToString();
                model.ykrq = dr["ykrq"].ToString();
                model.sqr = dr["sqr"].ToString();
                model.kxyt = dr["kxyt"].ToString();
                model.ykfs = dr["ykfs"].ToString();
                model.kxje_dx = dr["kxje_dx"].ToString();
                if (!DBNull.Value.Equals(dr["kxje_xx"]))
                {
                    model.kxje_xx = decimal.Parse(dr["kxje_xx"].ToString());
                }

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public dz_yksqd GetModel(string sqbh)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where sqbh=@sqbh ");
            SqlParameter[] parameters = {
					new SqlParameter("@sqbh", SqlDbType.VarChar,50)			};
            parameters[0].Value = sqbh;


            dz_yksqd model = new dz_yksqd();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.sqbh = dr["sqbh"].ToString();
                    model.skdw = dr["skdw"].ToString();
                    model.khh = dr["khh"].ToString();
                    model.zh = dr["zh"].ToString();
                    model.bmfzr_yj = dr["bmfzr_yj"].ToString();
                    model.bmfzr_qz = dr["bmfzr_qz"].ToString();
                    model.bmfzr_rq = dr["bmfzr_rq"].ToString();
                    model.yfzkzy_yj = dr["yfzkzy_yj"].ToString();
                    model.yfzkzy_qz = dr["yfzkzy_qz"].ToString();
                    model.yfzkzy_rq = dr["yfzkzy_rq"].ToString();
                    model.cwbfzr_yj = dr["cwbfzr_yj"].ToString();
                    model.sqsj = dr["sqsj"].ToString();
                    model.cwbfzr_qz = dr["cwbfzr_qz"].ToString();
                    model.cwbfzr_rq = dr["cwbfzr_rq"].ToString();
                    model.cwxz_yj = dr["cwxz_yj"].ToString();
                    model.cwxz_qz = dr["cwxz_qz"].ToString();
                    model.cwxz_rq = dr["cwxz_rq"].ToString();
                    model.dsz_yj = dr["dsz_yj"].ToString();
                    model.dsz_qz = dr["dsz_qz"].ToString();
                    model.dsz_rq = dr["dsz_rq"].ToString();
                    model.fj = dr["fj"].ToString();
                    model.note0 = dr["note0"].ToString();
                    model.sqlx = dr["sqlx"].ToString();
                    model.note1 = dr["note1"].ToString();
                    model.note2 = dr["note2"].ToString();
                    model.note3 = dr["note3"].ToString();
                    model.note4 = dr["note4"].ToString();
                    model.note5 = dr["note5"].ToString();
                    model.ykrq = dr["ykrq"].ToString();
                    model.sqr = dr["sqr"].ToString();
                    model.kxyt = dr["kxyt"].ToString();
                    model.ykfs = dr["ykfs"].ToString();
                    model.kxje_dx = dr["kxje_dx"].ToString();
                    if (!DBNull.Value.Equals(dr["kxje_xx"]))
                    {
                        model.kxje_xx = decimal.Parse(dr["kxje_xx"].ToString());
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
        public IList<dz_yksqd> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<dz_yksqd> GetAllList(int beg, int end)
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
        public IList<dz_yksqd> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
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
        public IList<dz_yksqd> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
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
        public IList<dz_yksqd> GetAllList(List<SqlParameter> paramter, string sqls)
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
