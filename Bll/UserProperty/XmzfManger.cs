using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.Bills;
using Models;
using System.Data;
using System.Data.SqlClient;

namespace Bll.UserProperty
{
    public class XmzfManger
    {
        XmzfdDal xmzfdal = new XmzfdDal();
        public void InsertXmzfdDal(Bill_xmzfd xmzfd,Bill_Main main)
        {
            xmzfdal.InsertXmzfdDal(xmzfd,main);
        }

        /// <summary>
        /// 根据传递的参数组建查询条件查询项目支付报销单
        /// </summary>
        /// <param name="deptcode">部门</param>
        /// <param name="usercode">人员</param>
        /// <param name="xmcode">项目</param>
        /// <param name="kssj">开始时间</param>
        /// <param name="jssj">结束时间</param>
        /// <param name="je">金额</param>
        /// <returns>返回datatable，无条件时参数为“”</returns>
        public DataTable GetXmzfsqd(string billcode,string deptcode, string usercode, string xmcode, string kssj, string jssj, string je)
        {
            return xmzfdal.GetXmzfsqd(billcode,deptcode, usercode, xmcode, kssj, jssj, je);
        }

        public void DeleteXmzfd(string billcode)
        {
            xmzfdal.DeleteXmzfd(billcode);
        }
    }
}
