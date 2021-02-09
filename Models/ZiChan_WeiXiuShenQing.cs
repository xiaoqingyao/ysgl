using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 维修申请
    /// </summary>
    public class ZiChan_WeiXiuShenQing
    {
        /// <summary>
        ///主表code
        /// </summary>		
        public string MainCode { get; set; }

        /// <summary>
        ///资产code
        /// </summary>		
        public string ZiChanCode { get; set; }

        /// <summary>
        ///维修类别 对应数据字典09
        /// </summary>		
        public string WeiXiuTypeCode { get; set; }

        /// <summary>
        ///预计金额
        /// </summary>		
        public decimal? YuJiJinE { get; set; }

        /// <summary>
        ///申请人编号
        /// </summary>		
        public string ShenQingRenCode { get; set; }

        /// <summary>
        ///经办人编号
        /// </summary>		
        public string JingBanRenCode { get; set; }

        /// <summary>
        ///申请说明
        /// </summary>		
        public string ShuoMing { get; set; }

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
