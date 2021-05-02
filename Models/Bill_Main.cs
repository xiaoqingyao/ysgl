using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Bill_Main
    {
        string billName;

        public string BillName
        {
            get { return billName; }
            set { billName = value; }
        }
        string billCode;

        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
        string stepId;

        public string StepId
        {
            get { return stepId; }
            set { stepId = value; }
        }
        string billUser;

        public string BillUser
        {
            get { return billUser; }
            set { billUser = value; }
        }
        DateTime? billDate;

        public DateTime? BillDate
        {
            get { return billDate; }
            set { billDate = value; }
        }
        string billDept;

        public string BillDept
        {
            get { return billDept; }
            set { billDept = value; }
        }
        decimal billJe;

        public decimal BillJe
        {
            get { return billJe; }
            set { billJe = value; }
        }
        int loopTimes;

        public int LoopTimes
        {
            get { return loopTimes; }
            set { loopTimes = value; }
        }
        string billType;

        public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        string billName2;
        /// <summary>
        /// 如果flowid='yzsj'则该字段记录追加说明  如果flowid='yshz'预算汇总  存汇总的部门编号字符串
        /// </summary>
        public string BillName2
        {
            get { return billName2; }
            set { billName2 = value; }
        }

        string isGk;

        public string IsGk
        {
            get { return isGk; }
            set { isGk = value; }
        }
        string gkDept;

        public string GkDept
        {
            get { return gkDept; }
            set { gkDept = value; }
        }

        string dydj;
        /// <summary>
        /// 对应单据的编号（预算类型）对应预算类型 01收入 02费用……
        /// </summary>
        public string Dydj
        {
            get { return dydj; }
            set { dydj = value; }
        }
        string note1;
        /// <summary>
        /// 如果是预算汇总 存showdepts
        /// </summary>
        public string Note1
        {
            get { return note1; }
            set { note1 = value; }
        }
        string note2;
        /// <summary>
        /// 如果是预算汇总 存财年
        /// </summary>
        public string Note2
        {
            get { return note2; }
            set { note2 = value; }
        }
        string note3;
        public string Note3
        {
            get { return note3; }
            set { note3 = value; }
        }
        string note4;
        public string Note4
        {
            get { return note4; }
            set { note4 = value; }
        }
        string note5;
        public string Note5
        {
            get { return note5; }
            set { note5 = value; }
        }
        public string needBx
        {
            get;
            set;
        }
    }
}
