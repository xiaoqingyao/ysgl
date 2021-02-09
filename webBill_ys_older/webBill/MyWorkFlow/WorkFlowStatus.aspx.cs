using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_MyWorkFlow_WorkFlowStatus : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlShenPiRen.DataSource = server.GetDataTable("select distinct checkuser from workflowrecords where rdstate='1'", null);
            ddlShenPiRen.DataTextField = "checkuser";
            ddlShenPiRen.DataValueField = "checkuser";
            ddlShenPiRen.DataBind();
            ddlShenPiRen.Items.Insert(0, new ListItem("--全部--", ""));
            binddata();
        }

    }
    private void binddata()
    {
        this.GridView1.DataSource = getdt();
        this.GridView1.DataBind();
    }

    public DataTable getdt()
    {
        string usercode = this.ddlShenPiRen.SelectedValue;


        string sql = @"select checkuser,item.rdstate,count(*) as sl,(select billname from billtoworkflow where flowid=main.flowid) as billname from workflowrecord main,workflowrecords item
		where main.recordid=item.recordid and  item.rdstate='1' {0}
        and (billcode in (select billcode from bill_main where stepid='-1' ) or billcode in (select billname from bill_main where stepid='-1' ) )
		group by checkuser,item.rdstate,main.flowid
		order by main.flowid,checkuser";
        string sqlappend = "";
        if (usercode != "")
        {
            sqlappend += " and checkuser='" + usercode + "'";
        }
        sql = string.Format(sql, sqlappend);
        return server.GetDataTable(sql, null);
    }
    protected void ddlShenPiRen_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddata();
    }
    protected void btn_excle_Click(object sender, EventArgs e)
    {

        DataTable dt = getdt();
      
        new ExcelHelper().ExpExcel(dt, GridView1, "审批状态");
    }
}