using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using Dal.Bills;

namespace Dal.SaleProcess
{
    public class RemittanceDal
    {

        string sql = "select NID, RemittanceUse,SystemDate,SystemuserCode,OrderCodeDate,Accessories,NOTE1,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,PaymentDeptName,NOTE7,NOTE8,NOTE9,NOTE0,PaymentDeptCode,OrderCode,TruckCode,RemittanceNumber,RemittanceDate,RemittanceType,RemittanceMoney,Row_Number()over(order by OrderCode) as crow from T_Remittance where 1=1";
        string sqlCont = "select count(*) from T_Remittance";

        public bool Exists(string strOrderCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" NID = @NID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = strOrderCode;

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

        public int Add(T_Remittance model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                    int row = Add(model, tran);
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

        public bool Addlistmodel(IList<T_Remittance> modellist)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in modellist)
                    {
                        int row = Add(i, tran);
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
        public DataTable GetAllListBySql1(T_Remittance model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select  a.billCode,a.billName,a.stepID,a.billDate,a.billDept,a.billJe,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName
            from  bill_main a   where a.flowid='cksj'");
          //  strSql.Append(@"select a.billCode,a.billName,a.stepID,a.billDate,a.billJe,b.*,'['+b.PaymentDeptCode+']'+b.PaymentDeptName as deptnames from 
              //   bill_main a ,T_Remittance b  where 1=1 and b.NID=a.billCode");



            //订单号
            if (model.OrderCode != null && model.OrderCode != "")
            {
                strSql.Append(" and a.billCode like'%" + model.OrderCode + "%'");
            }
            //申请日期始
            if (model.RemittanceDate != null && model.RemittanceDate != "")
            {
                strSql.Append(" and a.billDate>='" + model.RemittanceDate + "'");

            }
            //申请日期末
            if (model.NOTE1 != null && model.NOTE1 != "")
            {
                strSql.Append(" and  a.billDate<='" + model.NOTE1 + "'");
            }
            //TruckCode车架号
            if (model.TruckCode != null && model.TruckCode != "")
            {
                strSql.Append(" and a.billCode in (select NID from T_Remittance where TruckCode like'%" + model.TruckCode + "%')");


            }

            // 缴款单位
            if (model.PaymentDeptCode != null && model.PaymentDeptCode != "")
            {
                strSql.Append(" and billDept='" + model.PaymentDeptCode + "'");

            }
            ////回款形式
            //if (model.RemittanceType != null && model.RemittanceType != "")
            //{
            //    strSql.Append(" and RemittanceType like'%" + model.RemittanceType + "%'");

            //}
            ////审批状态


            strSql.Append(" order by a.billDate desc");

            return DataHelper.GetDataTable(strSql.ToString(), null, false);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string NID)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int row = Delete(NID, tran);

                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(NID, tran);
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
        public int Add(T_Remittance model, SqlTransaction tran)
        {
            //Delete(model.NID, tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_Remittance(");
            strSql.Append("RemittanceUse,SystemDate,SystemuserCode,OrderCodeDate,Accessories,NOTE1,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,PaymentDeptName,NOTE7,NOTE8,NOTE9,NOTE0,PaymentDeptCode,OrderCode,TruckCode,RemittanceNumber,RemittanceDate,RemittanceType,RemittanceMoney,NID");
            strSql.Append(") values (");
            strSql.Append("@RemittanceUse,@SystemDate,@SystemuserCode,@OrderCodeDate,@Accessories,@NOTE1,@NOTE2,@NOTE3,@NOTE4,@NOTE5,@NOTE6,@PaymentDeptName,@NOTE7,@NOTE8,@NOTE9,@NOTE0,@PaymentDeptCode,@OrderCode,@TruckCode,@RemittanceNumber,@RemittanceDate,@RemittanceType,@RemittanceMoney,@NID");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@RemittanceUse", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SystemDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SystemuserCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@OrderCodeDate", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@Accessories",SqlDbType.NVarChar,50),
                        new SqlParameter("@NOTE1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@PaymentDeptName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE0", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@PaymentDeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@OrderCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RemittanceNumber", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RemittanceDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RemittanceType", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@RemittanceMoney", SqlDbType.Decimal,9) ,
                        new SqlParameter("@NID",SqlDbType.NVarChar,50)
              
            };

            parameters[0].Value = SqlNull(model.RemittanceUse);

            parameters[1].Value = SqlNull(model.SystemDate);

            parameters[2].Value = SqlNull(model.SystemuserCode);

            parameters[3].Value = SqlNull(model.OrderCodeDate);

            parameters[4].Value = SqlNull(model.Accessories);

            parameters[5].Value = SqlNull(model.NOTE1);

            parameters[6].Value = SqlNull(model.NOTE2);

            parameters[7].Value = SqlNull(model.NOTE3);

            parameters[8].Value = SqlNull(model.NOTE4);

            parameters[9].Value = SqlNull(model.NOTE5);

            parameters[10].Value = SqlNull(model.NOTE6);

            parameters[11].Value = SqlNull(model.PaymentDeptName);

            parameters[12].Value = SqlNull(model.NOTE7);

            parameters[13].Value = SqlNull(model.NOTE8);

            parameters[14].Value = SqlNull(model.NOTE9);

            parameters[15].Value = SqlNull(model.NOTE0);

            parameters[16].Value = SqlNull(model.PaymentDeptCode);

            parameters[17].Value = SqlNull(model.OrderCode);

            parameters[18].Value = SqlNull(model.TruckCode);

            parameters[19].Value = SqlNull(model.RemittanceNumber);

            parameters[20].Value = SqlNull(model.RemittanceDate);

            parameters[21].Value = SqlNull(model.RemittanceType);

            parameters[22].Value = SqlNull(model.RemittanceMoney);
            parameters[23].Value = SqlNull(model.NID);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string NID, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_Remittance ");
            strSql.Append(" where NID=@NID");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = NID;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_Remittance> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_Remittance> list = new List<T_Remittance>();
            foreach (DataRow dr in dt.Rows)
            {
                T_Remittance model = new T_Remittance();
                model.NID = dr["NID"].ToString();
                model.RemittanceUse = dr["RemittanceUse"].ToString();
                model.SystemDate = dr["SystemDate"].ToString();
                model.SystemuserCode = dr["SystemuserCode"].ToString();
                model.OrderCodeDate = dr["OrderCodeDate"].ToString();
                model.Accessories = dr["Accessories"].ToString();
                model.NOTE1 = dr["NOTE1"].ToString();
                model.NOTE2 = dr["NOTE2"].ToString();
                model.NOTE3 = dr["NOTE3"].ToString();
                model.NOTE4 = dr["NOTE4"].ToString();
                model.NOTE5 = dr["NOTE5"].ToString();
                model.NOTE6 = dr["NOTE6"].ToString();
                model.PaymentDeptName = dr["PaymentDeptName"].ToString();
                model.NOTE7 = dr["NOTE7"].ToString();
                model.NOTE8 = dr["NOTE8"].ToString();
                model.NOTE9 = dr["NOTE9"].ToString();
                model.NOTE0 = dr["NOTE0"].ToString();
                model.PaymentDeptCode = dr["PaymentDeptCode"].ToString();
                model.OrderCode = dr["OrderCode"].ToString();
                model.TruckCode = dr["TruckCode"].ToString();
                model.RemittanceNumber = dr["RemittanceNumber"].ToString();
                model.RemittanceDate = dr["RemittanceDate"].ToString();
                model.RemittanceType = dr["RemittanceType"].ToString();
                if (!DBNull.Value.Equals(dr["RemittanceMoney"]))
                {
                    model.RemittanceMoney = decimal.Parse(dr["RemittanceMoney"].ToString());
                }

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_Remittance GetModel(string strOrderCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" and NID=@NID");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = strOrderCode;


            T_Remittance model = new T_Remittance();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.NID = dr["NID"].ToString();
                    model.RemittanceUse = dr["RemittanceUse"].ToString();
                    model.SystemDate = dr["SystemDate"].ToString();
                    model.SystemuserCode = dr["SystemuserCode"].ToString();
                    model.OrderCodeDate = dr["OrderCodeDate"].ToString();
                    model.Accessories = dr["Accessories"].ToString();
                    model.NOTE1 = dr["NOTE1"].ToString();
                    model.NOTE2 = dr["NOTE2"].ToString();
                    model.NOTE3 = dr["NOTE3"].ToString();
                    model.NOTE4 = dr["NOTE4"].ToString();
                    model.NOTE5 = dr["NOTE5"].ToString();
                    model.NOTE6 = dr["NOTE6"].ToString();
                    model.PaymentDeptName = dr["PaymentDeptName"].ToString();
                    model.NOTE7 = dr["NOTE7"].ToString();
                    model.NOTE8 = dr["NOTE8"].ToString();
                    model.NOTE9 = dr["NOTE9"].ToString();
                    model.NOTE0 = dr["NOTE0"].ToString();
                    model.PaymentDeptCode = dr["PaymentDeptCode"].ToString();
                    model.OrderCode = dr["OrderCode"].ToString();
                    model.TruckCode = dr["TruckCode"].ToString();
                    model.RemittanceNumber = dr["RemittanceNumber"].ToString();
                    model.RemittanceDate = dr["RemittanceDate"].ToString();
                    model.RemittanceType = dr["RemittanceType"].ToString();
                    if (!DBNull.Value.Equals(dr["RemittanceMoney"]))
                    {
                        model.RemittanceMoney = decimal.Parse(dr["RemittanceMoney"].ToString());
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
        public IList<T_Remittance> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_Remittance> GetAllList(int beg, int end)
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
