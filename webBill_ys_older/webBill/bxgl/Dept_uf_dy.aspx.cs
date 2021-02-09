using Bll.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_Dept_uf_dy : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                bindZhangTao();
            }
        }
    }
    public void BindDataGrid()
    {


        IList<Bill_Departments> list;
        SysManager sysMgr = new SysManager();
        list = sysMgr.GetAllDept();
        //----------给gridview赋值
        this.myGrid.DataSource = list;
        this.myGrid.DataBind();


    }

    #region 查询
    //protected void btn_sele_Click(object sender, EventArgs e)
    //{
    //    BindDataGrid();
    //}
    protected void OnddlZhangTao_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    #endregion
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    private void bindZhangTao()
    {

    

        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");

        string strselectsql = @"select dsname as db_data,iYear,
                        cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                        * from [{0}].UFTSystem.dbo.EAP_Account where iYear>='2014' order by iYear desc";
        strselectsql = string.Format(strselectsql, strlinkdbname);
     
        this.ddlZhangTao.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZhangTao.DataTextField = "companyname";
        this.ddlZhangTao.DataValueField = "db_data";
        this.ddlZhangTao.DataBind();
        ddlZhangTao.Items.Insert(0, (new ListItem("-选择帐套-", ""))); 
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string ysdeptcode = e.Item.Cells[0].Text;
            string ufcode = server.GetCellValue("select ufdeptcode from bill_ys_uf_dept where ufdata='" + ddlZhangTao.SelectedValue + "' and ysdeptcode='" + ysdeptcode + "'");
            if (!string.IsNullOrEmpty(ufcode))
            {
                TextBox txtufcode = e.Item.Cells[2].FindControl("txt_ufdeptcode") as TextBox;
                txtufcode.Text = ufcode;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        string strufdata = ddlZhangTao.SelectedValue;

        //string strdeptcode = ddl_dept.SelectedValue;
        if (string.IsNullOrEmpty(strufdata))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择帐套');", true);
            return;
        }
        IList<string> sqlList = new List<string>();
        List<string> tempList = new List<string>();


        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string yscode = myGrid.Items[i].Cells[0].Text.Trim();
            string ysname = myGrid.Items[i].Cells[1].Text.Trim();

            TextBox tb = myGrid.Items[i].Cells[2].FindControl("txt_ufdeptcode") as TextBox;
            string strufcode = tb.Text.Trim();


            sqlList.Add(@" delete bill_ys_uf_dept  where ysdeptcode='" + yscode + "' and  ufdata='" + strufdata + "'");// 

            sqlList.Add(@" insert into bill_ys_uf_dept(ysdeptcode,ysdeptname,ufdeptcode,ufdeptname,ufdata) values('" + yscode + "','" + ysname + "','" + strufcode + "','','" + strufdata + "')");// 

        }
        int row = server.ExecuteNonQuerysArray(sqlList.ToList());

        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功');", true);
            bindZhangTao();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败');", true);
            return;
        }
    }
}