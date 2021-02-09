using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 卡车类别表
    /// Edit by Lvcc
    /// </summary>
    public class T_truckType
    {
        public T_truckType() { }
        /// <summary>
        ///类型编号
        /// </summary>		
        public string typeCode { get; set; }

        /// <summary>
        ///类型名称
        /// </summary>		
        public string typeName { get; set; }

        /// <summary>
        ///父节点code
        /// </summary>		
        public string parentCode { get; set; }

        /// <summary>
        ///状态 1=启用 0=禁用
        /// </summary>		
        public string status { get; set; }

        /// <summary>
        ///是否是最末级别 1=是 0=否
        /// </summary>		
        public string IsLastNode { get; set; }

        /// <summary>
        /// 实际销售价每高于标准价格点数（单点）
        /// </summary>
        public float HigherPerPoint { get; set; }

        /// <summary>
        /// 销售返利对应奖励的返利点数
        /// </summary>
        public float RebatePoint { get; set; }
        /// <summary>
        /// 销售返利对应扣除的返利点数（单点）
        /// </summary>
        public float DeductionPoint { get; set; }
        /// <summary>
        ///NOTE1
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


    }
}
