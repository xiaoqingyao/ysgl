<%@ WebHandler Language="C#" Class="JkbxBillSave_Dz" %>

using System;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using Models;
using Bll.UserProperty;
using System.Linq;
using System.Text;

public class JkbxBillSave_Dz : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strtype = "";
    string strdiccode = "02";//
    string strdydj = "02";


    public void ProcessRequest(HttpContext context)
    {
        strtype = context.Request["type"];
        //strdydj = context.Request["dydj"];
        if (!string.IsNullOrEmpty(strdydj))
        {
            strdiccode = server.GetCellValue("select  top 1 diccode from bill_dataDic where dictype='00' and note3='" + strdydj + "'");
        }
        try
        {
            byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
            string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
            BillArray p1;
            //是否有财务预算 如果未启用预算 则不去做冲减预算和控制预算是否超过的处理 edit by Lvcc
            bool boHasBudgetControl = new Bll.ConfigBLL().GetModuleDisabled("HasBudgetControl");


            //反序列化json需要framework3.5sp1
            using (StringReader sr = new StringReader(jsonStr))
            {
                JsonSerializer serializer = new JsonSerializer();
                p1 = (BillArray)serializer.Deserialize(new JsonTextReader(sr), typeof(BillArray));
            }

            //开始取值



            //附件
            string fujian = HttpUtility.UrlDecode(p1.fujian);
            //检查一般报销说明是否符合配置项
            bool boIsYbbxSmMust = new Bll.ConfigBLL().GetValueByKey("IsYbbxSmMust").Equals("1") ? true : false;
            if (boIsYbbxSmMust && p1.sm.Trim().Length == 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("smbt");//说明必填
                return;
            }
            //检查是否月结
            string bxyd = p1.bxDate.Substring(0, 7);
            QueryManger query = new QueryManger();
            if (query.CheckYj(bxyd) != "")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-7");
                return;
            }


            //判断部门总金额是否等于科目花费金额
            bool IsSumTax = new Bll.ConfigBLL().GetValueByKey("TaxSwitch").Equals("0") ? false : true;//一般报销单税额是否进总金额的开关
            foreach (YbbxBillTemp fykmtemp in p1.list)
            {
                if (fykmtemp.bm.Length > 0)
                {
                    decimal sumbmje = fykmtemp.bm.Sum(p => p.bmje);
                    decimal kmje = 0;
                    //那么税额不算到部门里面去
                    //if (IsSumTax)
                    //{
                    //    kmje = fykmtemp.je + fykmtemp.se;
                    //}else{
                    kmje = fykmtemp.je;
                    //}
                    if (sumbmje != kmje)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(fykmtemp.km + "科目的部门核算金额与科目金额不相等,相差" + (kmje - sumbmje).ToString());
                        return;
                    }
                }
            }

            #region  判断项目申报金额是否
            ////判断需要进行预算的项目申报的总金额是否超支
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

            //判断项目金额是否超支 
            string hsxmModel = server.GetCellValue("select isnull(avalue,'') from T_config where akey='YbbxHsxmMode'");
            if (hsxmModel == "1")
            {
                List<Dept_XmTemp> deptxmList = new List<Dept_XmTemp>();

                string xmcodes = "";
                foreach (YbbxBillTemp fykm in p1.list)
                {

                    if (fykm.xm.Length > 0)
                    {
                        foreach (XmTemp xmtemp in fykm.xm)
                        {
                            xmcodes += "'" + xmtemp.xmbh + "',";
                        }
                    }
                }


                if (!string.IsNullOrEmpty(xmcodes))
                {
                    xmcodes = xmcodes.Substring(0, xmcodes.Length - 1);

                    //获取归口部门(只获取需要控制预算的项目)
                    string tt = p1.bxDate;
                    if (tt.Length > 4)
                    {
                        tt = tt.Substring(0, 4);
                    }
                    string xmssql = "select * from bill_xm_dept_nd where xmDept='" + p1.gkbmbh + "' and isCtrl='1' and nd='" + tt + "' and  xmCode in (" + xmcodes + ")";
                    System.Data.DataTable xms = server.GetDataSet(xmssql).Tables[0];

                    for (int i = 0; i < xms.Rows.Count; i++)
                    {
                        Dept_XmTemp deptxm = new Dept_XmTemp();
                        deptxm.xmcode = Convert.ToString(xms.Rows[i]["xmCode"]);

                        string sqlje = "select isnull(SUM(je),'0') as je  from bill_ybbxmxb_hsxm where xmCode='" + deptxm.xmcode + "'and kmmxGuid in(select mxGuid from bill_main,bill_ybbxmxb_fykm  where bill_main.billCode=bill_ybbxmxb_fykm.billCode and flowid in ('ybbx','qtbx','gkbx') and billDate>='" + tt + "-01-01' and billDate<='" + tt + "-12-31' )";
                        string xmzje = Convert.ToString(server.GetDataSet(sqlje).Tables[0].Rows[0]["je"]);

                        //context.Response.Write("<script>alert(" + Convert.ToDecimal(xms.Rows[i]["je"]) + ");</script>");
                        //context.Response.Write("<script>alert(" + xmzje + ");</script>");

                        deptxm.kyje = Convert.ToDecimal(Convert.ToDecimal(xms.Rows[i]["je"]) - Convert.ToDecimal(xmzje));
                        deptxm.sbzje = Convert.ToDecimal(0);
                        deptxmList.Add(deptxm);
                    }
                }


                foreach (YbbxBillTemp fykm in p1.list)
                {
                    if (fykm.xm.Length > 0)
                    {

                        foreach (XmTemp xmtemp in fykm.xm)
                        {

                            for (int i = 0; i < deptxmList.Count; i++)
                            {
                                if (deptxmList[i].xmcode == xmtemp.xmbh)
                                {
                                    deptxmList[i].sbzje += Convert.ToDecimal(xmtemp.xmje);
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < deptxmList.Count; i++)
                {
                    if (deptxmList[i].kyje - deptxmList[i].sbzje < 0)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(deptxmList[i].xmcode + "项目金额超支" + (deptxmList[i].kyje - deptxmList[i].sbzje).ToString());
                        return;
                    }
                }
            }
            #endregion
            //检测单据合法性完毕------------end
            //开始组织数据
            string billcode = p1.billcode;
            Bill_Main main = new Bill_Main();
            UserMessage user = new UserMessage(p1.billuser);
            main.BillDept = user.GetDept().DeptCode;
            main.BillCode = billcode;
            main.FlowId = "tfsq";//退费报销

            main.BillUser = p1.billuser;
            main.Note4 = p1.isxkfx;//是否新开分校
            main.Note1 = DateTime.Now.ToString("yyyy-MM-dd");//单据填报时间
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
            string gkbm = p1.gkbmbh;
            UserMessage mgr = new UserMessage(p1.billuser);
            string rootbm = mgr.GetRootDept().DeptCode;
            //判断是不是预算到末级
            string strifmj = new Bll.ConfigBLL().GetValueByKey("deptjc");

            if (!string.IsNullOrEmpty(gkbm) && !string.IsNullOrEmpty(p1.isgk) && p1.isgk.Equals("true"))//判断是不是归口部门
            {
                main.IsGk = "1";
                main.GkDept = gkbm;
            }
            else
            {
                main.IsGk = "0";
                if (strifmj == "Y")
                {

                    main.GkDept = p1.gkbmbh;
                    gkbm = p1.gkbmbh;
                }
                else
                {
                    main.IsGk = "0";
                    main.GkDept = rootbm;
                    gkbm = rootbm;
                }

            }

            //判断是否是新增
            BillManager bmgr = new BillManager();
            IList<Bill_Main> tempMain = bmgr.GetMainsByBillName(billcode);

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
                    context.Response.Write("-4");//不能跨月
                    return;
                }
                main.BillName = tempMain[0].BillName;
                tempkmList = bmgr.GetYbbx(tempMain[0].BillCode).KmList;
                main.BillCode = tempMain[0].BillCode;
            }
            else
            {
                SysManager sysMgr = new SysManager();

                main.BillName = sysMgr.GetYbbxBillName("TF", DateTime.Now.ToString("yyyMMdd"), 1);
                main.BillCode = new GuidHelper().getNewGuid();

            }
            //是否冲减预算  
            Bill_DataDic billDic = (new SysManager()).GetDicByTypeCode(strdiccode, p1.bxlx);
            if (billDic.Cys == "1" && boHasBudgetControl)//冲减预算必须还要启用预算管理
            {
                //冲减预算
                YsManager ysMgr = new YsManager();
                string nd = p1.bxDate.Substring(0, 4); ;
                string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];//预算到年 月还是季度
                for (int i = 0; i < p1.list.Length; i++)
                {
                    decimal kcje = 0;//本单据的占用金额
                                     //新增
                    if (tempMain != null)
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

                    //判断是否存在该预算过程编号 by wpp

                    string strcountsql = @" select count(*) from bill_ysgc where gcbh='" + gcbh + "'";
                    string strconunt = server.GetCellValue(strcountsql);
                    if (strconunt == "0")
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("wrong");
                        return;
                    }

                    //是否超出预算的控制 edit by lvcc 之前没有控制
                    decimal ysje = ysMgr.GetYueYs(gcbh, depCode, kmCode);//预算金额
                    decimal hfje = ysMgr.GetYueHf(gcbh, depCode, kmCode, strdiccode);//花费金额
                    decimal syje = 0;

                    // 2014-03-17回滚预算控功能添加---beg
                    decimal rollsy = 0;
                    //YsManager ysmgr = new YsManager();
                    //bool IsRollCtrl = new Bll.ConfigBLL().GetValueByKey("IsRollCtrl").Equals("1");
                    //if (IsRollCtrl)
                    //{
                    //    rollsy += ysmgr.GetRollSy(gcbh,depCode, kmCode);
                    //}
                    //2014-03-17--end

                    if (strtype.Equals("add"))
                    {
                        syje = ysje - hfje + rollsy;
                    }
                    else if (strtype.Equals("edit"))
                    {
                        syje = ysje - hfje + kcje + rollsy;
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
                    if (syje < billje)//剩余金额小于报销金额
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write("-2");
                        return;
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
                if (yskm.BmHs == "1" && p1.list[i].bm.Length < 1)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-5");
                    return;

                }
                //检查项目核算
                if (yskm.XmHs == "1" && p1.list[i].xm.Length < 1)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("-6");
                    return;

                }
            }
            //以上为检查科目的核算情况
            IList<Bill_Ybbxmxb> ybbxList = new List<Bill_Ybbxmxb>();
            Bill_Ybbxmxb bxmxb = new Bill_Ybbxmxb();
            bxmxb.BillCode = main.BillCode;
            bxmxb.Bxmxlx = p1.bxlx;
            bxmxb.Bxr = p1.bxr;
            bxmxb.Bxrzh = p1.bxrzh;
            bxmxb.Bxsm = p1.sm;
            bxmxb.Sqlx = p1.sqlx;
            bxmxb.Ykfs = p1.ykfs;
            bxmxb.Bxzy = p1.zy;
            bxmxb.Ybje = p1.gfje;
            bxmxb.fujian = fujian;
            bxmxb.note0 = p1.tfxx;
            bxmxb.note1 = p1.xfqk;
            bxmxb.note2 = p1.note2;


            main.StepId = "-1";
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
            //报销单据数
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
                    je += p1.list[i].je;//+ p1.list[i].修改于20121016，税额不进费用
                }
                fykm.MxGuid = (new GuidHelper()).getNewGuid();
                fykm.Se = p1.list[i].se;
                fykm.bxbm = p1.gkbmbh;
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
            if (main.IsGk.Equals("1"))
            {
                billmgr.insertYbbxForGkfj(main, ybbxList);
            }
            else
            {
                billmgr.InsertYbbx(main, ybbxList);
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
        public string bxr { get; set; }
        /// <summary>
        /// 报销人账号 格式：开户行|&|账号
        /// </summary>
        public string bxrzh { get; set; }

        public string zy { get; set; }
        public string sm { get; set; }
        public string sqlx { get; set; }//申请类型
        public string ykfs { get; set; }//用款方式
        public string bxlx { get; set; }
        public string gkbmbh { get; set; }
        public string billcode { get; set; }
        public string fysq { get; set; }//附加单据
        public string bxDate { get; set; }
        public string sfgf { get; set; }
        public string djs { get; set; }
        public string djlx { get; set; }
        public string isgk { get; set; }
        public string fujian { get; set; }
        public string yksqcode { get; set; }//用款申请单code
        public string isxkfx { get; set; }//是否是新财年
                                          /// <summary>
                                          /// 退费信息 格式： 所在分校|&|学员姓名|&|所在班级|&|协议编号|&|签单时间
                                          /// </summary>
        public string tfxx { get; set; }
        /// <summary>
        /// 消费情况  格式：协议辅导费用|&|已消费课时|&|对应课时单价|&|已消费费用|&|应扣其他费用
        /// </summary>
        public string xfqk { get; set; }
        /// <summary>
        /// 给付金额
        /// </summary>
        public decimal gfje { get; set; }
        /// <summary>
        /// 退费的时候记录对应的任课老师
        /// </summary>
        public string note2 { get; set; }


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
        public decimal xmsyje { get; set; }
        public string nd { get; set; }
    }

    class Dept_XmTemp
    {
        public string xmcode;
        public decimal kyje;
        public decimal sbzje;
        public bool isctrl;
        public bool isOut;
    }
}