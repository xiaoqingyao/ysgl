using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ZiChan_ShiYongZhuangKuang
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper(); 
        public ZiChan_ShiYongZhuangKuang() { }
        public ZiChan_ShiYongZhuangKuang(string strcode) {
            this.ZhuangKuangCode = strcode;
        }
        /// <summary>
        ///状况code
        /// </summary>		
        public string ZhuangKuangCode { get; set; }

        /// <summary>
        ///状况名称 
        /// </summary>		
        public string ZhuangKuangName { get; set; }

        /// <summary>
        ///上级编号
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
            string strsql = "select '['+ZhuangKuangCode+']'+ZhuangKuangName as showName from ZiChan_ShiYongZhuangKuang where ZhuangKuangCode='"+this.ZhuangKuangCode+"'";
            object objRel = server.ExecuteScalar(strsql);
            return objRel == null ? "" : objRel.ToString();
        }
    }
}
