using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 用户表 bill_users
    /// </summary>
    public class Bill_Users
    {
        private string userCode;

        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// 职务
        /// </summary>
        public string UserPosition { get; set; }
        private string userGroup;

        public string UserGroup
        {
            get { return userGroup; }
            set { userGroup = value; }
        }
        private string userStatus;
        /// <summary>
        /// 用户状态  1 启用 0 停用
        /// </summary>
        public string UserStatus
        {
            get { return userStatus; }
            set { userStatus = value; }
        }
        private string userDept;

        public string UserDept
        {
            get { return userDept; }
            set { userDept = value; }
        }
        private string userPwd;

        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }
        private string isSystem;
        /// <summary>
        /// 是否系统用户
        /// </summary>
        public string IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }
    }
}
