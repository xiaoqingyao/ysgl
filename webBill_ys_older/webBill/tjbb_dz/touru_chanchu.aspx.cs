using Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_touru_chanchu : System.Web.UI.Page
{
    ConfigDal configdal = new ConfigDal();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string bbTime = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dt = getdtSelect();
            string strexecsql = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                strexecsql = "exec dz_touru_chanchu_main '" + dt.Rows[0]["kssj"].ToString() + "','" + dt.Rows[0]["jzsj"].ToString() + "','" + dt.Rows[0]["deptCode"].ToString() + "','" + dt.Rows[0]["yskm"].ToString() + "'";
            }
           // Response.Write(strexecsql);
            this.GridView1.DataSource = server.GetDataTable(strexecsql, null);
            this.GridView1.DataBind();
            //显示报表时间
            bbTime = server.GetCellValue("select top 1 note1 from bill_tcsql");
        }

    }
    private DataTable getdtSelect()
    {
        string strsql = "select top 1 * from bill_tcsql ";
        return new sqlHelper.sqlHelper().GetDataTable(strsql, null);
        
    }
    protected void btn_export_Click(object sender, EventArgs e)
    {
        DataTable dt = getdtSelect();
        string strexecsql = "";
        if (dt != null && dt.Rows.Count > 0)
        {
            strexecsql = "exec dz_touru_chanchu '" + dt.Rows[0]["kssj"].ToString() + "','" + dt.Rows[0]["jzsj"].ToString() + "','" + dt.Rows[0]["deptCode"].ToString() + "','" + dt.Rows[0]["yskm"].ToString() + "'";
        }
        DataTable dtrel = server.GetDataTable(strexecsql, null);
        IDictionary<string, string> lstCols = new Dictionary<string, string>();
        new ExcelHelper().ExpExcel(dtrel, "投入产出报表决算明细", lstCols);
    }
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        Response.Redirect("zhYsTj.aspx");
    }
    protected void btn_export_main_Click(object sender, EventArgs e)
    {
        DataTable dt = getdtSelect();
        string strexecsql = "";
        if (dt != null && dt.Rows.Count > 0)
        {
            strexecsql = "exec dz_touru_chanchu_main '" + dt.Rows[0]["kssj"].ToString() + "','" + dt.Rows[0]["jzsj"].ToString() + "','" + dt.Rows[0]["deptCode"].ToString() + "','" + dt.Rows[0]["yskm"].ToString() + "'";
        }
        DataTable dtrel = server.GetDataTable(strexecsql, null);
        IDictionary<string, string> lstCols = new Dictionary<string, string>();
        new ExcelHelper().ExpExcel(dtrel, "投入产出报表", lstCols);
    }
}