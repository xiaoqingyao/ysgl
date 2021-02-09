using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkFlowLibrary.WorkFlowModel;
using WorkFlowLibrary.WorkFlowDal;
using Dal.Bills;
using System.Data.SqlClient;
using Dal.Loan;
using Models;
using System.Data;
using Dal;

namespace WorkFlowLibrary.WorkFlowBll
{
    public class WorkFlowRecordManager
    {
        WorkFlowRecordDal dal = new WorkFlowRecordDal();

        /// <summary>
        /// 建立审批流
        /// </summary>
        /// <param name="record"></param>
        public void InsertRecord(WorkFlowRecord record)
        {
            IList<WorkFlowRecords> list = record.RecordList;
            IList<WorkFlowRecords> inlist = new List<WorkFlowRecords>();
            foreach (WorkFlowRecords records in list)
            {
                if (records.StepId == 1)
                {
                    records.RdState = 1;
                }
                inlist.Add(records);
            }
            record.RecordList = inlist;
            dal.InsertRecord(record);
        }
        /// <summary>
        /// 获得审批流
        /// </summary>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public WorkFlowRecord GetWFRecordByBill(string billcode)
        {
            return dal.GetWFRecordByBill(billcode);
        }
        /// <summary>
        /// 获取单据审批状态
        /// </summary>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public string WFState(string billcode)
        {
            //验证一下是否有驳回失败的单子


            string strsql = @"select * 
                    from dbo.workflowrecord 
                    where rdState=3
                    and recordid not in (select recordid from workflowrecords where rdstate = 3)";
            DataTable dt = new DataTable();
            dt = DataHelper.GetDataTable(strsql, null, false);
            if (dt != null && dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string strrecordid = dt.Rows[i]["recordid"].ToString();

                    string updatesql = @"  update workflowrecords set rdstate=3 where recordid='" + strrecordid + "' and stepid=1";
                    DataHelper.ExcuteNonQuery(updatesql, null, false);
                }
            }



