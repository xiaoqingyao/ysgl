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
    public class WorkFlowRecordDal
    {
        /// <summary>
        /// 插入,更新审批记录
        /// </summary>
        /// <param name="record"></param>
        public void InsertRecord(WorkFlowRecord record)
        {
            using (SqlConnection con = new SqlConnection(DataHelper.constr))
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    InsertRecord(record, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }

        
        public void DeleteRecord(int recordid, SqlTransaction tran)
        {
            string delMainSql = @"delete workflowrecord where recordid=@recordid";
            string delChildSql = @"delete workflowrecords where recordid=@recordid";
            SqlParameter[] spId = { new SqlParameter("@recordid", recordid) };
            DataHelper.ExcuteNonQuery(delMainSql, tran, spId, false);
            DataHelper.ExcuteNonQuery(delChildSql, tran, spId, false);
        }

        public void InsertRecord(WorkFlowRecord record, SqlTransaction tran)
        {
            string delMainSql = @"delete workflowrecord where recordid=@recordid";
            string delChildSql = @"delete workflowrecords where recordid=@recordid";

            string sql = @"insert into workflowrecord(billCode,billType,flowId,isEdit,rdState)
                         values(@billCode,@billType,@flowId,@isEdit,@rdState)";
            try
            {
                SqlParameter[] bill = { new SqlParameter("@billcode", record.BillCode) };
                int recordid = Convert.ToInt32(DataHelper.ExecuteScalar("select recordid from workflowrecord where billcode=@billcode and rdstate=1", bill, false));
                if (recordid > 0)
                {
                    SqlParameter[] spId = { new SqlParameter("@recordid", recordid) };
                    DataHelper.ExcuteNonQuery(delMainSql, tran, spId, false);
                    DataHelper.ExcuteNonQuery(delChildSql, tran, spId, false);
                }

                SqlParameter[] delsp = { };
                SqlParameter[] sps = {
                                     new SqlParameter("@billCode",SqlNull(record.BillCode)),
                                     new SqlParameter("@billType",SqlNull(record.BillType)),
                                     new SqlParameter("@flowId",SqlNull(record.FlowId)),
                                     new SqlParameter("@isEdit",SqlNull(record.IsEdit)),
                                     new SqlParameter("@rdState",SqlNull(record.RdState))
                                 };
                DataHelper.ExcuteNonQuery(sql, tran, sps, false);

                string sqlToChild = @" insert into workflowrecords(recordid,flowid,stepid,steptext,recordtype,checkuser,finaluser,rdstate,mind,checkdate,checktype)
                                values(IDENT_CURRENT('workflowrecord'),@flowid,@stepid,@steptext,@recordtype,@checkuser,@finaluser,@rdstate,@mind,@checkdate,@checktype) ";


                foreach (WorkFlowRecords records in record.RecordList)
                {
                    SqlParameter[] spsToChild = {
                                     new SqlParameter("@flowid",SqlNull(records.FlowId)),
                                     new SqlParameter("@stepid",SqlNull(records.StepId)),
                                     new SqlParameter("@steptext",SqlNull(records.StepText)),
                                     new SqlParameter("@recordtype",SqlNull(records.RecordType)),
                                     new SqlParameter("@checkuser",SqlNull(records.CheckUser)),
                                     new SqlParameter("@finaluser",SqlNull(records.FinalUser)),
                                     new SqlParameter("@rdstate",SqlNull(records.RdState)),
                                     new SqlParameter("@mind",SqlNull(records.Mind)),
                                     new SqlParameter("@checkdate",SqlNull(records.CheckDate)),
                                     new SqlParameter("@checktype",SqlNull(records.CheckType))
                                 };
                    DataHelper.ExcuteNonQuery(sqlToChild, tran, spsToChild, false);
                }
             
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int GetAppraveCount(string userCode, string type)
        {
            Dal.ConfigDal config = new Dal.ConfigDal();
            string sql = "";
            string isdz = config.GetValueByKey("dz_syys_flg");
            if (isdz == "1" && (type == "ybbx" || type == "yksq_dz" || type == "tfsq"))
            {
                sql = @" select count(distinct billname) from  bill_main where billname in(
                             select billcode from workflowrecord a,workflowrecords b 
								where a.recordid=b.recordid and b.rdstate=1 and
								a.flowid=@type and b.checkuser=@userCode ) ";
            }
            else
            {
                sql = @" select count(*) from  bill_main where billcode in(
                             select billcode from workflowrecord a,workflowrecords b 
								where a.recordid=b.recordid and b.rdstate=1 and
								a.flowid=@type and b.checkuser=@userCode ) ";
            }

            //            string sql = @" select count(*) from workflowrecord a,workflowrecords b 
            //                            where a.recordid=b.recordid and b.rdstate=1 and
            //                            a.flowid=@type and b.checkuser=@userCode ";
            SqlParameter[] sps = { 
                                     new SqlParameter("@userCode", userCode) ,
                                     new SqlParameter("@type", type) 
                                 };
            return Convert.ToInt32(DataHelper.ExecuteScalar(sql, sps, false));
        }

        public WorkFlowRecord GetWFRecordByBill(string billcode)
        {
            string sql = " select * from workflowrecord where billcode=@billcode ";
            SqlParameter[] sps = { new SqlParameter("@billcode", billcode) };
            try
            {
                WorkFlowRecord record = new WorkFlowRecord();
                using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
                {
                    if (dr.Read())
                    {
                        record.RecordId = Convert.ToInt32(dr["RecordId"]);
                        record.BillCode = Convert.ToString(dr["BillCode"]);
                        record.BillType = Convert.ToString(dr["BillType"]);
                        record.FlowId = Convert.ToString(dr["FlowId"]);
                        record.IsEdit = Convert.ToInt32(dr["IsEdit"]);
                        record.RdState = Convert.ToInt32(dr["RdState"]);
                    }
                }
                string sqlToChild = " select * from workflowrecords where recordid =@recordid  order by stepid";
                SqlParameter[] spToChild = { new SqlParameter("@recordid", record.RecordId) };
                DataTable dt = DataHelper.GetDataTable(sqlToChild, spToChild, false);
                IList<WorkFlowRecords> list = new List<WorkFlowRecords>();
                foreach (DataRow dr in dt.Rows)
                {
                    WorkFlowRecords records = new WorkFlowRecords();
                    records.Recordid = Convert.ToInt32(dr["RecordId"]);

                    if (dr["CheckDate"].GetType().Name != "DBNull")
                    {
                        records.CheckDate = Convert.ToDateTime(dr["CheckDate"]);
                    }
                    else
                    {
                        records.CheckDate = null;
                    }

                    records.CheckUser = Convert.ToString(SetDBNull(dr["CheckUser"]));
                    records.FinalUser = Convert.ToString(SetDBNull(dr["FinalUser"]));
                    records.FlowId = Convert.ToString(SetDBNull(dr["FlowId"]));
                    records.Mind = Convert.ToString(SetDBNull(dr["Mind"]));
                    records.RdState = Convert.ToInt32(SetDBNull(dr["RdState"]));
                    records.RecordType = Convert.ToString(SetDBNull(dr["RecordType"]));
                    records.StepId = Convert.ToInt32(dr["StepId"]);
                    records.StepText = Convert.ToString(SetDBNull(dr["StepText"]));
                    records.CheckType = Convert.ToString(SetDBNull(dr["checktype"]));
                    if (records.RdState == 2 && records.FinalUser != records.CheckUser)
                    {
                        continue;
                    }
                    list.Add(records);
                }
                record.RecordList = list;
                return record;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string GetUserNameByCode(string code)
        {
            string sql = " select userName from bill_users where userCode =@usercode ";
            SqlParameter[] sps = { new SqlParameter("@usercode", code) };
            object ret = DataHelper.ExecuteScalar(sql, sps, false);
            return Convert.ToString(ret);
        }

        public IList<string> GetAppBill(string usercode, string type)
        {
            string sql = @"select billcode from workflowrecord a,workflowrecords b 
                         where billtype=@type and  a.recordid=b.recordid 
                         and b.rdstate=1 and checkuser=@usercode";
            SqlParameter[] spsToChild = {
                                             new SqlParameter("@usercode",usercode),
                                             new SqlParameter("@type",type)
                                        };
            DataTable dt = DataHelper.GetDataTable(sql, spsToChild, false);
            IList<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string billcode = Convert.ToString(dr["billcode"]);
                list.Add(billcode);
            }
            return list;
        }
        public IList<string> GetAppBill(string usercode, string type, string status)
        {
            string sql = @"select billcode from workflowrecord a,workflowrecords b 
                         where billtype=@type and  a.recordid=b.recordid 
                         and b.rdstate=@status and checkuser=@usercode";
            SqlParameter[] spsToChild = {
                                             new SqlParameter("@usercode",usercode),
                                             new SqlParameter("@type",type),
                                              new SqlParameter("@status",status)
                                        };
            DataTable dt = DataHelper.GetDataTable(sql, spsToChild, false);
            IList<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string billcode = Convert.ToString(dr["billcode"]);
                list.Add(billcode);
            }
            return list;
        }

        public void InsertDisAgreeMsg(string billcode, string usercode, string mind)
        {
            string sql = @"insert into bill_ReturnHistory(billcode,usercode,mind,dt) values(@billcode,@usercode,@mind,getdate())";
            SqlParameter[] sps = {
                                             new SqlParameter("@usercode",usercode),
                                             new SqlParameter("@billcode",billcode),
                                              new SqlParameter("@mind",mind)
                                        };
            DataHelper.ExcuteNonQuery(sql, sps, false);
        }

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
