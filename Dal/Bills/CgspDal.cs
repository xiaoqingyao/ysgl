using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Dal.Bills
{
    public class CgspDal
    {
        public void DeleteCgsp(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_cgsp where cgbh=@billCode";
            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }

        public void DeleteCgsp(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteCgsp(billCode, tran);
                    DeleteCgspMxb(billCode, tran);
                    DeleteCgspXjb(billCode, tran);
                    DeleteCgspFjb(billCode, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(billCode, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }


        public void DeleteCgspMxb(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_cgsp_mxb where cgbh=@billCode";
            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }

        public void DeleteCgspXjb(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_cgsp_xjb where cgbh=@billCode";
            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }

        public void DeleteCgspFjb(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_cgsp_fjb where billCode=@billCode";
            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }
    }
}
