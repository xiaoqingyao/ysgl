using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class webBill_cwgl_RollYsgcList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                BindDDL();
                this.BindDataGrid();
            }
        }
    }

    private void BindDDL() 
    {
        string sql = "select distinct nian from bill_ysgc order by nian desc";
        DataSet dt = server.GetDataSet(sql);
        if (dt.Tables[0].Rows.Count>0)
        {
            ddl_nd.DataSource = dt;
            ddl_nd.DataTextField = "nian";
            ddl_nd.DataValueField = "nian";
            ddl_nd.DataBind();
        }
    }
    public void BindDataGrid()
    {
        string sql = "select (case ysType when '0' then '年预算' when '1' then '季度预算' when '2' then '月预算' end) as ysType,nian,(case ysType when '0' then '' when '1' then '第'+yue+'季度' when '2' then yue+'月' end) as yue,gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status ,isnull((case (select top 1 isnull(hsdo,'0' )from ysjz_temptb where frmysgc=bill_ysgc.gcbh )when  '0' then '未结转' when '1' then '已结转' end ) ,'未结转')as Isjz from bill_ysgc where ysType='2'  ";
        if (!string.IsNullOrEmpty(ddl_nd.SelectedValue))
        {
            sql += " and nian='"+Convert.ToString(ddl_nd.SelectedValue)+"' ";
        }
        sql += " and right(gcbh,2)!='17' ";
        sql += " ORDER BY LEFT(gcbh, 4) DESC, RIGHT(gcbh, 4)";
        

        DataSet temp = server.GetDataSet(sql);      
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }


    protected void btn_js_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        string Isjz="";
        string status = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                status = this.myGrid.Items[i].Cells[7].Text.ToString().Trim();
                Isjz = this.myGrid.Items[i].Cells[12].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个预算过程！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待结转的预算过程！');", true);
        }
        else if (Isjz == "已结转")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该预算过程已结转！');", true);
        }
        else
        {

            string nian = ddl_nd.SelectedValue;
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='RollYsjzList.aspx?nian="+nian+"&ysgc="+billGuid+"'", true);
           
        }
    }

    protected void ddl_nd_selectedIndexChanged(object sendeer, EventArgs e) {
        BindDataGrid();
    }
}
