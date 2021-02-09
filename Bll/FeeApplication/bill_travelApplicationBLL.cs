using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.FeeApplication;
using Bll.Bills;
using Models;
using System.Data;

namespace Bll.FeeApplication
{
    /// <summary>
    /// 出差申请单BLL
    /// Edit by Lvcc
    /// </summary>
    public class bill_travelApplicationBLL
    {
        bill_travelApplicationDal dalTravelApplication = new bill_travelApplicationDal();
        BillMainBLL bllMainBill = new BillMainBLL();
        public Models.Bill_TravelApplication GetModel(string strBillCode)
        {
            return dalTravelApplication.GetModel(strBillCode);
        }
        public IList<Bill_TravelApplication> GetListByBillCode(string strBillCode)
        {
            string sql = "select maincode,MoreThanStandard,typecode,ReportCode,travelPersionCode,arrdess,travelDate,reasion,travelplan,needAmount,Transport,jiaotongfei,zhusufei,yewuzhaodaifei,huiyifei,yinshuafei,qitafei,sendDept from Bill_TravelApplication where maincode='" + strBillCode + "'";
            return dalTravelApplication.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model到申请单表
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.Bill_TravelApplication modelTravelApplication)
        {
            return dalTravelApplication.Add(modelTravelApplication);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode) {
            return dalTravelApplication.Delete(strMainCode);
        }
        /// <summary>
        /// 添加一个申请单
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int AddNote(Models.Bill_Main modelMainBill, Models.Bill_TravelApplication modelTravelApplication, out string strMsg)
        {
            try
            {
                strMsg = "";
                if (modelMainBill==null||modelTravelApplication==null)
	            {
                    throw new Exception("主表或出差申请单模型不能为空");
	            }
                //主表
                int iMainRel = bllMainBill.Add(modelMainBill);
                if (iMainRel<=0)
                {
                    throw new Exception("向主表插入数据时发生未知错误！");
                }
                //出差申请单
                string[] strTravelPersionCode=modelTravelApplication.travelPersionCode.Split(new string[]{"|&|"},StringSplitOptions.RemoveEmptyEntries);
                int iPersionLength=strTravelPersionCode.Length;
                if (iPersionLength <= 0)
                {
                    throw new Exception("出差申请人个数不能为0！");
                }
                //先删除
                Delete(modelTravelApplication.maincode);
                //循环添加
                for (int i = 0; i < iPersionLength; i++)
                {
                    modelTravelApplication.travelPersionCode = strTravelPersionCode[i].ToString();
                    int iRel = this.Add(modelTravelApplication);
                    if (iRel<=0)
                    {
                        throw new Exception("向出差申请单插入数据时发生未知错误！");
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return -1;
            }
        }

        /// <summary>
        /// 通过主表code获取所有出差的人员的编号
        /// </summary>
        /// <returns></returns>
        private string[] GetAllTraUserCodeByMainCode(string mainCode) {
            return dalTravelApplication.GetAllTraUserCodeByMainCode(mainCode);
        }

        /// <summary>
        /// 附加的报销单内是否有该用户
        /// </summary>
        /// <returns></returns>
        public bool userIsInBill(string mainCode,string userCode) {
            string[] arrStrRel = this.GetAllTraUserCodeByMainCode(mainCode);
            int iLength = arrStrRel.Length;
            bool boflg = false;
            for (int i = 0; i < iLength; i++)
            {
                if (arrStrRel[i].Equals(userCode))
                {
                    boflg = true;
                    break;
                }
            }
            return boflg;
        }

        /// <summary>
        /// 附加单据
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <param name="strRepCode"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int AddBill(string strBillCode, string strRepCode, out string strMsg)
        {
            try
            {
                strMsg = "";
                string strSql = "update Bill_TravelApplication set ReportCode='" + strRepCode + "' where maincode='" + strBillCode + "'";
                return new sqlHelper.sqlHelper().ExecuteNonQuery(strSql);
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return -1;
            }
           
        }
        /// <summary>
        /// 根据报告单号获取出差申请单号
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string GetReportCodeByAppCode(string strBillCode)
        {
            try
            {
                string strSql = "select ReportCode from Bill_TravelApplication where maincode='" + strBillCode + "'";
                object objRel = new sqlHelper.sqlHelper().ExecuteScalar(strSql);
                if (objRel == null)
                {
                    return "";
                }
                else
                {
                    return objRel.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 根据出差申请单号获取报告单号
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string GetAppCodeByReportCode(string strBillCode)
        {
            try
            {
                string strSql = "select maincode from Bill_TravelApplication where ReportCode='" + strBillCode + "'";
                object objRel = new sqlHelper.sqlHelper().ExecuteScalar(strSql);
                if (objRel == null)
                {
                    return "";
                }
                else
                {
                    return objRel.ToString();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        /// <summary>
        /// 通过出差单号获取出差所有人员组成的字符串
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string GetPersionStrByTravelAppCode(string strBillCode) {
            try
            {
                string strSql = "select (select '['+userCode+']'+userName from bill_users where userCode=bill_travelApplication.travelPersionCode) as userName from bill_travelApplication where maincode='" + strBillCode + "'";
                DataTable dtRel = new sqlHelper.sqlHelper().GetDataTable(strSql,null);
                StringBuilder sb = new StringBuilder();
                int iRows = dtRel.Rows.Count;
                if (iRows > 0)
                {
                    for (int i = 0; i < iRows; i++)
                    {
                        sb.Append(dtRel.Rows[i][0].ToString());
                        sb.Append(",");
                    }
                    return sb.ToString().Substring(0, sb.Length - 1) ;
                }
                else { return ""; }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
