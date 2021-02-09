<%@ WebHandler Language="C#" Class="YstzBillSave" %>

using System;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Linq;

public class YstzBillSave : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        AllIn p1;
        bool boYstzNeedAudit = new Bll.ConfigBLL().GetValueByKey("YstzNeedAudit").Equals("0") ? false : true;
        try
        {
            byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
            string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
            //反序列化json需要framework3.5sp1
            using (StringReader sr = new StringReader(jsonStr))
            {
                JsonSerializer serializer = new JsonSerializer();
                p1 = (AllIn)serializer.Deserialize(new JsonTextReader(sr), typeof(AllIn));
            }
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            //反序列化失败
            context.Response.Write("-1");
            return;
        }

        IList<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
        string userCode = Convert.ToString(context.Session["userCode"]);
        UserMessage userMgr = new UserMessage(userCode);
        string deptCode = p1.tzbm;
        YsManager ysMgr = new YsManager();
        Bill_Main main = new Bill_Main();
        main.BillCode = p1.billcode;
        main.BillDept = deptCode;
        main.BillUser = userCode;
        main.FlowId = "ystz";
        main.BillName2 = p1.zy;
            
        if (boYstzNeedAudit)
        {
            main.StepId = "-1";
        }
        else
        {
            main.StepId = "end";
        }

        Bill_Main oldMain = new BillManager().GetMainByCode(p1.billcode);
        IList<Bill_Ysmxb> oldList = new List<Bill_Ysmxb>();
        if (oldMain != null)
        {
            main.BillDate = oldMain.BillDate;
            oldList = ysMgr.GetYsmxByCode(p1.billcode);
        }
        else
        {
            main.BillDate = DateTime.Now;
        }
        string tgcbh = p1.forgcbh;//ysMgr.GetYsgcCode(DateTime.Now);
       
        int i = 1;

        foreach (BillArray temp in p1.list)
        {
            //检测金额是否超支
            decimal ysje = ysMgr.GetYueYs(temp.gcbh, deptCode, temp.kmbh);
            decimal hfje = ysMgr.GetYueHf(temp.gcbh, deptCode, temp.kmbh);
            decimal zyje = -ysMgr.GetYueNotEndje(temp.gcbh, deptCode, temp.kmbh);
            decimal ktzje = ysje - zyje - hfje;
            //修改扣除本身的金额
            if (oldMain != null)
            {
                var editList = from linq in oldList
                               where linq.Yskm == temp.kmbh && linq.Gcbh == temp.gcbh
                               select linq.Ysje;
                foreach (decimal oldje in editList)
                {
                    ktzje = ktzje - oldje;
                }
            }

            if (ktzje < temp.je)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(i.ToString());
                return;
            }
            i++;

            Bill_Ysmxb sYs = new Bill_Ysmxb();
            sYs.BillCode = p1.billcode;
            sYs.Gcbh = temp.gcbh;
            sYs.YsDept = deptCode;
            sYs.Ysje = -temp.je;
            sYs.Yskm = temp.kmbh;
            sYs.YsType = "3";
            list.Add(sYs);

            Bill_Ysmxb tYs = new Bill_Ysmxb();
            tYs.BillCode = p1.billcode;
            tYs.Gcbh = tgcbh;
            tYs.YsDept = deptCode;
            tYs.Ysje = temp.je;
            tYs.Yskm = temp.kmbh;
            tYs.YsType = "3";
         
            list.Add(tYs);
            main.BillJe += temp.je;
        }
        try
        {
            if (main.BillJe == 0)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-0");
            }
            else
            {
                ysMgr.InsertYsmx(list, main);
                context.Response.ContentType = "text/plain";
                context.Response.Write("0");
            }
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("-2");
        }
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    class AllIn
    {
        public BillArray[] list { get; set; }
        public string billcode { get; set; }
        public string tzbm { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string zy { get; set; }
        /// <summary>
        /// 目标预算过程
        /// </summary>
        public string forgcbh { get; set; }
    }
    class BillArray
    {
        public string gcbh { get; set; }
        public string kmbh { get; set; }
        public decimal je { get; set; }
    }

}