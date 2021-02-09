using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace Dal.Bills
{
    public class YbbxDal
    {
        public void DeleteYbbx(string billCode, SqlTransaction tran)
        {
            string delMx = "delete Bill_Ybbxmxb where billCode=@billCode";
            string delKm = "delete bill_ybbxmxb_fykm where billCode=@billCode";
            string delFysq = "delete bill_ybbx_fysq where billCode=@billCode";

            string delyksqdy = "delete dz_yksq_bxd where bxd_code in (select billcode from bill_main where billcode=@billCode or billname=@billCode)";//用款申请单和报销单对应表

            SqlParameter[] billsps = { new SqlParameter("@billCode", billCode) };
            IList<Bill_Ybbxmxb_Fykm> fykmList = GetFykm(billCode);
            DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
            DataHelper.ExcuteNonQuery(delKm, tran, billsps, false);
            DataHelper.ExcuteNonQuery(delFysq, tran, billsps, false);
            DataHelper.ExcuteNonQuery(delyksqdy, tran, billsps, false);
            foreach (Bill_Ybbxmxb_Fykm km in fykmList)
            {
                string mxCode = km.MxGuid;
                string delDep = "delete bill_ybbxmxb_fykm_dept where kmmxGuid=@mxCode";
                string delXm = "delete bill_ybbxmxb_hsxm where kmmxGuid=@mxCode";
                SqlParameter[] mxsps = { new SqlParameter("@mxCode", mxCode) };
                DataHelper.ExcuteNonQuery(delXm, tran, mxsps, false);
                DataHelper.ExcuteNonQuery(delDep, tran, mxsps, false);
            }
        }

        public void DeleteYbbx(string billCode)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteYbbx(billCode, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMain(billCode, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }

        #region for 归口分解
        public void DeleteYbbxsByName(string billName)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    DeleteYbbxsByName(billName, tran);
                    MainDal mdal = new MainDal();
                    mdal.DeleteMainByName(billName, tran);
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }

        public DataTable getbydt(string strcode)
        {
            string strsql = "select billcode from bill_main where billname=@billname";

            SqlParameter[] arrsp = { 
                                             new SqlParameter("@billname",SqlNull(strcode))
                                 };

            DataTable dtRel = new DataTable();
            return dtRel = DataHelper.GetDataTable(strsql, arrsp, false);

        }
        public void DeleteYbbxsByName(string strbillName, SqlTransaction tran)
        {
            //string strsql = "select billcode from bill_main where billname=@billname";

            //SqlParameter[] arrsp = { 
            //                                 new SqlParameter("@billname",SqlNull(strbillName))
            //                     };

            DataTable dtRel = new DataTable();


            dtRel = getbydt(strbillName);
            if (dtRel == null)
            {
                return;
            }
            string strmxguids = "";
            string strselectmxguidssql = "select MxGuid from Bill_Ybbxmxb_Fykm where billcode in (select billcode from bill_main where billname=@billname)";
            DataTable dtmxguids = DataHelper.GetDataTable(strselectmxguidssql, new SqlParameter[] { new SqlParameter("@billname", strbillName) }, false);
            if (dtmxguids != null)
            {
                for (int j = 0; j < dtmxguids.Rows.Count; j++)
                {
                    strmxguids += "'" + dtmxguids.Rows[j]["MxGuid"].ToString() + "',";
                }
                if (strmxguids.Length > 1)
                {
                    strmxguids = strmxguids.Substring(0, strmxguids.Length - 1);
                }
            }
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                string delMx = "delete Bill_Ybbxmxb where billCode=@billCode";
                string delKm = "delete bill_ybbxmxb_fykm where billCode=@billCode";
                string delFysq = "delete bill_ybbx_fysq where billCode=@billCode";

                string delyksqdy = "delete dz_yksq_bxd where bxd_code in (select billcode from bill_main where billcode=@billCode or billname=@billCode)";//用款申请单和报销单对应表


                SqlParameter[] billsps = { new SqlParameter("@billCode", dtRel.Rows[i]["billcode"].ToString()) };
                //IList<Bill_Ybbxmxb_Fykm> fykmList = GetFykm(dtRel.Rows[i]["billcode"].ToString());
                DataHelper.ExcuteNonQuery(delMx, tran, billsps, false);
                DataHelper.ExcuteNonQuery(delKm, tran, billsps, false);
                DataHelper.ExcuteNonQuery(delFysq, tran, billsps, false);
                DataHelper.ExcuteNonQuery(delyksqdy, tran, billsps, false);
                //foreach (Bill_Ybbxmxb_Fykm km in fykmList)
                //{ select * from Bill_Ybbxmxb_Fykm where  billcode
                //string mxCode = km.MxGuid;MxGuid
                string delDep = "delete bill_ybbxmxb_fykm_dept where kmmxGuid in (" + strmxguids + ")";
                string delXm = "delete bill_ybbxmxb_hsxm where kmmxGuid in (" + strmxguids + ")";
                //SqlParameter[] mxsps = { new SqlParameter("@billname", billName) };
                DataHelper.ExcuteNonQuery(delXm, tran, null, false);
                DataHelper.ExcuteNonQuery(delDep, tran, null, false);
                //}
            }
        }
        public void insertYbbxForGkfj(Bill_Main main, IList<Bill_Ybbxmxb> ybbxList)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal mainDal = new MainDal();

                    DeleteYbbxsByName(main.BillName, tran);

                    mainDal.DeleteMainByName(main.BillName, tran);

                    foreach (Bill_Ybbxmxb ybbxmx in ybbxList)
                    {

                        foreach (Bill_Ybbxmxb_Fykm km in ybbxmx.KmList)
                        {
                            //主表
                            main.GkDept = km.Bxbm;
                            main.BillCode = Guid.NewGuid().ToString(); ;
                            main.BillJe = km.Je;
                            mainDal.InsertMain(main, tran);

                            //明细表
                            ybbxmx.BillCode = main.BillCode;
                            InsertYbbxmxb(ybbxmx, tran);

                            //科目明细表
                            km.BillCode = main.BillCode;
                            InsertYbbxKm(km, tran);
                            if (km.DeptList != null)
                            {
                                foreach (Bill_Ybbxmxb_Fykm_Dept dept in km.DeptList)
                                {
                                    InsertYbbxDept(dept, tran);
                                }
                            }

                            if (km.XmList != null)
                            {
                                foreach (Bill_Ybbxmxb_Hsxm xm in km.XmList)
                                {
                                    InsertYbbxXm(xm, tran);
                                }
                            }

                        }
                        if (ybbxmx.FysqList != null)
                        {
                            foreach (Bill_Ybbx_Fysq fysq in ybbxmx.FysqList)
                            {
                                InsertYbbxFysq(fysq, tran);
                            }
                        }

                    }
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }
        #endregion

        public void InsertYbbx(Bill_Main main, IList<Bill_Ybbxmxb> mxList)
        {
            using (SqlConnection conn = new SqlConnection(DataHelper.constr))
            {
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                try
                {
                    MainDal mainDal = new MainDal();
                    mainDal.DeleteMain(main.BillCode, tran);
                    DeleteYbbx(main.BillCode, tran);

                    mainDal.InsertMain(main, tran);
                    foreach (Bill_Ybbxmxb ybbxmx in mxList)
                    {
                        InsertYbbxmxb(ybbxmx, tran);
                        if (ybbxmx.KmList != null)
                        {
                            foreach (Bill_Ybbxmxb_Fykm km in ybbxmx.KmList)
                            {
                                InsertYbbxKm(km, tran);
                                if (km.DeptList != null)
                                {
                                    foreach (Bill_Ybbxmxb_Fykm_Dept dept in km.DeptList)
                                    {
                                        InsertYbbxDept(dept, tran);
                                    }
                                }

                                if (km.XmList != null)
                                {
                                    foreach (Bill_Ybbxmxb_Hsxm xm in km.XmList)
                                    {
                                        InsertYbbxXm(xm, tran);
                                    }
                                }

                            }
                        }

                        if (ybbxmx.FysqList != null)
                        {
                            foreach (Bill_Ybbx_Fysq fysq in ybbxmx.FysqList)
                            {
                                InsertYbbxFysq(fysq, tran);
                            }
                        }

                    }

                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
            }
        }

        public void InsertYbbxFysq(Bill_Ybbx_Fysq fysq, SqlTransaction tran)
        {
            string sql = @"insert into bill_ybbx_fysq(billCode, sqCode, status) values
                           (@billCode, @sqCode, @status)";
            SqlParameter[] sps = { 
                                             new SqlParameter("@billCode",SqlNull(fysq.BillCode)),
                                             new SqlParameter("@sqCode",SqlNull(fysq.SqCode)),
                                             new SqlParameter("@status",SqlNull(fysq.Status))
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);

        }

        public void InsertYbbxmxb(Bill_Ybbxmxb ybbxmx, SqlTransaction tran)
        {
            string sql = @" insert into bill_ybbxmxb ( billCode, bxr,bxrzh,bxrphone,bxzy, bxsm, sfdk, ytje, ybje, sfgf, bxmxlx, gfr, gfsj,bxdjs,guazhang,pzcode,pzdate,bxr2,fujian,sqlx,ykfs,note0,note1,note2,note3,note4,note5) values
                                    (@billCode, @bxr,@bxrzh,@bxrphone, @bxzy, @bxsm, @sfdk, @ytje, @ybje, @sfgf, @bxmxlx, @gfr, @gfsj,@bxdjs,@guazhang,@pzcode,@pzdate,@bxr2,@fujian,@sqlx,@ykfs,@note0,@note1,@note2,@note3,@note4,@note5)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@billCode",SqlNull(ybbxmx.BillCode)),
                                     new SqlParameter("@bxr",SqlNull(ybbxmx.Bxr)),
                                     new SqlParameter("@bxrzh",SqlNull(ybbxmx.Bxrzh)),
                                     new SqlParameter("@bxrphone",SqlNull(ybbxmx.Bxrphone)),
                                     new SqlParameter("@bxzy",SqlNull(ybbxmx.Bxzy)),
                                     new SqlParameter("@bxsm",SqlNull(ybbxmx.Bxsm)),
                                     new SqlParameter("@sfdk",SqlNull(ybbxmx.Sfdk)),
                                     new SqlParameter("@ytje",SqlNull(ybbxmx.Ytje)),
                                     new SqlParameter("@ybje",SqlNull(ybbxmx.Ybje)),
                                     new SqlParameter("@sfgf",SqlNull(ybbxmx.Sfgf)),
                                     new SqlParameter("@bxmxlx",SqlNull(ybbxmx.Bxmxlx)),
                                     new SqlParameter("@gfr",SqlNull(ybbxmx.Gfr)),
                                     new SqlParameter("@gfsj",SqlNull(ybbxmx.Gfsj)),
                                     new SqlParameter("@guazhang",SqlNull(ybbxmx.Guazhang)),
                                     new SqlParameter("@pzcode",SqlNull(ybbxmx.Pzcode)),
                                     new SqlParameter("@pzdate",SqlNull(ybbxmx.Pzdate)),
                                     new SqlParameter("@bxdjs",SqlNull(ybbxmx.Bxdjs)),
                                     new SqlParameter("@bxr2",SqlNull(ybbxmx.Bxr2)),
                                     new SqlParameter("@fujian",SqlNull(ybbxmx.fujian)),
                                     new SqlParameter("@sqlx",SqlNull(ybbxmx.Sqlx)),
                                     new SqlParameter("@ykfs",SqlNull(ybbxmx.Ykfs)),
                                     new SqlParameter("@note0",SqlNull(ybbxmx.note0)),
                                     new SqlParameter("@note1",SqlNull(ybbxmx.note1)),
                                     new SqlParameter("@note2",SqlNull(ybbxmx.note2)),
                                     new SqlParameter("@note3",SqlNull(ybbxmx.note3)),
                                     new SqlParameter("@note4",SqlNull(ybbxmx.note4)),
                                      new SqlParameter("@note5",SqlNull(ybbxmx.note5))
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public void InsertYbbxKm(Bill_Ybbxmxb_Fykm fykm, SqlTransaction tran)
        {
            string sql = @"insert into bill_ybbxmxb_fykm( billCode, fykm, je, mxGuid, status, se,ms)values
                           ( @billCode, @fykm, @je, @mxGuid, @status, @se,@ms)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@billCode",fykm.BillCode),
                                     new SqlParameter("@fykm",fykm.Fykm),
                                     new SqlParameter("@je",SqlNull(fykm.Je)),
                                     new SqlParameter("@mxGuid",fykm.MxGuid),
                                     new SqlParameter("@status",fykm.Status),
                                     new SqlParameter("@se",SqlNull(fykm.Se)),
                                      new SqlParameter("@ms",SqlNull(fykm.ms))
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }

        public void InsertYbbxDept(Bill_Ybbxmxb_Fykm_Dept dept, SqlTransaction tran)
        {
            string sql = @"insert into bill_ybbxmxb_fykm_dept( kmmxGuid, mxGuid, deptCode, je, status)values
                           ( @kmmxGuid, @mxGuid, @deptCode, @je, @status)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@kmmxGuid",dept.KmmxGuid),
                                     new SqlParameter("@mxGuid",dept.MxGuid),
                                     new SqlParameter("@deptCode",dept.DeptCode),
                                     new SqlParameter("@je",dept.Je),
                                     new SqlParameter("@status",dept.Status)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }


        public void InsertYbbxXm(Bill_Ybbxmxb_Hsxm xm, SqlTransaction tran)
        {
            string sql = @"insert into bill_ybbxmxb_hsxm(  kmmxGuid, mxGuid, xmCode, je)values
                           (  @kmmxGuid, @mxGuid, @xmCode, @je)";
            SqlParameter[] sps = { 
                                     new SqlParameter("@kmmxGuid",xm.KmmxGuid),
                                     new SqlParameter("@mxGuid",xm.MxGuid),
                                     new SqlParameter("@xmCode",xm.XmCode),
                                     new SqlParameter("@je",xm.Je)
                                 };
            DataHelper.ExcuteNonQuery(sql, tran, sps, false);
        }


        public Bill_Ybbxmxb GetYbbx(string billCode)
        {
            string sql = "select * from Bill_Ybbxmxb where  billcode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            Bill_Ybbxmxb main = new Bill_Ybbxmxb();
            using (SqlDataReader dr = DataHelper.GetDataReader(sql, sps))
            {
                if (dr.Read())
                {
                    main.BillCode = Convert.ToString(dr["BillCode"]);
                    main.Bxmxlx = Convert.ToString(dr["Bxmxlx"]);
                    main.Bxr = Convert.ToString(dr["Bxr"]);
                    main.Bxsm = Convert.ToString(dr["Bxsm"]);
                    main.Bxzy = Convert.ToString(dr["Bxzy"]);
                    main.Bxrphone = dr["bxrphone"].ToString();
                    main.Bxrzh = dr["bxrzh"].ToString();
                    main.Gfr = Convert.ToString(SetDBNull(dr["Gfr"]));
                    if (dr["Gfsj"] == DBNull.Value)
                    {
                        main.Gfsj = null;
                    }
                    else
                    {
                        main.Gfsj = Convert.ToDateTime(dr["Gfsj"]);
                    }

                    main.Sfdk = Convert.ToString(dr["Sfdk"]);
                    main.Sfgf = Convert.ToString(dr["Sfgf"]);
                    main.Ybje = Convert.ToDecimal(dr["Ybje"]);
                    main.Ytje = Convert.ToDecimal(dr["Ytje"]);

                    main.Bxdjs = Convert.ToInt32(SetDBNull(dr["Bxdjs"]));
                    main.Bxr2 = Convert.ToString(dr["Bxr2"]);
                    main.Ykfs = Convert.ToString(dr["ykfs"]);//用款方式
                    main.Sqlx = Convert.ToString(dr["sqlx"]);//申请类型
                    main.note0 = Convert.ToString(dr["note0"]);//退费单的录入信息
                    main.note1 = Convert.ToString(dr["note1"]);//退费单报销费用信息
                    main.note2 = Convert.ToString(dr["note2"]);
                    main.note3 = Convert.ToString(dr["note3"]);
                    main.note4 = Convert.ToString(dr["note4"]);
                    main.note5 = Convert.ToString(dr["note5"]);

                }
                else
                {
                    return null;
                }
            }
            main.KmList = GetFykm(main.BillCode);
            main.FysqList = GetFysq(main.BillCode);
            return main;
        }

        public IList<Bill_Ybbx_Fysq> GetFysq(string billCode)
        {
            string sql = "select * from bill_ybbx_fysq where billcode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbx_Fysq> list = new List<Bill_Ybbx_Fysq>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbx_Fysq fysq = new Bill_Ybbx_Fysq();
                fysq.BillCode = Convert.ToString(dr["BillCode"]);
                fysq.SqCode = Convert.ToString(dr["SqCode"]);
                fysq.Status = Convert.ToString(dr["Status"]);
                list.Add(fysq);
            }
            return list;
        }

        public IList<Bill_Ybbxmxb_Fykm> GetFykm(string billCode)
        {
            string sql = "select * from Bill_Ybbxmxb_Fykm where  billcode=@billCode";
            SqlParameter[] sps = { new SqlParameter("@billCode", billCode) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Fykm> list = new List<Bill_Ybbxmxb_Fykm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Fykm fykm = new Bill_Ybbxmxb_Fykm();
                fykm.BillCode = Convert.ToString(dr["BillCode"]);
                fykm.Fykm = Convert.ToString(dr["Fykm"]);
                fykm.Je = Convert.ToDecimal(dr["Je"]);
                fykm.MxGuid = Convert.ToString(dr["MxGuid"]);
                fykm.Se = Convert.ToDecimal(SetDBNull(dr["Se"]));
                fykm.Status = Convert.ToString(dr["Status"]);
                fykm.DeptList = GetDept(fykm.MxGuid);
                fykm.XmList = GetXm(fykm.MxGuid);
                fykm.ms = Convert.ToString(dr["ms"]);
                list.Add(fykm);
            }
            return list;
        }


        public IList<Bill_Ybbxmxb_Fykm_Dept> GetDept(string kmmxGuid)
        {
            string sql = "select * from bill_ybbxmxb_fykm_dept where  kmmxGuid=@kmmxGuid";
            SqlParameter[] sps = { new SqlParameter("@kmmxGuid", kmmxGuid) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Fykm_Dept> list = new List<Bill_Ybbxmxb_Fykm_Dept>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Fykm_Dept dept = new Bill_Ybbxmxb_Fykm_Dept();
                dept.DeptCode = Convert.ToString(dr["DeptCode"]);
                dept.Je = Convert.ToDecimal(dr["Je"]);
                dept.KmmxGuid = Convert.ToString(dr["KmmxGuid"]);
                dept.MxGuid = Convert.ToString(dr["MxGuid"]);
                dept.Status = Convert.ToString(dr["Status"]);
                list.Add(dept);
            }
            return list;
        }

        public IList<Bill_Ybbxmxb_Hsxm> GetXm(string kmmxGuid)
        {
            string sql = "select * from Bill_Ybbxmxb_Hsxm where  kmmxGuid=@kmmxGuid";
            SqlParameter[] sps = { new SqlParameter("@kmmxGuid", kmmxGuid) };
            DataTable dt = DataHelper.GetDataTable(sql, sps, false);
            IList<Bill_Ybbxmxb_Hsxm> list = new List<Bill_Ybbxmxb_Hsxm>();
            foreach (DataRow dr in dt.Rows)
            {
                Bill_Ybbxmxb_Hsxm xm = new Bill_Ybbxmxb_Hsxm();
                xm.XmCode = Convert.ToString(dr["XmCode"]);
                xm.Je = Convert.ToDecimal(dr["Je"]);
                xm.KmmxGuid = Convert.ToString(dr["KmmxGuid"]);
                xm.MxGuid = Convert.ToString(dr["MxGuid"]);
                list.Add(xm);
            }
            return list;
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
