<%@ WebHandler Language="C#" Class="clbxBillSaveForGkfj" %>

using System;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Linq;


/// <summary>
/// 市立医院-差旅报销
/// </summary>
public class clbxBillSaveForGkfj : IHttpHandler
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strtype = "";
    string striscommit = "";
    public void ProcessRequest(HttpContext context)
    {
        strtype = context.Request["type"];
        striscommit = context.Request["iscommit"];
        try
        {
            byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
            string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
            BillArray p1;
            //是否启用预算 如果未启用预算 则不去做冲减预算和控制预算是否超过的处理 edit by Lvcc
            bool boHasBudgetControl = new Bll.ConfigBLL().GetModuleDisabled("HasBudgetControl");
            //一般报销单是否需要审核 默认是1 需要 edit by Lvcc
            bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;

            //反序列化json需要framework3.5sp1
            using (StringReader sr = new StringReader(jsonStr))
            {
                JsonSerializer serializer = new JsonSerializer();
                p1 = (BillArray)serializer.Deserialize(new JsonTextReader(sr), typeof(BillArray));
            }

            //检测单据年度是否在开启状 bill_SysConfig  ndStatus  edit by zyl 2015-01-06
            if (strtype == "add")
            {
                DateTime tbNd;
                bool isNd = DateTime.TryParse(p1.bxDate, out tbNd);
                if (!isNd)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("报销单日期格式不合法");
                    return;
                }
                string strNd = tbNd.Year.ToString();
                string ndStatus = server.GetCellValue("select isnull(ConfigValue,'1') from bill_sysConfig where nd='" + strNd + "' and configName='ndStatus'");
                if (ndStatus == "0")  //关闭填报了
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("对不起，该年度已结账，无法再继续报销");
                    return;
                }

            }


            //检测附加单据
            if (string.IsNullOrEmpty(p1.fysq))
            {
                Bill_DataDic datadic = new SysManager().GetDicByTypeCode("02", p1.bxlx);
                if (datadic.Cdj == "1")
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-3");
                    return;
                }
            }

            //判断部门总金额是否等于科目花费金额
            bool IsSumTax = new Bll.ConfigBLL().GetValueByKey("TaxSwitch").Equals("0") ? false : true;
            foreach (YbbxBillTemp fykmtemp in p1.list)
            {
                if (fykmtemp.bm.Length > 0)
                {
                    decimal sumbmje = fykmtemp.bm.Sum(p => p.bmje);
                    decimal kmje = 0;
                    //那么税额不算到部门里面去
                    if (IsSumTax)
                    {
                        kmje = fykmtemp.je + fykmtemp.se;
                    }
                    else
                    {
                        kmje = fykmtemp.je;
                    }
                    if (sumbmje != kmje)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(fykmtemp.km + "科目部门金额与科目金额不相等,相差" + (kmje - sumbmje).ToString());
                        return;
                    }
                }
            }
            //判断项目总金额是否等于科目花费金额
            foreach (YbbxBillTemp fykm in p1.list)
            {
                if (fykm.xm.Length > 0)
                {
                    decimal sumxmje = fykm.xm.Sum(p => p.xmje);
                    if (sumxmje != fykm.je)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(fykm.km + "科目项目金额与科目金额不相等,相差" + (fykm.je - sumxmje).ToString());
                        return;
                    }
                }
            }

            string billname = p1.billname;

            Bill_Main main = new Bill_Main();
            UserMessage user = new UserMessage(p1.billuser);
            main.BillDept = user.GetDept().DeptCode;

            if (string.IsNullOrEmpty(p1.bxDate))
            {
                main.BillDate = DateTime.Parse(DateTime.Now.ToString("G"));
            }
            else
            {
                string strHour = DateTime.Now.Hour.ToString();
                string strMinute = DateTime.Now.Minute.ToString();
                string strSecond = DateTime.Now.Second.ToString();
                DateTime dt = Convert.ToDateTime(p1.bxDate + string.Format(" {0}:{1}:{2}", strHour, strMinute, strSecond));

                main.BillDate = dt;
            }

            main.FlowId = p1.djlx;//"ybbx";
            main.BillUser = p1.billuser;
            main.LoopTimes = 1;//1：差旅费报销单，0：费用报销单

            string gkbm = p1.gkbmbh;
            UserMessage mgr = new UserMessage(p1.billuser);
            string rootbm = mgr.GetRootDept().DeptCode;

            if (!string.IsNullOrEmpty(gkbm) && !string.IsNullOrEmpty(p1.isgk) && p1.isgk.Equals("true"))
            {
                main.IsGk = "1";
                main.GkDept = gkbm;
            }
            else
            {
                main.IsGk = "0";
                main.GkDept = rootbm;
                gkbm = rootbm;
            }
            //判断是否是新增
            BillManager bmgr = new BillManager();
            IList<Bill_Main> tempMain = bmgr.GetMainsByBillName(billname);

            IList<Bill_Ybbxmxb_Fykm> tempkmList = new List<Bill_Ybbxmxb_Fykm>();
            if (tempMain != null && tempMain.Count > 0)
            {
                //如果是单据修改那么判断是否是跨月
                DateTime bxdate = Convert.ToDateTime(p1.bxDate);
                DateTime tempDate = tempMain[0].BillDate.Value;
                DateTime begDate = new DateTime(tempDate.Year, tempDate.Month, 1);
                DateTime endDate;
                if (tempDate.Month < 12)
                {
                    endDate = new DateTime(tempDate.Year, tempDate.Month + 1, 1);
                }
                else
                {
                    endDate = new DateTime(tempDate.Year + 1, 1, 1);
                }
                if (bxdate >= endDate || bxdate < begDate)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-4");
                    return;
                }
                main.BillName = tempMain[0].BillName;
                tempkmList = bmgr.GetYbbx(tempMain[0].BillCode).KmList;
                main.BillCode = tempMain[0].BillCode;
            }
            else
            {
                SysManager sysMgr = new SysManager();
                main.BillName = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);
                main.BillCode = new GuidHelper().getNewGuid();
            }
            //是否冲减预算  
            Bill_DataDic billDic = (new SysManager()).GetDicByTypeCode("02", p1.bxlx);
            if (billDic.Cjys == "1" && boHasBudgetControl)//冲减预算必须还要启用预算管理
            {
                //冲减预算
                YsManager ysMgr = new YsManager();
                //string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
                string nd = p1.bxDate.Substring(0, 4); ;
                string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];
                for (int i = 0; i < p1.list.Length; i++)
                {
                    decimal kcje = 0;
                    //新增
                    if (tempMain != null && tempMain.Count > 0)
                    {
                        var linqtemp = from lin in tempkmList
                                       where lin.Fykm == p1.list[i].km
                                       select lin.Je;
                        foreach (var kmje in linqtemp)
                        {
                            kcje = kmje;
                        }
                    }
                    string kmCode = p1.list[i].km;
                    string depCode = gkbm;

          
                    string gcbh= ysMgr.GetYsgcCode((DateTime)main.BillDate);
                    
                    //是否超出预算的控制 edit by lvcc 之前没有控制
                    decimal ysje = ysMgr.GetYueYs(gcbh, p1.list[i].bxbm, kmCode);
                    decimal hfje = ysMgr.GetYueHf(gcbh, p1.list[i].bxbm, kmCode);
                    decimal syje = 0;
                    if (strtype.Equals("add"))
                    {
                        syje = ysje - hfje;
                    }
                    else if (strtype.Equals("edit"))
                    {
                        syje = ysje - hfje + kcje;
                    }
                    else
                    {//未发现操作状态 是添加还是修改
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("-9");
                        return;
                    }

                    decimal billje = p1.list[i].je;

                    //是否启用销售提成模块
                    bool hasSaleRebate = new Bll.ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1");

                    decimal deTcje = 0;
                    if (hasSaleRebate)
                    {
                        deTcje = ysMgr.getEffectiveSaleRebateAmount(depCode, kmCode);
                    }
                    if (syje < billje)
                    {
                        if (billDic.Cys == "1")
                        {
                            //在这里判断是否启用销售提成 如果启用  超出预算的部分来用销售提成的部分来替 如果还不够 则返回预算超出的报告
                            //edit by lvcc 20121113
                            if (new Bll.ConfigBLL().GetValueByKey("HasSaleRebate").Equals("1"))//启用销售提成
                            {
                                if (syje + deTcje < billje)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("-2");
                                    return;
                                }
                            }
                            else
                            {
                                context.Response.ContentType = "text/plain";
                                context.Response.Write("-2");
                                return;
                            }
                        }
                        //费用提成不够了  自动增加预算
                        if (p1.bxlx == "01" && hasSaleRebate)
                        {
                            List<string> listSql = new List<string>();
                            string ysGuid = (new GuidHelper()).getNewGuid();
                            decimal deAddYuSuan = billje - syje;
                            string strSqlInsertToFymxb = @"insert into bill_ysmxb(gcbh,billCode,yskm,ysje,ysDept,ysType) 
                                                        values('" + gcbh + "','" + ysGuid + "','" + kmCode + "','" + deAddYuSuan + "','" + depCode + "','2')";
                            string strSqlInsertToBillMain = @"insert into bill_main(billCode,billName,flowID,stepID,billUser,billDate,billDept,billJe,loopTimes,billType) 
                                                            values('" + ysGuid + "','" + gcbh + "','yszj','end','','" + System.DateTime.Now.ToString() + "','" + depCode + "','" + deAddYuSuan + "','1','2')";//billuser不要为空 表示是话费费用提成的金额 程序自动添加的预算追加而不是手工添加的预算追加记录
                            listSql.Add(strSqlInsertToBillMain);//往单据主表里添加记录
                            listSql.Add(strSqlInsertToFymxb);//往预算明细表里添加记录
                            T_SaleFeeSpendNote modelSaleFeeSpendNote = new T_SaleFeeSpendNote();
                            modelSaleFeeSpendNote.Billcode = (new GuidHelper()).getNewGuid();
                            modelSaleFeeSpendNote.Deptcode = depCode;
                            modelSaleFeeSpendNote.Fee = deAddYuSuan;
                            modelSaleFeeSpendNote.Sysdatetime = DateTime.Now.ToString();
                            modelSaleFeeSpendNote.Sysusercode = main.BillUser;
                            modelSaleFeeSpendNote.YsgcCode = gcbh;
                            modelSaleFeeSpendNote.Yskmcode = kmCode;
                            int iRel = server.ExecuteNonQuerysArray(listSql);
                            string strErrorMsg = "";
                            if (iRel >= 0)
                            {
                                int iNoteRel = new Bll.SaleBill.T_SaleFeeSpendNoteBll().Add(modelSaleFeeSpendNote, out strErrorMsg);
                                if (iNoteRel < 1)
                                {
                                    context.Response.ContentType = "text/plain";
                                    context.Response.Write("-2");
                                    return;
                                }
                            }
                            else
                            {
                                context.Response.ContentType = "text/plain";
                                context.Response.Write("-2");
                                return;
                            }
                        }
                    }
                }
            }
            //检查预算科目是否有核算
            for (int i = 0; i < p1.list.Length; i++)
            {
                string kmCode = p1.list[i].km;
                Bill_Yskm yskm = new Bill_Yskm();
                SysManager sm = new SysManager();
                yskm = sm.GetYskmByCode(kmCode);
                //检查部门核算
                if (yskm.BmHs == "1")
                {
                    if (p1.list[i].bm.Length < 1)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("-5");
                        return;
                    }
                }
                //检查项目核算
                if (yskm.XmHs == "1")
                {
                    if (p1.list[i].xm.Length < 1)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("-6");
                        return;
                    }
                }
            }

            //以上为检查科目的核算情况


            IList<Bill_Ybbxmxb> ybbxList = new List<Bill_Ybbxmxb>();

            Bill_Ybbxmxb bxmxb = new Bill_Ybbxmxb();
            bxmxb.BillCode = main.BillCode;
            bxmxb.Bxmxlx = p1.bxlx;
            bxmxb.Bxr = p1.bxr;
            bxmxb.Bxrzh = p1.bxrzhanghao;
            bxmxb.Bxrphone = p1.bxrphone;
            bxmxb.Bxsm = p1.sm;
            bxmxb.Bxzy = p1.zy;
            bxmxb.Ybje = p1.gfje;
            bxmxb.Bxr2 = p1.bxr2;


            //如果是给付
            if (p1.sfgf == "1")
            {
                bxmxb.Gfr = Convert.ToString(context.Session["userCode"]);
                bxmxb.Gfsj = DateTime.Now;
                main.StepId = "end";
            }
            else
            {
                //可能是修改审核后的报销单，所以要先获取下单据的code

                if (tempMain != null && tempMain.Count > 0)
                {
                    main.StepId = tempMain[0].StepId;
                }
                else
                {
                    if (boYbbxNeedAudit)
                    {
                        main.StepId = "-1";
                    }
                    else
                    {
                        main.StepId = "end";
                    }
                }
            }
            bxmxb.Sfgf = p1.sfgf;//是否给付
            //凭证号 凭证日期 是否挂账
            string strSql = "select guazhang,pzcode,pzdate from bill_ybbxmxb where billCode='" + bxmxb.BillCode + "'";
            System.Data.DataTable dtMxb = server.GetDataTable(strSql, null);
            if (dtMxb.Rows.Count > 0)
            {
                bxmxb.Pzdate = dtMxb.Rows[0]["pzdate"].ToString();
                bxmxb.Pzcode = dtMxb.Rows[0]["pzcode"].ToString();
                bxmxb.Guazhang = dtMxb.Rows[0]["guazhang"].ToString();
            }
            if (p1.sfgf == "1")
            {
                bxmxb.Guazhang = "0";
            }

            int djs = 0;
            if (int.TryParse(p1.djs, out djs))
            {
                bxmxb.Bxdjs = djs;
            }

            IList<Bill_Ybbx_Fysq> fysqList = new List<Bill_Ybbx_Fysq>();
            if (!string.IsNullOrEmpty(p1.fysq))
            {
                string[] fysqCodes = p1.fysq.Split('|');
                bool boflg = false;
                for (int i = 0; i < fysqCodes.Length; i++)
                {
                    Bill_Ybbx_Fysq fyClass = new Bill_Ybbx_Fysq();
                    fyClass.BillCode = main.BillCode;
                    fyClass.SqCode = fysqCodes[i];
                    fyClass.Status = "0";
                    fysqList.Add(fyClass);
                }
            }
            bxmxb.FysqList = fysqList;

            decimal je = 0;
            IList<Bill_Ybbxmxb_Fykm> kmList = new List<Bill_Ybbxmxb_Fykm>();
            for (int i = 0; i < p1.list.Length; i++)
            {
                Bill_Ybbxmxb_Fykm fykm = new Bill_Ybbxmxb_Fykm();
                fykm.BillCode = main.BillCode;
                fykm.Fykm = p1.list[i].km;
                fykm.Je = p1.list[i].je;
                //通过配置项控制税额是否进费用 修改于20130121（Lvcc）  20121016次修改作废
                if (IsSumTax)
                {
                    je += p1.list[i].je + p1.list[i].se;
                }
                else
                {
                    je += p1.list[i].je;//+ p1.list[i].修改于20121016，税额不进费用
                }
                fykm.MxGuid = (new GuidHelper()).getNewGuid();
                fykm.Se = p1.list[i].se;
                fykm.Status = "0";
                fykm.Bxbm = p1.list[i].bxbm;
                IList<Bill_Ybbxmxb_Fykm_Dept> depList = new List<Bill_Ybbxmxb_Fykm_Dept>();
                for (int j = 0; j < p1.list[i].bm.Length; j++)
                {
                    Bill_Ybbxmxb_Fykm_Dept dept = new Bill_Ybbxmxb_Fykm_Dept();
                    dept.DeptCode = p1.list[i].bm[j].bmbh;
                    dept.Je = p1.list[i].bm[j].bmje;
                    dept.KmmxGuid = fykm.MxGuid;
                    dept.MxGuid = (new GuidHelper()).getNewGuid();
                    dept.Status = "0";
                    depList.Add(dept);
                }
                fykm.DeptList = depList;

                IList<Bill_Ybbxmxb_Hsxm> xmList = new List<Bill_Ybbxmxb_Hsxm>();
                for (int j = 0; j < p1.list[i].xm.Length; j++)
                {
                    Bill_Ybbxmxb_Hsxm xm = new Bill_Ybbxmxb_Hsxm();
                    xm.XmCode = p1.list[i].xm[j].xmbh;
                    xm.Je = p1.list[i].xm[j].xmje;
                    xm.KmmxGuid = fykm.MxGuid;
                    xm.MxGuid = (new GuidHelper()).getNewGuid();
                    xmList.Add(xm);
                }
                fykm.XmList = xmList;
                kmList.Add(fykm);
            }
            bxmxb.KmList = kmList;
            ybbxList.Add(bxmxb);
            main.BillJe = je;

            BillManager billmgr = new BillManager();
            //插入 一般报销表 主表 等数据表
            //if (main.IsGk.Equals("1"))
            //{
            billmgr.insertYbbxForGkfj(main, ybbxList);
            //}
            //else
            //{
            //    billmgr.InsertYbbx(main, ybbxList);
            //}
            //插入出差表
            bill__travelclaims modelbill__travelclaims = new bill__travelclaims();
            modelbill__travelclaims.begintime = p1.bgtime;
            modelbill__travelclaims.endtime = p1.edtime;
            modelbill__travelclaims.beginaddress = p1.didianqi;
            modelbill__travelclaims.endaddress = p1.didianzhi;
            modelbill__travelclaims.Note1 = p1.biaozhun;
            modelbill__travelclaims.bxdjs = djs;
            modelbill__travelclaims.bxzb = p1.zhibie;
            modelbill__travelclaims.tianshu = p1.tianshu;
            modelbill__travelclaims.hsbzfje = p1.huoshifei;
            modelbill__travelclaims.hwfje = p1.huiwufei;
            modelbill__travelclaims.ccfje = p1.chechuanfei;
            modelbill__travelclaims.suje = p1.zhusufei;
            modelbill__travelclaims.Note2 = p1.qitaje;
            modelbill__travelclaims.Note3 = p1.zdyfeiname1;
            modelbill__travelclaims.Note4 = p1.zdyfeival1;
            modelbill__travelclaims.Note5 = p1.zdyfeiname2;
            modelbill__travelclaims.Note6 = p1.zdyfeival2;
            modelbill__travelclaims.billCode = main.BillName;
            new Dal.FeeApplication.bill_travelclaimsDal().Add(modelbill__travelclaims);

            string strCurrentOrLastForYbbx = new Bll.ConfigBLL().GetValueByKey("CurrentOrLastForYbbx");
            if (strCurrentOrLastForYbbx.Equals("0"))
            {
                context.Session["LastBXR"] = main.BillUser;
            }

            //添加到审批流
            if (!string.IsNullOrEmpty(striscommit) && striscommit.Equals("1"))
            {
                WorkFlowLibrary.WorkFlowBll.WorkFlowManager bll = new WorkFlowLibrary.WorkFlowBll.WorkFlowManager();
                try
                {
                    WorkFlowLibrary.WorkFlowModel.WorkFlowRecord wfr = bll.CreateWFRecord(main.BillName, "gkbx", main.BillUser,main.BillDept);
                    WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager mgr2 = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
                    wfr.FlowId = "gkbx";
                    mgr2.InsertRecord(wfr);
                }
                catch (Exception e)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-3");
                    return;
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }
        catch (Exception e)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-1");
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    class BillArray
    {
        public YbbxBillTemp[] list { get; set; }
        public string billuser { get; set; }//
        public string bxr { get; set; }//
        public string zy { get; set; }//
        public string sm { get; set; }//
        public string bxlx { get; set; }//
        public string gkbmbh { get; set; }//
        public string billname { get; set; }//
        public string fysq { get; set; }//
        public string bxDate { get; set; }//
        public string sfgf { get; set; }//
        /// <summary>
        /// 单据数
        /// </summary>
        public string djs { get; set; }//
        public string djlx { get; set; }//
        public string isgk { get; set; }//
        /// <summary>
        /// 给付金额
        /// </summary>
        public decimal gfje { get; set; }//
        /// <summary>
        /// 自定义费用名1
        /// </summary>
        public string zdyfeiname1 { get; set; }//
        // <summary>
        /// 自定义费用名2
        /// </summary>
        public string zdyfeiname2 { get; set; }//
        // <summary>
        /// 自定义费用值1
        /// </summary>
        public string zdyfeival1 { get; set; }//
        // <summary>
        /// 自定义费用值2
        /// </summary>
        public string zdyfeival2 { get; set; }//
        /// <summary>
        /// 地点止
        /// </summary>
        public string didianzhi { get; set; }//
        /// <summary>
        /// 地点起
        /// </summary>
        public string didianqi { get; set; }//
        /// <summary>
        /// 其它金额
        /// </summary>
        public string qitaje { get; set; }//
        /// <summary>
        /// 报销人账号
        /// </summary>
        public string bxrzhanghao { get; set; }//
        /// <summary>
        /// 报销人/单位
        /// </summary>
        public string bxr2 { get; set; }//
        /// <summary>
        /// 报销人电话
        /// </summary>
        public string bxrphone { get; set; }//
        /// <summary>
        ///出发时间
        /// </summary>
        public string bgtime { get; set; }//
        /// <summary>
        ///结束时间
        /// </summary>
        public string edtime { get; set; }//
        /// <summary>
        /// 职别
        /// </summary>
        public string zhibie { get; set; }//
        /// <summary>
        /// 出差天数
        /// </summary>
        public string tianshu { get; set; }//
        /// <summary>
        /// 出差标准
        /// </summary>
        public string biaozhun { get; set; }//

        /// <summary>
        /// 伙食费
        /// </summary>
        public string huoshifei { get; set; }//

        /// <summary>
        /// 车船费
        /// </summary>
        public string chechuanfei { get; set; }//

        /// <summary>
        /// 会务费
        /// </summary>
        public string huiwufei { get; set; }//
        /// <summary>
        /// 住宿费
        /// </summary>
        public string zhusufei { get; set; }//

    }
    class YbbxBillTemp
    {
        public string km { get; set; }
        public decimal je { get; set; }
        public decimal se { get; set; }
        public BmTemp[] bm { get; set; }
        public XmTemp[] xm { get; set; }
        //报销部门
        public string bxbm { get; set; }
    }

    class BmTemp
    {
        public string bmbh { get; set; }
        public decimal bmje { get; set; }
    }

    class XmTemp
    {
        public string xmbh { get; set; }
        public decimal xmje { get; set; }
    }
}