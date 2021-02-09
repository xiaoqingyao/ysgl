using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.IO;
using System.Xml;
using System.Text;


namespace workFlowLibrary
{
    public class saveWkXml
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public string saveXml(string xml)
        {
            List<string> list = new List<string>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNode root = xmlDoc.SelectSingleNode("//WebFlow");

            //配置节
            XmlNode FlowConfig = root.SelectSingleNode("//FlowConfig");
            XmlNode basicConfig = FlowConfig.SelectSingleNode("//BaseProperties");
            XmlNode basciProper = FlowConfig.SelectSingleNode("//VMLProperties");

            string flowId = basicConfig.Attributes["flowId"].Value.ToString().Trim();

            #region 判断是否可以修改流程
            DataSet temp = server.GetDataSet("select count(1) from bill_main where flowID='" + flowId + "' and stepID not in ('-1','end','0')");//未提交 结束 和 退回
            if (temp.Tables[0].Rows[0][0].ToString().Trim() != "0")
            {
                return "使用该流程的单据未完全审核通过！";
            }
            #endregion
            list.Add("delete from bill_workFlow where flowid='" + flowId + "'");
            list.Add("delete from bill_workFlowStep where flowid='" + flowId + "'");
            list.Add("delete from bill_workFlowAction where flowid='" + flowId + "'");

            #region
            string sqlBasic = "insert into bill_workFlow(flowId,flowText,stepTextColor,stepStrokeColor,stepShadowColor,stepFocusedStrokeColor,isStepShadow,actionStrokeColor,actionTextColor,";
            sqlBasic += "actionFocusedStrokeColor,sStepTextColor,sStepStrokeColor,stepColor1,stepColor2,isStep3D,step3DDepth,orderBy) ";
            sqlBasic += "values('" + basicConfig.Attributes["flowId"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basicConfig.Attributes["flowText"].Value.ToString().Trim().Replace("?", "") + "',";
            sqlBasic += "'" + basciProper.Attributes["stepTextColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["stepStrokeColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["stepShadowColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["stepFocusedStrokeColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["isStepShadow"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["actionStrokeColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["actionTextColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["actionFocusedStrokeColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["sStepTextColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["sStepStrokeColor"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["stepColor1"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["stepColor2"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["isStep3D"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + basciProper.Attributes["step3DDepth"].Value.ToString().Trim() + "',";
            sqlBasic += "'" + System.DateTime.Now.ToString() + "')";
            list.Add(sqlBasic);
            #endregion

            //步骤节
            XmlNode Steps = root.SelectSingleNode("//Steps");
            for (int i = 0; i <= Steps.ChildNodes.Count - 1; i++)
            {
                XmlNode stepBasci = Steps.ChildNodes[i].ChildNodes[0];
                XmlNode stepProper = Steps.ChildNodes[i].ChildNodes[1];
                XmlNode stepGroup = Steps.ChildNodes[i].ChildNodes[2];

                #region Create SQL
                string sql = "insert into bill_workFlowStep(flowID,stepID,stepText,stepType,v_width,v_height,v_x,v_y,textWeight,strokeWeight,isFocused,zIndex) ";
                sql += " values('" + flowId + "',";
                try
                {
                    sql += "'" + stepBasci.Attributes["id"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepBasci.Attributes["text"].Value.ToString().Trim().Replace("?","") + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepBasci.Attributes["stepType"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["width"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["height"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["x"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["y"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["textWeight"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["strokeWeight"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["isFocused"].Value.ToString().Trim() + "',";

                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + stepProper.Attributes["zIndex"].Value.ToString().Trim() + "')";

                }
                catch
                {
                    sql += "'')";
                }

                #endregion

                //string userGroup = stepGroup.Attributes["wkGroup"].Value.ToString().Trim();
                string flowMode = stepGroup.Attributes["flowMode"].Value.ToString().Trim();
                if (flowMode == "")
                { }
                else
                {
                    //string[] wkGroup = userGroup.Split(',');
                    //for (int y = 0; y <= wkGroup.Length - 1; y++)
                    //{
                    //    if (stepBasci.Attributes["id"].Value.ToString().Trim() == "begin" || stepBasci.Attributes["id"].Value.ToString().Trim() == "end")
                    //    { }
                    //    else
                    //    {
                            //list.Add("insert into bill_workFlowGroup(flowid,stepid,wkGroup,flowMode) values('" + flowId + "','" + stepBasci.Attributes["id"].Value.ToString().Trim() + "','" + wkGroup[y].ToString().Trim() + "','" + flowMode + "')");
                            list.Add("update bill_workFlowGroup set flowMode='" + flowMode + "' where flowID='" + flowId + "' and stepID='" + stepBasci.Attributes["id"].Value.ToString().Trim() + "'");
                        //}
                    //}
                }

                list.Add(sql);
            }

            //活动节
            XmlNode Actions = root.SelectSingleNode("//Actions");
            for (int i = 0; i <= Actions.ChildNodes.Count - 1; i++)
            {
                XmlNode actioinBasci = Actions.ChildNodes[i].ChildNodes[0];
                XmlNode actioinProper = Actions.ChildNodes[i].ChildNodes[1];

                #region Create SQL
                string sql = "insert into bill_workFlowAction(flowID,actionID,actionText,actionType,actionFrom,actionTo,startArrow,endArrow,strokeWeight,isFocused,zIndex) ";
                sql += " values('" + flowId + "',";
                try
                {
                    sql += "'" + actioinBasci.Attributes["id"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinBasci.Attributes["text"].Value.ToString().Trim().Replace("?", "") + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinBasci.Attributes["actionType"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinBasci.Attributes["from"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinBasci.Attributes["to"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinProper.Attributes["startArrow"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinProper.Attributes["endArrow"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinProper.Attributes["strokeWeight"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinProper.Attributes["isFocused"].Value.ToString().Trim() + "',";
                }
                catch
                {
                    sql += "'',";
                }
                try
                {
                    sql += "'" + actioinProper.Attributes["zIndex"].Value.ToString().Trim() + "')";
                }
                catch
                {
                    sql += "'')";
                }
                #endregion

                list.Add(sql);
            }

            list.Add("delete from bill_workFlowGroup where flowid='" + flowId + "' and stepID not in (select stepID from bill_workflowstep where flowid='" + flowId + "')");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                return "保存失败！";
            }
            else
            {
                return "";
            }
        }
    }
}
