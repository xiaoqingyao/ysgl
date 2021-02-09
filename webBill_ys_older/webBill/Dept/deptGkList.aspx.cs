using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_Dept_deptGkList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Page.Request.QueryString["deptCode"] == "" || Page.Request.QueryString["deptCode"] == null)
            {
                this.Button1.Visible = false;
                this.Label1.Text = "请选择相关人员后进行操作权限设置...";
            }
            else
            {
                this.Button1.Visible = true;
                this.Label1.Text = "当前归口部门：[" + Page.Request.QueryString["deptCode"].ToString().Trim() + "]" + server.GetCellValue("select deptname from dbo.bill_departments where deptcode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");

            }
            Bind();
        }
    }
    private void Bind()
    {
        string sql = " select * from bill_departments where isnull(Isgk,'N')!='Y' ";
        DataSet dt = server.GetDataSet(sql);
        this.GridView1.DataSource = dt.Tables[0];
        this.GridView1.DataBind();

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_dept_gksz where gkdeptcode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");

        foreach (GridViewRow row in GridView1.Rows)
        {
            CheckBox cbox = (CheckBox)row.FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                if (row.Cells[1].Text.Length > 2)
                {
                    list.Add("insert into bill_dept_gksz(gkdeptcode,deptCode) values('" + Page.Request.QueryString["deptCode"].ToString().Trim() + "','" + row.Cells[1].Text + "')");
                }
            }
        }


        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Pager)
        {

            if (Page.Request.QueryString["deptCode"] != null)
            {
           
                string strsql = @" select count(*) from bill_dept_gksz where deptcode ='" + e.Row.Cells[1].Text + "' and gkdeptcode='" + Page.Request.QueryString["deptCode"] + "'";
                string strcount = server.GetCellValue(strsql);
                if (strcount!="0")
                {
                    CheckBox cbox = (CheckBox)e.Row.FindControl("CheckBox1");
                    cbox.Checked = true;
                }
                //if (e.Row.Cells[3].Text.Trim().Equals("1"))
                //{
                //    CheckBox cbox = (CheckBox)e.Row.FindControl("CheckBox1");
                //    cbox.Checked = true;
                //}

                if (e.Row.Cells[1].Text.Length < 3)
                {
                    string text = "&nbsp &nbsp " + e.Row.Cells[2].Text;
                    e.Row.Cells[2].Text = text;
                    e.Row.Cells[2].Font.Bold = true;
                }
                if (e.Row.Cells[1].Text.Length > 2)
                {
                    string text = "&nbsp &nbsp &nbsp &nbsp &nbsp " + e.Row.Cells[2].Text;
                    e.Row.Cells[2].Text = text;
                }
            }
        }
    }

}