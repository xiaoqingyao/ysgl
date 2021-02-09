using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Zichan;
using Models;
using Bll.Bills;
using System.Data;

namespace Bll.Zichan
{
    public class ZengJianFangShiBll
    {
        ZiChan_ZengJianFangShiDal zjfsdal = new  ZiChan_ZengJianFangShiDal();
        ZiChan_ZengJianFangShi zjfsmodel = new ZiChan_ZengJianFangShi();
        BillMainBLL bllMainBill = new BillMainBLL();
        public DataTable GetAllListBySql1(ZiChan_ZengJianFangShi model)
        {
            return zjfsdal.GetAllListBySql1(model);
        }



        public IList<ZiChan_ZengJianFangShi> GetListByBillCode(string FangshiCode)
        {
            string sql = "select * from ZiChan_ZengJianFangShi where FangshiCode='" + FangshiCode + "'";
            return zjfsdal.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.ZiChan_ZengJianFangShi emodeL)
        {
            return zjfsdal.Add(emodeL);
        }

        public bool Addlistmode(IList<ZiChan_ZengJianFangShi> listmodel)
        {
            return zjfsdal.Addlistmodel(listmodel);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode)
        {
            return zjfsdal.Delete(strMainCode);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool AddNote(IList<ZiChan_ZengJianFangShi> emodel, out string strMsg)
        {
            try
            {
                strMsg = "";

                if (this.Addlistmode(emodel))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 得到一个实例
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public ZiChan_ZengJianFangShi GetModel(string strBillCode)
        {
            return zjfsdal.GetModel(strBillCode);
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="strTrckCode"></param>
        /// <returns></returns>

        //public bool isExist(string strTrckCode)
        //{
        //    string sql = "select count(*) from T_Remittance where TruckCode='" + strTrckCode + "'";
        //    object objRel = DataHelper.ExecuteScalar(sql);
        //    string iRel = objRel == null ? "0" : objRel.ToString();
        //    return iRel == "0" ? false : true;
        //}

        ///// <summary>
        ///// 通过单据编号获取车辆code字符串
        ///// </summary>
        ///// <param name="strBillCode"></param>
        ///// <returns></returns>
        //public string getBillTruckCode(string strBillCode)
        //{
        //    string strSql = "select TruckCode from T_Remittance  where NID='" + strBillCode + "' ";
        //    DataTable dtRel = DataHelper.GetDataTable(strSql, null, false);
        //    if (dtRel.Rows.Count > 0)
        //    {
        //        StringBuilder sbRel = new StringBuilder();
        //        for (int i = 0; i < dtRel.Rows.Count; i++)
        //        {
        //            sbRel.Append(dtRel.Rows[i][0].ToString());
        //            sbRel.Append(",");
        //        }
        //        return sbRel.ToString().Substring(0, sbRel.Length - 1);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        ///// <summary>
        ///// 获取订单号
        ///// </summary>
        ///// <param name="strBillCode"></param>
        ///// <returns></returns>
        //public string getBillOrderCode(string strBillCode)
        //{
        //    string strSql = "select OrderCode from T_Remittance where NID='" + strBillCode + "'";
        //    DataTable dtRel = DataHelper.GetDataTable(strSql, null, false);
        //    if (dtRel.Rows.Count > 0)
        //    {
        //        StringBuilder sbRel = new StringBuilder();
        //        for (int i = 0; i < dtRel.Rows.Count; i++)
        //        {
        //            sbRel.Append(dtRel.Rows[i][0].ToString());
        //            sbRel.Append(",");
        //        }
        //        return sbRel.ToString().Substring(0, sbRel.Length - 1);
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
    }
}
