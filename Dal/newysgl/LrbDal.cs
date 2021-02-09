using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Dal.newysgl
{
   public  class LrbDal
    {
        public bool InsertJe(IList<Models.bill_ys_benefitpro> LrbList)
        {
            string Upsql = "update bill_ys_benefitpro set je=@je where procode=@procode and annual=@annual ";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in LrbList)
                    {
                        SqlParameter[] paramter = { new SqlParameter("@je",i.je),
                                                    new SqlParameter("@procode",i.procode), 
                                                    new SqlParameter("@annual",i.annual)};
                        DataHelper.ExcuteNonQuery(Upsql,tran,paramter,false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    return false;
                    throw;
                }
            }
        }
    }
}
