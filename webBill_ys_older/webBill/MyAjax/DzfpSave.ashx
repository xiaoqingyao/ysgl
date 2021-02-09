<%@ WebHandler Language="C#" Class="DzfpSave" %>

using System;
using System.Web;

public class DzfpSave : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        AllInfp p1;
        bool boYstzNeedAudit = new Bll.ConfigBLL().GetValueByKey("YstzNeedAudit").Equals("0") ? false : true;
        try
        {
            byte[] bin = context.Request.BinaryRead(context.Request.ContentLength);
            string jsonStr = System.Text.Encoding.UTF8.GetString(bin);
            //反序列化json需要framework3.5sp1
            using (System.IO.StringReader sr = new System.IO.StringReader(jsonStr))
            {
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                p1 = (AllInfp)serializer.Deserialize(new Newtonsoft.Json.JsonTextReader(sr), typeof(AllInfp));
            }
        }
        catch
        {
            context.Response.ContentType = "text/plain";
            //反序列化失败
            context.Response.Write("-1");
            return;
        }

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        string userCode = Convert.ToString(context.Session["userCode"]);
        Bll.UserProperty.UserMessage userMgr = new Bll.UserProperty.UserMessage(userCode);
        string deptCode = p1.deptCode;
        Bll.UserProperty.YsManager ysMgr = new Bll.UserProperty.YsManager();
        //bill_main
        list.Add("delete bill_main where billcode='"+p1.billCode+"'");
        list.Add("delete bill_fpfj where billcode='" + p1.billCode + "'");
        list.Add("delete bill_fpfjs where billcode='" + p1.billCode + "'");
      

        //发票主表
        string strfprq = p1.fprq;
        string strdeptCode = p1.deptCode;
        string strdeptname = p1.deptName;
       
        string fpusercode = p1.fpusercode;
        string fpusername = p1.fpusername;
        string strbz = p1.bz;
        list.Add("insert into bill_fpfj (fprq,billCode,deptCode,deptName,fpusercode,fpusername,bz) values('" + strfprq + "','" + p1.billCode + "','" + strdeptCode + "','" + strdeptname + "','" + fpusercode + "','" + fpusername + "','" + strbz + "')");
        decimal deczje = 0;
        //===子表
        foreach (BillfpArray temp in p1.list)
        {
            string strfph = temp.fpdh;
            string strfpdw = temp.fpdw;
            decimal fpje = temp.fpje;
            string strfpbz = temp.fpbz;
            deczje += temp.fpje;
            list.Add("insert into bill_fpfjs (billCode,fph,fpdw,fpje,bz) values('" + p1.billCode + "','" + strfph + "','" + strfpdw + "','" + fpje + "','" + strfpbz + "')");
 
        }
        list.Add("insert into bill_main (billCode,billName,flowid,stepid,billDept,billUser,billDate,billJe,LoopTimes,billType) values('" + p1.billCode + "','电子发票单','dzfp','end','" + deptCode + "','" + userCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + deczje + "','1','5')");

        try
        {
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("-0");
            }
            else
            {
                //ysMgr.InsertYsmx(list, main);
                context.Response.ContentType = "text/plain";
                context.Response.Write("1");
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

    class AllInfp
    {
        public string billCode { get; set; }
        public string fprq { get; set; }
        public string deptCode { get; set; }
        public string deptName { get; set; }
        public string fpusercode { get; set; }
        public string fpusername { get; set; }
        public string bz { get; set; }
        public BillfpArray[] list { get; set; }

    }
    class BillfpArray
    {
        public string fpdh { get; set; }
        public string fpdw { get; set; }
        public decimal fpje { get; set; }
        public string fpbz { get; set; }
    }
}