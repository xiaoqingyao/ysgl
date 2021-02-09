using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
   public class bill_yskm_dept
    {
        /// <summary>
        ///deptCode
        /// </summary>		
        public string deptCode { get; set; }

        /// <summary>
        ///yskmCode
        /// </summary>		
        public string yskmCode { get; set; }

        /// <summary>
        ///cwkmCode
        /// </summary>		
        public string cwkmCode { get; set; }

        /// <summary>
        ///jfkmcode1
        /// </summary>		
        public string jfkmcode1 { get; set; }

        /// <summary>
        ///dfkmcode1
        /// </summary>		
        public string dfkmcode1 { get; set; }

        /// <summary>
        ///jfkmcode2
        /// </summary>		
        public string jfkmcode2 { get; set; }

        /// <summary>
        ///dfkmcode2
        /// </summary>		
        public string dfkmcode2 { get; set; }

        /// <summary>
        ///科目对应type   1==》部门预算科目财务对应  其它为部门预算科目的对应
        /// </summary>		
        public string kmdytype { get; set; }
    }
}
