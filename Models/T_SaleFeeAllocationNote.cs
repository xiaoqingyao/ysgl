using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class T_SaleFeeAllocationNote
    {

        /// <summary>
        ///序号
        /// </summary>		
        public long Nid { get; set; }

        /// <summary>
        ///年份
        /// </summary>		
        public string ActionDate { get; set; }

        /// <summary>
        ///时间 年月日 时分秒
        /// </summary>		
        public string ActionTimes { get; set; }

        /// <summary>
        ///BillCode
        /// </summary>		
        public string BillCode { get; set; }

        /// <summary>
        ///车架号
        /// </summary>		
        public string TruckCode { get; set; }

        /// <summary>
        ///车型号
        /// </summary>		
        public string TruckTypeCode { get; set; }

        /// <summary>
        ///单位编号
        /// </summary>		
        public string DeptCode { get; set; }

        /// <summary>
        ///销售过程编号/控制项目编号
        /// </summary>		
        public string ControlItemCode { get; set; }

        /// <summary>
        ///销售费用类别
        /// </summary>		
        public string SaleFeeTypeCode { get; set; }

        /// <summary>
        ///费用
        /// </summary>		
        public decimal? Fee { get; set; }

        /// <summary>
        ///状态字段 0普通 1 财务确认 D 删除
        /// </summary>		
        public string Status { get; set; }

        /// <summary>
        ///审核人
        /// </summary>		
        public string AuditUserCode { get; set; }

        /// <summary>
        ///动作记录 删除、修改该记录 都要往该字段里更新说明
        /// </summary>		
        public string ActionNote { get; set; }

        /// <summary>
        ///费用类别 0 期初分配 1销售提成 2配置项
        /// </summary>		
        public string RebatesType { get; set; }

        /// <summary>
        ///备注
        /// </summary>		
        public string Remark { get; set; }

        /// <summary>
        ///上传附件的名称
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
