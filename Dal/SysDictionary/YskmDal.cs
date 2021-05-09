using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using Dal.Bills;

namespace Dal.SysDictionary
{
    public class YskmDal
    {
        /// <summary>
        /// 取得所有预算科目
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmAll()
        {
            string sql = " select * from Bill_Yskm where kmStatus='1'";
            return ListMaker(sql, null);
        }

        public IList<Bill_Yskm> GetGkYskmAll()
        {
            string sql = " select * from Bill_Yskm where kmStatus='1' and gkfy='1'";
            return ListMaker(sql, null);
        }
        public void InsertYskm(Bill_Yskm yskm, SqlTransaction tran)
        {
            string sql = @"INSERT INTO bill_yskm(yskmCode,yskmBm,yskmMc,gjfs,tbsm,tblx,kmStatus,kmlx,xmhs,bmhs,ryhs,dydj) VALUES(@yskmCode,@yskmBm,@yskmMc,@gjfs,@tbsm,@tblx,@kmStatus,@kmlx,@xmhs,@bmhs,@ryhs,@dydj)";

            SqlParameter[] parameters = {
					new SqlParameter("@yskmCode", SqlNull(yskm.YskmCode)),
					new SqlParameter("@yskmBm", SqlNull(yskm.YskmBm)),
					new SqlParameter("@yskmMc", SqlNull(yskm.YskmMc)),
					new SqlParameter("@gjfs", SqlNull(yskm.Gjfs)),
					new SqlParameter("@tbsm", SqlNull(yskm.Tbsm)),
					new SqlParameter("@tblx", SqlNull(yskm.Tblx)),
					new SqlParameter("@kmStatus", SqlNull(yskm.KmStatus)),
					new SqlParameter("@kmlx", SqlNull(yskm.KmLx)),
                    new SqlParameter("@gkfy", SqlNull(yskm.GkFy)),
                    new SqlParameter("@xmhs", SqlNull(yskm.XmHs)),
                    new SqlParameter("@bmhs", SqlNull(yskm.BmHs)),
					new SqlParameter("@ryhs", SqlNull(yskm.RyHs)),
                    new SqlParameter("@dydj", SqlNull(yskm.dydj))
                                        };


            DataHelper.ExcuteNonQuery(sql, tran, parameters, false);
        }
        /// <summary>
        /// 插入预算科目
        /// </summary>
        /// <param name="yskm"></param>
        public void InsertYskm(Bill_Yskm yskm)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    if (GetYskmByCode(yskm.YskmCode) != null)
                    {
                        DeleteYskm(yskm.YskmCode, tran);
                    }
                    InsertYskm(yskm, tran);
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
        /// 删除预算科目
        /// </summary>
        /// <param name="kmcode"></param>
        public void DeleteYskm(string kmcode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    if (GetTbysByCode(kmcode) == null)
                    {
                        DeleteYskm(kmcode, tran);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public void DeleteYskm(string kmcode, SqlTransaction tran)
        {
            string sql = @"delete Bill_Yskm where YskmCode=@YskmCode";
            SqlParameter[] sps = { new SqlParameter("@YskmCode", kmcode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        /// <summary>
        /// 根据填报类型取得预算科目(01,单位填报，02财务填报)
        /// </summary>
        /// <param name="ysType"></param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmByType(string ysType)
        {
            string sql = " select * from Bill_Yskm where tblx=@tblx ";
            SqlParameter[] sps = { new SqlParameter("@tblx", ysType) };
            return ListMaker(sql, sps);
        }

        /// <summary>
        /// 获得预算科目
        /// </summary>
        /// <param name="yskmCode"></param>
        /// <returns></returns>
        public Bill_Yskm GetYskmByCode(string yskmCode)
        {
            string sql = "select * from Bill_Yskm where yskmCode=@yskmCode";
            SqlParameter[] sps = { new SqlParameter("@yskmCode", yskmCode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    Bill_Yskm yskm = new Bill_Yskm();
                    yskm.Gjfs = Convert.ToString(dr["Gjfs"]);
                    yskm.KmStatus = Convert.ToString(dr["KmStatus"]);
                    yskm.Tblx = Convert.ToString(dr["Tblx"]);
                    yskm.Tbsm = Convert.ToString(dr["Tbsm"]);
                    yskm.YskmBm = Convert.ToString(dr["YskmBm"]);
                    yskm.YskmCode = Convert.ToString(dr["YskmCode"]);
                    yskm.YskmMc = Convert.ToString(dr["YskmMc"]);
                    yskm.KmLx = Convert.ToString(dr["kmlx"]);
                    yskm.GkFy = Convert.ToString(dr["gkfy"]);
                    yskm.XmHs = Convert.ToString(dr["xmhs"]);
                    yskm.BmHs = Convert.ToString(dr["bmhs"]);
                    yskm.RyHs = Convert.ToString(dr["ryhs"]);
                    yskm.dydj = Convert.ToString(dr["dydj"]);
                    return yskm;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 判断该科目是否已经使用过
        /// </summary>
        /// <param name="yskmCode">科目编号</param>
        /// <returns>预算过程的编号</returns>
        public string GetTbysByCode(string yskmCode)
        {
            string sql = "select gcbh from bill_ysmxb where yskm=@yskmCode";
            SqlParameter[] sps = { new SqlParameter("@yskmCode", yskmCode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    return Convert.ToString(dr["gcbh"]);
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 获得预算科目名称
        /// </summary>
        /// <param name="yskmCode"></param>
        /// <returns></returns>
        public string GetYskmNameCode(string yskmCode)
        {
            Bill_Yskm yskm = GetYskmByCode(yskmCode);
            return "[" + yskm.YskmCode + "]" + yskm.YskmMc;
        }

        public string[] GetYskmCodeByDept(string deptcode)
        {
            string sql = "select yskmcode from bill_yskm_dept where deptcode=@deptcode";
            SqlParameter[] sp = { new SqlParameter("@deptcode", deptcode) };
            DataTable dt = DataHelper.GetDataTable(sql, sp, false);
            string[] ret = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ret[i] = Convert.ToString(dr["yskmcode"]);
                i++;
            }
            return ret;
        }
        public string[] GetYskmCodeByDept(string deptcode, string strdjlx)
        {
            string sql = "select yskmcode from bill_yskm_dept where deptcode=@deptcode and djlx=@djlx";
            SqlParameter[] sp = { new SqlParameter("@deptcode", deptcode), new SqlParameter("@djlx", strdjlx) };
            DataTable dt = DataHelper.GetDataTable(sql, sp, false);
            string[] ret = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ret[i] = Convert.ToString(dr["yskmcode"]);
                i++;
            }
            return ret;
        }

        public string[] GetGkYskmCodeByDept(string deptcode)
        {
            string sql = "select yskmcode from bill_yskm_gkdept where deptcode=@deptcode";
            SqlParameter[] sp = { new SqlParameter("@deptcode", deptcode) };
            DataTable dt = DataHelper.GetDataTable(sql, sp, false);
            string[] ret = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                ret[i] = Convert.ToString(dr["yskmcode"]);
                i++;
            }
            return ret;
        }

        /// <summary>
        /// 根据部门编号获得该部门可以填报/报销的预算科目
        /// </summary>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmByDep(string depcode)
        {
            string sql = "select a.*,('['+yskmcode+']'+yskmMc) as yskmname from Bill_Yskm a where a.yskmcode in(select yskmcode from bill_yskm_dept where deptcode=@deptcode) and kmStatus='1'";
            SqlParameter[] sp = { new SqlParameter("@deptcode", depcode) };
            return ListMaker(sql, sp);
        }
        /// <summary>
        /// 根据部门编号和预算类型获取预算科目传入空字符串代表忽略该条件
        /// </summary>
        /// <param name="depcode">部门编号</param>
        /// <param name="strdydjcode">预算类型对应单据编号 01 收入类单据 02 报销单 03固定资产购置</param>
        ///  <param name="strdydjcode">生成凭证的决算单类型对应bill_datadic的dictype=07</param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskm(string depcode, string strdydjcode, string djlx)
        {
            string sql = "select a.*,('['+yskmcode+']'+yskmMc)as yskmname from Bill_Yskm a where kmStatus='1' ";
            List<SqlParameter> lstSp = new List<SqlParameter>();
            //部门
            if (!string.IsNullOrEmpty(depcode))
            {
                sql += " and a.yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) ";
                lstSp.Add(new SqlParameter("@deptcode", depcode));
            }
            //预算类型
            if (!string.IsNullOrEmpty(strdydjcode))
            {
                sql += " and dydj=@dydj ";
                lstSp.Add(new SqlParameter("@dydj", strdydjcode));
            }
            //凭证决算单类型
            if (!string.IsNullOrEmpty(djlx))
            {
                sql += " and a.yskmcode in (select yskmcode from bill_yskm_dept where djlx=@djlx)";
                lstSp.Add(new SqlParameter("@djlx", djlx));
            }
            return ListMaker(sql, lstSp.ToArray());
        }


        //public IList<Bill_Yskm> GetYskmByDep(string depcode, string strdydjcode, string strdjlx)
        //{
        //    string sql = "select a.*,('['+yskmcode+']'+yskmMc)as yskmname from Bill_Yskm a where a.yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode and djlx=@djlx) and kmStatus='1' and dydj=@dydj ";
        //    SqlParameter[] sp = { new SqlParameter("@deptcode", depcode)
        //                          ,new SqlParameter("@djlx",strdjlx)
        //                          ,new SqlParameter("@dydj",strdydjcode)
        //                        };
        //    return ListMaker(sql, sp);
        //}
        /// <summary>
        /// 根据部门编号获得该部门可以填报的归口费用预算科目
        /// </summary>
        /// <param name="depcode"></param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetGkYskmByDep(string depcode)
        {
            string sql = "";
            bool boIsGkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
            if (boIsGkfj)
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_gkdept where deptcode=@deptcode) and gkfy='1'   and kmStatus='1'";
            }
            else
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) and gkfy='1'   and kmStatus='1'";
            }
            SqlParameter[] sp = { new SqlParameter("@deptcode", depcode) };
            return ListMaker(sql, sp);
        }
        /// <summary>
        /// 根据部门编号获得该部门可以填报/报销的预算科目
        /// </summary>
        /// <param name="depcode">部门编号</param>
        /// <param name="strdydjcode">对应单据编号 01 收入类单据 02 报销单 03固定资产购置 04往来费用</param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetGkYskmByDep(string depcode, string strdydjcode)
        {
            string sql = "";
            bool boIsGkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
            if (boIsGkfj)
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_gkdept where deptcode=@deptcode) and gkfy='1'   and kmStatus='1' and (dydj=@dydj or dydj='04') ";
            }
            else
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) and gkfy='1'   and kmStatus='1'   and (dydj=@dydj or dydj='04')  ";
            }
            SqlParameter[] sp = { new SqlParameter("@deptcode", depcode), new SqlParameter("@dydj", strdydjcode) };
            return ListMaker(sql, sp);
        }


