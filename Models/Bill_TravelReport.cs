using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_TravelReport
    {
        /// <summary>
        ///主表code
        /// </summary>		
        public string MainCode { get; set; }

        /// <summary>
        ///出差过程/or Title
        /// </summary>		
        public string TravelProcess { get; set; }

        /// <summary>
        ///工作过程/ or 内容
        /// </summary>		
        public string WorkProcess { get; set; }

        /// <summary>
        ///结果
        /// </summary>		
        public string Result { get; set; }

        /// <summary>
        ///报告类别
        /// </summary>		
        public string Note1 { get; set; }

        /// <summary>
        ///所在部门负责人
        /// </summary>		
        public string Note2 { get; set; }

        /// <summary>
        ///发送部门负责人
        /// </summary>		
        public string Note3 { get; set; }

        /// <summary>
        ///出差报告特殊审批
        /// </summary>		
        public string Note4 { get; set; }

        /// <summary>
        ///Note5
        /// </summary>		
        public string Note5 { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Attachment { get; set; }

        /// <summary>
        /// 附件名称
        /// </summary>
        public string AttachmentName { get; set; }
    }
}
