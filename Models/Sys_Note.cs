using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Sys_Note
    {
        /// <summary>
        ///主键 唯一码
        /// </summary>		
        public string sysCode { get; set; }

        /// <summary>
        ///单据编号（主键）
        /// </summary>		
        public string billcode { get; set; }

        /// <summary>
        ///单据名称（编号）
        /// </summary>		
        public string billname { get; set; }

        /// <summary>
        ///操作人
        /// </summary>		
        public string usercode { get; set; }

        /// <summary>
        ///单据类型    对应bill_datadic的
        /// </summary>		
        public string billtype { get; set; }

        /// <summary>
        ///日志类型（登录/操作单据/改密/删除日志） 对应bill_datadic的某节点下的编号（找到后在此注释一下）
        /// </summary>		
        public string OperationType { get; set; }

        /// <summary>
        ///用户ip地址
        /// </summary>		
        public string userip { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public DateTime? ndate { get; set; }

        /// <summary>
        ///操作描述
        /// </summary>		
        public string opeDiscretion { get; set; }

        /// <summary>
        ///beforestr
        /// </summary>		
        public string beforestr { get; set; }

        /// <summary>
        ///afterstr
        /// </summary>		
        public string afterstr { get; set; }


        /// <summary>
        ///note1
        /// </summary>		
        public string note1 { get; set; }

        /// <summary>
        ///note2
        /// </summary>		
        public string note2 { get; set; }

        /// <summary>
        ///note3
        /// </summary>		
        public string note3 { get; set; }

        /// <summary>
        ///note4
        /// </summary>		
        public string note4 { get; set; }

        /// <summary>
        ///note5
        /// </summary>		
        public string note5 { get; set; }

        /// <summary>
        ///note6
        /// </summary>		
        public string note6 { get; set; }

        /// <summary>
        ///note7
        /// </summary>		
        public string note7 { get; set; }

        /// <summary>
        ///note8
        /// </summary>		
        public string note8 { get; set; }

        /// <summary>
        ///note9
        /// </summary>		
        public string note9 { get; set; }

        /// <summary>
        ///note10
        /// </summary>		
        public string note10 { get; set; }

        /// <summary>
        ///note11
        /// </summary>		
        public string note11 { get; set; }

        /// <summary>
        ///note12
        /// </summary>		
        public string note12 { get; set; }

        /// <summary>
        ///note13
        /// </summary>		
        public string note13 { get; set; }

        /// <summary>
        ///note14
        /// </summary>		
        public string note14 { get; set; }

        /// <summary>
        ///note15
        /// </summary>		
        public string note15 { get; set; }

        /// <summary>
        ///note16
        /// </summary>		
        public string note16 { get; set; }

        /// <summary>
        ///note17
        /// </summary>		
        public string note17 { get; set; }

        /// <summary>
        ///note18
        /// </summary>		
        public string note18 { get; set; }

        /// <summary>
        ///note19
        /// </summary>		
        public string note19 { get; set; }

        /// <summary>
        ///note20
        /// </summary>		
        public string note20 { get; set; }
    }
}
