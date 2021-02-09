using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Security;

public partial class webBill_cgzj_cgzjbxDetail : System.Web.UI.Page
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
            Ajax.Utility.RegisterTypeForAjax(typeof(webBill_cgzj_cgzjbxDetail));

            if (!IsPostBack)
            {
                this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
                this.bindData();
                //绑定选择单位
                PaginationToGV1.PageIndex = "1";

                string rowsql2 = @"select count(*) from (
                                   select gysbh from bill_cgzjjh  group by gysbh)t";

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

                string type = Request.Params["type"];
                string billcode = Request.Params["par"];
                if (type == "edit" || type == "look")
                {
                    if (type == "look")
                    {
                        this.btn_bc.Visible = false;
                    }
                    DataSet ds = server.GetDataSet(" select * from bill_cgzjfk where billcode='" + billcode + "'");
                    GridView1.DataSource = ds;
                    GridView1.DataBind();

                    DataSet dsMain = server.GetDataSet(" select * from bill_main where billcode='" + billcode + "'");
                    DataRow dr = dsMain.Tables[0].Rows[0];
                    lblDeptShow.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + Convert.ToString(dr["billDept"]) + "'");

                    lblUser.Text = server.GetCellValue("select '['+usercode+']'+userName  from bill_users where usercode='" + Convert.ToString(dr["billUser"]) + "'");
                    txtCgrq.Text = (Convert.ToDateTime(dr["billdate"])).ToString("yyyy-MM-dd");

                }
                else
                {
                    string userCode = Convert.ToString(Session["userCode"]);
                    string userName = Convert.ToString(Session["userName"]);
                    lblUser.Text = "[" + userCode + "]" + userName;

                    string deptTemp = Convert.ToString(server.ExecuteScalar("select userdept from bill_users where usercode='" + userCode + "'"));
                    string dept = "";
                    if (isTopDept("n", deptTemp))
                    {
                        dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + deptTemp + "'");
                    }
                    else
                    {
                        //上级部门
                        dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode='" + deptTemp + "')");
                    }
                    lblDeptShow.Text = dept;
                }

            }
        }
    }

    private void bindData()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            //this.txtCgrq.Attributes.Add("onfocus", "javascript:setday(this);");
            this.lblDept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            this.lblDeptShow.Text = (new billCoding()).getDeptLevel2Name(server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'"));


            DateTime dt = System.DateTime.Now;
            this.txtCgrq.Text = dt.ToShortDateString();
            //this.planuser.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");

            this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();

            //this.CreateCgspCode();

        }
        else
        {
            this.lbl_BillCode.Text = (new GuidHelper()).getNewGuid();

            //this.CreateCgspCode();
        }
        //明细
        DataSet temp1 = server.GetDataSet("select * from bill_cgzjfk where billcode='" + this.lbl_BillCode.Text.ToString().Trim() + "' order by jhindex");

        if (temp1.Tables[0].Rows.Count < 1)
        {
            DataRow addRow = temp1.Tables[0].NewRow();
            addRow["gysbh"] = "";
            addRow["gysmc"] = "";
            addRow["jhje"] = 0.00;
            addRow["fkje"] = 0;

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
        //string sql = "select bh,mc from zqxserp_jk.dbo.info_dy_ven order by bh";

        //DataSet temp = server.GetDataSet(sql);
        string strpage = PaginationToGV1.PageIndex;
        int pageIndex = 1;
        if (!string.IsNullOrEmpty(strpage))
        {
            pageIndex = Convert.ToInt32(PaginationToGV1.PageIndex);
        }

        int begRow = (pageIndex - 1) * 17;
        int endRow = pageIndex * 17;

        DateTime now = DateTime.Now;
        DateTime monthBeg = Convert.ToDateTime(now.ToString("yyyy-MM-dd").Substring(0, 7) + "-01");
        DateTime monthEnd = monthBeg.AddMonths(1);


        string sql = @" select gysbh,mc,je,Row_number()over(order by gysbh) as crow 
                    from (select gysbh,sum(byfkje) as je from bill_cgzjjh where cgbh in(
                    select billcode from bill_main where flowid='cgzjjh' and convert(varchar(10),billdate,121)>='" + monthBeg.ToString("yyyy-MM-dd") + "' and convert(varchar(10),billdate,121)<'" + monthEnd.ToString("yyyy-MM-dd") + "'" +
                    @") group by gysbh)a 
                    inner join 
                    (select bh,mc from zqxserp_jk.dbo.info_dy_ven) b on a.gysbh=b.bh";

        string bh = TextBox1.Text;
        string name = TextBox2.Text;

        string where = "";

        if (!string.IsNullOrEmpty(bh))
        {
            where += " and gysbh like '%" + bh + "%'";
        }
        if (!string.IsNullOrEmpty(name))
        {
            where += " and gysmc like '%" + name + "%'";
        }
        if (!string.IsNullOrEmpty(where))
        {
            where = " where " + where.Substring(5);
            sql += where;
        }


        sql = " select * from (" + sql + ")t where t.crow>" + Convert.ToString(begRow) + " and t.crow <=" + Convert.ToString(endRow);
        DataSet temp = server.GetDataSet(sql);

        this.DGgys.DataSource = temp;
        this.DGgys.DataBind();
    }
    /*
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
    */
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        string types = Request.Params["type"];
        string billcode = "";
        if (types == "add")
        {
            billcode = (new GuidHelper()).getNewGuid();
        }
        else
        {
            billcode = Request.Params["par"];
        }

        List<string> upStrList = new List<string>();

        string date = this.txtCgrq.Text;
        string userCode = "";
        try
        {

            string dept = "";
            if (types == "add")
            {
                userCode = Convert.ToString(Session["userCode"]);
                string deptTemp = Convert.ToString(server.ExecuteScalar("select userdept from bill_users where usercode='" + userCode + "'"));
                userCode = Convert.ToString(Session["userCode"]);
                if (isTopDept("n", deptTemp))
                {
                    dept = deptTemp;
                }
                else
                {
                    //上级部门
                    dept = server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode='" + deptTemp + "')");
                }
            }
            else
            {
                userCode = lblUser.Text.Split(']')[0].Trim('[');
                dept = lblDeptShow.Text.Split(']')[0].Trim('[');
                string deleteMain = " delete bill_main where billcode='" + billcode + "'";
                string deleteChild = " delete bill_cgzjfk where billcode='" + billcode + "'";
                upStrList.Add(deleteChild);
                upStrList.Add(deleteMain);
            }


            StringBuilder sbMain = new StringBuilder();
            sbMain.Append(" insert into bill_main(billcode,flowid,stepid,billuser,billdate,billdept,looptimes,billje) values( ");
            sbMain.Append("'" + billcode + "',");
            sbMain.Append("'cgzjfk',");
            sbMain.Append("'end',");
            sbMain.Append("'" + userCode + "',");
            sbMain.Append("'" + date + "',");
            sbMain.Append("'" + dept + "',");
            sbMain.Append("0,");

            decimal je = 0;

            foreach (GridViewRow dr in GridView1.Rows)
            {
                string gysbh = ((TextBox)dr.FindControl("gysbh")).Text;
                string gysmc = ((TextBox)dr.FindControl("gysmc")).Text;
                string fkje = ((TextBox)dr.FindControl("fkje")).Text;
                string jhje = ((TextBox)dr.FindControl("jhje")).Text;
                string bz = ((TextBox)dr.FindControl("bz")).Text;
                StringBuilder upstr = new StringBuilder();
                upstr.Append(" insert into bill_cgzjfk(billcode,gysbh,gysmc,fkje,jhje,bz) values(");
                upstr.Append("'" + billcode + "',");
                upstr.Append("'" + gysbh + "',");
                upstr.Append("'" + gysmc + "',");
                upstr.Append(fkje + ",");
                upstr.Append(jhje + ",");
                upstr.Append("'" + bz + "'");
                upstr.Append(")");
                upStrList.Add(upstr.ToString());

                je += Convert.ToDecimal(fkje);
            }

            sbMain.Append(Convert.ToString(je) + ")");
            upStrList.Add(sbMain.ToString());

            if (server.ExecuteNonQuerysArray(upStrList) < 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('保存失败,请重试！')</script>");
            }
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('保存成功！');window.returnValue ='1'; window.close();</script>");
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('保存失败,请重试！')</script>");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button5_Click(object sender, EventArgs e)
    {
        DataTable dt = server.RunQueryCmdToTable("select * from bill_cgzjfk where 1=2");

        foreach (GridViewRow dr in GridView1.Rows)
        {
            if (!((CheckBox)dr.FindControl("CheckBox1")).Checked)
            {
                DataRow newRow = dt.NewRow();
                newRow["gysbh"] = ((TextBox)dr.FindControl("gysbh")).Text;
                newRow["gysmc"] = ((TextBox)dr.FindControl("gysmc")).Text;
                newRow["jhje"] = ((TextBox)dr.FindControl("jhje")).Text;
                newRow["fkje"] = ((TextBox)dr.FindControl("fkje")).Text;
                newRow["bz"] = ((TextBox)dr.FindControl("bz")).Text;
                dt.Rows.Add(newRow);
            }
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        this.BindDGgys();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string alldwbh = this.TextBox4.Text.ToString().Trim();

        if (string.IsNullOrEmpty(alldwbh))
        {
            return;
        }

        string lsdw = "";
        this.TextBox4.Text = "";
        DataTable dt = server.RunQueryCmdToTable("select * from bill_cgzjfk where billcode='" + this.lbl_BillCode.Text.ToString().Trim() + "' order by jhindex");

        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            GridViewRow gRow = this.GridView1.Rows[i];
            string dwbh = ((TextBox)gRow.FindControl("gysbh")).Text;
            if (dwbh != "")
            {
                DataRow newRow = dt.NewRow();
                lsdw = lsdw + "," + dwbh;
                newRow["gysbh"] = ((TextBox)gRow.FindControl("gysbh")).Text;
                newRow["gysmc"] = ((TextBox)gRow.FindControl("gysmc")).Text;
                newRow["jhje"] = ((TextBox)gRow.FindControl("jhje")).Text;
                newRow["fkje"] = ((TextBox)gRow.FindControl("fkje")).Text;
                newRow["bz"] = ((TextBox)gRow.FindControl("bz")).Text;

                dt.Rows.Add(newRow);
            }
        }

        DateTime now = DateTime.Now;
        DateTime monthBeg = Convert.ToDateTime(now.ToString("yyyy-MM-dd").Substring(0, 7) + "-01");
        DateTime monthEnd = monthBeg.AddMonths(1);


        string sql = @" select gysbh,mc,je 
                    from (select gysbh,sum(byfkje) as je from bill_cgzjjh where cgbh in(
                    select billcode from bill_main where flowid='cgzjjh' and convert(varchar(10),billdate,121)>='" + monthBeg.ToString("yyyy-MM-dd") + "' and convert(varchar(10),billdate,121)<'" + monthEnd.ToString("yyyy-MM-dd") + "'" +
                    @") group by gysbh)a 
                    inner join 
                    (select bh,mc from zqxserp_jk.dbo.info_dy_ven) b on a.gysbh=b.bh where gysbh in(" + alldwbh + ") ";

        DataTable dtdw = server.RunQueryCmdToTable(sql);

        DataRow addRow;
        for (int i = 0; i < dtdw.Rows.Count; i++)
        {
            addRow = dt.NewRow();
            string dwbh = dtdw.Rows[i]["gysbh"].ToString().Trim();
            if (lsdw.IndexOf(dwbh) < 1)
            {
                addRow["gysbh"] = dtdw.Rows[i]["gysbh"];
                addRow["gysmc"] = dtdw.Rows[i]["mc"];
                addRow["jhje"] = dtdw.Rows[i]["je"];

                addRow["fkje"] = 0;
                addRow["bz"] = "";


                dt.Rows.Add(addRow);
                dt.AcceptChanges();
            }
        }

        //GridView重新绑定数据源
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    protected void Button3_Click(object sender, EventArgs e)
    {
        BindDGgys();
    }
    protected void Button4_Click(object sender, EventArgs e)
    {

        string thisname = FileUpload1.FileName;
        if (string.IsNullOrEmpty(thisname))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script> alert('请先选择上传的文件!');</script>");
            return;
        }

        if (thisname.Split('.')[1] != "xls")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script> alert('上传文件必须是xls格式!');</script>");
            return;
        }
        
        string path = Server.MapPath(@"~/Uploads/CaiGouZiJin/");
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        string[] filesNames = Directory.GetFiles(path);
        string date = DateTime.Now.ToString("yyyyMMdd");
        int todayFile = (from filesName in filesNames
                         where filesName.IndexOf(date) > 0
                         select filesName).Count();
        string newFileName = date + (todayFile + 1).ToString("0000");

        string nowPath = path + "\\" + newFileName + ".xls";
        FileUpload1.SaveAs(nowPath);

        #region --------读取文件内容到服务器内存----------
        string constr = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source =" + nowPath + ";Extended Properties=Excel 8.0";

        DataTable dt = new DataTable();
        using (OleDbConnection conn = new OleDbConnection(constr))
        {
            conn.Open();
            //取得excel表中的字段名和表名
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();

            string Sql = "select *,0 as jhje from [" + tableName + "]";
            OleDbDataAdapter mycommand = new OleDbDataAdapter(Sql, conn);
            DataSet ds = new DataSet();
            mycommand.Fill(ds, "[" + tableName + "]");
            dt = ds.Tables[0];
        }
        #endregion

        bool check = true;
        if (dt.Columns[1].ColumnName != "供应商编号" || dt.Columns.Count != 5)
        {
            check = false;
        }
        else if (dt.Columns[2].ColumnName != "供应商名称")
        {
            check = false;
        }
        else if (dt.Columns[3].ColumnName != "付款金额")
        {
            check = false;
        }
        else if (dt.Columns[4].ColumnName != "备注")
        {
            check = false;
        }
        if (!check)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script> alert('上传EXCEL格式不合法!');</script>");
            return;
        }
        dt.Columns[1].ColumnName = "gysbh";
        dt.Columns[2].ColumnName = "gysmc";
        dt.Columns[3].ColumnName = "fkje";
        dt.Columns[4].ColumnName = "bz";


        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}
