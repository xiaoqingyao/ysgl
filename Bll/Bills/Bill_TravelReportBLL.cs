using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;

namespace Bll.Bills
{
    /// <summary>
    /// 出差报告bll
    /// edit by Lvcc
    /// </summary>
    public class Bill_TravelReportBLL
    {
        bill_travelReportDal dalTravelReport = new bill_travelReportDal();
        BillMainBLL bllMainBill = new BillMainBLL();
        public Models.Bill_TravelReport GetModel(string strBillCode)
        {
            return dalTravelReport.GetModel(strBillCode);
        }
        /// <summary>
        /// 添加/修改
        /// </summary>
        /// <param name="modelBillMain"></param>
        /// <param name="modelTravelReport"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int Insert(Models.Bill_Main modelBillMain, Models.Bill_TravelReport modelTravelReport, out string strMsg)
        {
            try
            {
                strMsg = "";
                if (modelBillMain==null||modelTravelReport==null)
                {
                    throw new Exception("主表或出差报告单模型不能为空");
                }
                //主表
                int iMainRel = bllMainBill.Add(modelBillMain);
                if (iMainRel<=0)
                {
                    throw new Exception("向主表插入数据时发生未知错误！");
                }
                //出差申请单
                int iTravelRepRel = dalTravelReport.Add(modelTravelReport);
                if (iTravelRepRel<=0)
                {
                    throw new Exception("向出差报告单插入数据时发生未知错误！");
                }
                return 1;
            }
            catch (Exception e)
            {
                strMsg = e.Message;
                return -1;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode)
        {
            return dalTravelReport.Delete(strMainCode);
        }
    }
}
