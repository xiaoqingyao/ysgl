using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.TruckType;
using System.Data;
using Models;
using Dal;
using System.Data.SqlClient;

namespace Bll.TruckType
{
    /// <summary>
    /// 卡车类别管理类
    /// Edit by Lvcc
    /// </summary>
    public class TruckTypeBLL
    {
        TruckTypeDal dalTruckType = new TruckTypeDal();
        public DataSet GetAll()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dalTruckType.ReturnAllDt());
            return ds;
        }
        public T_truckType GetModel(string strCode)
        {
            return dalTruckType.GetModel(strCode);
        }
        /// <summary>
        /// 判断某记录是否有子节点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool HasChildren(string p)
        {
            IList<T_truckType> ilist = dalTruckType.GetAllChildren(p);
            if (ilist.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Add(T_truckType modelTruckType, out string strMsg)
        {
            try
            {
                strMsg = "";
                return dalTruckType.Add(modelTruckType);
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return 0;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modelTruckType"></param>
        /// <returns></returns>
        public int Upd(T_truckType modelTruckType, out string strmsg)
        {
            try
            {
                strmsg = "";
                return dalTruckType.updatemodel(modelTruckType);
            }
            catch (Exception ex)
            {
                strmsg = ex.Message;
                return 0;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int Delete(string strCode, out string msg)
        {
            try
            {
                //先获取model以操作上级节点
                T_truckType modelTruckType = this.GetModel(strCode);
                msg = "";
                int iRel = dalTruckType.Delete(strCode);
                if (iRel < 1)
                {
                    throw new Exception("删除车辆类型时失败！");
                }

                //删除车辆对应关系
                new T_TruckTypeCorrespondBLL().DeleteByTruckTypeCode(strCode);

                //判断父节点的子节点数量 如果为零 则修改状态为末节点
                if (modelTruckType != null && modelTruckType.parentCode != "0")
                {
                    int childrenCount = this.dalTruckType.GetAllChildren(modelTruckType.parentCode).Count;
                    if (childrenCount == 0)
                    {
                        string strUp = "update T_truckType set IsLastNode='1' where typeCode=@typeCode";
                        SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@typeCode", modelTruckType.parentCode) };
                        int iRel2 = DataHelper.ExcuteNonQuery(strUp, arrSp, false);
                        if (iRel2 < 1)
                        {
                            throw new Exception("在修改父节点状态时出现异常！");
                        }
                    }
                    else
                    {
                        throw new Exception("该节点下有子节点，不允许删除！");
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 修改父节点是否是子节点的状态
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public int changeParentStatus(string strCode)
        {
            int iRel = dalTruckType.changeParentStatus(strCode);
            if (iRel < 1)
            {
                throw new Exception("修改车辆类型状态时失败！");
            }
            new T_TruckTypeCorrespondBLL().DeleteByTruckTypeCode(strCode);
            return 1;
        }
        /// <summary>
        /// 判断该编号已经存在
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool IsTruckCode(string strTruckCode)
        {
            string strSql = "select count(*) from T_truckType where typeCode='" + strTruckCode + "'";
            //判断是否是特殊返利申请通过
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
    }
}
