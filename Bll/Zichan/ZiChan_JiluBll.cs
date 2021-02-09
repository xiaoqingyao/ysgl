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
    public class ZiChan_JiluBll
    {
        ZiChan_JiluDal jldal = new ZiChan_JiluDal();
        ZiChan_Jilu jlmodel = new ZiChan_Jilu();
        BillMainBLL bllMainBill = new BillMainBLL();
        public DataTable GetAllListBySql1(ZiChan_Jilu model)
        {
            return jldal.GetAllListBySql1(model);
        }



        public IList<ZiChan_Jilu> GetListByBillCode(string strCode)
        {
            string sql = "select * from ZiChan_Jilu where ZiChanCode='" + strCode + "'";
            return jldal.ListMaker(sql, null);
        }
        /// <summary>
        /// 添加model
        /// </summary>
        /// <param name="modelTravelApplication"></param>
        /// <returns></returns>
        public int Add(Models.ZiChan_Jilu emodeL)
        {
            return jldal.Add(emodeL);
        }

        public bool Addlistmode(IList<ZiChan_Jilu> listmodel)
        {
            return jldal.Addlistmodel(listmodel);
        }
      
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="modelMainBill">主表</param>
        /// <param name="modelTravelApplication">记录申请表之外的信息</param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool AddNote(IList<ZiChan_Jilu> emodel, out string strMsg)
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
        public ZiChan_Jilu GetModel(string strBillCode)
        {
            return jldal.GetModel(strBillCode);
        }


        public int Delete(string strCode, out string msg)
        {
            try
            {
                //先获取model以操作上级节点
                ZiChan_Jilu model = this.GetModel(strCode);
                msg = "";



                ////判断父节点的子节点数量 如果为零 则可以删除

                if (model != null)
                {

                    int iRel = jldal.Delete(strCode);
                        if (iRel < 1)
                        {
                            msg = "删除资产类型时失败！";
                            throw new Exception(msg);
                        }
                   
                    return 1;
                }
                else
                {
                    return -1;
                }


            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return -1;
            }
        }
    }
}

