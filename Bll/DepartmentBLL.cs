using System;
using System.Collections.Generic;

using System.Web;
using System.Data;
using System.Data.SqlClient;
using Dal;
using Models;

/// <summary>
///Departments 的摘要说明
/// </summary>
public class DepartmentBLL 
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    /// <summary>
    /// 获取人员权限内的全部单位编号信息
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="deptStatus"></param>
    /// <returns></returns>
    public string GetUserRightDepartments(string userCode, string deptStatus)
    {
        string deptStatusTj = "";
        if (deptStatus == "") 
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj;
        if (userCode == "admin")
        {
            sql += " and isnull(sjDeptCode,'')=''";
        }
        else
        {
            if (server.GetDataSet("select * from bill_userRight where rightType='2' and userCode='" + userCode + "'").Tables[0].Rows.Count == 0)//未分配权限
            {
                sql += " and deptCode=(select userDept from bill_users where userCode='" + userCode + "')";
            }
            else
            {
                sql += " and deptCode in (select objectID from bill_userRight where rightType='2' and userCode='" + userCode + "')";
            }
        }
        string returnStr = "";
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
            this.GetNextLevelDepartments2(ref returnStr, temp.Tables[0].Rows[i]["deptCode"].ToString().Trim(), deptStatus);
        }
        if (returnStr != "")
        {
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
        }
        return returnStr;
    }
    /// <summary>
    /// 获取人员权限内的全部单位编号信息
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="deptStatus"></param>
    /// <returns></returns>
    public string GetSearchDepartments(string userCode, string deptStatus)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj;
        if (userCode == "admin")
        {
            sql += " and isnull(sjDeptCode,'')=''";
        }
        else
        {
            if (server.GetDataSet("select * from bill_searchRight where userCode='" + userCode + "'").Tables[0].Rows.Count == 0)//未分配权限
            {
                sql += " and deptCode=(select userDept from bill_users where userCode='" + userCode + "')";
            }
            else
            {
                sql += " and deptCode in (select deptCode from bill_searchRight where userCode='" + userCode + "')";
            }
        }
        string returnStr = "";
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
            this.GetNextLevelDepartments2(ref returnStr, temp.Tables[0].Rows[i]["deptCode"].ToString().Trim(), deptStatus);
        }
        if (returnStr != "")
        {
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
        }
        return returnStr;
    }
    public string GetDepartments(string deptCode, string deptStatus)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj;

        sql += " and deptCode='" + deptCode + "'";
            
        string returnStr = "";
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
            this.GetNextLevelDepartments2(ref returnStr, temp.Tables[0].Rows[i]["deptCode"].ToString().Trim(), deptStatus);
        }
        if (returnStr != "")
        {
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
        }
        return returnStr;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public string GetZgDepartments(string userCode)
    {
        DataSet temp = server.GetDataSet("select * from bill_dept_ywzg where userCode='" + userCode + "'");
        string returnStr = "";
        if (temp.Tables[0].Rows.Count == 0)//没有设置主管单位 则默认管理本单位及其下级
        {
            return this.GetDepartments(server.GetCellValue("select userDept from bill_users where userCode='" + userCode + "'"), "");
        }
        else {
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
            }
        }
        if (returnStr != "")
        {
            returnStr = returnStr.Substring(0, returnStr.Length - 1);
        }
        return returnStr;
    }
    /// <summary>
    /// 获取下级单位编号信息
    /// </summary>
    /// <param name="deptCode"></param>
    /// <param name="deptStatus">格式： '1','2','0'</param>
    /// <param name="nextLevel">true:false 是否包含下级</param>
    /// <returns>000001，000002</returns>
    public string GetNextLevelDepartments(string deptCode, string deptStatus, bool nextLevel)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }
        string returnStr = "";
        if (nextLevel == false)
        {
            return deptCode;
        }
        else
        {
            DataSet temp = server.GetDataSet("select * from bill_departments where sjDeptCode='" + deptCode + "'" + deptStatusTj);
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
                this.GetNextLevelDepartments2(ref returnStr, temp.Tables[0].Rows[i]["deptCode"].ToString().Trim(), deptStatus);
            }
            returnStr += deptCode;
            return returnStr;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="returnStr"></param>
    /// <param name="deptCode"></param>
    /// <param name="deptStatus"></param>
    private void GetNextLevelDepartments2(ref string returnStr, string deptCode, string deptStatus)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }
        DataSet temp = server.GetDataSet("select * from bill_departments where sjDeptCode='" + deptCode + "'" + deptStatusTj);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            returnStr += temp.Tables[0].Rows[i]["deptCode"].ToString().Trim() + ",";
            this.GetNextLevelDepartments2(ref returnStr, temp.Tables[0].Rows[i]["deptCode"].ToString().Trim(), deptStatus);
        }
    }

    /// <summary>
    /// 根据编号 获取用于显示的名称
    /// </summary>
    /// <returns></returns>
    public string GetShowNameByCode(string strDeptCode) {
        string strSql = "select '['+deptCode+']'+deptName from bill_departments where deptCode=@deptCode ";
        SqlParameter[] arrSp=new SqlParameter[1];
        arrSp[0] = new SqlParameter("@deptCode",strDeptCode);
        object objRel = DataHelper.ExecuteScalar(strSql, arrSp, false);
        return objRel == null ? "" : objRel.ToString();
    }



}