using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkFlowLibrary.WorkFlowModel;
using WorkFlowLibrary.WorkFlowDal;
using System.Data.SqlClient;
using Bll.UserProperty;
using Dal.Bills;
using Models;
using System.Data;
using Dal;

namespace WorkFlowLibrary.WorkFlowBll
{
    public class WorkFlowManager
    {
        MainWorkFlow wf = new MainWorkFlow();
        MainWorkFlowDal dal = new MainWorkFlowDal();

        public IList<BillToWorkFlow> GetBillAll()
        {
            return dal.GetBillAll();
        }

        //public MainWorkFlow GetWorkFlow(string flowid)
        //{
        //    return dal.GetWorkFlow(flowid);
        //}

        public MainWorkFlow GetWorkFlow(string flowid, string billdept)
        {
            return dal.GetWorkFlow(flowid, billdept);
        }
        public void InsertWF(MainWorkFlow newwf)
        {
            dal.InsertWF(newwf);
        }

        public void EditWF(MainWorkFlow newwf)
        {
            dal.EditWF(newwf);
        }
        public void EditWF(MainWorkFlow newwf, string billdept)
        {
            dal.EditWF(newwf, billdept);
        }

        /// <summary>
        /// 创建审批流
        /// </summary>
        /// <param name="billCode">单据编号</param>
        /// <param name="billType">单据类型</param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public WorkFlowRecord CreateWFRecord(string billCode, string billType, string userCode, string dept)
        {
            MainWorkFlow wf = dal.GetWorkFlowByBillType(billType, dept);
            WorkFlowRecord wfr = new WorkFlowRecord();
            wfr.BillCode = billCode;
            wfr.BillType = billType;
            wfr.IsEdit = 0;
            wfr.RdState = 1;
            wfr.FlowId = wf.FlowId;

            sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
            string strDeptCode = "";
            if (billType.Equals("gkbx"))
            {
                strDeptCode = server.GetCellValue(" select top 1 billDept from bill_main where billname='" + billCode + "'");
            }
            else
            {
                strDeptCode = server.GetCellValue(" select billDept from bill_main where billcode='" + billCode + "'");
            }

            //生成审批记录ID
            IList<WorkFlowRecords> templist = new List<WorkFlowRecords>();
            foreach (WorkFlowStep wfs in wf.StepList)
            {
                IDictionary<string, string> dic = XmlHelper.GetWFTypeConfig(wfs.StepType);

                string checktype = dic["usercolum"];

                bool boCheckKmZg = wfs.IsKmZg.Equals("1");

                //DataTable dt_checkCode;
                IList<string> checkCodeList = new List<string>();

                //检测人员编号
                if (!string.IsNullOrEmpty(dic["codecolum"]))
                {
                    string[] tempArry = wfs.CheckCode.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < tempArry.Length; i++)
                    {

                        //验证该审批节点是否验证科目主管
                        if (boCheckKmZg)
                        {
                            if (this.IsKmZhuGuan(billCode, tempArry[i]))
                            {
                                checkCodeList.Add(tempArry[i]);
                            }
                        }
                        else
                        {
                            checkCodeList.Add(tempArry[i]);
                        }
                    }

                }
                //不查询人员编号
                else
                {
                    DataTable dt_checkCode;
                    string sql = "";
                    //部门主管
                    if (wfs.StepType == "02")
                    {
                        UserMessage userMgr = new UserMessage(userCode);
                        string deptcode = userMgr.GetRootDept().DeptCode;
                        sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + "=@deptcode";
                        SqlParameter[] sps = { new SqlParameter("@deptcode", deptcode) };
                        //checkman = Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
                        dt_checkCode = DataHelper.GetDataTable(sql, sps, false);

                    }
                    //归口
                    else if (wfs.StepType == "05" || wfs.StepType == "06")
                    {
                        string gkdept = "";
                        Bill_Main billMain = (new MainDal()).GetMainByCode(billCode);
                        if (billMain.IsGk.Equals("0"))
                        {
                            continue;
                        }
                        gkdept = billMain.GkDept;
                        if (string.IsNullOrEmpty(gkdept))
                        {
                            gkdept = billMain.BillDept;
                        }
                        sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + "=@deptcode";

                        SqlParameter[] sps = { new SqlParameter("@deptcode", gkdept) };
                        //checkman = Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
                        dt_checkCode = DataHelper.GetDataTable(sql, sps, false);
                    }
                    else
                    {
                        sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + " in(select userdept from " + dic["codemaintable"] + " where " + dic["usercolum"] + "=@usercode)"; ;
                        SqlParameter[] sps = { new SqlParameter("@usercode", userCode) };
                        //checkman = Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
                        dt_checkCode = DataHelper.GetDataTable(sql, sps, false);
                    }

                    foreach (DataRow dr in dt_checkCode.Rows)
                    {
                        //验证该审批节点是否验证科目主管
                        if (boCheckKmZg)
                        {
                            if (this.IsKmZhuGuan(billCode, dr[0].ToString()))
                            {
                                checkCodeList.Add(Convert.ToString(dr[0]));
                            }
                        }
                        else
                        {
                            checkCodeList.Add(Convert.ToString(dr[0]));
                        }
                    }
                }

                //多人
                if (checkCodeList.Count > 1)
                {
                    //多人单签
                    bool bchoose = false;
                    if (wfs.CheckType == "1")
                    {
                        //如果之前有多人审批中的一个那么直接把之前的数据拿到前面来,否则增加新记录。
                        foreach (string checkCode in checkCodeList)
                        {
                            var temp2 = from wfrs in templist
                                        where wfrs.CheckUser == checkCode && wfrs.CheckType == "2"
                                        select wfrs;
                            if (temp2.Count() > 0)
                            {
                                bchoose = true;
                                WorkFlowRecords records = temp2.First();
                                records.StepId = wfs.StepId;
                                records.StepText += "," + wfs.StepText;
                            }
                        }
                        //增加新纪录
                        if (!bchoose)
                        {
                            foreach (string checkCode in checkCodeList)
                            {
                                WorkFlowRecords records = new WorkFlowRecords();
                                records.RecordType = checktype;
                                records.CheckUser = checkCode;
                                records.StepText = wfs.StepText;
                                records.Mind = "";
                                records.CheckType = "1";
                                records.FlowId = wf.FlowId;
                                records.StepId = wfs.StepId;
                                //验证人员是否有对应权限 如果是多人单签 没有该权限直接忽略 edit by Lvcc
                                string msg = "";
                                bool boHas = this.HasDeptRight(checkCode, strDeptCode, out msg);
                                if (boHas)
                                {
                                    templist.Add(records);
                                }
                            }
                        }
                    }
                    //多人会签
                    else
                    {
                        foreach (string checkCode in checkCodeList)
                        {
                            WorkFlowRecords records = new WorkFlowRecords();
                            string steptext = "";
                            //查找是单签,插入过的记录。
                            var temp = from wfrs in templist
                                       where wfrs.CheckUser == checkCode && wfrs.CheckType == "1"
                                       select wfrs.StepId;
                            foreach (int roveId in temp)
                            {
                                var moveRecords = (from moveable in templist
                                                   where moveable.StepId == roveId
                                                   select moveable).ToArray();
                                foreach (WorkFlowRecords move in moveRecords)
                                {
                                    steptext = move.StepText;
                                    templist.Remove(move);
                                }
                                break;
                            }
                            //查找是会签已插入过的记录
                            var temp2 = from wfrs in templist
                                        where wfrs.CheckUser == checkCode && wfrs.CheckType == "2"
                                        select wfrs;
                            foreach (WorkFlowRecords move in temp2)
                            {
                                steptext = move.StepText;
                                templist.Remove(move);
                                break;
                            }

                            records.RecordType = checktype;
                            records.CheckUser = checkCode;
                            records.StepText = steptext + "," + wfs.StepText;
                            records.Mind = "";
                            //不是单签
                            records.CheckType = "2";
                            records.FlowId = wf.FlowId;
                            records.StepId = wfs.StepId;
                            //验证人员是否有对应权限 如果是多人会签 没有该权限报错 edit by Lvcc
                            string msg = "";

                            bool boHas = this.HasDeptRight(checkCode, strDeptCode, out msg);
                            if (!boHas)
                            {
                                throw new Exception(userCode);
                            }
                            templist.Add(records);
                        }
                    }
                } //单人
                else if (checkCodeList.Count == 1)
                {
                    WorkFlowRecords records = new WorkFlowRecords();
                    string steptext = "";
                    //查找是单签,插入过的记录。
                    var temp = from wfrs in templist
                               where wfrs.CheckUser == checkCodeList[0] && wfrs.CheckType == "1"
                               select wfrs.StepId;
                    foreach (int roveId in temp)
                    {
                        var moveRecords = (from moveable in templist
                                           where moveable.StepId == roveId
                                           select moveable).ToArray();
                        foreach (WorkFlowRecords move in moveRecords)
                        {
                            steptext = move.StepText;
                            templist.Remove(move);
                        }
                        break;
                    }
                    //查找是会签已插入过的记录
                    var temp2 = from wfrs in templist
                                where wfrs.CheckUser == checkCodeList[0] && wfrs.CheckType == "2"
                                select wfrs;
                    foreach (WorkFlowRecords move in temp2)
                    {
                        steptext = move.StepText;
                        templist.Remove(move);
                        break;
                    }

                    records.RecordType = checktype;
                    records.CheckUser = checkCodeList[0];
                    records.StepText = steptext + "," + wfs.StepText;
                    records.Mind = "";
                    //不是单签 单人就是会签的模式
                    records.CheckType = "2";
                    records.FlowId = wf.FlowId;
                    records.StepId = wfs.StepId;
                    //验证人员是否有对应权限 如果是多人会签 没有该权限直接忽略 edit by Lvcc
                    string msg = "";
                    bool boHas = this.HasDeptRight(checkCodeList[0], strDeptCode, out msg);
                    if (!boHas)
                    {
                        throw new Exception("创建审批流失败：用户编号为" + checkCodeList[0] + "的用户没有该单据所对应部门的操作权限！");
                    }
                    templist.Add(records);
                }
                else { }
            }
            //预留排序方法
            wfr.RecordList = SortRecords(templist);
            return wfr;
        }


