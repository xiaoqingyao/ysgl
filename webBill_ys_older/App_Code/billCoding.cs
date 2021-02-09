using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// billCoding 的摘要说明
/// </summary>
public class billCoding
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public billCoding()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取新增预算过程编号
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getYsgcCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select gcbh from bill_ysgc where left(gcbh,8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["gcbh"].ToString().Trim();
                if (gcbh.Length == 12)//固定12位编码
                {
                    gcbh = gcbh.Substring(8, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return date + index.ToString().PadLeft(4, '0');
        }
    }
    public string getYsgcCode(string orgGuid)
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select gcbh from bll_ysgc where isnull(orgGuid,'')='" + orgGuid + "' and left(gcbh,8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["gcbh"].ToString().Trim();
                if (gcbh.Length == 12)//固定12位编码
                {
                    gcbh = gcbh.Substring(8, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return date + index.ToString().PadLeft(4, '0');
        }
    }

    /// <summary>
    /// 生成单位编号
    /// </summary>
    /// <returns></returns>
    public string getDeptCode()
    {
        string sql = "select isnull(max(deptCode),0) from bill_departments where isnumeric(deptCode)=1 and len(deptCode)=6";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "000001";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 1000000)
            {
                return "";
            }

            return index.ToString().PadLeft(6, '0');
        }
    }

    /// <summary>
    /// 生成单位编号
    /// </summary>
    /// <returns></returns>
    public string getXmCode()
    {
        string sql = "select isnull(max(xmcode),0) from bill_xm where isnumeric(xmCode)=1 and len(xmcode)=6";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "000001";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 1000000)
            {
                return "";
            }

            return index.ToString().PadLeft(6, '0');
        }
    }

    /// <summary>
    /// 数据字典编号
    /// </summary>
    /// <returns></returns>
    public string getDicCode(string dicType)
    {
        string sql = "select isnull(max(dicCode),0) from bill_datadic where isnumeric(dicCode)=1 and len(dicCode)=2 and dicType='" + dicType + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "01";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 100)
            {
                return "";
            }

            return index.ToString().PadLeft(2, '0');
        }
    }


    /// <summary>
    /// 生成财务科目编号
    /// </summary>
    /// <returns></returns>
    public string getCwkmCode(string pNode)
    {
        string sql = "";
        if (pNode == "")
        {
            sql = "select isnull(max(cwkmCode),0)+1 from bill_cwkm where isnumeric(cwkmCode)=1 and len(cwkmCode)=2";
        }
        else
        {
            sql = "select isnull(max(right(cwkmCode,2)),0)+1 from bill_cwkm where isnumeric(cwkmCode)=1 and len(cwkmCode)=len('" + pNode + "')+2 and left(cwkmcode,len('" + pNode + "'))='" + pNode + "'";
        }


        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return pNode + "01";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim());

            if (index >= 100)
            {
                return "";
            }

            return pNode + index.ToString().PadLeft(2, '0');
        }
    }

    /// <summary>
    /// 生成财务科目编号
    /// </summary>
    /// <returns></returns>
    public string getCwkmCode(string pNode, string orgGuid)
    {
        string sql = "";
        if (pNode == "")
        {
            sql = "select isnull(max(cwkmCode),0)+1 from bll_cwkm where isnull(orgguid,'')='" + orgGuid + "' and isnumeric(cwkmCode)=1 and len(cwkmCode)=2";
        }
        else
        {
            sql = "select isnull(max(right(cwkmCode,2)),0)+1 from bll_cwkm where isnull(orgguid,'')='" + orgGuid + "' and isnumeric(cwkmCode)=1 and len(cwkmCode)=len('" + pNode + "')+2 and left(cwkmcode,len('" + pNode + "'))='" + pNode + "'";
        }


        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return pNode + "01";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim());

            if (index >= 100)
            {
                return "";
            }

            return pNode + index.ToString().PadLeft(2, '0');
        }
    }


    /// <summary>
    /// 生成财务科目编号
    /// </summary>
    /// <returns></returns>
    public string getYskmCode(string pNode)
    {
        string sql = "";
        if (pNode == "")
        {
            sql = "select isnull(max(yskmCode),0)+1 from bill_yskm where isnumeric(yskmCode)=1 and len(yskmCode)=2";
        }
        else
        {
            sql = "select isnull(max(right(yskmCode,2)),0)+1 from bill_yskm where isnumeric(yskmCode)=1 and len(yskmCode)=len('" + pNode + "')+2 and left(yskmCode,len('" + pNode + "'))='" + pNode + "'";
        }


        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return pNode + "01";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim());

            if (index >= 100)
            {
                return "";
            }

            return pNode + index.ToString().PadLeft(2, '0');
        }
    }
    /// <summary>
    /// 生成财务科目编号
    /// </summary>
    /// <returns></returns>
    public string getYskmCode(string pNode, string orgGuid)
    {
        string sql = "";
        if (pNode == "")
        {
            sql = "select isnull(max(yskmCode),0)+1 from bll_yskm where isnull(orgGuid,'')='" + orgGuid + "' and isnumeric(yskmCode)=1 and len(yskmCode)=2";
        }
        else
        {
            sql = "select isnull(max(right(yskmCode,2)),0)+1 from bll_yskm where isnull(orgGuid,'')='" + orgGuid + "' and isnumeric(yskmCode)=1 and len(yskmCode)=len('" + pNode + "')+2 and left(yskmCode,len('" + pNode + "'))='" + pNode + "'";
        }


        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return pNode + "01";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim());

            if (index >= 100)
            {
                return "";
            }

            return pNode + index.ToString().PadLeft(2, '0');
        }
    }


    /// <summary>
    /// 生成人员编号
    /// </summary>
    /// <returns></returns>
    public string getUserCode()
    {
        string sql = "select isnull(max(userCode),0) from bill_users where isnumeric(userCode)=1 and len(userCode)=6";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "000001";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 1000000)
            {
                return "";
            }

            return index.ToString().PadLeft(6, '0');
        }
    }

    /// <summary>
    /// 生成角色编号
    /// </summary>
    /// <returns></returns>
    public string getUserGroupCode()
    {
        string sql = "select isnull(max(groupID),0) from bill_usergroup where isnumeric(groupID)=1 and len(groupID)=4";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "0001";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 10000)
            {
                return "";
            }

            return index.ToString().PadLeft(4, '0');
        }
    }

    /// <summary>
    /// 生成成本中心
    /// </summary>
    /// <returns></returns>
    public string getCbzxCode()
    {
        string sql = "select isnull(max(zxCode),0) from bill_cbzx where isnumeric(zxCode)=1 and len(zxCode)=6";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows[0][0].ToString().Trim() == "0")
        {
            return "000001";
        }
        else
        {
            int index = int.Parse(temp.Tables[0].Rows[0][0].ToString().Trim()) + 1;

            if (index >= 1000000)
            {
                return "";
            }

            return index.ToString().PadLeft(6, '0');
        }
    }


    /// <summary>
    /// 获取新增预算过程编号
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getLscgCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select cgbh from bill_lscg where right(left(cgbh,12),8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return "lscg" + date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["cgbh"].ToString().Trim();
                if (gcbh.Length == 16)//固定12位编码
                {
                    gcbh = gcbh.Substring(12, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return "lscg" + date + index.ToString().PadLeft(4, '0');
        }
    }
    public string getLscgCode(string orgGuid)
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select cgbh from bill_lscg where orgGuid='" + orgGuid + "' and left(cgbh,8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["cgbh"].ToString().Trim();
                if (gcbh.Length == 12)//固定12位编码
                {
                    gcbh = gcbh.Substring(8, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return date + index.ToString().PadLeft(4, '0');
        }
    }
    /// <summary>
    /// 获取新增预算过程编号
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getCgspCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select cgbh from bill_cgsp where right(left(cgbh,12),8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return "cgsp" + date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["cgbh"].ToString().Trim();
                if (gcbh.Length == 16)//固定12位编码
                {
                    gcbh = gcbh.Substring(12, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return "cgsp" + date + index.ToString().PadLeft(4, '0');
        }
    }
    /// <summary>
    /// 获取新增预算过程编号
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getCgspCode(string orgGuid)
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select cgbh from bill_cgsp where orgGuid='" + orgGuid + "' and left(cgbh,8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string gcbh = temp.Tables[0].Rows[i]["cgbh"].ToString().Trim();
                if (gcbh.Length == 12)//固定12位编码
                {
                    gcbh = gcbh.Substring(8, 4);
                    try
                    {
                        if (int.Parse(gcbh) > index)
                        {
                            index = int.Parse(gcbh);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return date + index.ToString().PadLeft(4, '0');
        }
    }

    /// <summary>
    /// 获取二级单位编号
    /// </summary>
    /// <param name="deptCode"></param>
    /// <returns></returns>
    public string GetDeptLevel2(string deptCode)
    {
        string sjDept = "";
        string returnDept = deptCode;
        sjDept = server.GetCellValue("select sjDeptCode from bill_departments where deptCode='" + deptCode + "'");
        if (server.GetCellValue("select sjDeptCode from bill_departments where deptCode='" + sjDept + "'") == "")
        {
            return returnDept;
        }
        else
        {
            return this.GetDeptLevel2(sjDept);
        }
    }

    /// <summary>
    /// 获取人员所在二级单位编号
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    public string GetDeptLevel2_userCode(string userCode)
    {
        string deptCode = server.GetCellValue("select userDept from bill_users where userCode='" + userCode + "'");
        return this.GetDeptLevel2(deptCode);
    }

    public string getDeptLevel2Name(string deptCode)
    {
        string dept2 = this.GetDeptLevel2(deptCode);
        if (deptCode == dept2)
        {
            return server.GetCellValue("select deptName from bill_departments where deptcode='" + deptCode + "'");
        }
        else
        {
            return server.GetCellValue("select deptName from bill_departments where deptcode='" + dept2 + "'") + "-" + server.GetCellValue("select deptName from bill_departments where deptcode='" + deptCode + "'");
        }
    }

    /// <summary>
    /// 获取请假单
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getQjdCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select id from bill_qjd where right(left(id,11),8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return "qjd" + date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string id = temp.Tables[0].Rows[i]["id"].ToString().Trim();
                if (id.Length == 16)//固定12位编码
                {
                    id = id.Substring(12, 4);
                    try
                    {
                        if (int.Parse(id) > index)
                        {
                            index = int.Parse(id);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return "id" + date + index.ToString().PadLeft(4, '0');
        }
    }


    /// <summary>
    /// 领用单
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getLydCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select guid from bill_lyd where right(left(guid,11),8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return "lyd" + date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string id = temp.Tables[0].Rows[i]["guid"].ToString().Trim();
                if (id.Length == 16)//固定12位编码
                {
                    id = id.Substring(12, 4);
                    try
                    {
                        if (int.Parse(id) > index)
                        {
                            index = int.Parse(id);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return "lyd" + date + index.ToString().PadLeft(4, '0');
        }
    }


    /// <summary>
    /// 获取用款申请
    /// </summary>
    /// <returns>年月日+四位流水</returns>
    public string getYksqCode()
    {
        string date = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0') + System.DateTime.Now.Day.ToString().PadLeft(2, '0');

        string sql = "select billCode from bill_yksq where right(left(billCode,12),8)='" + date + "'";

        DataSet temp = server.GetDataSet(sql);

        if (temp.Tables[0].Rows.Count == 0)
        {
            return "yksq" + date + "0001";
        }
        else
        {
            int index = 1;
            for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
            {
                string id = temp.Tables[0].Rows[i]["billCode"].ToString().Trim();
                if (id.Length == 16)//固定12位编码
                {
                    id = id.Substring(12, 4);
                    try
                    {
                        if (int.Parse(id) > index)
                        {
                            index = int.Parse(id);
                        }
                    }
                    catch { }
                }
            }
            index += 1;

            return "yksq" + date + index.ToString().PadLeft(4, '0');
        }
    }

}