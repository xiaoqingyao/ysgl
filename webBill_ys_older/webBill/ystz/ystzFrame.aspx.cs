using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text;

public partial class webBill_ystz_ystzFrame : System.Web.UI.Page
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
                this.bindData();
                this.bindDataToY();
            }
        }
    }


    void bindData()
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0,4)+"0001";


        string sql = @"select xmmc,c.yskmcode,c.yskmmc,d.deptcode,d.deptname,isnull(ysje,0) as ysje,billcode
                    from dbo.bill_ysmxb a,bill_ysgc b,bill_yskm c ,bill_departments d
                    where a.gcbh='" + date + @"' and b.gcbh=a.gcbh  and c.yskmcode=a.yskm and a.ysdept=d.deptcode ";

        string filter = this.TextBox1.Text;
        if (!string.IsNullOrEmpty(filter))
        {
            sql += " and (deptcode like '%" + filter + "%' or deptname like '%" + filter + "%')";
        }

        string kmfilter = this.TextBox6.Text;
        if (!string.IsNullOrEmpty(kmfilter))
        {
            sql += " and (yskmcode like '%" + kmfilter + "%' or yskmmc like '%" + kmfilter + "%')";
        }

        sql += " order by deptcode,yskmcode";
        DataSet temp = server.GetDataSet(sql);




        #region 计算分页相关数据
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


        this.myGrid.DataSource = temp;
        
        this.myGrid.DataBind();
    }


    void bindDataToY()
    {
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4);
        StringBuilder jd = new StringBuilder();

        jd.Append("'"+date+"0001',");
        jd.Append("'"+date + "0002',");
        jd.Append("'"+date + "0003',");
        jd.Append("'"+date + "0004',");
        jd.Append("'"+date + "0005'");

        string sql = @"select xmmc,c.yskmcode,c.yskmmc,d.deptcode,d.deptname,isnull(ysje,0) as ysje,billcode,a.gcbh
                    from dbo.bill_ysmxb a,bill_ysgc b,bill_yskm c ,bill_departments d
                    where a.gcbh not in(" + jd.ToString() + @") and b.gcbh=a.gcbh  and c.yskmcode=a.yskm and a.ysdept=d.deptcode and left(a.gcbh,4)='" + date+"'";

        string filter = this.TextBox3.Text;
        if (!string.IsNullOrEmpty(filter))
        {
            sql += " and (deptcode like '%" + filter + "%' or deptname like '%" + filter + "%')";
        }

        string kmfilter = this.TextBox5.Text;
        if (!string.IsNullOrEmpty(kmfilter))
        {
            sql += " and (yskmcode like '%" + kmfilter + "%' or yskmmc like '%" + kmfilter + "%')";
        }

        sql += " order by deptcode,yskmcode";
        DataSet temp = server.GetDataSet(sql);


        #region 计算分页相关数据
        this.lblPageSize2.Text = this.DataGrid1.PageSize.ToString();
        this.lblItemCount2.Text = temp.Tables[0].Rows.Count.ToString();
        double pageCountDouble = double.Parse(this.lblItemCount2.Text) / double.Parse(this.lblPageSize2.Text);
        int pageCount = Convert.ToInt32(Math.Ceiling(pageCountDouble));
        this.lblPageCount2.Text = pageCount.ToString();
        this.drpPageIndex2.Items.Clear();
        for (int i = 0; i <= pageCount - 1; i++)
        {
            int pIndex = i + 1;
            ListItem li = new ListItem(pIndex.ToString(), pIndex.ToString());
            if (pIndex == this.DataGrid1.CurrentPageIndex + 1)
            {
                li.Selected = true;
            }
            this.drpPageIndex2.Items.Add(li);
        }
        this.showStatus();
        #endregion


        this.DataGrid1.DataSource = temp;
        this.DataGrid1.DataBind();
    }


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
        this.bindData();
    }
    protected void lBtnPrePage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex - 1;
        this.bindData();
    }
    protected void lBtnNextPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.CurrentPageIndex + 1;
        this.bindData();
    }
    protected void lBtnLastPage_Click(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = this.myGrid.PageCount - 1;
        this.bindData();
    }
    protected void drpPageIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.myGrid.CurrentPageIndex = int.Parse(this.drpPageIndex.SelectedItem.Value) - 1;
        this.bindData();
    }
    #endregion



    #region showStatus 分页相关2
    void showStatus2()
    {
        if (this.drpPageIndex.Items.Count == 0)
        {
            this.lBtnFirstPage2.Enabled = false;
            this.lBtnPrePage2.Enabled = false;
            this.lBtnNextPage2.Enabled = false;
            this.lBtnLastPage2.Enabled = false;
            return;
        }
        if (int.Parse(this.lblPageCount2.Text) == int.Parse(this.drpPageIndex2.SelectedItem.Value))//最后一页
        {
            this.lBtnNextPage2.Enabled = false;
            this.lBtnLastPage2.Enabled = false;
        }
        else
        {
            this.lBtnNextPage2.Enabled = true;
            this.lBtnLastPage2.Enabled = true;
        }
        if (int.Parse(this.drpPageIndex2.SelectedItem.Value) == 1)//第一页
        {
            this.lBtnFirstPage2.Enabled = false;
            this.lBtnPrePage2.Enabled = false;
        }
        else
        {
            this.lBtnFirstPage2.Enabled = true;
            this.lBtnPrePage2.Enabled = true;
        }
    }

    protected void lBtnFirstPage2_Click(object sender, EventArgs e)
    {
        this.DataGrid1.CurrentPageIndex = 0;
        this.bindDataToY();
    }
    protected void lBtnPrePage2_Click(object sender, EventArgs e)
    {
        this.DataGrid1.CurrentPageIndex = this.DataGrid1.CurrentPageIndex - 1;
        this.bindDataToY();
    }
    protected void lBtnNextPage2_Click(object sender, EventArgs e)
    {
        this.DataGrid1.CurrentPageIndex = this.DataGrid1.CurrentPageIndex + 1;
        this.bindDataToY();
    }
    protected void lBtnLastPage2_Click(object sender, EventArgs e)
    {
        this.DataGrid1.CurrentPageIndex = this.DataGrid1.PageCount - 1;
        this.bindDataToY();
    }
    protected void drpPageIndex2_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.DataGrid1.CurrentPageIndex = int.Parse(this.drpPageIndex2.SelectedItem.Value) - 1;
        this.bindDataToY();
    }
    #endregion


    protected void myGrid_EditCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            int index = e.Item.ItemIndex;
            string kmcode = Convert.ToString(myGrid.Items[index].Cells[4].Text);
            string billcode = Convert.ToString(myGrid.Items[index].Cells[7].Text);
            string deptcode = Convert.ToString(myGrid.Items[index].Cells[2].Text);
            decimal ysje = Convert.ToDecimal(((TextBox)myGrid.Items[index].Cells[6].FindControl("TextBox2")).Text);

            string url = @"ystzfp_Edit.aspx?kmcode=" + kmcode + "&billcode=" + billcode + "&deptcode=" + deptcode + "&ysje=" + Convert.ToString(ysje);
            StringBuilder script = new StringBuilder();
            script.Append("<script>");
            script.Append("openDetail('");
            script.Append(url);
            script.Append("')");
            script.Append("</script>");

            ClientScript.RegisterStartupScript(this.GetType(),"",script.ToString());
        }
    }

    protected void myGrid_EditCommand2(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            int index = e.Item.ItemIndex;
            string kmcode = Convert.ToString(DataGrid1.Items[index].Cells[4].Text);
            string billcode = Convert.ToString(DataGrid1.Items[index].Cells[7].Text);
            string deptcode = Convert.ToString(DataGrid1.Items[index].Cells[2].Text);
            decimal ysje = Convert.ToDecimal(((TextBox)DataGrid1.Items[index].Cells[6].FindControl("TextBox4")).Text);
            string gcbh = Convert.ToString(DataGrid1.Items[index].Cells[8].Text);

            SqlParameter[] sps = {
                                 new SqlParameter("@cllb","1"),
                                 new SqlParameter("@gcbh",gcbh),
                                 new SqlParameter("@bmbh",deptcode),
                                 new SqlParameter("@yskm",kmcode),
                                 new SqlParameter("@xgje",ysje),
                             };
            server.ExecuteProc("bill_pro_ystz", sps);

            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>alert('修改完成!');</script>");
        }
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.bindDataToY();
    }



}
