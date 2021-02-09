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

public partial class webBill_ysglnew_ViewFjDeptAmount : System.Web.UI.Page
{
    string strkmcode = "";//预算科目编号
    string strnd = "";//年度
    string strgkdeptcode = "";//归口部门编号
    string strkmje = "";//科目预算总金额
    protected void Page_Load(object sender, EventArgs e)
    {
        object objkmcode = Request["kmcode"];
        if (objkmcode != null)
        {
            strkmcode = objkmcode.ToString();
        }
        object objnd = Request["nd"];
        if (objnd != null)
        {
            strnd = objnd.ToString();
        }
        object objgkdeptcode = Request["gkdeptcode"];
        if (objgkdeptcode != null)
        {
            strgkdeptcode = objgkdeptcode.ToString();
        }
        object objkmje = Request["kmje"];
        if (objkmje != null)
        {
            strkmje = objkmje.ToString();
        }
        if (!IsPostBack)
        {
            bindData();
        }
    }
    private void bindData()
    {
        lblTotalAmount.Text = strkmje;
        lblKmmc.Text = new sqlHelper.sqlHelper().GetCellValue("select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode='" + strkmcode + "'").ToString();
        //bangding gridview
        bindGridView(strkmje);
    }
    /// <summary>
    /// 重新计算
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Calculate_Click(object sender, EventArgs e)
    {
        string strje = this.lblTotalAmount.Text.Trim();
        double dbamount = 0;
        bool bo = double.TryParse(strje, out dbamount);
        if (bo)
        {
            bindGridView(dbamount.ToString());
        }
    }
    private void bindGridView(string strje)
    {
        string strselectsql = "select * from (select deptcode,(select deptname from bill_departments where deptcode = yskmdept.deptcode) as deptname from bill_yskm_dept yskmdept where yskmcode = @yskmcode) a  inner join ( select yskmcode,fjdeptcode,fjbl,round((fjbl*cast(@je as numeric(18,4))),2) as fjje from bill_gkfjbili  where yskmcode=@yskmcode and nian=@nian and gkdeptcode=@gkdeptcode and fjbl!=0) b on a.deptcode=b.fjdeptcode";
        SqlParameter[] arrSp = new SqlParameter[] { new SqlParameter("@je", decimal.Parse(strje.Equals("") ? "0" : strje)), new SqlParameter("@yskmcode", strkmcode), new SqlParameter("@nian", strnd), new SqlParameter("gkdeptcode", strgkdeptcode) };
        DataTable dtrel = new sqlHelper.sqlHelper().GetDataTable(strselectsql, arrSp);
        GridView1.DataSource = dtrel;
        GridView1.DataBind();
    }
    decimal deTotalBili = 0;
    decimal deTotalAmount = 0;
    protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal deevebi = 0;
            string strbili = e.Row.Cells[3].Text.Trim();
            if (decimal.TryParse(strbili, out deevebi))
            {
                deTotalBili += deevebi;
            }
            decimal deeveamount = 0;
            string stramount = e.Row.Cells[2].Text.Trim();
            if (decimal.TryParse(stramount, out deeveamount))
            {
                deTotalAmount += deeveamount;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[2].Text = deTotalAmount.ToString();
            e.Row.Cells[3].Text = deTotalBili.ToString();
        }
    }
}
