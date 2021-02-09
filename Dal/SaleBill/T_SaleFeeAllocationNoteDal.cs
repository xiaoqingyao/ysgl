using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.SaleBill
{
    /// <summary>
    /// 销售费用返利记录dal
    /// Edit by Lvcc
    /// </summary>
    public class T_SaleFeeAllocationNoteDal
    {

        string sql = "select Fee,Status,AuditUserCode,ActionNote,RebatesType,Remark,Note1,Note2,Note3,ActionDate,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ActionTimes,BillCode,TruckCode,TruckTypeCode,DeptCode,ControlItemCode,SaleFeeTypeCode,Row_Number()over(order by Nid) as crow from T_SaleFeeAllocationNote";
        string sqlCont = "select count(*) from T_SaleFeeAllocationNote";

        public bool Exists(long Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" Nid = @Nid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.BigInt)
			};
            parameters[0].Value = Nid;

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

        public int Add(T_SaleFeeAllocationNote model)
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
        public void Delete(long Nid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(Nid, tran);
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
        public int Add(T_SaleFeeAllocationNote model, SqlTransaction tran)
        {
            Delete(model.Nid, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_SaleFeeAllocationNote(");
            strSql.Append("Fee,Status,AuditUserCode,ActionNote,RebatesType,Remark,Note1,Note2,Note3,ActionDate,Note4,Note5,Note6,Note7,Note8,Note9,Note10,ActionTimes,BillCode,TruckCode,TruckTypeCode,DeptCode,ControlItemCode,SaleFeeTypeCode");
            strSql.Append(") values (");
            strSql.Append("@Fee,@Status,@AuditUserCode,@ActionNote,@RebatesType,@Remark,@Note1,@Note2,@Note3,@ActionDate,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@ActionTimes,@BillCode,@TruckCode,@TruckTypeCode,@DeptCode,@ControlItemCode,@SaleFeeTypeCode");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Fee", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1) ,            
                        new SqlParameter("@AuditUserCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ActionNote", SqlDbType.NVarChar,300) ,            
                        new SqlParameter("@RebatesType", SqlDbType.Char,1) ,            
                        new SqlParameter("@Remark", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ActionDate", SqlDbType.Char,10) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ActionTimes", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@BillCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckTypeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@DeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlItemCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SaleFeeTypeCode", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.Fee);

            parameters[1].Value = SqlNull(model.Status);

            parameters[2].Value = SqlNull(model.AuditUserCode);

            parameters[3].Value = SqlNull(model.ActionNote);

            parameters[4].Value = SqlNull(model.RebatesType);

            parameters[5].Value = SqlNull(model.Remark);

            parameters[6].Value = SqlNull(model.Note1);

            parameters[7].Value = SqlNull(model.Note2);

            parameters[8].Value = SqlNull(model.Note3);

            parameters[9].Value = SqlNull(model.ActionDate);

            parameters[10].Value = SqlNull(model.Note4);

            parameters[11].Value = SqlNull(model.Note5);

            parameters[12].Value = SqlNull(model.Note6);

            parameters[13].Value = SqlNull(model.Note7);

            parameters[14].Value = SqlNull(model.Note8);

            parameters[15].Value = SqlNull(model.Note9);

            parameters[16].Value = SqlNull(model.Note10);

            parameters[17].Value = SqlNull(model.ActionTimes);

            parameters[18].Value = SqlNull(model.BillCode);

            parameters[19].Value = SqlNull(model.TruckCode);

            parameters[20].Value = SqlNull(model.TruckTypeCode);

            parameters[21].Value = SqlNull(model.DeptCode);

            parameters[22].Value = SqlNull(model.ControlItemCode);

            parameters[23].Value = SqlNull(model.SaleFeeTypeCode);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public int Del(T_SaleFeeAllocationNote model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SaleFeeAllocationNote ");
            strSql.Append(" where TruckTypeCode=@TruckTypeCode and DeptCode=@DeptCode and SaleFeeTypeCode=@SaleFeeTypeCode");
            SqlParameter[] parameters = {
					new SqlParameter("@TruckTypeCode", model.TruckTypeCode),
                    new SqlParameter("DeptCode",model.DeptCode),
                    new SqlParameter("SaleFeeTypeCode",model.SaleFeeTypeCode)
			};
            // parameters[0].Value = Nid;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), parameters, false);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(long Nid, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SaleFeeAllocationNote ");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.BigInt)
			};
            parameters[0].Value = Nid;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_SaleFeeAllocationNote> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_SaleFeeAllocationNote> list = new List<T_SaleFeeAllocationNote>();
            foreach (DataRow dr in dt.Rows)
            {
                T_SaleFeeAllocationNote model = new T_SaleFeeAllocationNote();
                if (!DBNull.Value.Equals(dr["Nid"]))
                {
                    model.Nid = long.Parse(dr["Nid"].ToString());
                }
                if (!DBNull.Value.Equals(dr["Fee"]))
                {
                    model.Fee = decimal.Parse(dr["Fee"].ToString());
                }
                model.Status = dr["Status"].ToString();
                model.AuditUserCode = dr["AuditUserCode"].ToString();
                model.ActionNote = dr["ActionNote"].ToString();
                model.RebatesType = dr["RebatesType"].ToString();
                model.Remark = dr["Remark"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.ActionDate = dr["ActionDate"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.ActionTimes = dr["ActionTimes"].ToString();
                model.BillCode = dr["BillCode"].ToString();
                model.TruckCode = dr["TruckCode"].ToString();
                model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                model.DeptCode = dr["DeptCode"].ToString();
                model.ControlItemCode = dr["ControlItemCode"].ToString();
                model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_SaleFeeAllocationNote GetModel(long Nid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.BigInt)
			};
            parameters[0].Value = Nid;


            T_SaleFeeAllocationNote model = new T_SaleFeeAllocationNote();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["Nid"]))
                    {
                        model.Nid = long.Parse(dr["Nid"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["Fee"]))
                    {
                        model.Fee = decimal.Parse(dr["Fee"].ToString());
                    }
                    model.Status = dr["Status"].ToString();
                    model.AuditUserCode = dr["AuditUserCode"].ToString();
                    model.ActionNote = dr["ActionNote"].ToString();
                    model.RebatesType = dr["RebatesType"].ToString();
                    model.Remark = dr["Remark"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.ActionDate = dr["ActionDate"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.ActionTimes = dr["ActionTimes"].ToString();
                    model.BillCode = dr["BillCode"].ToString();
                    model.TruckCode = dr["TruckCode"].ToString();
                    model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                    model.DeptCode = dr["DeptCode"].ToString();
                    model.ControlItemCode = dr["ControlItemCode"].ToString();
                    model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();

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
        public IList<T_SaleFeeAllocationNote> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_SaleFeeAllocationNote> GetAllList(int beg, int end)
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
