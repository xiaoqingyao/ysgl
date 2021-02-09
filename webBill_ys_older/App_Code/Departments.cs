using System;
using System.Collections.Generic;

using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

/// <summary>
///Departments 的摘要说明
/// </summary>
public class Departments : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public Departments()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 归口部门
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="otherParameter"></param>
    /// <param name="showChk"></param>
    /// <param name="officeImgUrl"></param>
    /// <param name="deptStatus"></param>
    /// <param name="showUser"></param>
    /// <param name="userUrl"></param>
    /// <param name="userTarget"></param>
    /// <param name="userImageUrl"></param>


    public void BindOfficeGk(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string deptStatus, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj + " and Isgk='Y'";
        //if (Session["userCode"].ToString().Trim() == "admin")
        //{
        //    sql += " and isnull(sjDeptCode,'')=''";
        //}
        //else
        //{
        //    if (otherParameter.IndexOf("All") == -1)
        //    {
        //        if (server.GetDataSet("select * from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "'").Tables[0].Rows.Count == 0)//未分配权限
        //        {
        //            sql += " and deptCode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')";
        //        }
        //        else
        //        {
        //            sql += " and deptCode in (select objectID from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "')";
        //        }
        //    }
        //}
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim();

            if (url != "")
            {
                if (true)
                {

                }
                tNode.NavigateUrl = url + "?deptCode=" + tNode.Value + "&deptName=" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim() + otherParameter;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevelGk(tNode, url, target, otherParameter, showChk, officeImgUrl, deptStatus, showUser, userUrl, userTarget, userImageUrl);
        }
    }


    public void BindOfficeNextLevelGk(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string deptStatus, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj + "and Isgk='Y' and sjDeptCode='" + pNode.Value + "' order by deptCode asc ";
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim();

            if (url != "")
            {
                tNode.NavigateUrl = url + "?deptCode=" + tNode.Value + "&deptName=" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim() + otherParameter;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevelGk(tNode, url, target, otherParameter, showChk, officeImgUrl, deptStatus, showUser, userUrl, userTarget, userImageUrl);

        }
    }
    /// <summary>
    /// 绑定单位的一级部门
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="otherParameter"></param>
    /// <param name="showChk"></param>
    /// <param name="OrgImgUrl"></param>
    /// <param name="officeImgUrl"></param>
    /// <param name="deptStatus">格式： '1','2','0'</param>
    public void BindOffice(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string deptStatus, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj;
        if (Session["userCode"].ToString().Trim() == "admin")
        {
            sql += " and isnull(sjDeptCode,'')=''";
        }
        else
        {
            if (otherParameter.IndexOf("All") == -1)
            {
                if (server.GetDataSet("select * from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "'").Tables[0].Rows.Count == 0)//未分配权限
                {
                    sql += " and deptCode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')";
                }
                else
                {
                    sql += " and deptCode in (select objectID from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "')";
                }
            }
        }
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim();

            if (url != "")
            {
                if (true)
                {
                    
                }
                tNode.NavigateUrl = url + "?deptCode=" + tNode.Value + "&deptName=" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim() + otherParameter;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevel(tNode, url, target, otherParameter, showChk, officeImgUrl, deptStatus, showUser, userUrl, userTarget, userImageUrl);
        }
    }

    /// <summary>
    /// 递归绑定下级部门
    /// </summary>
    /// <param name="orgCode"></param>
    /// <param name="pNode"></param>
    /// <param name="officeDataSet"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="otherParameter"></param>
    /// <param name="showChk"></param>
    /// <param name="OrgImgUrl"></param>
    /// <param name="officeImgUrl"></param>
    /// <param name="deptStatus">格式： '1','2','0'</param>
    public void BindOfficeNextLevel(TreeNode pNode, string url, string target, string otherParameter, bool showChk, string officeImgUrl, string deptStatus, bool showUser, string userUrl, string userTarget, string userImageUrl)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }

        string sql = "select * from bill_departments where 1=1 " + deptStatusTj + " and sjDeptCode='" + pNode.Value + "' order by deptCode asc ";
        DataSet officeDataSet = server.GetDataSet(sql);

        for (int i = 0; i <= officeDataSet.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim() + "]" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim();
            tNode.Value = officeDataSet.Tables[0].Rows[i]["deptCode"].ToString().Trim();

            if (url != "")
            {
                tNode.NavigateUrl = url + "?deptCode=" + tNode.Value + "&deptName=" + officeDataSet.Tables[0].Rows[i]["deptName"].ToString().Trim() + otherParameter;
                tNode.Target = target;
            }
            tNode.ShowCheckBox = showChk;
            if (officeImgUrl != "")
            {
                tNode.ImageUrl = officeImgUrl + "office.gif";
            }
            if (showUser == true)
            {
                this.bindOfficeUser(tNode, userUrl, userTarget, userImageUrl, otherParameter);
            }
            pNode.ChildNodes.Add(tNode);
            this.BindOfficeNextLevel(tNode, url, target, otherParameter, showChk, officeImgUrl, deptStatus, showUser, userUrl, userTarget, userImageUrl);

        }
    }

    public void bindOfficeUser(TreeNode pNode, string userUrl, string target, string imgUrl, string otherParameter)
    {
        DataSet temp = server.GetDataSet("select * from bill_users where userDept='" + pNode.Value + "' and userStatus='1'");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode tNode = new TreeNode();
            tNode.Text = "[" + temp.Tables[0].Rows[i]["userCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["userName"].ToString().Trim();
            tNode.Value = temp.Tables[0].Rows[i]["userCode"].ToString().Trim();

            if (userUrl != "")
            {
                tNode.NavigateUrl = userUrl + "?userCode=" + tNode.Value + otherParameter;
                tNode.Target = target;
            }
            if (userUrl != "")
            {
                tNode.ImageUrl = imgUrl + "user.gif";
            }
            pNode.ChildNodes.Add(tNode);
        }
    }

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
            returnStr = "'" + returnStr.Replace(",", "','") + "'";
        }
        return returnStr;
    }
    /// <summary>
    /// 获取人员权限内的全部单位编号信息
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="deptStatus"></param>
    /// /// <param name="IsSell">是否是销售公司 1是0否</param>
    /// <returns></returns>
    public string GetUserRightDepartments(string userCode, string deptStatus, string IsSell)
    {
        string deptStatusTj = "";
        if (deptStatus == "")
        { }
        else
        {
            deptStatusTj = " and deptStatus in (" + deptStatus + ")";
        }
        string strIsSell = "";
        if (IsSell == "1")
        {
            strIsSell = "and IsSell='Y' ";
        }
        string sql = "select * from bill_departments where 1=1 " + deptStatusTj + strIsSell;
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
            returnStr = "'" + returnStr.Replace(",", "','") + "'";
        }
        else
        {
            returnStr = "''";
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
    public string GetZgDepartments(string userCode)
    {
        DataSet temp = server.GetDataSet("select * from bill_dept_ywzg where userCode='" + userCode + "'");
        string returnStr = "";
        if (temp.Tables[0].Rows.Count == 0)//没有设置主管单位 则默认管理本单位及其下级
        {
            return this.GetDepartments(server.GetCellValue("select userDept from bill_users where userCode='" + userCode + "'"), "");
        }
        else
        {
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
            return "'" + deptCode + "'";
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
            returnStr = "'" + returnStr.Replace(",", "','") + "'";
            return returnStr;
        }
    }

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
    /// 获取人员权限内的全部单位信息
    /// </summary>
    /// <param name="userCode"></param>
    /// <param name="deptStatus"></param>
    /// <returns></returns>
    public DataTable GetUserRightDepartmentsDT(string userCode, string deptStatus)
    {
        DataTable dtRel = new DataTable();
        string strDeptCodes = this.GetUserRightDepartments(userCode, deptStatus);
        if (userCode == "")
        {
            return null;
        }
        dtRel = server.GetDataTable("select * from bill_departments where  sjdeptCode='000001' and deptCode in (" + strDeptCodes + ")", null);
        return dtRel;
    }
}