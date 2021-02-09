using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using Models;
using Dal.Bills;

namespace Dal.FeeApplication
{
    public class bill_travelclaimsDal
    {
        string sql = "select billCode,ccfbz,hwfje,hwfbz,suje,sufbz,begintime,endtime,beginaddress,endaddress,tianshu,bxr,je,syjsm,ccsm,bxzy,bxsm,sfdk,ytje,ybje,sfgf,bxmxlx,bxzb,gfr,gfsj,cxsj,cxr,cxyy,se,pzcode,pzdate,guazhang,zhangtao,bxdept,bxrzh,bxrphone,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,bxdjs,Note9,Note10,Note11,Note12,Note13,Note14,Note15,sqrq,hsbzfje,hsbzfbz,ccfje,Row_Number()over(order by billCode) as crow from bill_travelclaims";
        string sqlCont = "select count(*) from bill_travelclaims";

        public bool Exists(string billCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(sqlCont);
            strSql.Append(" where ");
            strSql.Append(" billCode = @billCode  ");
          
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)	};
            parameters[0].Value = billCode;
          

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

        public void Add(bill__travelclaims model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.billCode, tran);
                    Add(model, tran);
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
        /// 删除一条数据
        /// </summary>
        public void Delete(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(billCode, tran);
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
        public void Add(bill__travelclaims model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_travelclaims(");
            strSql.Append("billCode,ccfbz,hwfje,hwfbz,suje,sufbz,begintime,endtime,beginaddress,endaddress,tianshu,bxr,je,syjsm,ccsm,bxzy,bxsm,sfdk,ytje,ybje,sfgf,bxmxlx,bxzb,gfr,gfsj,cxsj,cxr,cxyy,se,pzcode,pzdate,guazhang,zhangtao,bxdept,bxrzh,bxrphone,Note1,Note2,Note3,Note4,Note5,Note6,Note7,Note8,bxdjs,Note9,Note10,Note11,Note12,Note13,Note14,Note15,sqrq,hsbzfje,hsbzfbz,ccfje");
            strSql.Append(") values (");
            strSql.Append("@billCode,@ccfbz,@hwfje,@hwfbz,@suje,@sufbz,@begintime,@endtime,@beginaddress,@endaddress,@tianshu,@bxr,@je,@syjsm,@ccsm,@bxzy,@bxsm,@sfdk,@ytje,@ybje,@sfgf,@bxmxlx,@bxzb,@gfr,@gfsj,@cxsj,@cxr,@cxyy,@se,@pzcode,@pzdate,@guazhang,@zhangtao,@bxdept,@bxrzh,@bxrphone,@Note1,@Note2,@Note3,@Note4,@Note5,@Note6,@Note7,@Note8,@bxdjs,@Note9,@Note10,@Note11,@Note12,@Note13,@Note14,@Note15,@sqrq,@hsbzfje,@hsbzfbz,@ccfje");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@billCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ccfbz", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@hwfje", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@hwfbz", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@suje", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@sufbz", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@begintime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@endtime", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@beginaddress", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@endaddress", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@tianshu", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bxr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@je", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@syjsm", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@ccsm", SqlDbType.VarChar,300) ,            
                        new SqlParameter("@bxzy", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@bxsm", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@sfdk", SqlDbType.VarChar,1) ,            
                        new SqlParameter("@ytje", SqlDbType.Float,8) ,            
                        new SqlParameter("@ybje", SqlDbType.Float,8) ,            
                        new SqlParameter("@sfgf", SqlDbType.VarChar,1) ,            
                        new SqlParameter("@bxmxlx", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bxzb", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@gfr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@gfsj", SqlDbType.DateTime) ,            
                        new SqlParameter("@cxsj", SqlDbType.DateTime) ,            
                        new SqlParameter("@cxr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@cxyy", SqlDbType.VarChar,4000) ,            
                        new SqlParameter("@se", SqlDbType.Decimal,9) ,            
                        new SqlParameter("@pzcode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@pzdate", SqlDbType.DateTime) ,            
                        new SqlParameter("@guazhang", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@zhangtao", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bxdept", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@bxrzh", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@bxrphone", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@Note1", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note2", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note3", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note4", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note5", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note6", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note7", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note8", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@bxdjs", SqlDbType.Int,4) ,            
                        new SqlParameter("@Note9", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note10", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note11", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note12", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note13", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note14", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@Note15", SqlDbType.NVarChar,100) ,            
                        new SqlParameter("@sqrq", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@hsbzfje", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@hsbzfbz", SqlDbType.VarChar,200) ,            
                        new SqlParameter("@ccfje", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.billCode);

            parameters[1].Value = SqlNull(model.ccfbz);

            parameters[2].Value = SqlNull(model.hwfje);

            parameters[3].Value = SqlNull(model.hwfbz);

            parameters[4].Value = SqlNull(model.suje);

            parameters[5].Value = SqlNull(model.sufbz);

            parameters[6].Value = SqlNull(model.begintime);

            parameters[7].Value = SqlNull(model.endtime);

            parameters[8].Value = SqlNull(model.beginaddress);

            parameters[9].Value = SqlNull(model.endaddress);

            parameters[10].Value = SqlNull(model.tianshu);

            parameters[11].Value = SqlNull(model.bxr);

            parameters[12].Value = SqlNull(model.je);

            parameters[13].Value = SqlNull(model.syjsm);

            parameters[14].Value = SqlNull(model.ccsm);

            parameters[15].Value = SqlNull(model.bxzy);

            parameters[16].Value = SqlNull(model.bxsm);

            parameters[17].Value = SqlNull(model.sfdk);

            parameters[18].Value = SqlNull(model.ytje);

            parameters[19].Value = SqlNull(model.ybje);

            parameters[20].Value = SqlNull(model.sfgf);

            parameters[21].Value = SqlNull(model.bxmxlx);

            parameters[22].Value = SqlNull(model.bxzb);

            parameters[23].Value = SqlNull(model.gfr);

            parameters[24].Value = SqlNull(model.gfsj);

            parameters[25].Value = SqlNull(model.cxsj);

            parameters[26].Value = SqlNull(model.cxr);

            parameters[27].Value = SqlNull(model.cxyy);

            parameters[28].Value = SqlNull(model.se);

            parameters[29].Value = SqlNull(model.pzcode);

            parameters[30].Value = SqlNull(model.pzdate);

            parameters[31].Value = SqlNull(model.guazhang);

            parameters[32].Value = SqlNull(model.zhangtao);

            parameters[33].Value = SqlNull(model.bxdept);

            parameters[34].Value = SqlNull(model.bxrzh);

            parameters[35].Value = SqlNull(model.bxrphone);

            parameters[36].Value = SqlNull(model.Note1);

            parameters[37].Value = SqlNull(model.Note2);

            parameters[38].Value = SqlNull(model.Note3);

            parameters[39].Value = SqlNull(model.Note4);

            parameters[40].Value = SqlNull(model.Note5);

            parameters[41].Value = SqlNull(model.Note6);

            parameters[42].Value = SqlNull(model.Note7);

            parameters[43].Value = SqlNull(model.Note8);

            parameters[44].Value = SqlNull(model.bxdjs);

            parameters[45].Value = SqlNull(model.Note9);

            parameters[46].Value = SqlNull(model.Note10);

            parameters[47].Value = SqlNull(model.Note11);

            parameters[48].Value = SqlNull(model.Note12);

            parameters[49].Value = SqlNull(model.Note13);

            parameters[50].Value = SqlNull(model.Note14);

            parameters[51].Value = SqlNull(model.Note15);

            parameters[52].Value = SqlNull(model.sqrq);

            parameters[53].Value = SqlNull(model.hsbzfje);

            parameters[54].Value = SqlNull(model.hsbzfbz);

            parameters[55].Value = SqlNull(model.ccfje);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string billCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_travelclaims ");
            strSql.Append(" where billCode=@billCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)	};
            parameters[0].Value = billCode;
           


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        public IList<bill__travelclaims> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<bill__travelclaims> list = new List<bill__travelclaims>();
            foreach (DataRow dr in dt.Rows)
            {
                bill__travelclaims model = new bill__travelclaims();
                model.billCode = dr["billCode"].ToString();
                model.ccfbz = dr["ccfbz"].ToString();
                model.hwfje = dr["hwfje"].ToString();
                model.hwfbz = dr["hwfbz"].ToString();
                model.suje = dr["suje"].ToString();
                model.sufbz = dr["sufbz"].ToString();
                model.begintime = dr["begintime"].ToString();
                model.endtime = dr["endtime"].ToString();
                model.beginaddress = dr["beginaddress"].ToString();
                model.endaddress = dr["endaddress"].ToString();
                model.tianshu = dr["tianshu"].ToString();
                model.bxr = dr["bxr"].ToString();
                model.je = dr["je"].ToString();
                model.syjsm = dr["syjsm"].ToString();
                model.ccsm = dr["ccsm"].ToString();
                model.bxzy = dr["bxzy"].ToString();
                model.bxsm = dr["bxsm"].ToString();
                model.sfdk = dr["sfdk"].ToString();
                if (!DBNull.Value.Equals(dr["ytje"]))
                {
                    model.ytje = decimal.Parse(dr["ytje"].ToString());
                }
                if (!DBNull.Value.Equals(dr["ybje"]))
                {
                    model.ybje = decimal.Parse(dr["ybje"].ToString());
                }
                model.sfgf = dr["sfgf"].ToString();
                model.bxmxlx = dr["bxmxlx"].ToString();
                model.bxzb = dr["bxzb"].ToString();
                model.gfr = dr["gfr"].ToString();
                if (!DBNull.Value.Equals(dr["gfsj"]))
                {
                    model.gfsj = DateTime.Parse(dr["gfsj"].ToString());
                }
                if (!DBNull.Value.Equals(dr["cxsj"]))
                {
                    model.cxsj = DateTime.Parse(dr["cxsj"].ToString());
                }
                model.cxr = dr["cxr"].ToString();
                model.cxyy = dr["cxyy"].ToString();
                if (!DBNull.Value.Equals(dr["se"]))
                {
                    model.se = decimal.Parse(dr["se"].ToString());
                }
                model.pzcode = dr["pzcode"].ToString();
                if (!DBNull.Value.Equals(dr["pzdate"]))
                {
                    model.pzdate = dr["pzdate"].ToString();
                }
                model.guazhang = dr["guazhang"].ToString();
                model.zhangtao = dr["zhangtao"].ToString();
                model.bxdept = dr["bxdept"].ToString();
                model.bxrzh = dr["bxrzh"].ToString();
                model.bxrphone = dr["bxrphone"].ToString();
                model.Note1 = dr["Note1"].ToString();
                model.Note2 = dr["Note2"].ToString();
                model.Note3 = dr["Note3"].ToString();
                model.Note4 = dr["Note4"].ToString();
                model.Note5 = dr["Note5"].ToString();
                model.Note6 = dr["Note6"].ToString();
                model.Note7 = dr["Note7"].ToString();
                model.Note8 = dr["Note8"].ToString();
                if (!DBNull.Value.Equals(dr["bxdjs"]))
                {
                    model.bxdjs = int.Parse(dr["bxdjs"].ToString());
                }
                model.Note9 = dr["Note9"].ToString();
                model.Note10 = dr["Note10"].ToString();
                model.Note11 = dr["Note11"].ToString();
                model.Note12 = dr["Note12"].ToString();
                model.Note13 = dr["Note13"].ToString();
                model.Note14 = dr["Note14"].ToString();
                model.Note15 = dr["Note15"].ToString();
                model.sqrq = dr["sqrq"].ToString();
                model.hsbzfje = dr["hsbzfje"].ToString();
                model.hsbzfbz = dr["hsbzfbz"].ToString();
                model.ccfje = dr["ccfje"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public bill__travelclaims GetModel(string billCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where billCode=@billCode  ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)	};
            parameters[0].Value = billCode;


            bill__travelclaims model = new bill__travelclaims();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.billCode = dr["billCode"].ToString();
                    model.ccfbz = dr["ccfbz"].ToString();
                    model.hwfje = dr["hwfje"].ToString();
                    model.hwfbz = dr["hwfbz"].ToString();
                    model.suje = dr["suje"].ToString();
                    model.sufbz = dr["sufbz"].ToString();
                    model.begintime = dr["begintime"].ToString();
                    model.endtime = dr["endtime"].ToString();
                    model.beginaddress = dr["beginaddress"].ToString();
                    model.endaddress = dr["endaddress"].ToString();
                    model.tianshu = dr["tianshu"].ToString();
                    model.bxr = dr["bxr"].ToString();
                    model.je = dr["je"].ToString();
                    model.syjsm = dr["syjsm"].ToString();
                    model.ccsm = dr["ccsm"].ToString();
                    model.bxzy = dr["bxzy"].ToString();
                    model.bxsm = dr["bxsm"].ToString();
                    model.sfdk = dr["sfdk"].ToString();
                    if (!DBNull.Value.Equals(dr["ytje"]))
                    {
                        model.ytje = decimal.Parse(dr["ytje"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["ybje"]))
                    {
                        model.ybje = decimal.Parse(dr["ybje"].ToString());
                    }
                    model.sfgf = dr["sfgf"].ToString();
                    model.bxmxlx = dr["bxmxlx"].ToString();
                    model.bxzb = dr["bxzb"].ToString();
                    model.gfr = dr["gfr"].ToString();
                    if (!DBNull.Value.Equals(dr["gfsj"]))
                    {
                        model.gfsj = DateTime.Parse(dr["gfsj"].ToString());
                    }
                    if (!DBNull.Value.Equals(dr["cxsj"]))
                    {
                        model.cxsj = DateTime.Parse(dr["cxsj"].ToString());
                    }
                    model.cxr = dr["cxr"].ToString();
                    model.cxyy = dr["cxyy"].ToString();
                    if (!DBNull.Value.Equals(dr["se"]))
                    {
                        model.se = decimal.Parse(dr["se"].ToString());
                    }
                    model.pzcode = dr["pzcode"].ToString();
                    if (!DBNull.Value.Equals(dr["pzdate"]))
                    {
                        model.pzdate = dr["pzdate"].ToString();
                    }
                    model.guazhang = dr["guazhang"].ToString();
                    model.zhangtao = dr["zhangtao"].ToString();
                    model.bxdept = dr["bxdept"].ToString();
                    model.bxrzh = dr["bxrzh"].ToString();
                    model.bxrphone = dr["bxrphone"].ToString();
                    model.Note1 = dr["Note1"].ToString();
                    model.Note2 = dr["Note2"].ToString();
                    model.Note3 = dr["Note3"].ToString();
                    model.Note4 = dr["Note4"].ToString();
                    model.Note5 = dr["Note5"].ToString();
                    model.Note6 = dr["Note6"].ToString();
                    model.Note7 = dr["Note7"].ToString();
                    model.Note8 = dr["Note8"].ToString();
                    if (!DBNull.Value.Equals(dr["bxdjs"]))
                    {
                        model.bxdjs = int.Parse(dr["bxdjs"].ToString());
                    }
                    model.Note9 = dr["Note9"].ToString();
                    model.Note10 = dr["Note10"].ToString();
                    model.Note11 = dr["Note11"].ToString();
                    model.Note12 = dr["Note12"].ToString();
                    model.Note13 = dr["Note13"].ToString();
                    model.Note14 = dr["Note14"].ToString();
                    model.Note15 = dr["Note15"].ToString();
                    model.sqrq = dr["sqrq"].ToString();
                    model.hsbzfje = dr["hsbzfje"].ToString();
                    model.hsbzfbz = dr["hsbzfbz"].ToString();
                    model.ccfje = dr["ccfje"].ToString();

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
        public IList<bill__travelclaims> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<bill__travelclaims> GetAllList(int beg, int end)
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
        public IList<bill__travelclaims> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls)
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
        /// 根据查询条件分页行数
        /// </summary>
        public int GetAllListCount(int beg, int end, List<SqlParameter> paramter, string sqls)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from(");
            strSql.Append(sqlCont);
            strSql.Append(" where 1=1 ");
            strSql.Append(sqls);
            strSql.Append(")t where t.crow>");
            strSql.Append(beg.ToString());
            strSql.Append(" and t.crow<=");
            strSql.Append(end.ToString());
            if (paramter == null)
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), null, false));
            }
            else
            {
                return Convert.ToInt32(DataHelper.ExecuteScalar(strSql.ToString(), paramter.ToArray(), false));
            }

        }



        /// <summary>
        /// 根据查询条件
        /// </summary>
        public IList<bill__travelclaims> GetAllList(List<SqlParameter> paramter, string sqls)
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

        public IList<Bill_Ybbxmxb_Fykm> GetFykm(string billCode)
        {
            string sql = "select * from Bill_Ybbxmxb_Fykm where  billcode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Fykm> list = new List<Bill_Ybbxmxb_Fykm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Fykm fykm = new Bill_Ybbxmxb_Fykm();
                fykm.BillCode = Convert.ToString(dr["BillCode"]);
                fykm.Fykm = Convert.ToString(dr["Fykm"]);
                fykm.Je = Convert.ToDecimal(dr["Je"]);
                fykm.MxGuid = Convert.ToString(dr["MxGuid"]);
                fykm.Se = Convert.ToDecimal(SetDBNull(dr["Se"]));
                fykm.Status = Convert.ToString(dr["Status"]);
                fykm.DeptList = GetDept(fykm.MxGuid);
                fykm.XmList = GetXm(fykm.MxGuid);
                list.Add(fykm);
            }
            return list;
        }

