using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using System.Data;
using System.Drawing;


public partial class webBill_tjbb_graph_DeptsFykmdbNew : System.Web.UI.Page
{
    private string _fusionHTML = "";
    /// <summary>
    /// 用于显示图表的html
    /// </summary>
    public string FusionHTML
    {
        get { return _fusionHTML; }
    }
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}

        if (!IsPostBack)
        {
            string value = "";
            if (!string.IsNullOrEmpty(Request["ysgcs"]))
            {
                value = Request["ysgcs"];
            }
            else
            {
                value = DateTime.Now.Year.ToString() + "00" + (Convert.ToInt32(DateTime.Now.Month) + 5).ToString();
                hfYsgc.Value = value;
                txt_ysgc.Value = DateTime.Now.Year.ToString() + "年" + DateTime.Now.Month.ToString() + "月";
            }
           
            
            if (!string.IsNullOrEmpty(Request["depts"]))
            {
                value = Request["depts"];
            }
            else
            {
                value = server.GetCellValue("select userDept from bill_users where usercode ='20114'");
            }

            hfDept.Value = value;
            txt_dept.Value = server.GetCellValue("select '['+deptCode+']'+deptname from bill_departments where deptcode ='" + value + "'");
            
            if (!string.IsNullOrEmpty(Request["yskm"]))
            {
                value = Request["yskm"];
            }
            else
            {
                DataSet ds = server.GetDataSet("select  top 1 '['+yskmcode+']'+yskmMc as yskmMc ,yskmCode from bill_yskm  where  kmStatus ='1' ");
                hfYskm.Value = Convert.ToString(ds.Tables[0].Rows[0]["yskmCode"]);
                txt_yskm.Value = Convert.ToString( ds.Tables[0].Rows[0]["yskmMc"]);
            }

            if (!string.IsNullOrEmpty(hfYsgc.Value)&&!string.IsNullOrEmpty(hfDept.Value)&&!string.IsNullOrEmpty(hfYskm.Value))
            {
                BuildGraph();
            }
        }
    }

    /// <summary>
    /// 显示图表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        BuildGraph();
    }


    protected void BuildGraph()
    {
        string ysgc = hfYsgc.Value;
        string dept = hfDept.Value;
        string yskm = hfYskm.Value;
        DataTable dt = server.GetDataTable("exec pro_zdy_dept_ysgc_km '" + dept + "','" + ysgc + "','" + yskm + "'", null);
        string strcaption = server.GetCellValue("select yskmmc from bill_yskm where yskmcode='" + yskm + "'", null);
        strcaption = "各部门" + strcaption + "预算情况分析";
        //组件xml
        StringBuilder sbXML = new StringBuilder("<chart caption='" + strcaption + "' shownames='1' showvalues='0' decimals='0' numberPrefix='10000' baseFontSize='13' ><categories>");
        StringBuilder sbDataSets = new StringBuilder();
        //组件categories
        for (int i = 2; i < dt.Columns.Count; i++)
        {
            sbXML.Append(string.Format("<category label='{0}' />", dt.Columns[i].ColumnName));
        }
        sbXML.Append("</categories>");

        //组件dataset
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sbDataSets.Append(string.Format("<dataset seriesName='{0}'  showValues='1'>", dt.Rows[i][1]));
            for (int j = 2; j < dt.Columns.Count; j++)
            {
                sbDataSets.Append(string.Format("<set value='{0}' />", dt.Rows[i][j].ToString()));
            }
            sbDataSets.Append("</dataset>");
        }
        sbXML.Append(sbDataSets);
        sbXML.Append("</chart>");
        //Response.Write(sbXML.ToString());
        this._fusionHTML = FusionCharts.RenderChart("../FusionCharts/MSColumnLine3D.swf", "", sbXML.ToString(), "FactorySum", "1200", "550", false, false);

    }
}
