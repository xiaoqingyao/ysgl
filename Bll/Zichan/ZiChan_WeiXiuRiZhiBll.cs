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
    public class ZiChan_WeiXiuRiZhiBll
    {
        ZiChan_WeiXiuRiZhiDal wxdal = new ZiChan_WeiXiuRiZhiDal();
        ZiChan_WeiXiuRiZhi model = new ZiChan_WeiXiuRiZhi();
        BillMainBLL bllMainBill = new BillMainBLL();
        public DataTable GetAllListBySql1(ZiChan_WeiXiuRiZhi model)
        {
            return wxdal.GetAllListBySql1(model);
        }



        public IList<ZiChan_WeiXiuRiZhi> GetListByBillCode(string strlistid)
        {
            string sql = "select * from ZiChan_WeiXiuRiZhi where listid='" + strlistid + "'";
            return wxdal.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.ZiChan_WeiXiuRiZhi emodeL)
        {
            return wxdal.Add(emodeL);
        }

        public bool Addlistmode(IList<ZiChan_WeiXiuRiZhi> listmodel)
        {
            return wxdal.Addlistmodel(listmodel);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(int strMainCode)
        {
            return wxdal.Delete(strMainCode);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool AddNote(IList<ZiChan_WeiXiuRiZhi> emodel, out string strMsg)
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
        public ZiChan_WeiXiuRiZhi GetModel(int strBillCode)
        {
            return wxdal.GetModel(strBillCode);
        }
       
    }
}
