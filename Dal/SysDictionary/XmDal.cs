using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.SysDictionary
{
    public class XmDal
    {
        /// <summary>
        /// 根据部门编号查找项目
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <returns></returns>
        public IList<Bill_Xm> GetXmByDep(string deptCode)
        {
            string sql = " select * from bill_xm where (xmdept=@xmdept) or (xmdept=(select top 1 deptcode from dbo.bill_departments where isnull(sjdeptcode,'')='' and deptstatus='1')) ";
            SqlParameter[] sps = { new SqlParameter("@xmdept", deptCode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<Bill_Xm> list = new List<Bill_Xm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Xm dic = new Bill_Xm();
                dic.SjXm = Convert.ToString(dr["SjXm"]);
                dic.XmCode = Convert.ToString(dr["XmCode"]);
                dic.XmDept = Convert.ToString(dr["XmDept"]);
                dic.XmName = Convert.ToString(dr["XmName"]);
                dic.XmStatus = Convert.ToString(dr["XmStatus"]);
                list.Add(dic);
            }
            return list;
        }

        /// <summary>
        /// 根据部门编号和年度查找项目
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <returns></returns>
        public IList<bill_xm_dept_nd> GetXmByDepNd(string deptCode, string nd)
        {
            string sql = " select a.xmCode,a.xmDept,a.je,a.isCtrl,a.nd,a.status,(select sjXm from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as sjXm ,(select xmName from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as XmName from bill_xm_dept_nd  a where a.status='1'and   a.nd=@nd and  (a.xmDept=@xmdept) union select a.xmCode,a.xmDept,a.je,a.isCtrl,a.nd,a.status,(select sjXm from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as sjXm ,(select xmName from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as XmName from bill_xm_dept_nd  a where  a.status='1'  and    a.nd=@nd  and a.xmDept=(select top 1 deptCode from bill_departments where deptCode=@xmdept and isnull(deptStatus,'1')!='0') ";
            SqlParameter[] sps = { new SqlParameter("@nd", nd),
                                   new SqlParameter("@xmdept", deptCode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<bill_xm_dept_nd> list = new List<bill_xm_dept_nd>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_xm_dept_nd dic = new bill_xm_dept_nd();
                dic.xmCode = Convert.ToString(dr["xmCode"]);
                dic.xmDept = Convert.ToString(dr["xmDept"]);
                dic.nd = Convert.ToString(dr["nd"]);
                dic.je = Convert.ToDecimal(dr["je"]);
                dic.isCtrl = Convert.ToString(dr["isCtrl"]);
                dic.SjXm = Convert.ToString(dr["SjXm"]);
                dic.XmName = Convert.ToString(dr["XmName"]);
                dic.status = Convert.ToString(dr["status"]);
                list.Add(dic);
            }
            return list;
        }
        /// <summary>
        /// 根据部门编号，得到所有末级项目
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public IList<Bill_Xm> GetmjXmByDep(string deptCode)
        {
            string sql = " select * from bill_xm where xmdept=@xmdept and xmcode not in (select distinct sjxm from bill_xm  where xmdept=@xmdept ) ";
            SqlParameter[] sps = { new SqlParameter("@xmdept", deptCode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<Bill_Xm> list = new List<Bill_Xm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Xm dic = new Bill_Xm();
                dic.SjXm = Convert.ToString(dr["SjXm"]);
                dic.XmCode = Convert.ToString(dr["XmCode"]);
                dic.XmDept = Convert.ToString(dr["XmDept"]);
                dic.XmName = Convert.ToString(dr["XmName"]);
                dic.XmStatus = Convert.ToString(dr["XmStatus"]);
                list.Add(dic);
            }
            return list;
        }
        /// <summary>
        /// 根据项目编号得到项目信息
        /// </summary>
        /// <param name="xmCode"></param>
        /// <returns></returns>
        public Bill_Xm GetXmByCode(string xmCode)
        {
            string sql = " select * from bill_xm where xmCode=@xmCode ";
            SqlParameter[] sps = { new SqlParameter("@xmCode", xmCode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    Bill_Xm dic = new Bill_Xm();
                    dic.SjXm = Convert.ToString(dr["SjXm"]);
                    dic.XmCode = Convert.ToString(dr["XmCode"]);
                    dic.XmDept = Convert.ToString(dr["XmDept"]);
                    dic.XmName = Convert.ToString(dr["XmName"]);
                    dic.XmStatus = Convert.ToString(dr["XmStatus"]);
                    return dic;
                }
                else
                {
                    return null;
                }
            }

        }

        
    }
}
