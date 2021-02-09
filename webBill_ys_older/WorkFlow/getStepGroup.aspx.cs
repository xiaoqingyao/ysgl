using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WorkFlow_getStepGroup : System.Web.UI.Page
{
    public string strJson;
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        string groupStr = "<table>";
        DataSet temp = server.GetDataSet("select * from bill_workFlowGroup where flowID='" + Page.Request.QueryString["flowID"].ToString().Trim() + "' and stepID='" + Page.Request.QueryString["stepID"].ToString().Trim() + "'");
        if (temp.Tables[0].Rows.Count == 0)
        {
            groupStr += "<tr><td><font color=red>未设置</front></td></tr>";
        }
        else
        {
            if (temp.Tables[0].Rows[0]["wkModel"].ToString().Trim() == "")
            {
                groupStr += "<tr><td>一般审核</td></tr>";
            }
            else {
                groupStr += "<tr><td>" + temp.Tables[0].Rows[0]["wkModel"].ToString().Trim() + "</td></tr>";
            }
            if (temp.Tables[0].Rows[0]["wkGroup"].ToString().Trim() == "")//只指定到人员
            {
                temp = server.GetDataSet("select '【'+userCode+'】'+userName as userName from bill_users where userCode in (select wkUser from bill_workflowgroup where flowID='" + Page.Request.QueryString["flowID"].ToString().Trim() + "' and stepID='" + Page.Request.QueryString["stepID"].ToString().Trim() + "')");
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    groupStr += "<tr><td>人员：" + temp.Tables[0].Rows[i]["userName"].ToString().Trim()+"</td></tr>";
                }
            }
            else
            {
                temp = server.GetDataSet("select distinct b.groupID,b.groupName,a.flowMode,a.wkModel from bill_workFlowGroup a,bill_userGroup b where b.groupID=a.wkGroup and a.flowID='" + Page.Request.QueryString["flowID"].ToString().Trim() + "' and a.stepID='" + Page.Request.QueryString["stepID"].ToString().Trim() + "'");

             
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    groupStr += "<tr><td colSpan=2>角色：【" + temp.Tables[0].Rows[i]["groupID"].ToString().Trim() + "】" + temp.Tables[0].Rows[i]["groupName"].ToString().Trim() + "</td></tr>";

                    DataSet temp2 = server.GetDataSet("select '【'+userCode+'】'+userName as userName from bill_users where userCode in (select isnull(wkUser,'') from bill_workFlowGroup where flowID='" + Page.Request.QueryString["flowID"].ToString().Trim() + "' and stepID='" + Page.Request.QueryString["stepID"].ToString().Trim() + "' and wkGroup='" + temp.Tables[0].Rows[i]["groupID"].ToString().Trim() + "')");
                    if (temp2.Tables[0].Rows.Count == 0)
                    { }
                    else
                    {
                        for (int j = 0; j <= temp2.Tables[0].Rows.Count - 1; j++)
                        {
                            groupStr += "<tr><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;人员：</td><td>" + temp2.Tables[0].Rows[j]["userName"].ToString().Trim() + "</td></tr>";
                        }
                    }
                }
            }
        }
        groupStr += "</table>";

        groupStr = "{\"group\":[\"" + groupStr + "\"]}";

        string modelStr = "";
        if (temp.Tables[0].Rows.Count == 0)
        {
            modelStr = "{\"model\":\"并行\"}";
        }
        else
        {
            modelStr = "{\"model\":\"\"}";
        }

        strJson = "{ret:[" + groupStr + "," + modelStr + "]}";
    }
}