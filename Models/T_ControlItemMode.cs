using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// T_ControlItem
    /// </summary>
    public class T_ControlItemMode
    {
        /// <summary>
        ///配置项编号
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        ///配置项名称
        /// </summary>		
        public string CName { get; set; }
        /// <summary>
        ///状态 1=启用 0 禁用
        /// </summary>		
        public string Status { get; set; }

        /// <summary>
        ///控制点1code
        /// </summary>		
        public string ControlCodeFirst { get; set; }

        /// <summary>
        ///控制点1name
        /// </summary>		
        public string ControlNameFirst { get; set; }

        /// <summary>
        ///控制点2code
        /// </summary>		
        public string ControlCodeSecond { get; set; }

        /// <summary>
        ///控制点2Name
        /// </summary>		
        public string ControlNameSecond { get; set; }

        /// <summary>
        ///超出月份数
        /// </summary>		
        public string Months { get; set; }

        /// <summary>
        ///备注
        /// </summary>		
        public string Remark { get; set; }

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

    }
}
