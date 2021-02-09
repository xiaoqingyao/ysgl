using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Zichan;
using System.Data;
using Models;
using Dal;

namespace Bll.Zichan
{
    public class ZiChan_ShiYongZhuangKuangBll
    {
        ZiChan_LeibieDal zichanleibiedal = new ZiChan_LeibieDal();
        ZiChan_ShiYongZhuangKuangDal syzkDal = new ZiChan_ShiYongZhuangKuangDal();
        public DataSet GetAll()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(syzkDal.ReturnAllDt());
            return ds;
        }

        public DataTable GetAlldt()
        {
            DataTable dt = new DataTable();
            dt = syzkDal.ReturnAllDt();
            return dt;
        }
        public ZiChan_ShiYongZhuangKuang GetModel(string strCode)
        {
            return syzkDal.GetModel(strCode);
        }
        /// <summary>
        /// 判断某记录是否有子节点
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool HasChildren(string p)
        {
            IList<ZiChan_ShiYongZhuangKuang> ilist = syzkDal.GetAllChildren(p);
            if (ilist.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Add(ZiChan_ShiYongZhuangKuang model, out string strMsg)
        {
            try
            {
                strMsg = "";
                return syzkDal.Add(model);
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return 0;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="modelTruckType"></param>
        /// <returns></returns>
        public int Upd(ZiChan_ShiYongZhuangKuang model, out string strmsg)
        {
            try
            {
                strmsg = "";
                return syzkDal.updatemodel(model);
            }
            catch (Exception ex)
            {
                strmsg = ex.Message;
                return 0;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public int Delete(string strCode, out string msg)
        {
            try
            {
                //先获取model以操作上级节点
                ZiChan_ShiYongZhuangKuang model = this.GetModel(strCode);
                msg = "";



                ////判断父节点的子节点数量 如果为零 则可以删除

                if (model != null)
                {
                    int childrenCount = this.syzkDal.GetAllChildren(model.ZhuangKuangCode).Count;//获取该类别下子节点数
                    if (childrenCount > 0)//有子节点 不允许删除
                    {

                        msg = "该节点下有子节点，不允许删除！";
                        throw new Exception(msg);

                    }
                    else//没有子节点 删除
                    {
                        int iRel = syzkDal.Delete(strCode);
                        if (iRel < 1)
                        {
                            msg = "删除资产类型时失败！";
                            throw new Exception(msg);
                        }
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
      
        /// <summary>
        /// 判断该编号已经存在
        /// </summary>
        /// <param name="strTruckCode"></param>
        /// <returns></returns>
        public bool IsTruckCode(string strCode)
        {
            string strSql = "select count(*) from ZiChan_ShiYongZhuangKuang where ZhuangKuangCode='" + strCode + "'";
            //判断是否是特殊返利申请通过
            object objRel = DataHelper.ExecuteScalar(strSql);
            string iRel = objRel == null ? "0" : objRel.ToString();
            return iRel == "0" ? false : true;
        }
    }
}
