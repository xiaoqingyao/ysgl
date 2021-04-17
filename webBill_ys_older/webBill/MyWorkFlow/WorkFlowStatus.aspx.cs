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
            ddlShenPiRen.DataSource = server.GetDataTable("select distinct checkuser from workflowrecord m inner join workflowrecords s on m.recordid=s.recordid where m.rdstate=1 and s.rdstate='1'", null);
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


        string sql = @"select checkuser,item.rdstate,count(*) as sl,(select billname from billtoworkflow where flowid=main.flowid) as billname,main.flowid from workflowrecord main,workflowrecords item
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

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            this.GridView1.Rows[i].CssClass = "";
        }
        this.GridView1.SelectedRow.CssClass = "highlight";
        string user = this.GridView1.SelectedRow.Cells[1].Text.ToString();
        string flowid = this.GridView1.SelectedRow.Cells[3].Text.ToString();
     
        string appendsql = "  and main.flowId='"+ flowid + "' and item.checkuser='"+user+"' ";
        string sql = @"select billName,billUser,sum(billje) je
	                ,(select billname from billtoworkflow where flowid=m.flowid) as billtype
	                ,(select deptname from bill_departments where deptcode=m.billdept) as billdept
				                from bill_main m where m.stepid='-1'
					                   and (
							                m.billCode in (
								                select main.billcode from workflowrecord main,workflowrecords item 
									                where main.recordid=item.recordid 
											                and item.rdstate='1' 
											                {0}
							                )
							                or
							                m.billname in (
								                select main.billcode from workflowrecord main,workflowrecords item 
									                where main.recordid=item.recordid 
											                and item.rdstate='1' 
											                {0}
							                )
						                )
                group by billName,billUser,billDept,flowID";
        sql = string.Format(sql, appendsql);
        var dt = server.GetDataTable(sql, null);
        this.GridView2.DataSource = dt;
        this.GridView2.DataBind();
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Attributes["style"] = "cursor:hand";
                #region   
                //点击行触发SelectedIndexChanged事件
                PostBackOptions myPostBackOptions = new PostBackOptions(this);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.PerformValidation = false;
                myPostBackOptions.RequiresJavaScriptProtocol = true; //加入javascript:头
                String evt = Page.ClientScript.GetPostBackClientHyperlink(sender as GridView, "Select$" + e.Row.RowIndex.ToString());
                e.Row.Attributes.Add("onclick", evt);
                #endregion
                break;
        }
    }
}