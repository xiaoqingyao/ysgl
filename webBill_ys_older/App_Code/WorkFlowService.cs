using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Script.Serialization;
using WorkFlowLibrary.WorkFlowDal;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using System.Text;

/// <summary>
///WorkFlowService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
[System.Web.Script.Services.ScriptService]
public class WorkFlowService : System.Web.Services.WebService
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public WorkFlowService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 根据不同审批类型获得审批人
    /// </summary>
    /// <param name="typecode"></param>
    /// <returns></returns>
    [WebMethod]
    public string GetCheckList(string typecode)
    {
        IDictionary<string, string> dic = XmlHelper.GetWFTypeConfig(typecode);
        if (string.IsNullOrEmpty(dic["codecolum"]))
        {
            return "";
        }
        else
        {
            string sql = "select " + dic["filter"] + " as ccode," + dic["codemainvalue"] + " as cname  from " + dic["codemaintable"];
            DataSet ds = server.GetDataSet(sql);
            string ret = Serialize(ds.Tables[0]);
            return ret;
        }
    }

    /// <summary>
    /// 获得当前单据的审批状态
    /// </summary>
    /// <param name="billcode"></param>
    /// <returns></returns>
    [WebMethod]
    public string WorkFlowState(string billcode)
    {
        StringBuilder ret = new StringBuilder();
        WorkFlowRecordManager bll = new WorkFlowRecordManager();
        WorkFlowRecord recode = bll.GetWFRecordByBill(billcode);
        if (recode == null)
        {
            ret.Append("<span>未提交</span>");
        }
        else
        {
            ret.Append("<span>");
            foreach (WorkFlowRecords records in recode.RecordList)
            {
                ret.Append(server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + records.CheckUser + "'"));
                //状态(0,等待;1,正在执行;2,通过;3,废弃)
                if (records.RdState == 0)
                {
                    ret.Append(",等待");
                }
                else if (records.RdState == 1)
                {
                    ret.Append(",正在执行");
                }
                else if (records.RdState == 2)
                {
                    ret.Append(",通过");
                }
                else if (records.RdState == 3)
                {
                    ret.Append(",否决");
                }

                ret.Append("-->");
            }
            ret.Append("结束");
            ret.Append("</span>");
        }
        return (ret.ToString());
    }

    /// <summary>
    /// 修改审批流
    /// </summary>
    /// <param name="datas"></param>
    /// <param name="flowid"></param>
    /// <returns></returns>
    [WebMethod]
    public int UpdateWorkFlows(string datas, string flowid)
    {
        try
        {
            MainWorkFlow mwf = new MainWorkFlow();
            string[] data = datas.Split('|');

            mwf.FlowId = flowid;
            int i = 1;
            IList<WorkFlowStep> list = new List<WorkFlowStep>();
            foreach (string wfs in data)
            {
                string[] array = wfs.Split(',');
                WorkFlowStep temp = new WorkFlowStep();
                temp.FlowId = flowid;
                temp.StepType = array[0].Split('[')[0];
                //审批人员
                if (array.Length > 2)
                {
                    if (!string.IsNullOrEmpty(array[1]))
                    {
                        string[] tempCheck = array[1].Split(':');
                        if (tempCheck.Length == 1 && !string.IsNullOrEmpty(tempCheck[0]))
                        {
                            temp.CheckType = "2";//个人审批
                        }
                        else
                        {
                            temp.CheckType = array[5].Split('[')[0];
                        }

                        StringBuilder checkCode = new StringBuilder();


                        for (int j = 0; j < tempCheck.Length; j++)
                        {
                            checkCode.Append(tempCheck[j].Split('[')[0]);
                            checkCode.Append("|");
                        }
                        temp.CheckCode = checkCode.Remove(checkCode.Length - 1, 1).ToString();

                    }
                    temp.StepText = array[0].Split('[')[1].Trim(']');
                    temp.Memo = array[4].Trim();
                    temp.IsKmZg = array[6].Substring(0, 1);
                    temp.kmType = string.IsNullOrEmpty(array[7]) ? "" : array[7].Split('[')[0];
                    if (!string.IsNullOrEmpty(array[2]))
                        temp.MinMoney = Convert.ToDecimal(array[2]);

                    if (!string.IsNullOrEmpty(array[3]))
                    {
                        temp.MinDate = ConvertToDt(array[3]);
                    }
                    temp.StepId = i;
                    list.Add(temp);
                    i++;
                }
            }
            mwf.StepList = list;
            new WorkFlowManager().EditWF(mwf);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    /// <summary>
    /// 修改审批流
    /// </summary>
    /// <param name="datas"></param>
    /// <param name="flowid"></param>
    /// <returns></returns>
    [WebMethod]
    public int UpdateWorkFlow(string datas, string flowid, string billdept)
    {
        try
        {

            //保存之前判断是否还有未处理的单据

            string strmes = getworkpo(flowid, billdept);

            if (!string.IsNullOrEmpty(strmes))
            {
                return -2;
            }

            MainWorkFlow mwf = new MainWorkFlow();
            string[] data = datas.Split('|');

            mwf.FlowId = flowid;
            int i = 1;
            IList<WorkFlowStep> list = new List<WorkFlowStep>();
            foreach (string wfs in data)
            {
                string[] array = wfs.Split(',');
                //typecode + "," + checkcode + "," + isKmZhuguan + "," + cje + "," + checktype + "," + cdate + "," + maxje + "," + bz + ",|";
                if (array.Length > 2)
                {
                    if (!string.IsNullOrEmpty(array[1]))
                    {
                        WorkFlowStep temp = new WorkFlowStep();
                        temp.FlowId = flowid;
                        temp.Memo = billdept;
                        temp.StepType = array[0].Split('[')[0];
                        temp.StepText = array[0].Split('[')[1].Trim(']');
                        //审批人员
                        string[] tempCheck = array[1].Split(':');

                        if (tempCheck.Length == 1 && !string.IsNullOrEmpty(tempCheck[0]))
                        {
                            temp.CheckType = "2";//个人审批
                        }
                        else
                        {
                            temp.CheckType = array[4].Split('[')[0];
                        }

                        StringBuilder checkCode = new StringBuilder();


                        for (int j = 0; j < tempCheck.Length; j++)
                        {
                            checkCode.Append(tempCheck[j].Split('[')[0]);
                            checkCode.Append("|");
                        }
                        temp.CheckCode = checkCode.Remove(checkCode.Length - 1, 1).ToString();



                        temp.Memo = array[7].Trim();
                        temp.IsKmZg = array[2].Split('[')[0];//"0";//array[6].Substring(0, 1);
                        string strisje = array[8].Split('[')[0];
                        temp.kmType = strisje;
                        if (strisje == "1")
                        {
                            if (!string.IsNullOrEmpty(array[3]))
                            {
                                temp.MinMoney = Convert.ToDecimal(array[3]);

                            }
                            if (!string.IsNullOrEmpty(array[6]))
                            {
                                temp.MaxMoney = Convert.ToDecimal(array[6]);
                            }

                        }

                        if (!string.IsNullOrEmpty(array[5]))
                            temp.MinDate = Convert.ToDateTime(array[5]);
                        temp.StepId = i;
                        list.Add(temp);
                        i++;
                    }
                }

            }
            mwf.StepList = list;
            new WorkFlowManager().EditWF(mwf, billdept);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }
    /// <summary>
    /// 判断是否还有未处理的单据
    /// </summary>
    /// <param name="flowid"></param>
    /// <param name="billdept"></param>
    /// <returns></returns>
    public string getworkpo(string flowid, string billdept)
    {
        string mes = "";

        string strsql = @" 
                             select count(*) sl from dbo.workflowrecord 
                                             where billCode in 
                                             (
	                                            select  billcode from bill_main
	                                             where flowID='" + flowid + "' and billDept='" + billdept + @"'  and stepID='-1')
	                                             or billCode in(  select  billName from bill_main
	                                             where flowID='" + flowid + "' and billDept='" + billdept + @"'  and stepID='-1' )
	                                             and rdState=1";
        try
        {
            DataTable dt = new DataTable();
            dt = server.GetDataTable(strsql, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                string strsl = dt.Rows[0]["sl"].ToString();
                if (strsl != "0")
                {
                    mes += "有" + strsl + "条没处理的单据，请处理完毕后再进行修改审批流程。";
                }
            }
        }
        catch (Exception ex)
        {
            mes = "";
            throw;
        }

        return mes;

    }
    private DateTime ConvertToDt(string str)
    {
        DateTime dt;
        DateTime.TryParse(str, out dt);
        if (dt == null || dt == DateTime.MinValue)
        {
            dt = new DateTime(1900, 1, 1);
        }

        return dt;
    }
    /// <summary>
    /// 审批
    /// </summary>
    /// <param name="billcode"></param>
    /// <param name="mind"></param>
    /// <returns></returns>
    [WebMethod]
    public int Approve(string billcode, string mind)
    {
        try
        {
            WorkFlowRecordManager mgr = new WorkFlowRecordManager();
            string usercode = Convert.ToString(Session["userCode"]);
            mgr.Next(billcode, usercode, mind);
            return 1;
        }
        catch (Exception e)
        {
            return -1;
        }
    }

    /// <summary>
    /// 序列化datatable
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private string Serialize(DataTable dt)
    {
        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        foreach (DataRow dr in dt.Rows)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (DataColumn dc in dt.Columns)
            {
                result.Add(dc.ColumnName, dr[dc].ToString());
            }
            list.Add(result);
        }
        JavaScriptSerializer seria = new JavaScriptSerializer();
        string json = seria.Serialize(list);
        return json;
    }


}

