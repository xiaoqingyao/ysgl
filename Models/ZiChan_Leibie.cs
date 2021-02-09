using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 资产类别
    /// </summary>
    public class ZiChan_Leibie
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper(); 
        public ZiChan_Leibie() { }
        public ZiChan_Leibie(string strcode)
        {
            this.LeibieCode = strcode;
        }
        /// <summary>
        ///资产类别编号
        /// </summary>		
        public string LeibieCode { get; set; }

        /// <summary>
        ///资产类别name
        /// </summary>		
        public string LeibieName { get; set; }

        /// <summary>
        ///使用期限（月）
        /// </summary>		
        public int? ShiYongQiXian { get; set; }

        /// <summary>
        ///计量单位
        /// </summary>		
        public string JiLiangDanWei { get; set; }

        /// <summary>
        ///上级code
        /// </summary>		
        public string ParentCode { get; set; }

        /// <summary>
        ///备注
        /// </summary>		
        public string BeiZhu { get; set; }

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
        public override string ToString()
        {
            string strsql = "select '['+LeibieCode+']'+LeibieName as showName from ZiChan_Leibie where LeibieCode='" + this.LeibieCode + "'";
            object objRel = server.ExecuteScalar(strsql);
            return objRel == null ? "" : objRel.ToString();
        }
    }
}
