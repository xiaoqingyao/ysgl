using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;

namespace Dal.SaleProcess
{
    public class RebatesStandardDal
    {
        string sql = "select Type,SaleProcessCode,Remark,AuditUserCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,EffectiveDateFrm,Note9,Note10,EffectiveDateTo,SaleCountFrm,SaleCountTo,TruckTypeCode,DeptCode,SaleFeeTypeCode,ControlItemCode,Fee,Status,Row_Number()over(order by NID) as crow from T_RebatesStandard";
        string sqlCont = "select count(*) from T_RebatesStandard";

        public bool Exists(long NID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" NID = @NID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.BigInt)
			};
            parameters[0].Value = NID;

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

        public int Add(T_RebatesStandard model)
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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string strcartype, string strdeptcode, string strfeecode, string strconitemcode, string strEffictFrm, string strEffictTo, int intSaleCountFrm, int intSaleCountTo)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(strcartype, strdeptcode, strfeecode, strconitemcode, strEffictFrm, strEffictTo, intSaleCountFrm, intSaleCountTo, tran);
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
        public int Add(T_RebatesStandard model, SqlTransaction tran)
        {
          
            StringBuilder strSql = new StringBuilder();
          

                this.Delete(model.TruckTypeCode, model.DeptCode, model.SaleFeeTypeCode, model.ControlItemCode, model.EffectiveDateFrm, model.EffectiveDateTo, model.SaleCountFrm, model.SaleCountTo);
                
                strSql.Append("insert into T_RebatesStandard(");
                strSql.Append("Type,SaleProcessCode,Remark,AuditUserCode,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,EffectiveDateFrm,Note9,Note10,EffectiveDateTo,SaleCountFrm,SaleCountTo,TruckTypeCode,DeptCode,SaleFeeTypeCode,ControlItemCode,Fee,Status");
                strSql.Append(") values (");
                strSql.Append("@Type,@SaleProcessCode,@Remark,@AuditUserCode,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@EffectiveDateFrm,@Note9,@Note10,@EffectiveDateTo,@SaleCountFrm,@SaleCountTo,@TruckTypeCode,@DeptCode,@SaleFeeTypeCode,@ControlItemCode,@Fee,@Status");
                strSql.Append(") ");
                SqlParameter[] parameters = {
			            new SqlParameter("@Type", SqlDbType.Char,1) , 
                        new SqlParameter("@SaleProcessCode",SqlDbType.NVarChar,50),
                        new SqlParameter("@Remark", SqlDbType.NVarChar,50) ,
                        new SqlParameter("@AuditUserCode",SqlDbType.NVarChar,50),
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@EffectiveDateFrm", SqlDbType.Char,10) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@EffectiveDateTo", SqlDbType.Char,10) ,
                        new SqlParameter("@SaleCountFrm",SqlDbType.Int,4),
                        new SqlParameter("@SaleCountTo",SqlDbType.Int,4),
                        new SqlParameter("@TruckTypeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@DeptCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@SaleFeeTypeCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ControlItemCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Fee", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@Status", SqlDbType.Char,1)             
              
            };

                parameters[0].Value = SqlNull(model.Type);
                parameters[1].Value = SqlNull(model.SaleProcessCode);

                parameters[2].Value = SqlNull(model.Remark);
                parameters[3].Value = SqlNull(model.AuditUserCode);
                parameters[4].Value = SqlNull(model.Note1);

                parameters[5].Value = SqlNull(model.Note2);

                parameters[6].Value = SqlNull(model.Note3);

                parameters[7].Value = SqlNull(model.Note4);

                parameters[8].Value = SqlNull(model.Note5);

                parameters[9].Value = SqlNull(model.Note6);

                parameters[10].Value = SqlNull(model.Note7);

                parameters[11].Value = SqlNull(model.Note8);

                parameters[12].Value = SqlNull(model.EffectiveDateFrm);

                parameters[13].Value = SqlNull(model.Note9);

                parameters[14].Value = SqlNull(model.Note10);

                parameters[15].Value = SqlNull(model.EffectiveDateTo);
                parameters[16].Value = SqlNull(model.SaleCountFrm);
                parameters[17].Value = SqlNull(model.SaleCountTo);

                parameters[18].Value = SqlNull(model.TruckTypeCode);

                parameters[19].Value = SqlNull(model.DeptCode);

                parameters[20].Value = SqlNull(model.SaleFeeTypeCode);

                parameters[21].Value = SqlNull(model.ControlItemCode);

                parameters[22].Value = SqlNull(model.Fee);

                parameters[23].Value = SqlNull(model.Status);
                return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);


           
          

        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string strcartype, string strdeptcode, string strfeecode, string strconitemcoed, string strEffctiveFrm, string strEfficeiveTo, int intSaleCountFrm, int intSaleCountTo, SqlTransaction tran)
        {
           
            try
            {
               
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("delete from T_RebatesStandard ");
                    strSql.Append(" where TruckTypeCode=@TruckTypeCode and DeptCode=@DeptCode and SaleFeeTypeCode=@SaleFeeTypeCode and ControlItemCode=@ControlItemCode and EffectiveDateFrm=@EffectiveDateFrm and EffectiveDateTo=@EffectiveDateTo and SaleCountFrm=@SaleCountFrm and SaleCountTo=@SaleCountTo");
                    SqlParameter[] parameters = {
					new SqlParameter("@TruckTypeCode", strcartype),
                    new SqlParameter("@DeptCode",strdeptcode),
                    new SqlParameter("@SaleFeeTypeCode",strfeecode),
                    new SqlParameter("@ControlItemCode",strconitemcoed),
                    new SqlParameter("@EffectiveDateFrm",strEffctiveFrm),
                    new SqlParameter("@EffectiveDateTo",strEfficeiveTo),
                    new SqlParameter("@SaleCountFrm",intSaleCountFrm),
                    new SqlParameter("@SaleCountTo",intSaleCountTo)
			        };
                  
                    DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);

               
            }
            catch (Exception ex)
            {
                throw;
            }


        }


        public IList<T_RebatesStandard> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<T_RebatesStandard> list = new List<T_RebatesStandard>();
            foreach (DataRow dr in dt.Rows)
            {
                T_RebatesStandard model = new T_RebatesStandard();
                if (!DBNull.Value.Equals(dr["NID"]))
                {
                    model.NID = long.Parse(dr["NID"].ToString());
                }
                model.Type = dr["Type"].ToString();
                model.SaleProcessCode = dr["SaleProcessCode"].ToString();
                model.Remark = dr["Remark"].ToString();
                model.AuditUserCode = dr["AuditUserCode"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.EffectiveDateFrm = dr["EffectiveDateFrm"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.EffectiveDateTo = dr["EffectiveDateTo"].ToString();
                model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                model.DeptCode = dr["DeptCode"].ToString();
                model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();
                model.ControlItemCode = dr["ControlItemCode"].ToString();
                model.SaleCountFrm = int.Parse(dr["SaleCountFrm"].ToString());
                model.SaleCountTo = int.Parse(dr["SaleCountTo"].ToString());
                if (!DBNull.Value.Equals(dr["Fee"]))
                {
                    model.Fee = decimal.Parse(dr["Fee"].ToString());
                }
                model.Status = dr["Status"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public T_RebatesStandard GetModel(long NID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where NID=@NID");
            SqlParameter[] parameters = {
					new SqlParameter("@NID", SqlDbType.BigInt)
			};
            parameters[0].Value = NID;


            T_RebatesStandard model = new T_RebatesStandard();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    if (!DBNull.Value.Equals(dr["NID"]))
                    {
                        model.NID = long.Parse(dr["NID"].ToString());
                    }
                    model.Type = dr["Type"].ToString();
                    model.SaleProcessCode = dr["SaleProcessCode"].ToString();
                    model.Remark = dr["Remark"].ToString();
                    model.AuditUserCode = dr["AuditUserCode"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.EffectiveDateFrm = dr["EffectiveDateFrm"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.EffectiveDateTo = dr["EffectiveDateTo"].ToString();
                    model.TruckTypeCode = dr["TruckTypeCode"].ToString();
                    model.DeptCode = dr["DeptCode"].ToString();
                    model.SaleFeeTypeCode = dr["SaleFeeTypeCode"].ToString();
                    model.ControlItemCode = dr["ControlItemCode"].ToString();
                    model.SaleCountFrm = int.Parse(dr["SaleCountFrm"].ToString());
                    model.SaleCountTo = int.Parse(dr["SaleCountTo"].ToString());
                    if (!DBNull.Value.Equals(dr["Fee"]))
                    {
                        model.Fee = decimal.Parse(dr["Fee"].ToString());
                    }
                    model.Status = dr["Status"].ToString();

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
        public IList<T_RebatesStandard> GetAllList()
        {
            return ListMaker(sql, null);
        }
        /// <summary>
        /// 获取table类型所有行
        /// </summary>
        /// <returns></returns>
        public DataTable GetAlltable(T_RebatesStandard model)
        {
            StringBuilder sbQuery = new StringBuilder();

            List<SqlParameter> lstParams = new List<SqlParameter>();
            string strsql = @"select a.* ,(select '['+deptCode+']'+deptName from bill_departments where deptCode=a.DeptCode )as deptname,
                (select '['+yskmCode+']'+ yskmMc from bill_yskm where yskmCode=a.SaleFeeTypeCode)as feename,
                (select '['+CAST(typeCode AS nvarchar)+']'+typeName as namecode from T_truckType where CAST(typeCode AS varchar(100))=a.TruckTypeCode) as caname,
                (case [Type] when '0' then '期初分配' when '1' then '销售提成' when '2' then '配置项' end )as alltype,
               isnull( (case ControlItemCode  when (select Code from T_SaleProcess where Code=a.ControlItemCode) 
                 then (select  '['+Code+']'+PName from T_SaleProcess where Code=a.ControlItemCode) 
                when (select Code from  T_ControlItem where Code=a.ControlItemCode)
                 then (select'['+Code+']'+ CName from  T_ControlItem where Code=a.ControlItemCode)
                end),'期初分配')as feekz,
				(case Status when '1' then '未批复' when '0' then '禁用' when '2' then '已批复' end) as astatus,
                (select '['+userCode+']'+userName from bill_users where userCode=a.AuditUserCode)as username
                 from dbo.T_RebatesStandard a  where 1=1";

            if (model.Status!=null&&model.Status.Trim()!="")
            {
                sbQuery.Append(" and a.Status='"+model.Status+"'");
            }
            if (model.TruckTypeCode != null && model.TruckTypeCode.Trim() != "")
            {
                sbQuery.Append(" and TruckTypeCode ='" + model.TruckTypeCode + "'");

            }
            if (model.DeptCode != null && model.DeptCode.Trim() != "")
            {

                sbQuery.Append(" and DeptCode='" + model.DeptCode + "'");

            }
            if (model.SaleFeeTypeCode != null && model.SaleFeeTypeCode.Trim() != "")
            {
                sbQuery.Append(" and SaleFeeTypeCode like '" + model.SaleFeeTypeCode + "%'");

            }
            if (model.SaleProcessCode != null && model.SaleProcessCode != "")
            {
                sbQuery.Append(" and ControlItemCode='" + model.SaleProcessCode + "'");

            }

            if (model.Type != null && model.Type.Trim() != "")
            {
                sbQuery.Append(" and Type='" + model.Type + "'");

            }
            if (model.Status != null && model.Status.Trim() != "")
            {
                sbQuery.Append(" and Status='" + model.Status + "'");

            }
            if (model.EffectiveDateFrm != null && model.EffectiveDateFrm != "")
            {
                sbQuery.Append(" and EffectiveDateFrm<='" + model.EffectiveDateFrm + "'");
            }
            if (model.EffectiveDateTo != null && model.EffectiveDateTo != "")
            {
                sbQuery.Append(" and EffectiveDateTo>='" + model.EffectiveDateTo + "'");
            }
            strsql += sbQuery.ToString();
            strsql += " order by nid desc";

            return DataHelper.GetDataTable(strsql, null, false);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<T_RebatesStandard> GetAllList(int beg, int end)
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
