using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.UserProperty
{

    public class DepartmentDal
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        /// <summary>
        /// 获得根部门
        /// </summary>
        /// <returns></returns>
        public Bill_Departments GetRootDept()
        {
            string sql = " select top 1 * from bill_departments where isnull(sjDeptCode,'')='' ";
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, null))
            {
                if (dr.Read())
                {
                    Bill_Departments dept = new Bill_Departments();
                    dept.DeptCode = Convert.ToString(dr["deptCode"]);
                    dept.DeptName = Convert.ToString(dr["DeptName"]);
                    dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                    dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                    dept.isSell = Convert.ToString(dr["IsSell"]);
                    dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                    dept.forU8id = Convert.ToString(dr["forU8id"]);
                    return dept;
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<Bill_Departments> GetAllDept()
        {
            string sql = @" select deptCode,deptName,sjDeptCode,deptStatus,IsSell,deptJianma
                         ,forU8id,Row_Number()over(order by deptCode) as crow 
                         from bill_departments 
                         where deptStatus !='D'  order by deptCode";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);

            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                dept.rownum = Convert.ToInt32(dr["crow"]);
                list.Add(dept);
            }
            return list;
        }


        public IList<Bill_Departments> ListMaker(string tempsql, SqlParameter[] sps)
        {
            DataTable dt = DataHelper.GetDataTable(tempsql, sps, false);
            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;
        }


        /// <summary>
        /// 获取所有二级部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllDeptsed()
        {
            string sql = " select * from bill_departments where sjDeptCode='000001'";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);

            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;
        }

        /// <summary>
        /// 获取所有属于销售公司的二级部门 
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllDeptsedsale()
        {
            string sql = " select * from bill_departments where sjDeptCode='000001'and IsSell='Y' ";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);

            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;
        }
        /// <summary>
        /// 获取汇总明细档案
        /// </summary>
        /// <returns></returns>

        public IList<bill_ys_benefitpro> GetAllfy(string strYear, string strType)
        {
            string sql = "select proname,procode from bill_ys_benefitpro where fillintype=@strType and  left(proCode,4)=@strYear and status='1'";
            SqlParameter[] arrSp = new SqlParameter[] { 
                new SqlParameter("@strType",strType),
                new SqlParameter("@strYear",strYear)
            };
            DataTable dt = DataHelper.GetDataTable(sql, arrSp, false);

            IList<bill_ys_benefitpro> list = new List<bill_ys_benefitpro>();
            foreach (DataRow dr in dt.Rows)
            {
                bill_ys_benefitpro dtda = new bill_ys_benefitpro();
                dtda.proname = Convert.ToString(dr["proname"]);
                list.Add(dtda);
            }
            return list;
        }
        /// <summary>
        /// 通过部门编号 查找部门 （不包含下级）
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<Bill_Departments> GetListWithOutChild(string strCode)
        {
            if (strCode.Equals(""))
            {
                return null;
            }
            string sql = " select * from bill_departments where deptCode='" + strCode + "'";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);
            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;
        }


        /// <summary>
        /// 获得部门
        /// </summary>
        /// <param name="deptCode">部门编号</param>
        /// <returns></returns>
        public Bill_Departments GetDeptByCode(string deptCode)
        {
            string sql = " select * from bill_departments where deptCode=@deptCode ";
            SqlParameter[] sps = { new SqlParameter("@deptCode", deptCode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    Bill_Departments dept = new Bill_Departments();
                    dept.DeptCode = Convert.ToString(dr["deptCode"]);
                    dept.DeptName = Convert.ToString(dr["DeptName"]);
                    dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                    dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                    dept.isSell = Convert.ToString(dr["IsSell"]);
                    dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                    dept.forU8id = Convert.ToString(dr["forU8id"]);
                    return dept;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得下级部门
        /// </summary>
        /// <param name="rootCode">上级部门编号</param>
        /// <returns></returns>
        public IList<Bill_Departments> GetDeptByRoot(string rootCode)
        {
            string sql = "select * from bill_departments where sjDeptCode=@rootCode";
            SqlParameter[] sps = { new SqlParameter("@rootCode", rootCode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;
        }

        /// <summary>
        /// 获取末级部门
        /// </summary>
        /// <param name="rootCode"></param>
        /// <returns></returns>

        public IList<Bill_Departments> GetdeptMj(string rootCode)
        {
            string sql = "select * from bill_departments  where sjdeptcode!='' and sjdeptcode!=@rootCode";
            SqlParameter[] sps = { new SqlParameter("@rootCode", rootCode) };

            DataTable dt = DataHelper.GetDataTable(sql, sps, false);

            IList<Bill_Departments> list = new List<Bill_Departments>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Departments dept = new Bill_Departments();
                dept.DeptCode = Convert.ToString(dr["deptCode"]);
                dept.DeptName = Convert.ToString(dr["DeptName"]);
                dept.DeptStatus = Convert.ToString(dr["DeptStatus"]);
                dept.SjDeptCode = Convert.ToString(dr["SjDeptCode"]);
                dept.isSell = Convert.ToString(dr["IsSell"]);
                dept.deptJianma = Convert.ToString(dr["deptJianma"]);
                dept.forU8id = Convert.ToString(dr["forU8id"]);
                list.Add(dept);
            }
            return list;

        }
        /// <summary>
        /// 根据人员编号删除业务主管
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="tran"></param>
        public void DeleteYwzgByUser(string userCode, SqlTransaction tran)
        {
            string sql = "delete bill_dept_ywzg where userCode=@userCode";
            SqlParameter[] sps = { new SqlParameter("@userCode", userCode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        /// <summary>
        /// 插入业务主管表
        /// </summary>
        /// <param name="deptCode"></param>
        /// <param name="userCode"></param>
        /// <param name="tran"></param>
        public void InsertYwzgByUser(string deptCode, string userCode, SqlTransaction tran)
        {
            string sql = "insert bill_dept_ywzg(deptCode,userCode) values(@deptCode,@userCode)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode", userCode) ,
                                     new SqlParameter("@deptCode", deptCode)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>

        public string GetDeptByUser(string usercode)
        {
            string cxsql = @"select '['+deptcode+']'+deptname from bill_departments where deptcode=
                             (select userdept from bill_users where usercode=@usercode )";
            SqlParameter[] paramter = { new SqlParameter("@usercode", usercode) };
            return DataHelper.ExecuteScalar(cxsql, paramter, false).ToString();
        }
        /// <summary>
        /// 根据登陆用户获取二级部门
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetDeptNameByUser(string usercode)
        {
            string cxsql = "";
            if (isTopDept("y", usercode))
            {
                cxsql = "select '['+deptcode+']'+deptname from bill_departments where deptcode=(select userdept from bill_users where usercode=@usercode)";
            }
            else
            {
                cxsql = "select '['+deptcode+']'+deptname from bill_departments where deptcode =(select sjDeptcode from bill_departments where  deptcode=(select userdept from bill_users where usercode=@usercode))";
            }
            SqlParameter[] paramter = { new SqlParameter("@usercode", usercode) };
            return DataHelper.ExecuteScalar(cxsql, paramter, false).ToString();
        }


        /// <summary>
        /// 查询是不是二级单位
        /// </summary>
        /// <param name="strus">是人员CODE？y:n</param>
        /// <param name="usercode">人员CODE</param>
        /// <returns></returns>
        public bool isTopDept(string strus, string usercode)
        {
            string sql = "";
            if (strus == "y")
            {
                sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
            }
            else
            {
                sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
            }

            if (server.GetCellValue(sql) == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取当前用户名用户编号
        /// </summary>
        /// <param name="strusercode"></param>
        /// <returns></returns>
        public DataTable getUsercodeName(string strusercode)
        {
            string strdeptnamecode = "";
            bool istopdept = isTopDept("y", strusercode);//是否末级部门
            string strsfmj = server.GetCellValue("select avalue from dbo.t_Config where akey='deptjc'");//是否预算到末级
            if (!istopdept && strsfmj.Equals("Y"))
            {
                strdeptnamecode = "select deptcode,deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + strusercode + "')";
            }
            else
            {
                strdeptnamecode = "select deptcode,deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + strusercode + "'))";
            }



            return DataHelper.GetDataTable(strdeptnamecode, null, false);
        }

        /// <summary>
        /// 获取当前用户拥有权限下的管理部门
        /// </summary>
        /// <param name="strdeptCodes"></param>
        /// <param name="strnowdeptcode"></param>
        /// <returns></returns>
        public DataTable getRigtusers(string strdeptCodes, string strnowdeptcode)
        {
            string strsfmj = server.GetCellValue("select avalue from dbo.t_Config where akey='deptjc' ");
            string strsqldtright = "";
            if (!string.IsNullOrEmpty(strsfmj))
            {
                strsqldtright = "select deptCode,deptName from bill_departments where   deptCode in (" + strdeptCodes + ") and deptCode not in ('" + strnowdeptcode + "') and sjDeptCode!=''";

            }
            else
            {
                strsqldtright = "select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strdeptCodes + ") and deptCode not in ('" + strnowdeptcode + "')";

            }
            return DataHelper.GetDataTable(strsqldtright, null, false);

        }

        public DataTable getRigtusers(string strdeptCodes, string strnowdeptcode, string strsfmj, string strnd)
        {
            string strsqldtright = "";
            if (!string.IsNullOrEmpty(strsfmj) && strsfmj == "Y" && (!string.IsNullOrEmpty(strnd)))
            {
                strsqldtright = "select deptCode,deptName from bill_departments where   deptCode in (" + strdeptCodes + ") and deptCode not in (" + strnowdeptcode + ") and deptcode in( select distinct deptcode from bill_ys_xmfjbm where procode='" + strnd + "')";
            }
            else
            {
                strsqldtright = "select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (" + strdeptCodes + ") and deptCode not in (" + strnowdeptcode + ")";
            }

            return DataHelper.GetDataTable(strsqldtright, null, false);

        }

        public bool InsertList(IList<Bill_Departments> depts)
        {
            string insql = " insert into bill_departments(deptCode,deptName,sjDeptCode,deptStatus, IsSell,deptJianma,foru8id) values (@deptCode,@deptName,@sjDeptCode,@deptStatus, @IsSell,@deptJianma,@u8id)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in depts)
                    {
                        SqlParameter[] inparamter = { new SqlParameter("@deptCode",SqlNull(i.DeptCode)),
                                                          new SqlParameter("@deptName",SqlNull(i.DeptName)),
                                                          new SqlParameter("@sjDeptCode",SqlNull(i.SjDeptCode)),
                                                          new SqlParameter("@deptStatus",SqlNull(i.DeptStatus)),
                                                          new SqlParameter("@IsSell",SqlNull(i.isSell)),
                                                           new SqlParameter("@deptJianma",SqlNull(i.deptJianma)),
                                                            new SqlParameter ("@u8id",SqlNull(i.forU8id))};
                        DataHelper.ExcuteNonQuery(insql, inparamter, false);
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

        public bool Exists(string deptCode)
        {
            string strSql = "select count(*) from bill_departments where deptCode='" + deptCode + "' ";
            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(strSql, null, false));
            if (cont > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }






        public IList<Bill_Departments> GetAllDept(int pagefrm, int pageto, out int count)
        {
            string sql = "select deptCode,deptName,sjDeptCode,deptStatus,IsSell,deptJianma,forU8id,Row_Number()over(order by deptCode) as crow from bill_departments where deptStatus !='D' ";
            string strsqlcount = "select count(*) from bill_departments where deptStatus !='D'";

            count = int.Parse(server.GetCellValue(strsqlcount));

            string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} order  by t.deptCode asc";
            strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
            return ListMaker(strsqlframe, null);
        }



        /// <summary>
        /// 通过部门编号 查找部门 （不包含下级）
        /// </summary>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public IList<Bill_Departments> GetListWithOutChild(string strCode, int pagefrm, int pageto, out int count)
        {
            if (strCode.Equals(""))
            {
                count = 0;
                return null;
            }
            string sql = " select deptCode,deptName,sjDeptCode,deptStatus,IsSell,deptJianma,forU8id,Row_Number()over(order by deptCode) as crow from bill_departments where deptCode='" + strCode + "'";
            string strsqlcount = " select count(*) from bill_departments where deptCode='" + strCode + "'";
            count = int.Parse(server.GetCellValue(strsqlcount));

            string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
            strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
            return ListMaker(strsqlframe, null);
        }
    }
}
