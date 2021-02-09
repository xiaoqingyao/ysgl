using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowDal
{
    public class BillToWorkFlow
    {
        private string billType;

        public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        private string billName;

        public string BillName
        {
            get { return billName; }
            set { billName = value; }
        }
        private string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
    }
}
