using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Bll
{
    public class AddChangyong
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        public int intRowAdd( string strtype,string strdiccode,string strdicname)
        {
            string strsqlexit = "select count(*) from bill_datadic where dictype=@dictype and diccode=@deptcode and dicname=@dicname";
            string strrel = server.GetCellValue(strsqlexit, new SqlParameter[] {new SqlParameter("@dictype",strtype), new SqlParameter("@deptcode", strdiccode), new SqlParameter("@dicname", strdicname) });
            int count = string.IsNullOrEmpty(strrel) ? 0 : int.Parse(strrel);
            if (count > 0)
            {
              
                return 0;
            }
            string strsqladd = "insert into bill_datadic (dicType,dicCode,dicName,cjys,cys,cdj) values(@dictype,@diccode,@dicname,'','','')";
            return server.ExecuteNonQuery(strsqladd, new SqlParameter[] { new SqlParameter("@dictype", strtype), new SqlParameter("@diccode", strdiccode), new SqlParameter("dicname", strdicname) });
        }
    }
    
}