        public IList<Bill_Yskm> GetYskmBydjlx(string strdydjcode)
        {
           string sql = @" select * from Bill_Yskm where dydj=@dydj   ";

            SqlParameter[] sp = { new SqlParameter("@dydj", strdydjcode) };
            return ListMaker(sql, sp);
        }

        public IList<Bill_Yskm> GetGkYskmByDep(string depcode, string strdydjcode, string strdjlx)
        {
            string sql = "";
            bool boIsGkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;
            if (boIsGkfj)
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_gkdept where deptcode=@deptcode ) and gkfy='1'   and kmStatus='1' and (dydj=@dydj or dydj='04') ";
                SqlParameter[] sp = { new SqlParameter("@deptcode", depcode), new SqlParameter("@dydj", strdydjcode) };
                return ListMaker(sql, sp);
            }
            else
            {
                sql = " select * from Bill_Yskm where yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode and djlx=@djlx) and gkfy='1'   and kmStatus='1'   and (dydj=@dydj or dydj='04')  ";
                SqlParameter[] sp2 = { new SqlParameter("@deptcode", depcode), new SqlParameter("@djlx", strdjlx), new SqlParameter("@dydj", strdydjcode) };
                return ListMaker(sql, sp2);
            }

        }
        /// <summary>
        /// 判断是否末级，0，是，> 0，非
        /// </summary>
        /// <param name="yskmcode"></param>
        /// <returns></returns>
        public string GetYskmIsmj(string yskmcode)
        {
            string sql = " select count(*) as sl from Bill_Yskm where yskmCode like '" + yskmcode + "%' and yskmCode <> '" + yskmcode + "'";
            return DataHelper.ExecuteScalar(sql, null, false).ToString();
        }

        private IList<Bill_Yskm> ListMaker(string sql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<Bill_Yskm> list = new List<Bill_Yskm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Yskm yskm = new Bill_Yskm();
                yskm.Gjfs = Convert.ToString(dr["Gjfs"]);
                yskm.KmStatus = Convert.ToString(dr["KmStatus"]);
                yskm.Tblx = Convert.ToString(dr["Tblx"]);
                yskm.Tbsm = Convert.ToString(dr["Tbsm"]);
                yskm.YskmBm = Convert.ToString(dr["YskmBm"]);
                yskm.YskmCode = Convert.ToString(dr["YskmCode"]);
                yskm.YskmMc = Convert.ToString(dr["YskmMc"]);
                yskm.dydj = Convert.ToString(dr["dydj"]);
                string iszyys = Convert.ToString(dr["iszyys"]);
                iszyys = iszyys.Equals("") ? "1" : iszyys;
                yskm.iszyys = iszyys;
                list.Add(yskm);
            }
            return list;
        }
        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }
        /// <summary>
        /// 大智退费调整
        /// </summary>
        /// <param name="depcode"></param>
        /// <param name="tblx"></param>
        /// <param name="nd"></param>
        /// <param name="yskmType"></param>
        /// <returns></returns>

        public IList<Bill_Yskm> GetYskmByDeptTblx_tf(string depcode, string tblx, string nd, string yskmType)
        {
            if (new Dal.Bills.SysconfigDal().GetsysConfigBynd(nd)["ystbfs"] == "0")//0是部门汇总
            {
                string sql = " select * from Bill_Yskm where  yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) and tblx=@tblx   and yskmMc like '%退费%'";//  and isnull(iszyys,'1')='1' 
                if (!string.IsNullOrEmpty(yskmType))
                {
                    sql += " and dydj='" + yskmType + "'";
                }
                SqlParameter[] sp = { new SqlParameter("@deptcode", depcode),
                                 new SqlParameter("@tblx",tblx)};
                return ListMaker(sql, sp);
            }
            else//1是预算分解
            {
                if (tblx == "02")
                {
                    return null;
                }
                else
                {
                    bool boIsgkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;

                    if (boIsgkfj)
                    {
                        string cxsql = @"select * from bill_yskm where yskmcode in (select  distinct kmcode  from bill_ys_xmfjbm where deptcode = @deptcode and by3 = '2'  and  left(procode,4) = @nd) 
                                            and yskmcode not in (select distinct yskmcode from bill_yskm_gkdept gkdept inner join bill_ys_xmfjbm xmfjbm on gkdept.yskmcode=xmfjbm.kmcode where xmfjbm.procode=@nd
                                            and xmfjbm.deptcode=@deptcode and  gkdept.deptcode=@deptcode)";//and isnull(iszyys,'1')='1'
                        if (!string.IsNullOrEmpty(yskmType))
                        {
                            cxsql += " and dydj='" + yskmType + "'";
                        }
                        SqlParameter[] paramter = { new SqlParameter("@deptcode",depcode),
                                            new SqlParameter("@nd",nd)};
                        return ListMaker(cxsql, paramter);
                    }
                    else
                    {
                        string cxsql = @"select * from bill_yskm where yskmcode in (select  yskmcode from bill_yskm_dept where deptcode = @deptcode )  and yskmMc like '%退费%'";// and isnull(iszyys,'1')='1'
                        if (!string.IsNullOrEmpty(yskmType))
                        {
                            cxsql += " and dydj='" + yskmType + "'";
                        }
                        SqlParameter[] paramter = { new SqlParameter("@deptcode", depcode) };
                        return ListMaker(cxsql, paramter);
                    }
                }
            }
        }

        /// <summary>
        ///根据部门和填报类型获取预算科目
        /// </summary>
        /// <param name="yskmcode"></param>
        /// <returns></returns>
        public IList<Bill_Yskm> GetYskmByDeptTblx(string depcode, string tblx, string nd, string yskmType)
        {
            if (new Dal.Bills.SysconfigDal().GetsysConfigBynd(nd)["ystbfs"] == "0")//0是部门汇总
            {
                string sql = " select * from Bill_Yskm where  yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) and tblx=@tblx  and yskmMc not  like '%退费%' ";//  and isnull(iszyys,'1')='1'   and yskmBm not like '0234%'
                if (!string.IsNullOrEmpty(yskmType))
                {
                    sql += " and dydj='" + yskmType + "'";
                }
                SqlParameter[] sp = { new SqlParameter("@deptcode", depcode),
                                 new SqlParameter("@tblx",tblx)};
                return ListMaker(sql, sp);
            }
            else//1是预算分解
            {
                if (tblx == "02")
                {
                    return null;
                }
                else
                {
                    bool boIsgkfj = new Dal.ConfigDal().GetValueByKey("UseGKFJ").Equals("1") ? true : false;

                    if (boIsgkfj)
                    {
                        string cxsql = @"select * from bill_yskm where yskmcode in (select  distinct kmcode  from bill_ys_xmfjbm where deptcode = @deptcode and by3 = '2'  and  left(procode,4) = @nd) 
                                            and yskmcode not in (select distinct yskmcode from bill_yskm_gkdept gkdept inner join bill_ys_xmfjbm xmfjbm on gkdept.yskmcode=xmfjbm.kmcode where xmfjbm.procode=@nd
                                            and xmfjbm.deptcode=@deptcode and  gkdept.deptcode=@deptcode)";//and isnull(iszyys,'1')='1'
                        if (!string.IsNullOrEmpty(yskmType))
                        {
                            cxsql += " and dydj='" + yskmType + "'";
                        }
                        SqlParameter[] paramter = { new SqlParameter("@deptcode",depcode),
                                            new SqlParameter("@nd",nd)};
                        return ListMaker(cxsql, paramter);
                    }
                    else
                    {
                        string cxsql = @"select * from bill_yskm where yskmcode in (select  yskmcode from bill_yskm_dept where deptcode = @deptcode ) ";// and isnull(iszyys,'1')='1' and yskmMc not  like '%退费% 大智专用
                        if (!string.IsNullOrEmpty(yskmType))
                        {
                            cxsql += " and dydj='" + yskmType + "'";
                        }
                        SqlParameter[] paramter = { new SqlParameter("@deptcode", depcode) };
                        return ListMaker(cxsql, paramter);
                    }
                }
            }
        }

        public IList<Bill_Yskm> GetYskmByDeptTblx_qt(string depcode, string tblx, string nd, string yskmType)
        {
            string text = " select * from Bill_Yskm where  yskmcode in (select yskmcode from bill_yskm_dept where deptcode=@deptcode) and tblx=@tblx and isnull(iszyys,0)=0  and yskmMc not like '%退费%'";
            if (!string.IsNullOrEmpty(yskmType))
            {
                text = text + " and dydj='" + yskmType + "'";
            }
            SqlParameter[] sps = new SqlParameter[]
            {
        new SqlParameter("@deptcode", depcode),
        new SqlParameter("@tblx", tblx)
            };
            return this.ListMaker(text, sps);
        }


        #region 服务于预算部门分解
        public DataTable GetDeptByNd(string p, string strstatus, string strxmcode)
        {
            string strsqlappend = " and 1=1";
            if (!strstatus.Equals(""))
            {
                strsqlappend += " and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='" + strstatus + "') and deptcode in (select deptcode from bill_ys_xmfjbm where by3='" + strstatus + "')";
            }
            if (!strxmcode.Equals(""))
            {
                strsqlappend += " and yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode='" + strxmcode + "')";
            }
            string cxsql = string.Format(@"select * from (select distinct deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=bill_yskm_dept.yskmcode) as yskmname,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=bill_yskm_dept.deptcode)  as deptname
                               from dbo.bill_yskm_dept where  yskmcode in (select distinct kmcode from bill_ys_xmfjlrb  where left(procode,4) = @nd {0} and kmcode not in (select yskmcode from bill_yskm where kmStatus!='1') )  and deptcode <> '') b where b.deptname!=''", strsqlappend);
            SqlParameter[] paramter = { new SqlParameter("@nd", p) };
            return DataHelper.GetDataTable(cxsql, paramter, false);
        }

        public DataTable GetGkDeptBynd(string p, string strstatus, string strxmcode)
        {
            string strsqlappend = " and 1=1";
            if (!strstatus.Equals(""))
            {
                strsqlappend += " and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='" + strstatus + "') and deptcode in (select deptcode from bill_ys_xmfjbm where by3='" + strstatus + "')";
            }
            if (!strxmcode.Equals(""))
            {
                strsqlappend += " and yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode='" + strxmcode + "')";
            }
            string cxsql = string.Format(@"select * from (select distinct deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=bill_yskm_gkdept.yskmcode) as yskmname,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=bill_yskm_gkdept.deptcode)  as deptname
                               from dbo.bill_yskm_gkdept where  yskmcode in (select distinct kmcode from bill_ys_xmfjlrb  where left(procode,4) = @nd and kmcode not in (select yskmcode from bill_yskm where kmStatus!='1' )) {0}  and deptcode <> '') b where b.deptname!='' order by yskmcode", strsqlappend);
            SqlParameter[] paramter = { new SqlParameter("@nd", p) };
            return DataHelper.GetDataTable(cxsql, paramter, false);
        }
        public DataTable GetDeptByYskm(string yskm, string strstatus, string strxmcode)
        {
            string strsqlappend = " and 1=1";
            if (!strstatus.Equals(""))
            {
                strsqlappend += " and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='" + strstatus + "') and deptcode in (select deptcode from bill_ys_xmfjbm where by3='" + strstatus + "')";
            }
            if (!strxmcode.Equals(""))
            {
                strsqlappend += " and yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode='" + strxmcode + "')";
            }
            string cxsql = string.Format(@"select * from (select distinct deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=bill_yskm_dept.yskmcode) as yskmname,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=bill_yskm_dept.deptcode)  as deptname
                               from dbo.bill_yskm_dept where  yskmcode  like  '" + yskm + "%'  and deptcode <> ''{0}) b where b.deptname!=''", strsqlappend);
            //SqlParameter[] paramter = { new SqlParameter("@yskmcode", yskm) };
            return DataHelper.GetDataTable(cxsql, null, false);
        }
        public DataTable GetGkDeptByYskm(string yskm, string strstatus, string strxmcode)
        {
            string strsqlappend = " and 1=1";
            if (!strstatus.Equals(""))
            {
                strsqlappend += " and yskmcode in (select kmcode from bill_ys_xmfjbm where by3='" + strstatus + "') and deptcode in (select deptcode from bill_ys_xmfjbm where by3='" + strstatus + "')";
            }
            if (!strxmcode.Equals(""))
            {
                strsqlappend += " and yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode='" + strxmcode + "')";
            }
            string cxsql = string.Format(@"select * from (select deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=bill_yskm_dept.yskmcode) as yskmname,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=bill_yskm_dept.deptcode)  as deptname
                               from dbo.bill_yskm_dept where  yskmcode = @yskmcode  and deptcode <> ''{0}) b where b.deptname!=''", strsqlappend);

            string strexitsql = "select count(*) from bill_yskm_gkdept where yskmcode=@yskmcode";
            object objRel = null;
            try
            {
                objRel = DataHelper.ExecuteScalar(strexitsql, new SqlParameter[] { new SqlParameter("@yskmcode", yskm) }, false);
            }
            catch (Exception) { }
            bool bl;
            if (objRel == null)
            {
                bl = false;
            }
            else
            {
                bl = int.Parse(objRel.ToString()) > 0 ? true : false;
            }
            if (!bl)
            {
                return DataHelper.GetDataTable(cxsql, new SqlParameter[] { new SqlParameter("@yskmcode", yskm) }, false);
            }
            else
            {
                cxsql = string.Format("select * from (select deptcode,yskmcode,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=bill_yskm_gkdept.yskmcode) as yskmname,(select  '['+deptcode+']'+ deptname from bill_departments where bill_departments.deptcode=bill_yskm_gkdept.deptcode) as deptname from bill_yskm_gkdept where yskmcode=@yskmcode and deptcode<>''{0} ) b where b.deptname!=''", strsqlappend);
                return DataHelper.GetDataTable(cxsql, new SqlParameter[] { new SqlParameter("@yskmcode", yskm) }, false);
            }
        }

        #endregion
        public IList<Bill_Yskm> GetChilds(string strCode, string strDeptcode)
        {
            string strSql = "select * from bill_yskm where yskmCode like '" + strCode + "%' and len(yskmCode)>len('" + strCode + "') and yskmCode in(select yskmCode from bill_yskm_dept where deptCode='" + strDeptcode + "')";
            return ListMaker(strSql, null);
        }


        public bool InsertList(IList<Bill_Yskm> yskms)
        {
            string insql = " insert into bill_yskm(yskmCode,yskmBm,yskmMc, tbsm,tblx,kmStatus,kmlx,gkfy,xmhs,bmhs,ryhs,kmzg,dydj) values (@yskmCode,@yskmBm,@yskmMc,@tbsm,@tblx,@kmStatus,@kmlx,@gkfy,@xmhs,@bmhs,@ryhs,@kmzg,@dydj)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in yskms)
                    {
                        SqlParameter[] inparamter = { new SqlParameter("@yskmCode", SqlNull(i.YskmCode)),
					                                    new SqlParameter("@yskmBm", SqlNull(i.YskmBm)),
					                                    new SqlParameter("@yskmMc", SqlNull(i.YskmMc)),
					                                    new SqlParameter("@tbsm", SqlNull(i.Tbsm)),
					                                    new SqlParameter("@tblx", SqlNull(i.Tblx)),
					                                    new SqlParameter("@kmStatus", SqlNull(i.KmStatus)),
					                                    new SqlParameter("@kmlx", SqlNull(i.KmLx)),
                                                        new SqlParameter("@gkfy", SqlNull(i.GkFy)),
                                                        new SqlParameter("@xmhs", SqlNull(i.XmHs)),
                                                        new SqlParameter("@bmhs", SqlNull(i.BmHs)),
					                                    new SqlParameter("@ryhs", SqlNull(i.RyHs)),
                                                        new SqlParameter("@kmzg",SqlNull(i.Kmzg)),
                                                        new SqlParameter("@dydj", SqlNull(i.dydj))};
                        DataHelper.ExcuteNonQuery(insql, inparamter, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }

        public bool Exists(string yskmCode)
        {
            string strSql = "select count(*) from bill_yskm where yskmCode='" + yskmCode + "' ";
            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql, null, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }

}

