using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_Dept_selctZgUser : System.Web.UI.Page
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
                DataSet temp = server.GetDataSet("select groupid,'['+convert(varchar(6), groupid)+']'+groupname as groupname from bill_usergroup  order by groupid");
                this.ddl_group.DataTextField = "groupName";
                this.ddl_group.DataValueField = "groupID";
                this.ddl_group.DataSource = temp;
                this.ddl_group.DataBind();
                this.BindDataGrid();
            }
        }
    }

    #region 绑定数据
    public void BindDataGrid()
    {
        string alreadyDeptCode = "";
        DataSet temp1 = server.GetDataSet("select * from bill_dept_fgld where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        for (int i = 0; i <= temp1.Tables[0].Rows.Count - 1; i++)
        {
            alreadyDeptCode += "'" + temp1.Tables[0].Rows[i]["userCode"].ToString().Trim() + "',";
        }

        string sql = "select usercode,username,(select groupname from bill_usergroup where groupid=usergroup) as usergroup ,case userstatus when 1 then '正常' else '禁用' end as userstatus,";
        sql += " (select deptname from bill_departments where deptcode =userdept ) as userdept,userpwd  from bill_users  where userCode<>'admin' ";

        if (txb_where.Text != "")
        {
            sql += " and (usercode like'%" + txb_where.Text + "%' or username like '%" + txb_where.Text + "%')";
        }
        if (ddl_group.SelectedValue != null)
        {
            sql += " and userGroup='" + ddl_group.SelectedValue + "'";
        }
        DataSet temp = server.GetDataSet(sql);


        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            if (alreadyDeptCode.IndexOf("'" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "'") >= 0)
            {
                CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
                chk.Checked = true;
            }
        }
    }
    #endregion


    #region 修改
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_dept_fgld where deptCode like '" + Page.Request.QueryString["deptCode"].ToString().Trim() + "%';");
        string userCode = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                userCode = myGrid.Items[i].Cells[1].Text;
                list.Add("insert into bill_dept_fgld(deptcode,usercode) values('" + Page.Request.QueryString["deptCode"].ToString().Trim() + "','" + userCode + "')");
                //增加下面部门的领导权限
                //list.Add("insert into bill_dept_fgld select  deptCode,'" + userCode + "' from   bill_departments where deptcode like '" + Page.Request.QueryString["deptCode"].ToString().Trim() + "%';");
            }
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存设置失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存完成！');self.close();", true);
        }
    }
    #endregion


    #region 查询
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion

    protected void btn_del_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
    protected void ddl_group_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
}