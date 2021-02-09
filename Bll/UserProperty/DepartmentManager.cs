using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal.UserProperty;
using Models;
using System.Data;

namespace Bll.UserProperty
{
    public class DepartmentManager
    {
        DepartmentDal deptDal = new DepartmentDal();
        public Bill_Departments dept;
        public DepartmentManager(Bill_Departments dept)
        {
            this.dept = dept;
        }
        public DepartmentManager(string deptCode)
        {
            dept = deptDal.GetDeptByCode(deptCode);
        }

        /// <summary>
        /// 获得根部门
        /// </summary>
        /// <returns></returns>
        public Bill_Departments GetRoot()
        {
            IList<Bill_Departments> allList = deptDal.GetAllDept();
            Bill_Departments ret = new Bill_Departments();

            FindRoot(allList, dept.DeptCode, ret);


            return ret;
        }


        /// <summary>
        /// 获得子部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllChild()
        {
            IList<Bill_Departments> allList = deptDal.GetAllDept();
            IList<Bill_Departments> retList = new List<Bill_Departments>();
            retList.Add(dept);
            FindChild(allList, dept.DeptCode, ref retList);
            return retList;
        }
        /// <summary>
        /// 不包含父级部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllChild2()
        {
            IList<Bill_Departments> allList = deptDal.GetAllDept();
            IList<Bill_Departments> retList = new List<Bill_Departments>();
            retList.Add(dept);
            FindChild(allList, dept.DeptCode, ref retList);
            return retList;
        }
        /// <summary>
        /// 通过部门编号 查找部门 （不包含下级）
        /// </summary>
        /// <param name="strDeptCode"></param>
        /// <returns></returns>
        public IList<Bill_Departments> GetListWithOutChild(string strDeptCode)
        {
            return new DepartmentDal().GetListWithOutChild(strDeptCode);
        }
        /// <summary>
        /// 将子部门拼接成字符串
        /// </summary>
        /// <returns></returns>
        public string GetAllChildToString()
        {
            IList<Bill_Departments> list = GetAllChild();
            StringBuilder sb = new StringBuilder();
            foreach (Bill_Departments tempdep in list)
            {
                sb.Append(tempdep.DeptCode);
                sb.Append(",");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获得子部门
        /// </summary>
        private void FindChild(IList<Bill_Departments> allList, string deptcode, ref IList<Bill_Departments> retList)
        {
            var linq = from child in allList
                       where child.SjDeptCode == deptcode
                       select child;
            foreach (Bill_Departments tempDep in linq)
            {
                retList.Add(tempDep);
                FindChild(allList, tempDep.DeptCode, ref retList);
            }
        }

        /// <summary>
        /// 递归找到根部门
        /// </summary>
        /// <param name="allList">整个部门集合</param>
        /// <param name="deptcode">部门编号</param>
        private void FindRoot(IList<Bill_Departments> allList, string deptcode, Bill_Departments ret)
        {
            if (deptcode == "000001")
            {
                Bill_Departments rt = (from rootDept in allList
                                       where rootDept.DeptCode == "000001"
                                       select rootDept).First();
                ret.DeptCode = rt.DeptCode;
                ret.DeptName = rt.DeptName;
                ret.DeptStatus = rt.DeptStatus;
                ret.SjDeptCode = rt.SjDeptCode;
                return;
            }
            Bill_Departments linq = (from rootDept in allList
                                     where rootDept.DeptCode == deptcode
                                     select rootDept).First();

            ConfigBLL config = new ConfigBLL();
            string sfysmj = config.GetValueByKey("deptjc");
            if (!string.IsNullOrEmpty(sfysmj) && sfysmj == "Y")
            {
                ret.DeptCode = linq.DeptCode;
                ret.DeptName = linq.DeptName;
                ret.DeptStatus = linq.DeptStatus;
                ret.SjDeptCode = linq.SjDeptCode;
            }
            else
            {
                if (linq.SjDeptCode == "000001")
                {
                    //深拷贝可以用clone
                    ret.DeptCode = linq.DeptCode;
                    ret.DeptName = linq.DeptName;
                    ret.DeptStatus = linq.DeptStatus;
                    ret.SjDeptCode = linq.SjDeptCode;
                }
                else
                {
                    FindRoot(allList, linq.SjDeptCode, ret);
                }
            }


        }


        public override string ToString()
        {
            return "[" + this.dept.DeptCode + "]" + this.dept.DeptName;
        }

        /// <summary>
        /// 获得子部门
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Departments> GetAllChild(int pagefrm, int pageto, out int count)
        {
            IList<Bill_Departments> allList = deptDal.GetAllDept();
            IList<Bill_Departments> retList = new List<Bill_Departments>();
            retList.Add(dept);
            count = 0;
            //获取所有的子部门
            FindChild(allList, dept.DeptCode, ref retList);
            for (int i = 0; i < retList.Count; i++)
            {
                retList[i].rownum = i + 1;
            }
            count = retList.Count;
            var lst = from child in retList where child.rownum > pagefrm && child.rownum < pageto select child;
            List<Bill_Departments> lstdept = new List<Bill_Departments>();
            foreach (Bill_Departments depts in lst)
            {
                lstdept.Add(depts);
            }
            return lstdept;
        }

        /// <summary>
        /// 通过部门编号 查找部门 （不包含下级）
        /// </summary>
        /// <param name="strDeptCode"></param>
        /// <returns></returns>
        public IList<Bill_Departments> GetListWithOutChild(string strDeptCode, int pagefrm, int pageto, out int count)
        {
            return new DepartmentDal().GetListWithOutChild(strDeptCode, pagefrm, pageto, out count);
        }

    }
}
