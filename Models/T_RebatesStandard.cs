using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// T_RebatesStandard特殊返利配置表
    /// </summary>
    public class T_RebatesStandard
    {


        /// <summary>
        ///NID
        /// </summary>		
        public long NID { get; set; }

        /// <summary>
        ///有效日期起始固定八位2012-01-01
        /// </summary>		
        public string EffectiveDateFrm { get; set; }

        /// <summary>
        ///有效日期止2012-12-31
        /// </summary>		
        public string EffectiveDateTo { get; set; }

        /// <summary>
        ///车辆类型Code（最末级类型）
        /// </summary>		
        public string TruckTypeCode { get; set; }

        /// <summary>
        ///销售单位Code
        /// </summary>		
        public string DeptCode { get; set; }

        /// <summary>
        ///销售费用类别
        /// </summary>		
        public string SaleFeeTypeCode { get; set; }

        /// <summary>
        ///销售过程/配置项code
        /// </summary>		
        public string ControlItemCode { get; set; }

        /// <summary>
        ///提成金额
        /// </summary>		
        public decimal? Fee { get; set; }

        /// <summary>
        ///状态 1=启用 0=禁用 2财务确认通过
        /// </summary>		
        public string Status { get; set; }
        /// <summary>
        ///费用类别 0 期初分配 1 销售提成  2 配置项
        /// </summary>		
        public string Type { get; set; }
        /// <summary>
        ///销售过程code(废弃)
        /// </summary>		

        public string SaleProcessCode { get; set; }

        /// <summary>
        ///  SaleCountFrm销售辆数起
        /// </summary>		
        public int SaleCountFrm { get; set; }

        /// <summary>
        ///  SaleCountTo销售辆数止
        /// </summary>		
        public int SaleCountTo { get; set; }
        /// <summary>
        ///备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        ///AuditUserCode 审核人code
        /// </summary>		
        public string AuditUserCode { get; set; }
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


    }
}
