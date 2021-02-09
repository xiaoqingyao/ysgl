using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.SaleBill;
using Models;
using Dal;
using Bll.Bills;
using System.Data.SqlClient;

namespace Bll.SaleBill
{
    /// <summary>
    /// 返利记录表
    /// edit by lvcc
    /// </summary>
    public class T_SaleFeeAllocationNoteBLL
    {
        T_SaleFeeAllocationNoteDal DalT_SaleFeeAllocationNote = new T_SaleFeeAllocationNoteDal();
        public int Add(T_SaleFeeAllocationNote model)
        {
            return DalT_SaleFeeAllocationNote.Add(model);
        }

        public int Del(T_SaleFeeAllocationNote model) 
        {
            return DalT_SaleFeeAllocationNote.Del(model);
        
        }
        /// <summary>
        /// 驳回开票
        /// </summary>
        /// <param name="lstTruckCode"></param>
        /// <param name="strErrMsg"></param>
        /// <returns></returns>
        public int DoRevertRebate(List<T_BillingApplication> lstTruck, string strcurrentUser, out string strErrMsg)
        {
            strErrMsg = "";
            string strTruckCodes = "";
            string strBillCodes = "";
            int iTruckCount=lstTruck.Count;
            if (iTruckCount <= 0)
            {
                strErrMsg = "请选择一条记录";
                return -1;
            }
            for (int i = 0; i < iTruckCount; i++)
            {
                strTruckCodes += lstTruck[i].TruckCode + ",";
                strBillCodes += lstTruck[i].Code + ",";
            }
            strTruckCodes = strTruckCodes.Substring(0, strTruckCodes.Length - 1);
            strBillCodes = strBillCodes.Substring(0, strBillCodes.Length - 1);

            string strActionNote = DateTime.Now.ToString()+":"+strcurrentUser+":驳回开票，同时禁用所有的记录";
            string strsql = "update T_SaleFeeAllocationNote set Status='D',ActionNote='" + strActionNote + "' where TruckCode in (" + strTruckCodes + ") and BillCode in(" + strBillCodes + ");update T_BillingApplication set Note4=getdate() where TruckCode in (" + strTruckCodes + ") and Code in(" + strBillCodes + ")";//还得修改开票申请
            return DataHelper.ExcuteNonQuery(strsql, null, false);
        }
        public int DoRebate(List<string> lstTruckCode, out string strErrMsg)
        {
            strErrMsg = "";
            List<T_SaleFeeAllocationNote> lstTruck = new List<T_SaleFeeAllocationNote>();//合法的，可以进一步生成返利明细的信息 用返利明细的model来记录
            List<string> lstErrorTruckCodeList = new List<string>();//特殊返利没有批复的车辆

            int iListCount = lstTruckCode.Count;
            if (iListCount == 0)
            {
                strErrMsg = "开票车辆记录不能为空！";
                return -1;
            }
            for (int i = 0; i < iListCount; i++)
            {
                string strTruckCode = lstTruckCode[i];
                T_SaleFeeAllocationNote TruckMsg = new T_SaleFeeAllocationNote();
                T_BillingApplication modelBillApp = new T_BillingApplicationBll().getModelByTruckCode(strTruckCode);
                TruckMsg.BillCode = modelBillApp.Code;
                TruckMsg.TruckCode = strTruckCode;
                TruckMsg.DeptCode = modelBillApp.SaleDeptCode;
                TruckMsg.ActionDate = DateTime.Now.ToString("yyyy-MM-dd") ;
                //判断车辆是否是特殊返利
                bool isSpecialRebate = new SpecialRebatesAppBLL().IsSpecialRebatesTruck(strTruckCode);
                if (isSpecialRebate)
                {
                    //特殊返利
                    if (!this.CheckSpecialTruckHasReplay(strTruckCode))
                    {
                        lstErrorTruckCodeList.Add(strTruckCode);
                    }
                    else
                    {
                        lstTruck.Add(TruckMsg);
                    }
                }
                else
                {
                    //一般返利
                    lstTruck.Add(TruckMsg);
                }
            }
            if (lstErrorTruckCodeList.Count > 0)
            {
                string strCodeList = "";
                foreach (var str in lstErrorTruckCodeList)
                {
                    strCodeList += str;
                    strCodeList += ",";
                }
                strErrMsg = strCodeList + "等车辆所在的特殊返利申请未批复";
                return -1;
            }
            else
            {
                if (this.MakeRebateNotes(lstTruck, out strErrMsg) < 1)
                {
                    throw new Exception(strErrMsg);
                }
            }
            return 1;
        }
        ///// <summary>
        ///// 生成返利记录
        ///// </summary>
        ///// <param name="strCode">开票申请单等返利过程点的单据</param>
        ///// <returns></returns>
        //public int DoRebate(string strBillCode, out string strMsg)
        //{
        //    strMsg = "";
        //    List<string> lstErrorTruckCodeList = new List<string>();//特殊返利没有批复的车辆
        //    string strCodeType = strBillCode.Substring(0, 4);
        //    //合法的，可以进一步生成返利明细的信息 用返利明细的model来记录
        //    List<T_SaleFeeAllocationNote> lstTruck = new List<T_SaleFeeAllocationNote>();
        //    if (strCodeType.Equals("kpsq"))//开票申请
        //    {
        //        IList<T_BillingApplication> lstBillingApp = new T_BillingApplicationBll().GetListByCode(strBillCode);
        //        int iCount = lstBillingApp.Count;
        //        for (int i = 0; i < iCount; i++)
        //        {
        //            string strTruckCode = lstBillingApp[i].TruckCode;
        //            T_SaleFeeAllocationNote TruckMsg = new T_SaleFeeAllocationNote();
        //            TruckMsg.BillCode = lstBillingApp[i].Code;
        //            TruckMsg.TruckCode = strTruckCode;
        //            TruckMsg.TruckTypeCode = new ViewBLL().GetTruckTypeByCode(strTruckCode);
        //            TruckMsg.DeptCode = lstBillingApp[i].SaleDeptCode;
        //            TruckMsg.ActionDate = lstBillingApp[i].BillingDate;
        //            //判断车辆是否是特殊返利
        //            bool isSpecialRebate = new SpecialRebatesAppBLL().IsSpecialRebatesTruck(strTruckCode);
        //            if (isSpecialRebate)
        //            {
        //                //特殊返利
        //                if (!this.CheckSpecialTruckHasReplay(strTruckCode))
        //                {
        //                    lstErrorTruckCodeList.Add(strTruckCode);
        //                }
        //                else
        //                {
        //                    lstTruck.Add(TruckMsg);
        //                }
        //            }
        //            else {
        //                //一般返利
        //                lstTruck.Add(TruckMsg);
        //            }
        //        }
        //        if (lstErrorTruckCodeList.Count > 0)
        //        {
        //            string strCodeList = "";
        //            foreach (var str in lstErrorTruckCodeList)
        //            {
        //                strCodeList += str;
        //                strCodeList += ",";
        //            }
        //            strMsg=strCodeList + "等车辆所在的特殊返利申请未批复";
        //            return -1;
        //        }
        //        else {
        //            if (this.MakeRebateNotes(lstTruck, out strMsg)<1) {
        //                throw new Exception(strMsg);
        //            }
        //        }
        //    }
        //    return 1;
        //}
        /// <summary>
        /// 检查特殊返利车辆是否已批复
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        private bool CheckSpecialTruckHasReplay(string strTruckCode)
        {
            string strSql = "select count(*) from T_SpecialRebatesStandard where status='1' and TruckCode='" + strTruckCode + "'";
            object objRel = DataHelper.ExecuteScalar(strSql);
            string strRel = objRel == null ? "0" : objRel.ToString();
            return strRel.Equals("0") ? false : true;
        }
       
