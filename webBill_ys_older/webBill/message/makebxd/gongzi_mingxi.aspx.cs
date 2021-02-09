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


public partial class webBill_makebxd_gongzi_mingxi : System.Web.UI.Page
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

    protected void showlist_Click(object sender, EventArgs e)
    {
        //string strsqldistinctyskm = "select distinct yskmCode from bill_gzxmdy";
        //DataTable dt = server.GetDataTable(strsqldistinctyskm, null);
        //if (dt == null || dt.Rows.Count <= 0)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('请先设置工资项目与预算科目的对应关系。');", false);
        //    return;
        //}
        string strshowlist = "exec pro_bill_makebxd_showgongzilist";
        DataTable datatable = server.GetDataTable(strshowlist, null);
        this.GridView1.DataSource = datatable;
        this.GridView1.DataBind();
        this.btn_make.Enabled = true;

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
        //报销摘要
        string strbxzy = dt.Year + "年" + dt.Month + "月" + strzdrname + "报销工资";
        //单据唯一键
        string strbillcode = new GuidHelper().getNewGuid();
        List<string> lstsql = new List<string>();
        int igridviewrows = this.GridView1.Rows.Count;
        for (int i = 0; i < igridviewrows; i++)
        {
            string strsql = "";
            string stryskmcode = this.GridView1.Rows[i].Cells[1].Text.Trim();//预算科目编号
            string strhsdeptcode = this.GridView1.Rows[i].Cells[3].Text.Trim();//核算科室编号
            strhsdeptcode = strhsdeptcode = strhsdeptcode.Replace("&nbsp;", "");
            if (string.IsNullOrEmpty(strhsdeptcode))
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：第" + (i + 1) + "行核算部门编号未能成功匹配，请确认导入的excel文档内的部门名称与系统内的部门名称是否一致。');", true);
                //return;
                continue;
            }
            //核算金额
            string strhsje = ((TextBox)this.GridView1.Rows[i].Cells[5].FindControl("txtje")).Text.Trim();
            double dbje = 0;
            if (!string.IsNullOrEmpty(strhsje) && !double.TryParse(strhsje, out dbje))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('操作失败：第" + (i + 1) + "行核算金额不合法。');", true);
                return;
            }
            strsql = @"insert into lsbxd_main(billcode,flowid,billUser,billDate,billDept,je,se,isgk,gkdept,bxzy,bxsm,fykmcode,sydept,bxlx)
                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')
                        ";
            strsql = string.Format(strsql, strbillcode, "gkbx", strzdrcode, strdate, strzddept, strhsje, "0", "1", strgkdept, strbxzy, "", stryskmcode, strhsdeptcode, "01");
            lstsql.Add(strsql);
        }
        if (lstsql.Count > 0)
        {
            int irels = server.ExecuteNonQuerysArray(lstsql);
            if (irels >= 1)
            {
                string strbillname = server.GetCellValue("exec pro_makebxd '" + strbillcode + "','gzbxd'");
                ClientScript.RegisterStartupScript(this.GetType(), "a", "showsuccess('生成报销单成功，单号为：" + strbillname + "');", true);
            }
        }
    }

    double dbhsje = 0;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.EmptyDataRow && e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            TextBox tb = e.Row.Cells[5].FindControl("txtje") as TextBox;
            double dbeve = 0;
            if (double.TryParse(tb.Text.ToString(),out dbeve))
            {
                dbhsje += dbeve;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = dbhsje.ToString("0.00");
            e.Row.Cells[4].Text = "合计：";
            e.Row.Cells[4].Style.Add("text-align", "right");
        }else{}
    }
}