        /// <summary>
        /// 创建审批流
        /// </summary>
        /// <param name="billCode">单据编号</param>
        /// <param name="billType">单据类型</param>
        /// <param name="userCode"></param>
        /// <returns></returns>
        public WorkFlowRecord CreateWFRecordNew(string billCode, string billType, string userCode, string dept)
        {
            decimal billJe = 0;
            string flowid = "";
            string strDeptCode = "";
            if (billType != "gkbx" && billType != "ybbx" && billType != "yksq_dz")//因为市立医院 //&& billType != "ybbx" && billType != "yksq_dz"大智没有金额限制 所以暂时这样处理
            {
                Bill_Main main = new MainDal().GetMainByCode(billCode);
                billJe = main.BillJe;
                flowid = main.FlowId;
                strDeptCode = main.BillDept;
            }
            else
            {
                Bill_Main main = new MainDal().GetMainByCode(billCode);
                billJe = main.BillJe;
                flowid = main.FlowId;
                strDeptCode = main.BillDept;

                //IList<Bill_Main> main = new MainDal().GetMainsByBillName(billCode);
                //billJe = main.Sum(p => p.BillJe);
                //flowid = main[0].FlowId;
                //strDeptCode = main[0].BillDept;
            }


            MainWorkFlow wf = dal.GetWorkFlowByBillType(flowid, dept);
            WorkFlowRecord wfr = new WorkFlowRecord();

            wfr.BillCode = billCode;
            wfr.BillType = flowid;
            wfr.IsEdit = 0;
            wfr.RdState = 1;
            wfr.FlowId = flowid;
            sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
            //生成审批记录ID
            IList<WorkFlowRecords> templist = new List<WorkFlowRecords>();
            foreach (WorkFlowStep wfs in wf.StepList)
            {
                if (!string.IsNullOrEmpty(wfs.kmType))
                {
                    if (wfs.kmType == "1")//如果设置了
                    {
                        decimal tempJe = GetSxje(flowid, wfs.StepId.ToString(), wfs.Memo);//有效金额min
                        decimal tepmaxje = GetMaxSxje(flowid, wfs.StepId.ToString(), wfs.Memo);//有效金额max

                        if (billJe >= tepmaxje || billJe < tempJe)
                        {
                            continue;
                        }

                    }

                    //decimal tempJe = GetSxje(flowid, wfs.StepId.ToString());//有效金额
                    //if (wfs.kmType != "sum")
                    //{
                    //    billJe = GetKmTypeJe(billCode, wfs.kmType);
                    //}
                    //if (tempJe > billJe)//如果有效金额大于
                    //{
                    //    continue;
                    //}
                }


                IDictionary<string, string> dic = XmlHelper.GetWFTypeConfig(wfs.StepType);

                string checktype = dic["usercolum"];

                bool boCheckKmZg = wfs.IsKmZg.Equals("1");

                //DataTable dt_checkCode;
                IList<string> checkCodeList = new List<string>();

                //检测人员编号
                if (!string.IsNullOrEmpty(dic["codecolum"]))
                {
                    string[] tempArry = wfs.CheckCode.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < tempArry.Length; i++)
                    {

                        //验证该审批节点是否验证科目主管
                        if (boCheckKmZg)
                        {
                            if (this.IsKmZhuGuan(billCode, tempArry[i]))
                            {
                                checkCodeList.Add(tempArry[i]);
                            }
                        }
                        else
                        {
                            checkCodeList.Add(tempArry[i]);
                        }
                    }

                }
                //不查询人员编号
                else
                {
                    DataTable dt_checkCode = new DataTable();
                    string sql = "";
                    //部门主管
                    if (wfs.StepType == "02")
                    {
                        UserMessage userMgr = new UserMessage(userCode);
                        string deptcode = userMgr.GetRootDept().DeptCode;
                        sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + "=@deptcode";
                        SqlParameter[] sps = { new SqlParameter("@deptcode", deptcode) };
                        //checkman = Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
                        dt_checkCode = DataHelper.GetDataTable(sql, sps, false);

                    }
                    //归口
                    else if (wfs.StepType == "05" || wfs.StepType == "06")
                    {
                        string gkdept = "";
                        string billdept = "";
                        IList<Bill_Main> billMain = (new MainDal()).GetMainsByBillNameorCode(billCode);
                        if (billMain == null || billMain.Count == 0)
                        {
                            continue;
                        }

                        for (int j = 0; j < billMain.Count; j++)
                        {
                            if (billMain[j].IsGk.Equals("0"))
                            {
                                continue;
                            }
                            gkdept = billMain[j].GkDept;

                            if (string.IsNullOrEmpty(gkdept))
                            {
                                gkdept = billMain[j].BillDept;
                            }

                            sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + "=@deptcode";
                            SqlParameter[] sps = { new SqlParameter("@deptcode", gkdept) };
                            DataTable dteve = DataHelper.GetDataTable(sql, sps, false);
                            for (int k = 0; k < dteve.Rows.Count; k++)
                            {
                                if (dt_checkCode.Columns.Count == 0)
                                {
                                    dt_checkCode = dteve.Clone();
                                }
                                dt_checkCode.Rows.Add(dteve.Rows[k][0]);
                            }
                        }
                    }
                    else
                    {
                        sql = "select " + dic["usercolum"] + " from " + dic["name"] + " where " + dic["filter"] + " in(select userdept from " + dic["codemaintable"] + " where " + dic["usercolum"] + "=@usercode)"; ;
                        SqlParameter[] sps = { new SqlParameter("@usercode", userCode) };
                        //checkman = Convert.ToString(DataHelper.ExecuteScalar(sql, sps, false));
                        dt_checkCode = DataHelper.GetDataTable(sql, sps, false);
                    }

                    foreach (DataRow dr in dt_checkCode.Rows)
                    {
                        //验证该审批节点是否验证科目主管
                        if (boCheckKmZg)
                        {
                            if (this.IsKmZhuGuan(billCode, dr[0].ToString()))
                            {
                                checkCodeList.Add(Convert.ToString(dr[0]));
                            }
                        }
                        else
                        {
                            checkCodeList.Add(Convert.ToString(dr[0]));
                        }
                    }
                }

                //多人
                if (checkCodeList.Count > 1)
                {
                    //多人单签
                    bool bchoose = false;
                    if (wfs.CheckType == "1")
                    {
                        //如果之前有多人审批中的一个那么直接把之前的数据拿到前面来,否则增加新记录。
                        foreach (string checkCode in checkCodeList)
                        {
                            var temp2 = from wfrs in templist
                                        where wfrs.CheckUser == checkCode && wfrs.CheckType == "2"
                                        select wfrs;
                            if (temp2.Count() > 0)
                            {
                                bchoose = true;
                                WorkFlowRecords records = temp2.First();
                                records.StepId = wfs.StepId;
                                records.StepText += "," + wfs.StepText;
                            }
                        }
                        //增加新纪录
                        if (!bchoose)
                        {
                            foreach (string checkCode in checkCodeList)
                            {
                                WorkFlowRecords records = new WorkFlowRecords();
                                records.RecordType = checktype;
                                records.CheckUser = checkCode;
                                records.StepText = wfs.StepText;
                                records.Mind = "";
                                records.CheckType = "1";
                                records.FlowId = wf.FlowId;
                                records.StepId = wfs.StepId;
                                //验证人员是否有对应权限 如果是多人单签 没有该权限直接忽略 edit by Lvcc
                                string msg = "";
                                bool boHas = this.HasDeptRight(checkCode, strDeptCode, out msg);
                                if (boHas)
                                {
                                    templist.Add(records);
                                }
                            }
                        }
                    }
                    //多人会签
                    else
                    {
                        foreach (string checkCode in checkCodeList)
                        {
                            WorkFlowRecords records = new WorkFlowRecords();
                            string steptext = "";
                            //查找是单签,插入过的记录。
                            var temp = from wfrs in templist
                                       where wfrs.CheckUser == checkCode && wfrs.CheckType == "1"
                                       select wfrs.StepId;
                            foreach (int roveId in temp)
                            {
                                var moveRecords = (from moveable in templist
                                                   where moveable.StepId == roveId
                                                   select moveable).ToArray();
                                foreach (WorkFlowRecords move in moveRecords)
                                {
                                    steptext = move.StepText;
                                    templist.Remove(move);
                                }
                                break;
                            }
                            //查找是会签已插入过的记录
                            var temp2 = from wfrs in templist
                                        where wfrs.CheckUser == checkCode && wfrs.CheckType == "2"
                                        select wfrs;
                            foreach (WorkFlowRecords move in temp2)
                            {
                                steptext = move.StepText;
                                templist.Remove(move);
                                break;
                            }

                            records.RecordType = checktype;
                            records.CheckUser = checkCode;
                            records.StepText = steptext + "," + wfs.StepText;
                            records.Mind = "";
                            //不是单签
                            records.CheckType = "2";
                            records.FlowId = wf.FlowId;
                            records.StepId = wfs.StepId;
                            //验证人员是否有对应权限 如果是多人会签 没有该权限报错 edit by Lvcc
                            string msg = "";

                            bool boHas = this.HasDeptRight(checkCode, strDeptCode, out msg);
                            if (!boHas)
                            {
                                throw new Exception(userCode);
                            }
                            templist.Add(records);
                        }
                    }
                } //单人
                else if (checkCodeList.Count == 1)
                {
                    WorkFlowRecords records = new WorkFlowRecords();
                    string steptext = "";
                  //  查找是单签,插入过的记录。
                    var temp = from wfrs in templist
                               where wfrs.CheckUser == checkCodeList[0] && wfrs.CheckType == "1"
                               select wfrs.StepId;
                    foreach (int roveId in temp)
                    {
                        var moveRecords = (from moveable in templist
                                           where moveable.StepId == roveId
                                           select moveable).ToArray();
                        foreach (WorkFlowRecords move in moveRecords)
                        {
                            steptext = move.StepText;
                            templist.Remove(move);
                        }
                        break;
                    }
                    //查找是会签已插入过的记录
                    var temp2 = from wfrs in templist
                                where wfrs.CheckUser == checkCodeList[0] && wfrs.CheckType == "2"
                                select wfrs;
                    foreach (WorkFlowRecords move in temp2)
                    {
                        steptext = move.StepText;
                        templist.Remove(move);
                        break;
                    }

                    records.RecordType = checktype;
                    records.CheckUser = checkCodeList[0];
                    records.StepText = steptext + "," + wfs.StepText;
                    records.Mind = "";
                    //不是单签 单人就是会签的模式
                    records.CheckType = "2";
                    records.FlowId = wf.FlowId;
                    records.StepId = wfs.StepId;
                    //验证人员是否有对应权限 如果是多人会签 没有该权限直接忽略 edit by Lvcc
                    string msg = "";
                    bool boHas = this.HasDeptRight(checkCodeList[0], strDeptCode, out msg);
                    if (!boHas)
                    {
                        throw new Exception("创建审批流失败：用户编号为" + checkCodeList[0] + "的用户没有该单据所对应部门的操作权限！");
                    }
                    templist.Add(records);
                }
                else { }
            }

