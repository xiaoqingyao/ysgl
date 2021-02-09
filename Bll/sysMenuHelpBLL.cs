using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal;
using System.Data.SqlClient;
using System.Data;

namespace Bll
{
    public class sysMenuHelpBLL
    {
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="menuid"></param>
        /// <param name="content"></param>
        /// <returns></returns>

        public int Add(string menuid, string content)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    Delete(menuid, tran);
                    int introw = Add(menuid, content, tran);
                    tran.Commit();
                    return introw;
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
        public int Delete(string menuid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    int intRow = Delete(menuid, tran);
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
        public int Add(string menuid, string content, SqlTransaction tran)
        {

            try
            {
                string Sql = "insert into  bill_sysMenuHelp  (menuid,menusm) values (@menuid,@menusm)";

                SqlParameter[] parms = new SqlParameter[] {
            new SqlParameter("@menuid", menuid),
            new SqlParameter ("@menusm",content)};
                return DataHelper.ExcuteNonQuery(Sql, tran, parms, false);
            }
            catch (Exception)
            {

                throw;
            }
        }




        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string menuid, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from bill_sysMenuHelp ");
            strSql.Append(" where menuid=@menuid ");
            SqlParameter[] parameters = {
					new SqlParameter("@menuid", SqlDbType.VarChar,20)		};
            parameters[0].Value = menuid;
            return DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        /// <summary>
        ///根据菜单的menuid获取菜单帮助信息并返回
        /// </summary>
        /// <param name="menuid"></param>
        /// <returns></returns>
        public string GetContent(string menuid)
        {

            try
            {
                string result = "";
                result = Convert.ToString(DataHelper.ExecuteScalar("select menusm from  bill_sysMenuHelp where menuid='" + menuid + "' "));
                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// 根据菜单的showName获取帮助信息，并返回
        /// </summary>
        /// <param name="showName"></param>
        /// <returns></returns>
        public string GetContentByShowName(string showName)
        {
            try
            {
                string result = "";
                result = Convert.ToString(DataHelper.ExecuteScalar("select menusm from bill_sysMenuHelp where menuid =( select top 1 menuid from bill_sysMenu where showname =('" + showName + "') and  isnull(menustate,'1') !='D')"));
                return result;
            }
            catch (Exception)
            {
                return "";
            }

        }
    }
}
