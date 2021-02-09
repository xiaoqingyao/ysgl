using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_DataDic
    {
        private string dicType;

        public string DicType
        {
            get { return dicType; }
            set { dicType = value; }
        }
        private string dicName;

        public string DicName
        {
            get { return dicName; }
            set { dicName = value; }
        }
        private string dicCode;

        public string DicCode
        {
            get { return dicCode; }
            set { dicCode = value; }
        }
        private string cjys;
        /// <summary>
        /// 是否冲减预算
        /// </summary>
        public string Cjys
        {
            get { return cjys; }
            set { cjys = value; }
        }
        private string cys;
        /// <summary>
        /// 是否控制超预算
        /// </summary>
        public string Cys
        {
            get { return cys; }
            set { cys = value; }
        }
        private string cdj;
        /// <summary>
        /// 是否附加单据
        /// </summary>
        public string Cdj
        {
            get { return cdj; }
            set { cdj = value; }
        }
    }
}
