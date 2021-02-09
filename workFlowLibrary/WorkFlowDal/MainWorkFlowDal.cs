using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorkFlowLibrary.WorkFlowModel;
using System.Data.SqlClient;
using System.Data;
using Dal;

namespace WorkFlowLibrary.WorkFlowDal
{
    public class MainWorkFlowDal
    {
        string sqlToMain = " select * from mainworkflow ";
        string sqlToBillToWork = " select * from billtoworkflow ";
        string sqlStep = " select * from workflowstep ";

        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public IList<BillToWorkFlow> GetBillAll()
        {
            string sql = "select * from billtoworkflow";
            DataTable dt = DataHelper.GetDataTable(sql, null, false);
            IList<BillToWorkFlow> list = new List<BillToWorkFlow>();
            foreach (DataRow dr in dt.Rows)
            {
                BillToWorkFlow btf = new BillToWorkFlow();
                btf.BillName = Convert.ToString(dr["billname"]);
                btf.BillType = Convert.ToString(dr["billtype"]);
                btf.FlowId = Convert.ToString(dr["flowid"]);
                list.Add(btf);
            }
            return list;
        }
        /// <summary>
        /// 取得审批流的步骤
        /// </summary>
        /// <param name="flowId">审批流编号</param>
        /// <returns></returns>
        private IList<WorkFlowStep> GetStepList(string flowId)
        {
            SqlParameter[] sps = {
                       new SqlParameter("@flowid",flowId)
                     };
            DataTable dtToStep = DataHelper.GetDataTable(sqlStep + " where flowid = @flowid order by stepid ", sps, false);

            IList<WorkFlowStep> list = new List<WorkFlowStep>();
            foreach (DataRow dr in dtToStep.Rows)
            {
                WorkFlowStep wfs = new WorkFlowStep();
                wfs.FlowId = Convert.ToString(dr["FlowId"]);
                wfs.CheckCode = Convert.ToString(SetDBNull(dr["CheckCode"]));
                wfs.MaxDate = Convert.ToDateTime(SetDBNull(dr["MaxDate"]));
                wfs.MaxMoney = Convert.ToDecimal(SetDBNull(dr["MaxMoney"]));
                wfs.MinDate = Convert.ToDateTime(SetDBNull(dr["MinDate"]));
                wfs.MinMoney = Convert.ToDecimal(SetDBNull(dr["MinMoney"]));
                wfs.StepId = Convert.ToInt32(dr["StepId"]);
                wfs.StepText = Convert.ToString(SetDBNull(dr["StepText"]));
                wfs.StepType = Convert.ToString(dr["StepType"]);
                wfs.CheckType = Convert.ToString(dr["CheckType"]);
                wfs.IsKmZg = Convert.ToString(dr["filterkemuManager"]);
                wfs.Memo = Convert.ToString(dr["memo"]);
                wfs.kmType = Convert.ToString(dr["kmType"]);
                list.Add(wfs);
            }
            return list;
        }
        /// <summary>
        /// 取得审批流的步骤
        /// </summary>
        /// <param name="flowId">审批流编号</param>
        /// <returns></returns>
        private IList<WorkFlowStep> GetStepList(string flowId, string strdeptcode)
        {

            List<SqlParameter> lstParameter = new List<SqlParameter>();
            sqlStep += " where 1=1";
            if (!string.IsNullOrEmpty(flowId))
            {
                sqlStep += " and flowid=@flowid";
                lstParameter.Add(new SqlParameter("@flowid", flowId));

            }

            if (!string.IsNullOrEmpty(strdeptcode))
            {
                sqlStep += " and memo=@memo";
                lstParameter.Add(new SqlParameter("@memo", strdeptcode));
            }
            //SqlParameter[] sps = {
            //           new SqlParameter("@flowid",flowId),
            //            new SqlParameter("@meno",strdeptcode)
            //         };

            // string strsql = sqlStep + " where flowid = @flowid and meno=@meno order by stepid";
            DataTable dtToStep = DataHelper.GetDataTable(sqlStep, lstParameter.ToArray(), false);

            IList<WorkFlowStep> list = new List<WorkFlowStep>();
            foreach (DataRow dr in dtToStep.Rows)
            {
                WorkFlowStep wfs = new WorkFlowStep();
                wfs.FlowId = Convert.ToString(dr["FlowId"]);
                wfs.CheckCode = Convert.ToString(SetDBNull(dr["CheckCode"]));
                wfs.MaxDate = Convert.ToDateTime(SetDBNull(dr["MaxDate"]));
                wfs.MaxMoney = Convert.ToDecimal(SetDBNull(dr["MaxMoney"]));
                wfs.MinDate = Convert.ToDateTime(SetDBNull(dr["MinDate"]));
                wfs.MinMoney = Convert.ToDecimal(SetDBNull(dr["MinMoney"]));
                wfs.StepId = Convert.ToInt32(dr["StepId"]);
                wfs.StepText = Convert.ToString(SetDBNull(dr["StepText"]));
                wfs.StepType = Convert.ToString(dr["StepType"]).Trim();
                wfs.CheckType = Convert.ToString(dr["CheckType"]);
                wfs.IsKmZg = Convert.ToString(dr["filterkemuManager"]);
                wfs.Memo = Convert.ToString(dr["memo"]);
                wfs.kmType = Convert.ToString(dr["kmType"]);
                list.Add(wfs);
            }
            return list;
        }

