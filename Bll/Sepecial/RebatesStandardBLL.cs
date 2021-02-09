using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;

namespace Bll.Sepecial
{
    public class RebatesStandardBLL
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        Dal.SaleProcess.RebatesStandardDal spedal = new Dal.SaleProcess.RebatesStandardDal();

        public int Insert(IList<T_RebatesStandard> Ilistmode, out string msg)
        {
            try
            {
                msg = "";
                //spedal.Delete(Ilistmode[0].TruckTypeCode.ToString(), Ilistmode[0].DeptCode.ToString(), Ilistmode[0].SaleFeeTypeCode.ToString(),Ilistmode[0].ControlItemCode.ToString());
                for (int i = 0; i < Ilistmode.Count; i++)
                {
                    bool bo = this.AuditModel(Ilistmode[i], out msg);
                    if (!bo)
                    {
                        throw new Exception(msg);
                    }
                    int iRel = spedal.Add(Ilistmode[i]);
                    if (iRel <= 0)
                    {
                        throw new Exception("未知错误！");
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

        public DataTable getalltable(T_RebatesStandard model)
        {

            return spedal.GetAlltable(model);
        }

        public void Deletemode(T_RebatesStandard model)
        {
            spedal.Delete(model.TruckTypeCode, model.DeptCode, model.SaleFeeTypeCode, model.ControlItemCode, model.EffectiveDateFrm, model.EffectiveDateTo, model.SaleCountFrm, model.SaleCountTo);
        }

        /// <summary>
        /// 验证model的合法性
        /// </summary>
        /// <returns></returns>
        private bool AuditModel(T_RebatesStandard model, out string msg)
        {
            msg = "";
            bool flg = true;
            if (model.ControlItemCode=="Qcfp")
            {
                flg = true;
            }
            else
            {
                string strsql = @"select count(*) from dbo.T_RebatesStandard where (('" + model.EffectiveDateFrm + "'>=EffectiveDateFrm and '" + model.EffectiveDateFrm + "'<=EffectiveDateTo) or ('" + model.EffectiveDateTo + "'<= EffectiveDateTo and '" + model.EffectiveDateTo +
                          "'>=EffectiveDateFrm ) )and  TruckTypeCode='" + model.TruckTypeCode + "' and DeptCode='" + model.DeptCode + "' and SaleFeeTypeCode='"+model.SaleFeeTypeCode+"' and ControlItemCode='" + model.ControlItemCode
                          + "' and ((" + model.SaleCountFrm + ">=SaleCountFrm and " + model.SaleCountFrm +
                          "<=SaleCountTo )or (" + model.SaleCountTo + "<=SaleCountTo and " + model.SaleCountTo + ">=SaleCountFrm))";
                int ierx = int.Parse(server.ExecuteScalar(strsql).ToString());
                if (ierx > 0)
                {
                    msg = "该返利标准与数据库中已存在的标准存在交集，请重新设定！";
                    flg = false;
                }
            }

           
            return flg;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="modelRebatesStandard"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int Insert(T_RebatesStandard modelRebatesStandard, out string strMsg)
        {
            if (!this.AuditModel(modelRebatesStandard,out strMsg))
            {
                return 0;   
            }
            return spedal.Add(modelRebatesStandard);
        }
    }
}
