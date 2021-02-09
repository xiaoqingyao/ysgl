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

public partial class webBill_tjbb_graph_DeptsFykmdb : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    private string _fusionHTML = "";

    /// <summary>
    /// 用于显示图表的html
    /// </summary>
    public string FusionHTML
    {
        get { return _fusionHTML; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterArrayDeclaration("availableTags", GetYskmAll());
        if (!IsPostBack)
        {
            BindYsgc();
            BindDept();
            BindYskm();
        }
    }

    private string GetYskmAll()
    {
        DataSet ds = server.GetDataSet("select '['+yskmcode+']'+yskmMc as yskmMc  from bill_yskm  where  kmStatus ='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["yskmMc"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }
    private void BindYsgc()
    {

        string sql = "select gcbh,xmmc from bill_ysgc  where 1=1 and ysType !='0' order by nian desc , gcbh asc";
        DataSet temp = server.GetDataSet(sql);
        rptYsgc.DataSource = temp;
        rptYsgc.DataBind();
    }

    private void BindYskm()
    {
        string sql = "select * from (	select yskmcode,'['+yskmcode+']'+yskmMc as yskmMc,(select count(*) from bill_yskm where len(yskmcode)>len(a.yskmcode) and yskmcode like a.yskmcode+'%') as childcount from bill_yskm a where  kmStatus ='1') b where b.childcount=0 order by yskmcode";
        DataSet temp = server.GetDataSet(sql);
        rptYskm.DataSource = temp;
        rptYskm.DataBind();
    }
    private void BindDept()
    {
        DataSet temp = server.GetDataSet("select deptCode,('['+deptCode+']'+deptName) as deptName  from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='')");
        rptDept.DataSource = temp;
        rptDept.DataBind();
    }
    /// <summary>
    /// 显示图表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        string ysgc = "";
        string dept = "";
        string yskm = "";
        //获取预算过程
        for (int i = 0; i < rptYsgc.Items.Count; i++)
        {
            CheckBox check = (CheckBox)this.rptYsgc.Items[i].FindControl("ckYsgc");
            if (check.Checked == false)
            {
                continue;
            }
            else
            {
                HiddenField hf1 = (HiddenField)this.rptYsgc.Items[i].FindControl("hfYsgc");
                ysgc += hf1.Value + ",";
            }
        }
        if (ysgc.Length - 1 > 0)
        {
            ysgc = ysgc.Substring(0, ysgc.Length - 1);
        }
        else
        {
            showMessage("请至少选择一个预算过程。", false, "");
            return;
        }

        //获取部门

        for (int i = 0; i < rptDept.Items.Count; i++)
        {
            CheckBox check = (CheckBox)this.rptDept.Items[i].FindControl("ckDept");
            if (check.Checked == false)
            {
                continue;
            }
            else
            {
                HiddenField hf2 = (HiddenField)this.rptDept.Items[i].FindControl("hfDept");
                dept += hf2.Value + ",";
            }
        }
        if (dept.Length - 1 > 0)
        {
            dept = dept.Substring(0, dept.Length - 1);
        }
        else
        {
            showMessage("请至少选择一个部门。", false, "");
            return;
        }
        //获取预算科目

        for (int i = 0; i < rptYskm.Items.Count; i++)
        {
            CheckBox check = (CheckBox)this.rptYskm.Items[i].FindControl("ckYskm");
            if (check.Checked == false)
            {
                continue;
            }
            else
            {
                HiddenField hf3 = (HiddenField)this.rptYskm.Items[i].FindControl("hfYskm");
                yskm = hf3.Value;
            }
        }
        if (yskm.Length < 1)
        {
            showMessage("选择一个预算科目。", false, "");
            return;
        }
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
        this._fusionHTML = FusionCharts.RenderChart("../FusionCharts/MSColumnLine3D.swf", "", sbXML.ToString(), "FactorySum", "700", "500", false, false);
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }
}
