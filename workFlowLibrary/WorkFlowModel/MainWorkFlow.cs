using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowModel
{
    public class MainWorkFlow
    {
        //审批流编号
        private string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
        //审批名称
        private string flowName;

        public string FlowName
        {
            get { return flowName; }
            set { flowName = value; }
        }
        //走该审批流的单据(类名，或表名)
        private IList<string> billType;

        public IList<string> BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        //步骤
        private IList<WorkFlowStep> stepList;

        public IList<WorkFlowStep> StepList
        {
            get { return stepList; }
            set { stepList = value; }
        }

        
    }
}
