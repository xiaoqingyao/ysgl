using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    //项目部门年度表
  public  class bill_xm_dept_nd
    {
        /// <summary>
        ///项目编号
        /// </summary>		
        public string xmCode { get; set; }
      /// <summary>
        ///项目编号
        /// </summary>		
        public string XmName { get; set; }
        /// <summary>
        ///项目部门  其中'000001'为公司项目 其他为部门下的项目
        /// </summary>		
        public string xmDept { get; set; }
        /// <summary>
        ///项目部门  其中'000001'为公司项目 其他为部门下的项目
        /// </summary>		
        public string SjXm { get; set; }
        /// <summary>
        ///预算金额
        /// </summary>		
        public decimal? je { get; set; }

        /// <summary>
        ///是否开启控制
        /// </summary>		
        public string isCtrl { get; set; }

        /// <summary>
        ///年度
        /// </summary>		
        public string nd { get; set; }

        /// <summary>
        ///状态
        /// </summary>		
        public string status { get; set; }

        /// <summary>
        ///note0
        /// </summary>		
        public string note0 { get; set; }

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
    }
}
