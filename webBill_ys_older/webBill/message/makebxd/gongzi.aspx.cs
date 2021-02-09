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
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Bll.UserProperty;
using Models;

public partial class webBill_makebxd_gongzi : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("availableTagsdept", GetDeptAll());
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                this.txtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                Models.Bill_Users user = new Dal.UserProperty.UsersDal().GetUserByCode(Session["userCode"].ToString());
                this.txtzdr.Value = "[" + user.UserCode + "]" + user.UserName;

                this.txtgkdept.Value = new Dal.UserProperty.DepartmentDal().GetDeptNameByUser(user.UserCode);
                bindGridView();
            }
        }
    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users where userStatus='1' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    private string GetDeptAll()
    {
        IList<Bill_Departments> lstdept = new Dal.UserProperty.DepartmentDal().GetAllDeptsed();
        StringBuilder arry = new StringBuilder();
        foreach (Bill_Departments item in lstdept)
        {
            arry.Append("'");
            arry.Append("[" + item.DeptCode + "]" + item.DeptName);
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    private void bindGridView()
    {
        DataTable dt = new DataTable();
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    /// <summary>
    /// 显示分解情况
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_showfenjie_click(object sender, EventArgs e)
    {
        //金额
        string strje = this.txttotalamount.Value;
        decimal je = 0;
        if (!decimal.TryParse(strje, out je))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('金额请填写阿拉伯数字。');", true);
            return;
        }
        //科目
        string strkm = this.txtfykm.Value;
        strkm = strkm.Substring(1, strkm.IndexOf("]") - 1);
        string strsql = @"select (select yskmmc from bill_yskm where yskmcode=a.yskmcode) as yskmname
		,(select deptname from bill_departments where deptcode=a.deptcode) as deptname
		,bili
		,cast(round( (@je*bili),2)   as   numeric(18,2))  as je
        ,deptcode
        from  mkbxd_feetodeptbili  a where yskmcode=@yskmcode";
        SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@je", strje), new SqlParameter("@yskmcode", strkm) };
        DataTable dtrel = server.GetDataTable(strsql, arrsp);
        if (dtrel != null)
        {
            this.GridView1.DataSource = dtrel;
            this.GridView1.DataBind();
        }
        //显示预算余额
        string strdeptcode = this.txtgkdept.Value.Trim();
        try
        {
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            strdeptcode = "";
        }
        if (strdeptcode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('请先选择归口部门，格式为[编码]名称');", true);
            return;
        }
        string stryskmcode = this.txtfykm.Value.Trim();
        stryskmcode = stryskmcode.Substring(1, stryskmcode.IndexOf("]") - 1);
        string strdate = this.txtDate.Value.Trim();
        DateTime dt;
        if (!DateTime.TryParse(strdate, out dt))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('日期格式不正确。');", true);
            return;
        }
        Bll.UserProperty.YsManager ysmgr = new Bll.UserProperty.YsManager();
      
        string gcbh= ysmgr.GetYsgcCode(dt);
        
        decimal hfje = ysmgr.GetYueHf(gcbh, strdeptcode, stryskmcode);
        decimal ysje = ysmgr.GetYueYs(gcbh, strdeptcode, stryskmcode);
        decimal syje = ysje - hfje;
        lblsyje.InnerText = syje.ToString();
        
    }
    /// <summary>
    /// 制单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_make_Click(object sender, EventArgs e)
    {
        //制单人
        string strzdr = this.txtzdr.Value.Trim();
        string strzdrcode = "";
        string strzdrname = "";
        try
        {
            strzdrcode = strzdr.Substring(1, strzdr.IndexOf("]") - 1);
            strzdrname = strzdr.Substring(strzdr.IndexOf("]") + 1);
        }
        catch (Exception)
        {
            strzdrcode = "";
        }
        if (string.IsNullOrEmpty(strzdr))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：制单人未填写或格式不正确。');", true);
            return;
        }
        string strzddept = new UserMessage(strzdrcode).GetRootDept().DeptCode;
        //归口部门
        string strgkdept = this.txtgkdept.Value.Trim();
        try
        {
            strgkdept = strgkdept.Substring(1, strgkdept.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            strgkdept = "";
        }
        if (string.IsNullOrEmpty(strgkdept))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：归口部门未填写或格式不正确。');", true);
            return;
        }
        //日期
        string strdate = this.txtDate.Value.Trim();
        DateTime dt;
        if (!DateTime.TryParse(strdate, out dt))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：制单日期未填写或格式不正确。');", true);
            return;
        }
        //费用科目
        string stryskm = this.txtfykm.Value.Trim();
        string stryskmcode = "";//费用科目编号
        string stryskmname = "";//费用科目名称
        try
        {
            stryskmcode = stryskm.Substring(1, stryskm.IndexOf("]") - 1);
            stryskmname = stryskm.Substring(stryskm.IndexOf("]") + 1);
        }
        catch (Exception)
        {
            stryskmcode = "";
        }
        if (string.IsNullOrEmpty(stryskmcode))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：费用科目未选择或格式不正确。');", true);
            return;
        }
        int igridcount = this.GridView1.Rows.Count;
        if (igridcount <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：表体行数为0，请先设置科室分解比例。');", true);
            return;
        }
        //报销摘要
        string strbxzy = dt.Year + "年" + dt.Month + "月" + strzdrname + "报销" + stryskmname;
        //单据唯一键
        string strbillcode = new GuidHelper().getNewGuid();
        List<string> lstsql = new List<string>();
        for (int i = 0; i < igridcount; i++)
        {
            string strsql = "";
            string strdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            TextBox txtfjje = this.GridView1.Rows[i].Cells[3].FindControl("txtje") as TextBox;
            string strje = txtfjje.Text.Trim();
            decimal deje = 0;
            if (!decimal.TryParse(strje, out deje))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：第" + (i + 1) + "行分解金额格式不正确。');", true);
                return;
            }
            strsql = @"insert into lsbxd_main(billcode,flowid,billUser,billDate,billDept,je,se,isgk,gkdept,bxzy,bxsm,fykmcode,sydept,bxlx)
                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')
                        ";
            strsql = string.Format(strsql, strbillcode, "gkbx", strzdrcode, strdate, strzddept, strje, "0", "1", strgkdept, strbxzy, "", stryskmcode, strdeptcode, "01");
            lstsql.Add(strsql);
        }
        if (lstsql.Count > 0)
        {
            int irels = server.ExecuteNonQuerysArray(lstsql);
            if (irels >= 1)
            {
                string strbillname = server.GetCellValue("exec pro_makebxd '" + strbillcode + "','sdnbxd'");
                //if (!string.IsNullOrEmpty(strbillname))
                //{
                //    ClientScript.RegisterStartupScript(this.GetType(), "a", "openDetail('" + strbillcode + "');", true);
                //}
                ClientScript.RegisterStartupScript(this.GetType(), "a", "showsuccess('生成报销单成功，单号为：" + strbillname + "');", true);
            }
        }
    }

    decimal debili = 0;
    decimal deje = 0;
    protected void Gridview_OnrowdataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {
            //比例
            string strbili = e.Row.Cells[2].Text.Trim();
            decimal deevebili = 0;
            if (decimal.TryParse(strbili, out deevebili))
            {
                debili += deevebili;
            }
            //金额
            TextBox txtje = e.Row.Cells[3].FindControl("txtje") as TextBox;
            string strje = txtje.Text.Trim();
            decimal deeveje = 0;
            if (decimal.TryParse(strje, out deeveje))
            {
                deje += deeveje;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[2].Text = debili.ToString();
            e.Row.Cells[3].Text = deje.ToString();
            e.Row.Cells[0].Style.Add("text-align", "right");
            e.Row.Cells[2].Style.Add("text-align", "right");
            e.Row.Cells[3].Style.Add("text-align", "right");

        }
    }
}
