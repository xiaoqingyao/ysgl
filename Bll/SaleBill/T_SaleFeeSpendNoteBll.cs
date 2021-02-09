using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.SaleBill;
using Models;
using System.Data;
using System.Data.SqlClient;
using Dal;

namespace Bll.SaleBill
{
    /// <summary>
    /// 提成费用报销类
    /// </summary>
    public class T_SaleFeeSpendNoteBll
    {
        T_SaleFeeSpendNoteDal salefeedal = new T_SaleFeeSpendNoteDal();
        T_SaleFeeSpendNote model = new T_SaleFeeSpendNote();
        public DataTable GetAllListBySql(T_SaleFeeSpendNote model)
        {
            return salefeedal.GetAllListBySql(model);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="modelSaleFeeSpendNote">对象模型</param>
        /// <param name="strErrorMsg">返回错误信息</param>
        /// <returns></returns>
        public int Add(T_SaleFeeSpendNote modelSaleFeeSpendNote, out string strErrorMsg)
        {
            strErrorMsg = "";
            try
            {
                return salefeedal.Add(modelSaleFeeSpendNote);
               
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 获取所有花费的金额
        /// </summary>
        /// <param name="deptCode">单位code</param>
        /// <param name="kmCode">费用科目code</param>
        /// <returns></returns>
        public decimal getSpendAmount(string deptCode, string kmCode)
        {
            string strSql = "select isnull(sum(fee),0) from dbo.T_SaleFeeSpendNote where isnull(status,'')!='D' and Deptcode=@deptCode and Yskmcode=@saleFeeTypeCode";
            SqlParameter[] arrPara ={
                                   new SqlParameter("deptCode",deptCode),
                                   new SqlParameter("saleFeeTypeCode",kmCode)
                                   };
            object objRel = DataHelper.ExecuteScalar(strSql,arrPara,false);
            return objRel == null ? 0 : decimal.Parse(objRel.ToString());
        }
    }
}
