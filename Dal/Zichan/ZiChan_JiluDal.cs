using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Models;

namespace Dal.Zichan
{
    public class ZiChan_JiluDal
    {
        string sql = "select ZiChanCode,WeiZhi,CaiGouBuMenCode,QiYongDate,LuRuDate,LuRuRenCode,BeiZhu,Note1,Note2,Note3,Note4,ZiChanName,Note5,Note6,Note7,Note8,Note9,Note10,Note11,Note12,Note13,Note14,LeiBieCode,Note15,Note16,Note17,Note18,Note19,Note20,ZengJianFangShiCode,ShiYongZhuangKuangCode,GuiGeXingHao,ShiYongQiXian,YuanZhi,ShiYongBuMenCode,Row_Number()over(order by ZiChanCode) as crow from ZiChan_Jilu";
        string sqlCont = "select count(*) from ZiChan_Jilu";

        public bool Exists(string ZiChanCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" ZiChanCode = @ZiChanCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZiChanCode;

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
        public bool Addlistmodel(IList<ZiChan_Jilu> modellist)
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
        public DataTable GetAllListBySql1(ZiChan_Jilu model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select a.*,
(select '['+LeibieCode+']'+LeibieName from  ZiChan_Leibie where LeibieCode=a.LeibieCode )as leibiename,
(select '['+FangshiCode+']'+Fangshiname from ZiChan_ZengJianFangShi where FangshiCode=a.ZengJianFangShiCode )as zjfsname,
(select '['+ZhuangKuangCode+']'+ZhuangKuangName from ZiChan_ShiYongZhuangKuang where ZhuangKuangCode=a.ShiYongZhuangKuangCode)as syzkname,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=a.ShiYongBuMenCode)as sybmname,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=a.CaiGouBuMenCode)as cgbmname,
(select '['+userCode+']'+userName from bill_users where userCode=a.LuRuRenCode)as lururenName
 from ZiChan_Jilu a where 1=1");

            if (model.ZiChanCode != null && model.ZiChanCode != "")
            {
                strSql.Append(" and a.ZiChanCode like'%" + model.ZiChanCode + "%'");
            }
            if (model.ZiChanName != null && model.ZiChanName != "")
            {
                strSql.Append(" and a.ZiChanName like'%" + model.ZiChanName + "%'");
            }
            if (model.LeiBieCode != null && model.LeiBieCode != "")
            {
                strSql.Append(" and a.LeiBieCode like'%" + model.LeiBieCode + "%'");
            }



            if (model.ZengJianFangShiCode != null && model.ZengJianFangShiCode != "")
            {
                strSql.Append(" and a.ZengJianFangShiCode like'%" + model.ZengJianFangShiCode + "%'");
            }
            if (model.ShiYongZhuangKuangCode != null && model.ShiYongZhuangKuangCode != "")
            {
                strSql.Append(" and a.ShiYongZhuangKuangCode like'%" + model.ShiYongZhuangKuangCode + "%'");
            }
            if (model.GuiGeXingHao != null && model.GuiGeXingHao != "")
            {
                strSql.Append(" and a.GuiGeXingHao like'%" + model.GuiGeXingHao + "%'");
            }

            if (model.ShiYongBuMenCode != null && model.ShiYongBuMenCode != "")
            {
                strSql.Append(" and a.ShiYongBuMenCode like'%" + model.ShiYongBuMenCode + "%'");
            }
            if (model.CaiGouBuMenCode != null && model.CaiGouBuMenCode != "")
            {
                strSql.Append(" and a.CaiGouBuMenCode like'%" + model.CaiGouBuMenCode + "%'");
            }