        /// <summary>
        /// 删除差旅费报销单
        /// </summary>
        /// <param name="billCode"></param>
        /// <param name="tran"></param>
        public void Deleteclbx(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_ybbxmxb where billCode=@billCode";
            string delKm = "delete bill_ybbxmxb_fykm where billCode=@billCode";
            string delFysq = "delete bill_ybbx_fysq where billCode=@billCode";

            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            IList<Bill_Ybbxmxb_Fykm> fykmList = GetFykm(billCode);
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
            DataHelper.ExcuteNonQuery(delKm, tran, billsps, false);
            DataHelper.ExcuteNonQuery(delFysq, tran, billsps, false);
            foreach (Bill_Ybbxmxb_Fykm km in fykmList)
            {
                string mxCode = km.MxGuid;
                string delDep = "delete bill_ybbxmxb_fykm_dept where kmmxGuid=@mxCode";
                string delXm = "delete bill_ybbxmxb_hsxm where kmmxGuid=@mxCode";
                SqlParameter[] mxsps = { new SqlParameter("@mxCode", mxCode) };
                DataHelper.ExcuteNonQuery(delXm, tran, mxsps, false);
                DataHelper.ExcuteNonQuery(delDep, tran, mxsps, false);
            }
        }
        /// <summary>
        /// 新增项目
        /// </summary>
        /// <param name="xm"></param>
        /// <param name="tran"></param>
        public void InsertYbbxXm(Bill_Ybbxmxb_Hsxm xm, SqlTransaction tran)
        {
            string sql = @"insert into bill_ybbxmxb_hsxm(  kmmxGuid, mxGuid, xmCode, je)values
                           (  @kmmxGuid, @mxGuid, @xmCode, @je)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@kmmxGuid",xm.KmmxGuid),
                                     new SqlParameter("@mxGuid",xm.MxGuid),
                                     new SqlParameter("@xmCode",xm.XmCode),
                                     new SqlParameter("@je",xm.Je)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

   
        public IList<Bill_Ybbxmxb_Fykm_Dept> GetDept(string kmmxGuid)
        {
            string sql = "select * from bill_ybbxmxb_fykm_dept where  kmmxGuid=@kmmxGuid";
            SqlParameter[] sps = { new SqlParameter("@kmmxGuid", kmmxGuid) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Fykm_Dept> list = new List<Bill_Ybbxmxb_Fykm_Dept>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Fykm_Dept dept = new Bill_Ybbxmxb_Fykm_Dept();
                dept.DeptCode = Convert.ToString(dr["DeptCode"]);
                dept.Je = Convert.ToDecimal(dr["Je"]);
                dept.KmmxGuid = Convert.ToString(dr["KmmxGuid"]);
                dept.MxGuid = Convert.ToString(dr["MxGuid"]);
                dept.Status = Convert.ToString(dr["Status"]);
                list.Add(dept);
            }
            return list;
        }

        public IList<Bill_Ybbxmxb_Hsxm> GetXm(string kmmxGuid)
        {
            string sql = "select * from Bill_Ybbxmxb_Hsxm where  kmmxGuid=@kmmxGuid";
            SqlParameter[] sps = { new SqlParameter("@kmmxGuid", kmmxGuid) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Hsxm> list = new List<Bill_Ybbxmxb_Hsxm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Hsxm xm = new Bill_Ybbxmxb_Hsxm();
                xm.XmCode = Convert.ToString(dr["XmCode"]);
                xm.Je = Convert.ToDecimal(dr["Je"]);
                xm.KmmxGuid = Convert.ToString(dr["KmmxGuid"]);
                xm.MxGuid = Convert.ToString(dr["MxGuid"]);
                list.Add(xm);
            }
            return list;
        }

        private object SqlNull2(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }

        private object SetDBNull(object obj)
        {
            if (obj.GetType().Name == "DBNull")
            {
                return null;
            }
            else
            {
                return obj;
            }
        }
    }
}
