using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;
using Dal;
using System.Data.SqlClient;
using System.Data;

namespace Bll.Bills
{
    public class bill_yksqBll
    {
        bill_yksqDal dal = new bill_yksqDal();

        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();


        /// <summary>
        /// 根据查询条件分页并返回记录数 
        /// </summary>
        public IList<bill_yksq> GetAllList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
        {
          return  dal.GetAllList(beg,end,paramter,sqls,out totalCount);
        }

        /// <summary>
        /// 得到一个实例
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public bill_yksq GetModel(string strBillCode)
        {
            return dal.GetModel(strBillCode);
        }
        /// <summary>
        /// 删除  删除本表 删除主表 清空所选入库单的cdf13
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public bool Deletes(string strMainCode)
        {
            bool isSet;
            string dbMsg = GetDbName(out isSet);
            if (!isSet) //如果没有配置直接返删除失败
            {
                return false;
            }
            string rkCodes = GetCodes(strMainCode,dbMsg);
            string strUfsql = "";
            if (string.IsNullOrEmpty(rkCodes))
            {
                strUfsql = "update    " + dbMsg + "  set cDefine13='' where ID in (" + rkCodes + ")";
            }
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    new MainDal().DeleteMain(strMainCode, tran);
                    dal.Delete(strMainCode, tran);
                    if (string.IsNullOrEmpty(strUfsql))
                        DataHelper.ExcuteNonQuery(strUfsql, tran, null, false);
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
        /// 添加 添加本表 添加主表 将所选的入库的cdf13 赋值为本主表的主键
        /// </summary>
        /// <param name="model"></param>
        public bool Add(bill_yksq model, Bill_Main main)
        {
            string[] arrCodes = model.rkCodes.Split(',');
            string dbMsg = "";
            if (arrCodes.Length > 0)
            {
                bool isSet;
                dbMsg = GetDbName(out isSet);
                if (!isSet) //如果没有配置直接返删除失败
                    return false;
            }
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    new MainDal().DeleteMain(main.BillCode, tran);
                    new MainDal().InsertMain(main, tran);
                    dal.Delete(model.billCode, tran);
                    dal.Add(model, tran);
                    for (int i = 0; i < arrCodes.Length; i++)
                    {
                        string temp = "update    " + dbMsg + "  set cDefine13='" + main.BillCode + "' where ID= " + arrCodes[i];
                        DataHelper.ExcuteNonQuery(temp, tran, null, false);
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
        /// 删除  删除本表 清空所选入库单的cdf13
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public bool Delete(string strMainCode)
        {
            bool isSet;
            string dbMsg = GetDbName(out isSet);
            if (!isSet) //如果没有配置直接返删除失败
            {
                return false;
            }
            string rkCodes = GetCodes(strMainCode, dbMsg);
            string strUfsql = "";
            if (!string.IsNullOrEmpty(rkCodes))
            {
                strUfsql = "update   " + dbMsg + "  set cDefine13='' where ID in (" + rkCodes + ")";
            }
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    dal.Delete(strMainCode, tran);
                    if (!string.IsNullOrEmpty(strUfsql))
                        DataHelper.ExcuteNonQuery(strUfsql, tran, null, false);
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
        /// 添加 添加本表 添加主表 将所选的入库的cdf13 赋值为本主表的主键
        /// </summary>
        /// <param name="model"></param>
        public bool Add(bill_yksq model)
        {
            //string[] arrCodes = model.rkCodes.Split(',');
            //string dbMsg = "";
            //if (arrCodes.Length > 0)
            //{
            //    bool isSet;
            //    dbMsg = GetDbName(out isSet);
            //    if (!isSet) //如果没有配置直接返删除失败
            //        return false;
            //}
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    dal.Delete(model.billCode, tran);
                    dal.Add(model, tran);
                    //for (int i = 0; i < arrCodes.Length; i++)
                    //{
                    //    if (string.IsNullOrEmpty(arrCodes[i]))
                    //    {
                    //        continue;
                    //    }
                    //    string temp = "update    " + dbMsg + "  set cDefine13 ='" + model.billCode + "' where ID = " + arrCodes[i];
                    //    DataHelper.ExcuteNonQuery(temp, tran, null, false);
                    //}
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
        /// 根据配置项获取 呼获取链接数据库的字段
        /// </summary>
        /// <returns></returns>
        public string GetDbName()
        {

            string ret = "";
            string ip = server.GetCellValue("select avalue  from t_Config  where akey='ykspIP' ");
            string dbName = server.GetCellValue("select avalue  from t_Config  where akey='ykspDbName' ");
            string structure = server.GetCellValue("select avalue  from t_Config  where akey='ykspStructureName' ");
            string tbName = server.GetCellValue("select avalue  from t_Config  where akey='ykspTbName' ");

            if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(structure) && !string.IsNullOrEmpty(tbName))
                ret = dbName + "." + structure + "." + tbName;

            return ret;
        }


        /// <summary>
        ///  获取数据库 配置  返回true说明已配置好必须项 返回false 需要配置相关配置项
        /// </summary>
        /// <param name="isSet">返回是否配置好</param>
        /// <returns></returns>
        public string GetDbName(out bool isSet)
        {
            string strRet = GetDbName();
            isSet = string.IsNullOrEmpty(strRet) ? false : true;
            return strRet;
        }
        /// <summary>
        /// 根据配置项获取 呼获取链接数据库的字段
        /// </summary>
        /// <returns></returns>
        public string[] GetDbNames()
        {

            string[] ret=new string[2]{"",""};
            string ip = server.GetCellValue("select avalue  from t_Config  where akey='ykspIP' ");
            string dbName = server.GetCellValue("select avalue  from t_Config  where akey='ykspDbName' ");
            string structure = server.GetCellValue("select avalue  from t_Config  where akey='ykspStructureName' ");
            string tbName = server.GetCellValue("select avalue  from t_Config  where akey='ykspTbName' ");
            string tbNames = server.GetCellValue("select avalue  from t_Config  where akey='ykspTbNames' ");

            if (!string.IsNullOrEmpty(dbName) && !string.IsNullOrEmpty(structure) && !string.IsNullOrEmpty(tbName) && !string.IsNullOrEmpty(tbNames))
                {
                ret[0] = dbName + "." + structure + "." + tbName; 
                ret[1] = dbName + "." + structure + "." + tbNames;
                }

            return ret;
        }


        /// <summary>
        ///  获取数据库 配置  返回true说明已配置好必须项 返回false 需要配置相关配置项
        /// </summary>
        /// <param name="isSet">返回是否配置好</param>
        /// <returns></returns>
        public string[] GetDbNames(out bool isSet)
        {
            string[] strRet = GetDbNames();
            isSet = (string.IsNullOrEmpty(strRet[0])||string.IsNullOrEmpty(strRet[1]))? false : true;
            return strRet;
        }
        
        
        private string GetCodes(string billCode, string dbString)
        {
            string ret = "";

            DataTable dt = server.GetDataTable("select rkCodes from bill_yksq where billCode='" + billCode + "'", null);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ret = "" + Convert.ToString(dt.Rows[i]["rkCodes"]) + ",";
            }
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }


    }
}
