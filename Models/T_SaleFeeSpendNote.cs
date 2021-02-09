using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// T_SaleFeeSpendNote
    /// </summary>
    public class T_SaleFeeSpendNote
    {
        /// <summary>
        ///主键
        /// </summary>		
        public int Listnid { get; set; }

        /// <summary>
        ///预算过程Code
        /// </summary>		
        public string YsgcCode { get; set; }

        /// <summary>
        ///单据编号
        /// </summary>		
        public string Billcode { get; set; }

        /// <summary>
        ///部门编号
        /// </summary>		
        public string Deptcode { get; set; }

        /// <summary>
        ///预算科目code
        /// </summary>		
        public string Yskmcode { get; set; }

        /// <summary>
        ///状态
        /// </summary>		
        public string Status { get; set; }

        /// <summary>
        ///费用
        /// </summary>		
        public decimal? Fee { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string Sysdatetime { get; set; }

        /// <summary>
        ///登陆用户Code
        /// </summary>		
        public string Sysusercode { get; set; }

        /// <summary>
        ///备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        ///Note1
        /// </summary>		
        public string Note1 { get; set; }

        /// <summary>
        ///Note2
        /// </summary>		
        public string Note2 { get; set; }

        /// <summary>
        ///Note3
        /// </summary>		
        public string Note3 { get; set; }

        /// <summary>
        ///Note4
        /// </summary>		
        public string Note4 { get; set; }

        /// <summary>
        ///Note5
        /// </summary>		
        public string Note5 { get; set; }

        /// <summary>
        ///Note6
        /// </summary>		
        public string Note6 { get; set; }

        /// <summary>
        ///Note7
        /// </summary>		
        public string Note7 { get; set; }

        /// <summary>
        ///Note8
        /// </summary>		
        public string Note8 { get; set; }

        /// <summary>
        ///Note9
        /// </summary>		
        public string Note9 { get; set; }

        /// <summary>
        ///Note0
        /// </summary>		
        public string Note0 { get; set; }
        /// <summary>
        /// 查询属性 时间起 非数据库字段
        /// </summary>
        public string dateFrm { get; set; }
        /// <summary>
        /// 查询属性 时间止 非数据库字段
        /// </summary>
        public string dateTo { get; set; }
    }
}
