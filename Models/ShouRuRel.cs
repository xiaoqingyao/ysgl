using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ShouRuRel
    {
        public List<ShouRuTemp> Data { get; set; }
        /// <summary>
        /// 错误号
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
