using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Models;


namespace Dal.Bills
{
    public class SysconfigDal
    {
        public  IDictionary<string, string> GetsysConfig()
        {
            string sql = "select * from bill_SysConfig where nd='2012'";
            //SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataTable dt = DataHelper.GetDataTable(sql, null, false);

            IDictionary<string, string> retDic = new Dictionary<string, string>();       
            foreach (DataRow dr in dt.Rows)
            {
                retDic.Add(Convert.ToString(dr["ConfigName"]), Convert.ToString(dr["ConfigValue"]));
            }

            return retDic;
        }

        public IDictionary<string, string> GetsysConfigBynd(string nd)
        {
            string sql = "select * from bill_SysConfig where nd='"+nd+"'";
            //SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataTable dt = DataHelper.GetDataTable(sql, null, false);
            if (dt!=null&&dt.Rows.Count>0)
            {
                IDictionary<string, string> retDic = new Dictionary<string, string>();
                foreach (DataRow dr in dt.Rows)
                {
                    retDic.Add(Convert.ToString(dr["ConfigName"]), Convert.ToString(dr["ConfigValue"]));
                }

                return retDic;
            }
            else
            {
                return null;
            }
          
        }
    }
}


