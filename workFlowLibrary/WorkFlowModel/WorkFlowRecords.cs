using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowModel
{
    public class WorkFlowRecords
    {
        //记录主表编号(被审批的单据持有此号)
        private int recordid;

        public int Recordid
        {
          get { return recordid; }
          set { recordid = value; }
        }

        //流程编号
        private string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
        //步骤编号
        private int stepId;

        public int StepId
        {
            get { return stepId; }
            set { stepId = value; }
        }
        //步骤名称
        private string stepText;

        public string StepText
        {
            get { return stepText; }
            set { stepText = value; }
        }

        //类型(一个部门的人都能审，还是只能是个人))
        private string recordType;

        public string RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }
        //审批人(应当审批的人,或部门)
        private string checkUser;

        public string CheckUser
        {
            get { return checkUser; }
            set { checkUser = value; }
        }
        //单据审批人(真正审批的人)
        private string finalUser;

        public string FinalUser
        {
            get { return finalUser; }
            set { finalUser = value; }
        }
        //状态(0,等待;1,正在执行;2,通过;3,废弃)
        private int rdState;

        public int RdState
        {
            get { return rdState; }
            set { rdState = value; }
        }
        //审批意见
        private string mind;

        public string Mind
        {
            get { return mind; }
            set { mind = value; }
        }
        //审批时间
        private DateTime? checkDate;

        public DateTime? CheckDate
        {
            get { return checkDate; }
            set { checkDate = value; }
        }
        //审批类型
        private string checkType;

        public string CheckType
        {
            get { return checkType; }
            set { checkType = value; }
        }
    }
}
