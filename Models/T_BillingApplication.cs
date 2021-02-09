using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class T_BillingApplication
    {
        /// <summary>
        ///申请单号
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        ///BillMain表的code（外键）
        /// </summary>		
        public string BillMainCode { get; set; }

        /// <summary>
        ///车架号
        /// </summary>		
        public string TruckCode { get; set; }

        /// <summary>
        ///销售公司code
        /// </summary>		
        public string SaleDeptCode { get; set; }

        /// <summary>
        ///申请日期
        /// </summary>		
        public string AppDate { get; set; }
        /// <summary>
        /// 经销商姓名
        /// </summary>
        public string DealersName { get; set; }
        /// <summary>
        ///系统登录用户code
        /// </summary>		
        public string SysPersionCode { get; set; }

        /// <summary>
        ///系统时间
        /// </summary>		
        public string SysDateTime { get; set; }

        /// <summary>
        ///申请返利说明
        /// </summary>		
        public string Explain { get; set; }

        /// <summary>
        ///是否是军车 1=是 0=否
        /// </summary>		
        public string IsJC { get; set; }
        /// <summary>
        /// 是否做过特殊返利申请  1=是 0=否
        /// </summary>
        public string IsSpApp { get; set; }
        /// <summary>
        ///附件
        /// </summary>		
        public string AttachmentUrl { get; set; }

        /// <summary>
        ///Note1上传附件的名称
        /// </summary>		
        public string Note1 { get; set; }

        /// <summary>
        ///Note2 订单号
        /// </summary>		
        public string Note2 { get; set; }

        /// <summary>
        ///Note3已对付 0--否 1--是
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
        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceCode { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public string BillingDate { get; set; }
        /// <summary>
        /// 开票系统时间
        /// </summary>
        public string BIllingSysTime { get; set; }



    }
}
