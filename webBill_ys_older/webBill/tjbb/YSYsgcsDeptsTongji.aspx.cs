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

public partial class webBill_tjbb_YSYsgcsDeptsTongji : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {


            BindDept();
            ddlNd.DataSource = server.GetDataSet("select distinct nian from bill_ysgc order by nian desc");
            ddlNd.DataTextField = "nian";
            ddlNd.DataValueField = "nian";
            ddlNd.DataBind();

            BindYsgc();
        }
    }

    private void BindDept()
    {
        DataSet temp = server.GetDataSet("select deptCode,('['+deptCode+']'+deptName) as deptName  from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");
        drpDept.DataSource = temp;
        drpDept.DataTextField = "deptName";
        drpDept.DataValueField = "deptCode";
        drpDept.DataBind();
        drpDept.Items.Insert(0, new ListItem("所有单位", ""));
    }

    private void BindYsgc()
    {

        string sql = "	select gcbh,xmmc from bill_ysgc  where 1=1  ";
        if (!string.IsNullOrEmpty(ddlNd.SelectedValue))
        {
            sql += "  and nian='" + ddlNd.SelectedValue + "'";
        }
        sql += " order by gcbh";
        DataSet temp = server.GetDataSet(sql);
        ddlYsgc.DataSource = temp;
        ddlYsgc.DataTextField = "xmmc";
        ddlYsgc.DataValueField = "gcbh";
        ddlYsgc.DataBind();
        ddlYsgc.Items.Insert(0, new ListItem("所有预算过程", ""));
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string dept = "";
        string ysgc = "";
        string[] temp = hf_dept.Value.Split('|');
        string[] ysgcs=hf_ysgc.Value.Split('|');
        if (temp[0] != "所有单位")
        {
            dept = hf_dept.Value;
        } 
        if (ysgcs[0]!="所有预算过程")
        {
            ysgc=hf_ysgc.Value;
        }
        Response.Redirect("YSYsgcsDeptsTongjiResult.aspx?deptCode=" + dept + "&Ysgc=" + ysgc);
    }
    protected void ddlNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindYsgc();
    }
}
