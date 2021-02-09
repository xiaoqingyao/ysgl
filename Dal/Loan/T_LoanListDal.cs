using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using Dal.Bills;

namespace Dal.Loan
{
    public class T_LoanListDal
    {
        string sql = "select Listid,CJCode,ResponsibleCode,ResponsibleDate,ResponsibleSysTime,NOTE1,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,LoanCode,NOTE7,NOTE8,NOTE9,NOTE10,NOTE11,NOTE12,NOTE13,NOTE14,NOTE15,NOTE16,LoanDeptCode,NOTE17,NOTE18,NOTE19,NOTE20,LoanDate,LoanSystime,LoanMoney,LoanExplain,Status,SettleType,Row_Number()over(order by Listid) as crow from T_LoanList";
        string sqlCont = "select count(*) from T_LoanList";

        public bool Exists(string Listid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" Listid = @Listid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Listid", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Listid;

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

        public string GetSql(T_LoanList model,string type)
        {

           
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select (select ResponsibleCode from T_LoanList where Listid=a.billCode )as jkr,a.billuser as jbr, a.billCode,a.billName,( case a.stepID when '-1' then '未提交' when 'end' then '审批通过'  end) as stepID,a.billDate,a.billDept,a.billJe,
(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName,
(select  '['+usercode+']'+ username from bill_users where usercode=
(select LoanCode from T_LoanList where Listid=a.billCode ) ) as loanName,
(select   '['+deptCode+']'+ deptName from bill_departments
 where deptCode=(select LoanDeptCode from T_LoanList where Listid=a.billCode ) ) as loanDeptName,
(select LoanDate from T_LoanList where Listid=a.billCode )as loandate,
(select LoanSystime from T_LoanList where Listid=a.billCode )as Respondate,
(case (select Status from T_LoanList where Listid=a.billCode) 
when '1' then '借款' when '2' then '结算完毕' when '3' then '冲减中'  end) as loanStatus,
(select convert(decimal(18,2),isnull(Note3,0)) from T_LoanList where Listid=a.billCode) as Note3,
(a.billJe-(select convert(decimal(18,2),isnull(Note3,0)) from T_LoanList where Listid=a.billCode))as wcjmoney,
( case (select SettleType from T_LoanList where Listid=a.billCode) 
when '0' then '现金' when '1' then '单据冲减'  end) as loanType,
(select CJCode from T_LoanList where Listid=a.billCode) as cjcode,
(select '['+usercode+']'+ username from bill_users where usercode=a.billuser) as jbname,Row_Number()over(order by  a.billCode desc) as crow
,(case (select len(isnull(note7,'')) from t_loanlist where listid=a.billcode) when '0' then '否'  else  '是'  end) as isdj
,(select isnull(gys,'无') from bill_cgsp where cgbh=(select top 1 note7 from t_loanlist where listid=a.billcode)) as gys
,(select  '['+usercode+']'+ username from bill_users where usercode=
(select ResponsibleCode from T_LoanList where Listid=a.billCode ) ) as ResponsibleCode
,(select note4 from T_LoanList where Listid=a.billCode) as jksj  
,(select dicName from T_LoanList,bill_dataDic  where Listid=a.billCode and dicType='20' and dicCode=note6) as note6
,(select isnull(note4,0)-datediff(day,loandate,getdate()) from T_LoanList where Listid=a.billCode) as chaoqidays
,isnull((select datediff(day,loandate,(select top 1 ldate from T_ReturnNote where loancode=a.billCode order by ldate desc)) as chaoqidays_wanbi from t_loanlist  where Listid=a.billCode),0)-(select isnull(note4,0) from T_LoanList where Listid=a.billCode) as chaoqidays_wanbi
from  bill_main a   where  a.flowid=left('" + type+"',4)");

           
            //申请日期始
            if (model.LoanDate != null && model.LoanDate != "")
            {
                strSql.Append(" and (select LoanDate from T_LoanList where Listid=a.billCode )>='" + model.LoanDate + "'");
            }
            //申请日期末
            if (model.NOTE20 != null && model.NOTE20 != "")
            {
                strSql.Append(" and  (select LoanDate from T_LoanList where Listid=a.billCode )<='" + model.NOTE20 + "'");
            }

            ////申请单号
            if (model.Listid != null && model.Listid != "")
            {
                strSql.Append(" and a.billCode like'%" + model.Listid + "%'");
            }

            ////经办日期从
            if (model.ResponsibleDate != null && model.ResponsibleDate != "")
            {
                strSql.Append(" and (select ResponsibleDate from T_LoanList where Listid=a.billCode )>='" + model.ResponsibleDate + "'");

            }
            //经办日期末
            if (model.NOTE19 != null && model.NOTE19 != "")
            {
                strSql.Append(" and  (select ResponsibleDate from T_LoanList where Listid=a.billCode )<='" + model.NOTE19 + "'");
            }

            //经办人
            if (model.ResponsibleCode != null && model.ResponsibleCode != "")
            {
                strSql.Append(" and  (select ResponsibleCode from T_LoanList where Listid=a.billCode )='" + model.ResponsibleCode + "'");
            }
            //借款人
            if (model.LoanCode != null && model.LoanCode != "")
            {
                strSql.Append(" and  (select LoanCode from T_LoanList where Listid=a.billCode )='" + model.LoanCode + "'");
            }
            //// 缴款单位
            if (model.LoanDeptCode != null && model.LoanDeptCode != "")
            {
                strSql.Append(" and  (select LoanDeptCode from T_LoanList where Listid=a.billCode )='" + model.LoanDeptCode + "'");

            }
            //冲减状态
            if (model.Status != "" && model.Status != null)
            {
                if (model.Status == "4")
                {
                    strSql.Append(" and  (select Status from T_LoanList where Listid=a.billCode )in('1','3')");

                }
                else
                {
                    strSql.Append(" and  (select Status from T_LoanList where Listid=a.billCode )='" + model.Status + "'");

                }

            }
            //审批状态
            if (model.NOTE3 != "" && model.NOTE3 != null)
            {
                strSql.Append(" and  a.stepID='" + model.NOTE3 + "'");

            }
            if (!string.IsNullOrEmpty(model.NOTE21) && model.NOTE21.Equals("超期未还款"))
            {
                strSql.Append(" and  (select datediff(day,loandate,getdate()) from T_LoanList where Listid=a.billCode )  >(select note4 from T_LoanList where Listid=a.billCode ) ");
                //超期的只查看没有还款完毕的
                strSql.Append(" and (select Status from T_LoanList where Listid=a.billCode ) in ('1','3')");
            }
            else if (!string.IsNullOrEmpty(model.NOTE21) && model.NOTE21.Equals("临期超期未还款"))
            {
                strSql.Append(" and  (select datediff(day,loandate,getdate()) from T_LoanList where Listid=a.billCode )  ='"+model.NOTE22+"' ");
                //超期的只查看没有还款完毕的
                strSql.Append(" and (select Status from T_LoanList where Listid=a.billCode ) in ('1','3')");
            }
            return strSql.ToString();
        }
        public DataTable GetAllListBySql1(T_LoanList model,string type)
        {

            string sqlStr = GetSql(model,type);
            if (string.IsNullOrEmpty(sqlStr))
            {
                return null;
            }
            return DataHelper.GetDataTable(sqlStr, null, false);

        }


        public int Add(T_LoanList model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.Listid, tran);
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Listid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int row = Delete(Listid, tran);
                    DeleteItem(Listid,tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(Listid, tran);
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
        public int Add(T_LoanList model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_LoanList(");
            strSql.Append("Listid,CJCode,ResponsibleCode,ResponsibleDate,ResponsibleSysTime,NOTE1,NOTE2,NOTE3,NOTE4,NOTE5,NOTE6,LoanCode,NOTE7,NOTE8,NOTE9,NOTE10,NOTE11,NOTE12,NOTE13,NOTE14,NOTE15,NOTE16,LoanDeptCode,NOTE17,NOTE18,NOTE19,NOTE20,LoanDate,LoanSystime,LoanMoney,LoanExplain,Status,SettleType");
            strSql.Append(") values (");
            strSql.Append("@Listid,@CJCode,@ResponsibleCode,@ResponsibleDate,@ResponsibleSysTime,@NOTE1,@NOTE2,@NOTE3,@NOTE4,@NOTE5,@NOTE6,@LoanCode,@NOTE7,@NOTE8,@NOTE9,@NOTE10,@NOTE11,@NOTE12,@NOTE13,@NOTE14,@NOTE15,@NOTE16,@LoanDeptCode,@NOTE17,@NOTE18,@NOTE19,@NOTE20,@LoanDate,@LoanSystime,@LoanMoney,@LoanExplain,@Status,@SettleType");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Listid", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@CJCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ResponsibleCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ResponsibleDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ResponsibleSysTime", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE5", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@NOTE6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LoanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE11", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE12", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE13", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE14", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE15", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE16", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LoanDeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE17", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE18", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE19", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@NOTE20", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LoanDate", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LoanSystime", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LoanMoney", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@LoanExplain", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1) ,            
                        new SqlParameter("@SettleType", SqlDbType.Char,1)             
              
            };

            parameters[0].Value = SqlNull(model.Listid);

            parameters[1].Value = SqlNull(model.CJCode);

            parameters[2].Value = SqlNull(model.ResponsibleCode);

            parameters[3].Value = SqlNull(model.ResponsibleDate);

            parameters[4].Value = SqlNull(model.ResponsibleSysTime);

            parameters[5].Value = SqlNull(model.NOTE1);

            parameters[6].Value = SqlNull(model.NOTE2);

            parameters[7].Value = SqlNull(model.NOTE3);

            parameters[8].Value = SqlNull(model.NOTE4);

            parameters[9].Value = SqlNull(model.NOTE5);

            parameters[10].Value = SqlNull(model.NOTE6);

            parameters[11].Value = SqlNull(model.LoanCode);

            parameters[12].Value = SqlNull(model.NOTE7);

            parameters[13].Value = SqlNull(model.NOTE8);

            parameters[14].Value = SqlNull(model.NOTE9);

            parameters[15].Value = SqlNull(model.NOTE10);

            parameters[16].Value = SqlNull(model.NOTE11);

            parameters[17].Value = SqlNull(model.NOTE12);

            parameters[18].Value = SqlNull(model.NOTE13);

            parameters[19].Value = SqlNull(model.NOTE14);

            parameters[20].Value = SqlNull(model.NOTE15);

            parameters[21].Value = SqlNull(model.NOTE16);

            parameters[22].Value = SqlNull(model.LoanDeptCode);

            parameters[23].Value = SqlNull(model.NOTE17);

            parameters[24].Value = SqlNull(model.NOTE18);

            parameters[25].Value = SqlNull(model.NOTE19);

            parameters[26].Value = SqlNull(model.NOTE20);

            parameters[27].Value = SqlNull(model.LoanDate);

            parameters[28].Value = SqlNull(model.LoanSystime);

            parameters[29].Value = SqlNull(model.LoanMoney);

            parameters[30].Value = SqlNull(model.LoanExplain);

            parameters[31].Value = SqlNull(model.Status);

            parameters[32].Value = SqlNull(model.SettleType);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Listid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_LoanList ");
            strSql.Append(" where Listid=@Listid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Listid", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Listid;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int DeleteItem(string Listid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from t_returnnote ");
            strSql.Append(" where loancode=@loancode ");
            SqlParameter[] parameters = {
					new SqlParameter("@loancode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Listid;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 修改已还款金额
        /// </summary>
        public int UpdateYhkje(string listid,decimal  je,string Status, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  T_LoanList ");
            strSql.Append(" set  ");
            strSql.Append(" note3=convert( decimal(18,2),isnull(note3,0))+(@je) ");
            if (!string.IsNullOrEmpty(Status))
            {
                strSql.Append(" ,Status=@Status  ");
            }
            strSql.Append(" where listid=@listid ");
            SqlParameter[] parameters = {
					new SqlParameter("@je", SqlDbType.Decimal),
					new SqlParameter("@Status", SqlDbType.VarChar),
					new SqlParameter("@listid", SqlDbType.NVarChar,50)			};
            parameters[0].Value = je;
            parameters[1].Value = Status;
            parameters[2].Value = listid;

            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }
        
         /// <summary>
        /// 修改已还款金额
        /// </summary>
        public int UpdateZT(string billcode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  t_returnnote ");
            strSql.Append(" set  ");
            strSql.Append(" note1='1' ");
            strSql.Append(" where billcode=@billcode ");
            SqlParameter[] parameters = {
					new SqlParameter("@billcode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = billcode;

            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }
        public IList<T_LoanList> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_LoanList> list = new List<T_LoanList>();
            foreach (DataRow dr in dt.Rows)
            {
                T_LoanList model = new T_LoanList();
                model.Listid = dr["Listid"].ToString();
                model.CJCode = dr["CJCode"].ToString();
                model.ResponsibleCode = dr["ResponsibleCode"].ToString();
                model.ResponsibleDate = dr["ResponsibleDate"].ToString();
                model.ResponsibleSysTime = dr["ResponsibleSysTime"].ToString();
                model.NOTE1 = dr["NOTE1"].ToString();
                model.NOTE2 = dr["NOTE2"].ToString();
                model.NOTE3 = dr["NOTE3"].ToString();
                model.NOTE4 = dr["NOTE4"].ToString();
                model.NOTE5 = dr["NOTE5"].ToString();
                model.NOTE6 = dr["NOTE6"].ToString();
                model.LoanCode = dr["LoanCode"].ToString();
                model.NOTE7 = dr["NOTE7"].ToString();
                model.NOTE8 = dr["NOTE8"].ToString();
                model.NOTE9 = dr["NOTE9"].ToString();
                model.NOTE10 = dr["NOTE10"].ToString();
                model.NOTE11 = dr["NOTE11"].ToString();
                model.NOTE12 = dr["NOTE12"].ToString();
                model.NOTE13 = dr["NOTE13"].ToString();
                model.NOTE14 = dr["NOTE14"].ToString();
                model.NOTE15 = dr["NOTE15"].ToString();
                model.NOTE16 = dr["NOTE16"].ToString();
                model.LoanDeptCode = dr["LoanDeptCode"].ToString();
                model.NOTE17 = dr["NOTE17"].ToString();
                model.NOTE18 = dr["NOTE18"].ToString();
                model.NOTE19 = dr["NOTE19"].ToString();
                model.NOTE20 = dr["NOTE20"].ToString();
                model.LoanDate = dr["LoanDate"].ToString();
                model.LoanSystime = dr["LoanSystime"].ToString();
                if (!DBNull.Value.Equals(dr["LoanMoney"]))
                {
                    model.LoanMoney = decimal.Parse(dr["LoanMoney"].ToString());
                }
                model.LoanExplain = dr["LoanExplain"].ToString();
                model.Status = dr["Status"].ToString();
                model.SettleType = dr["SettleType"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_LoanList GetModel(string Listid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Listid=@Listid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Listid", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Listid;


            T_LoanList model = new T_LoanList();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Listid = dr["Listid"].ToString();
                    model.CJCode = dr["CJCode"].ToString();
                    model.ResponsibleCode = dr["ResponsibleCode"].ToString();
                    model.ResponsibleDate = dr["ResponsibleDate"].ToString();
                    model.ResponsibleSysTime = dr["ResponsibleSysTime"].ToString();
                    model.NOTE1 = dr["NOTE1"].ToString();
                    model.NOTE2 = dr["NOTE2"].ToString();
                    model.NOTE3 = dr["NOTE3"].ToString();
                    model.NOTE4 = dr["NOTE4"].ToString();
                    model.NOTE5 = dr["NOTE5"].ToString();
                    model.NOTE6 = dr["NOTE6"].ToString();
                    model.LoanCode = dr["LoanCode"].ToString();
                    model.NOTE7 = dr["NOTE7"].ToString();
                    model.NOTE8 = dr["NOTE8"].ToString();
                    model.NOTE9 = dr["NOTE9"].ToString();
                    model.NOTE10 = dr["NOTE10"].ToString();
                    model.NOTE11 = dr["NOTE11"].ToString();
                    model.NOTE12 = dr["NOTE12"].ToString();
                    model.NOTE13 = dr["NOTE13"].ToString();
                    model.NOTE14 = dr["NOTE14"].ToString();
                    model.NOTE15 = dr["NOTE15"].ToString();
                    model.NOTE16 = dr["NOTE16"].ToString();
                    model.LoanDeptCode = dr["LoanDeptCode"].ToString();
                    model.NOTE17 = dr["NOTE17"].ToString();
                    model.NOTE18 = dr["NOTE18"].ToString();
                    model.NOTE19 = dr["NOTE19"].ToString();
                    model.NOTE20 = dr["NOTE20"].ToString();
                    model.LoanDate = dr["LoanDate"].ToString();
                    model.LoanSystime = dr["LoanSystime"].ToString();
                    if (!DBNull.Value.Equals(dr["LoanMoney"]))
                    {
                        model.LoanMoney = decimal.Parse(dr["LoanMoney"].ToString());
                    }
                    model.LoanExplain = dr["LoanExplain"].ToString();
                    model.Status = dr["Status"].ToString();
                    model.SettleType = dr["SettleType"].ToString();

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
        public IList<T_LoanList> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_LoanList> GetAllList(int beg, int end)
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

        public string GetMainCode(string billcode)
        {
        return DataHelper.ExecuteScalar("select loancode from t_returnNote where billcode='"+billcode+"'",null,false).ToString();
        }


    }
}
