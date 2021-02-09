using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bll.SaleBill
{
    /// <summary>
    /// 回款明细BLL
    /// </summary>
    public class V_HuiKuanMingXiBLL
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public DataTable GetAllHuiKuanNote(string strAppendSql)
        {
            string strSql = "select * from V_HuiKuanMingXi where 1=1";
            if (!string.IsNullOrEmpty(strAppendSql))
            {
                strSql += strAppendSql;
            }
            return server.RunQueryCmdToTable(strSql);
        }
    }
}