            string stepid = Convert.ToString(DataHelper.ExecuteScalar("select  stepID  from bill_main where billCode='" + billcode + "' or billname='" + billcode + "'", null, false));
            WorkFlowRecord record = GetWFRecordByBill(billcode);
            string ret = "";
            if (record == null && stepid != "end")
            {
                ret = "未提交";
            }
            else if (stepid == "end" && record == null)
            {
                ret = "审批通过";
            }
            else
            {//状态(0,等待;1,正在执行;2,通过;3,废弃)
                if (record.RdState == 3)
                {
                   



                    IList<WorkFlowRecords> list = record.RecordList;
                    WorkFlowRecords disRecord = (from records in list
                                                 where records.RdState == 3
                                                 select records).First();
                    string usercode = disRecord.FinalUser;
                    string username = dal.GetUserNameByCode(usercode);
                    ret = username + " 否决";
                }
                else if (record.RdState == 2)
                {
                    ret = "审批通过";
                }
                else if (record.RdState == 0)
                {
                    ret = "未提交";
                }
                else
                {//0 等待
                    IList<WorkFlowRecords> list = record.RecordList;
                    string userName = "";
                    string checkType = "";

                    var tempRecords = from records in list
                                      where records.RdState == 1
                                      select records;
                    foreach (WorkFlowRecords s in tempRecords)
                    {
                        checkType = s.CheckType;
                        userName += dal.GetUserNameByCode(s.CheckUser) + ",";
                    }

                    if (userName.Length > 0)
                    {
                        ret = userName.Substring(0, userName.Length - 1) + " 审批中";
                    }

                }
            }
            return ret;
        }
        /// <summary>
        /// 审批否决
        /// </summary>
        /// <param name="billcode"></param>
        /// <param name="usercode"></param>
        /// <param name="mind"></param>
        public void DisAgree(string billcode, string usercode, string mind)
        {
            WorkFlowRecord record = GetWFRecordByBill(billcode);
            IList<WorkFlowRecords> list = record.RecordList;
            var temp = from lin in list
                       where lin.RdState == 1
                       select lin;
            //判断驳回的人是否是正在执行的
            int count = temp.Count(p => p.CheckUser == usercode);
            if (count > 0)//如果是大于0说明是正在执行的
            {
                foreach (WorkFlowRecords records in temp)
                {
                    records.FinalUser = usercode;
                    records.Mind = mind;
                    records.RdState = 3;
                }
            }
            else
            { //等于0说明是审批通过了又想驳回的

                //1将审批通过了的意见改成是驳回
                int stepid = temp.FirstOrDefault().StepId;
                var temp_YiTongGuo = from lin in list
                                     where lin.StepId == (stepid - 1) && lin.CheckUser == usercode
                                     select lin;
                foreach (WorkFlowRecords records in temp_YiTongGuo)
                {
                    records.Mind = mind;
                    records.RdState = 3;
                }
                //2将正在执行的也就是下一位审批人的状态变成是等待  

                foreach (WorkFlowRecords records in temp)
                {
                    records.RdState = 0;
                }
            }
            record.RdState = 3;
            record.RecordList = list;
            dal.InsertRecord(record);
            //将审批驳回的动作记录下来
            dal.InsertDisAgreeMsg(billcode, usercode, mind);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordid"></param>
        /// <param name="stepid"></param>
        /// <returns></returns>
        public bool DisAgreeToSpecial(string billCode, string mind, string usercode, string recordid, string stepid)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    string sql1 = "update  workflowrecords  set finaluser=null , rdstate='0',checkdate=null where recordid=@recordid1  and stepid>@stepid1";
                    string sql2 = "update  workflowrecords  set finaluser=null, rdstate='1',checkdate=null where recordid=@recordid2 and  stepid=@stepid2";
                    SqlParameter[] parm1 = {
                                             new SqlParameter("@recordid1",recordid),
                                             new SqlParameter("@stepid1",stepid)};
                    SqlParameter[] parm2 = {
                                             new SqlParameter("@recordid2",recordid),
                                             new SqlParameter("@stepid2",stepid)};
                    DataHelper.ExcuteNonQuery(sql1, tran, parm1, false);
                    DataHelper.ExcuteNonQuery(sql2, tran, parm2, false);
                    dal.InsertDisAgreeMsg(billCode, usercode, mind);
                    tran.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="billcode"></param>
        /// <param name="usercode"></param>
        /// <param name="mind"></param>
        public void Next(string billcode, string usercode, string mind)
        {


            WorkFlowRecord record = GetWFRecordByBill(billcode);
            IList<WorkFlowRecords> records = record.RecordList;
            //当前用户 当前billcode 等待的行数
            int count = (from reds in records where reds.RdState == 1 && reds.CheckUser == usercode select reds).Count();
            if (count <= 0)
            {
                return;
            }
            //验证结束

            var approve = from linreds in records
                          where linreds.RdState == 1
                          select linreds;
            int stepid = approve.First().StepId;
            var nextreds = from linnext in records
                           where linnext.StepId == stepid + 1
                           select linnext;
            int approve2 = (from lin2 in records
                            where lin2.RdState == 1 && lin2.CheckType == "2"
                            select lin2).Count();

            foreach (WorkFlowRecords app in approve)
            {
                //单签的,或者审批编号是当前审批人的修改记录
                if (app.CheckType == "1" || app.CheckUser == usercode)
                {
                    app.RdState = 2;
                    app.FinalUser = usercode;
                    app.Mind = mind;
                    app.CheckDate = DateTime.Now;
                }
            }
            //多人审批会签的,最后一步的,不进入下一个审批流,
            if (approve2 < 2 && nextreds.Count() > 0)
            {
                foreach (WorkFlowRecords next in nextreds)
                {
                    next.RdState = 1;
                }
                record.RecordList = records;
                dal.InsertRecord(record);
            }

            int isEnd = (from linreds in records
                         where linreds.RdState == 1 || linreds.RdState == 0
                         select linreds).Count();
            //下一步
            if (isEnd > 0)
            {
                record.RecordList = records;
                dal.InsertRecord(record);
            }
            //最后一步
            else
            {
                MainDal mainDal = new MainDal();
                Bill_Main mainModel = mainDal.GetMainByCode(billcode);

                string loancode = "";
                string zt = "";
                if (record.FlowId == "hksq")
                {
                    loancode = new T_LoanListDal().GetMainCode(billcode);
                    T_LoanList loan = new T_LoanListDal().GetModel(loancode);
                    decimal ycj = string.IsNullOrEmpty(loan.NOTE3) ? 0 : Convert.ToDecimal(loan.NOTE3);
                    if (loan.LoanMoney <= ycj + mainModel.BillJe)
                    {
                        zt = "2";
                    }
                }

                using (SqlConnection conn = new SqlConnection(DataHelper.constr))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    try
                    {
                        record.RdState = 2;
                        record.IsEdit = 1;
                        //Models.Bill_Main mainModel;
                        bool isdz = new Bll.ConfigBLL().GetValueByKey("dz_syys_flg").Equals("1");//判断是否是大智
                        if (record.FlowId == "gkbx")
                        {
                            mainDal.ApproveMainForGk(record.BillCode, tran);
                            //mainModel = new Models.Bill_Main();
                        }
                        else
                        {
                            mainDal.ApproveMain(record.BillCode, tran);
                            //mainModel = new Bll.Bills.BillMainBLL().GetModel(billcode); ||(&&)
                            if (record.FlowId == "tfsq" || (isdz && ((record.FlowId == "ybbx" || record.FlowId == "yksq_dz"))))
                            {
                                mainDal.ApproveMainForGk(record.BillCode, tran);
                            }
                        }

                        dal.InsertRecord(record, tran);
                        if (record.FlowId.Equals("hksq"))
                        {
                            //删除 修改主表已还款金额
                            new T_LoanListDal().UpdateYhkje(loancode, mainModel.BillJe, zt, tran);
                            new T_LoanListDal().UpdateZT(billcode, tran);
                        }


                        tran.Commit();

                        #region 如果有对应的用款申请单，将对应用款申请金额改成报销金额   大智专用
                        if (isdz && record.FlowId == "yksq_dz")
                        {
                            string strexesql = @"exec [ykdje_dz] '" + billcode + "'";
                            DataHelper.ExcuteNonQuery(strexesql, null, false);
                        }

                        #endregion
                        #region 判断如果是维修申请wxsq 则最后一步自动生成维修记录
                        //if (record.FlowId.Equals("wxsq"))
                        //{
                        //    List<Models.ZiChan_WeiXiuShenQing> lstmodelWeiXiuShenQing = (List<Models.ZiChan_WeiXiuShenQing>)new Bll.Zichan.ZiChan_WeiXiuShenQingBLL().GetListModel(billcode);
                        //    for (int i = 0; i < lstmodelWeiXiuShenQing.Count; i++)
                        //    {
                        //        Models.ZiChan_WeiXiuRiZhi modelWeiXiuJiLu = new Models.ZiChan_WeiXiuRiZhi();
                        //        modelWeiXiuJiLu.BeiZhu = "维修申请单转为维修记录";
                        //        modelWeiXiuJiLu.ShenPiDanCode = billcode;
                        //        modelWeiXiuJiLu.ShiFouShenPi = "1";
                        //        modelWeiXiuJiLu.WeiXiuBuMenCode = mainModel.BillDept;
                        //        modelWeiXiuJiLu.WeiXiuJinE = lstmodelWeiXiuShenQing[i].YuJiJinE;
                        //        modelWeiXiuJiLu.WeiXiuRenCode = mainModel.BillUser;
                        //        modelWeiXiuJiLu.WeiXiuTypeCode = lstmodelWeiXiuShenQing[i].WeiXiuTypeCode;
                        //        modelWeiXiuJiLu.XiTongShiJian = Convert.ToDateTime(mainModel.BillDate).ToString("yyyy-MM-dd");
                        //        modelWeiXiuJiLu.ZiChanCode = lstmodelWeiXiuShenQing[i].ZiChanCode;
                        //        new Bll.Zichan.ZiChan_WeiXiuRiZhiBll().Add(modelWeiXiuJiLu);
                        //    }
                        //}
                        #endregion



                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 撤销提交(删除审批流)  用于归口报销单
        /// </summary>
        /// <param name="billcode">单据编号</param>
        /// <returns>是否能够撤销(已有人审批过将不能删除)</returns>
        public bool ReplaceForGk(string billcode)
        {
            WorkFlowRecord record = GetWFRecordByBill(billcode);
            int cont = (from isapprov in record.RecordList
                        where isapprov.RdState == 2
                        select isapprov).Count();
            int unapp = (from isapprov in record.RecordList
                         where isapprov.RdState == 3
                         select isapprov).Count();
            if (cont > 0 && unapp < 1)
            {
                return false;
            }
            else
            {
                MainDal mainDal = new MainDal();
                using (SqlConnection conn = new SqlConnection(DataHelper.constr))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    try
                    {
                        mainDal.ReplaceMainForGk(record.BillCode, tran);
                        dal.DeleteRecord(record.RecordId, tran);
                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                    }
                }
            }
        }
        /// <summary>
        /// 撤销提交(删除审批流)
        /// </summary>
        /// <param name="billcode">单据编号</param>
        /// <returns>是否能够撤销(已有人审批过将不能删除)</returns>
        public bool Replace(string billcode)
        {
            WorkFlowRecord record = GetWFRecordByBill(billcode);
            int cont = (from isapprov in record.RecordList
                        where isapprov.RdState == 2
                        select isapprov).Count();
            int unapp = (from isapprov in record.RecordList
                         where isapprov.RdState == 3
                         select isapprov).Count();
            if (cont > 0 && unapp < 1)
            {
                return false;
            }
            else
            {
                MainDal mainDal = new MainDal();
                using (SqlConnection conn = new SqlConnection(DataHelper.constr))
                {
                    conn.Open();
                    SqlTransaction tran = conn.BeginTransaction();
                    try
                    {
                        mainDal.ReplaceMain(record.BillCode, tran);
                        dal.DeleteRecord(record.RecordId, tran);
                        tran.Commit();
                        return true;
                    }
                    catch (Exception e)
                    {
                        tran.Rollback();
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 获取审批记录
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="type">单据类型</param>
        /// <returns></returns>
        public IList<string> GetAppBill(string usercode, string type)
        {
            return dal.GetAppBill(usercode, type);
        }

        /// <summary>
        /// 获取审批记录
        /// </summary>
        /// <param name="usercode">用户编号</param>
        /// <param name="type">单据类型</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public IList<string> GetAppBill(string usercode, string type, string status)
        {
            return dal.GetAppBill(usercode, type, status);
        }
        /// <summary>
        /// 获取正在执行状态的审批单子
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="type">单据类型</param>
        /// <returns></returns>
        public int GetAppraveCount(string userCode, string type)
        {
            return dal.GetAppraveCount(userCode, type);
        }


    }
}
