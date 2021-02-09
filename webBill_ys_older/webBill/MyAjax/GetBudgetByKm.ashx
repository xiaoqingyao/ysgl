<%@ WebHandler Language="C#" Class="GetBudgetByKm" %>

using System;
using System.Web;
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class GetBudgetByKm : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string deptCode = context.Request.Params["dept"];
        string type = context.Request.Params["type"];
        //string mbysgc = context.Request.Params["mbysgc"];
        YsManager ysMgr = new YsManager();
        SysManager sysMgr = new SysManager();

        string[] yskm = context.Request.Params["km"].Split('|');
        List<Bill_Yskm> lstAllYskm = new List<Bill_Yskm>();

        for (int i = 0; i < yskm.Length; i++)
        {
            if (!string.IsNullOrEmpty(yskm[i]))
            {
                Bill_Yskm modelyskm = new Bill_Yskm();
                modelyskm.YskmCode = yskm[i];
                modelyskm.YskmBm = yskm[i];
                lstAllYskm.Add(modelyskm);
            }
        }
        sysMgr.SetEndYskmByAll(lstAllYskm);
        List<Bill_Yskm> lstend = new List<Bill_Yskm>();
        sysMgr.GetAllChildren(lstAllYskm, lstend, deptCode);
        //List<Bill_Yskm> lstErJiYskm = new List<Bill_Yskm>();//非终端的预算科目
        //List<Bill_Yskm> lstEndYskm = new List<Bill_Yskm>();//最终的预算科目集合
        //for (int i = 0; i < lstAllYskm.Count; i++)
        //{
        //    if (lstAllYskm[i].IsEnd.Equals("1"))
        //    {
        //        lstEndYskm.Add(lstAllYskm[i]);
        //    }
        //    else {
        //        lstErJiYskm.Add(lstAllYskm[i]);
        //    }
        //}


       

        string nowGcbh = context.Request.Params["mbysgc"];//ysMgr.GetYsgcCode(DateTime.Now);
        IList<Bill_Ysgc> list = ysMgr.GetYsgcByYear(nowGcbh.Substring(0,4));
        StringBuilder sb = new StringBuilder();



        //foreach (string km in yskm)
        //{
        for (int i = 0; i < lstend.Count; i++)
        {
            string km = lstend[i].YskmCode;
            string tempkm = sysMgr.GetYskmNameCode(km);
            foreach (Bill_Ysgc ysgc in list)
            {
                if (ysgc.Gcbh != nowGcbh)
                {
                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(tempkm);//科目
                    sb.Append("</td>");

                    sb.Append("<td>");
                    sb.Append("[" + ysgc.Gcbh + "]" + ysgc.Xmmc);//源预算过程
                    sb.Append("</td>");

                    sb.Append("<td>");
                    decimal ysje = ysMgr.GetYueYs(ysgc.Gcbh, deptCode, km);//预算金额
                    sb.Append(ysje.ToString("N02"));
                    sb.Append("</td>");

                    sb.Append("<td>");
                    decimal hfje = ysMgr.GetYueHf(ysgc.Gcbh, deptCode, km);//花费金额
                    sb.Append(hfje.ToString("N02"));
                    sb.Append("</td>");

                    sb.Append("<td>");
                    decimal zyje = -ysMgr.GetYueNotEndje(ysgc.Gcbh, deptCode, km);//占用金额
                    sb.Append(zyje.ToString("N02"));
                    sb.Append("</td>");

                    sb.Append("<td>");
                    decimal ktzje = ysje - hfje - zyje;//可调整金额
                    sb.Append(ktzje.ToString("N02"));
                    sb.Append("</td>");
                    sb.Append("<td><input type=\"text\" class=\"ysje\" /></td>");

                    sb.Append("</tr>");
                }
            }
        }
        //}



        context.Response.ContentType = "text/plain";
        context.Response.Write(sb.ToString());
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    
}