using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class T_SpecialRebatesAppmode
    {
        /// <summary>
        ///申请单号
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        ///BillMain表的code（外键）
        /// </summary>		
        public string BillMainCode { get; set; }

      

        /// <summary>
        ///车架号
        /// </summary>		
        public string TruckCode { get; set; }

        /// <summary>
        ///辆数
        /// </summary>		
        public int? TruckCount { get; set; }

     

        /// <summary>
        ///销售公司code
        /// </summary>		
        public string SaleDeptCode { get; set; }

        /// <summary>
        ///标准(正常)返利
        /// </summary>		
        public decimal? StandardSaleAmount { get; set; }

        /// <summary>
        ///超出实际销售点数
        /// </summary>		
        public decimal? ExceedStandardPoint { get; set; }

        /// <summary>
        ///申请日期
        /// </summary>		
        public string AppDate { get; set; }

        /// <summary>
        ///系统登录用户code
        /// </summary>		
        public string SysPersionCode { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string SysDateTime { get; set; }

        /// <summary>
        ///有效期始
        /// </summary>		
        public string EffectiveDateFrm { get; set; }

        /// <summary>
        ///有效期末
        /// </summary>		
        public string EffectiveDateTo { get; set; }

        /// <summary>
        ///附件 列表页查询时用于传递单据状态
        /// </summary>		
        public string Attachment { get; set; }

        /// <summary>
        ///申请说明
        /// </summary>		
        public string Explain { get; set; }

        /// <summary>
        ///审批附件
        /// </summary>		
        public string CheckAttachment { get; set; }

        /// <summary>
        ///Note1--订单号
        /// </summary>		
        public string Note1 { get; set; }

        /// <summary>
        ///Note2--申请返利原因
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
        ///Note10
        /// </summary>		
        public string Note10 { get; set; }
    }
}
