using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;
using System.Data.SqlClient;


namespace Bll.Bills
{
    /// <summary>
    /// 所有业务表的主表操作类
    /// Edit by Lvcc
    /// </summary>
    public class BillMainBLL
    {
        MainDal dalMain = new MainDal();

        public int Add(Models.Bill_Main modelMainBill)
        {
            int iRel = dalMain.DeleteMain(modelMainBill.BillCode);
            int iMainRel = dalMain.InsertMain(modelMainBill);
            if (iMainRel < 1)
            {
                throw new Exception("未知错误！");
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// 通过编号获取模型
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public Models.Bill_Main GetModel(string strBillCode)
        {
            return dalMain.GetMainByCode(strBillCode);
        }


        public string GetBillcodeByDeptAndYsgc(string gcbh, string deptcode,string flowid)
        {

            return dalMain.GetBillcodeByDeptAndYsgc(gcbh, deptcode, flowid);

        }
        public string GetBillcodeByDeptAndYsgc(string gcbh, string deptcode, string flowid,string xmcode)
        {

            return dalMain.GetBillcodeByDeptAndYsgc(gcbh, deptcode, flowid,xmcode);

        }

        public string GetBillcode(string gcbh, string deptcode, string strgkdept, string strkmbh, string flowid)
        {
            return dalMain.GetBillcode(gcbh, deptcode, strgkdept, strkmbh, flowid);
        }
    }
}
