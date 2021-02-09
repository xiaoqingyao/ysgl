using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Bll
{
    /// <summary>
    /// 归口分解比例
    /// </summary>
    public class bill_gkfjbiliBLL
    {
        sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stryear">年</param>
        /// <param name="strdept">部门</param>
        /// <param name="stryskmcode">预算科目编号</param>
        /// <returns></returns>
        public System.Data.DataTable GetDt(string stryear, string strdept, string stryskmcode)
        {
            string strselectsql = @"select *,@gkdeptcode as gkdeptcode,(select deptname from bill_departments where deptcode=@gkdeptcode) as gkdeptname from (select yskmcode,(select yskmmc from bill_yskm where yskmCode=bill_yskm_dept.yskmcode) as yskmmc,deptcode,(select deptname from bill_departments where deptcode=bill_yskm_dept.deptcode) as deptname  from bill_yskm_dept where yskmcode=@yskmcode) a 
                left join ( select nian,yskmcode,fjdeptcode,fjbl,(select deptname from bill_departments where deptcode=bili.fjdeptcode) as fjdeptname
                 from bill_gkfjbili bili where nian=@nian and gkdeptcode=@gkdeptcode and yskmcode=@yskmcode) b
                 on a.deptcode=b.fjdeptcode";
            SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@nian", stryear), new SqlParameter("@gkdeptcode", strdept), new SqlParameter("@yskmcode", stryskmcode) };
            DataTable dtRel = sqlHelper.GetDataTable(strselectsql, arrsp);
            return dtRel;
        }
    }
}
