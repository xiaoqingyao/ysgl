using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using Dal;
using Bll.SaleBill;
using System.Data;

namespace Bll.Bills
{
   
    public class T_BillingApplicationBll
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        Dal.Bills.T_BillingApplicationDal dal = new Dal.Bills.T_BillingApplicationDal();
        //增加记录
        public bool Add(Bill_Main main, IList<T_BillingApplication> kpsqLists)
        {
           
          
            return dal.Add(main,kpsqLists);
        }
        //根据编号获得List
        public IList<T_BillingApplication> GetListByCode(string bh)
        {
            return dal.GetListByCode(bh);
        }
        //修改记录
        public bool Edit(Bill_Main main, IList<T_BillingApplication> kpsqLists)
        {
            return dal.Edit(main,kpsqLists);
        }
        //  删除记录
        public int Delete(string bh)
        {
            return dal.Delete(bh);
        }
        /// <summary>
        /// 根据车架号和订单号进行删除
        /// </summary>
        /// <param name="bh"></param>
        /// <returns></returns>
        public int DeletebyTrckCode(string strCarCode,string strOrderCode)
        {
            return dal.DeletebyCarCode(strCarCode, strOrderCode);
        }

        /// <summary>
        ///   //判断该车辆是否做过开票申请
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool isExist(string strTruckCode)
        {
            string strSql = "select count(*) from T_BillingApplication  where TruckCode='" + strTruckCode + "'";
          
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 开票是否制定过标准返利
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool isExistrebatekp(string strTruckCode,string strdatetime,string strdeptcode,string strxsgc)
        {
            string strSql = "select count(*) from T_RebatesStandard where  TruckTypeCode='" + strTruckCode.Trim() + "'and EffectiveDateTo>='" + strdatetime + "'and EffectiveDateFrm<='" + strdatetime + "'and ControlItemCode='" + strxsgc + "' and DeptCode='" + strdeptcode + "' and Status='2'";
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }

        /// <summary>
        /// 特殊返利是否制定过标准返利
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool isExistrebate(string strTruckCode, string strdatetime, string strdeptcode)
        {
            string strSql = "select count(*) from T_RebatesStandard where  TruckTypeCode='" + strTruckCode.Trim() + "'and EffectiveDateTo>='" + strdatetime + "'and EffectiveDateFrm<='" + strdatetime + "'and ControlItemCode!='Qcfp' and DeptCode='" + strdeptcode + "' and Status='2'";
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
        /// <summary>
        /// 判断该车辆类型的标准返利是否已经批复
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool ispf(string strTruckCode)
        {
            bool flg = true;
            string strsql = @"select Status from T_SpecialRebatesStandard where TruckCode ='" + strTruckCode + "'";
            DataTable dt = server.RunQueryCmdToTable(strsql);
            if (dt.Rows.Count!=0 && dt!=null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Status"].ToString()=="1")
                    {
                        flg= true;

                    }
                    else
                    {
                        flg = false;
                        break;
                    }
                    
                }

            }
            else
            {
                   flg= false;
            }

            return flg;
        
        
        }
        /// <summary>
        /// 判断是否做过特殊返利申请
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool isSpecialRebatesApp(string strTruckCode,out string strCode) 
        {
            strCode = "";
            string strSql = "select Code from T_SpecialRebatesApp where TruckCode='" + strTruckCode + "'";
            object objRel = DataHelper.ExecuteScalar(strSql);
            strCode = objRel == null ? "" : objRel.ToString();
            return strCode == "" ? false : true;
        }


        /// <summary>
        /// 判断是否已对付
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool isDF(string strTruckCode, out string strdfFlg)
        {
            strdfFlg = "";
            string strSql = "select dfFlg from V_TruckMsg where cjh='" + strTruckCode + "'";
            object objRel = DataHelper.ExecuteScalar(strSql);
            if (objRel==null)
            {
                return false;
            }
            strdfFlg = objRel.ToString() == "1" ? "已对付" : "未对付";
            return strdfFlg == "未对付" ? false : true;
        }
        /// <summary>
        /// 通过车架号获取模型
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public T_BillingApplication getModelByTruckCode(string strTruckCode)
        {
            return dal.GetModelByTruckCode(strTruckCode);
        }
        /// <summary>
        /// 通过单据编号获取车辆code字符串
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public string GetTruckStrByBillCode(string strBillCode)
        {
            string strSql = "select TruckCode from T_BillingApplication  where Code='" + strBillCode + "' ";
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
