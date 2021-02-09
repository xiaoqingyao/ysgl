<%@ WebHandler Language="C#" Class="ybbxBillSaveHandler" %>

using System;
using System.Web;
using System.Data;
using Models;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Bll.UserProperty;
using System.Linq;

public class ybbxBillSaveHandler : IHttpHandler
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {
        byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
        string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
        YbbxBillTemp p1;
        //是否启用预算 如果未启用预算 则不去做冲减预算和控制预算是否超过的处理 edit by Lvcc
        bool boHasBudgetControl = new Bll.ConfigBLL().GetModuleDisabled("HasBudgetControl");
        //一般报销单是否需要审核 默认是1 需要 
        bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;

        //反序列化json需要framework3.5sp1
        using (StringReader sr = new StringReader(jsonStr))
        {
            JsonSerializer serializer = new JsonSerializer();
            p1 = (YbbxBillTemp)serializer.Deserialize(new JsonTextReader(sr), typeof(YbbxBillTemp));
        }
      string mainStr="";
        if (p1.type == "addmx")
        {
            int tempCount = Convert.ToInt32(server.GetCellValue("select count(*) from  bill_ybbxmxb_fykm  where billCode='" + p1.billcode + "' and fykm='" + p1.km + "'"));
            if (tempCount > 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(p1.km + "明细记录已存在！");
                return;
            }
            mainStr = "select a.billCode,a.billName,a.flowID,a.stepID,a.billUser,b.bxr,convert(varchar(10),a.billDate,121) as billDate,a.billDept,a.loopTimes,a.isgk,a.gkdept,b.bxmxlx,b.bxzy,b.bxsm  from bill_main a  inner join bill_ybbxmxb b on a.billCode=b.billCode and a.billCode='" + p1.billcode + "'";
        }
        else
        {
            mainStr = "select * from ph_main where billCode='" + p1.billcode + "'";
        }
        DataTable main = server.GetDataTable(mainStr, null);
        //if(main.Rows.Count==0)
        //    main = server.GetDataTable("select * from ph_main where billCode='" + p1.billcode + "'",null);
        //    if(main.Rows.Count==0)
        //    main=server.GetDataTable("select a.billCode,a.billName,a.flowID,a.stepID,a.billUser,b.bxr,convert(varchar(10),a.billDate,121) as billDate,a.billDept,a.loopTimes,a.isgk,a.gkdept,b.bxmxlx,b.bxzy,b.bxsm  from bill_main a  inner join bill_ybbxmxb b on a.billCode=b.billCode and a.billCode='" + p1.billcode + "'",null);
        //检查是否月结
        string bxyd = (main.Rows[0]["billDate"]).ToString().Substring(0, 7);
        QueryManger query = new QueryManger();
        if (query.CheckYj(bxyd) != "")
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-7");
            return;
        }

        //判断部门总金额是否等于科目花费金额
        bool IsSumTax = new Bll.ConfigBLL().GetValueByKey("TaxSwitch").Equals("0") ? false : true;



        //检查预算科目是否有核算
        Bill_Yskm yskm = new Bill_Yskm();
        SysManager sm = new SysManager();
        yskm = sm.GetYskmByCode(PubMethod.SubString(p1.km));
        //检查部门核算
        if (yskm.BmHs == "1")
        {
            if (p1.bm.Length < 1)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-5");
                return;
            }
        }
        //检查项目核算
        if (yskm.XmHs == "1")
        {
            if (p1.xm.Length < 1)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-6");
                return;
            }
        }



        //判断核算部门是否等于核算科目金额
        if (p1.bm.Length > 0)
        {
            decimal sumbmje = p1.bm.Sum(p => p.bmje);
            if (sumbmje != p1.je)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("科目部门金额与科目金额不相等,相差" + (p1.je - sumbmje).ToString());
                return;
            }
        }

        //判断核算项目是否等于核算科目金额
        if (p1.xm.Length > 0)
        {
            decimal sumxmje = p1.xm.Sum(p => p.xmje);

            if (sumxmje != p1.je)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("科目项目金额与科目金额不相等,相差" + (p1.je - sumxmje).ToString());
                return;
            }
        }

        //判断 状态为受控制的项目金额是否超支
        //获取归口部门(只获取需要控制预算的项目)
        string hsxmModel = server.GetCellValue("select isnull(avalue,'') from T_config where akey='YbbxHsxmMode'");
        string tt = Convert.ToDateTime(main.Rows[0]["billDate"]).Year.ToString();
        if (hsxmModel == "1")
        {
            for (int i = 0; i < p1.xm.Length; i++)
            {
                string xmtemp = PubMethod.SubString(p1.xm[i].xmbh);
                DataTable xmdt = server.GetDataTable("select * from bill_xm_dept_nd where xmDept='" + Convert.ToString(main.Rows[0]["gkdept"]) + "'  and nd='" + tt + "' and  xmCode ='" + xmtemp + "'", null);
                if (xmdt == null || xmdt.Rows.Count == 0)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("核算项目" + p1.xm[i].xmbh + "不合法");
                    return;
                }
                if (xmdt.Rows[0]["isCtrl"].ToString() == "1")
                {
                    decimal xmzje = Convert.ToDecimal(server.GetCellValue("select isnull(SUM(je),'0') as je  from bill_ybbxmxb_hsxm where xmCode='" + xmtemp + "'and kmmxGuid in(select mxGuid from bill_main,bill_ybbxmxb_fykm  where bill_main.billCode=bill_ybbxmxb_fykm.billCode and flowid in ('ybbx','qtbx','gkbx') and billDate>='" + tt + "-01-01' and billDate<='" + tt + "-12-31' )"));
                    decimal kyje = Convert.ToDecimal(xmdt.Rows[0]["je"]) - xmzje;
                    if (kyje - p1.xm[i].xmje < 0)
                    {
                        context.Response.ContentType = "text/plain";
                        context.Response.Write(p1.xm[i].xmbh + "项目金额超支" + (kyje - p1.xm[i].xmje).ToString());
                        return;
                    }

                }
            }
        }
        //是否冲减预算  
        Bill_DataDic billDic = (new SysManager()).GetDicByTypeCode("02", Convert.ToString(main.Rows[0]["bxmxlx"]));
        if (billDic.Cjys == "1" && boHasBudgetControl)//冲减预算必须还要启用预算管理
        {
            //冲减预算
            YsManager ysMgr = new YsManager();
            string nd = tt; ;
            string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];

            decimal kcje = p1.je;
            string kmCode = PubMethod.SubString(p1.km);
            string depCode = Convert.ToString(main.Rows[0]["gkdept"]);

            string gcbh = ysMgr.GetYsgcCode(Convert.ToDateTime(main.Rows[0]["billDate"]));
            //是否超出预算的控制 edit by lvcc 之前没有控制
            decimal ysje = ysMgr.GetYueYs(gcbh, depCode, kmCode);
            decimal hfje = ysMgr.GetYueHf(gcbh, depCode, kmCode);
            decimal syje = 0;

            // 2014-03-17回滚预算控功能添加---beg
            decimal rollsy = 0;
            //YsManager ysmgr = new YsManager();
            //bool IsRollCtrl = new Bll.ConfigBLL().GetValueByKey("IsRollCtrl").Equals("1");
            //if (IsRollCtrl)
            //{
            //    rollsy += ysmgr.GetRollSy(gcbh, depCode, kmCode);
            //}
            //2014-03-17--end

            //if (strtype.Equals("add"))
            //{
            syje = ysje - hfje + rollsy;
            //}
            //else if (strtype.Equals("edit"))
            //{
            //    syje = ysje - hfje + kcje + rollsy;
            //}
            //else
            //{//未发现操作状态 是添加还是修改
            //    context.Response.ContentType = "text/plain";
            //    context.Response.Write("-9");
            //    return;
            //}
            //context.Response.Write("<script>alert(" + ysje + ");</script>");
            decimal billje = p1.je;
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

        DataRow dr = main.Rows[0];
        string billCode = Convert.ToString(dr["BillCode"]);

        Bill_Main mainM = new Bill_Main();
        mainM.BillCode = billCode;
        mainM.BillName = Convert.ToString(dr["BillName"]);
        mainM.FlowId = Convert.ToString(dr["FlowId"]);
        mainM.StepId = Convert.ToString(dr["StepId"]);
        mainM.BillUser = Convert.ToString(dr["billUser"]);

        mainM.BillDate = Convert.ToDateTime(dr["BillDate"]);
        mainM.BillDept = Convert.ToString(dr["BillDept"]);
        mainM.BillJe = p1.bm.Sum(p => p.bmje);

        mainM.BillUser = Convert.ToString(dr["BillUser"]);

        mainM.LoopTimes = Convert.ToInt32(dr["LoopTimes"]);

        mainM.IsGk = Convert.ToString(dr["IsGk"]);
        mainM.GkDept = Convert.ToString(dr["GkDept"]);
        mainM.BillJe = p1.je;


        Bill_Ybbxmxb ybbxmxb = new Bill_Ybbxmxb();
        if (p1.type == "add")
        {
            ybbxmxb.BillCode = billCode;
            ybbxmxb.Bxr = Convert.ToString(dr["bxr"]);
            ybbxmxb.Bxzy = Convert.ToString(dr["bxzy"]);
            ybbxmxb.Bxsm = Convert.ToString(dr["bxsm"]);
            ybbxmxb.Ytje = 0;
            ybbxmxb.Ybje = 0;
            ybbxmxb.Sfgf = "0";
            ybbxmxb.Bxmxlx = Convert.ToString(dr["bxmxlx"]);
        }



        Bill_Ybbxmxb_Fykm fykm = new Bill_Ybbxmxb_Fykm();
        fykm.BillCode = billCode;
        fykm.Fykm = p1.km;

        //通过配置项控制税额是否进费用 修改于20130121（Lvcc）  20121016次修改作废
        if (IsSumTax)
        {
            fykm.Je = p1.je + p1.se;
        }
        else
        {
            fykm.Je = p1.je;//+ p1.list[i].修改于20121016，税额不进费用
        }
        fykm.MxGuid = (new webBillLibrary.GuidHelper()).getNewGuid();
        fykm.Se = p1.se;
        fykm.Status = "0";




        IList<Bill_Ybbxmxb_Fykm_Dept> hsbmList = new List<Bill_Ybbxmxb_Fykm_Dept>();
        for (int j = 0; j < p1.bm.Length; j++)
        {
            Bill_Ybbxmxb_Fykm_Dept dept = new Bill_Ybbxmxb_Fykm_Dept();
            dept.DeptCode = PubMethod.SubString(p1.bm[j].bmbh);
            for (int k = 0; k < hsbmList.Count; k++)
            {
                if (hsbmList[k].DeptCode == dept.DeptCode)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("部门" + p1.bm[j].bmbh + "重复");
                    return;
                }
            }
            dept.Je = p1.bm[j].bmje;
            dept.KmmxGuid = fykm.MxGuid;
            dept.MxGuid = (new webBillLibrary.GuidHelper()).getNewGuid();
            dept.Status = "0";
            hsbmList.Add(dept);
        }


        IList<Bill_Ybbxmxb_Hsxm> hsxmList = new List<Bill_Ybbxmxb_Hsxm>();
        for (int j = 0; j < p1.xm.Length; j++)
        {
            Bill_Ybbxmxb_Hsxm xm = new Bill_Ybbxmxb_Hsxm();
            xm.XmCode = PubMethod.SubString(p1.xm[j].xmbh);
            for (int k = 0; k < hsxmList.Count; k++)
            {
                if (hsxmList[k].XmCode == xm.XmCode)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("项目" + p1.xm[j].xmbh + "重复");
                    return;
                }
            }
            xm.Je = p1.xm[j].xmje;
            xm.KmmxGuid = fykm.MxGuid;
            xm.MxGuid = (new webBillLibrary.GuidHelper()).getNewGuid();
            hsxmList.Add(xm);
        }

        Dal.Bills.MainDal dal = new Dal.Bills.MainDal();
        bool result;
        if (p1.type == "add")
        {
            result = dal.Ph_Add(mainM, ybbxmxb, fykm, hsbmList, hsxmList);
        }
        else
        {
            result = dal.Ph_AddMx(mainM, fykm, hsbmList, hsxmList);
        }
        if (result)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("1");
        }
        else
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



    class YbbxBillTemp
    {
        public string km { get; set; }
        public decimal je { get; set; }
        public decimal se { get; set; }
        public string billcode { get; set; }
        public string type { get; set; }
        public BmTemp[] bm { get; set; }
        public XmTemp[] xm { get; set; }
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