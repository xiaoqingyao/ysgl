using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll.Zichan
{
    /// <summary>
    /// 资产维修申请
    /// Edit by lvcc
    /// </summary>
    public class ZiChan_WeiXiuShenQingBLL
    {
        Dal.Zichan.ZiChan_WeiXiuShenQingDAL dalZiChan_WeiXiuShenQing = new Dal.Zichan.ZiChan_WeiXiuShenQingDAL();
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="lstmodelWeiXiuShenQing"></param>
        /// <param name="strmsg"></param>
        /// <returns></returns>
        public int Add(List<Models.ZiChan_WeiXiuShenQing> lstmodelWeiXiuShenQing, out string strmsg)
        {
            strmsg = "";
            if (lstmodelWeiXiuShenQing.Count == 0)
            {
                strmsg = "添加明细不能为空！";
                return -1;
            }
            //添加到主表
            Models.Bill_Main modelBillMain = new Models.Bill_Main();
            modelBillMain.BillCode = lstmodelWeiXiuShenQing[0].MainCode;
            modelBillMain.BillDate = DateTime.Parse(lstmodelWeiXiuShenQing[0].Note1);
            modelBillMain.BillDept = lstmodelWeiXiuShenQing[0].Note2;
            modelBillMain.BillJe = lstmodelWeiXiuShenQing.Sum(p => Convert.ToInt32(p.YuJiJinE));
            modelBillMain.BillName = lstmodelWeiXiuShenQing[0].MainCode;
            modelBillMain.BillName2 = "";
            modelBillMain.BillType = "";
            modelBillMain.BillUser = lstmodelWeiXiuShenQing[0].ShenQingRenCode;
            modelBillMain.FlowId = "wxsq";
            modelBillMain.GkDept = "";
            modelBillMain.IsGk = "0";
            modelBillMain.LoopTimes = 0;
            modelBillMain.StepId = "-1";
            try
            {
                if (new Bll.Bills.BillMainBLL().Add(modelBillMain) <= 0)
                {
                    throw new Exception("未知错误！");
                }
            }
            catch (Exception ex)
            {
                strmsg = "添加主表时失败，原因："+ex.Message;
                return -1;
            }
            //添加明细表
            
            dalZiChan_WeiXiuShenQing.Deletezb(lstmodelWeiXiuShenQing[0].MainCode);
            for (int i = 0; i < lstmodelWeiXiuShenQing.Count; i++)
            {
                try
                {
                    int iRel = dalZiChan_WeiXiuShenQing.Add(lstmodelWeiXiuShenQing[i]);
                    if (iRel <= 0)
                    {
                        throw new Exception("未知错误！");
                    }
                }
                catch (Exception ex)
                {
                    strmsg = "添加子表时失败，原因：" + ex.Message;
                    return -1; ;
                }
            }
            return 1;
        }

        /// <summary>
        /// 通过一个编号 获取多条记录
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<Models.ZiChan_WeiXiuShenQing> GetListModel(string strCode)
        {
             return dalZiChan_WeiXiuShenQing.GetListModel(strCode);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="billCode"></param>
        public void Delete(string billCode)
        {
            dalZiChan_WeiXiuShenQing.Delete(billCode);
        }
    }
}
