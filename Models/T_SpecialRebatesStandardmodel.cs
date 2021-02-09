using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    ///T_SpecialRebatesStandard 特殊返利批复
    /// </summary>
  public  class T_SpecialRebatesStandardmodel
    {
        /// <summary>
        ///NID
        /// </summary>		
        public long NID { get; set; }

        /// <summary>
        ///特殊返利申请单号
        /// </summary>		
        public string AppBillCode { get; set; }

        /// <summary>
        ///卡车底盘号
        /// </summary>		
        public string TruckCode { get; set; }

        /// <summary>
        ///车辆类型编号
        /// </summary>		
        public string TruckTypeCode { get; set; }

        /// <summary>
        ///单位code
        /// </summary>		
        public string DeptCode { get; set; }

        /// <summary>
        ///费用类型code
        /// </summary>		
        public string SaleFeeTypeCode { get; set; }

        /// <summary>
        ///销售过程code(废弃)
        /// </summary>		
        public string SaleProcessCode { get; set; }

        /// <summary>
        ///配置项/销售过程code
        /// </summary>		
        public string ControlItemCode { get; set; }

        /// <summary>
        ///提成费用
        /// </summary>		
        public decimal? Fee { get; set; }

        /// <summary>
        ///费用批复人
        /// </summary>		
        public string MarkerCode { get; set; }

        /// <summary>
        ///状态 默认为0  1为确认批复完毕
        /// </summary>		
        public string Status { get; set; }
         /// <summary>
        /// 费用类别 1 销售过程 2 配置项
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///系统用户
        /// </summary>		
        public string SysUserCode { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string SysTime { get; set; }

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
        ///Note10
        /// </summary>		
        public string Note10 { get; set; }
    }
}
