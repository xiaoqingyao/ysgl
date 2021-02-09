using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Dal.UserProperty;
using System.Data.SqlClient;
using Dal;
using System.Configuration;
using Dal.SysDictionary;

namespace Bll.UserProperty
{
    public class UserMessage
    {
        Bill_Users users;

        private DepartmentDal depDal = new DepartmentDal();
        private UsersDal userDal = new UsersDal();
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public UserMessage(string code)
        {
            users = userDal.GetUserByCode(code);
        }
        public UserMessage(Bill_Users tempuser)
        {
            users = tempuser;
        }
        //人员信息
        public Bill_Users Users
        {
            get { return users; }
            set { users = value; }
        }
        //[人员编号]人员名称
        public string GetName()
        {
            return "[" + users.UserCode + "]" + users.UserName;
        }

        //是否系统管理员
        public string GetIsSystem()
        {
            return "[" + users.IsSystem + "]" + users.IsSystem;
        }

        //取得人员部门
        public Bill_Departments GetDept()
        {
            return depDal.GetDeptByCode(users.UserDept);
        }

       


        //取得最上级部门
        public Bill_Departments GetRootDept()
        {
            DepartmentManager deptMgr = new DepartmentManager(users.UserDept);
            return deptMgr.GetRoot();
        }

        /// <summary>
        /// 修改人员密码
        /// </summary>
        /// <param name="pwd"></param>
        public void EditPwd(string pwd)
        {
            string sqlcon = GetConStr();
            users.UserPwd = pwd;
            using (SqlConnection conn = new SqlConnection(sqlcon))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    userDal.DeleteUser(users.UserCode, trans);
                    userDal.InserUser(users, trans);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        public void DeleteUser()
        {
            using (SqlConnection conn = new SqlConnection(GetConStr()))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    userDal.DeleteUser(users.UserCode, trans);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        public bool CheckUserCode()
        {
            return userDal.CheckUser(users.UserCode);
        }
        /// <summary>
        /// 插入人员信息，根据人员信息修改业务主管信息
        /// </summary>
        public void InserUser()
        {
            string stroderpws = "";
            string sqlcon = GetConStr();
            using (SqlConnection conn = new SqlConnection(sqlcon))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    if (users.UserPwd != null && users.UserPwd != "")
                    {
                        stroderpws = users.UserPwd;
                    }
                    userDal.DeleteUser(users.UserCode, trans);
                    depDal.DeleteYwzgByUser(users.UserCode, trans);
                    string deptcode = "";
                    if (users.UserGroup == "02")
                    {
                        //部门经理
                        deptcode = this.GetRootDept().DeptCode;
                    }
                    else if (users.UserGroup == "03")
                    {
                        //业务主管
                        deptcode = users.UserDept;
                    }
                    if (!string.IsNullOrEmpty(deptcode))
                    {
                        depDal.InsertYwzgByUser(deptcode, users.UserCode, trans);
                    }

                    users.UserPwd = stroderpws;
                    userDal.InserUser(users, trans);
                    trans.Commit();
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }
        }

        private void ForeachDept(Bill_Departments dept, IList<Bill_Departments> rootList, ref Bill_Departments ret)
        {
            int cont = (from rootDep in rootList
                        where rootDep.DeptCode == dept.DeptCode
                        select rootDep).Count();
            if (cont < 1)
            {
                dept = depDal.GetDeptByCode(dept.SjDeptCode);
                ForeachDept(dept, rootList, ref ret);
            }
            else
            {
                ret = dept;
            }

        }

        private string GetConStr()
        {
            return ConfigurationManager.AppSettings["ConnectionStringvUnionDataBase"].ToString().Trim();
        }
        /// <summary>
        /// 取得菜单
        /// </summary>
        /// <returns></returns>
        public IList<Bill_SysMenu> GetMenu()
        {
            MenuDal menu = new MenuDal();
            IList<Bill_SysMenu> userList = menu.GetMenuByUser(users.UserCode);
            IList<Bill_SysMenu> roleList = menu.GetMenuByRole(users.UserGroup);

            foreach (Bill_SysMenu roleMenu in roleList)
            {
                var temp = from linqtemp in userList
                           where linqtemp.MenuId == roleMenu.MenuId
                           select linqtemp;
                if (temp.Count() < 1)
                {
                    userList.Add(roleMenu);
                }
            }

            return userList;
        }
    }
}
