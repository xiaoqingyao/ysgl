using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 实际内部车型号与编号的对应情况
    /// </summary>
    public class T_TruckTypeCorrespond
    {

        /// <summary>
        ///list_id
        /// </summary>		
        public long list_id { get; set; }

        /// <summary>
        ///车辆类型
        /// </summary>		
        public string truckTypeCode { get; set; }

        /// <summary>
        ///内部车型号
        /// </summary>		
        public string factTruckType { get; set; }

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

        /// <summary>
        /// 查询字段 是否显示所有未对应的内部车型号   非数据库字段
        /// </summary>
        public string IsShowNoCorrespond { get; set; }

    }
}
