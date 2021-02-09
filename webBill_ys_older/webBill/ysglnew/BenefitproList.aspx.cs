using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class webBill_ysglnew_BenefitproList : System.Web.UI.Page
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
                DrpSelectBid();
                this.BindDataGrid();
            }
        }
    }
    private void DrpSelectBid()
    {
        string selectndsql = @" select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
        DataTable selectdt = server.GetDataTable(selectndsql, null);   
        drpSelectNd.DataSource=selectdt;
        drpSelectNd.DataTextField = "xmmc"; 
        drpSelectNd.DataValueField="nian";
        drpSelectNd.DataBind();
        //string selectndsql = "select distinct annual from bill_ys_benefitpro order by annual desc ";
        //DataTable selectdt = server.GetDataTable(selectndsql, null);
        //for (int i = 0; i < selectdt.Rows.Count; i++)
        //{
        //    drpSelectNd.Items.Add(new ListItem(selectdt.Rows[i]["annual"].ToString(), selectdt.Rows[i]["annual"].ToString()));
        //}
        //if (selectdt.Rows.Count > 0)
        //{
        //    drpSelectNd.SelectedValue = selectdt.Rows[0]["annual"].ToString();
        //}
    }

    public void BindDataGrid()
    {
        string sql = @"select procode,proname,calculatype,fillintype,sortcode,
(case isnull(status,'0') when '1' then '正常' when '0' then '禁用' end) as status,
(select '['+userCode+']'+userName from bill_users where userCode=a.adduser)as
adduser,adddate,
modifyuser,modifydate,annual  from bill_ys_benefitpro a where 1=1 ";
        sql += " and annual='"+drpSelectNd.SelectedValue.ToString().Trim()+"' ";
        if (this.CheckBox2.Checked == true)
        { }
        else
        {
            sql += " and Status='1'";
        }
        if (txb_where.Text != "")
        {
            sql += " and (procode like '%" + txb_where.Text + "%' or proname like '%" + txb_where.Text + "%')";
        }
        DataSet temp = server.GetDataSet(sql);

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }

    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string strCodes = procode.Value.ToString().Trim();

        System.Collections.Generic.List<string> list = new List<string>();

        list.Add("update bill_ys_benefitpro set status='0' where procode='" + strCodes + "'");

        server.ExecuteNonQuerysArray(list);
        this.BindDataGrid();
    }

    #endregion

    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion

    protected void CheckBox2_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void btn_copYear_Click(object sender, EventArgs e)
    {
        string cxsql = @"insert into bill_ys_benefitpro( procode,proname,calculatype,fillintype,sortcode,status,adduser,adddate,annual)
                        select (convert( varchar(10) , (select top 1 annual+1 from bill_ys_benefitpro order by annual desc))  +( convert( varchar(10),right(procode,2)))),
						proname,calculatype,fillintype,sortcode,status,'"+Session["userCode"].ToString().Trim()+@"','"+DateTime.Now.ToString("yyyy-MM-dd")+@"',
						(select top 1 annual+1 from bill_ys_benefitpro order by annual desc) as annual
						from bill_ys_benefitpro
						where annual=(select top 1 annual from bill_ys_benefitpro order by annual desc)";
        if (server.ExecuteNonQuery(cxsql)!= -1 )
        {
            drpSelectNd.Items.Clear();
            DrpSelectBid();
            this.BindDataGrid();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string strCodes = procode.Value.ToString().Trim();

        System.Collections.Generic.List<string> list = new List<string>();

        list.Add("update bill_ys_benefitpro set status='1' where procode='" + strCodes + "'");

        server.ExecuteNonQuerysArray(list);
        this.BindDataGrid();
    }
}