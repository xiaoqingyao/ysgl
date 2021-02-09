using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace workFlowLibrary
{
    /// <summary>
    /// ��ȡ�������Ȩ�޲���
    /// </summary>
    public class workFlow
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        /// <summary>
        /// ��ȡ��ǰ��¼��Ա��Ȩ����˵ĵ��ݱ��
        /// </summary>
        /// <param name="flowID">�������ͣ�ÿ�ֵ��ݵĹ̶�ֵ</param>
        /// <param name="groupID">��ǰ��¼��Ա��������</param>
        /// <returns>ע�⣺����λȨ��������� �������ݸ�ʽ�� 'bill001','bill002','bill003',...'bill00n'</returns>
        public string getRightStepBills(string flowID, string groupID, string userCode, string rightDeptCodes)
        {
            if (userCode == "admin")
            {
                return "''";
            }
            //�ù��̲��ÿ��ǻ�ǩ����� ��ǩֻ�����ͨ�� ����״̬ʱʹ��
            //�÷��� ֻ��Ҫ��ȡ��Ȩ����˲�����ϼ����輴��
            string billCodes = "";

            #region ��ȡ��ǰ��ɫȨ�޵Ĳ����
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
                    string stepID = temp.Tables[0].Rows[i]["stepID"].ToString().Trim();//��Ȩ�޵Ĳ�����
                    string checkStep = "'',";
                    DataSet temp2 = server.GetDataSet("select * from bill_workFlowAction where flowID='" + flowID + "' and actionTo='" + stepID + "'");//��ֹ״̬ʱ��ǰ״̬�������ϼ�״̬
                    for (int j = 0; j <= temp2.Tables[0].Rows.Count - 1; j++)
                    {
                        checkStep += "'" + temp2.Tables[0].Rows[j]["actionFrom"].ToString().Trim() + "',";//��Ȩ����˵������ϼ�������
                    }
                    if (checkStep != "") checkStep = checkStep.Substring(0, checkStep.Length - 1);

                    if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "һ�����")//ָ����Ա ���� ͬʱָ����ɫ����Ա
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == "" && temp.Tables[0].Rows[i]["wkUser"].ToString().Trim() == userCode)//��ָ����Ա�ģ�ȫ����λ
                        {
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and isnull(b.checkGroup,'')='' and isnull(b.stepUser,'')='" + userCode + "' and b.wkModel='һ�����')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//����Ȩ�޵��ݵı��
                            }
                        }
                        else if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID && temp.Tables[0].Rows[i]["wkUser"].ToString().Trim() == userCode)//ͬʱ��ɫ����Ա��Ȩ�޵�λ�ڵĵ���
                        {
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in (" + rightDeptCodes + ") and  flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in ("+checkStep+") and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.stepUser='" + userCode + "' and b.wkModel='һ�����')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//����Ȩ�޵��ݵı��
                            }
                        }
                    }
                    else if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "ҵ���������")
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID)
                        {
                            //������˻�ֹ��쵼���
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in(select deptCode from bill_dept_ywzg where userCode='" + userCode + "') and flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.wkModel='ҵ���������')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//����Ȩ�޵��ݵı��
                            }
                        }
                    }
                    else if (temp.Tables[0].Rows[i]["wkModel"].ToString().Trim() == "�ֹ��쵼���")
                    {
                        if (temp.Tables[0].Rows[i]["wkGroup"].ToString().Trim() == groupID)
                        {//������˻�ֹ��쵼���
                            DataSet temp3 = server.GetDataSet("select * from bill_main a where billDept in (select deptCode from bill_dept_fgld where userCode='" + userCode + "') and flowID='" + flowID + "' and stepID in (" + checkStep + ") and billCode not in (select billCode from bill_workFlowRecord b where b.flowID='" + flowID + "' and b.beginstep in (" + checkStep + ")  and b.billCode=a.billCode and b.loopTimes=a.loopTimes and b.checkGroup='" + groupID + "' and b.wkModel='�ֹ��쵼���')");
                            for (int h = 0; h <= temp3.Tables[0].Rows.Count - 1; h++)
                            {
                                billCodes += "'" + temp3.Tables[0].Rows[h]["billCode"].ToString().Trim() + "',";//����Ȩ�޵��ݵı��
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
        /// ������˷��� 
        /// </summary>
        /// <param name="flowID">��������</param>
        /// <param name="billCode">���ݱ��</param>
        /// <param name="checkUser">�����</param>
        /// <param name="checkDate">���ʱ�䣺��ϸ����</param>
        /// <param name="checkBz">����������ע��</param>
        /// <param name="result">��ͽ����trueͨ�� false��ͨ��</param>
        /// <returns>�ɹ������ؿ� ���򣺷�����Ӧ����ʾ��Ϣ</returns>
        public string checkBills(string flowID, string billCode, string checkUser, string checkDate, string checkBz, bool result)
        {
            string userGroup = server.GetCellValue("select userGroup from bill_users where userCode='" + checkUser + "'");
            if (userGroup == "")
            {
                return "��ǰ��Ա���û��飡";
            }

            List<string> list = new List<string>();
            DataSet temp = server.GetDataSet("select * from bill_main where flowid='" + flowID + "' and billCode='" + billCode + "'");//��ȡ���ݵ���ϸ��Ϣ
            string stepID = temp.Tables[0].Rows[0]["stepID"].ToString().Trim();//��ǰ������
            string loopTimes = temp.Tables[0].Rows[0]["loopTimes"].ToString().Trim();//�ڼ����ύ���

            //��ȡ���п��ܵ���һ���� �����ǵ�ǰ��Ա��Ȩ�޵Ĳ���:��������Ȩ�� || ������������Ա || ����Ա  ��һ���� ֻ����ֻ��״̬���������
            DataSet temp1 = server.GetDataSet("select actionTo from bill_workFlowAction where flowID='" + flowID + "' and actionFrom='" + stepID + "'");// and actionTo in (select stepID from bill_workFlowGroup where flowid='" + flowID + "' and (wkGroup='" + userGroup + "' and (isnull(wkUser,'')='' or isnull(wkUser,'')='" + checkUser + "')) or (isnull(wkGroup,'')='' and isnull(wkUser,'')='" + checkUser + "'))");
            if (temp1.Tables[0].Rows.Count == 0)
            {
                return "δ�ҵ���Ӧ��������̣�";
            }


            //�ҵ���һ������
            string actionTo = temp1.Tables[0].Rows[0][0].ToString().Trim();

            //��һ�����Ȩ���������
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
                    //���ǵ����ڶ������״̬�����ô�������ֱ�Ӹ���Ϊ����״̬
                }
                else
                {
                    actionTo = "end";
                }
                list.Add("update bill_main set stepID='" + actionTo + "' where flowID='" + flowID + "' and billCode='" + billCode + "'");
                list.Add("insert into bill_workFlowRecord(billcode,flowid,beginstep,endstep,checkuser,checkdate,checkbz,looptimes,checkgroup,result,stepUser,wkModel) values('" + billCode + "','" + flowID + "','" + stepID + "','" + actionTo + "','" + checkUser + "','" + checkDate + "','" + checkBz + "','" + loopTimes + "','" + wkGroup + "','true','" + wkUser + "','" + wkModel + "')");

            }

            if (result == true && actionTo == "end" && flowID == "ystz")//Ԥ����������⴦��
            {
                string sCode = server.GetCellValue("select sCode from bill_ystz where billcode='" + billCode + "'");
                string tCode = server.GetCellValue("select tCode from bill_ystz where billcode='" + billCode + "'");
                string nian = server.GetCellValue("select nian from bill_ysgc where gcbh='" + sCode + "'");
                int sYue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + sCode + "'"));
                int tYue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + tCode + "'"));

                string sJd = "";
                if (sYue >= 1 && sYue <= 3) { sJd = "һ"; }
                if (sYue >= 4 && sYue <= 6) { sJd = "��"; }
                if (sYue >= 7 && sYue <= 9) { sJd = "��"; }
                if (sYue >= 10 && sYue <= 12) { sJd = "��"; }
                string sGcbh_Jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='"+sJd+"' and ystype='1'");

                string tJd = "";
                if (tYue >= 1 && tYue <= 3) { tJd = "һ"; }
                if (tYue >= 4 && tYue <= 6) { tJd = "��"; }
                if (tYue >= 7 && tYue <= 9) { tJd = "��"; }
                if (tYue >= 10 && tYue <= 12) { tJd = "��"; }
                string tGcbh_Jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='"+tJd+"' and ystype='1'");

                //��ʼ������Ԥ���ֵ
                string billDept = server.GetCellValue("select billdept from bill_main where billcode='" + billCode + "'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_before where billcode='" + billCode + "' and bill_ystz_before.km=bill_ysmxb.yskm),0) where gcbh='" + sCode + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_after where billcode='" + billCode + "' and bill_ystz_after.km=bill_ysmxb.yskm),0) where gcbh='" + tCode + "' and ysdept='" + billDept + "' and ystype='1'");

                if (sJd == tJd)
                {
                    //���������Ԥ��
                }
                else { //��������Ԥ��
                    list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_before where billcode='" + billCode + "' and bill_ystz_before.km=bill_ysmxb.yskm),0) where gcbh='" + sGcbh_Jd + "' and ysdept='" + billDept + "' and ystype='1'");
                    list.Add("update bill_ysmxb set ysje=ysje+isnull((select tje-sje from bill_ystz_after where billcode='" + billCode + "' and bill_ystz_after.km=bill_ysmxb.yskm),0) where gcbh='" + tGcbh_Jd + "' and ysdept='" + billDept + "' and ystype='1'");
                }
            }
            else if (result == true && actionTo == "end" && flowID == "yszj")//Ԥ��׷��
            {
                DataSet gcDataSet = server.GetDataSet("select top 1 * from bill_ysmxb where billcode='" + billCode + "'");
                string gcbh_yue = gcDataSet.Tables[0].Rows[0]["gcbh"].ToString().Trim();
                gcDataSet = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh_yue + "'");

                int yue = int.Parse(server.GetCellValue("select yue from bill_ysgc where gcbh='" + gcbh_yue + "'"));
                string jd = "";
                if (yue >= 1 && yue <= 3) { jd = "һ"; }
                if (yue >= 4 && yue <= 6) { jd = "��"; }
                if (yue >= 7 && yue <= 9) { jd = "��"; }
                if (yue >= 10 && yue <= 12) { jd = "��"; }
                string nian = gcDataSet.Tables[0].Rows[0]["nian"].ToString().Trim();
                string gcbh_jd = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='" + jd + "' and ystype='1'");
                string gcbh_nian = server.GetCellValue("select gcbh from bill_ysgc where nian='" + nian + "' and yue='' and ystype='0'");
                string billDept = server.GetCellValue("select billdept from bill_main where billcode='" + billCode + "'");

                list.Add("select * into ysmxb" + checkUser + " from bill_ysmxb where billCode='" + billCode + "'");//������ʱ�� �洢Ԥ��׷������
                //����ȱ�ٵ�Ԥ���Ŀ��
                string tempStr20120119 = "";
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_yue + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_jd + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);
                tempStr20120119 = "insert into bill_ysmxb select '" + gcbh_nian + "',(select top 1 billcode from bill_ysmxb where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1'),yskm,0,ysdept,'1' from bill_ysmxb where billCode='" + billCode + "' and yskm not in (select yskm from bill_ysmxb where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1')";
                list.Add(tempStr20120119);

                //�����ܶ�
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_yue + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_jd + "' and ysdept='" + billDept + "' and ystype='1'");
                list.Add("update bill_ysmxb set ysje=ysje+isnull((select ysje from ysmxb" + checkUser + " where ysmxb" + checkUser + ".yskm=bill_ysmxb.yskm),0) where gcbh='" + gcbh_nian + "' and ysdept='" + billDept + "' and ystype='1'");

                list.Add("drop table ysmxb" + checkUser);
            }


            if (server.ExecuteNonQuerysArray(list) == -1)
            {

                return "���ʧ�ܣ�";
            }
            else
            {
                return "�����ɣ�";
            }
        }

        /// <summary>
        /// �����⣺��ʱ����
        /// �ݹ��ȡ���ݵ���һ״̬��Ŀ�ģ���������˹��õ��ݵ���Ա����ֹ�ظ����
        /// </summary>
        /// <returns></returns>
        private string getNextStepIDLoop(string flowID, string pStepID, string loopTimes)
        {
            DataSet temp = server.GetDataSet("select * from bill_workflowaction where flowID='" + flowID + "' and actionFrom='" + pStepID + "'");//��ȡ��һ���Ĳ�������

            temp = server.GetDataSet("select * from bill_workflowGroup where flowID='" + flowID + "' and stepID='" + temp.Tables[0].Rows[0]["actionTo"].ToString().Trim() + "'");//��һ�� ������̵�Ȩ���������

            string user = temp.Tables[0].Rows[0]["wkUser"].ToString().Trim();//��һ������Ȩ����˵���Ա
            if (user == "")//�����ڲ���ˣ����������֮ǰ�����ԡ�����
            {
                return pStepID;
            }
            else { 
                //�ƶ���Ա��˵�������ж�Ȩ����Ա�Ƿ��Ǹõ��ݵ�λ��ҵ�����ܣ�����ֱ������
                return "";
            }
        }


        /// <summary>
        /// ��ȡĳһ�������ͨ�������һ״̬ ���ڸ���bill_main���stepID�ֶ�ֵ
        /// </summary>
        /// <param name="flowID">�������ͣ�ÿ�ֵ��ݵĹ̶�ֵ</param>
        /// <param name="currentStepID">�õ��ݵ�ǰ�Ĳ�����</param>
        /// <returns>�������ݸ�ʽ</returns>
        private string getNextStepID(string flowID, string currentStepID)
        {
            //���ǻ���Ͳ��е����
            return "";
        }


        public string getShlcWord(string flowID)
        {
            try
            {
                string returnStr = "�Ƶ��ύ";
                DataSet temp = server.GetDataSet("select * from bill_workflowAction where flowID='" + flowID + "' and actionFrom='begin'");
                if (temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() == "end")
                {
                    returnStr += "-->����";
                }
                else
                {
                    returnStr += "-->" + server.GetCellValue("select stepText from bill_workflowstep where flowid='" + flowID + "' and stepID='" + temp.Tables[0].Rows[0]["actionTo"].ToString().ToLower() + "'");
                    this.getShlcWord(flowID, temp.Tables[0].Rows[0]["actionTo"].ToString().Trim(), ref returnStr);
                }
                return returnStr;
            }
            catch {
                return "";//����������ô���
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
                    returnStr += "-->����";
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
