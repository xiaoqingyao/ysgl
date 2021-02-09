using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class webBill_ystz_ystzFrame2 : System.Web.UI.Page
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
                PaginationToGV1.PageIndex = "1";

                string rowsql = @"select count(*) from (" + SqlMaker() + ")t";

                string row1 = Convert.ToString(server.ExecuteScalar(rowsql));

                if (row1 == "0")
                {
                    PaginationToGV1.RowsCount = "1";
                }
                else
                {
                    PaginationToGV1.RowsCount = row1 ;
                }

                PaginationToGV2.PageIndex = "1";

                string rowsql2 = @"select count(*) from (" + SqlMakerToY() + ")t";

                string row2 = Convert.ToString(server.ExecuteScalar(rowsql2));

                if (row2 == "0")
                {
                    PaginationToGV2.RowsCount = "1";
                }
                else
                {
                    PaginationToGV2.RowsCount = row2;
                }
                this.bindData();
                this.bindDataToY();

                #region 提供部门,预算的数组
                DataSet ds = server.GetDataSet("select deptname from bill_departments");
                StringBuilder arry = new StringBuilder();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    arry.Append("'");
                    arry.Append(Convert.ToString(dr["deptname"]));
                    arry.Append("',");
                }
                string script = arry.ToString().Substring(0, arry.Length - 1) ;
                ClientScript.RegisterArrayDeclaration("availableTags", script);

                DataSet dskm = server.GetDataSet("select yskmmc from bill_yskm");
                StringBuilder arrykm = new StringBuilder();
                foreach (DataRow dr in dskm.Tables[0].Rows)
                {
                    arrykm.Append("'");
                    arrykm.Append(Convert.ToString(dr["yskmmc"]));
                    arrykm.Append("',");
                }
                string scriptkm = arrykm.ToString().Substring(0, arrykm.Length - 1);
                ClientScript.RegisterArrayDeclaration("availablekm", scriptkm);
                #endregion
            }
            if (this.GridView1.Rows.Count > 0)
            {
                GridView1.UseAccessibleHeader = true;
                GridView1.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            if (this.GridView2.Rows.Count > 0)
            {
                GridView2.UseAccessibleHeader = true;
                GridView2.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
    }

    private void bindData()
    {
        string strpage = PaginationToGV1.PageIndex;
        int pageIndex = 0;
        if (!string.IsNullOrEmpty(strpage))
        {
            pageIndex = Convert.ToInt32(PaginationToGV1.PageIndex);
        }

        int begRow = (pageIndex - 1) * 17;
        int endRow = pageIndex * 17;

        string sql = @"select * from("+SqlMaker()+")t where t.crow>"+Convert.ToString(begRow)+" and t.crow <="+Convert.ToString(endRow);


        
        DataSet temp = server.GetDataSet(sql);
        this.GridView1.DataSource = temp;
        this.GridView1.DataBind();
    }

    private string SqlMaker()
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        string sql = @"select xmmc,c.yskmcode,c.yskmmc,d.deptcode,d.deptname,isnull(ysje,0) as ysje,billcode,Row_number()over(order by deptcode) as crow 
                    from dbo.bill_ysmxb a,bill_ysgc b,bill_yskm c ,bill_departments d
                    where a.gcbh='" + date + @"' and b.gcbh=a.gcbh  and c.yskmcode=a.yskm and a.ysdept=d.deptcode ";

        string filter = this.TextBox1.Text;
        if (!string.IsNullOrEmpty(filter))
        {
            sql += " and (deptcode like '%" + filter + "%' or deptname like '%" + filter + "%')";
        }

        string kmfilter = this.TextBox6.Text;
        if (!string.IsNullOrEmpty(kmfilter))
        {
            sql += " and (yskmcode like '%" + kmfilter + "%' or yskmmc like '%" + kmfilter + "%')";
        }

        return sql;
    }

    private string SqlMakerToY()
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4);
        StringBuilder jd = new StringBuilder();

        jd.Append("'" + date + "0001',");
        jd.Append("'" + date + "0002',");
        jd.Append("'" + date + "0003',");
        jd.Append("'" + date + "0004',");
        jd.Append("'" + date + "0005'");

        string sql = @"select xmmc,c.yskmcode,c.yskmmc,d.deptcode,d.deptname,isnull(ysje,0) as ysje,billcode,a.gcbh,Row_number()over(order by deptcode) as crow 
                    from dbo.bill_ysmxb a,bill_ysgc b,bill_yskm c ,bill_departments d
                    where a.gcbh not in(" + jd.ToString() + @") and b.gcbh=a.gcbh  and c.yskmcode=a.yskm and a.ysdept=d.deptcode and left(a.gcbh,4)='" + date + "'";

        string filter = this.TextBox3.Text;
        if (!string.IsNullOrEmpty(filter))
        {
            sql += " and (deptcode like '%" + filter + "%' or deptname like '%" + filter + "%')";
        }

        string kmfilter = this.TextBox5.Text;
        if (!string.IsNullOrEmpty(kmfilter))
        {
            sql += " and (yskmcode like '%" + kmfilter + "%' or yskmmc like '%" + kmfilter + "%')";
        }
        return sql;
    }

    private void bindDataToY()
    {
        int pageIndex = Convert.ToInt32(PaginationToGV2.PageIndex);

        int begRow = (pageIndex - 1) * 17;
        int endRow = pageIndex * 17;

        string sql = @"select * from(" + SqlMakerToY() + ")t where t.crow>" + Convert.ToString(begRow) + " and t.crow <=" + Convert.ToString(endRow);

        DataSet temp = server.GetDataSet(sql);
        this.GridView2.DataSource = temp;

        this.GridView2.DataBind();
    }



    protected void Button4_Click(object sender, EventArgs e)
    {
        PaginationToGV1.PageIndex = "1";

        string rowsql = @"select count(*) from (" + SqlMaker() + ")t";

        PaginationToGV1.RowsCount = Convert.ToString(server.ExecuteScalar(rowsql));
        this.bindData();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        PaginationToGV2.PageIndex = "1";

        string rowsql2 = @"select count(*) from (" + SqlMakerToY() + ")t";

        PaginationToGV2.RowsCount = Convert.ToString(server.ExecuteScalar(rowsql2));
        this.bindDataToY();
    }



    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void PaginationToGV2_GvBind(object sender, EventArgs e)
    {
        bindDataToY();
    }
}
