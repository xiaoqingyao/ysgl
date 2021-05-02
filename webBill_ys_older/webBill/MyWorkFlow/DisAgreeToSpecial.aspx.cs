using Dal.Bills;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_MyWorkFlow_DisAgreeToSpecial : System.Web.UI.Page
{
    string billCode = "";
    string flowid = "";
    string billType = "";
    string mind = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        billCode = Request["billCode"];
        //  flowid = Request["flowid"];
        mind = Request["mind"];
        Bill_Main main = new MainDal().GetMainByCode(billCode);
        if (main != null)
        {
            billType = flowid = main.FlowId;

        }
        else
        {
            flowid = server.GetCellValue("select flowid from bill_main where billcode='" + billCode + "' or billname='" + billCode + "'");
        }

        if (!IsPostBack)
        {
            this.rdb_first.Checked = true;
            ddl_prevLiuCheng.Visible = false;
            string sql = @"select  substring(ltrim(steptext),2,len(ltrim(steptext))-1)+':'+isnull((select '['+usercode+']'+username from bill_users where usercode=ws.checkuser),checkuser) as txt,
                            ws.stepid as code   from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid
                             and (billCode='" + billCode + "' or billcode in ( select billname from bill_main where billCode='" + billCode + "') )  and w.flowId='" + flowid + "' and ws.rdstate>1";


            DataTable dt = server.GetDataTable(sql, null);
            ddl_prevLiuCheng.DataSource = dt;
            ddl_prevLiuCheng.DataTextField = "txt";
            ddl_prevLiuCheng.DataValueField = "code";
            ddl_prevLiuCheng.DataBind();
            if (dt.Rows.Count == 0)
            {
                rdb_special.Visible = false;
            }
            if (!string.IsNullOrEmpty(mind))
            {
                txt_mind.Text = mind;
            }
        }
    }

    protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_first.Checked)
        {
            ddl_prevLiuCheng.Visible = false;
            return;
        }

        if (rdb_special.Checked)
        {
            ddl_prevLiuCheng.Visible = true;
        }
    }


    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (rdb_first.Checked || !string.IsNullOrEmpty(ddl_prevLiuCheng.SelectedValue))
        {
            string recordid = server.GetCellValue("select recordid from workflowrecord where  (billCode='" + billCode + "'  or billCode in( select billname from bill_main where billCode='" + billCode + "')) and flowid='" + flowid + "'");
            WorkFlowRecordManager mgr = new WorkFlowLibrary.WorkFlowBll.WorkFlowRecordManager();
            string usercode = Convert.ToString(Session["userCode"]);
            string mind = txt_mind.Text;//System.Web.HttpUtility.UrlDecode(Request["mind"]);
            if (rdb_first.Checked)
            {

                //  mgr.DisAgree(billCode, mind, "disagree");
                mgr.DisAgree(billCode, usercode, mind);
            }
            else
            {
                mgr.DisAgreeToSpecial(billCode, mind, usercode, recordid, ddl_prevLiuCheng.SelectedValue);

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "close2", "parent.closeDetail();alert('驳回成功！');", true);
        }

    }


}