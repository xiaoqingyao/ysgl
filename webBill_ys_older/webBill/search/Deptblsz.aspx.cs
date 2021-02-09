using Bll.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_search_Deptblsz : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strND;
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
                //hf_dept.Value = Request.QueryString["deptCode"].ToString().Trim();
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["deptCode"]).Trim()))
                {
                    this.Label1.Text = "当前部门：[" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "]" + server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                }
                DrpSelectBid();
                this.BindDataGrid();


                strND = drpSelectNd.SelectedValue;
            }
        }
    }
    private void DrpSelectBid()
    {
        string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";

        //string selectndsql = "select distinct annual from bill_ys_benefitpro order by annual desc ";
        DataTable selectdt = server.GetDataTable(selectndsql, null);
        drpSelectNd.DataSource = selectdt;
        drpSelectNd.DataTextField = "xmmc";
        drpSelectNd.DataValueField = "nian";
        drpSelectNd.DataBind();
        //for (int i = 0; i < selectdt.Rows.Count; i++)
        //{
        //    drpSelectNd.Items.Add(new ListItem(selectdt.Rows[i]["annual"].ToString(), selectdt.Rows[i]["annual"].ToString()));
        //}
        //if (selectdt.Rows.Count > 0)
        //{
        //    drpSelectNd.SelectedValue = selectdt.Rows[0]["annual"].ToString();
        //}
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    public void BindDataGrid()
    {
        // DrpSelectBid();
        ////获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        //int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        ////ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        //int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 110);
        ////获取pagesize 每页的高度
        //int ipagesize = arrpage[2];
        ////总的符合条件的记录数
        //int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>


        IList<Bill_Departments> list;


        string deptcode = Request.QueryString["deptCode"].ToString().Trim();
        if (string.IsNullOrEmpty(deptcode))
        {
            SysManager sysMgr = new SysManager();
            list = sysMgr.GetAllDept();//(arrpage[0], arrpage[1], out icount);
        }
        else
        {
            //修改是否包含下级 Edit by Lvcc
            DepartmentManager deptMgr = new DepartmentManager(deptcode);
            ////if (this.chkNextLevel.Checked)
            ////{
            list = deptMgr.GetAllChild2();//(arrpage[0], arrpage[1], out icount);
            ////}
            ////else
            ////{
            //    //不包含下级
            //    list = deptMgr.GetListWithOutChild(deptcode);//;(deptcode, arrpage[0], arrpage[1], out icount);
            //}

        }
        this.GridView1.DataSource = list;
        this.GridView1.DataBind();
    }
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    private void showMessage(string strMsg)
    {
        string strScript = "alert('" + strMsg + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strnd = this.drpSelectNd.SelectedValue.Trim();
        if (strnd.Equals(""))
        {
            showMessage("请先选择年度。"); return;
        }
        else if (string.IsNullOrEmpty(Request.QueryString["deptCode"].ToString()))
        {
            showMessage("请先选择部门。"); return;
        }

        else { }

        List<string> lstSql = new List<string>();

        int iGridviewRowCount = this.GridView1.Rows.Count;
        for (int i = 0; i < iGridviewRowCount; i++)
        {
            string strdeptcode = GridView1.Rows[i].Cells[0].Text;
            TextBox tbBili = this.GridView1.Rows[i].Cells[2].FindControl("bl") as TextBox;
            Label lblzje = this.GridView1.Rows[i].Cells[3].FindControl("lbl_je") as Label;
            TextBox tbfjje = this.GridView1.Rows[i].Cells[4].FindControl("blje") as TextBox;
            lstSql.Add("delete from bill_deptFyblDy where cdefine1='" + strnd + "' and deptCode='" + strdeptcode + "'");

            if (tbBili == null)
            {
                continue;
            }
            string strBili = tbBili.Text.Trim();
            decimal decbl = decimal.Parse(strBili);
            if (lblzje == null)
            {
                continue;
            }
            string strzje = lblzje.Text.Trim();
            decimal deczje = decimal.Parse(strzje);
            if (tbfjje == null)
            {
                continue;
            }
            string fjje = tbfjje.Text.Trim();
            decimal decfjje = deczje * decbl;


            bool bohasbaifen = false;
            if (strBili.IndexOf("%") != -1)
            {
                strBili = strBili.Replace("%", "");
                bohasbaifen = true;
            }
            if (strBili.Equals(""))
            {
                strBili = "0";
            }
            double bdBili = 0;
            bool bo = double.TryParse(strBili, out bdBili);
            if (!bo)
            {
                showMessage("第" + i + "行输入格式不正确。");
                break;
            }
            //分解到的部门编号
            string strfjdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            string strdeptname = this.GridView1.Rows[i].Cells[1].Text.Trim();
            strfjdeptcode = strfjdeptcode.Replace("&nbsp;", "");
            if (strfjdeptcode.Equals(""))
            {
                continue;
            }
            if (bohasbaifen)
            {
                bdBili = bdBili / 100;
            }
            string straddsql = "insert into bill_deptFyblDy(deptCode,deptName,cdefine1,fjbl,ddefine7) values('" + strdeptcode + "','" + strdeptname + "','" + strnd + "','" + bdBili.ToString("0.00000000") + "','" + decfjje.ToString("0.00") + "')";
            lstSql.Add(straddsql);
        }
        if (lstSql.Count > 0)
        {
            try
            {
                int irel = server.ExecuteNonQuerysArray(lstSql);
                if (irel > -1)
                {
                    lblmsg.Text = "保存成功。";
                }
                else { throw new Exception("未知原因。"); }
            }
            catch (Exception ex)
            {
                showMessage("保存失败，原因：" + ex.Message);
            }



        }
    }
    public string getzje()
    {
        string strnd = drpSelectNd.SelectedValue;
        string strzje = "";
        if (!string.IsNullOrEmpty(strnd))
        {
            string strsql = " select zje from  bill_Srmb where nd='" + strnd + "' and note0='fy'";
            strzje = server.GetCellValue(strsql);

        }
        return strzje;
    }

    decimal deTotalBili = 0;
    decimal detotalje = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string strzje = getzje();
        string strnd = drpSelectNd.SelectedValue;
        if (string.IsNullOrEmpty(strzje))
        {
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[4].Visible = false;
        }


        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {

            string strdeptcode = e.Row.Cells[0].Text;
            string strsql = "select * from bill_deptFyblDy where deptCode='" + strdeptcode + "' and cdefine1='" + strnd + "'";
            DataTable bldt = server.GetDataTable(strsql, null);

            TextBox txtbl = e.Row.Cells[2].FindControl("bl") as TextBox;
            Label lblzje = e.Row.Cells[3].FindControl("lbl_je") as Label;
            TextBox blje = e.Row.Cells[4].FindControl("blje") as TextBox;
            //string txtje = txtbl.Text;
            if (bldt != null && bldt.Rows.Count != 0)
            {
                txtbl.Text = bldt.Rows[0]["fjbl"].ToString();
                blje.Text = bldt.Rows[0]["ddefine7"].ToString();

                if (!string.IsNullOrEmpty(bldt.Rows[0]["ddefine7"].ToString()))
                {
                    detotalje += decimal.Parse(bldt.Rows[0]["ddefine7"].ToString());
                }
            }
            lblzje.Text = strzje;


            //deTotalBili += decimal.Parse(txtbl.Text);

            //加合计行
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = detotalje.ToString();
            e.Row.Cells[0].Text = "合计";
        }

    }
    /// <summary>
    /// 复制上年
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_fzsn_Click(object sender, EventArgs e)
    {
        string strnd = drpSelectNd.SelectedValue;
        List<string> list = new List<string>();
        int intsn = Convert.ToInt32(strnd) - 1;
        string strsql = @" select * from bill_deptFyblDy where cdefine1='" + intsn.ToString() + "'";
        DataTable dt = server.GetDataTable(strsql, null);
        if (dt != null && dt.Rows.Count > 0)
        {


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string strdeptcode = dt.Rows[i]["deptCode"].ToString();
                string strdeptname = dt.Rows[i]["deptName"].ToString();
                string strbl = dt.Rows[i]["fjbl"].ToString();
                string strfjje = dt.Rows[i]["ddefine7"].ToString();
                list.Add(" delete from bill_deptFyblDy where cdefine1='" + strnd + "' and deptCode='" + strdeptcode + "'");
                list.Add(" insert into bill_deptFyblDy(deptCode,deptName,cdefine1,fjbl,ddefine7) values('" + strdeptcode + "','" + strdeptname + "','" + strnd + "','" + strbl + "','" + strfjje + "')");
            }

        }
        if (list.Count > 0)
        {
            try
            {
                int irel = server.ExecuteNonQuerysArray(list);
                if (irel > -1)
                {
                    lblmsg.Text = "保存成功。";
                    BindDataGrid();
                }
                else { throw new Exception("未知原因。"); }
            }
            catch (Exception ex)
            {
                showMessage("保存失败，原因：" + ex.Message);
            }
        }
    }
    /// <summary>
    /// 导出excel表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_excl_Click(object sender, EventArgs e)
    {
        string deptcode = Request.QueryString["deptCode"].ToString();
        string strnd = drpSelectNd.SelectedValue;
        DataTable dt = new DataTable();
        string strsql = @"  select dept.deptCode as bmbh, dept.deptName as bmmc,fjbl,ddefine7,(select zje from  bill_Srmb where nd='"+strnd+"' and note0='fy') as zje,* from bill_deptFyblDy bl,bill_departments dept  where  bl.deptCode=dept.deptCode and (sjDeptCode='"+deptcode+"' or dept.deptCode='"+deptcode+"') ";
        dt = server.GetDataTable(strsql, null);
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //使用OleDb连接  
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand(@"CREATE TABLE Sheet1 ([部门编号] VarChar,[部门名称] VarChar,[分解比例] VarChar,[总金额] VarChar,[分解金额] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@deptcode, @deptname,@fjbl,@zje,@fjje)", con);
                cmd.Parameters.AddWithValue("@deptcode", dt.Rows[i]["bmbh"].ToString());
                cmd.Parameters.AddWithValue("@deptname", dt.Rows[i]["bmmc"].ToString());
                cmd.Parameters.AddWithValue("@fjbl", dt.Rows[i]["fjbl"].ToString());
                cmd.Parameters.AddWithValue("@zje", dt.Rows[i]["zje"].ToString());
                cmd.Parameters.AddWithValue("@fjje", dt.Rows[i]["ddefine7"].ToString());
                cmd.ExecuteNonQuery();
            }

        }
        Response.ContentType = "application/ms-excel";
        Response.AppendHeader("Content-Disposition", "attachment;filename=info.xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }
}