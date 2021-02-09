using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Dal.Bills
{
    public class LscgDal
    {
        public void DeleteLscg(string billCode, SqlTransaction tran)
        {
            string delMx = "delete bill_lscg where cgbh=@billCode";
            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
        }

        public void DeleteLscg(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteLscg(billCode, tran);
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
    }
}
