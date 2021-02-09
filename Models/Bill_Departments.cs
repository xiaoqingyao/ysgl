using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Departments
    {
        private string deptCode;

        public string DeptCode
        {
            get { return deptCode; }
            set { deptCode = value; }
        }
        private string deptName;

        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }
        private string sjDeptCode;

        public string SjDeptCode
        {
            get { return sjDeptCode; }
            set { sjDeptCode = value; }
        }
        private string deptStatus;
        /// <summary>
        /// 
        /// </summary>
        public string DeptStatus
        {
            get { return deptStatus; }
            set { deptStatus = value; }
        }
        private string IsSell;
        /// <summary>
        /// 是否是销售公司Y--是 N--否 默认N
        /// </summary>
        public string isSell
        {
            get { return IsSell; }
            set { IsSell = value; }
        }
        /// <summary>
        /// 部门简码 财务部-CW
        /// </summary>
        public string deptJianma { get; set; }

        /// <summary>
        /// 对应U8系统的id
        /// </summary>
        public string forU8id { get; set; }
        
        /// <summary>
        /// 分页用
        /// </summary>
        public int rownum{get;set;}
    }
}
