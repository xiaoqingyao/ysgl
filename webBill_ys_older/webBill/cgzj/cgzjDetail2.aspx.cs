using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class webBill_cgzj_cgzjDetail2 : System.Web.UI.Page
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
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_cgzj_cgzjDetail2));

            if (!IsPostBack)
            {
                DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='04' order by dicCode");

                this.ddl_cglb.DataTextField = "dicName";
                this.ddl_cglb.DataValueField = "dicCode";
                this.ddl_cglb.DataSource = temp;
                this.ddl_cglb.DataBind();

                this.bindData();
                //绑定选择单位
                PaginationToGV1.PageIndex = "1";

                string rowsql2 = @"select count(*) from jk_zqxs.dbo.info_dy_ven";

                string row2 = Convert.ToString(server.ExecuteScalar(rowsql2));

                if (row2 == "0")
                {
                    PaginationToGV1.RowsCount = "1";
                }
                else
                {
                    PaginationToGV1.RowsCount = row2;
                }

                this.BindDGgys();
            }
        }
    }

    private void bindData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            //this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
            this.ddl_cglb.SelectedIndex = 0;
            this.lblDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            this.lblDeptShow.Text = (new billCoding()).getDeptLevel2Name(server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'"));


            DateTime dt = System.DateTime.Now;
            this.txtCgrq.Value = dt.ToShortDateString();
            //this.planuser.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");

            this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();

            this.CreateCgspCode();

        }
        else
        {
            this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();

            this.CreateCgspCode();
        }
        //明细
        DataSet temp1 = server.GetDataSet("select * from bill_cgzjjh where cgbh='" + this.lbl_BillCode.Text.ToString().Trim() + "' order by jhindex");

        if (temp1.Tables[0].Rows.Count < 1)
        {
            DataRow addRow = temp1.Tables[0].NewRow();
            addRow["gysbh"] = "";
            addRow["gysmc"] = "";
            addRow["syrkje"] = 0.00;
            addRow["byjhje"] = 0;
            addRow["byfkje"] = 0;
            addRow["bz"] = "";


            temp1.Tables[0].Rows.Add(addRow);
            temp1.Tables[0].AcceptChanges();
            this.GridView1.DataSource = temp1;
            this.GridView1.DataBind();
        }
        else
        {
            this.GridView1.DataSource = temp1;
            this.GridView1.DataBind();
        }
    }

    //绑定选择单位
    protected void BindDGgys()
    {
        //string sql = "select bh,mc from jk_zqxs.dbo.info_dy_ven order by bh";

        //DataSet temp = server.GetDataSet(sql);
        string strpage = PaginationToGV1.PageIndex;
        int pageIndex = 1;
        if (!string.IsNullOrEmpty(strpage))
        {
            pageIndex = Convert.ToInt32(PaginationToGV1.PageIndex);
        }

        int begRow = (pageIndex - 1) * 17;
        int endRow = pageIndex * 17;

        string sql = @"select * from (select bh,mc,Row_number()over(order by bh) as crow from jk_zqxs.dbo.info_dy_ven) t where t.crow>" + Convert.ToString(begRow) + " and t.crow <=" + Convert.ToString(endRow);

        DataSet temp = server.GetDataSet(sql);

        this.DGgys.DataSource = temp;
        this.DGgys.DataBind();
    }

    public void CreateCgspCode()
    {
        string cgspCode = (new billCoding()).getCgspCode();
        if (cgspCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_bc.Visible = false;
        }
        else
        {
            this.lblCgbh.Text = cgspCode;
        }
    }
    
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dr in GridView1.Rows)
        {
            string gysbh = ((TextBox)dr.FindControl("gysbh")).Text;
            string gysmc = ((TextBox)dr.FindControl("gysmc")).Text;
            decimal syrkje = Convert.ToDecimal(((TextBox)dr.FindControl("syrkje")).Text);
            decimal byjhje = Convert.ToDecimal(((TextBox)dr.FindControl("byjhje")).Text);
            decimal byfkje = Convert.ToDecimal(((TextBox)dr.FindControl("byfkje")).Text);
            string bh = ((TextBox)dr.FindControl("bz")).Text;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dr in GridView1.Rows)
        {
            if (((CheckBox)dr.FindControl("CheckBox1")).Checked)
            {
                GridView1.DeleteRow(dr.RowIndex);
            }
        }
    }

    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        this.BindDGgys();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string alldwbh = this.TextBox4.Text.ToString().Trim();
        string lsdw="";
        this.TextBox4.Text = "";
        DataTable dt = server.RunQueryCmdToTable("select * from bill_cgzjjh where cgbh='" + this.lbl_BillCode.Text.ToString().Trim() + "' order by jhindex");
        
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            GridViewRow gRow = this.GridView1.Rows[i];
            string dwbh = ((TextBox)gRow.FindControl("gysbh")).Text;
            if (dwbh!="")
            {
                DataRow newRow = dt.NewRow();
                lsdw = lsdw + "," + dwbh;
                newRow["gysbh"] = ((TextBox)gRow.FindControl("gysbh")).Text;
                newRow["gysmc"] = ((TextBox)gRow.FindControl("gysmc")).Text;
                newRow["syrkje"] = ((TextBox)gRow.FindControl("syrkje")).Text;
                newRow["byjhje"] = ((TextBox)gRow.FindControl("byjhje")).Text;
                newRow["byfkje"] = ((TextBox)gRow.FindControl("byfkje")).Text;
                newRow["bz"] = ((TextBox)gRow.FindControl("bz")).Text;

                dt.Rows.Add(newRow);
            }
        }

        DataTable dtdw = server.RunQueryCmdToTable("select bh,mc,100 as je from jk_zqxs.dbo.info_dy_ven where bh in ("+alldwbh+") order by bh ");

        DataRow addRow;
        for (int i = 0; i < dtdw.Rows.Count; i++)
        {
            addRow = dt.NewRow();
            string dwbh = dtdw.Rows[i]["bh"].ToString().Trim();
            if (lsdw.IndexOf(dwbh) < 1)
            {
                addRow["gysbh"] = dtdw.Rows[i]["bh"];
                addRow["gysmc"] = dtdw.Rows[i]["mc"];
                addRow["syrkje"] = dtdw.Rows[i]["je"];
                addRow["byjhje"] = 0;
                addRow["byfkje"] = 0;
                addRow["bz"] = "";


                dt.Rows.Add(addRow);
                dt.AcceptChanges();
            }
        }
       
        //GridView重新绑定数据源
        GridView1.DataSource = dt;
        GridView1.DataBind();

    }





}

