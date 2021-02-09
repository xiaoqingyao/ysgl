using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;

namespace Bll.UserProperty
{
    public class BudgetManager
    {
        YsglDal ysgldal =  new YsglDal();
        /// <summary>
        /// 第一个金额是年预算金额，第二个是根据过程编号取得的季度或(月度,年度)总金额
        /// </summary>
        /// <param name="gcbh">过程编号</param>
        /// <param name="deptcode">部门编号</param>
        /// <param name="kmcode">科目编号</param>
        /// <returns></returns>
        public decimal[] GetYsMaxJe(string gcbh,string deptcode,string kmcode) 
        {
            IDictionary<string, string> dic = new SysconfigDal().GetsysConfigBynd(gcbh.Substring(0,4));

            Bill_Ysgc ysgc = ysgldal.GetYsgcByCode(gcbh);

            decimal[] arr = new decimal[2];

            if (ysgc.YsType == "0")//年预算
            {
                arr[0] = -1;
                arr[1] = ysgldal.GetYsje("0", ysgc.Nian , deptcode, kmcode);
            }
            else//季度预算,月度预算
            {
                if (dic["YearBudget"] == "0")
                {
                    arr[0] = -1;
                }
                else
                {
                    arr[0] = ysgldal.GetYsje("0", ysgc.Nian, deptcode, kmcode);
                }
                arr[1] = ysgldal.GetYsje(ysgc.YsType, ysgc.Nian, deptcode, kmcode);
            }
            return arr;
        }
    }
}
