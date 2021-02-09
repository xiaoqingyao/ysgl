using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.SaleProcess;
using Models;
using System.Data;
using Bll.Bills;
using Dal;

namespace Bll.Sepecial
{
    public class RemittanceBll
    {
        RemittanceDal remitdal = new RemittanceDal();
        T_Remittance remitmodel = new T_Remittance();
        BillMainBLL bllMainBill = new BillMainBLL();
        public DataTable GetAllListBySql1(T_Remittance model)
        {
            return remitdal.GetAllListBySql1(model);
        }



        public IList<T_Remittance> GetListByBillCode(string strBillCode)
        {
            string sql = "select * from T_Remittance where NID='" + strBillCode + "'";
            return remitdal.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model到申请单表
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.T_Remittance emodeL)
        {
            return remitdal.Add(emodeL);
        }

        public bool Addlistmode(IList<T_Remittance> listmodel)
        {
            return remitdal.Addlistmodel(listmodel);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode)
        {
            return remitdal.Delete(strMainCode);
        }
        /// <summary>
        /// 添加一个申请单
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool AddNote(IList<T_Remittance> Rmitemode, out string strMsg)
        {
            try
            {
                strMsg = "";
                //if (modelMainBill == null || Rmitemode == null)
                //{
                //    throw new Exception("主表或出差申请单模型不能为空");
                //}
                ////主表
                //int iMainRel = bllMainBill.Add(modelMainBill);
                //if (iMainRel <= 0)
                //{
                //    throw new Exception("向主表插入数据时发生未知错误！");
                //}
                //车款上缴明细单
                //string[] strTravelPersionCode = Rmitemode.SystemuserCode.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries);
                //int iPersionLength = strTravelPersionCode.Length;
                //if (iPersionLength <= 0)
                //{
                //    throw new Exception("出差申请人个数不能为0！");
                //}
                //  Delete(spemode.Code);
                if (this.Addlistmode(Rmitemode))
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
        public T_Remittance GetModel(string strBillCode)
        {
            return remitdal.GetModel(strBillCode);
        }
        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="strTrckCode"></param>
        /// <returns></returns>

        public bool isExist(string strTrckCode)
        {
            string sql = "select count(*) from T_Remittance where TruckCode='" + strTrckCode + "'";
            object objRel = DataHelper.ExecuteScalar(sql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }

        /// <summary>
        /// 通过单据编号获取车辆code字符串
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string getBillTruckCode(string strBillCode)
        {
            string strSql = "select TruckCode from T_Remittance  where NID='" + strBillCode + "' ";
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
        /// 获取订单号
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string getBillOrderCode(string strBillCode)
        {
            string strSql = "select OrderCode from T_Remittance where NID='" + strBillCode + "'";
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
    }
}
