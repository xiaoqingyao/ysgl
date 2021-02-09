using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Zichan;
using Models;

namespace Bll.Zichan
{
    public class ChuZhiDanBll
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        ZiChan_ChuZhiDanDal dal = new ZiChan_ChuZhiDanDal();
        //增加记录
        public bool Add(Bill_Main main, IList<ZiChan_ChuZhiDan> kpsqLists)
        {
            return dal.Add(main, kpsqLists);
        }
        //根据编号获得List
        public IList<ZiChan_ChuZhiDan> GetListByCode(string bh)
        {
            return dal.GetListByCode(bh);
        }
        /// <summary>
        /// 获取model
        /// </summary>
        /// <param name="strcode"></param>
        /// <returns></returns>
        public ZiChan_ChuZhiDan getmodel(string strcode)
        {
            return dal.GetModel(strcode);
        }
        //修改记录
        public bool Edit(Bill_Main main, ZiChan_ChuZhiDan sqmodel)
        {
            return dal.Edit(main, sqmodel);
        }
        /// <summary>
        /// 删除处置单
        /// </summary>
        /// <param name="bh"></param>
        /// <returns></returns>
        public int Delete(string bh)
        {
            return dal.Delete(bh);
        }
    }
}
