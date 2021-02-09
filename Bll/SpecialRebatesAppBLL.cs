using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dal;
using Models;
using Bll.Bills;

namespace Bll
{
    public class SpecialRebatesAppBLL
    {
        Dal.SaleProcess.SpecialRebatesAppDal specidal = new Dal.SaleProcess.SpecialRebatesAppDal();
        BillMainBLL bllMainBill = new BillMainBLL();
        public DataTable GetAllListBySql1(T_SpecialRebatesAppmode specimode)
        {
            return specidal.GetAllListBySql1(specimode);
        }

        public DataTable GetAllListBySqlPF(T_SpecialRebatesAppmode specimode)
        {
            return specidal.GetAllListBySqlPF(specimode);
        }
        public Models.T_SpecialRebatesAppmode GetModel(string strBillCode)
        {
            return specidal.GetModel(strBillCode);
        }

        public IList<T_SpecialRebatesAppmode> GetListByBillCode(string strBillCode)
        {
            string sql = "select * from T_SpecialRebatesApp where Code='" + strBillCode + "'";
            return specidal.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model到申请单表
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.T_SpecialRebatesAppmode spemode)
        {
            return specidal.Add(spemode);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode)
        {
            return specidal.Delete(strMainCode);
        }
        /// <summary>
        /// 添加一个申请单
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int AddNote(Models.Bill_Main modelMainBill, Models.T_SpecialRebatesAppmode spemode, out string strMsg)
        {
            try
            {
                strMsg = "";
                if (modelMainBill == null || spemode == null)
                {
                    throw new Exception("主表或出差申请单模型不能为空");
                }
                //bool isSpecial = IsSpecialRebatesTruckCode(spemode.TruckCode);
                //bool iskpsq = IsKpsqTruckCode(spemode.TruckCode);
                //if (!iskpsq)
                //{
                //    if (!isSpecial)
                //    {
                        //主表
                        int iMainRel = bllMainBill.Add(modelMainBill);
                        if (iMainRel <= 0)
                        {
                            throw new Exception("向主表插入数据时发生未知错误！");
                        }
                        //出差申请单
                        string[] strTravelPersionCode = spemode.SysPersionCode.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries);
                        int iPersionLength = strTravelPersionCode.Length;
                        if (iPersionLength <= 0)
                        {
                            throw new Exception("出差申请人个数不能为0！");
                        }
                        //  Delete(spemode.Code);
                        int iRel = this.Add(spemode);

                        return 1;

                //    }
                //    else
                //    {
                //        throw new Exception("该车辆已经申请过特殊返利！");
                //        return -1;
                //    }

                //}
                //else
                //{
                //    throw new Exception("该车辆已经做过开票申请！");
                //    return -1;
                //}
                
              
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 通过单据编号获取车辆code字符串
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string getBillTruckCode(string strBillCode)
        {
            string strSql = "select TruckCode from dbo.T_SpecialRebatesApp where Code='" + strBillCode + "' ";
            DataTable dtRel = DataHelper.GetDataTable(strSql, null, false);
            if (dtRel.Rows.Count > 0)
            {
                StringBuilder sbRel = new StringBuilder();
                for (int i = 0; i < dtRel.Rows.Count; i++)
                {
                    sbRel.Append(dtRel.Rows[i][0].ToString());
                    sbRel.Append(",");
                }
                return sbRel.ToString().Substring(0, sbRel.Length - 1);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取批复状态
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string getBillTruckStatus(string strBillCode)
        {
            string strSql = "select top 1 Status from T_SpecialRebatesStandard  where AppBillCode='" + strBillCode + "' ";
            DataTable dtRel = DataHelper.GetDataTable(strSql, null, false);
            if (dtRel.Rows.Count > 0)
            {
                StringBuilder sbRel = new StringBuilder();
                for (int i = 0; i < dtRel.Rows.Count; i++)
                {
                    sbRel.Append(dtRel.Rows[i][0].ToString());
                    sbRel.Append(",");
                }
                return sbRel.ToString().Substring(0, sbRel.Length - 1);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 添加list
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>

        public bool AddNotelist(IList<T_SpecialRebatesAppmode> mode, out string strMsg)
        {
            try
            {
                strMsg = "";
                if (this.Addlistmode(mode))
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
        /// 添加listmode
        /// </summary>
        /// <param name="listmodel"></param>
        /// <returns></returns>
        public bool Addlistmode(IList<T_SpecialRebatesAppmode> listmodel)
        {
            return specidal.Addlistmodel(listmodel);
        }
        /// <summary>
        /// 判断车辆是否是特殊返利的车辆
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool IsSpecialRebatesTruck(string strTruckCode) {
            string strSql = "select count(*) from T_SpecialRebatesApp a ,bill_main b where a.BillMainCode=b.billCode and b.stepId='end' and a.TruckCode='"+strTruckCode+"'";
            //判断是否是特殊返利申请通过
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 判断该车架号是否申请过特殊返利
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool IsSpecialRebatesTruckCode(string strTruckCode)
        {
            string strSql = "select count(*) from T_SpecialRebatesApp where TruckCode='" + strTruckCode + "'";
            //判断是否是特殊返利申请通过
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 是否做过开票申请
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool IsKpsqTruckCode(string strTruckCode)
        {
            string strSql = "select count(*) from T_BillingApplication  where TruckCode='" + strTruckCode + "'";
            //判断是否是特殊返利申请通过
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
     
    }
}
