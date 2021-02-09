using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class bill_ys_xmfjbm
    {
        /// <summary>
        ///procode
        /// </summary>		
        public string procode { get; set; }

        /// <summary>
        ///deptcode
        /// </summary>		
        public string deptcode { get; set; }

        /// <summary>
        ///kmcode
        /// </summary>		
        public string kmcode { get; set; }

        /// <summary>
        ///je
        /// </summary>		
        public decimal? je { get; set; }

        /// <summary>
        ///by1 建议金额
        /// </summary>		
        public string by1 { get; set; }

        /// <summary>
        ///by2 说明
        /// </summary>		
        public string by2 { get; set; }

        /// <summary>
        ///by3 状态 1--预算确认 2 --部门确认 3--部门异议
        /// </summary>		
        public string by3 { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }
    }
}
