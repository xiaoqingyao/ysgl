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
/// yskm 的摘要说明
/// </summary>
public class yskm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public yskm()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 绑定归口预算科目
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    /// <param name="kmStatus"></param>
    public void BindGkYskm(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string kmStatus)
    {
        string sql = "select * from bill_yskm where gkfy='1'";
        if (kmStatus != "")
        {
            sql += " and kmStatus in (" + kmStatus + ")";
        }
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    /// <summary>
    /// 根据对应单据筛选科目
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    /// <param name="kmStatus"></param>
    public void BindYskmbydydj(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string kmStatus, string dydj)
    {

        string sql = "select * from bill_yskm";
        if (kmStatus != "")
        {
            sql += " where kmStatus in (" + kmStatus + ")";
        }
        if (!string.IsNullOrEmpty(dydj))
        {
            sql += " and dydj='" + dydj + "'";
        }
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                tNode.ToolTip = tNode.Text;
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    /// <param name="kmStatus"></param>
    public void BindYskm(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string kmStatus)
    {

        string sql = "select * from bill_yskm";
        if (kmStatus != "")
        {
            sql += " where kmStatus in (" + kmStatus + ")";
        }
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                tNode.ToolTip = tNode.Text;
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    /// <param name="orgGuid"></param>
    /// <param name="kmStatus"></param>

    public void BindYskm(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string orgGuid, string kmStatus)
    {
        string sql = "select * from bll_yskm where isnull(orgGuid,'')='" + orgGuid + "'";
        if (kmStatus != "")
        {
            sql += " and kmStatus in (" + kmStatus + ")";
        }
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="temp"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    private void BindYskm2(TreeNode pNode, DataSet temp, string url, string target, string imgUrl, bool showCheckBox)
    {
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Substring(0, kmCode.Length - 2) == pNode.Value
                && kmCode.Length == pNode.Value.Length + 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                tNode.ToolTip = tNode.Text;
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    public void BindYskmH(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string kmStatus, string otherParameter)
    {
        string sql = "select * from bill_yskm";
        if (kmStatus != "")
        {
            sql += " where kmStatus in (" + kmStatus + ")";
        }
        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value + otherParameter;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                tNode.ToolTip = tNode.Text;
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2H(tNode, temp, url, target, imgUrl, showCheckBox, otherParameter);
            }
        }
    }
    private void BindYskm2H(TreeNode pNode, DataSet temp, string url, string target, string imgUrl, bool showCheckBox, string otherParameter)
    {
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Substring(0, kmCode.Length - 2) == pNode.Value
                && kmCode.Length == pNode.Value.Length + 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                if (url != "")
                {
                    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value + otherParameter;
                    tNode.Target = target;
                }
                if (imgUrl != "")
                {
                    tNode.ImageUrl = imgUrl;
                }
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                tNode.ToolTip = tNode.Text;
                pNode.ChildNodes.Add(tNode);
                this.BindYskm2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pNode"></param>
    /// <param name="url"></param>
    /// <param name="target"></param>
    /// <param name="imgUrl"></param>
    /// <param name="showCheckBox"></param>
    /// <param name="kmStatus"></param>
    public void BindYskm_NoUrl(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string kmStatus)
    {
        string sql = "select * from bill_yskm";
        if (kmStatus != "")
        {
            sql += " where kmStatus in (" + kmStatus + ")";
        }

        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                tNode.SelectAction = TreeNodeSelectAction.None;
                //if (url != "")
                //{
                //    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                //    tNode.Target = target;
                //}
                //if (imgUrl != "")
                //{
                //    tNode.ImageUrl = imgUrl;
                //}
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindYskm_No2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    public void BindYskm_No2(TreeNode pNode, DataSet temp, string url, string target, string imgUrl, bool showCheckBox)
    {
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Substring(0, kmCode.Length - 2) == pNode.Value
                && kmCode.Length == pNode.Value.Length + 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                tNode.SelectAction = TreeNodeSelectAction.Select;
                //if (url != "")
                //{
                //    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                //    tNode.Target = target;
                //}
                //if (imgUrl != "")
                //{
                //    tNode.ImageUrl = imgUrl;
                //}
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                pNode.SelectAction = TreeNodeSelectAction.Expand;
                this.BindYskm_No2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }


    public string getYsxx(string deptCode, string rq, string yskm)
    {
        string returnStr = "";
        deptCode = deptCode.Substring(1, deptCode.IndexOf("]") - 1);
        deptCode = (new billCoding()).GetDeptLevel2(deptCode);

        string xjDeptCodes = (new Departments()).GetNextLevelDepartments(deptCode, "", true);

        DateTime dt = DateTime.Parse(rq);
        string nian = dt.Year.ToString();
        //string yue = dt.Month.ToString().PadLeft(2, '0');
        string yue = int.Parse(dt.Month.ToString()).ToString();
        string jd = "";
        string months1 = "";
        string months2 = "";
        string months3 = "";
        if (int.Parse(yue) >= 1 && int.Parse(yue) <= 3)
        {
            jd = "一";
            months1 = nian + "01";
            months2 = nian + "02";
            months3 = nian + "03";
        }
        else if (int.Parse(yue) >= 4 && int.Parse(yue) <= 6)
        {
            jd = "二";
            months1 = nian + "04";
            months2 = nian + "05";
            months3 = nian + "06";
        }
        else if (int.Parse(yue) >= 7 && int.Parse(yue) <= 9)
        {
            jd = "三";
            months1 = nian + "07";
            months2 = nian + "08";
            months3 = nian + "09";
        }
        else if (int.Parse(yue) >= 10 && int.Parse(yue) <= 12)
        {
            jd = "四";
            months1 = nian + "10";
            months2 = nian + "11";
            months3 = nian + "12";
        }
        //年预算数据
        DataSet temp = server.GetDataSet("select * from bill_ysgc where ystype='0' and nian='" + nian + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnStr += "<font color=red>选择时间内无年度预算过程！</font>";
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select * from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskm + "'");
            if (temp.Tables[0].Rows.Count == 0)
            {
                return "<font color=red>选择单位在选定时间内无年度预算数据！</font>";
            }
            else
            {
                double nys = 0;
                if ((temp.Tables[0].Rows[0]["ysje"]).GetType().Name == "DBNull")
                {
                    nys = 0;
                }
                else
                {
                    nys = double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                }

                //string nbx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where fykm='" + yskm + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),4)='" + nian + "' and billDept in (" + xjDeptCodes + "))");
                string nbx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and fykm='" + yskm + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),4)='" + nian + "' and billDept in (" + xjDeptCodes + "))");
                /*
                double[] dbnian = getNianYs(deptCode, rq, yskm);
                double[] dbjd = getJdYs(deptCode, rq, yskm);
                double[] dby = getYueYs(deptCode, rq, yskm);
                */

                double nye = nys - double.Parse(nbx);
                returnStr += "【年】" + nye.ToString("0.00") + "/" + nys.ToString("0.00");
            }
        }
        //季度预算数据
        temp = server.GetDataSet("select * from bill_ysgc where ystype='1' and nian='" + nian + "' and yue='" + jd + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnStr += "<font color=red>选择时间内无季度预算过程！</font>";
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select * from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskm + "'");
            if (temp.Tables[0].Rows.Count == 0)
            {
                returnStr += "<font color=red>选择单位在选定时间内无季度预算数据！</font>";
            }
            else
            {
                double jdys = 0;
                if ((temp.Tables[0].Rows[0]["ysje"]).GetType().Name == "DBNull")
                {
                    jdys = 0;
                }
                else
                {
                    jdys = double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                }

                string jdbx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and fykm='" + yskm + "' and billCode in (select billCode from bill_main where (left(convert(varchar(8),billDate,112),6)='" + months1 + "' or left(convert(varchar(8),billDate,112),6)='" + months2 + "' or left(convert(varchar(8),billDate,112),6)='" + months3 + "') and billDept in (" + xjDeptCodes + "))");

                double jdye = jdys - double.Parse(jdbx);

                returnStr += "【第" + jd + "季度】" + jdye.ToString("0.00") + "/" + jdys.ToString("0.00");
            }
        }
        //月预算数据
        temp = server.GetDataSet("select * from bill_ysgc where ystype='2' and nian='" + nian + "' and yue='" + yue + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnStr += "<font color=red>选择时间内无月预算过程！</font>";
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select * from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskm + "'");// and gcbh in (select billName from bill_main where (flowid='ys' or flowid='yszj') and stepid='end')");
            if (temp.Tables[0].Rows.Count == 0)
            {
                returnStr += "<font color=red>选择单位在选定时间内无月预算数据！</font>";
            }
            else
            {
                double yueys = 0;
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    if ((temp.Tables[0].Rows[0]["ysje"]).GetType().Name != "DBNull")
                    {
                        yueys += double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                    }
                }
                //returnStr += "【" + yue + "月】" + je.ToString("0.00");


                //string yuebx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and status='1' and fykm='" + yskm + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),6)='" + nian + yue.ToString().PadLeft(2, '0') + "' and billDept in (" + xjDeptCodes + "))");
                string yuebx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1'))  and fykm='" + yskm + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),6)='" + nian + yue.ToString().PadLeft(2, '0') + "' and billDept in (" + xjDeptCodes + "))");

                double yueye = yueys - double.Parse(yuebx);

                returnStr += "【" + yue + "月】" + yueye.ToString("0.00") + "/" + yueys.ToString("0.00");
            }
        }

        return returnStr;
    }

    //获取某单位 某时间 某预算科目的年度预算额及余额
    public double[] getNianYs(string deptCode, string time, string yskmCode)
    {
        double[] returnValue = new double[2];
        deptCode = (new billCoding()).GetDeptLevel2(deptCode);

        string xjDeptCodes = (new Departments()).GetNextLevelDepartments(deptCode, "", true);

        string nian = DateTime.Parse(time).Year.ToString();

        //年预算数据
        DataSet temp = server.GetDataSet("select * from bill_ysgc where ystype='0' and nian='" + nian + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnValue[0] = 0;
            returnValue[1] = 0;
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select gcbh,billcode,yskm,isnull(ysje,0) as ysje,ysdept,ystype from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskmCode + "'");
            if (temp.Tables[0].Rows.Count == 0)
            {
                returnValue[0] = 0;
                returnValue[1] = 0;
            }
            else
            {
                double nys = double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                returnValue[0] = nys;

                string nbx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and fykm='" + yskmCode + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),4)='" + nian + "' and billDept in (" + xjDeptCodes + "))");

                double nye = nys - double.Parse(nbx);
                returnValue[1] = nye;
            }
        }
        return returnValue;
    }

    //获取某单位 某时间 某预算科目的季度预算额及余额
    public double[] getJdYs(string deptCode, string time, string yskmCode)
    {
        double[] returnValue = new double[2];
        deptCode = (new billCoding()).GetDeptLevel2(deptCode);

        string xjDeptCodes = (new Departments()).GetNextLevelDepartments(deptCode, "", true);

        DateTime dt = DateTime.Parse(time);
        string nian = dt.Year.ToString();
        string jd = "";
        string yue = dt.Month.ToString().PadLeft(2, '0');
        string months1 = "";
        string months2 = "";
        string months3 = "";
        if (int.Parse(yue) >= 1 && int.Parse(yue) <= 3)
        {
            jd = "一";
            months1 = nian + "01";
            months2 = nian + "02";
            months3 = nian + "03";
        }
        else if (int.Parse(yue) >= 4 && int.Parse(yue) <= 6)
        {
            jd = "二";
            months1 = nian + "04";
            months2 = nian + "05";
            months3 = nian + "06";
        }
        else if (int.Parse(yue) >= 7 && int.Parse(yue) <= 9)
        {
            jd = "三";
            months1 = nian + "07";
            months2 = nian + "08";
            months3 = nian + "09";
        }
        else if (int.Parse(yue) >= 10 && int.Parse(yue) <= 12)
        {
            jd = "四";
            months1 = nian + "10";
            months2 = nian + "11";
            months3 = nian + "12";
        }

        //季度预算数据
        DataSet temp = server.GetDataSet("select * from bill_ysgc where ystype='1' and nian='" + nian + "' and yue='" + jd + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnValue[0] = 0;
            returnValue[1] = 0;
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select gcbh,billcode,yskm,isnull(ysje,0) as ysje,ysdept,ystype from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskmCode + "'");
            if (temp.Tables[0].Rows.Count == 0)
            {
                returnValue[0] = 0;
                returnValue[1] = 0;
            }
            else
            {
                double jdys = double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                returnValue[0] = jdys;

                string jdbx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and fykm='" + yskmCode + "' and billCode in (select billCode from bill_main where (left(convert(varchar(8),billDate,112),6)='" + months1 + "' or left(convert(varchar(8),billDate,112),6)='" + months2 + "' or left(convert(varchar(8),billDate,112),6)='" + months3 + "') and billDept in (" + xjDeptCodes + "))");

                double jdye = jdys - double.Parse(jdbx);
                returnValue[1] = jdye;
            }
        }
        return returnValue;
    }


    //获取某单位 某时间 某预算科目的余额预算额及余额
    public double[] getYueYs(string deptCode, string time, string yskmCode)
    {
        double[] returnValue = new double[2];
        deptCode = (new billCoding()).GetDeptLevel2(deptCode);

        string xjDeptCodes = (new Departments()).GetNextLevelDepartments(deptCode, "", true);

        DateTime dt = DateTime.Parse(time);
        //string yue = dt.Month.ToString().PadLeft(2, '0');
        string yue = dt.Month.ToString();
        string nian = dt.Year.ToString();
        //月预算数据
        DataSet temp = server.GetDataSet("select * from bill_ysgc where ystype='2' and nian='" + nian + "' and yue='" + yue + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            returnValue[0] = 0;
            returnValue[1] = 0;
        }
        else
        {
            string ysgc = temp.Tables[0].Rows[0]["gcbh"].ToString().Trim();
            temp = server.GetDataSet("select gcbh,billcode,yskm,isnull(ysje,0) as ysje,ysdept,ystype from bill_ysmxb where gcbh='" + ysgc + "' and ysDept='" + deptCode + "' and yskm='" + yskmCode + "'");// and gcbh in (select billName from bill_main where (flowid='ys' or flowid='yszj') and stepid='end')");
            if (temp.Tables[0].Rows.Count == 0)
            {
                returnValue[0] = 0;
                returnValue[1] = 0;
            }
            else
            {
                double yueys = 0;
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    yueys += double.Parse(temp.Tables[0].Rows[0]["ysje"].ToString().Trim());
                }
                returnValue[0] = yueys;


                string yuebx = server.GetCellValue("select isnull(sum(je),0) from bill_ybbxmxb_fykm where billcode in (select billcode from bill_ybbxmxb where bxmxlx in (select isnull(diccode,'1') from bill_datadic where cjys='1')) and status='1' and fykm='" + yskmCode + "' and billCode in (select billCode from bill_main where left(convert(varchar(8),billDate,112),6)='" + nian + yue.ToString().PadLeft(2, '0') + "' and billDept in (" + xjDeptCodes + "))");

                double yueye = yueys - double.Parse(yuebx);

                returnValue[1] = yueye;
            }
        }
        return returnValue;
    }
}