using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_xtsz_CaiwubuDuiYingRight : BasePage
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
                string deptcode = Request.QueryString["deptCode"].ToString().Trim();
                if (!string.IsNullOrEmpty(deptcode.Trim()))
                {
                    string deptname = server.GetCellValue(" select deptName  from  bill_departments where deptCode='" + Convert.ToString(Request.QueryString["deptCode"]).Trim() + "'");
                    if (!string.IsNullOrEmpty(deptname))
                    {
                        this.lblCaiwu.Text = "归集部门编号：[" + deptcode.Trim() + "]" + deptname;
                    }
                }

                this.BindDataGrid(deptcode);
            }
        }
    }

    private void BindDataGrid(string deptcode)
    {
        string sql = @"select distinct dept.deptCode,dept.deptName from bill_departments dept 
                        inner join  bill_sqjfbmdy dy on dept.deptCode=dy.dydeptcode
                        where dept.deptStatus='1'
		                        and dy.billDept='{0}' order by dept.deptCode";
        sql = string.Format(sql, deptcode);
        this.myGrid.DataSource = server.GetDataTable(sql, null);
        this.myGrid.DataBind();
    }

    protected void btn_add_Click(object sender, EventArgs e)
    {
        string deptcode = Request.QueryString["deptCode"];
        string dydept = this.deptCode.Text.Trim();
        if (dydept.Length == 0)
        {
            Response.Write("部门编号不能为空");
            return;
        }
        if (string.IsNullOrEmpty(deptcode))
        {
            Response.Write("请先选择左侧要归集的财务部门");
            return;
        }

        string sql = "select count(1) from bill_departments where deptcode='"+dydept+"'";
        string count= server.ExecuteScalar(sql).ToString();
        if (count=="0")
        {
            Response.Write("添加的部门编号不存在，请核实！");
            return;
        }
        string insql = "insert into bill_sqjfbmdy(billdept,dydeptcode) values('{0}','{1}')";
        insql = string.Format(insql, deptcode, dydept);
        server.ExecuteNonQuery(insql);
        BindDataGrid(deptcode);
        Response.Write("<label style='color:red'>添加成功！</label>");
    }

    protected void btn_dele_Click(object sender, EventArgs e)
    {
        string deptcode = Request.QueryString["deptCode"];
        string dydeptcode = hf_user.Value;
        if (string.IsNullOrEmpty(deptcode))
        {
            Response.Write("请选择要删除的行");
            return;
        }
        string sql = "delete from bill_sqjfbmdy where billdept='"+deptcode+"' and dydeptcode='"+dydeptcode+"'";
        server.ExecuteNonQuery(sql);
        BindDataGrid(deptcode);
        Response.Write("<label style='color:red'>删除成功！</label>");
    }
}