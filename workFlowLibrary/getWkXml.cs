using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace workFlowLibrary
{
    public class getWkXml
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        public string getXml(string flowID)
        {
            string strJson = "";
            strJson = "<?xml version='1.0' encoding='gb2312'?>";
            strJson += "<WebFlow>";

            DataSet wkDataSet = server.GetDataSet("select * from bill_workFlow where flowID='" +flowID + "'");
            strJson += "<FlowConfig>";
            strJson += "<BaseProperties flowId=\"" + wkDataSet.Tables[0].Rows[0]["flowID"].ToString().Trim() + "\" flowText=\"" + wkDataSet.Tables[0].Rows[0]["flowText"].ToString().Trim().Replace("?", "") + "\"/>";
            strJson += "<VMLProperties stepTextColor=\"" + wkDataSet.Tables[0].Rows[0]["stepTextColor"].ToString().Trim() + "\" stepStrokeColor=\"" + wkDataSet.Tables[0].Rows[0]["stepStrokeColor"].ToString().Trim() + "\" stepShadowColor=\"" + wkDataSet.Tables[0].Rows[0]["stepShadowColor"].ToString().Trim() + "\" stepFocusedStrokeColor=\"" + wkDataSet.Tables[0].Rows[0]["stepFocusedStrokeColor"].ToString().Trim() + "\" isStepShadow=\"" + wkDataSet.Tables[0].Rows[0]["isStepShadow"].ToString().Trim() + "\" actionStrokeColor=\"" + wkDataSet.Tables[0].Rows[0]["actionStrokeColor"].ToString().Trim() + "\" actionTextColor=\"" + wkDataSet.Tables[0].Rows[0]["actionTextColor"].ToString().Trim() + "\" actionFocusedStrokeColor=\"" + wkDataSet.Tables[0].Rows[0]["actionFocusedStrokeColor"].ToString().Trim() + "\" sStepTextColor=\"" + wkDataSet.Tables[0].Rows[0]["sStepTextColor"].ToString().Trim() + "\" sStepStrokeColor=\"" + wkDataSet.Tables[0].Rows[0]["sStepStrokeColor"].ToString().Trim() + "\" stepColor1=\"" + wkDataSet.Tables[0].Rows[0]["stepColor1"].ToString().Trim() + "\" stepColor2=\"" + wkDataSet.Tables[0].Rows[0]["stepColor2"].ToString().Trim() + "\" isStep3D=\"" + wkDataSet.Tables[0].Rows[0]["isStep3D"].ToString().Trim() + "\" step3DDepth=\"" + wkDataSet.Tables[0].Rows[0]["step3DDepth"].ToString().Trim() + "\"/>";
            strJson += "<FlowProperties flowMode=\"\" startTime=\"\" endTime=\"\" ifMonitor=\"\" runMode=\"\" noteMode=\"\" activeForm=\"\" autoExe=\"\"/>";
            strJson += "</FlowConfig>";


            strJson += "<Steps>";
            DataSet stepDataSet = server.GetDataSet("select * from bill_workFlowStep where flowID='" +flowID + "'");
            for (int i = 0; i <= stepDataSet.Tables[0].Rows.Count - 1; i++)
            {
                strJson += "<Step>";
                strJson += "<BaseProperties id=\"" + stepDataSet.Tables[0].Rows[i]["stepID"].ToString().Trim() + "\" text=\"" + stepDataSet.Tables[0].Rows[i]["stepText"].ToString().Trim().Replace("?", "") + "\" stepType=\"" + stepDataSet.Tables[0].Rows[i]["stepType"].ToString().Trim() + "\"/>";
                strJson += "<VMLProperties width=\"" + stepDataSet.Tables[0].Rows[i]["v_width"].ToString().Trim() + "\" height=\"" + stepDataSet.Tables[0].Rows[i]["v_height"].ToString().Trim() + "\" x=\"" + stepDataSet.Tables[0].Rows[i]["v_x"].ToString().Trim() + "\" y=\"" + stepDataSet.Tables[0].Rows[i]["v_y"].ToString().Trim() + "\" textWeight=\"" + stepDataSet.Tables[0].Rows[i]["textWeight"].ToString().Trim() + "\" strokeWeight=\"" + stepDataSet.Tables[0].Rows[i]["strokeWeight"].ToString().Trim() + "\" isFocused=\"" + stepDataSet.Tables[0].Rows[i]["isFocused"].ToString().Trim() + "\" zIndex=\"" + stepDataSet.Tables[0].Rows[i]["zIndex"].ToString().Trim() + "\"/>";
                DataSet wkGroupDataSet = server.GetDataSet("select * from bill_workFlowGroup where flowid='" +flowID + "' and stepid='" + stepDataSet.Tables[0].Rows[i]["stepID"].ToString().Trim() + "'");

                string flowMode = "";
                string wkGroup = "";
                if (wkGroupDataSet.Tables[0].Rows.Count == 0)
                { }
                else
                {
                    flowMode = wkGroupDataSet.Tables[0].Rows[0]["flowMode"].ToString().Trim();
                }
                for (int x = 0; x <= wkGroupDataSet.Tables[0].Rows.Count - 1; x++)
                {
                    wkGroup += wkGroupDataSet.Tables[0].Rows[0]["wkGroup"].ToString().Trim() + ",";
                }
                if (wkGroup != "")
                {
                    wkGroup = wkGroup.Substring(0, wkGroup.Length - 1);
                }

                strJson += "<FlowProperties wkGroup=\"" + wkGroup + "\" flowMode=\"" + flowMode + "\" />";
                strJson += "</Step>";
            }
            strJson += "</Steps><Actions>";

            DataSet actioinDataSet = server.GetDataSet("select * from bill_workFlowAction where flowID='" +flowID + "'");
            for (int i = 0; i <= actioinDataSet.Tables[0].Rows.Count - 1; i++)
            {
                strJson += "<Action>";
                strJson += "<BaseProperties id=\"" + actioinDataSet.Tables[0].Rows[i]["actionID"].ToString().Trim() + "\" text=\"" + actioinDataSet.Tables[0].Rows[i]["actionText"].ToString().Trim().Replace("?", "") + "\" actionType=\"" + actioinDataSet.Tables[0].Rows[i]["actionType"].ToString().Trim() + "\" from=\"" + actioinDataSet.Tables[0].Rows[i]["actionFrom"].ToString().Trim() + "\" to=\"" + actioinDataSet.Tables[0].Rows[i]["actionTo"].ToString().Trim() + "\"/>";
                strJson += "<VMLProperties startArrow=\"" + actioinDataSet.Tables[0].Rows[i]["startArrow"].ToString().Trim() + "\" endArrow=\"" + actioinDataSet.Tables[0].Rows[i]["endArrow"].ToString().Trim() + "\" strokeWeight=\"" + actioinDataSet.Tables[0].Rows[i]["strokeWeight"].ToString().Trim() + "\" isFocused=\"" + actioinDataSet.Tables[0].Rows[i]["isFocused"].ToString().Trim() + "\" zIndex=\"" + actioinDataSet.Tables[0].Rows[i]["zIndex"].ToString().Trim() + "\"/>";
                strJson += "<FlowProperties/>";
                strJson += "</Action>";
            }
            strJson += "</Actions>";

            strJson += "</WebFlow>";

            return strJson;
        }
    }
}
