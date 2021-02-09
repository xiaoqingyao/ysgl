using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Dal.Bills;
using System.Data.SqlClient;
using Dal;

namespace Bll.Bills
{
    public class bill_lydBll
    {

        bill_lydDal dalMain =new bill_lydDal ();
        bill_lydsDal dalItem = new bill_lydsDal();

        public bool Add(bill_lyd main, IList<bill_lyds> list)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    dalMain.Delete(main.guid, tran);
                    dalItem.Delete(main.guid, tran);
                    dalMain.Add(main, tran);
                    for (int i = 0; i < list.Count; i++)
                        dalItem.Add(list[i], tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        public bool Delete(string guid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    dalMain.Delete(guid, tran);
                    dalItem.Delete(guid, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }
        /// <summary>
        /// 通过编号获取模型
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bill_lyd GetModel(string guid)
        {
            return dalMain.GetModel(guid);
        }

        public IList<bill_lyd> GetList(int beg, int end, List<SqlParameter> paramter, string sqls, out int totalCount)
        {
            return dalMain.GetAllList(beg, end, paramter, sqls, out  totalCount);

        }
        
        
        public IList<bill_lyds> GetItemByMain(string guid)
        {
        return dalItem.GetAllList();
        }
    }
}
