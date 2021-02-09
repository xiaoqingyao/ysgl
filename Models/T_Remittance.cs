using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 车款上缴明细表
    /// </summary>
    public  class T_Remittance
    {
        /// <summary>
        ///NID
        /// </summary>		
        public string NID { get; set; }

        /// <summary>
        ///汇款单位name
        /// </summary>		
        public string PaymentDeptName { get; set; }

        /// <summary>
        ///汇款单位code
        /// </summary>		
        public string PaymentDeptCode { get; set; }

        /// <summary>
        ///订单号
        /// </summary>		
        public string OrderCode { get; set; }

        /// <summary>
        ///车架号
        /// </summary>		
        public string TruckCode { get; set; }

        /// <summary>
        ///汇款辆数
        /// </summary>		
        public string RemittanceNumber { get; set; }

        /// <summary>
        ///汇款时间
        /// </summary>		
        public string RemittanceDate { get; set; }

        /// <summary>
        ///汇款形式
        /// </summary>		
        public string RemittanceType { get; set; }

        /// <summary>
        ///汇款金额(万元)
        /// </summary>		
        public decimal? RemittanceMoney { get; set; }

        /// <summary>
        ///汇款用途
        /// </summary>		
        public string RemittanceUse { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string SystemDate { get; set; }

        /// <summary>
        ///系统操作人code
        /// </summary>		
        public string SystemuserCode { get; set; }

        /// <summary>
        ///订单日期
        /// </summary>		
        public string OrderCodeDate { get; set; }
         /// <summary>
        ///附件url
        /// </summary>		
        public string Accessories { get; set; }
        
        /// <summary>
        ///NOTE1经销商
        /// </summary>		
        public string NOTE1 { get; set; }

        /// <summary>
        ///NOTE2
        /// </summary>		
        public string NOTE2 { get; set; }

        /// <summary>
        ///NOTE3
        /// </summary>		
        public string NOTE3 { get; set; }

        /// <summary>
        ///NOTE4
        /// </summary>		
        public string NOTE4 { get; set; }

        /// <summary>
        ///NOTE5
        /// </summary>		
        public string NOTE5 { get; set; }

        /// <summary>
        ///NOTE6
        /// </summary>		
        public string NOTE6 { get; set; }

        /// <summary>
        ///NOTE7
        /// </summary>		
        public string NOTE7 { get; set; }

        /// <summary>
        ///NOTE8
        /// </summary>		
        public string NOTE8 { get; set; }

        /// <summary>
        ///NOTE9
        /// </summary>		
        public string NOTE9 { get; set; }

        /// <summary>
        ///NOTE0
        /// </summary>		
        public string NOTE0 { get; set; }
    }
}
