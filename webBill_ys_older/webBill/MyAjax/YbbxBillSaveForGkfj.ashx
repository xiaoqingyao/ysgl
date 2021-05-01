<%@ WebHandler Language="C#" Class="YbbxBillSaveForGkfj" %>

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
/// 市立医院 归口报销（费用报销）
/// </summary>
public class YbbxBillSaveForGkfj : IHttpHandler
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strtype = "";
    string striscommit = "";//是否自动提交
    string strdiccode = "02";//对应的预算类型的决算明细类型（该值存于bill_datadic中 ）
    string strdydj = "02";//对应单据 也就是预算类型
    public void ProcessRequest(HttpContext context)
    {
        strtype = context.Request["type"];
        striscommit = context.Request["iscommit"];
        strdydj = context.Request["dydj"];
        if (!string.IsNullOrEmpty(strdydj))
        {
            strdiccode = server.GetCellValue("select  top 1 diccode from bill_dataDic where dictype='00' and note3='" + strdydj + "'");
        }
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

            //附件
            string fujian = HttpUtility.UrlDecode(p1.fujian);
            //if (fujian!="")
            //{
            //    string[] arrTemp = fujian.Split('|');
            //    string[] arrname = arrTemp[0].Split(';');
            //    string[] arrfilepath = arrTemp[1].Split(';');
            //    for (int i = 0; i < arrfilepath.Length; i++)
            //    {
            //        string filename = arrfilepath[i].Substring(arrfilepath[i].LastIndexOf("\\") + 1);
            //        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //        ////转换成绝对地址,
            //        string serverpath = context.Server.MapPath(@"~\Uploads\ybbx\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
            //        ////转换成与相对地址,相对地址为将来访问图片提供
            //        string relativepath = @"~\Uploads\ybbx\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
            //        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
            //        if (!Directory.Exists(context.Server.MapPath(@"~\Uploads\ybbx\")))
            //        {
            //            Directory.CreateDirectory(context.Server.MapPath(@"~\Uploads\ybbx\"));
            //        }
            //        HttpPostedFile file = new HttpPostedFile();
            //        file.SaveAs()
            //        upLoadFiles.PostedFile.SaveAs(serverpath);
            //    }
            //}

            #region  检测单据年度是否在开启状态
            // bill_SysConfig  ndStatus  edit by zyl 2015-01-06
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
            #endregion

            #region 检测强制附加单据选项
            if (string.IsNullOrEmpty(p1.fysq))
            {
                //根据单据类型(strdiccode，比如报销单、收入单……) 和选择的具体值（下拉框选择的值） 去校验是否附加单据
                Bill_DataDic datadic = new SysManager().GetDicByTypeCode(strdiccode, p1.bxlx);
                if (datadic.Cdj == "1")
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-3");
                    return;
                }
            }
            #endregion

            #region  判断部门总金额是否等于科目花费金额
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
                        context.Response.Write(fykmtemp.km + "部门核算金额与科目金额不相等,相差" + (kmje - sumbmje).ToString());
                        return;
                    }
                }
            }
            #endregion

            #region  判断项目总金额是否等于科目花费金额
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
            #endregion

            string billname = p1.billname;
            Bill_Main main = new Bill_Main();
            UserMessage user = new UserMessage(p1.billuser);
            main.BillDept = user.GetDept().DeptCode;
            main.Note5 = p1.yksqcode;
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
            main.Note4 = p1.isxkfx;
            if (p1.djlx.Equals("yksq_dz") && p1.isxkfx == "是")//如果是大智的费用报销单  那么note1=前台选择的报销日期
            {
                main.Note1 = p1.bxDate;
                //获取用款申请单的日期
                string yksqcode = p1.yksqcode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
                string dt = server.GetCellValue("select billdate from bill_main where billname='" + yksqcode + "'");
                main.BillDate = DateTime.Parse(dt);//实际报销日期以用款申请单为准
            }
            else
            {
                main.Note1 = DateTime.Now.ToString("yyyy-MM-dd");//单据填报时间
            }
            main.needBx = p1.needBx;

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
                if (!p1.djlx.Equals("yksq_dz") && (bxdate >= endDate || bxdate < begDate))
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-4");
                    return;
                }
                main.BillName = tempMain[0].BillName;
                tempkmList = bmgr.GetYbbx(tempMain[0].BillCode).KmList;
                main.BillCode = tempMain[0].BillCode;
                main.StepId = tempMain[0].StepId;
            }
            else
            {
                SysManager sysMgr = new SysManager();
                main.BillName = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);
                main.BillCode = new GuidHelper().getNewGuid();
                main.StepId = "-1";
            }

            //是否冲减预算  
            Bill_DataDic billDic = (new SysManager()).GetDicByTypeCode(strdiccode, p1.bxlx);
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

                    string gcbh = ysMgr.GetYsgcCode((DateTime)main.BillDate);

                    //是否超出预算的控制 edit by lvcc 之前没有控制
                    decimal ysje = ysMgr.GetYueYs(gcbh, p1.list[i].bxbm, kmCode);
                    decimal hfje = ysMgr.GetYueHf(gcbh, p1.list[i].bxbm, kmCode, strdiccode);
                    decimal syje = 0;
                    // 2014-03-17回滚预算控功能添加---beg
                    decimal rollsy = 0;
                    YsManager ysmgr = new YsManager();
                    bool IsRollCtrl = new Bll.ConfigBLL().GetValueByKey("IsRollCtrl").Equals("1");
                    if (IsRollCtrl)
                    {
                        rollsy += ysmgr.GetRollSy(gcbh, depCode, kmCode);
                    }
                    //2014-03-17--end
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

                    }

                }

                //添加note5 = 1 20160830   如果用款申请单被用了，就写个1表示同时对应的报销单填写用款申请单的单号  删除逆操作写为0
                string ntsql = @"update bill_main set note5='1' where flowID='ybbx' and billName='" + p1.yksqcode + "'";
                server.ExecuteNonQuery(ntsql);

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
            bxmxb.Bxrzh = p1.bxrzh;
            bxmxb.Bxrphone = p1.bxrphone;
            bxmxb.Bxsm = p1.sm;
            bxmxb.Sqlx = p1.sqlx;
            bxmxb.Ykfs = p1.ykfs;
            bxmxb.Bxzy = p1.zy;
            bxmxb.Ybje = p1.gfje;
            bxmxb.Bxr2 = p1.bxr2;
            bxmxb.fujian = fujian;

            int djs = 0;
            if (int.TryParse(p1.djs, out djs))
            {
                bxmxb.Bxdjs = djs;
            }



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
                    je += p1.list[i].je;
                }
                fykm.MxGuid = (new GuidHelper()).getNewGuid();
                fykm.Se = p1.list[i].se;
                fykm.Status = "0";
                fykm.Bxbm = p1.list[i].bxbm;
                fykm.ms = p1.list[i].syje.ToString();//保存制单时的剩余预算金额
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

                //Note1
                if (p1.djlx.Equals("yksq_dz") && p1.isxkfx == "是")//如果是大智的费用报销单  那么note1=前台选择的报销日期
                {
                    main.Note1 = p1.bxDate;
                    //获取用款申请单的日期
                    string dt = server.GetCellValue("select billdate from bill_main where billname='" + p1.list[i].yksqcode + "'");
                   fykm.yksqBillDate = dt;
                }
                else
                {
                    main.Note1 = DateTime.Now.ToString("yyyy-MM-dd");//单据填报时间
                }
                
                kmList.Add(fykm);
            }
            bxmxb.KmList = kmList;
            ybbxList.Add(bxmxb);
            main.BillJe = je;

            BillManager billmgr = new BillManager();
            billmgr.insertYbbxForGkfj(main, ybbxList);
            //获取用款申请单的code
            List<string> lisql = new List<string>();
            string strykcode = p1.yksqcode;
            string[] sArray = strykcode.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            if (sArray.Length > 0)
            {
                for (int i = 0; i < sArray.Length; i++)
                {
                    if (!string.IsNullOrEmpty(sArray[i]))
                    {
                        lisql.Add("insert into dz_yksq_bxd(yksq_code,bxd_code)  select '" + sArray[i] + "',billcode from bill_main where billname='" + main.BillName + "'");
                    }
                }
            }

            if (lisql.Count > 0)
            {
                int introw = server.ExecuteNonQuerys(lisql.ToArray());
                if (introw < 1)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("yksq");
                    return;
                }
            }
            //添加到审批流
            if (!string.IsNullOrEmpty(striscommit) && striscommit.Equals("1"))
            {
                WorkFlowLibrary.WorkFlowBll.WorkFlowManager bll = new WorkFlowLibrary.WorkFlowBll.WorkFlowManager();
                try
                {
                    WorkFlowLibrary.WorkFlowModel.WorkFlowRecord wfr = bll.CreateWFRecord(main.BillName, main.FlowId, main.BillUser, main.BillDept);
                    WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager mgr2 = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
                    wfr.FlowId = main.FlowId;
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
        public string billuser { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string bxr { get; set; }
        /// <summary>
        /// 报销人账号 格式：开户行|&|账号|&|收款单位
        /// </summary>
        public string bxrzh { get; set; }
        /// <summary>
        /// 报销人电话
        /// </summary>
        public string bxrphone { get; set; }
        /// <summary>
        /// 报销人
        /// </summary>
        public string bxr2 { get; set; }
        public string zy { get; set; }
        public string sm { get; set; }
        public string bxlx { get; set; }
        public string gkbmbh { get; set; }
        public string billname { get; set; }
        public string fysq { get; set; }//附加单据
        public string bxDate { get; set; }
        public string sfgf { get; set; }
        public string djs { get; set; }
        public string djlx { get; set; }
        public string isgk { get; set; }
        /// <summary>
        /// 给付金额
        /// </summary>
        public decimal gfje { get; set; }

        public string sqlx { get; set; }//申请类型 大智用
        public string ykfs { get; set; }//用款方式 大智用
        public string yksqcode { get; set; }//用款申请单code 大智用
        public string isxkfx { get; set; }//是否是新财年
        public string fujian { get; set; }
        public string needBx { get; set; }
    }

    class YbbxBillTemp
    {
        public string km { get; set; }
        public decimal je { get; set; }
        public decimal se { get; set; }
        public decimal syje { get; set; }//剩余金额
        public BmTemp[] bm { get; set; }
        public XmTemp[] xm { get; set; }
        //报销部门
        public string bxbm { get; set; }
        public string yksqcode { get; set; }//报销明细对应的用款申请单
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