            return DataHelper.GetDataTable(strSql.ToString(), null, false);

        }
        public int Add(ZiChan_Jilu model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.ZiChanCode, tran);
                    int intRow = Add(model, tran);
                    tran.Commit();
                    return intRow;
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
        public int Delete(string ZiChanCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int intRow = Delete(ZiChanCode, tran);
                    tran.Commit();
                    return intRow;
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
        public int Add(ZiChan_Jilu model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_Jilu(");
            strSql.Append("ZiChanCode,WeiZhi,CaiGouBuMenCode,QiYongDate,LuRuDate,LuRuRenCode,BeiZhu,Note1,Note2,Note3,Note4,ZiChanName,Note5,Note6,Note7,Note8,Note9,Note10,Note11,Note12,Note13,Note14,LeiBieCode,Note15,Note16,Note17,Note18,Note19,Note20,ZengJianFangShiCode,ShiYongZhuangKuangCode,GuiGeXingHao,ShiYongQiXian,YuanZhi,ShiYongBuMenCode");
            strSql.Append(") values (");
            strSql.Append("@ZiChanCode,@WeiZhi,@CaiGouBuMenCode,@QiYongDate,@LuRuDate,@LuRuRenCode,@BeiZhu,@Note1,@Note2,@Note3,@Note4,@ZiChanName,@Note5,@Note6,@Note7,@Note8,@Note9,@Note10,@Note11,@Note12,@Note13,@Note14,@LeiBieCode,@Note15,@Note16,@Note17,@Note18,@Note19,@Note20,@ZengJianFangShiCode,@ShiYongZhuangKuangCode,@GuiGeXingHao,@ShiYongQiXian,@YuanZhi,@ShiYongBuMenCode");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiZhi", SqlDbType.NVarChar,200) ,            
                        new SqlParameter("@CaiGouBuMenCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@QiYongDate", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@LuRuDate", SqlDbType.VarChar,30) ,            
                        new SqlParameter("@LuRuRenCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@BeiZhu", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZiChanName", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note11", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note12", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note13", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note14", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@LeiBieCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note15", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note16", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note17", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note18", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note19", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note20", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZengJianFangShiCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShiYongZhuangKuangCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@GuiGeXingHao", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShiYongQiXian", SqlDbType.Int,4) ,            
                        new SqlParameter("@YuanZhi", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@ShiYongBuMenCode", SqlDbType.NVarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.ZiChanCode);

            parameters[1].Value = SqlNull(model.WeiZhi);

            parameters[2].Value = SqlNull(model.CaiGouBuMenCode);

            parameters[3].Value = SqlNull(model.QiYongDate);

            parameters[4].Value = SqlNull(model.LuRuDate);

            parameters[5].Value = SqlNull(model.LuRuRenCode);

            parameters[6].Value = SqlNull(model.BeiZhu);

            parameters[7].Value = SqlNull(model.Note1);

            parameters[8].Value = SqlNull(model.Note2);

            parameters[9].Value = SqlNull(model.Note3);

            parameters[10].Value = SqlNull(model.Note4);

            parameters[11].Value = SqlNull(model.ZiChanName);

            parameters[12].Value = SqlNull(model.Note5);

            parameters[13].Value = SqlNull(model.Note6);

            parameters[14].Value = SqlNull(model.Note7);

            parameters[15].Value = SqlNull(model.Note8);

            parameters[16].Value = SqlNull(model.Note9);

            parameters[17].Value = SqlNull(model.Note10);

            parameters[18].Value = SqlNull(model.Note11);

            parameters[19].Value = SqlNull(model.Note12);

            parameters[20].Value = SqlNull(model.Note13);

            parameters[21].Value = SqlNull(model.Note14);

            parameters[22].Value = SqlNull(model.LeiBieCode);

            parameters[23].Value = SqlNull(model.Note15);

            parameters[24].Value = SqlNull(model.Note16);

            parameters[25].Value = SqlNull(model.Note17);

            parameters[26].Value = SqlNull(model.Note18);

            parameters[27].Value = SqlNull(model.Note19);

            parameters[28].Value = SqlNull(model.Note20);

            parameters[29].Value = SqlNull(model.ZengJianFangShiCode);

            parameters[30].Value = SqlNull(model.ShiYongZhuangKuangCode);

            parameters[31].Value = SqlNull(model.GuiGeXingHao);

            parameters[32].Value = SqlNull(model.ShiYongQiXian);

            parameters[33].Value = SqlNull(model.YuanZhi);

            parameters[34].Value = SqlNull(model.ShiYongBuMenCode);


          return  DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string ZiChanCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_Jilu ");
            strSql.Append(" where ZiChanCode=@ZiChanCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZiChanCode;


           return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_Jilu> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_Jilu> list = new List<ZiChan_Jilu>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_Jilu model = new ZiChan_Jilu();
                model.ZiChanCode = dr["ZiChanCode"].ToString();
                model.WeiZhi = dr["WeiZhi"].ToString();
                model.CaiGouBuMenCode = dr["CaiGouBuMenCode"].ToString();
                model.QiYongDate = dr["QiYongDate"].ToString();
                model.LuRuDate = dr["LuRuDate"].ToString();
                model.LuRuRenCode = dr["LuRuRenCode"].ToString();
                model.BeiZhu = dr["BeiZhu"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.ZiChanName = dr["ZiChanName"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.Note11 = dr["Note11"].ToString();
                model.Note12 = dr["Note12"].ToString();
                model.Note13 = dr["Note13"].ToString();
                model.Note14 = dr["Note14"].ToString();
                model.LeiBieCode = dr["LeiBieCode"].ToString();
                model.Note15 = dr["Note15"].ToString();
                model.Note16 = dr["Note16"].ToString();
                model.Note17 = dr["Note17"].ToString();
                model.Note18 = dr["Note18"].ToString();
                model.Note19 = dr["Note19"].ToString();
                model.Note20 = dr["Note20"].ToString();
                model.ZengJianFangShiCode = dr["ZengJianFangShiCode"].ToString();
                model.ShiYongZhuangKuangCode = dr["ShiYongZhuangKuangCode"].ToString();
                model.GuiGeXingHao = dr["GuiGeXingHao"].ToString();
                if (!DBNull.Value.Equals(dr["ShiYongQiXian"]))
                {
                    model.ShiYongQiXian = int.Parse(dr["ShiYongQiXian"].ToString());
                }
                if (!DBNull.Value.Equals(dr["YuanZhi"]))
                {
                    model.YuanZhi = decimal.Parse(dr["YuanZhi"].ToString());
                }
                model.ShiYongBuMenCode = dr["ShiYongBuMenCode"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_Jilu GetModel(string ZiChanCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where ZiChanCode=@ZiChanCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50)			};
            parameters[0].Value = ZiChanCode;


            ZiChan_Jilu model = new ZiChan_Jilu();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.ZiChanCode = dr["ZiChanCode"].ToString();
                    model.WeiZhi = dr["WeiZhi"].ToString();
                    model.CaiGouBuMenCode = dr["CaiGouBuMenCode"].ToString();
                    model.QiYongDate = dr["QiYongDate"].ToString();
                    model.LuRuDate = dr["LuRuDate"].ToString();
                    model.LuRuRenCode = dr["LuRuRenCode"].ToString();
                    model.BeiZhu = dr["BeiZhu"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.ZiChanName = dr["ZiChanName"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.Note11 = dr["Note11"].ToString();
                    model.Note12 = dr["Note12"].ToString();
                    model.Note13 = dr["Note13"].ToString();
                    model.Note14 = dr["Note14"].ToString();
                    model.LeiBieCode = dr["LeiBieCode"].ToString();
                    model.Note15 = dr["Note15"].ToString();
                    model.Note16 = dr["Note16"].ToString();
                    model.Note17 = dr["Note17"].ToString();
                    model.Note18 = dr["Note18"].ToString();
                    model.Note19 = dr["Note19"].ToString();
                    model.Note20 = dr["Note20"].ToString();
                    model.ZengJianFangShiCode = dr["ZengJianFangShiCode"].ToString();
                    model.ShiYongZhuangKuangCode = dr["ShiYongZhuangKuangCode"].ToString();
                    model.GuiGeXingHao = dr["GuiGeXingHao"].ToString();
                    if (!DBNull.Value.Equals(dr["ShiYongQiXian"]))
                    {
                        model.ShiYongQiXian = int.Parse(dr["ShiYongQiXian"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["YuanZhi"]))
                    {
                        model.YuanZhi = decimal.Parse(dr["YuanZhi"].ToString());
                    }
                    model.ShiYongBuMenCode = dr["ShiYongBuMenCode"].ToString();

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
        public IList<ZiChan_Jilu> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_Jilu> GetAllList(int beg, int end)
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
