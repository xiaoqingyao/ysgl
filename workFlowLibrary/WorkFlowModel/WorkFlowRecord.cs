using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkFlowLibrary.WorkFlowModel
{
    public class WorkFlowRecord
    {
        //记录编号(被审批的单据持有此号)
        private int recordId;

        public int RecordId
        {
            get { return recordId; }
            set { recordId = value; }
        }
        //单据编号
        private string billCode;

        public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        //工作流编号
        private string billType;

        public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        
        //单据类型(是什么单子,类名)
        private string flowId;

        public string FlowId
        {
            get { return flowId; }
            set { flowId = value; }
        }
        //是否经过修改,0没有,1有
        private int isEdit;

        public int IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; }
        }
        //状态(0,等待;1,正在执行;2,通过;3,废弃)
        private int rdState;

        public int RdState
        {
            get { return rdState; }
            set { rdState = value; }
        }
        //字表
        private IList<WorkFlowRecords> recordList;

        public IList<WorkFlowRecords> RecordList
        {
            get { return recordList; }
            set { recordList = value; }
        }

    }
}
