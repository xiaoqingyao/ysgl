using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Bll.SaleBill
{
    /// <summary>
    /// 回款类别操作类
    /// </summary>
    public class V_HuikuanLeiBie
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        //V_HuiKuanLeiBie
        public DataSet getall() 
        {
            string sql = "select * from dbo.V_HuiKuanLeiBie ";
            return server.GetDataSet(sql);
        }
    }
}