        /// <summary>
        /// 生成返利明细
        /// </summary>
        /// <param name="lstTruck"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private int MakeRebateNotes(List<T_SaleFeeAllocationNote> lstTruck, out string strMsg)
        {
            int iCount = lstTruck.Count;
            strMsg = "";
            try
            {
                int iRel = 0;
                for (int i = 0; i < iCount; i++)
                {
                    string sql = " exec SaleBill_MakeRebateNotes '" + lstTruck[i].BillCode + "','" + lstTruck[i].TruckCode + "','" + lstTruck[i].DeptCode + "','" + lstTruck[i].ActionDate + "'";
                    iRel = DataHelper.ExcuteNonQuery(sql, null, false);
                    if (iRel < 1)
                    {
                        throw new Exception("生成费用明细时失败！");
                    }
                }
                return iRel;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 获取返利金额
        /// </summary>
        /// <returns></returns>
        public decimal getAllocationAmount(string deptCode, string kmCode)
        {
            string strSql = "select isnull(sum(fee),0) from dbo.T_SaleFeeAllocationNote where status='1' and DeptCode=@deptCode and SaleFeeTypeCode=@saleFeeTypeCode";
            SqlParameter[] arrPara ={
                                   new SqlParameter("deptCode",deptCode),
                                   new SqlParameter("saleFeeTypeCode",kmCode)
                                   };
            object objRel = DataHelper.ExecuteScalar(strSql,arrPara,false);
            return objRel == null ? 0 : decimal.Parse(objRel.ToString());
        }
    }
    ///// <summary>
    ///// 车辆明细
    ///// </summary>
    //public class TruckDetail
    //{
    //    private string truckCode;
    //    /// <summary>
    //    /// 车架号
    //    /// </summary>
    //    public string TruckCode
    //    {
    //        get { return truckCode; }
    //        set { truckCode = value; }
    //    }
    //    private string billCode;
    //    /// <summary>
    //    /// 单据号
    //    /// </summary>
    //    public string BillCode
    //    {
    //        get { return billCode; }
    //        set { billCode = value; }
    //    }
    //    private bool rebateType;
    //    /// <summary>
    //    /// 是否特殊返利
    //    /// </summary>
    //    public bool RebateType
    //    {
    //        get { return rebateType; }
    //        set { rebateType = value; }
    //    }
    //    private string saleDeptCode;
    //    /// <summary>
    //    /// 销售公司code
    //    /// </summary>
    //    public string SaleDeptCode
    //    {
    //        get { return saleDeptCode; }
    //        set { saleDeptCode = value; }
    //    }

    //    public TruckDetail(string strTruckCode, string strBillCode, bool strRebateType, string strSaleDeptCode)
    //    {
    //        this.TruckCode = strTruckCode;
    //        this.BillCode = strBillCode;
    //        this.RebateType = strRebateType;
    //        this.saleDeptCode = strSaleDeptCode;
    //    }
    //}
}
