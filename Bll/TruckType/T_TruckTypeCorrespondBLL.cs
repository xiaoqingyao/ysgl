using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.TruckType;
using Models;
using Dal;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace Bll.TruckType
{
    /// <summary>
    /// 车辆对象表
    /// </summary>
    public class T_TruckTypeCorrespondBLL
    {
        T_TruckTypeCorrespondDal Dal = new T_TruckTypeCorrespondDal();

        public int Add(T_TruckTypeCorrespond model,out string strMsg) 
        {
            try
            {
                strMsg = "";
                return Dal.Add(model);

            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                throw;
            }
        
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="strTrckCode"></param>
        /// <returns></returns>
        public bool isExist(string strTrckCode)
        {
            string sql = "select count(*) from T_TruckTypeCorrespond where factTruckType='" + strTrckCode + "' and isnull(truckTypeCode,'')!=''";
            object objRel = DataHelper.ExecuteScalar(sql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 添加未对应的值
        /// </summary>
        /// <param name="strTrckCode"></param>
        /// <returns></returns>
        public bool isExistfact(string strTrckCode)
        {
            string sql = "select count(*) from T_TruckTypeCorrespond where factTruckType='" + strTrckCode + "'";
            object objRel = DataHelper.ExecuteScalar(sql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable getalltable(T_TruckTypeCorrespond model)
        {
            return Dal.GetAlltable(model);
        }

        /// <summary>
        /// 根据车辆类型编号 删除对应关系
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public int DeleteByTruckTypeCode(string strCode)
        {
            string strSql = "delete from T_TruckTypeCorrespond where truckTypeCode=@truckTypeCode";
            SqlParameter[] arrSqlPara=new SqlParameter[1];
            arrSqlPara[0] = new SqlParameter("@truckTypeCode", strCode);
            return DataHelper.ExcuteNonQuery(strSql, arrSqlPara, false);
        }

        public int DeleteByIndex(long index){
            return Dal.Delete(index);
        }

        /// <summary>
        /// 根据内部车架号 删除对应关系
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public int DeleteByNbcxh(string strCode)
        {
            string strSql = "delete from T_TruckTypeCorrespond where factTruckType=@factTruckType";
            SqlParameter[] arrSqlPara = new SqlParameter[1];
            arrSqlPara[0] = new SqlParameter("@factTruckType", strCode);
            return DataHelper.ExcuteNonQuery(strSql, arrSqlPara, false);
        }
        /// <summary>
        /// 根据内部车型号找设置车辆类型对应的车辆类型号
        /// </summary>
        /// <param name="strTrckCode"></param>
        /// <returns></returns>
        public string GetTruckTypeNumByFactCode(string strTrckCode)
        {
            string strSql = "select truckTypeCode from T_TruckTypeCorrespond where factTruckType=@factTruckType";
            SqlParameter[] arrSqlPara = new SqlParameter[1];
            arrSqlPara[0] = new SqlParameter("@factTruckType",strTrckCode);
            object objRel = DataHelper.ExecuteScalar(strSql, arrSqlPara, false);
            return objRel == null ? "" : objRel.ToString();
        }
        /// <summary>
        /// 通过内部车型号获取model
        /// </summary>
        /// <param name="strFactTruckCode"></param>
        /// <returns></returns>
        public IList<T_TruckTypeCorrespond> GetModelListByFactTruckCode(string strFactTruckCode)
        {
            string strSql = "select * from T_TruckTypeCorrespond where factTruckType=@factTruckType";
            SqlParameter[] arrSp={
                new SqlParameter("@factTruckType",strFactTruckCode)
            };
            IList <T_TruckTypeCorrespond> listModel=Dal.ListMaker(strSql, arrSp);
            if (listModel == null || listModel.Count <= 0)
            {
                return null;
            }
            else {
                return listModel;
            }
        }
    }
}
