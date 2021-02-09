using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 项目分解利润表
    /// </summary>
   public  class bill_ys_xmfjlrb
    {
        /// <summary>
        ///procode
        /// </summary>		
        public string procode { get; set; }

        /// <summary>
        ///kmcode
        /// </summary>		
        public string kmcode { get; set; }

        /// <summary>
        ///budgetmoney
        /// </summary>		
        public decimal? budgetmoney { get; set; }

        /// <summary>
        ///annual
        /// </summary>		
        public string annual { get; set; }

        /// <summary>
        ///by1
        /// </summary>		
        public string by1 { get; set; }

        /// <summary>
        ///by2
        /// </summary>		
        public string by2 { get; set; }

        /// <summary>
        ///by3
        /// </summary>		
        public string by3 { get; set; }
    }
}
