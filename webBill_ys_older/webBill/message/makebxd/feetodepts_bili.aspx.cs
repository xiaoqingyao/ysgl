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
using System.Collections.Generic;

public partial class webBill_makebxd_feetodepts_bili : System.Web.UI.Page
{
    string stryskmcode = "";//类别
    sqlHelper.sqlHelper sqlhelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        object objyskm = Request["type"];//获取预算科目类型 没有的话直接不加载页面
        if (objyskm == null)
        {
            return;
        }
        else
        {
            stryskmcode = objyskm.ToString();
        }
        if (!IsPostBack)
        {
            bindGridView();
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_click(object sender, EventArgs e)
    {
        List<string> lstSql = new List<string>();
        if (stryskmcode.Equals(""))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('参数不完整，无法保存。');", true);
            return;
        }
        lstSql.Add("delete from mkbxd_feetodeptbili where yskmcode='" + stryskmcode + "'");
        double dbtotalbili = 0;//比例总和
        int irowscount = this.GridView1.Rows.Count;
        for (int i = 0; i < irowscount; i++)
        {
            //部门
            string strdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            strdeptcode = strdeptcode.Replace("&nbsp;", "");
            if (strdeptcode.Equals(""))
            {
                continue;
            }
            //科目类别
            string yskmcode = this.GridView1.Rows[i].Cells[5].Text.Trim();
            stryskmcode = stryskmcode.Replace("&nbsp;", "");
            if (stryskmcode.Equals(""))
            {
                continue;
            }
            //比例
            string strbili = "";
            TextBox txtbili = this.GridView1.Rows[i].Cells[2].FindControl("txtbili") as TextBox;
            double dbbili = 0;
            if (txtbili != null && double.TryParse(txtbili.Text, out dbbili))
            {
                strbili = txtbili.Text;
            }
            if (dbbili <= 0)//比例必须大于0
            {
                continue;
            }
            dbtotalbili = dbtotalbili + dbbili;
            lstSql.Add(string.Format("insert into mkbxd_feetodeptbili(yskmcode,deptcode,bili) values('{0}','{1}','{2}')", yskmcode, strdeptcode, strbili));
        }
        if (lstSql.Count >= 2)
        {
            //if (dbtotalbili != 1)
            //{
            //    lblmsg.InnerText = "保存失败，原因：比例总和不等于1，差值为：" + (1 - dbtotalbili).ToString(); ;
            //    return;
            //}
            int irel = sqlhelper.ExecuteNonQuerysArray(lstSql);
            if (irel >= 1)
            {
                lblmsg.InnerText = "保存成功。";
                bindGridView();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('保存失败。');", true);
            }
        }
    }
    /// <summary>
    /// 初始化gridview
    /// </summary>
    private void bindGridView()
    {
        string sql = @"select * from (
                    select yskm.yskmcode,yskmmc,deptcode,
                    (select deptname from bill_departments where deptcode=yskmdept.deptcode) as deptname 
                    from bill_yskm yskm,bill_yskm_dept yskmdept 
                    where yskm.yskmcode=yskmdept.yskmcode and isnull(kmdytype,'000')!='1' and yskm.yskmCode=@yskmcode  and yskmdept.deptcode!='000001') a
                    left join mkbxd_feetodeptbili b
                    on a.deptcode=b.deptcode and a.yskmCode=b.yskmcode";
        SqlParameter[] arrsp = new SqlParameter[] { new SqlParameter("@yskmcode", stryskmcode) };
        DataTable dtrel = sqlhelper.GetDataTable(sql, arrsp);
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
    }
    double dbtotal = 0;
    /// <summary>
    /// 行绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            TextBox txtbili = e.Row.Cells[2].FindControl("txtbili") as TextBox;
            string strbili = e.Row.Cells[4].Text.Trim();//比例
            double debili = 0;
            if (txtbili != null && double.TryParse(strbili, out debili))
            {
                txtbili.Text = debili.ToString();
                dbtotal = dbtotal + debili;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[2].Text = dbtotal.ToString();
        }
        else { }
    }

}
