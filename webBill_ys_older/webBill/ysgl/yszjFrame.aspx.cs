using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Bll.UserProperty;

public partial class ysgl_yszjFrame : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl;
    string strDeptCode;
    string strXmcode;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["Ctrl"] != null)
            {
                strCtrl = Request["Ctrl"].ToString().Trim();
            }
            if (Request["deptcode"] != null)
            {
                strDeptCode = Request["deptcode"].ToString().Trim();
            }
            if (!string.IsNullOrEmpty(Request["xmcode"]))
            {
                strXmcode = Request["xmcode"].ToString();
            }
            if (!string.IsNullOrEmpty(Request["page"]))
            {
                hf_page.Value = Request["page"].ToString().Trim();
            }
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }

    public void BindDataGrid()
    {

        string strsql2 = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
        this.ddlnian.DataSource = server.GetDataTable(strsql2, null);
        this.ddlnian.DataTextField = "xmmc";
        this.ddlnian.DataValueField = "nian";
        this.ddlnian.DataBind();

        string sql = "select gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status from bill_ysgc  ";

        SysManager sysMgr = new SysManager();
        string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
        IDictionary<string, string> config = sysMgr.GetsysConfigBynd(nd);
        string strisdz = "";
        if (!string.IsNullOrEmpty(Request["isdz"]))
        {
            strisdz = Request["isdz"].ToString();
        }
        if (strisdz != "1")
        {
            //0，年度预算，1，季度预算，2，月度预算
            if (config["MonthOrQuarter"] == "1")
            {
                sql += " where ystype='1' and (status='2' or jzsj<'" + System.DateTime.Now.ToString("yyyy-MM-dd") + "') ";
            }
            else if (config["MonthOrQuarter"] == "0")
            {
                sql += " where ystype='0' and (status='2' or jzsj<'" + System.DateTime.Now.ToString("yyyy-MM-dd") + "') ";
            }
            else
            {
                sql += " where ystype='2' and (status='2' or jzsj<'" + System.DateTime.Now.ToString("yyyy-MM-dd") + "') ";
            }
        }
        else
        {
            lblmsg.Visible = false;
            sql += " where ystype='2'";
        }

        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        if (ddlnian.SelectedValue != null)
        {
            sql += " and nian='" + ddlnian.SelectedValue + "'";
        }

        //2014-05-16 beg 预算调整时目标预算过程必须是已开启预算
        // and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='2014' and ysdept='05' and ystype='1')
        string configVal = server.GetCellValue("select avalue from t_Config where akey='AllowTzUodoYs' ");
        if (!string.IsNullOrEmpty(configVal) && configVal == "1")
        {
            sql += "  and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='" + ddlnian.SelectedValue + "' and ysdept='" + Request["deptcode"] + "' and ystype='1')";
            lblmsg.InnerText = lblmsg.InnerText + "只能选择做过年初预算的预算过程；";
        }
        //2014-05-16 end
        sql += " order by nian desc,gcbh ";
       // Response.Write(sql);
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

    protected void ddlnianselectindexchanged(object sender, EventArgs e)
    {
        string sql = "select gcbh,xmmc,kssj,jzsj,(select username from bill_users where usercode=fqr) as fqr,fqsj,(case status when '0' then '未开始' when '1' then '进行中' when '2' then '已结束' end) as statusName,status from bill_ysgc ";


        SysManager sysMgr = new SysManager();
        string nd = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4);
        IDictionary<string, string> config = sysMgr.GetsysConfigBynd(nd);
        //0，年度预算，1，季度预算，2，月度预算
        if (config["MonthOrQuarter"] == "1")
        {
            sql += " where ystype='1' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
        }
        else if (config["MonthOrQuarter"] == "0")
        {
            sql += " where ystype='0' and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "') ";
        }
        else
        {
            sql += " where ystype='2' ";//and (status='2' or jzsj<'" + System.DateTime.Now.ToShortDateString() + "' ) 
        }

        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (gcbh like '%" + this.TextBox1.Text.ToString().Trim() + "%' or  xmmc like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        }
        if (ddlnian.SelectedValue != null)
        {
            sql += " and nian='" + ddlnian.SelectedValue + "'";
        }
        //2014-05-16 beg 预算调整时目标预算过程必须是已开启预算
        // and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='2014' and ysdept='05' and ystype='1')
        string configVal = server.GetCellValue("select avalue from t_Config where akey='AllowTzUodoYs' ");
        if (!string.IsNullOrEmpty(configVal) && configVal == "1")
        {
            sql += "  and gcbh in (select distinct gcbh from  bill_ysmxb where left(gcbh,4)='" + ddlnian.SelectedValue + "' and ysdept='" + Request["deptcode"] + "' and ystype='1')";
        }
        //2014-05-16 end
        sql += " order by nian desc,gcbh ";
        //Response.Write(sql);
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
    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个过程！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待填报的过程！');", true);
        }
        else
        {
            if (strCtrl != "" && strCtrl == "ysnzj")
            {
                Response.Redirect("ysnzjAdd.aspx?gcbh=" + billGuid + "&deptcode=" + strDeptCode);
            }
            else
            {
                if ((!string.IsNullOrEmpty(Request["ishz"]) && Request["ishz"].ToString() == "1") || (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"].ToString() == "1"))
                {

                    string strurl = @"yszjAddhz.aspx?gcbh=" + billGuid + "&ishz=1&deptcode=" + strDeptCode;
                    if (!string.IsNullOrEmpty(Request["isxm"]))
                    {
                        strurl += "&isxm=" + Request["isxm"] + "&xmcode=" + strXmcode;
                    }

                    Response.Redirect(strurl);
                }
                else
                {
                    string strurl = @"yszjAdd.aspx?gcbh=" + billGuid + "&deptcode=" + strDeptCode;
                    if (!string.IsNullOrEmpty(strXmcode))
                    {
                        strurl += "&xmcode=" + strXmcode;
                    }
                    Response.Redirect(strurl);
                }


            }
            //ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('yszjList.aspx?gcbh=" + billGuid + "');", true);

        }
    }
}