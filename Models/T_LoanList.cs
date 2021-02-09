using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 namespace Models
{
     public class T_LoanList
    {
        /// <summary>
        ///主键
        /// </summary>		
        public string Listid { get; set; }

        /// <summary>
        ///经办人Code
        /// </summary>		
        public string LoanCode { get; set; }

        /// <summary>
        ///借款人单位code
        /// </summary>		
        public string LoanDeptCode { get; set; }

        /// <summary>
        ///借款日期
        /// </summary>		
        public string LoanDate { get; set; }

        /// <summary>
        ///借款系统时间
        /// </summary>		
        public string LoanSystime { get; set; }

        /// <summary>
        ///借款金额
        /// </summary>		
        public decimal? LoanMoney { get; set; }

        /// <summary>
        ///借款说明/借款事由
        /// </summary>		
        public string LoanExplain { get; set; }

        /// <summary>
        ///状态（1：借款 3：冲减中 2：结算完毕）
        /// </summary>		
        public string Status { get; set; }

        /// <summary>
        ///结算方式（0 ：现金 1： 单据冲减）
        /// </summary>		
        public string SettleType { get; set; }

        /// <summary>
        ///冲减单号
        /// </summary>		
        public string CJCode { get; set; }

        /// <summary>
        ///借款人Code
        /// </summary>		
        public string ResponsibleCode { get; set; }

        /// <summary>
        ///经办日期
        /// </summary>		
        public string ResponsibleDate { get; set; }

        /// <summary>
        ///经办系统时间
        /// </summary>		
        public string ResponsibleSysTime { get; set; }

        /// <summary>
        ///NOTE1 冲减经办人Code
        /// </summary>		
        public string NOTE1 { get; set; }

        /// <summary>
        ///NOTE2借款科目Code
        /// </summary>		
        public string NOTE2 { get; set; }

        /// <summary>
        ///NOTE3 已冲减金额
        /// </summary>		
        public string NOTE3 { get; set; }

        /// <summary>
        ///NOTE4 借款天数
        /// </summary>		
        public string NOTE4 { get; set; }

        /// <summary>
        ///NOTE5 备注
        /// </summary>		
        public string NOTE5 { get; set; }

        /// <summary>
        ///NOTE6  借款类型 对应数据字典20
        /// </summary>		
        public string NOTE6 { get; set; }

        /// <summary>
        ///NOTE7 附加单据
        /// </summary>		
        public string NOTE7 { get; set; }

        /// <summary>
        ///NOTE8 附件名称
        /// </summary>		
        public string NOTE8 { get; set; }

        /// <summary>
        ///NOTE9 附件url
        /// </summary>		
        public string NOTE9 { get; set; }

        /// <summary>
        ///NOTE10
        /// </summary>		
        public string NOTE10 { get; set; }

        /// <summary>
        ///NOTE11
        /// </summary>		
        public string NOTE11 { get; set; }

        /// <summary>
        ///NOTE12
        /// </summary>		
        public string NOTE12 { get; set; }

        /// <summary>
        ///NOTE13
        /// </summary>		
        public string NOTE13 { get; set; }

        /// <summary>
        ///NOTE14
        /// </summary>		
        public string NOTE14 { get; set; }

        /// <summary>
        ///NOTE15
        /// </summary>		
        public string NOTE15 { get; set; }

        /// <summary>
        ///NOTE16
        /// </summary>		
        public string NOTE16 { get; set; }

        /// <summary>
        ///NOTE17
        /// </summary>		
        public string NOTE17 { get; set; }

        /// <summary>
        ///NOTE18
        /// </summary>		
        public string NOTE18 { get; set; }

        /// <summary>
        ///NOTE19
        /// </summary>		
        public string NOTE19 { get; set; }

        /// <summary>
        ///NOTE20
        /// </summary>		
        public string NOTE20 { get; set; }


        /// <summary>
        ///查询的时候记录查询条件超期状态：全部/超期/临期
        /// </summary>		
        public string NOTE21 { get; set; }

        /// <summary>
        ///查询的时候记录超期天数
        /// </summary>		
        public string NOTE22 { get; set; }

    }
}
