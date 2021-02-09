<%@ WebHandler Language="C#" Class="GetDept" %>

using System;
using System.Web;
using Bll.UserProperty;
using System.Collections.Generic;
using Models;
using System.Text;

public class GetDept : IHttpHandler {
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Dal.ConfigDal configdal = new Dal.ConfigDal();
    private Dal.UserProperty.DepartmentDal depDal = new Dal.UserProperty.DepartmentDal();
    public void ProcessRequest (HttpContext context) {
        string action = context.Request.Params["action"];
        string code = context.Request.Params["code"];
        code = HttpUtility.UrlDecode(code);
        string ret = "";

        //1. 获取配置项中deptjc的值  判断是否是预算到末级

        string strdeptjc = configdal.GetValueByKey("deptjc");
        if (action == "user")
        {
          
            string usercode = code.Split(']')[0].Trim('[');
            if (strdeptjc=="Y")
            {
               // string strsql = @"select (select '['+deptcode+']'+deptname from dbo.bill_departments where deptcode=a.userDept) as showdept from bill_users a where usercode='" + usercode + "' ";
                string strdeptname = depDal.GetDeptByUser(usercode);//server.GetCellValue(strsql);
                if (!string.IsNullOrEmpty(strdeptname))
                {
                    ret = strdeptname;
                }
               
            }
            else
            {
                UserMessage um = new UserMessage(usercode);
                Bill_Departments dept = um.GetRootDept();
                ret = "[" + dept.DeptCode + "]" + dept.DeptName;
            }
           
        }
        else if (action == "gk")
        {
            SysManager smgr = new SysManager();
            IList<Bill_Departments> list = smgr.GetAllRootDept();
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (Bill_Departments dept in list)
            {
                sb.Append("\"[" + dept.DeptCode + "]" + dept.DeptName + "\",");
            }
            sb.Remove(sb.Length-1, 1);
            sb.Append("]");
            ret = sb.ToString();
        }

        else if (action=="gys")
        {
            string dept= context.Request.Params["dept"];
            dept = dept.Substring(1, dept.IndexOf(']') - 1);
            string strDef = server.GetCellValue("select  dicname from bill_datadic where dictype='21' and dicname like '" + code + "|%'  and dicCode='"+dept+"'");
            string[] arrDef=strDef.Split('|');
            if (arrDef.Length>=3)
            {  
               ret="{\"khh\":\""+arrDef[1]+"\",\"zh\":\""+arrDef[2]+"\"}";  
            }
           
        }
        context.Response.ContentType = "text/plain";
        context.Response.Write(ret);
    }


    
    public bool IsReusable {
        get {
            return false;
        }
    }

}