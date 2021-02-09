using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Models;

namespace Dal.Zichan
{
    public class ZiChan_WeiXiuRiZhiDal
    {
        string sql = "select BeiZhu,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,ZiChanCode,Note10,WeiXiuRenCode,WeiXiuBuMenCode,WeiXiuJinE,XiTongShiJian,ShiFouShenPi,ShenPiDanCode,WeiXiuTypeCode,Row_Number()over(order by listid) as crow from ZiChan_WeiXiuRiZhi";
        string sqlCont = "select count(*) from ZiChan_WeiXiuRiZhi";

        public bool Exists(int listid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" listid = @listid  ");
            SqlParameter[] parameters = {
					new SqlParameter("@listid", SqlDbType.Int,4)
			};
            parameters[0].Value = listid;

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

        public int Add(ZiChan_WeiXiuRiZhi model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.listid, tran);
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
        public bool Addlistmodel(IList<ZiChan_WeiXiuRiZhi> modellist)
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
        public int Delete(int listid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int intRow = Delete(listid, tran);
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
        public int Add(ZiChan_WeiXiuRiZhi model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ZiChan_WeiXiuRiZhi(");
            strSql.Append("BeiZhu,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,Note9,ZiChanCode,Note10,WeiXiuRenCode,WeiXiuBuMenCode,WeiXiuJinE,XiTongShiJian,ShiFouShenPi,ShenPiDanCode,WeiXiuTypeCode");
            strSql.Append(") values (");
            strSql.Append("@BeiZhu,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@Note9,@ZiChanCode,@Note10,@WeiXiuRenCode,@WeiXiuBuMenCode,@WeiXiuJinE,@XiTongShiJian,@ShiFouShenPi,@ShenPiDanCode,@WeiXiuTypeCode");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@BeiZhu", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ZiChanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuRenCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuBuMenCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuJinE", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@XiTongShiJian", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ShiFouShenPi", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@ShenPiDanCode", SqlDbType.NVarChar,50) ,            
                        new SqlParameter("@WeiXiuTypeCode", SqlDbType.VarChar,10)             
              
            };

            parameters[0].Value = SqlNull(model.BeiZhu);

            parameters[1].Value = SqlNull(model.Note1);

            parameters[2].Value = SqlNull(model.Note2);

            parameters[3].Value = SqlNull(model.Note3);

            parameters[4].Value = SqlNull(model.Note4);

            parameters[5].Value = SqlNull(model.Note5);

            parameters[6].Value = SqlNull(model.Note6);

            parameters[7].Value = SqlNull(model.Note7);

            parameters[8].Value = SqlNull(model.Note8);

            parameters[9].Value = SqlNull(model.Note9);

            parameters[10].Value = SqlNull(model.ZiChanCode);

            parameters[11].Value = SqlNull(model.Note10);

            parameters[12].Value = SqlNull(model.WeiXiuRenCode);

            parameters[13].Value = SqlNull(model.WeiXiuBuMenCode);

            parameters[14].Value = SqlNull(model.WeiXiuJinE);

            parameters[15].Value = SqlNull(model.XiTongShiJian);

            parameters[16].Value = SqlNull(model.ShiFouShenPi);

            parameters[17].Value = SqlNull(model.ShenPiDanCode);

            parameters[18].Value = SqlNull(model.WeiXiuTypeCode);


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(int listid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ZiChan_WeiXiuRiZhi ");
            strSql.Append(" where listid=@listid");
            SqlParameter[] parameters = {
					new SqlParameter("@listid", SqlDbType.Int,4)
			};
            parameters[0].Value = listid;


            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<ZiChan_WeiXiuRiZhi> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<ZiChan_WeiXiuRiZhi> list = new List<ZiChan_WeiXiuRiZhi>();
            foreach (DataRow dr in dt.Rows)
            {
                ZiChan_WeiXiuRiZhi model = new ZiChan_WeiXiuRiZhi();
                if (!DBNull.Value.Equals(dr["listid"]))
                {
                    model.listid = int.Parse(dr["listid"].ToString());
                }
                model.BeiZhu = dr["BeiZhu"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                model.Note9 = dr["Note9"].ToString();
                model.ZiChanCode = dr["ZiChanCode"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.WeiXiuRenCode = dr["WeiXiuRenCode"].ToString();
                model.WeiXiuBuMenCode = dr["WeiXiuBuMenCode"].ToString();
                if (!DBNull.Value.Equals(dr["WeiXiuJinE"]))
                {
                    model.WeiXiuJinE = decimal.Parse(dr["WeiXiuJinE"].ToString());
                }
                model.XiTongShiJian = dr["XiTongShiJian"].ToString();
                model.ShiFouShenPi = dr["ShiFouShenPi"].ToString();
                model.ShenPiDanCode = dr["ShenPiDanCode"].ToString();
                model.WeiXiuTypeCode = dr["WeiXiuTypeCode"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public ZiChan_WeiXiuRiZhi GetModel(int listid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where listid=@listid");
            SqlParameter[] parameters = {
					new SqlParameter("@listid", SqlDbType.Int,4)
			};
            parameters[0].Value = listid;


            ZiChan_WeiXiuRiZhi model = new ZiChan_WeiXiuRiZhi();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {

                   // model.listid = int.Parse(dr["listid"].ToString());
                    model.BeiZhu = dr["BeiZhu"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    model.Note9 = dr["Note9"].ToString();
                    model.ZiChanCode = dr["ZiChanCode"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.WeiXiuRenCode = dr["WeiXiuRenCode"].ToString();
                    model.WeiXiuBuMenCode = dr["WeiXiuBuMenCode"].ToString();
                    if (!DBNull.Value.Equals(dr["WeiXiuJinE"]))
                    {
                        model.WeiXiuJinE = decimal.Parse(dr["WeiXiuJinE"].ToString());
                    }
                    model.XiTongShiJian = dr["XiTongShiJian"].ToString();
                    model.ShiFouShenPi = dr["ShiFouShenPi"].ToString();
                    model.ShenPiDanCode = dr["ShenPiDanCode"].ToString();
                    model.WeiXiuTypeCode = dr["WeiXiuTypeCode"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public DataTable GetAllListBySql1(ZiChan_WeiXiuRiZhi model)
        {
            StringBuilder strSql = new StringBuilder();
            List<object> objParams = new List<object>();
            strSql.Append(@"select a.*,(case when ShiFouShenPi='0' then '否' when ShiFouShenPi='1' then '是' end ) as tjsp,
(select '['+userCode+']'+userName  from bill_users where userCode=a.WeiXiuRenCode)as wxname,
(select '['+deptCode+']'+deptName from bill_departments where deptCode=a.WeiXiuBuMenCode) as wxbmname,
(select '['+ZiChanCode+']'+ZiChanName  from ZiChan_Jilu where ZiChanCode=a.ZiChanCode)as zcnames,
(select '['+dicCode+']'+dicName  from bill_dataDic where dicType='09' and dicCode= a.WeiXiuTypeCode)as dictypecode,
convert(varchar(10),XiTongShiJian,121) as tbilldate
 from ZiChan_WeiXiuRiZhi a where 1=1");

            if (model.ShiFouShenPi != null && model.ShiFouShenPi != "")
            {
                strSql.Append(" and a.ShiFouShenPi='" + model.ShiFouShenPi + "'");
            }
            if (model.WeiXiuBuMenCode != null && model.WeiXiuBuMenCode != "")
            {
                strSql.Append(" and a.WeiXiuBuMenCode like'%" + model.WeiXiuBuMenCode + "%'");
            }
            if (model.ZiChanCode != null && model.ZiChanCode != "")
            {
                strSql.Append(" and a.ZiChanCode like'%" + model.ZiChanCode + "%'");
            }
            strSql.Append(" order by listid desc");
            return DataHelper.GetDataTable(strSql.ToString(), null, false);

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
        public IList<ZiChan_WeiXiuRiZhi> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<ZiChan_WeiXiuRiZhi> GetAllList(int beg, int end)
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
