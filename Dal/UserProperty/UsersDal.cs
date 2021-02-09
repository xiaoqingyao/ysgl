using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace Dal.UserProperty
{
    public class UsersDal
    {
        /// <summary>
        /// 获得人员
        /// </summary>
        /// <param name="code">人员编号</param>
        /// <returns></returns>
        public Bill_Users GetUserByCode(string code)
        {
            string sql = " select * from bill_users where userCode=@userCode ";
            SqlParameter[] sps = { new SqlParameter("@userCode", code) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    Bill_Users users = new Bill_Users();
                    users.IsSystem = Convert.ToString(dr["IsSystem"]);
                    users.UserCode = Convert.ToString(dr["UserCode"]);
                    users.UserDept = Convert.ToString(dr["UserDept"]);
                    users.UserGroup = Convert.ToString(dr["UserGroup"]);
                    users.UserName = Convert.ToString(dr["UserName"]);
                    users.UserPwd = Convert.ToString(dr["UserPwd"]);
                    users.UserStatus = Convert.ToString(dr["UserStatus"]);
                    users.UserPosition = Convert.ToString(dr["userPosition"]);
                    
                    return users;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得所有的人员
        /// </summary>
        /// <returns></returns>
        public IList<Bill_Users> GetAllUser()
        {
            string sql = "select * from bill_users ";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);
            IList<Bill_Users> list = new List<Bill_Users>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Users users = new Bill_Users();
                users.IsSystem = Convert.ToString(dr["IsSystem"]);
                users.UserCode = Convert.ToString(dr["UserCode"]);
                users.UserDept = Convert.ToString(dr["UserDept"]);
                users.UserGroup = Convert.ToString(dr["UserGroup"]);
                users.UserName = Convert.ToString(dr["UserName"]);
                users.UserPwd = Convert.ToString(dr["UserPwd"]);
                users.UserStatus = Convert.ToString(dr["UserStatus"]);
                list.Add(users);
            }
            return list;
        }


        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="userCode"></param>
        /// <param name="trans"></param>
        public void DeleteUser(string userCode, SqlTransaction trans)
        {
            string sql = @"delete bill_users where userCode=@userCode";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",userCode)
                                 };
            DataHelper.ExcuteNonQuery(sql, trans, sps, false);
        }
        /// <summary>
        /// 插入人员
        /// </summary>
        /// <param name="user"></param>
        /// <param name="trans"></param>
        public void InserUser(Bill_Users user, SqlTransaction trans)
        {
            string sql = @"insert into bill_users(userCode, userName, userGroup, userStatus, userDept, userPwd, isSystem,userPosition)values
                           ( @userCode, @userName, @userGroup, @userStatus, @userDept, @userPwd, @isSystem,@userPosition)";
            SqlParameter[] sps = {
                                     new SqlParameter("@userCode",user.UserCode),
                                     new SqlParameter("@userName",user.UserName),
                                     new SqlParameter("@userGroup",user.UserGroup),
                                     new SqlParameter("@userStatus",user.UserStatus),
                                     new SqlParameter("@userDept",user.UserDept),
                                     new SqlParameter("@userPwd",user.UserPwd),
                                     new SqlParameter("@isSystem",user.IsSystem),
                                     new SqlParameter("@userPosition",user.UserPosition)
                                 };
            DataHelper.ExcuteNonQuery(sql, trans, sps, false);
        }
        

        
        /// <summary>
        /// 根据角色编号获得角色名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetUserGroup(string code)
        {
            string sql = " select * from bill_usergroup where groupid=@groupid ";
            SqlParameter[] sps = { new SqlParameter("@groupid", code) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {

                    return Convert.ToString(dr["groupName"]);
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 检测编号是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool CheckUser(string code)
        {
            string sql = " select count(*) from bill_users where usercode=@usercode ";
            SqlParameter[] sps = { new SqlParameter("@usercode", code) };
            int cont = Convert.ToInt32(DataHelper.ExecuteScalar(sql,sps,false));
            if(cont>0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool InsertList(IList<Bill_Users>  users)
        {
            string insql = " insert into bill_users(userCode,userName,userGroup,userStatus, userDept,userPwd,isSystem,userPosition) values (@userCode,@userName,@userGroup,@userStatus, @userDept,@userPwd,@isSystem,@userPosition)";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in users)
                    {
                        SqlParameter[] inparamter = { new SqlParameter("@userCode",SqlNull(i.UserCode)),
                                                          new SqlParameter("@userName",SqlNull(i.UserName)),
                                                          new SqlParameter("@userGroup",SqlNull(i.UserGroup)),
                                                          new SqlParameter("@userStatus",SqlNull(i.UserStatus)),
                                                          new SqlParameter("@userDept",SqlNull(i.UserDept)),
                                                           new SqlParameter("@userPwd",SqlNull(i.UserPwd)),
                                                            new SqlParameter ("@isSystem",SqlNull(i.IsSystem)),
                                                            new SqlParameter("@userPosition",SqlNull(i.UserPosition))};
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

        public bool Exists(string userCode)
        {
            string strSql = "select count(*) from bill_users where userCode='" + userCode + "' ";
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
    }
}
