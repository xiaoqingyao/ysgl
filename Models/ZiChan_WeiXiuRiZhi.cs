using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ZiChan_WeiXiuRiZhi
    {
        /// <summary>
        ///listid
        /// </summary>		
        public int listid { get; set; }

        /// <summary>
        ///资产code
        /// </summary>		
        public string ZiChanCode { get; set; }

        /// <summary>
        ///维修人
        /// </summary>		
        public string WeiXiuRenCode { get; set; }

        /// <summary>
        ///维修部门
        /// </summary>		
        public string WeiXiuBuMenCode { get; set; }

        /// <summary>
        ///维修金额
        /// </summary>		
        public decimal? WeiXiuJinE { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string XiTongShiJian { get; set; }

        /// <summary>
        ///是否提交过审批
        /// </summary>		
        public string ShiFouShenPi { get; set; }

        /// <summary>
        ///审批单号(如果提交过审批则该单号必须存在)
        /// </summary>		
        public string ShenPiDanCode { get; set; }

        /// <summary>
        ///维修类别 对应数据字典09
        /// </summary>		
        public string WeiXiuTypeCode { get; set; }

        /// <summary>
        ///备注
        /// </summary>		
        public string BeiZhu { get; set; }

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
