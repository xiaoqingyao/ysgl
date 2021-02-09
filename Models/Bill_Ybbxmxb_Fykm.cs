using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Ybbxmxb_Fykm
    {
        string billCode;

        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        string fykm;

        public string Fykm
        {
            get { return fykm; }
            set { fykm = value; }
        }
        decimal je;

        public decimal Je
        {
            get { return je; }
            set { je = value; }
        }
        decimal se;

        public decimal Se
        {
            get { return se; }
            set { se = value; }
        }
        string mxGuid;

        public string MxGuid
        {
            get { return mxGuid; }
            set { mxGuid = value; }
        }
        string status;

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private IList<Bill_Ybbxmxb_Fykm_Dept> deptList;

        public IList<Bill_Ybbxmxb_Fykm_Dept> DeptList
        {
            get { return deptList; }
            set { deptList = value; }
        }
        private IList<Bill_Ybbxmxb_Hsxm> xmList;

        public IList<Bill_Ybbxmxb_Hsxm> XmList
        {
            get { return xmList; }
            set { xmList = value; }
        }

        public string bxbm;
        /// <summary>
        /// 报销部门  不是数据库字段  程序操作所需 是为归口分解形式的预算报销服务的  edit by lvcc
        /// </summary>
        public string Bxbm { get; set; }

        /// <summary>
        /// 未知字段 大智用于存储单据的剩余金额
        /// </summary>
        public string ms { get; set; }
    }
}
