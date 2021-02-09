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

public partial class webBill_select_selectRoleUser : System.Web.UI.Page
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
                ListItem li = new ListItem("-=未设置角色=-", "reliy");
                this.DropDownList1.Items.Add(li);

                DataSet temp = server.GetDataSet("select groupID,'['+groupID+']'+groupName as groupName from bill_userGroup order by groupID");
                for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
                {
                    ListItem li2 = new ListItem();
                    li2.Text = temp.Tables[0].Rows[i]["groupName"].ToString().Trim();
                    li2.Value = temp.Tables[0].Rows[i]["groupID"].ToString().Trim();
                    this.DropDownList1.Items.Add(li2);
                }
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {
        string userGroup = this.DropDownList1.SelectedItem.Value;
        string sql = "";
        if (userGroup == "reliy")
        {
            sql = "select usercode,username,(select groupname from bill_usergroup where groupid=usergroup) as usergroup ,case userstatus when 1 then '正常' else '禁用' end as userstatus,";
            sql += " (select deptname from bill_departments where deptcode =userdept ) as userdept,userpwd  from bill_users order by usercode";
        }
        else
        {
            sql = "select usercode,username,(select groupname from bill_usergroup where groupid=usergroup) as usergroup ,case userstatus when 1 then '正常' else '禁用' end as userstatus,";
            sql += " (select deptname from bill_departments where deptcode =userdept ) as userdept,userpwd  from bill_users where userGroup='" + userGroup + "'  order by usercode";
        }

        DataSet temp = server.GetDataSet(sql);

        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }


    #region 确定
    protected void btn_select_Click(object sender, EventArgs e)
    {
        string flowID = Page.Request.QueryString["flowID"].ToString().Trim();
        string stepID = Page.Request.QueryString["stepID"].ToString().Trim();

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //确认所有审核流程 是相同类型的权限设置:删除重写
        list.Add("delete from bill_workflowgroup where flowid='" + flowID + "' and stepID='" + stepID + "'");

        string userCode = "";
        int selectCount = 0;
        if (this.DropDownList2.SelectedItem.Value == "一般审核")
        {
            if (this.DropDownList1.SelectedItem.Value == "reliy")//只设置到人
            {
                for (int i = 0; i < myGrid.Items.Count; i++)
                {
                    CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
                    if (cbox.Checked == true)
                    {
                        userCode = myGrid.Items[i].Cells[1].Text;
                        list.Add("insert into bill_workflowGroup values('" + flowID + "','" + stepID + "','','','" + userCode + "','" + this.DropDownList2.SelectedItem.Value + "')");
                        selectCount += 1;
                    }
                }
                if (selectCount == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('未选择待分配权限的人员！');", true);
                    return;
                }
            }
            else
            {
                for (int i = 0; i < myGrid.Items.Count; i++)
                {
                    CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
                    if (cbox.Checked == true)
                    {
                        userCode = myGrid.Items[i].Cells[1].Text;
                        list.Add("insert into bill_workflowGroup values('" + flowID + "','" + stepID + "','" + this.DropDownList1.SelectedItem.Value + "','','" + userCode + "','"+this.DropDownList2.SelectedItem.Value+"')");
                        selectCount += 1;
                    }
                }
                if (selectCount == 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('未选择待分配权限的人员！');", true);
                    return;
                }
                
            }
        }
        else { //主管审核和分管领导审核：需要单的的管理权限设置,且，只能指定到角色，不能指定人员
            if (this.DropDownList1.SelectedItem.Value == "reliy")//未选择角色
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('未选择待分配审核权限的角色！');", true);
                return;
            }
            else
            {
                list.Add("insert into bill_workflowGroup values('" + flowID + "','" + stepID + "','" + this.DropDownList1.SelectedItem.Value + "','',null,'" + this.DropDownList2.SelectedItem.Value + "')");
            }
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }

    }
    #endregion

    #region 取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Page.RegisterStartupScript("", "<script>self.close();</script>");
    }
    #endregion

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.DropDownList2.SelectedItem.Value == "一般审核")
        {
            //一般审核:必须指定人员。可以不指定角色，也可以指定
            this.myGrid.Columns[0].Visible = true;
            this.showInfo.InnerHtml = "指定角色和人员：审核管理单位单据！仅指定人员：审核所有单位单据！";
        }
        else if (this.DropDownList2.SelectedItem.Value == "业务主管审核")
        {
            //主管审核 和 分管领导审核：只指定角色
            this.myGrid.Columns[0].Visible = false;
            this.showInfo.InnerHtml = "指定角色：审核主管单位单据！";
        }
        else if (this.DropDownList2.SelectedItem.Value == "分管领导审核")
        {
            //主管审核 和 分管领导审核：只指定角色
            this.myGrid.Columns[0].Visible = false;
            this.showInfo.InnerHtml = "指定角色：审核分管单位单据！";
        }
    }
}
