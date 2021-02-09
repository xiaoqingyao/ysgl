using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 出差申请Model
    /// Edit by Lvcc
    /// </summary>
    public class Bill_TravelApplication
    {
        /// <summary>
        ///bill_main主表code
        /// </summary>		
        public string maincode { get; set; }

        /// <summary>
        ///出差类型code
        /// </summary>		
        public string typecode { get; set; }

        /// <summary>
        ///出差人
        /// </summary>		
        public string travelPersionCode { get; set; }

        /// <summary>
        ///出差地址
        /// </summary>		
        public string arrdess { get; set; }

        /// <summary>
        ///出差日期
        /// </summary>		
        public string travelDate { get; set; }

        /// <summary>
        ///出差事由
        /// </summary>		
        public string reasion { get; set; }

        /// <summary>
        ///出差安排及日程计划
        /// </summary>		
        public string travelplan { get; set; }

        /// <summary>
        ///预计费用
        /// </summary>		
        public int? needAmount { get; set; }

        /// <summary>
        ///预计交通工具
        /// </summary>		
        public string Transport { get; set; }

        /// <summary>
        /// 派遣单位
        /// </summary>
        public string sendDept { get; set; }

        /// <summary>
        ///是否超出规定的标准 1是 0 否
        /// </summary>		
        public int? MoreThanStandard { get; set; }

        /// <summary>
        /// 报告单号
        /// </summary>
        public string ReportCode { get; set; }

        /// <summary>
        /// 交通费
        /// </summary>
        public int jiaotongfei { get; set; }

        /// <summary>
        /// 住宿费
        /// </summary>
        public int zhusufei { get; set; }

        /// <summary>
        /// 业务招待费
        /// </summary>
        public int yewuzhaodaifei { get; set; }

        /// <summary>
        /// 会议费
        /// </summary>
        public int huiyifei { get; set; }

        /// <summary>
        /// 印刷费
        /// </summary>
        public int yinshuafei { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        public int qitafei { get; set; }
    }
}
