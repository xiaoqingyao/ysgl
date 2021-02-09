using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace Dal.SaleBill
{
    /// <summary>
    /// 
    /// </summary>
    public class T_SaleFeeSpendNoteDal
    {
        string sql = "select Remark,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,YsgcCode,Note0,Billcode,Deptcode,Yskmcode,Status,Fee,Sysdatetime,Sysusercode,Row_Number()over(order by Listnid) as crow from T_SaleFeeSpendNote";
        string sqlCont = "select count(*) from T_SaleFeeSpendNote";

        public bool Exists(int Listnid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" Listnid = @Listnid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Listnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Listnid;

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

        public int Add(T_SaleFeeSpendNote model)
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
        public void Delete(int Listnid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(Listnid, tran);
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
        public int  Add(T_SaleFeeSpendNote model, SqlTransaction tran)
        {
            Delete(model.Listnid, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_SaleFeeSpendNote(");
            strSql.Append("Remark,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,YsgcCode,Note0,Billcode,Deptcode,Yskmcode,Status,Fee,Sysdatetime,Sysusercode");
            strSql.Append(") values (");
            strSql.Append("@Remark,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@YsgcCode,@Note0,@Billcode,@Deptcode,@Yskmcode,@Status,@Fee,@Sysdatetime,@Sysusercode");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Remark", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@YsgcCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note0", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Billcode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Deptcode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Yskmcode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1) ,            
                        new SqlParameter("@Fee", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@Sysdatetime", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Sysusercode", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.Remark);

            parameters[1].Value = SqlNull(model.Note1);

            parameters[2].Value = SqlNull(model.Note2);

            parameters[3].Value = SqlNull(model.Note3);

            parameters[4].Value = SqlNull(model.Note4);

            parameters[5].Value = SqlNull(model.Note5);

            parameters[6].Value = SqlNull(model.Note6);

            parameters[7].Value = SqlNull(model.Note7);

            parameters[8].Value = SqlNull(model.Note8);

            parameters[9].Value = SqlNull(model.Note9);

            parameters[10].Value = SqlNull(model.YsgcCode);

            parameters[11].Value = SqlNull(model.Note0);

            parameters[12].Value = SqlNull(model.Billcode);

            parameters[13].Value = SqlNull(model.Deptcode);

            parameters[14].Value = SqlNull(model.Yskmcode);

            parameters[15].Value = SqlNull(model.Status);

            parameters[16].Value = SqlNull(model.Fee);

            parameters[17].Value = SqlNull(model.Sysdatetime);

            parameters[18].Value = SqlNull(model.Sysusercode);


           return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Listnid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SaleFeeSpendNote ");
            strSql.Append(" where Listnid=@Listnid");
            SqlParameter[] parameters = {
					new SqlParameter("@Listnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Listnid;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_SaleFeeSpendNote> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_SaleFeeSpendNote> list = new List<T_SaleFeeSpendNote>();
            foreach (DataRow dr in dt.Rows)
            {
                T_SaleFeeSpendNote model = new T_SaleFeeSpendNote();
                if (!DBNull.Value.Equals(dr["Listnid"]))
                {
                    model.Listnid = int.Parse(dr["Listnid"].ToString());
                }
                model.Remark = dr["Remark"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.YsgcCode = dr["YsgcCode"].ToString();
                model.Note0 = dr["Note0"].ToString();
                model.Billcode = dr["Billcode"].ToString();
                model.Deptcode = dr["Deptcode"].ToString();
                model.Yskmcode = dr["Yskmcode"].ToString();
                model.Status = dr["Status"].ToString();
                if (!DBNull.Value.Equals(dr["Fee"]))
                {
                    model.Fee = decimal.Parse(dr["Fee"].ToString());
                }
                model.Sysdatetime = dr["Sysdatetime"].ToString();
                model.Sysusercode = dr["Sysusercode"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_SaleFeeSpendNote GetModel(int Listnid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Listnid=@Listnid");
            SqlParameter[] parameters = {
					new SqlParameter("@Listnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Listnid;


            T_SaleFeeSpendNote model = new T_SaleFeeSpendNote();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["Listnid"]))
                    {
                        model.Listnid = int.Parse(dr["Listnid"].ToString());
                    }
                    model.Remark = dr["Remark"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.YsgcCode = dr["YsgcCode"].ToString();
                    model.Note0 = dr["Note0"].ToString();
                    model.Billcode = dr["Billcode"].ToString();
                    model.Deptcode = dr["Deptcode"].ToString();
                    model.Yskmcode = dr["Yskmcode"].ToString();
                    model.Status = dr["Status"].ToString();
                    if (!DBNull.Value.Equals(dr["Fee"]))
                    {
                        model.Fee = decimal.Parse(dr["Fee"].ToString());
                    }
                    model.Sysdatetime = dr["Sysdatetime"].ToString();
                    model.Sysusercode = dr["Sysusercode"].ToString();

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
        public IList<T_SaleFeeSpendNote> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_SaleFeeSpendNote> GetAllList(int beg, int end)
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


        public DataTable GetAllListBySql(T_SaleFeeSpendNote model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select a.*, b.billCode,b.billName,b.stepID,b.billDate,
            (select xmmc from bill_ysgc where gcbh=a.YsgcCode ) as ysgcname,
            (select '['+deptCode+']'+deptName from bill_departments where deptCode=a.Deptcode)as deptname,
            (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=a.Yskmcode )as yskmname,
            (select '['+userCode+']'+userName from bill_users where userCode=a.Sysusercode ) as usernames 
            from dbo.T_SaleFeeSpendNote a, bill_main b where a.Billcode=b.billCode");
            //日期起
            if (!string.IsNullOrEmpty(model.dateFrm))
            {
                strSql.Append(" and convert(varchar(10),sysDatetime,25)>='" + model.dateFrm + "'");
            }
            if (!string.IsNullOrEmpty(model.dateTo))
            {
                strSql.Append(" and convert(varchar(10),sysDatetime,25)<='" + model.dateTo + "'");
            }
            ////申请单号
            if (model.Billcode!=null&&model.Billcode!="")
            {
                strSql.Append(" and a.Billcode like '%" + model.Billcode + "%' ");
            }
           
            ////预算科目
            if (model.Yskmcode != null && model.Yskmcode!= "")
            {
                strSql.Append(" and a.Yskmcode='" + model.Yskmcode + "'");

            }
            ////预算过程
            if (model.YsgcCode != null && model.YsgcCode != "")
            {
                strSql.Append(" and a.YsgcCode='" + model.YsgcCode + "'");
            }
            ////单位
            if (model.Deptcode != null && model.Deptcode != "")
            {
                strSql.Append(" and a.Deptcode ='" + model.Deptcode + "'");


            }

            strSql.Append(" order by Listnid desc");

            return DataHelper.GetDataTable(strSql.ToString(), null, false);
        }
    }
}
