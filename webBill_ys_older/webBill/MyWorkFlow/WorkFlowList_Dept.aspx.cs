using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowDal;
using WorkFlowLibrary.WorkFlowModel;

public partial class webBill_MyWorkFlow_WorkFlowList_Dept : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            Response.Redirect("~/login.aspx");
            return;
        }
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        if (!IsPostBack)
        {
            string flowid = Request.Params["flowid"];
            string billdept = Request.Params["billdept"];
            if (!string.IsNullOrEmpty(flowid))
            {
                string flowName = server.GetCellValue("  select flowName from	dbo.mainworkflow where flowid='" + flowid + "' ");
                lb_msg.Text = "你当前选择的单据：" + flowName;
            }
            
            if (!string.IsNullOrEmpty(flowid) && !string.IsNullOrEmpty(billdept))
            {
                this.TextBox3.Text = DateTime.Now.ToString("yyyy-MM-dd");
                hd_flowid.Value = flowid;
                WorkFlowManager wfmg = new WorkFlowManager();
                IList<ConfigModel> configlist = XmlHelper.GetConfigAll();

                this.DropDownList1.DataSource = configlist;
                DropDownList1.DataTextField = "Typename";
                DropDownList1.DataValueField = "Typecode";
                DropDownList1.DataBind();

                //DropDownList1.Items.Add(new ListItem("请选择..", ""));

                //DropDownList1.SelectedValue = "";

                MainWorkFlow work = wfmg.GetWorkFlow(flowid, billdept);

                if (work == null || work.StepList == null)
                {
                    return;
                }
                StringBuilder sb = new StringBuilder();
                foreach (WorkFlowStep step in work.StepList)
                {
                    string sptype = step.StepType;
                    string spr = step.CheckCode;
                    string sxje = Convert.ToString(step.MinMoney);
                    string maxmoney = Convert.ToString(step.MaxMoney);
                    string sxrq = "";

                    if (step.MinDate.ToString() != "0001-1-1 0:00:00")
                    {
                        sxrq = step.MinDate.ToString();
                    }

                    string bz = step.Memo;
                    string type = step.CheckType;
                    GetName(ref sptype, ref spr);
                    sb.Append("<li id='li_");
                    sb.Append(Convert.ToString(step.StepId));
                    sb.Append("' class='ui-state-default'><input type='checkbox' id='chk_");
                    sb.Append(Convert.ToString(step.StepId));
                    sb.Append("' /><table><tr><td style='text-align:left'>审批类型:</td><td style='text-align:left'> ");
                    sb.Append(sptype);
                    sb.Append("</td><td style='text-align:right;' >审批人:</td><td  style='text-align:right'>");
                    sb.Append(spr);
                  
                    //隐藏虽然不显示单需要保存的时候获取数据
                    sb.Append("</td><td style='display:none'>生效金额:</td><td style='display:none'>");
                    if (sxje != "0")
                    {
                        sb.Append(sxje);
                    }
                    if (!string.IsNullOrEmpty(step.MaxMoney.ToString()))
                    {
                        sb.Append("</td><td style='display:none'>限额</td><td style='display:none'>");
                        sb.Append(step.MaxMoney.ToString());
                    }
                    sb.Append("</td><td style='display:none'>生效日期:</td><td style='display:none'>");
                    sb.Append(sxrq);

                    sb.Append("</td><td style='display:none'>类型:</td><td style='display:none'>");
                    if (type == "2")
                    {
                        sb.Append(step.CheckType);
                        sb.Append("[会签]");
                    }
                    else
                    {
                        sb.Append(step.CheckType);
                        sb.Append("[单签]");
                    }
                    sb.Append("</td><td style='display:none'>是否必须科目主管：</td><td style='display:none'>");
                    string strIskmzg = step.IsKmZg;
                    strIskmzg = strIskmzg.Equals("") ? "0" : strIskmzg;
                    sb.Append(strIskmzg + "[" + (strIskmzg == "0" ? "否" : "是") + "]");
                    sb.Append("</td><td></td><td>");
                    //sb.Append(bz);
                    sb.Append("</td>");
                    //显示流程
                    sb.Append("<td title='");
                    sb.Append("生效金额:");
                    if (sxje != "0")
                    {
                        sb.Append(sxje);
                    }
                    if (!string.IsNullOrEmpty(step.MaxMoney.ToString()))
                    {
                        sb.Append("；限额");
                        sb.Append(step.MaxMoney.ToString());
                    }
                    sb.Append("；生效日期:");
                    sb.Append(sxrq);

                    sb.Append("；类型:");
                    if (type == "2")
                    {
                        sb.Append(step.CheckType);
                        sb.Append("[会签]");
                    }
                    else
                    {
                        sb.Append(step.CheckType);
                        sb.Append("[单签]");
                    }
                    sb.Append("；费用类别：");
                    string strkmType = step.kmType;
                    strkmType = strkmType.Equals("") ? "0" : strkmType;
                    sb.Append(strkmType + "[" + (strkmType == "0" ? "不设置" : "总金额") + "]");
                    sb.Append("'>...</td>");
                    //备注拿出来  不显示 备注:

                    sb.Append("</tr></table></li>");

                }
                sortable.InnerHtml = (sb.ToString());
            }
        }
    }



    private void GetName(ref string sptype, ref string spr)
    {
        IDictionary<string, string> dic = XmlHelper.GetWFTypeConfig(sptype);
        StringBuilder ret = new StringBuilder();
        if (!string.IsNullOrEmpty(spr))
        {
            string[] array = spr.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                if (!string.IsNullOrEmpty(array[i]))
                {
                    string sql = "select " + dic["filter"] + " as ccode," + dic["codemainvalue"] + " as cname  from " + dic["codemaintable"] + " where " + dic["filter"] + " ='" + array[i] + "'";
                    DataTable dt = server.GetDataSet(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        ret.Append(array[i]);
                        ret.Append("[");

                        ret.Append(Convert.ToString(dt.Rows[0]["cname"]));
                        ret.Append("]:");

                    }
                }


                //spr += "[" + Convert.ToString(dt.Rows[0]["cname"]) + "]";
            }
            if (ret.Length - 1 > 0)
            {
                spr = ret.Remove(ret.Length - 1, 1).ToString();//ret.ToString();
            }

            /*
            string sql = "select " + dic["filter"] + " as ccode," + dic["codemainvalue"] + " as cname  from " + dic["codemaintable"] + " where " + dic["filter"] + " ='" + spr + "'";
            DataTable dt = server.GetDataSet(sql).Tables[0];
            spr += "[" + Convert.ToString(dt.Rows[0]["cname"]) + "]";
             */
        }
        sptype += "[" + dic["typename"] + "]";
    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select usercode+'['+username+']' as username from bill_users where userStatus='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

}