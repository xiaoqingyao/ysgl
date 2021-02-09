<%@ WebHandler Language="C#" Class="Ykyshc" %>

using System;
using System.Web;

public class Ykyshc : IHttpHandler
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    public void ProcessRequest(HttpContext context)
    {

        try
        {
            System.Collections.Generic.List<string> listsql = new System.Collections.Generic.List<string>();
            string bxdbillcode = context.Request["bxdbillcode"];
            string usercode = context.Request["billuser"];
            string hcje =context.Request["hcje"];

            string strsql = @"update bill_main set billJe=(select billJe from bill_main where
                        billCode='" + bxdbillcode + "') where billName=(select yksq_code from dz_yksq_bxd where bxd_code='" + bxdbillcode + "')";
            listsql.Add(strsql);
            string strupmxsql = @"  update bill_ybbxmxb_fykm set je=
                                     (
                                     select je from bill_ybbxmxb_fykm where
                                    billCode='" + bxdbillcode + "'  )    where billCode= (select billCode from bill_main where billName=  (select yksq_code from dz_yksq_bxd where bxd_code='" + bxdbillcode + "')  )";

            listsql.Add(strupmxsql);
            string strupsql = @" Update dz_yksq_bxd set  note2='回冲人:" + usercode + ",冲金额 :" + hcje + "回冲时间:" + DateTime.Now.ToString() + "'where bxd_code='" + bxdbillcode + "'";

            listsql.Add(strupsql);
             int introws =0;
            if (listsql.Count > 0)
            {
               introws= server.ExecuteNonQuerys(listsql.ToArray());
            }



        
            if (introws > 0)
            {

            
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("ok");
             
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("wrong");
            }

        }
        catch (Exception)
        {

            context.Response.ContentType = "text/plain";
            context.Response.Write("wrong");
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}