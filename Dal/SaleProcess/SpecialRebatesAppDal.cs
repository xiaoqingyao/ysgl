using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;
using System.Configuration;
using Dal.Bills;
namespace Dal.SaleProcess
{
    public class SpecialRebatesAppDal
    {
        string sql = "select Code,AppDate,SysPersionCode,SysDateTime,EffectiveDateFrm,EffectiveDateTo,Attachment,Explain,CheckAttachment,Note1,Note2,BillMainCode,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,TruckCode,TruckCount,SaleDeptCode,StandardSaleAmount,ExceedStandardPoint,Row_Number()over(order by Code) as crow from T_SpecialRebatesApp";
        string sqlCont = "select count(*) from T_SpecialRebatesApp";

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

        public int  Add(T_SpecialRebatesAppmode model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {

                 int iRel=Add(model, tran);
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
        /// 添加
        /// </summary>
        /// <param name="modellist"></param>
        /// <returns></returns>
        public bool Addlistmodel(IList<T_SpecialRebatesAppmode> modellist)
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Code)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    
                  int row=  Delete(Code, tran);
                  MainDal mdal = new MainDal();
                  mdal.DeleteMain(Code, tran);
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
        public int  Add(T_SpecialRebatesAppmode model, SqlTransaction tran)
        {
           // Delete(model.Code,tran);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into T_SpecialRebatesApp(");
            strSql.Append("Code,AppDate,SysPersionCode,SysDateTime,EffectiveDateFrm,EffectiveDateTo,Attachment,Explain,CheckAttachment,Note1,Note2,BillMainCode,Note3,Note4,Note5,Note6,Note7,Note8,Note9,Note10,TruckCode,TruckCount,SaleDeptCode,StandardSaleAmount,ExceedStandardPoint");
            strSql.Append(") values (");
            strSql.Append("@Code,@AppDate,@SysPersionCode,@SysDateTime,@EffectiveDateFrm,@EffectiveDateTo,@Attachment,@Explain,@CheckAttachment,@Note1,@Note2,@BillMainCode,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@TruckCode,@TruckCount,@SaleDeptCode,@StandardSaleAmount,@ExceedStandardPoint");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@Code", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@AppDate", SqlDbType.Char,10) ,            
                        new SqlParameter("@SysPersionCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SysDateTime", SqlDbType.VarChar,20) ,            
                        new SqlParameter("@EffectiveDateFrm", SqlDbType.Char,10) ,            
                        new SqlParameter("@EffectiveDateTo", SqlDbType.Char,10) ,            
                        new SqlParameter("@Attachment", SqlDbType.NVarChar,200) ,            
                        new SqlParameter("@Explain", SqlDbType.NVarChar,500) ,            
                        new SqlParameter("@CheckAttachment", SqlDbType.NVarChar,200) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BillMainCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                                 
                        new SqlParameter("@TruckCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@TruckCount", SqlDbType.Int,4) ,            
                                
                        new SqlParameter("@SaleDeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@StandardSaleAmount", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ExceedStandardPoint", SqlDbType.Decimal,9)             
              
            };

            parameters[0].Value = SqlNull(model.Code);

            parameters[1].Value = SqlNull(model.AppDate);

            parameters[2].Value = SqlNull(model.SysPersionCode);

            parameters[3].Value = SqlNull(model.SysDateTime);

            parameters[4].Value = SqlNull(model.EffectiveDateFrm);

            parameters[5].Value = SqlNull(model.EffectiveDateTo);

            parameters[6].Value = SqlNull(model.Attachment);

            parameters[7].Value = SqlNull(model.Explain);

            parameters[8].Value = SqlNull(model.CheckAttachment);

            parameters[9].Value = SqlNull(model.Note1);

            parameters[10].Value = SqlNull(model.Note2);

            parameters[11].Value = SqlNull(model.BillMainCode);

            parameters[12].Value = SqlNull(model.Note3);

            parameters[13].Value = SqlNull(model.Note4);

            parameters[14].Value = SqlNull(model.Note5);

            parameters[15].Value = SqlNull(model.Note6);

            parameters[16].Value = SqlNull(model.Note7);

            parameters[17].Value = SqlNull(model.Note8);

            parameters[18].Value = SqlNull(model.Note9);

            parameters[19].Value = SqlNull(model.Note10);

          

            parameters[20].Value = SqlNull(model.TruckCode);

            parameters[21].Value = SqlNull(model.TruckCount);

          

            parameters[22].Value = SqlNull(model.SaleDeptCode);

            parameters[23].Value = SqlNull(model.StandardSaleAmount);

            parameters[24].Value = SqlNull(model.ExceedStandardPoint);


         return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Code, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from T_SpecialRebatesApp ");
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<T_SpecialRebatesAppmode> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_SpecialRebatesAppmode> list = new List<T_SpecialRebatesAppmode>();
            foreach (DataRow dr in dt.Rows)
            {
                T_SpecialRebatesAppmode model = new T_SpecialRebatesAppmode();
                model.Code = dr["Code"].ToString();
                model.AppDate = dr["AppDate"].ToString();
                model.SysPersionCode = dr["SysPersionCode"].ToString();
                model.SysDateTime = dr["SysDateTime"].ToString();
                model.EffectiveDateFrm = dr["EffectiveDateFrm"].ToString();
                model.EffectiveDateTo = dr["EffectiveDateTo"].ToString();
                model.Attachment = dr["Attachment"].ToString();
                model.Explain = dr["Explain"].ToString();
                model.CheckAttachment = dr["CheckAttachment"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.BillMainCode = dr["BillMainCode"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
             
                model.TruckCode = dr["TruckCode"].ToString();
                if (!DBNull.Value.Equals(dr["TruckCount"]))
                {
                    model.TruckCount = int.Parse(dr["TruckCount"].ToString());
                }
            
                model.SaleDeptCode = dr["SaleDeptCode"].ToString();
                if (!DBNull.Value.Equals(dr["StandardSaleAmount"]))
                {
                    model.StandardSaleAmount = decimal.Parse(dr["StandardSaleAmount"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ExceedStandardPoint"]))
                {
                    model.ExceedStandardPoint = decimal.Parse(dr["ExceedStandardPoint"].ToString());
                }

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_SpecialRebatesAppmode GetModel(string Code)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where Code=@Code ");
            SqlParameter[] parameters = {
					new SqlParameter("@Code", SqlDbType.NVarChar,50)			};
            parameters[0].Value = Code;


            T_SpecialRebatesAppmode model = new T_SpecialRebatesAppmode();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Code = dr["Code"].ToString();
                    model.AppDate = dr["AppDate"].ToString();
                    model.SysPersionCode = dr["SysPersionCode"].ToString();
                    model.SysDateTime = dr["SysDateTime"].ToString();
                    model.EffectiveDateFrm = dr["EffectiveDateFrm"].ToString();
                    model.EffectiveDateTo = dr["EffectiveDateTo"].ToString();
                    model.Attachment = dr["Attachment"].ToString();
                    model.Explain = dr["Explain"].ToString();
                    model.CheckAttachment = dr["CheckAttachment"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.BillMainCode = dr["BillMainCode"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    
                    model.TruckCode = dr["TruckCode"].ToString();
                    if (!DBNull.Value.Equals(dr["TruckCount"]))
                    {
                        model.TruckCount = int.Parse(dr["TruckCount"].ToString());
                    }
                  
                    model.SaleDeptCode = dr["SaleDeptCode"].ToString();
                    if (!DBNull.Value.Equals(dr["StandardSaleAmount"]))
                    {
                        model.StandardSaleAmount = decimal.Parse(dr["StandardSaleAmount"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ExceedStandardPoint"]))
                    {
                        model.ExceedStandardPoint = decimal.Parse(dr["ExceedStandardPoint"].ToString());
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
        public IList<T_SpecialRebatesAppmode> GetAllList()
        {
            return ListMaker(sql, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userlogmode"></param>
        /// <returns></returns>
        public DataTable GetAllListBySql1(T_SpecialRebatesAppmode specimode)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();

            strSql.Append("select * from (select  flowid,bill_main.billCode,bill_main.billName,bill_main.stepID,bill_main.billDate,bill_main.billJe,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=bill_main.billDept ) as deptName,(select count(*) from workflowrecord where billcode=bill_main.billcode) as recordcount,(select top 1 rdState from workflowrecord where billcode=bill_main.billcode) as rdState from  bill_main ) a   where flowid='tsfl' ");
            // 
            //申请单号
            if (specimode.Code != null && specimode.Code != "")
            {
                strSql.Append(" and a.billCode like'%" + specimode.Code + "%'");


            }
            //申请日期始
            if (specimode.AppDate != null && specimode.AppDate != "")
            {
                strSql.Append(" and a.billDate>='" + specimode.AppDate + "'");

            }
           //申请日期末
            if (specimode.Note1!=null&&specimode.Note1!="")
            {
                strSql.Append(" and a.billDate<='" + specimode.Note1 + "'");
            }
            //TruckCode车架号
            if (specimode.TruckCode != null && specimode.TruckCode != "")
            {
                strSql.Append(" and a.billCode in (select Code from T_SpecialRebatesApp where  TruckCode like'%" + specimode.TruckCode + "%')");
            }
            if (!string.IsNullOrEmpty(specimode.Attachment))
            {
                switch (specimode.Attachment)
                {
                    case "end": strSql.Append(" and stepID='end' "); break;
                    case "-1": strSql.Append(" and stepID='-1' and recordcount=0"); break;//未提交
                    case "0": strSql.Append(" and stepID='-1' and recordcount!=0 and rdState!='3' "); break;//审核中 
                    case "1": strSql.Append(" and stepID='-1' and recordcount!=0 and rdState='3'"); break;//审核驳回
                    default:
                        break;
                }
            }
           //// EffectiveDateFrm有效期始
           // if (specimode.EffectiveDateFrm != null && specimode.EffectiveDateTo != "")
           // {
           //     strSql.Append(" and a.billCode in (select Code from T_SpecialRebatesApp where EffectiveDateFrm<='" + specimode.EffectiveDateFrm + "')");

           // }
           // //EffectiveDateTo有效期末
           // if (specimode.EffectiveDateTo != null && specimode.EffectiveDateTo != "")
           // {
           //     strSql.Append(" and a.billCode in (select Code from T_SpecialRebatesApp where EffectiveDateTo>='" + specimode.EffectiveDateTo + "')");

           // }
            //审批状态
            //if (specimode.Note3!=null&&specimode.Note3!="")
            //{
            //     strSql.Append(" and a.stepID='" + specimode.Note3 + "'");
            //}
            strSql.Append(" order by a.billCode desc");
         
            return DataHelper.GetDataTable(strSql.ToString(),null, false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specimode"></param>
        /// <returns></returns>
        public DataTable GetAllListBySqlPF(T_SpecialRebatesAppmode specimode)
        {
            StringBuilder strSql = new StringBuilder();
             strSql.Append( @"select  a.billCode,a.billName,a.stepID,a.billDate,a.billJe,
        (select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.billDept ) as deptName from  bill_main a   where a.flowid='tsfl'");
//            string strStart = @"select *,(case isnull(c.spStatus,'0') 
//                when '1' then '已批复' when '0' then '未批复' end) as endStatus  from
//                 (select a.billCode,a.billName,a.stepID,a.billDate,a.billJe,
//                (select top 1 status from T_SpecialRebatesStandard dd where AppBillCode= a.billCode) as spStatus,
//                (select '['+deptCode+']'+deptName from bill_departments where deptCode=b.SaleDeptCode) as deptName,
//                b.* from  bill_main a , T_SpecialRebatesApp b
//                 where b.BillMainCode=a.billCode {0}) c where 1=1 ";
            //StringBuilder strSql = new StringBuilder(" and 1=1");
            List<object> objParams = new List<object>();
            #region 根据model 产生查询条件
            //申请单号
            if (specimode.Code != null && specimode.Code != "")
            {
                strSql.Append(" and a.billCode like'%" + specimode.Code + "%'");
            }
            //申请日期始
            if (specimode.AppDate != null && specimode.AppDate != "")
            {
                strSql.Append(" and a.billDate>='" + specimode.AppDate + "'");
            }
            //申请日期末
            if (specimode.Note1 != null && specimode.Note1 != "")
            {
                strSql.Append(" and a.billDate<='" + specimode.Note1 + "'");
            }
            //TruckCode车架号
            if (specimode.TruckCode != null && specimode.TruckCode != "")
            {
                strSql.Append(" and a.billCode in (select Code from T_SpecialRebatesApp where  TruckCode like'%" + specimode.TruckCode + "%')");
            }

          
          
            //审批状态
            if (specimode.Note3 != null && specimode.Note3 != "")
            {
                strSql.Append(" and a.stepID='" + specimode.Note3 + "'");
            }
           
            #endregion
            //string strEnd = string.Format(strStart, strSql.ToString());
            //批复状态
            //if (specimode.Note4 != null && specimode.Note4 != "")
            //{
            //    if (specimode.Note4.Equals("1"))
            //    {
            //        //select top 1 Status from T_SpecialRebatesStandard  
            //        strSql.Append(" and a.billCode in(select AppBillCode  from T_SpecialRebatesStandard where Status='1')");
            //    }
            //    else
            //    {
            //        strSql.Append(" and a.billCode in(select AppBillCode  from T_SpecialRebatesStandard where Status<>'1')");
            //    }
            //}
            strSql.Append( " order by billDate desc");
            return DataHelper.GetDataTable(strSql.ToString(), null, false);

        }
        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_SpecialRebatesAppmode> GetAllList(int beg, int end)
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
