using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowModel;
using WorkFlowLibrary.WorkFlowDal;
using System.Text;
using System.Data;

public partial class webBill_MyWorkFlow_WorkFlowList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string flowid = Request.Params["flowid"];
            if (!string.IsNullOrEmpty(flowid))
            {
                this.TextBox3.Text = DateTime.Now.ToString("yyyy-MM-dd");
                hd_flowid.Value = flowid;
                WorkFlowManager wfmg = new WorkFlowManager();
                IList<ConfigModel> configlist = XmlHelper.GetConfigAll();

                this.DropDownList1.DataSource = configlist;
                DropDownList1.DataTextField = "Typename";
                DropDownList1.DataValueField = "Typecode";
                DropDownList1.DataBind();
                DropDownList1.Items.Add(new ListItem("请选择..", ""));

                DropDownList1.SelectedValue = "";

                MainWorkFlow work = wfmg.GetWorkFlow(flowid, "");

                StringBuilder sb = new StringBuilder();

                if (work != null && work.StepList.Count > 0)
                {
                    foreach (WorkFlowStep step in work.StepList)
                    {
                        string sptype = step.StepType;
                        string spr = step.CheckCode;
                        string sxje = Convert.ToString(step.MinMoney);
                        string sxrq = "";
                        string kmtype = step.kmType;

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
                        sb.Append("' /><table><tr><td>审批类型:</td><td>");
                        sb.Append(sptype);
                        sb.Append("</td><td>审批人:</td><td>");
                        sb.Append(spr);
                        sb.Append("</td><td>费用类型:</td><td>");
                        sb.Append(kmtype == "sum" ? "sum" : server.GetCellValue("select dicCode+'['+dicName+']' from  bill_dataDic	 where dicType='22' and dicCode='" + kmtype + "'  "));
                        sb.Append("</td><td>生效金额:</td><td>");
                        if (sxje != "0")
                        {
                            sb.Append(sxje);
                        }

                        //if (sxrq != "")
                        //{
                        //    sb.Append(sxrq);
                        //}
                        //else
                        //{
                        //    sb.Append(DateTime.Now.ToString("yyyy-MM-dd"));
                        //}
                        sb.Append("</td><td>类型:</td><td>");
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
                        sb.Append("</td><td>必须科目主管：</td><td>");
                        string strIskmzg = step.IsKmZg;
                        strIskmzg = strIskmzg.Equals("") ? "0" : strIskmzg;
                        sb.Append(strIskmzg + "[" + (strIskmzg == "0" ? "否" : "是") + "]");
                        sb.Append("</td><td>生效日期:</td><td>");
                        sb.Append(sxrq);
                        sb.Append("</td><td>备注:</td><td>");
                        sb.Append(bz);
                        sb.Append("</td>");

                        sb.Append("</tr></table></li>");
                    }
                }
                sortable.InnerHtml = (sb.ToString());

                if (flowid == "ybbx")
                {
                    DataTable dtbill = server.GetDataTable("select diccode,dicname from bill_datadic where dictype='22'", null);
                    if (dtbill != null && dtbill.Rows.Count > 0)
                    {
                        this.DropDownList2.DataSource = dtbill;
                        this.DropDownList2.DataTextField = "dicname";
                        this.DropDownList2.DataValueField = "diccode";
                        this.DropDownList2.DataBind();
                    }
                }
                this.DropDownList2.Items.Insert(0, new ListItem("不设置", ""));
                this.DropDownList2.Items.Insert(1, new ListItem("总金额", "sum"));


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
                string sql = "select " + dic["filter"] + " as ccode," + dic["codemainvalue"] + " as cname  from " + dic["codemaintable"] + " where " + dic["filter"] + " ='" + array[i] + "'";
                DataTable dt = server.GetDataSet(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ret.Append(array[i]);
                    ret.Append("[");
                    ret.Append(Convert.ToString(dt.Rows[0]["cname"]));
                    ret.Append("]:");
                }

                //spr += "[" + Convert.ToString(dt.Rows[0]["cname"]) + "]";
            }
            if (ret.Length - 1 > 0)
            {
                spr = ret.Remove(ret.Length - 1, 1).ToString();
            }

            /*
            string sql = "select " + dic["filter"] + " as ccode," + dic["codemainvalue"] + " as cname  from " + dic["codemaintable"] + " where " + dic["filter"] + " ='" + spr + "'";
            DataTable dt = server.GetDataSet(sql).Tables[0];
            spr += "[" + Convert.ToString(dt.Rows[0]["cname"]) + "]";
             */
        }
        sptype += "[" + dic["typename"] + "]";
    }
}
