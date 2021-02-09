using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;
using System.Data;

namespace Dal.newysgl
{
    public class bill_ysmxbDal
    {
        string sql = "select gcbh,billCode,yskm,ysje,ysDept,ysType,Row_Number()over(order by gcbh) as crow from bill_ysmxb";
        string sqlCont = "select count(*) from bill_ysmxb";
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public void Add(Bill_Ysmxb model)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(model.BillCode, tran);
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
        public void Delete(string list_id)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(list_id, tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        public void DeleteByBillCode(string strBillcode)
        {
            string strsqls = "delete from bill_ysmxb where billcode=@billcode";
            DataHelper.ExcuteNonQuery(strsqls, new SqlParameter[] { new SqlParameter("@billcode", strBillcode) }, false);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(Bill_Ysmxb model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_ysmxb(");
            strSql.Append("gcbh,billCode,yskm,ysje,ysDept,ysType");
            strSql.Append(") values (");
            strSql.Append("@gcbh,@billCode,@yskm,@ysje,@ysDept,@ysType");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@gcbh", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@billCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@yskm", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ysje", SqlDbType.Float,8) ,            
                        new SqlParameter("@ysDept", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@ysType", SqlDbType.VarChar,1) ,            
              
            };

            parameters[0].Value = SqlNull(model.Gcbh);

            parameters[1].Value = SqlNull(model.BillCode);

            parameters[2].Value = SqlNull(model.Yskm);

            parameters[3].Value = SqlNull(model.Ysje);

            parameters[4].Value = SqlNull(model.YsDept);

            parameters[5].Value = SqlNull(model.YsType);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }


        /// <summary>
        /// 根据部门获取科目调整的列表值
        /// </summary>
        /// <param name="strdeptcode"></param>
        /// <returns></returns>
        public DataTable getAlltzlist(string strdeptcode)
        {
            string sqlkmlist = "select (select deptname from bill_departments where deptcode=billdept) as billDept,stepid,billCode,(select username from bill_users where usercode=billuser) as billUser,billdate  from bill_main where flowID='kmystz' and billDept='" + strdeptcode + "' order by billDate desc";
            DataTable dtlist = new DataTable();
            return DataHelper.GetDataTable(sqlkmlist, null, false);

        }
        /// <summary>
        /// 根据部门获取科目调整的列表值
        /// </summary>
        /// <param name="strdeptcode"></param>
        /// <returns></returns>
        public DataTable getAlltzlist(string strdeptcode, bool isLong)
        {
            string sqlkmlist = "select billJe,billName2,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid,billCode,(select username from bill_users where usercode=billuser) as billUser,billdate , ( select xmmc from bill_ysgc where gcbh in ( select gcbh from Bill_Ysmxb where billCode=bill_main.billCode and ysje>0)) as gcmc ,(select mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind   from bill_main where flowID='kmystz' and billDept='" + strdeptcode + "' order by billDate desc";
            DataTable dtlist = new DataTable();
            return DataHelper.GetDataTable(sqlkmlist, null, false);

        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(string list_id, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_ysmxb ");
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Decimal)
			};
            parameters[0].Value = list_id;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        /// 按条件获取一个实例
        /// </summary>
        /// <param name="tempsql"></param>
        /// <param name="sps"></param>
        /// <returns></returns>
        public IList<Bill_Ysmxb> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ysmxb model = new Bill_Ysmxb();
                model.Gcbh = dr["gcbh"].ToString();
                model.BillCode = dr["billCode"].ToString();
                model.Yskm = dr["yskm"].ToString();
                if (!DBNull.Value.Equals(dr["ysje"]))
                {
                    model.Ysje = decimal.Parse(dr["ysje"].ToString());
                }
                model.YsDept = dr["ysDept"].ToString();
                model.YsType = dr["ysType"].ToString();

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bill_Ysmxb GetModel(decimal list_id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where list_id=@list_id");
            SqlParameter[] parameters = {
					new SqlParameter("@list_id", SqlDbType.Decimal)
			};
            parameters[0].Value = list_id;


            Bill_Ysmxb model = new Bill_Ysmxb();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Gcbh = dr["gcbh"].ToString();
                    model.BillCode = dr["billCode"].ToString();
                    model.Yskm = dr["yskm"].ToString();
                    if (!DBNull.Value.Equals(dr["ysje"]))
                    {
                        model.Ysje = decimal.Parse(dr["ysje"].ToString());
                    }
                    model.YsDept = dr["ysDept"].ToString();
                    model.YsType = dr["ysType"].ToString();

                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Bill_Ysmxb GetModel(string billcode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(sql);
            strSql.Append(" where billCode=@billCode");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.Char,50)
			};
            parameters[0].Value = billcode;


            Bill_Ysmxb model = new Bill_Ysmxb();

            using (SqlDataReader dr = DataHelper.GetDataReader(strSql.ToString(), parameters))
            {
                if (dr.Read())
                {
                    model.Gcbh = dr["gcbh"].ToString();
                    model.BillCode = dr["billCode"].ToString();
                    model.Yskm = dr["yskm"].ToString();
                    if (!DBNull.Value.Equals(dr["ysje"]))
                    {
                        model.Ysje = decimal.Parse(dr["ysje"].ToString());
                    }
                    model.YsDept = dr["ysDept"].ToString();
                    model.YsType = dr["ysType"].ToString();

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
        public IList<Bill_Ysmxb> GetAllList()
        {
            return ListMaker(sql, null);
        }

        /// <summary>
        /// 获得所有数据
        /// </summary>
        public IList<Bill_Ysmxb> GetAllList(int beg, int end)
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

        public IList<Bill_Ysmxb> GetYsmxByNian(string nd, string deptcode, string xmbh, string stepid)
        {
            string mysql = "select gcbh,main.billCode,yskm,ysje,ysDept,ysType,Row_Number()over(order by gcbh) as crow from bill_ysmxb mxb ,bill_main main where mxb.billcode=main.billcode and 1=1 ";
            List<SqlParameter> lstsp = new List<SqlParameter>();
            if (deptcode != "")
            {
                mysql += " and mxb.ysdept=@ysdept ";
                lstsp.Add(new SqlParameter("@ysdept", deptcode));
            }
            if (nd != "")
            {
                mysql += " and  left(mxb.gcbh,4) = @gcbh ";
                lstsp.Add(new SqlParameter("@gcbh", nd));
            }
            if (!xmbh.Equals("all"))//预算调整的时候传入all  传入all 的时候代表忽略这个项目的条件
            {
                if (xmbh != "")
                {
                    mysql += " and main.note3=@xmbh and main.flowid='xmys' and mxb.ystype='8'";
                    //mysql += " and mxb.billcode in (select billcode from bill_main where note3=@xmbh and flowid='xmys') and ystype='8' ";
                    lstsp.Add(new SqlParameter("@xmbh", xmbh));
                }
                else
                {
                    mysql += " and  isnull(main.note3,'')=''";
                    //mysql += " and mxb.billcode in (select billcode from bill_main where isnull(note3,'')='')  ";
                }
            }
            if (!string.IsNullOrEmpty(stepid))
            {
                mysql += " and main.stepid='" + stepid + "'";
            }

            return ListMaker(mysql, lstsp.ToArray());
            //if (deptcode == "")
            //{
            //    string cxsql = sql + " where  ";
            //    SqlParameter[] paramter = { new SqlParameter("@gcbh",nd),
            //                          };
            //    return ListMaker(cxsql, paramter);
            //}
            //else
            //{
            //    string cxsql = sql + " where  left(gcbh,4) = @gcbh and ysdept=@ysdept ";
            //    SqlParameter[] paramter = { new SqlParameter("@gcbh",nd),
            //                          };
            //    return ListMaker(cxsql, paramter);
            //}
        }
     
        public IList<Bill_Ysmxb> InsetNdysmx(string nd, string deptcode, string gcbh, string billcode)
        {
            bool boIsGkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
            string cxsql = "";
            if (boIsGkfj)//and kmcode not in (select distinct yskmcode from bill_yskm_gkdept)
            {
                cxsql = @"insert into bill_ysmxb ( gcbh , billcode , yskm , ysje , ysdept , ystype ) 
                                   select '" + gcbh + "' , '" + billcode + "', kmcode as yskmcode , je , '" + deptcode + @"' , '1' 
					               from   (select (je-isnull(ysje,0)) as ncys,* from (select * from bill_ys_xmfjbm where procode=@nd and by3 = '2'
                        and deptcode=@deptcode 
                       
                        ) xmfjbm left join (select * from bill_ysmxb where ystype='5' and gcbh=@gcbh ) mxb on mxb.yskm=xmfjbm.kmcode and left(mxb.gcbh,4)=xmfjbm.procode and mxb.ysdept=xmfjbm.deptcode) a  ";
            }
            else
            {
                cxsql = @"insert into bill_ysmxb ( gcbh , billcode , yskm , ysje , ysdept , ystype ) 
                                   select '" + gcbh + "' , '" + billcode + "', kmcode as yskmcode ,a.ncys as je , '" + deptcode + @"' , '1' 
					               from 
                                        (select (je-isnull(ysje,0)) as ncys,* from (select * from bill_ys_xmfjbm where procode=@nd and by3 = '2' 
                                and deptcode=@deptcode
                            ) xmfjbm left join (select SUM(ysje) as ysje,gcbh,ysDept,yskm from bill_ysmxb where ystype='5' and gcbh=@gcbh  group by gcbh,ysDept,yskm) mxb on mxb.yskm=xmfjbm.kmcode and left(mxb.gcbh,4)=xmfjbm.procode and mxb.ysdept=xmfjbm.deptcode) a
					               ";
            }

            SqlParameter[] paramter = { new SqlParameter("@deptcode", deptcode), new SqlParameter("@gcbh", nd + "0001"), new SqlParameter("@nd", nd) };
            string cxjesql = "select * from bill_ysmxb where gcbh = @gcbh and billcode=@billcode ";
            SqlParameter[] cxparamter = { new SqlParameter("@gcbh",gcbh),
                                        new SqlParameter("@billcode",billcode)};
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DataHelper.ExcuteNonQuery(cxsql, tran, paramter, false);
                    tran.Commit();
                    return ListMaker(cxjesql, cxparamter);
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }


        /// <summary>
        /// 根据部门获取科目调整的列表值
        /// </summary>
        /// <param name="strdeptcode"></param>
        /// <returns></returns>
        public DataTable getAlltzlist(string strdeptcode, bool isLong, int pagefrm, int pageto, out int count)
        {
            string sqlkmlist = @"select billJe,billName2,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid,billCode
                ,(select username from bill_users where usercode=billuser) as billUser,billdate 
                , ( select xmmc from bill_ysgc where gcbh in ( select gcbh from Bill_Ysmxb where billCode=bill_main.billCode and ysje>0)) as gcmc
                ,(select mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind
                ,Row_Number()over(order by billDate desc) as crow   from bill_main where flowID='kmystz' and billDept='" + strdeptcode + "'";
            string strsqlcount = "select count(*) from ( {0} ) t";
            strsqlcount = string.Format(strsqlcount, sqlkmlist);
            count = Convert.ToInt32(DataHelper.ExecuteScalar(strsqlcount));

            string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
            strsqlframe = string.Format(strsqlframe, sqlkmlist, pagefrm, pageto);

            return DataHelper.GetDataTable(sqlkmlist, null, false);
        }
    }
}
