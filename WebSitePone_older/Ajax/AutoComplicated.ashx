<%@ WebHandler Language="C#" Class="AutoComplicated" %>

using System;
using System.Web;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Web.Script.Serialization;
public class AutoComplicated : IHttpHandler
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public void ProcessRequest(HttpContext context)
    {

        string requestType = context.Request.Params["type"];
        string search = context.Request.Params["search"];
        string otherParm = context.Request.Params["otherParm"];
        string result = "";
        switch (requestType)
        {
            case "hsbm":
                result = GetHsbm(search);
                break;
            case "hsxm":
                result = GetHsxm(search, otherParm);
                break;
            case "userDept":
                result = GetUserDept(search);
                break;
            case "user":
                result = GetUser(search, otherParm);
                break;
            default:
                break;
        }
        context.Response.ContentType = "text/plan";
        context.Response.Write(result);
    }

    private string GetUser(string code, string otherParm)
    {
        string sql = "select '['+usercode+']'+username as Name  from bill_users  where (usercode  like '%" + code + "%' or userName like '%" + code + "%' ) and userStatus !='0' ";
        if (!string.IsNullOrEmpty(otherParm))
        {
            string[] userArr = otherParm.Split(',');
            string travelPersons = "";
            for (int i = 0; i < userArr.Length; i++)
            {

                travelPersons += "'" + PubMethod.SubString(userArr[i]) + "',";
            }
            travelPersons = travelPersons.Substring(0, travelPersons.Length - 1);
            sql+="  and userCode not in ("+travelPersons+")";
        }
        sql += " order by userCode asc";
        DataTable dt = server.GetDataTable(sql, null);
        IList<string> ret = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ret.Add(dt.Rows[i]["Name"].ToString());
        }
        if (ret.Count > 0)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(ret);
        }
        else
            return "";

    }
    private string GetUserDept(string usercode)
    {
        usercode = PubMethod.SubString(usercode);
        string dept = server.GetCellValue("select userDept from bill_users where usercode='" + usercode + "'");

        if (!string.IsNullOrEmpty(usercode) && !string.IsNullOrEmpty(dept))
            return server.GetCellValue("select '['+deptcode+']'+deptname as Dept from bill_departments where deptCode='" + dept + "'");
        else
            return "-1";

    }
    private string GetHsbm(string text)
    {
        DataTable dt = server.GetDataTable("select '['+deptCode+']'+deptName as Name,deptCode  from bill_departments  where sjDeptCode=(select top 1 deptCode from bill_departments where sjDeptCode='') and deptStatus!='0' and deptCode like '%" + text + "%' order by deptCode asc ", null);
        //核算部门是否必须是末级部门配置项
        IList<string> ret = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (server.GetCellValue("select  avalue  from T_Config where akey='HsDeptIsLast' ") != "1")
            {
                ret.Add(dt.Rows[i]["Name"].ToString());
            }
            DataTable temp = server.GetDataTable("select  '['+deptCode+']'+deptName as Name from bill_departments where sjDeptCode='" + dt.Rows[i]["deptCode"].ToString() + "' and deptStatus!='0'  and deptCode like '%" + text + "%' order by deptCode asc  ", null);
            for (int j = 0; j < temp.Rows.Count; j++)
            {
                ret.Add(temp.Rows[j]["Name"].ToString());
            }
        }
        if (ret.Count > 0)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            return jserializer.Serialize(ret);
        }
        else
            return "";
    }


    private string GetHsxm(string text, string billCode)
    {
        DataTable dt = server.GetDataTable("select billDept,left(convert(varchar(10),billDate,121),4) as nd,billDate from bill_main where billCode='" + billCode + "'", null);
        if (dt.Rows.Count == 0)
            dt = server.GetDataTable("select billDept,left(convert(varchar(10),billDate,121),4) as nd,billDate from ph_main where billCode='" + billCode + "'", null);
        string sql = "select (select distinct '['+xmCode+']'+xmName as Name from bill_xm where xmcode= a.xmcode)as Name from bill_xm_dept_nd  a where  a.status='1'  and    a.nd='" + dt.Rows[0]["nd"].ToString() + "'  and a.xmDept=(select top 1 deptCode from bill_departments where deptCode='" + dt.Rows[0]["billDept"].ToString() + "' and isnull(deptStatus,'1')!='0') and a.xmCode like '%" + text + "%' ";
        dt = server.GetDataTable(sql, null);

        JavaScriptSerializer jserializer = new JavaScriptSerializer();
        return jserializer.Serialize(ExchangeData.DataTableToList(dt, "Name"));
    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}