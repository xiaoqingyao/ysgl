using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowModel
{
    public class WorkFlowStep
    {
        //主表编号
        private string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
        //字表编号(按顺序)
        private int stepId;

        public int StepId
        {
            get { return stepId; }
            set { stepId = value; }
        }
        //类型(根据不同类型，调用不同的工厂，制作不同的工作流记录，类型在配置文件里)
        private string stepType;

        public string StepType
        {
            get { return stepType; }
            set { stepType = value; }
        }
        //步骤名称
        private string stepText;

        public string StepText
        {
            get { return stepText; }
            set { stepText = value; }
        }
        //审批号(人员编号,或者部门号,或者...)
        private string checkCode;

        public string CheckCode
        {
            get { return checkCode; }
            set { checkCode = value; }
        }
        //备注
        private string memo;

        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }

        //审批类型,1是单签，2是会签
        private string checkType;

        public string CheckType
        {
            get { return checkType; }
            set { checkType = value; }
        }

        
        //添加一些条件，以限制审批步骤是否生效
        private decimal? minMoney;

        public decimal? MinMoney
        {
            get { return minMoney; }
            set { minMoney = value; }
        }
        private decimal? maxMoney;

        public decimal? MaxMoney
        {
            get { return maxMoney; }
            set { maxMoney = value; }
        }
        private DateTime? minDate;

        public DateTime? MinDate
        {
            get { return minDate; }
            set { minDate = value; }
        }
        private DateTime? maxDate;

        public DateTime? MaxDate
        {
            get { return maxDate; }
            set { maxDate = value; }
        }
        private string isKmZg="0";
        /// <summary>
        /// 是否必须是科目主管  如果判断到该审批流的对应人员不是该报销单对应科目的科目主管 则忽略  1是 0否
        /// </summary>
        public string IsKmZg
        {
            get { return isKmZg; }
            set { isKmZg = value; }
        }
           /// <summary>
        /// 科目类型 对应数据字典22下的某一项   对应生产还是办公之类的
        /// </summary>
        private string kmType_; 
        public string kmType
        {
            get { return kmType_; }
            set { kmType_ = value; }
        }
    }
}