        /// <summary>
        /// 取得该审批流生效单据类型
        /// </summary>
        /// <param name="flowid">审批流编号</param>
        /// <returns></returns>
        private IList<string> GetBillTypeList(string flowid)
        {
            SqlParameter[] sps = {
                                   new SqlParameter("@flowid",flowid)
                                 };
            DataTable dtToBill = DataHelper.GetDataTable(sqlToBillToWork + " where flowid=@flowid", sps, false);

            IList<string> list = new List<string>();
            foreach (DataRow dr in dtToBill.Rows)
            {
                list.Add(Convert.ToString(dr["billtype"]));
            }
            return list;
        }

        /// <summary>
        /// 根据审批流编号获得审批流
        /// </summary>
        /// <param name="flowid">审批流编号</param>
        /// <returns></returns>
        public MainWorkFlow GetWorkFlow(string flowid, string dept)
        {
            string sql = sqlToMain + " where flowid=@flowid ";
            SqlParameter[] sps = {
                                   new SqlParameter("@flowid",flowid)
                                 };
            MainWorkFlow wf = new MainWorkFlow();
            wf.FlowId = flowid;
            //取审批流主表
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    wf.FlowName = Convert.ToString(dr["flowname"]);
                }
                else
                {
                    return null;
                }
            }
            string strDepworkflow = server.GetCellValue("select avalue from dbo.t_Config where akey='Depworkflow'");
            if (!string.IsNullOrEmpty(strDepworkflow) && strDepworkflow == "Y")
            {
                wf.StepList = GetStepList(flowid, dept);

            }
            else
            {
                wf.StepList = GetStepList(flowid);
            }
            if (wf.StepList.Count() > 0)
            {
                wf.BillType = GetBillTypeList(flowid);
                return wf;
            }
            else
            {
                return null;
            }




        }
        /// <summary>
        /// 根据审批流编号获得审批流
        /// </summary>
        /// <param name="flowid">审批流编号</param>
        /// <returns></returns>
        //public MainWorkFlow GetWorkFlow(string flowid, string strdeptcode)
        //{
        //    string sql = sqlToMain + " where flowid=@flowid ";
        //    SqlParameter[] sps = {
        //                           new SqlParameter("@flowid",flowid)
        //                         };
        //    MainWorkFlow wf = new MainWorkFlow();

        //    //取审批流主表
        //    using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
        //    {
        //        if (dr.Read())
        //        {
        //            wf.FlowName = Convert.ToString(dr["flowname"]);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }

        //    wf.StepList = GetStepList(flowid, strdeptcode);
        //    wf.BillType = GetBillTypeList(flowid);
        //    return wf;

        //}
        /// <summary>
        /// 新增审批流
        /// </summary>
        /// <param name="newwf">要新增的审批流对象</param>
        public void InsertWF(MainWorkFlow newwf)
        {
            using (SqlConnection con = new SqlConnection(DataHelper.constr))
            {
                SqlTransaction trans = con.BeginTransaction();
                try
                {
                    //插入审批主表
                    string insertToMain = " insert into mainworkflow (flowid,flowname) values(@flowid,@flowname) ";
                    SqlParameter[] spsToMain = {
                                               new SqlParameter("@flowid",newwf.FlowId),
                                               new SqlParameter("@flowname",newwf.FlowName)
                                           };
                    DataHelper.ExcuteNonQuery(insertToMain, trans, spsToMain, false);

                    //插入单据类型表
                    foreach (string billType in newwf.BillType)
                    {
                        string sqlToBill = " insert into billtoworkflow(flowid,billtype) values(@flowid,@billtype) ";
                        SqlParameter[] spsToBill = {
                                               new SqlParameter("@flowid",newwf.FlowId),
                                               new SqlParameter("@billtype",billType)
                                           };
                        DataHelper.ExcuteNonQuery(sqlToBill, trans, spsToBill, false);
                    }

                    //插入子表
                    foreach (WorkFlowStep wfs in newwf.StepList)
                    {
                        string sqlToStep = @" insert into workflowstep(flowid,stepid ,steptype,steptext,checkcode,minmoney,maxmoney,mindate,maxdate,checktype,filterkemuManager,kmType)
                                         values(@flowid,@stepid ,@steptype,@steptext,@checkcode,@minmoney,@maxmoney,@mindate,@maxdate,@checktype,@filterkemuManager,@kmType) ";
                        SqlParameter[] spsToStep = {
                                               new SqlParameter("@flowid",wfs.FlowId),
                                               new SqlParameter("@stepid",wfs.StepId),
                                               new SqlParameter("@steptype",wfs.StepType),
                                               new SqlParameter("@steptext",wfs.StepText),
                                               new SqlParameter("@checkcode",wfs.CheckCode),
                                               new SqlParameter("@minmoney",wfs.MinMoney),
                                               new SqlParameter("@maxmoney",wfs.MaxMoney),
                                               new SqlParameter("@mindate",wfs.MinDate),
                                               new SqlParameter("@maxdate",wfs.MaxDate),
                                               new SqlParameter("@checktype",wfs.CheckType),
                                               new SqlParameter("@filterkemuManager",wfs.IsKmZg),
                                               new SqlParameter("@kmType",wfs.kmType)
                                           };
                        DataHelper.ExcuteNonQuery(sqlToStep, trans, spsToStep, false);
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 修改审批流
        /// </summary>
        /// <param name="newwf"></param>
        public void EditWF(MainWorkFlow newwf)
        {
            using (SqlConnection con = new SqlConnection(DataHelper.constr))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();
                try
                {
                    //删除子表
                    string deleteToMain = " delete workflowstep where flowid=@flowid ";
                    SqlParameter[] spsToMain = {
                                               new SqlParameter("@flowid",newwf.FlowId),                                               
                                           };
                    DataHelper.ExcuteNonQuery(deleteToMain, trans, spsToMain, false);

                    //插入子表
                    foreach (WorkFlowStep wfs in newwf.StepList)
                    {
                        string sqlToStep = @" insert into workflowstep(flowid,stepid ,steptype,steptext,checkcode,minmoney,maxmoney,mindate,maxdate,checktype,filterkemuManager,kmType,memo)
                                         values(@flowid,@stepid ,@steptype,@steptext,@checkcode,@minmoney,@maxmoney,@mindate,@maxdate,@checktype,@filterkemuManager,@kmType,@memo) ";
                        SqlParameter[] spsToStep = {
                                               new SqlParameter("@flowid",wfs.FlowId),
                                               new SqlParameter("@stepid",wfs.StepId),
                                               new SqlParameter("@steptype",wfs.StepType),
                                               new SqlParameter("@steptext",SqlNull(wfs.StepText)),
                                               new SqlParameter("@checkcode",SqlNull(wfs.CheckCode)),
                                               new SqlParameter("@minmoney",SqlNull(wfs.MinMoney)),
                                               new SqlParameter("@maxmoney",SqlNull(wfs.MaxMoney)),
                                               new SqlParameter("@mindate",SqlNull(wfs.MinDate)),
                                               new SqlParameter("@maxdate",SqlNull(wfs.MaxDate)),
                                               new SqlParameter("@checktype",wfs.CheckType),
                                               new SqlParameter("@filterkemuManager",wfs.IsKmZg),
                                               new SqlParameter("@kmType",wfs.kmType),
                                                new SqlParameter("@memo",wfs.Memo)
                                           };
                        DataHelper.ExcuteNonQuery(sqlToStep, trans, spsToStep, false);
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newwf"></param>
        /// <param name="billdept"></param>
        public void EditWF(MainWorkFlow newwf, string billdept)
        {
            using (SqlConnection con = new SqlConnection(DataHelper.constr))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();
                try
                {
                    //删除子表
                    string deleteToMain = " delete workflowstep where flowid=@flowid and memo=@memo";
                    SqlParameter[] spsToMain = {
                                               new SqlParameter("@flowid",newwf.FlowId),                                               
                                               new SqlParameter("@memo",billdept)
                                           };
                    DataHelper.ExcuteNonQuery(deleteToMain, trans, spsToMain, false);

                    //插入子表 ,mindate,maxdate ,@mindate,@maxdate
                    foreach (WorkFlowStep wfs in newwf.StepList)
                    {
                        string sqlToStep = @" insert into workflowstep(flowid,stepid ,steptype,steptext,checkcode,minmoney,maxmoney,checktype,filterkemuManager,memo,kmType)
                                         values(@flowid,@stepid ,@steptype,@steptext,@checkcode,@minmoney,@maxmoney,@checktype,@filterkemuManager,@memo,@kmType) ";
                        SqlParameter[] spsToStep = {
                                               new SqlParameter("@flowid",wfs.FlowId),
                                               new SqlParameter("@stepid",wfs.StepId),
                                               new SqlParameter("@steptype",wfs.StepType),
                                               new SqlParameter("@steptext",SqlNull(wfs.StepText)),
                                               new SqlParameter("@checkcode",SqlNull(wfs.CheckCode)),
                                               new SqlParameter("@minmoney",SqlNull(wfs.MinMoney)),
                                               new SqlParameter("@maxmoney",SqlNull(wfs.MaxMoney)),
                                               //new SqlParameter("@mindate",SqlNull(wfs.MinDate)),
                                               //new SqlParameter("@maxdate",SqlNull(wfs.MaxDate)),
                                               new SqlParameter("@checktype",wfs.CheckType),
                                               new SqlParameter("@filterkemuManager",wfs.IsKmZg),
                                               new SqlParameter("@memo",billdept),
                                               new SqlParameter("@kmType",wfs.kmType)
                                           };
                        DataHelper.ExcuteNonQuery(sqlToStep, trans, spsToStep, false);
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw e;
                }
            }
        }
        /// <summary>
        /// 根据单据类型取得审批流
        /// </summary>
        /// <param name="billType"></param>
        /// <returns></returns>
        public MainWorkFlow GetWorkFlowByBillType(string billType, string dept)
        {
            string sql = "select * from billtoworkflow where billtype=@billtype ";
            SqlParameter[] sps = {
                                   new SqlParameter("@billtype",billType)
                                 };
            string flowId;
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    flowId = Convert.ToString(dr["flowid"]);
                    return GetWorkFlow(flowId, dept);
                }
                else
                {
                    return null;
                }
            }
        }
        //public MainWorkFlow GetWorkFlowByBillType(string billType)
        //{
        //    string sql = "select * from billtoworkflow where billtype=@billtype ";
        //    SqlParameter[] sps = {
        //                           new SqlParameter("@billtype",billType)
        //                         };
        //    string flowId;
        //    using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
        //    {
        //        if (dr.Read())
        //        {
        //            flowId = Convert.ToString(dr["flowid"]);
        //            return GetWorkFlow(flowId);
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}


        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }

        private object SetDBNull(object obj)
        {
            if (obj.GetType().Name == "DBNull")
            {
                return null;
            }
            else
            {
                return obj;
            }
        }


    }
}
