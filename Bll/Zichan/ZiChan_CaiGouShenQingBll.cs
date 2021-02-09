using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dal;
using Models;
using Dal.Zichan;

namespace Bll.Zichan
{
    public class ZiChan_CaiGouShenQingBll
    {

        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        CaiGouShenQingDal dal = new CaiGouShenQingDal();
        //增加记录
        public bool Add(Bill_Main main, IList<ZiChan_CaiGouShenQing> kpsqLists)
        {
            return dal.Add(main, kpsqLists);
        }
        //根据编号获得List
        public IList<ZiChan_CaiGouShenQing> GetListByCode(string bh)
        {
            return dal.GetListByCode(bh);
        }
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="strcode"></param>
        /// <returns></returns>
        public ZiChan_CaiGouShenQing getmodel(string strcode) 
        {
            return dal.GetModel(strcode);
        }
        //修改记录
        public bool Edit(Bill_Main main, ZiChan_CaiGouShenQing sqmodel)
        {
            return dal.Edit(main, sqmodel);
        }
        //  删除记录
        public int Delete(Bill_Main main,string bh)
        {
            return dal.Delete(main,bh);
        }
      
    }
}
