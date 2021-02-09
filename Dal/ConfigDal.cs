using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Models;

namespace Dal
{
    /// <summary>
    /// 参数表dal 对应数据库表t_systemparameter
    /// Edit by Lvcc
    /// </summary>
    public class ConfigDal
    {
        /// <summary>
        /// 根据配置项名称获取配置项值
        /// </summary>
        /// <param name="strKey">配置项名</param>
        /// <returns></returns>
        public string GetValueByKey(string strKey)
        {
            string strSql = "select avalue from t_Config where akey=@akey";
            SqlParameter[] arrSp = { new SqlParameter("@akey", strKey) };
            object objRel = DataHelper.ExecuteScalar(strSql, arrSp, false);
            if (objRel == null)
            {
                return "";
            }
            else
            {
                return objRel.ToString();
            }
        }
        public System.Data.DataTable GetDtByKey(string key)
        {
            string strSql = "select avalue from t_Config where akey=@akey";
            SqlParameter[] arrSp = { new SqlParameter("@akey", key) };
            return DataHelper.GetDataTable(strSql, arrSp, false);
        }
        /// <summary>
        /// 根据配置项名称设置value值
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public int SetValueByKey(string strKey, string strVal)
        {
            string strexitsql = "select count(*) from t_Config where aKey=@key";
            SqlParameter[] arr = { new SqlParameter("@key", strKey) };
            int icount = DataHelper.GetDataTable(strexitsql, arr, false).Rows.Count;
            if (icount > 0)
            {
                string strUpSql = "update t_Config set avalue=@value where aKey=@key";
                SqlParameter[] arrSp = { new SqlParameter("@value", strVal), new SqlParameter("@key", strKey) };
                return DataHelper.ExcuteNonQuery(strUpSql, arrSp, false);
            }
            else
            {
                string strAddSql = "insert into t_Config(akey,meaning,avalue,classify) values (@key,@meaning,@avalue,@classify)";
                SqlParameter[] arrSp = { new SqlParameter("@key", strKey), new SqlParameter("@meaning", ""), new SqlParameter("@avalue", strVal), new SqlParameter("@classify", "0") };
                return DataHelper.ExcuteNonQuery(strAddSql, arrSp, false);
            }
        }

        //isYear 是否年度 isMonth 月度还是季度 tbfs填报方式  parList设置日期
        public bool SetYscs(string isYear, string isMonth, string tbfs, IList<Models.bill_syspar> parList, IList<Bill_SysMenu> menulist, string nd, string ndStatus)
        {
            string upnd = "update bill_SysConfig set ConfigValue='" + isYear + "' where ConfigName='YearBudget' and nd='" + nd + "'  ";
            string upminth = "update   bill_SysConfig set ConfigValue='" + isMonth + "' where ConfigName='MonthOrQuarter'   and nd='" + nd + "'  ";
            string uptbfs = "update   bill_SysConfig set ConfigValue='" + tbfs + "' where ConfigName='ystbfs'   and nd='" + nd + "'  ";
            string upstatus = "update   bill_SysConfig set ConfigValue='" + ndStatus + "' where ConfigName='ndStatus'   and nd='" + nd + "'  ";
            string uppar = "update  bill_syspar set  parval=@parval where parname=@parname";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DataHelper.ExcuteNonQuery(upnd, tran, null, false);
                    DataHelper.ExcuteNonQuery(upminth, tran, null, false);
                    DataHelper.ExcuteNonQuery(uptbfs, tran, null, false);
                    DataHelper.ExcuteNonQuery(upstatus, tran, null, false);
                    foreach (var i in parList)
                    {
                        SqlParameter[] paramter = { new SqlParameter("@parval",i.parVal),
                                                  new SqlParameter("@parname",i.parname)};
                        DataHelper.ExcuteNonQuery(uppar, tran, paramter, false);
                    }
                    foreach (var s in menulist)
                    {
                        SetMenuState(s, tran);
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
        //根据菜单的id修改菜单的状态
        private void SetMenuState(Bill_SysMenu s, SqlTransaction trans)
        {
            string UpMenuStateSql = "update bill_sysMenu set menustate=@menustate where   menuid=@menuid";
            SqlParameter[] patamter = { new SqlParameter("@menustate",s.MenuState),
                                        new SqlParameter("@menuid",s.MenuId)};
            DataHelper.ExcuteNonQuery(UpMenuStateSql, trans, patamter, false);

        }

        public bool AddYscs(string isYear, string isMonth, string tbfs, string nd, string ndStatus)
        {
            string insert1 = @"insert into bill_SysConfig ([ConfigName]
                               ,[ConfigValue]
                               ,[Memo]
                               ,[nd])values('YearBudget',	'" + isYear + "',	'1是启用 其他是不启用',	'" + nd + "')";
            string insert2 = @"insert into bill_SysConfig ([ConfigName]
                               ,[ConfigValue]
                               ,[Memo]
                               ,[nd])values('MonthOrQuarter',	'" + isMonth + "',	'1是季度2是月度',	'" + nd + "')";
            string insert3 = @"insert into bill_SysConfig ([ConfigName]
                               ,[ConfigValue]
                               ,[Memo]
                               ,[nd])values('ystbfs',	'" + tbfs + "',	'0是部门汇总是预算分解',	'" + nd + "')";
            string insert4 = @"insert into bill_SysConfig ([ConfigName]
                               ,[ConfigValue]
                               ,[Memo]
                               ,[nd])values('ndStatus',	'" + ndStatus + "',	'年度开关 1 进行中 0 关闭',	'" + nd + "')";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DataHelper.ExcuteNonQuery(insert1, tran, null, false);
                    DataHelper.ExcuteNonQuery(insert2, tran, null, false);
                    DataHelper.ExcuteNonQuery(insert3, tran, null, false); 
                    DataHelper.ExcuteNonQuery(insert4, tran, null, false);
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
    }
}
