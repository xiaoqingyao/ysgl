using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Loan;
using Models;
using System.Data;
using System.Data.SqlClient;
using Dal;
using Dal.Bills;

namespace Bll.Bills
{
    public class LoanListBLL
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        T_LoanListDal LoanDal = new T_LoanListDal();
        T_LoanList LoanModel = new T_LoanList();
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable GetAllListBySql1(T_LoanList model, string type)
        {
            return LoanDal.GetAllListBySql1(model, type);
        }
        /// <summary>
        /// 通过编号获取model
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public IList<T_LoanList> GetListByBillCode(string strBillCode)
        {
            string sql = "select * from T_LoanList where Listid='" + strBillCode + "'";
            return LoanDal.ListMaker(sql, null);
        }
        /// <summary>
        /// 得到一个实例
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <returns></returns>
        public T_LoanList GetModel(string strBillCode)
        {
            return LoanDal.GetModel(strBillCode);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int Delete(string strMainCode)
        {
            return LoanDal.Delete(strMainCode);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public int AddModel(T_LoanList model)
        {
            return LoanDal.Add(model);
        }


        public string GetSql(T_LoanList model, string type)
        {
            return LoanDal.GetSql(model, type);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        public bool AddModel(T_LoanList model, Bill_Main main)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    new MainDal().DeleteMain(main.BillCode, tran);
                    new MainDal().InsertMain(main, tran);
                    LoanDal.Delete(model.Listid, tran);
                    LoanDal.Add(model, tran);
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
        /// 删除子项
        /// </summary>
        /// <param name="strMainCode"></param>
        /// <returns></returns>
        public int DeleteI(string id)
        {
            DataTable dt=server.GetDataTable("select * from t_returnnote where listid="+id,null);
            string billcode = dt.Rows[0]["billcode"].ToString();
           // decimal je=Convert.ToDecimal(dt.Rows[0]["je"]);
            if (string.IsNullOrEmpty(id)||string.IsNullOrEmpty(billcode))
            {
                return 0;
            }
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    //删除main
                    new MainDal().DeleteMain(billcode, tran);
                    ////删除 修改主表已还款金额
                    //LoanDal.UpdateYhkje(billcode, je, tran);
                    //删除子表
                    DataHelper.ExcuteNonQuery("delete from t_returnnote where billcode='"+billcode+"'", tran, null, false);
                    //LoanDal.DeleteItem(id, tran);
                    tran.Commit();
                    return 1;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

    }
}
