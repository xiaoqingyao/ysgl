using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Data.OleDb;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Drawing;

public partial class webBill_ysglnew_ShouRu : System.Web.UI.Page
{
    sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindYear();
            bindDept();
            bindGridView();
        }
    }
    /// <summary>
    /// 年度变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGridView();
    }

    /// <summary>
    /// 部门变化
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindGridView();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Onclick(object sender, EventArgs e)
    {
        string stryear = this.ddlNd.SelectedValue;
        if (string.IsNullOrEmpty(stryear))
        {
            showMessage("请先选择年份，如果没有数据请先开启年预算过程。");
            return;
        }
        string strmonth = this.ddlMonths.SelectedValue;
        string strdept = this.ddlDepartment.SelectedValue;
        string strysgc = new YsManager().GetYsgcCode(DateTime.Parse(stryear + strmonth + "01"));
        int irows = GridView1.Rows.Count;
        if (irows <= 0)
        {
            showMessage("没有要保存的记录。");
        }

        //制单日期
        string strdate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

        //单据唯一键
        string strbillcode = new GuidHelper().getNewGuid();
        List<string> lstsql = new List<string>();
        for (int i = 0; i < irows; i++)
        {
            string strsql = "";
            GridViewRow gvr = GridView1.Rows[i];
            double dbje = 0;
            TextBox txtje = gvr.Cells[4].FindControl("txtje") as TextBox;
            if (txtje == null || txtje.Text.Equals("") || !double.TryParse(txtje.Text.ToString(), out dbje))
            {
                continue;
            }
            //制单部门
            string strdeptcode = gvr.Cells[0].Text.Trim();
            string strzdrcode = sqlHelper.GetCellValue("select top 1  usercode from bill_users where userDept='" + strdeptcode + "'");
            ///制单人;
            string strzdrname = sqlHelper.GetCellValue("select top 1  userName from bill_users where userDept='" + strdeptcode + "'");
            //报销摘要
            string strbxzy = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + gvr.Cells[1].Text.Trim() + "收入录入";
            string stryskmcode = gvr.Cells[2].Text.Trim();//费用科目编号

            strsql = @"insert into lsbxd_main(billcode,flowid,billUser,billDate,billDept,je,se,isgk,gkdept,bxzy,bxsm,fykmcode,sydept,bxlx)
                                           values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')
                                   ";
            strsql = string.Format(strsql, strbillcode, "sr", strzdrcode, strdate, strdeptcode, dbje, "0", "0", strdeptcode, strbxzy, "", stryskmcode, strdeptcode, "04");
            lstsql.Add(strsql);
        }
        if (lstsql.Count > 0)
        {
            int irels = sqlHelper.ExecuteNonQuerysArray(lstsql);
            if (irels >= 1)
            {
                string strbillname = sqlHelper.GetCellValue("exec pro_makebxd '" + strbillcode + "','sr'");
                //if (!string.IsNullOrEmpty(strbillname))
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "a", "openDetail('" + strbillcode + "');", true);
                //}
                ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('收入单生成成功，单号为：" + strbillname + "');", true);
            }
        }
    }

    /// <summary>
    /// 绑定gridview
    /// </summary>
    private void bindGridView()
    {
        string strdept = this.ddlDepartment.SelectedValue;
        if (string.IsNullOrEmpty(strdept))
        {
            showMessage("没有需要填报的部门。");
            return;
        }
        string stryear = this.ddlNd.SelectedValue;
        if (string.IsNullOrEmpty(stryear))
        {
            showMessage("请先选择年份，如果没有数据请先开启年预算过程。");
            return;
        }
        string strsql = "select '' as je,(select deptname from bill_departments where deptcode=yskmdept.deptcode) as deptname, deptcode,(select yskmmc from bill_yskm where yskmcode =yskmdept.yskmcode) as yskmname,yskmcode from bill_yskm_dept yskmdept where yskmcode in (select yskmcode from bill_yskm  where isnull(zjhs,'0')='1' ) and  deptcode=@deptcode";
        //yskmcode in (select yskmcode from bill_ys_benefits_yskm where procode in (select procode from dbo.bill_ys_benefitpro where calculatype='加' and fillintype='明细汇总' and status='1' and annual=@nd))
        SqlParameter[] arrsp = { new SqlParameter("@nd", stryear), new SqlParameter("@deptcode", strdept) };
        DataTable dtRel = sqlHelper.GetDataTable(strsql, arrsp);
        this.GridView1.DataSource = dtRel;
        this.GridView1.DataBind();
        this.lblMsg.Text = "";
    }

    /// <summary>
    /// 绑定年份
    /// </summary>
    private void bindYear()
    {
        string strbindndsql = "select distinct  nian from  dbo.bill_ysgc order by nian desc";
        DataTable dtNd = sqlHelper.GetDataTable(strbindndsql, null);
        if (dtNd != null)
        {
            this.ddlNd.DataSource = dtNd;
            this.ddlNd.DataTextField = "nian";
            this.ddlNd.DataValueField = "nian";
            this.ddlNd.DataBind();
        }
        this.ddlMonths.SelectedValue = DateTime.Now.Month.ToString();
    }

    /// <summary>
    /// 绑定部门
    /// </summary>
    private void bindDept()
    {
        string strsql = " select deptcode,('['+deptcode+']'+deptname) as deptname from bill_departments";
        DataTable dtDept = sqlHelper.GetDataTable(strsql, null);
        this.ddlDepartment.DataSource = dtDept;
        this.ddlDepartment.DataTextField = "deptname";
        this.ddlDepartment.DataValueField = "deptcode";
        this.ddlDepartment.DataBind();
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    private void showMessage(string strMsg)
    {
        string strScript = "alert('" + strMsg + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }

    /// <summary>
    /// 导出excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnExcelOut_Onclick(object sender, EventArgs e)
    {
        string strdeptname = "";
        if (GridView1.Rows.Count > 0)
        {
            strdeptname = GridView1.Rows[0].Cells[1].Text.ToString();
        }
        string filename = this.ddlNd.SelectedValue + "年" + this.ddlMonths.SelectedItem.Text + "月份" + strdeptname + "收入情况";
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([部门编号] VarChar,[部门名称] VarChar,[科目编号] VarChar,[科目名称] VarChar,[实际发生额] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@deptcode,@deptname,@yskmcode,@yskmname,@je)", con))
                {
                    string strdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
                    string stryskmcode = this.GridView1.Rows[i].Cells[2].Text.Trim();
                    string stryskmname = this.GridView1.Rows[i].Cells[3].Text.Trim();

                    cmd.Parameters.AddWithValue("@deptcode", strdeptcode);
                    cmd.Parameters.AddWithValue("@deptname", strdeptname);
                    cmd.Parameters.AddWithValue("@yskmcode", stryskmcode);
                    cmd.Parameters.AddWithValue("@yskmname", stryskmname);
                    cmd.Parameters.AddWithValue("@je", "");
                    cmd.ExecuteNonQuery();
                }
            }
        }
        Response.Charset = "UTF-8";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.ContentType = "application/ms-excel;charset=UTF-8";

        Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode(filename) + ".xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }

    /// <summary>
    /// 导入excel模板
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_reload_Click(object sender, EventArgs e)
    {
        DataTable dtrel = new DataTable();
        object objdt = Session["nowshouru"];
        if (objdt == null)
        {
            return;
        }
        dtrel = objdt as DataTable;
        if (dtrel == null || dtrel.Rows.Count <= 0)
        {
            return;
        }
        if (this.GridView1.Rows.Count <= 0)
        {
            return;
        }
        string strgridviewdeptcode = this.GridView1.Rows[0].Cells[0].Text.Trim();
        if (dtrel.Rows[0]["deptcode"] == strgridviewdeptcode || "'" + dtrel.Rows[0]["deptcode"] == strgridviewdeptcode)
        {
            showMessage("导入excel文件与当前部门不一致。"); return;
        }
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
        Session.Remove("nowshouru");
    }

    double deTotalAmount = 0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {

        }

        if (e == null)
        {
            return;
        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            deTotalAmount = 0;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strje = e.Row.Cells[5].Text.Trim();
            double dbje = 0;
            TextBox txtje = e.Row.Cells[4].FindControl("txtje") as TextBox;
            if (double.TryParse(strje, out dbje) && txtje != null)
            {
                txtje.Text = dbje.ToString("N02");
                deTotalAmount += dbje;
            }
            int sonCount = Convert.ToInt32(sqlHelper.GetCellValue(" select count(*) from bill_yskm where yskmcode like '" + e.Row.Cells[2].Text.Trim() + "%'"));
            if (sonCount > 1)
            {
                e.Row.BackColor = Color.Silver;
                txtje.Text = "";
                txtje.Enabled = false;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[4].Text = deTotalAmount.ToString("N02");
            e.Row.Cells[0].Style.Add("text-align", "right");
            e.Row.Cells[4].Style.Add("text-align", "right");

            e.Row.Cells[1].Text = "";
            e.Row.Cells[2].Text = "";
            e.Row.Cells[3].Text = "";

        }
    }
}
