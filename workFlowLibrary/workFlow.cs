using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace workFlowLibrary
{
    /// <summary>
    /// 获取审核流程权限步骤
    /// </summary>
    public class workFlow
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        /// <summary>
        /// 获取当前登录人员有权限审核的单据编号
        /// </summary>
        /// <param name="flowID">单据类型：每种单据的固定值</param>
        /// <param name="groupID">当前登录人员所在组编号</param>
        /// <returns>注意：管理单位权限自行添加 返回数据格式： 'bill001','bill002','bill003',...'bill00n'</returns>
        public string getRightStepBills(string flowID, string groupID, string userCode, string rightDeptCodes)
        {
            if (userCode == "admin")
            {
                return "''";
            }
            //该过程不用考虑会签的情况 会签只在审核通过 更新状态时使用
            //该方法 只需要获取到权限审核步骤的上级步骤即可
            string billCodes = "";

            #region 获取当前角色权限的步骤号
            string sql = "select * from bill_workFlowGroup where flowID='" + flowID + "'";
            DataSet temp = server.GetDataSet(sql);
            if (temp.Tables[0].Rows.Count == 0)
            {
                billCodes = "''";
            }
            else
            {
                billCodes = "'',";
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    string stepID = temp.Tables[0].Rows[i]["stepID"].ToString().Trim();//有权限的步骤编号
                    string checkStep = "'',";
                    DataSet temp2 = server.GetDataSet("select * from bill_workFlowAction where flowID='" + flowID + "' and actionTo='" + stepID + "'");//截止状态时当前状态的所有上级状态
                    for (int j = 0; j <= temp2.Tables[0].Rows.Count - 1; j++)
                    {
                        checkStep += "'" + temp2.Tables[0].Rows[j]["actionFrom"].ToString().Trim() + "',";//有权限审核的所有上级步骤编号
                    }
                    if (checkStep != "") checkStep = checkStep.Substring(0, checkStep.Length - 1);

                    if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "一般审核")//指定人员 或者 同时指定角色和人员
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == "" && temp.Tables[0].Rows[i]["wkUser"].ToString().Trim() == userCode)//仅指定人员的：全部单位
                        {
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and isnull(b.checkGroup,'')='' and isnull(b.stepUser,'')='" + userCode + "' and b.wkModel='一般审核')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//所有权限单据的编号
                            }
                        }
                        else if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID && temp.Tables[0].Rows[i]["wkUser"].ToString().Trim() == userCode)//同时角色和人员：权限单位内的单据
                        {
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in (" + rightDeptCodes + ") and  flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in ("+checkStep+") and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.stepUser='" + userCode + "' and b.wkModel='一般审核')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//所有权限单据的编号
                            }
                        }
                    }
                    else if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "业务主管审核")
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID)
                        {
                            //主管审核或分管领导审核
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in(select deptCode from bill_dept_ywzg where userCode='" + userCode + "') and flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.wkModel='业务主管审核')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//所有权限单据的编号
                            }
                        }
                    }
                    else if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "分管领导审核")
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID)
                        {//主管审核或分管领导审核
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in (select deptCode from bill_dept_fgld where userCode='" + userCode + "') and flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.wkModel='分管领导审核')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//所有权限单据的编号
                            }
                        }
                    }

                }
                if (billCodes != "") billCodes = billCodes.Substring(0, billCodes.Length - 1);
            }
            #endregion

            return billCodes;
        }

        /// <summary>
        /// 单据审核方法 
        /// </summary>
        /// <param name="flowID">单据类型</param>
        /// <param name="billCode">单据编号</param>
        /// <param name="checkUser">审核人</param>
        /// <param name="checkDate">审核时间：详细到秒</param>
        /// <param name="checkBz">审核意见（备注）</param>
        /// <param name="result">设和结果：true通过 false不通过</param>
        /// <returns>成功：返回空 否则：返回相应的提示信息</returns>
        public string checkBills(string flowID, string billCode, string checkUser, string checkDate, string checkBz, bool result)
        {
            string userGroup = server.GetCellValue("select userGroup from bill_users where userCode='" + checkUser + "'");
            if (userGroup == "")
            {
                return "当前人员无用户组！";
            }

            List<string> list = new List<string>();
            DataSet temp = server.GetDataSet("select * from bill_main where flowid='" + flowID + "' and billCode='" + billCode + "'");//获取单据的详细信息
            string stepID = temp.Tables[0].Rows[0]["stepID"].ToString().Trim();//当前步骤编号
            string loopTimes = temp.Tables[0].Rows[0]["loopTimes"].ToString().Trim();//第几次提交审核

            //获取所有可能的下一步骤 而且是当前人员有权限的步骤:是所在组权限 || 即是组又是人员 || 是人员  下一步骤 只能有只用状态，否则混乱
            DataSet temp1 = server.GetDataSet("select actionTo from bill_workFlowAction where flowID='" + flowID + "' and actionFrom='" + stepID + "'");// and actionTo in (select stepID from bill_workFlowGroup where flowid='" + flowID + "' and (wkGroup='" + userGroup + "' and (isnull(wkUser,'')='' or isnull(wkUser,'')='" + checkUser + "')) or (isnull(wkGroup,'')='' and isnull(wkUser,'')='" + checkUser + "'))");
            if (temp1.Tables[0].Rows.Count == 0)
            {
                return "未找到相应的审核流程！";
            }


            //找到下一步骤编号
            string actionTo = temp1.Tables[0].Rows[0][0].ToString().Trim();

            //下一步骤的权限设置情况
            DataSet temp6 = server.GetDataSet("select * from bill_workflowGroup where flowid='" + flowID + "' and stepID='" + actionTo + "'");

            string wkModel = temp6.Tables[0].Rows[0]["wkModel"].ToString().Trim();
            string wkGroup = temp6.Tables[0].Rows[0]["wkGroup"].ToString().Trim();
            string wkUser = temp6.Tables[0].Rows[0]["wkUser"].ToString().Trim();

            if (result == false)
            {
                list.Add("update bill_main set stepID='0' where flowID='" + flowID + "' and billCode='" + billCode + "'");
                list.Add("insert into bill_workFlowRecord(billcode,flowid,beginstep,endstep,checkuser,checkdate,checkbz,looptimes,checkgroup,result,stepUser,wkModel) values('" + billCode + "','" + flowID + "','" + stepID + "','0','" + checkUser + "','" + checkDate + "','" + checkBz + "','" + loopTimes + "','" + wkGroup + "','false','" + wkUser + "','" + wkModel + "')");
            }
            else
            {
                DataSet tempAct = server.GetDataSet("select * from bill_workFlowAction where flowID='" + flowID + "' and actionFrom='" + actionTo + "' and actionTo='end'");
                if (tempAct.Tables[0].Rows.Count == 0)
                {
                    //不是倒数第二步骤的状态，则不用处理，否则，直接更新为结束状态
                }
                else
                {
                    actionTo = "end";
                }
                list.Add("update bill_main set stepID='" + actionTo + "' where flowID='" + flowID + "' and billCode='" + billCode + "'");
                list.Add("insert into bill_workFlowRecord(billcode,flowid,beginstep,endstep,checkuser,checkdate,checkbz,looptimes,checkgroup,result,stepUser,wkModel) values('" + billCode + "','" + flowID + "','" + stepID + "','" + actionTo + "','" + checkUser + "','" + checkDate + "','" + checkBz + "','" + loopTimes + "','" + wkGroup + "','true','" + wkUser + "','" + wkModel + "')");

            }

            if (result == true && actionTo == "end" && flowID == "ystz")//预算调整的特殊处理
            {
                string sCode = server.GetCellValue("select sCode from bill_ystz where billcode='" + billCode + "'");
                string tCode = server.GetCellValue("select tCode from bill_ystz where billcode='" + billCode + "'");
                string nian = server.GetCellValue("select nian from bill_ysgc where gcbh='" + sCode + "'");
                int sYue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + sCode + "'"));
                int tYue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + tCode + "'"));

                string sJd = "";
                if (sYue >= 1 && sYue <= 3) { sJd = "一"; }
                if (sYue >= 4 && sYue <= 6) { sJd = "二"; }
                if (sYue >= 7 && sYue <= 9) { sJd = "三"; }
                if (sYue >= 10 && sYue <= 12) { sJd = "四"; }
                string sGcbh_Jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='"+sJd+"' and ystype='1'");

                string tJd = "";
                if (tYue >= 1 && tYue <= 3) { tJd = "一"; }
                if (tYue >= 4 && tYue <= 6) { tJd = "二"; }
                if (tYue >= 7 && tYue <= 9) { tJd = "三"; }
                if (tYue >= 10 && tYue <= 12) { tJd = "四"; }
                string tGcbh_Jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='"+tJd+"' and ystype='1'");

                //开始调整月预算的值
                string billDept = server.GetCellValue("select billdept from bill_main where billcode='" + billCode + "'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_before where billcode='" + billCode + "' and bill_ystz_before.km=bill_ysmxb.yskm),0) where gcbh='" + sCode + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_after where billcode='" + billCode + "' and bill_ystz_after.km=bill_ysmxb.yskm),0) where gcbh='" + tCode + "' and ysdept='" + billDept + "' and ystype='1'");

                if (sJd == tJd)
                {
                    //不调整解读预算
                }
                else { //调整季度预算
                    list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_before where billcode='" + billCode + "' and bill_ystz_before.km=bill_ysmxb.yskm),0) where gcbh='" + sGcbh_Jd + "' and ysdept='" + billDept + "' and ystype='1'");
                    list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_after where billcode='" + billCode + "' and bill_ystz_after.km=bill_ysmxb.yskm),0) where gcbh='" + tGcbh_Jd + "' and ysdept='" + billDept + "' and ystype='1'");
                }
            }
            else if (result == true && actionTo == "end" && flowID == "yszj")//预算追加
            {
                DataSet gcDataSet = server.GetDataSet("select top 1 * from bill_ysmxb where billcode='" + billCode + "'");
                string gcbh_yue = gcDataSet.Tables[0].Rows[0]["gcbh"].ToString().Trim();
                gcDataSet = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh_yue + "'");

                int yue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + gcbh_yue + "'"));
                string jd = "";
                if (yue >= 1 && yue <= 3) { jd = "一"; }
                if (yue >= 4 && yue <= 6) { jd = "二"; }
                if (yue >= 7 && yue <= 9) { jd = "三"; }
                if (yue >= 10 && yue <= 12) { jd = "四"; }
                string nian = gcDataSet.Tables[0].Rows[0]["nian"].ToString().Trim();
                string gcbh_jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='" + jd + "' and ystype='1'");
                string gcbh_nian = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='' and ystype='0'");
                string billDept = server.GetCellValue("select billdept from bill_main where billcode='" + billCode + "'");

                list.Add("select * into ysmxb" + checkUser + " from bill_ysmxb where billCode='" + billCode + "'");//创建临时表 存储预算追加数据
                //补充缺少的预算科目号
                string tempStr20120119 = "";
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_yue + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_jd + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_nian + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);

                //更新总额
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1'");

                list.Add("drop table ysmxb" + checkUser);
            }


            if (server.ExecuteNonQuerysArray(list) == -1)
            {

                return "审核失败！";
            }
            else
            {
                return "审核完成！";
            }
        }

        /// <summary>
        /// 有问题：暂时不用
        /// 递归获取单据的下一状态：目的，跳过已审核过该单据的人员：防止重复审核
        /// </summary>
        /// <returns></returns>
        private string getNextStepIDLoop(string flowID, string pStepID, string loopTimes)
        {
            DataSet temp = server.GetDataSet("select * from bill_workflowaction where flowID='" + flowID + "' and actionFrom='" + pStepID + "'");//获取下一步的步骤名称

            temp = server.GetDataSet("select * from bill_workflowGroup where flowID='" + flowID + "' and stepID='" + temp.Tables[0].Rows[0]["actionTo"].ToString().Trim() + "'");//下一步 审核流程的权限设置情况

            string user = temp.Tables[0].Rows[0]["wkUser"].ToString().Trim();//下一步骤有权限审核的人员
            if (user == "")//部门内部审核，再所有审核之前，所以。。。
            {
                return pStepID;
            }
            else { 
                //制定人员审核的情况：判断权限人员是否是该单据单位的业务主管：是则直接跳过
                return "";
            }
        }


        /// <summary>
        /// 获取某一单据审核通过后的下一状态 用于更新bill_main表的stepID字段值
        /// </summary>
        /// <param name="flowID">单据类型：每种单据的固定值</param>
        /// <param name="currentStepID">该单据当前的步骤编号</param>
        /// <returns>返回数据格式</returns>
        private string getNextStepID(string flowID, string currentStepID)
        {
            //考虑会审和并行的情况
            return "";
        }


        public string getShlcWord(string flowID)
        {
            try
            {
                string returnStr = "制单提交";
                DataSet temp = server.GetDataSet("select * from bill_workflowAction where flowID='" + flowID + "' and actionFrom='begin'");
                if (temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() == "end")
                {
                    returnStr += "-->结束";
                }
                else
                {
                    returnStr += "-->" + server.GetCellValue("select stepText from bill_workflowstep where flowid='" + flowID + "' and stepID='" + temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() + "'");
                    this.getShlcWord(flowID, temp.Tables[0].Rows[0]["actionTo"].ToString().Trim(), ref returnStr);
                }
                return returnStr;
            }
            catch {
                return "";//审核流程设置错误！
            }
        }
        public void getShlcWord(string flowID,string stepID,ref string returnStr)
        {
            if (returnStr == "end")
            {
            }
            else
            {
                DataSet temp = server.GetDataSet("select * from bill_workflowAction where flowID='" + flowID + "' and actionFrom='" + stepID + "'");
                if (temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() == "end")
                {
                    returnStr += "-->结束";
                }
                else
                {
                    returnStr += "-->" + server.GetCellValue("select stepText from bill_workflowstep where flowid='" + flowID + "' and stepID='" + temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() + "'");
                    this.getShlcWord(flowID, temp.Tables[0].Rows[0]["actionTo"].ToString().Trim(),ref returnStr);
                }
            }
        }
    }
}
