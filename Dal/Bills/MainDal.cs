using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections;

namespace Dal.Bills
{
    public class MainDal
    {
        public int InsertMain(Bill_Main main, SqlTransaction tran)
        {
            string sql = @"insert into bill_main( billCode, billName, flowID, stepID, billUser, billDate, billDept, billJe, loopTimes, billType, billName2,isGk,gkDept,dydj,note1,note2,note3,note4,note5) values
                           (@billCode, @billName, @flowID, @stepID, @billUser, @billDate, @billDept, @billJe, @loopTimes, @billType, @billName2,@isGk,@gkDept,@dydj,@note1,@note2,@note3,@note4,@note5)";
            SqlParameter[] sps = { 
                                             new SqlParameter("@billCode",SqlNull(main.BillCode)),
                                             new SqlParameter("@billName",SqlNull(main.BillName)),
                                             new SqlParameter("@flowID",SqlNull(main.FlowId)),
                                             new SqlParameter("@stepID",SqlNull(main.StepId)),
                                             new SqlParameter("@billUser",SqlNull(main.BillUser)),
                                             new SqlParameter("@billDate",SqlNull(main.BillDate)),
                                             new SqlParameter("@billDept",SqlNull(main.BillDept)),
                                             new SqlParameter("@billJe",SqlNull(main.BillJe)),
                                             new SqlParameter("@loopTimes",SqlNull(main.LoopTimes)),
                                             new SqlParameter("@billType",SqlNull(main.BillType)),
                                             new SqlParameter("@billName2",SqlNull(main.BillName2)),
                                             new SqlParameter("@isGk",SqlNull(main.IsGk)),
                                             new SqlParameter("@gkDept",SqlNull(main.GkDept)),
                                             new SqlParameter("@dydj",SqlNull(main.Dydj)),
                                             new SqlParameter("@note1",SqlNull(main.Note1)),
                                             new SqlParameter("@note2",SqlNull(main.Note2)),
                                             new SqlParameter("@note3",SqlNull(main.Note3)),
                                             new SqlParameter("@note4",SqlNull(main.Note4)),
                                             new SqlParameter("@note5",SqlNull(main.Note5))
                                         };

            return DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        public int InsertMain(Bill_Main main)
        {
            string sql = @"insert into bill_main( billCode, billName, flowID, stepID, billUser, billDate, billDept, billJe, loopTimes, billType, billName2,isGk,gkDept,dydj,note1,note2,note3,note4,note5) values
                           (@billCode, @billName, @flowID, @stepID, @billUser, @billDate, @billDept, @billJe, @loopTimes, @billType, @billName2,@isGk,@gkDept,@dydj,@note1,@note2,@note3,@note4,@note5)";
            SqlParameter[] sps = { 
                                             new SqlParameter("@billCode",SqlNull(main.BillCode)),
                                             new SqlParameter("@billName",SqlNull(main.BillName)),
                                             new SqlParameter("@flowID",SqlNull(main.FlowId)),
                                             new SqlParameter("@stepID",SqlNull(main.StepId)),
                                             new SqlParameter("@billUser",SqlNull(main.BillUser)),
                                             new SqlParameter("@billDate",SqlNull(main.BillDate)),
                                             new SqlParameter("@billDept",SqlNull(main.BillDept)),
                                             new SqlParameter("@billJe",SqlNull(main.BillJe)),
                                             new SqlParameter("@loopTimes",SqlNull(main.LoopTimes)),
                                             new SqlParameter("@billType",SqlNull(main.BillType)),
                                             new SqlParameter("@billName2",SqlNull(main.BillName2)),
                                             new SqlParameter("@isGk",SqlNull(main.IsGk)),
                                             new SqlParameter("@gkDept",SqlNull(main.GkDept)),
                                              new SqlParameter("@dydj",SqlNull(main.Dydj)),
                                             new SqlParameter("@note1",SqlNull(main.Note1)),
                                             new SqlParameter("@note2",SqlNull(main.Note2)),
                                             new SqlParameter("@note3",SqlNull(main.Note3)),
                                             new SqlParameter("@note4",SqlNull(main.Note4)),
                                             new SqlParameter("@note5",SqlNull(main.Note5))
                                         };

            return DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        public void ApproveMain(string billCode, SqlTransaction tran)
        {
            string sql = @"update bill_main set stepid='end' where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public void ReplaceMain(string billCode, SqlTransaction tran)
        {
            string sql = @"update bill_main set stepid='-1' where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public void ApproveMainForGk(string billCode, SqlTransaction tran)
        {
            string sql = @"update bill_main set stepid='end' where billname=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        public void ReplaceMainForGk(string billname, SqlTransaction tran)
        {
            string sql = @"update bill_main set stepid='-1' where billname=@billname";
            SqlParameter[] sps = { new SqlParameter("@billname", billname) };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public int DeleteMain(string billCode, SqlTransaction tran)
        {
            string sql = @"delete bill_main where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            return DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        public int DeleteMain(string billCode)
        {
            string sql = "delete bill_main where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            return DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        public int DeleteMainByName(string billName)
        {
            string sql = "delete bill_main where billName=@billName";
            SqlParameter[] sps = { new SqlParameter("@billName", billName) };
            return DataHelper.ExcuteNonQuery(sql, sps, false);
        }

        public int DeleteMainByName(string billName, SqlTransaction tran)
        {
            string sql = "delete bill_main where billName=@billName";
            SqlParameter[] sps = { new SqlParameter("@billName", billName) };
            return DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="billCode"></param>
        /// <returns></returns>
        public Bill_Main GetMainByCode(string billCode)
        {
            string sql = "select * from bill_main where billCode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    Bill_Main main = new Bill_Main();
                    main.BillCode = Convert.ToString(dr["BillCode"]);
                    main.BillDate = Convert.ToDateTime(dr["BillDate"]);
                    main.BillDept = Convert.ToString(dr["BillDept"]);
                    main.BillJe = Convert.ToDecimal(dr["BillJe"]);
                    main.BillName = Convert.ToString(dr["BillName"]);
                    main.BillName2 = Convert.ToString(dr["BillName2"]);
                    main.BillType = Convert.ToString(dr["BillType"]);
                    main.BillUser = Convert.ToString(dr["BillUser"]);
                    main.FlowId = Convert.ToString(dr["FlowId"]);
                    main.LoopTimes = Convert.ToInt32(dr["LoopTimes"]);
                    main.StepId = Convert.ToString(dr["StepId"]);
                    main.IsGk = Convert.ToString(dr["IsGk"]);
                    main.GkDept = Convert.ToString(dr["GkDept"]);
                    main.Dydj = Convert.ToString(dr["Dydj"]);
                    main.Note1 = Convert.ToString(dr["Note1"]);
                    main.Note2 = Convert.ToString(dr["Note2"]);
                    main.Note3 = Convert.ToString(dr["Note3"]);
                    main.Note4 = Convert.ToString(dr["Note4"]);
                    main.Note5 = Convert.ToString(dr["Note5"]);
                    return main;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 通过报销单号 billname获取多个报销单（归口分解用到）
        /// </summary>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public IList<Bill_Main> GetMainsByBillName(string billname)
        {
            string sql = "select * from bill_main where billname=@billname";
            SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@billname", billname) };
            DataTable dtrel = DataHelper.GetDataTable(sql, arrsp, false);
            IList<Bill_Main> lstmain = new List<Bill_Main>();
            int idtrows = dtrel.Rows.Count;
            for (int i = 0; i < idtrows; i++)
            {
                DataRow dr = dtrel.Rows[i];
                Bill_Main main = new Bill_Main();
                main.BillCode = Convert.ToString(dr["BillCode"]);
                main.BillDate = Convert.ToDateTime(dr["BillDate"]);
                main.BillDept = Convert.ToString(dr["BillDept"]);
                main.BillJe = Convert.ToDecimal(dr["BillJe"]);
                main.BillName = Convert.ToString(dr["BillName"]);
                main.BillName2 = Convert.ToString(dr["BillName2"]);
                main.BillType = Convert.ToString(dr["BillType"]);
                main.BillUser = Convert.ToString(dr["BillUser"]);
                main.FlowId = Convert.ToString(dr["FlowId"]);
                main.LoopTimes = Convert.ToInt32(dr["LoopTimes"]);
                main.StepId = Convert.ToString(dr["StepId"]);
                main.IsGk = Convert.ToString(dr["IsGk"]);
                main.GkDept = Convert.ToString(dr["GkDept"]);
                main.Dydj = Convert.ToString(dr["Dydj"]);
                main.Note1 = Convert.ToString(dr["Note1"]);
                main.Note2 = Convert.ToString(dr["Note2"]);
                main.Note3 = Convert.ToString(dr["Note3"]);
                main.Note4 = Convert.ToString(dr["Note4"]);
                main.Note5 = Convert.ToString(dr["Note5"]);
                lstmain.Add(main);
            }
            return lstmain;
        }

        /// <summary>
        /// 通过报销单号 billname获取多个报销单（归口分解用到）
        /// </summary>
        /// <param name="billcode"></param>
        /// <returns></returns>
        public IList<Bill_Main> GetMainsByBillNameorCode(string billname)
        {
            string sql = "select * from bill_main where billname=@billname or billcode=@billname";
            SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@billname", billname) };
            DataTable dtrel = DataHelper.GetDataTable(sql, arrsp, false);
            IList<Bill_Main> lstmain = new List<Bill_Main>();
            int idtrows = dtrel.Rows.Count;
            for (int i = 0; i < idtrows; i++)
            {
                DataRow dr = dtrel.Rows[i];
                Bill_Main main = new Bill_Main();
                main.BillCode = Convert.ToString(dr["BillCode"]);
                main.BillDate = Convert.ToDateTime(dr["BillDate"]);
                main.BillDept = Convert.ToString(dr["BillDept"]);
                main.BillJe = Convert.ToDecimal(dr["BillJe"]);
                main.BillName = Convert.ToString(dr["BillName"]);
                main.BillName2 = Convert.ToString(dr["BillName2"]);
                main.BillType = Convert.ToString(dr["BillType"]);
                main.BillUser = Convert.ToString(dr["BillUser"]);
                main.FlowId = Convert.ToString(dr["FlowId"]);
                main.LoopTimes = Convert.ToInt32(dr["LoopTimes"]);
                main.StepId = Convert.ToString(dr["StepId"]);
                main.IsGk = Convert.ToString(dr["IsGk"]);
                main.GkDept = Convert.ToString(dr["GkDept"]);
                main.Dydj = Convert.ToString(dr["Dydj"]);
                main.Note1 = Convert.ToString(dr["Note1"]);
                main.Note2 = Convert.ToString(dr["Note2"]);
                main.Note3 = Convert.ToString(dr["Note3"]);
                main.Note4 = Convert.ToString(dr["Note4"]);
                main.Note5 = Convert.ToString(dr["Note5"]);
                lstmain.Add(main);
            }
            return lstmain;
        }

        private object SqlNull(object obj)
        {
            if (obj == null)
                return DBNull.Value;
            return obj;
        }



        public string GetBillcodeByDeptAndYsgc(string gcbh, string deptcode, string flowid)
        {
            string cxsql = "select billCode from bill_main where flowID=@flowid and billDept=@billDept and billName=@billName ";
            SqlParameter[] paramter = { new SqlParameter("@billDept",deptcode),
                                        new SqlParameter("@billName",gcbh),
                                      new SqlParameter("@flowid",flowid)
                                      };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count > 0)
            {
                //修改
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public string GetBillcodeByDeptAndYsgc(string gcbh, string deptcode, string flowid, string xmcode)
        {
            string cxsql = "select billCode from bill_main where flowID=@flowid and billDept=@billDept and billName=@billName and note3=@xmcode";
            SqlParameter[] paramter = { new SqlParameter("@billDept",deptcode),
                                        new SqlParameter("@billName",gcbh),
                                      new SqlParameter("@flowid",flowid),
                                      new SqlParameter("@xmcode",xmcode)
                                      };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count > 0)
            {
                //修改
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }
        public string GetBillcode(string gcbh, string deptcode, string strgkdept, string strkmbh, string flowid)
        {
            string cxsql = @"select main.billcode from bill_main main,bill_ysmxb mxb where 
                        main.billcode=mxb.billcode and main.flowid=@flowid
                        and main.gkdept=@gkdept and main.billName=@billName and main.billdept=@billDept
                        and mxb.yskm=@yskm    select billCode from bill_main 
                                            where flowID='ys' and billDept=@billDept and billName=@billName and gkdept=@gkdept";
            SqlParameter[] paramter = { new SqlParameter("@billDept",deptcode),
                                        new SqlParameter("@billName",gcbh),
                                        new SqlParameter("@gkdept",strgkdept),
                                         new SqlParameter("@yskm",strkmbh),
                                          new SqlParameter("@flowid",flowid)
                                      };
            DataTable dt = DataHelper.GetDataTable(cxsql, paramter, false);
            if (dt.Rows.Count > 0)
            {
                //修改
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public bool Addtb(IList<Bill_Ysmxb> ysmxb, IList<Bill_Main> MainList, string tblx)
        {
            string delete1;
            if (tblx == "02")
            {
                delete1 = "delete from bill_ysmxb where billCode=@billCode and yskm in (select yskmCode from bill_yskm where tblx=@tblx)"; //财务的时候免得把部门预算的删了
            }
            else
            {
                delete1 = "delete from bill_ysmxb where billCode=@billCode and yskm in (select yskmCode from bill_yskm where tblx='01') ";
            }
            //---- string Setje = "update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billcode=@billcode ) where billCode=@billcode";

            string Setje = "update bill_main set billJe=(select isnull(sum(isnull(ysje,0)),0) from bill_ysmxb where billcode=@billcode and  isnull(yskm,'')!='') where billCode=@billcode";
            string SetBillUserSql = @"update bill_main set billuser=@billuser where billcode=@billcode and 
                                      billname in (select gcbh from bill_ysgc where status = '1' and nian = @nian ) 
                                      and billcode not in (select billcode  from workflowrecord where  billcode = @billcode )";
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    foreach (var i in MainList)
                    {
                        //刚才添加到MainList中的需要删除的删除掉明细
                        if (i.FlowId == "DELETE")
                        {
                            SqlParameter[] delete1par = { new SqlParameter("@billCode", i.BillCode),
                                                          new SqlParameter("@tblx",tblx)};
                            DataHelper.ExcuteNonQuery(delete1, tran, delete1par, false);

                            SqlParameter[] SetUserPar = { new SqlParameter("@billuser",i.BillUser),
                                                          new SqlParameter("@billcode",i.BillCode),
                                                          new SqlParameter("@nian",i.BillName.Substring(0,4))};
                            DataHelper.ExcuteNonQuery(SetBillUserSql, tran, SetUserPar, false);  //需要把制单人修改
                        }
                        else
                        {
                            //如果不需要删除的就添加
                            InsertMain(i);
                        }

                    }
                    Dal.newysgl.bill_ysmxbDal mxdal = new Dal.newysgl.bill_ysmxbDal();
                    //将方法序列好的明细加到明细表中
                    foreach (var i in ysmxb)
                    {
                        mxdal.Add(i, tran);
                    }
                    //将main表中的金额刷上
                    foreach (var i in MainList)
                    {
                        SqlParameter[] paramterje = { new SqlParameter("@billcode", i.BillCode) };
                        DataHelper.ExcuteNonQuery(Setje, tran, paramterje, false);
                    }
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取bill_main对应的预算flowid
        /// </summary>
        /// <param name="yslx"></param>
        /// <returns></returns>
        public string getFlowId(string yslx)
        {
            if (string.IsNullOrEmpty(yslx))
            {
                return "ys";
            }
            return new sqlHelper.sqlHelper().GetCellValue("select note1 from bill_datadic where dictype='18' and diccode='" + yslx + "'");
            //switch (yslx)
            //{
            //    case "01": return "srys";
            //    case "02": return "ys";
            //    case "03": return "zcys";
            //    case "04": return "chys";
            //    case "05": return "wlys";
            //    default: return "ys";
            //}
        }

        /// <summary>
        /// 获取bill_main对应的决算flowid
        /// </summary>
        /// <param name="yslx"></param>
        /// <returns></returns>
        public string getJSFlowId(string yslx)
        {
            string strflowid = "ybbx";
            if (string.IsNullOrEmpty(yslx))
            {
                strflowid = "ybbx";
            }
            else
            {
                string strflow = new sqlHelper.sqlHelper().GetCellValue("select note2 from bill_datadic where dictype='18' and diccode='" + yslx + "'");
                if (!string.IsNullOrEmpty(strflow))
                {
                    strflowid = strflow;
                }
            }
            return strflowid;
            //switch (yslx)
            //{
            //    case "01": return "srys";
            //    case "02": return "ys";
            //    case "03": return "zcys";
            //    case "04": return "chys";
            //    case "05": return "wlys";
            //    default: return "ys";
            //}
        }

        /// <summary>
        /// 获取预算类型
        /// </summary>
        /// <param name="flowid"></param>
        /// <returns></returns>
        public string getYskmType(string flowid)
        {
            if (string.IsNullOrEmpty(flowid))
            {
                return "02";//默认是费用预算
            }
            return new sqlHelper.sqlHelper().GetCellValue("select diccode from bill_datadic where dictype='18' and note1='" + flowid + "'");
        }

        #region  Ph_Add
        public bool Ph_Add(Bill_Main main, Bill_Ybbxmxb ybbx, Bill_Ybbxmxb_Fykm fykm, IList<Bill_Ybbxmxb_Fykm_Dept> hsbmList, IList<Bill_Ybbxmxb_Hsxm> hsxmList)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteMain(main.BillCode, tran);
                    InsertMain(main, tran);
                    AddYbbxmx(ybbx, tran);
                    AddFykm(fykm, tran);
                    AddHsbm(hsbmList, tran);
                    AddHsXm(hsxmList, tran);
                    DeletePh_mainByBillCode(main.BillCode, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

        }


        public bool Ph_AddMx(Bill_Main main, Bill_Ybbxmxb_Fykm fykm, IList<Bill_Ybbxmxb_Fykm_Dept> hsbmList, IList<Bill_Ybbxmxb_Hsxm> hsxmList)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    AddFykm(fykm, tran);
                    AddHsbm(hsbmList, tran);
                    AddHsXm(hsxmList, tran);
                    UpdateMain(main, tran);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

        }


        public void DeletePh_mainByBillCode(string billCode, SqlTransaction tran)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ph_main ");
            strSql.Append(" where billCode=@billCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@billCode", SqlDbType.VarChar,50)			};
            parameters[0].Value = billCode;


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        private void UpdateMain(Bill_Main main, SqlTransaction tran)
        {
            string sql = "update bill_main set billje=billje+@je  where billCode=@billCode and flowId=@flowid ";
            SqlParameter[] parms ={
                     new SqlParameter ("@je",main.BillJe),
                     new SqlParameter ("@billCode",main.BillCode),
                     new SqlParameter ("@flowid",main.FlowId)
        };
            DataHelper.ExcuteNonQuery(sql, tran, parms, false);

        }
        public void AddYbbxmx(Bill_Ybbxmxb model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_ybbxmxb(");
            strSql.Append("billCode,bxr,bxzy,bxsm,ytje,ybje,sfgf,bxmxlx");
            strSql.Append(") values (");
            strSql.Append("@billCode,@bxr,@bxzy,@bxsm,@ytje,@ybje,@sfgf,@bxmxlx");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@billCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bxr", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@bxzy", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@bxsm", SqlDbType.VarChar,2000) ,            
                        new SqlParameter("@ytje", SqlDbType.Float,8) ,            
                        new SqlParameter("@ybje", SqlDbType.Float,8) ,            
                        new SqlParameter("@sfgf", SqlDbType.VarChar,1) ,            
                        new SqlParameter("@bxmxlx", SqlDbType.VarChar,50)             
              
            };

            parameters[0].Value = SqlNull(model.BillCode);

            parameters[1].Value = SqlNull(model.Bxr);

            parameters[2].Value = SqlNull(model.Bxzy);

            parameters[3].Value = SqlNull(model.Bxsm);

            parameters[4].Value = SqlNull(model.Ytje);

            parameters[5].Value = SqlNull(model.Ybje);

            parameters[6].Value = SqlNull(model.Sfgf);

            parameters[7].Value = SqlNull(model.Bxmxlx);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);
        }

        public void AddFykm(Bill_Ybbxmxb_Fykm model, SqlTransaction tran)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into bill_ybbxmxb_fykm(");
            strSql.Append("billCode,fykm,je,mxGuid,status,se");
            strSql.Append(") values (");
            strSql.Append("@billCode,@fykm,@je,@mxGuid,@status,@se");
            strSql.Append(") ");
            SqlParameter[] parameters = {
			            new SqlParameter("@billCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@fykm", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@je", SqlDbType.Float,8) ,            
                        new SqlParameter("@mxGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@status", SqlDbType.VarChar,2) ,                       
                        new SqlParameter("@se", SqlDbType.Decimal,9)             
              
            };

            parameters[0].Value = SqlNull(model.BillCode);

            parameters[1].Value = SqlNull(model.Fykm);

            parameters[2].Value = SqlNull(model.Je);

            parameters[3].Value = SqlNull(model.MxGuid);

            parameters[4].Value = SqlNull(model.Status);

            parameters[5].Value = SqlNull(model.Se);


            DataHelper.ExcuteNonQuery(strSql.ToString(), tran, parameters, false);

        }
        public void AddHsbm(IList<Bill_Ybbxmxb_Fykm_Dept> hsbmList, SqlTransaction tran)
        {
            string sql = "insert into bill_ybbxmxb_fykm_dept(kmmxGuid,mxGuid,deptCode,je,status) values (@kmmxGuid,@mxGuid,@deptCode,@je,@status)";
            for (int i = 0; i < hsbmList.Count; i++)
            {
                SqlParameter[] parameters = 
                {
			            new SqlParameter("@kmmxGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@mxGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@deptCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@je", SqlDbType.Float,8) ,            
                        new SqlParameter("@status", SqlDbType.VarChar,1)             
              
                 };
                parameters[0].Value = SqlNull(hsbmList[i].KmmxGuid);

                parameters[1].Value = SqlNull(hsbmList[i].MxGuid);

                parameters[2].Value = SqlNull(hsbmList[i].DeptCode);

                parameters[3].Value = SqlNull(hsbmList[i].Je);

                parameters[4].Value = SqlNull(hsbmList[i].Status);


                DataHelper.ExcuteNonQuery(sql, tran, parameters, false);
            }

        }
        public void AddHsXm(IList<Bill_Ybbxmxb_Hsxm> hsxmList, SqlTransaction tran)
        {
            string sql = "insert into bill_ybbxmxb_hsxm(kmmxGuid,mxGuid,xmCode,je) values (@kmmxGuid,@mxGuid,@xmCode,@je) ";

            for (int i = 0; i < hsxmList.Count; i++)
            {

                SqlParameter[] parameters = {
			            new SqlParameter("@kmmxGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@mxGuid", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@xmCode", SqlDbType.VarChar,50) ,            
                        new SqlParameter("@je", SqlDbType.Float,8)             
              
            };

                parameters[0].Value = SqlNull(hsxmList[i].KmmxGuid);

                parameters[1].Value = SqlNull(hsxmList[i].MxGuid);

                parameters[2].Value = SqlNull(hsxmList[i].XmCode);

                parameters[3].Value = SqlNull(hsxmList[i].Je);


                DataHelper.ExcuteNonQuery(sql, tran, parameters, false);
            }

        }

        #endregion end ph_main edit by zyl 2014-06-23
    }



}
