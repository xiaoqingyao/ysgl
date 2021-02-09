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

public partial class webBill_bxgl_ybbxSetDept : System.Web.UI.Page
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
                //this.btn_add.Attributes.Add("onclick", "javascript:selectdeptAbc('" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "');");
                this.BindDataGrid();

                TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
                tNode.NavigateUrl = "deptList.aspx?deptCode=&mxGuid=" + Page.Request.QueryString["mxGuid"].ToString().Trim();
                tNode.Target = "list";
                tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
                this.TreeView1.Nodes.Add(tNode);

                (new Departments()).BindOffice(tNode, "deptList.aspx", "list", "&mxGuid=" + Page.Request.QueryString["mxGuid"].ToString().Trim(), true, "../Resources/images/treeview/", "", false, "", "", "");

                this.TreeView1.Nodes[0].ShowCheckBox = false;
                for (int i = 0; i <= this.TreeView1.Nodes[0].ChildNodes.Count - 1; i++)
                {
                    //this.TreeView1.Nodes[0].ChildNodes[i].ShowCheckBox = false;
                    string deptCode = this.TreeView1.Nodes[0].ChildNodes[i].Value.ToString().Trim();
                    if (server.GetCellValue("select count(1) from bill_departments where sjdeptcode='" + deptCode + "'") != "0")
                    {
                        this.TreeView1.Nodes[0].ChildNodes[i].ShowCheckBox = false;
                    }

                    for (int j = 0; j <= this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes.Count - 1; j++)
                    {
                        deptCode = this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].Value.ToString().Trim();
                        if (server.GetCellValue("select count(1) from bill_departments where sjdeptcode='" + deptCode + "'") != "0")
                        {
                            this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ShowCheckBox = false;
                        }
                    }
                }
            }
        }
    }

    #region 绑定
    public void BindDataGrid()
    {
        string sql = "select (select '['+deptcode+']'+deptname from bill_departments where bill_departments.deptcode=a.deptcode) as deptname,je,mxGuid from bill_ybbxmxb_fykm_dept a where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "' ";
       

        DataSet temp = server.GetDataSet(sql);
        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = temp.Tables[0].Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount.Text) / double.Parse(this.lblPageSize.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount.Text = pageCount.ToString();
        this.drpPageIndex.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.myGrid.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex.Items.Add(li);
        }
        this.showStatus();
        #endregion
        if (temp.Tables[0].Rows.Count == 0)
        {
            temp = null;
        }
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    #endregion

    #region showStatus 分页相关
    void showStatus()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount.Text) == int.Parse(this.drpPageIndex.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage.Enabled = false;
            this.lBtnLastPage.Enabled = false;
        }
        else
        {
            this.lBtnNextPage.Enabled = true;
            this.lBtnLastPage.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage.Enabled = false;
            this.lBtnPrePage.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage.Enabled = true;
            this.lBtnPrePage.Enabled = true;
        }
    }

    protected void lBtnFirstPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = 0;
        this.BindDataGrid();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.BindDataGrid();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.BindDataGrid();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.BindDataGrid();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.BindDataGrid();
    }
    #endregion

    #region 包含下级
    protected void chkNextLevel_CheckedChanged(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    #endregion


    #region 删除
    protected void btn_dele_Click(object sender, EventArgs e)
    {
        string str_deptcode = "";
        int sel_count = 0;
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                list.Add("delete from bill_ybbxmxb_fykm_dept where mxGuid='" + myGrid.Items[i].Cells[1].Text + "'");
                //str_deptcode = myGrid.Items[i].Cells[1].Text;
                sel_count += 1;
            }
        }
        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择要删除的项目！');</script>");
        }
        //else if (sel_count > 1)
        //{
        //    Page.RegisterStartupScript("", "<script>window.alert('每次只允许删除一个项目！');</script>");
        //}
        else
        {
           
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                this.BindDataGrid();
            }
        }
    }
    #endregion


    #region 查询
    protected void btn_sele_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    #endregion
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_ybbxmxb_fykm_dept where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "'");
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            //CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            //if (cbox.Checked == true)
            //{
                string guid = (new GuidHelper()).getNewGuid();
                string dept = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                dept = dept.Substring(1, dept.IndexOf("]") - 1);

                string je = ((TextBox)this.myGrid.Items[i].FindControl("TextBox2")).Text.ToString().Trim();

                list.Add("insert into bill_ybbxmxb_fykm_dept values('" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + guid + "','" + dept + "'," + je + ",0)");
            //}
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');self.close();", true);
            this.BindDataGrid();
        }
    }
    protected void btnRefresh2_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {
        this.selectButton.Style["display"] = "";
        this.selectTree.Style["display"] = "";
        this.resultButton.Style["display"] = "none";
        this.resultList.Style["display"] = "none";
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        int sel_count = 0;
        System.Collections.Generic.List<string> sqls = new System.Collections.Generic.List<string>();
        for (int i = 0; i <= this.TreeView1.Nodes[0].ChildNodes.Count - 1; i++)
        {
            if (this.TreeView1.Nodes[0].ChildNodes[i].ShowCheckBox == true && this.TreeView1.Nodes[0].ChildNodes[i].Checked == true)
            {
                sel_count += 1;
                string guid0 = (new GuidHelper()).getNewGuid();
                sqls.Add("insert into bill_ybbxmxb_fykm_dept select '" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + guid0 + "',deptcode,0,0 from bill_departments where deptcode='" + this.TreeView1.Nodes[0].ChildNodes[i].Value + "' and deptcode not in (select deptcode from bill_ybbxmxb_fykm_dept where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "')");
            }

            for (int j = 0; j <= this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes.Count - 1; j++)
            {
                if (this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ShowCheckBox == true && this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].Checked == true)
                {
                    sel_count += 1;
                    string guid1 = (new GuidHelper()).getNewGuid();
                    sqls.Add("insert into bill_ybbxmxb_fykm_dept select '" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + guid1 + "',deptcode,0,0 from bill_departments where deptcode='" + this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].Value + "' and deptcode not in (select deptcode from bill_ybbxmxb_fykm_dept where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "')");
                }
                for (int k = 0; k <= this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ChildNodes.Count - 1; k++)
                {
                    if (this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[k].ShowCheckBox == true && this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[k].Checked == true)
                    {
                        sel_count += 1;
                        string guid2 = (new GuidHelper()).getNewGuid();
                        sqls.Add("insert into bill_ybbxmxb_fykm_dept select '" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + guid2 + "',deptcode,0,0 from bill_departments where deptcode='" + this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[k].Value + "' and deptcode not in (select deptcode from bill_ybbxmxb_fykm_dept where kmmxguid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "')");
                        this.TreeView1.Nodes[0].ChildNodes[i].ChildNodes[j].ChildNodes[k].Checked = false;
                    }
                }
            }
        }
        if (sel_count == 0)
        {
            Page.RegisterStartupScript("", "<script>window.alert('请选择部门！');</script>");
        }
        else
        {
            if (server.ExecuteNonQuerysArray(sqls) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
                return;
            }
            else
            {
                //this.selectButton.Style["display"] = "none";
                //this.selectTree.Style["display"] = "none";
                //this.resultButton.Style["display"] = "";
                //this.resultList.Style["display"] = "";
                //this.BindDataGrid();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');self.close();", true);
            }
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //this.selectButton.Style["display"] = "none";
        //this.selectTree.Style["display"] = "none";
        //this.resultButton.Style["display"] = "";
        //this.resultList.Style["display"] = "";
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
}