            //删除重复审批节点
            //使用匿名方法

            IList<WorkFlowRecords> newtemplist = new List<WorkFlowRecords>();

            IList<WorkFlowRecords> delegateList = templist.Distinct(new Compare<WorkFlowRecords>(
                      delegate(WorkFlowRecords x, WorkFlowRecords y)
                      {
                          if (null == x || null == y) return false;
                          return x.CheckUser == y.CheckUser;
                      })).ToList();
            newtemplist = delegateList;

         
            //预留排序方法
            wfr.RecordList = SortRecords(newtemplist);
            return wfr;
        }
        //================================
        public delegate bool EqualsComparer<T>(T x, T y);

        public class Compare<T> : IEqualityComparer<T>
        {
            private EqualsComparer<T> _equalsComparer;

            public Compare(EqualsComparer<T> equalsComparer)
            {
                this._equalsComparer = equalsComparer;
            }

            public bool Equals(T x, T y)
            {
                if (null != this._equalsComparer)
                    return this._equalsComparer(x, y);
                else
                    return false;
            }

            public int GetHashCode(T obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
        //=================================
        /// <summary>
        /// 根据单号获取一般报销单对应费用科目类型的科目的总金额
        /// </summary>
        /// <param name="billcode">单号</param>
        /// <param name="kmType">费用类型 来自于workstep kmtype 对应于数据字典的00 22</param>
        /// <returns>某一类型科目的总金额</returns>
        private decimal GetKmTypeJe(string billcode, string kmType)
        {
            string strJe = DataHelper.ExecuteScalar("select  ISNULL(sum(mx.je),0) from bill_main m , bill_ybbxmxb_fykm mx	 where m.billcode=mx.billcode and m.billcode='" + billcode + "' and (select kmtype from bill_yskm where yskmCode=mx.fykm)='" + kmType + "'", null, false).ToString();
            return Convert.ToDecimal(strJe);
        }
        private decimal GetSxje(string flowid, string stepid, string strdeptcode)
        {
            decimal decje = 0;
            string strsql = @"select isnull(minmoney,0) as je from  dbo.workflowstep where flowid='" + flowid + "' and stepid='" + stepid + "' and  memo='" + strdeptcode + "'  ";
            DataTable dtJe = DataHelper.GetDataTable(strsql, null, false);
            if (dtJe != null && dtJe.Rows.Count != 0)
            {
                decje = Convert.ToDecimal(dtJe.Rows[0]["je"].ToString()) * 10000;
            }

            return decje;
        }
        private decimal GetMaxSxje(string flowid, string stepid, string strdeptcode)
        {
            decimal decje = 0;
            DataTable dtJe = DataHelper.GetDataTable("select isnull(maxmoney,0) as je  from  dbo.workflowstep where flowid='" + flowid + "' and stepid='" + stepid + "' and  memo='" + strdeptcode + "'  ", null, false);
            if (dtJe != null && dtJe.Rows.Count != 0)
            {
                decje = Convert.ToDecimal(dtJe.Rows[0]["je"].ToString()) * 10000;
            }

            return decje;
        }
        public int UpdateBillToEnd(string billCode)
        {
            return DataHelper.ExcuteNonQuery("update bill_main set stepID='end' where billCode='" + billCode + "' ", null, false);
        }
        private decimal GetBillJe(string billCode)
        {
            string strJe = DataHelper.ExecuteScalar("select isnull(billJe,0) from bill_main where billcode='" + billCode + "' ", null, false).ToString();
            return Convert.ToDecimal(strJe);
        }


        private decimal GetSxje(string flowid, string stepid)
        {
            string strJe = DataHelper.ExecuteScalar("select isnull(minmoney,0) from  dbo.workflowstep where flowid='" + flowid + "' and stepid='" + stepid + "' ", null, false).ToString();
            return Convert.ToDecimal(strJe);
        }

        //审批流排序
        private IList<WorkFlowRecords> SortRecords(IList<WorkFlowRecords> recordsList)
        {
            int preStep = 0;
            int i = 0;
            var tempList = from records in recordsList
                           orderby records.StepId
                           select records;

            IList<WorkFlowRecords> retList = new List<WorkFlowRecords>();
            foreach (WorkFlowRecords records in tempList)
            {
                if (records.StepId != preStep)
                {
                    i++;
                }

                preStep = records.StepId;
                records.StepId = i;
                retList.Add(records);

            }
            return retList;
        }

        /// <summary>
        /// 该审核人是否有管理权限
        /// </summary>
        private bool HasDeptRight(string strCheckUserCode, string strDeptCode, out string msg)
        {
            //////单据对应部门
            ////string sqlDept = "select userDept from bill_users where userCode='" + strBillUser + "'";
            ////object objDept=DataHelper.ExecuteScalar(sqlDept,null,false);
            ////if (objDept==null)
            ////{
            ////    msg="没有找到填报人对应的部门！";
            ////    return false;
            ////}
            ////审批人对应部门
            //string sqlCheckUserDept = "select userDept from bill_users where userCode='" + strCheckUserCode + "'";
            //object objCheckDept = DataHelper.ExecuteScalar(sqlCheckUserDept, null, false);
            //if (objCheckDept == null)
            //{
            //    msg = "没有找到审批人对应的部门！";
            //    return false;
            //}
            ////获取审批人部门对应上级部门
            //DepartmentManager deptManager = new DepartmentManager(objCheckDept.ToString());
            //Bill_Departments modelParentDept = new Bill_Departments();
            //modelParentDept = deptManager.GetRoot();
            //List<string> lstRightDeptCodes = new List<string>();

            ////找到默认存在的权限
            //string deptCodes = (new DepartmentBLL()).GetUserRightDepartments(strCheckUserCode, "");
            //string[] strDeptCodes = deptCodes.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            ////找到userright表中记录的权限
            ////因为在控制管理权限的时候 如果上级部门的权限有了 下级部门在userright表中是没有数据的  当没有上级部门权限 下级部门的权限才会一个个的添加到表中
            //string strSqlRight = "select objectID from bill_userRight where (rightType='2' and (objectID='" + modelParentDept.DeptCode + "' or objectID='" + strDeptCode + "' or objectID='000001') and usercode='" + strCheckUserCode + "') or (rightType='3' and (objectID='" + modelParentDept.DeptCode + "' or objectID='" + strDeptCode + "' or objectID='000001') and usercode=(select userGroup from bill_users where usercode='" + strCheckUserCode + "'))";
            //DataTable dtRel = DataHelper.GetDataTable(strSqlRight,null,false);
            //int iArrRelCount = strDeptCodes.Length;
            //int iDtRelCount = dtRel.Rows.Count;
            ////将两个结果集合并
            //if (iArrRelCount > 0)
            //{
            //    for (int i = 0; i < iArrRelCount; i++)
            //    {
            //        lstRightDeptCodes.Add(strDeptCodes[i]);
            //    }
            //}

            //if (iDtRelCount>0)
            //{
            //    for (int i = 0; i < iDtRelCount; i++)
            //    {
            //        lstRightDeptCodes.Add(dtRel.Rows[i][0].ToString());
            //    }
            //}
            ////判断单据对应的部门是否存在于集合中
            //bool boFlg = false;
            //int iLstAllCount = lstRightDeptCodes.Count;
            //for (int i = 0; i < iLstAllCount; i++)
            //{
            //    if (lstRightDeptCodes[i].Equals(strDeptCode))
            //    {
            //        boFlg = true;
            //        break;
            //    }
            //}
            msg = "";
            //return boFlg;
            return true;
        }
        /// <summary>
        /// 是否是科目主管
        /// </summary>
        /// <param name="strBillCode"></param>
        /// <param name="strUserCode"></param>
        /// <returns></returns>
        private bool IsKmZhuGuan(string strBillCode, string strUserCode)
        {
            bool boHasKmzg = false;
            string flowid = "";
            string sqlFlow = "select flowid from bill_main where billcode='" + strBillCode + "' or billcode in (select billcode from bill_main where billname = '" + strBillCode + "')";
            object objflow = DataHelper.ExecuteScalar(sqlFlow, null, false);
            flowid = objflow == null ? "" : objflow.ToString();
            DataTable dtAllKmRel = new DataTable();
            if (flowid == "ybbx" || flowid == "yksq_dz" || flowid == "gkbx")//费用报销类
            {
                string strSelectBillKmSql = "select fykm from bill_ybbxmxb_fykm where billCode='" + strBillCode + "' or billcode in (select billcode from bill_main where billname = '" + strBillCode + "')";
                dtAllKmRel = DataHelper.GetDataTable(strSelectBillKmSql, null, false);
            }
            else if (flowid == "yszj" || flowid == "xmyszj")
            {
                string sqlzj = "select yskm as fykm from bill_ysmxb where billcode='" + strBillCode + "'";
                dtAllKmRel = DataHelper.GetDataTable(sqlzj, null, false);
            }

            for (int i = 0; i < dtAllKmRel.Rows.Count; i++)
            {
                string strSelectKmZg = "select kmzg from bill_yskm where yskmCode='" + dtAllKmRel.Rows[i]["fykm"].ToString() + "'";
                object objCurrentKmZg = DataHelper.ExecuteScalar(strSelectKmZg, null, false);
                string strCurrentKmZg = objCurrentKmZg == null ? "" : objCurrentKmZg.ToString();

                if (!strCurrentKmZg.Equals(""))
                {
                    string[] arrAllZg = strCurrentKmZg.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < arrAllZg.Length; j++)
                    {
                        if (strUserCode.Equals(arrAllZg[j]))
                        {
                            boHasKmzg = true;
                            break;
                        }
                    }
                }
            }

            return boHasKmzg;
        }
    }
}
