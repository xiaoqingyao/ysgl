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
using Bll.UserProperty;
using WorkFlowLibrary.WorkFlowBll;

public partial class Phone_ybbxSelect : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    //一般报销单是否需要审核 默认是1 需要 edit by Lvcc
  //  bool boYbbxNeedAudit = new Bll.ConfigBLL().GetValueByKey("YbbxNeedAudit").Equals("0") ? false : true;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
          
            if (!IsPostBack)
            {
                this.txtLoanDateFrm.Attributes.Add("onfocus", "javascript:setday(this);");
                this.txtLoanDateTo.Attributes.Add("onfocus", "javascript:setday(this);");
                this.bindData();
            }
        //}
    }

    void bindData()
    {
        string sql = @"select billName,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,
(select xmmc from bill_ysgc where gcbh=billName) as billName,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,
billdate,billje from bill_main where (billUser='21373' or billCode in (select billCode from bill_ybbxmxb where bxr='21373')) and flowID='ybbx'and stepID!='end'";

        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            sql += " and (billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
        }
        if (txtLoanDateFrm.Text.Trim() != "")
        {
            sql += " and billdate>='" + this.txtLoanDateFrm.Text.Trim() + "'";
        }
        if (this.txtLoanDateTo.Text.Trim() != "")
        {
            sql += " and billdate<='" + this.txtLoanDateTo.Text.Trim() + "'";
        }
        sql += "order by billName desc,billdate desc";
        DataSet temp = server.GetDataSet(sql);

        #region 计算分页相关数据1
        this.lblPageSize.Text = this.myGrid.PageSize.ToString();
        this.lblItemCount.Text = Convert.ToString(temp.Tables[0].Rows.Count);
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
        //通过一般报销是否自动审核的配置项来控制删除是否好用 如果是自动审核的  允许删除审核成功了的单子 edit by lvcc 20130124
       // this.hdYbbxNeedAudit.Value = boYbbxNeedAudit ? "1" : "0";

        //注册
        object objregistermark_date = System.Configuration.ConfigurationManager.AppSettings["RegistDate"];
        DateTime dtReg;
        if (objregistermark_date != null)
        {
            dtReg = DateTime.Parse(objregistermark_date.ToString());
            DateTime strnowdate = DateTime.Now;
            if (strnowdate > dtReg)
            {
                Random dom = new Random();
                int idom = dom.Next(0, 10);
                if (idom % 3 == 0)
                {
                    TimeSpan aa = DateTime.Parse("2013-05-26") - DateTime.Now;
                    int iDays = aa.Days + 1;
                    if (iDays > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期，还有" + iDays + "天系统将锁定，请联系软件开发商！');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('您好，试用版本已经到期,，请联系软件开发商！');", true);

                    }
                }
            }
        }
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



    protected void Button4_Click(object sender, EventArgs e)
    {
        this.bindData();
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            SysManager sysMgr = new SysManager();
            e.Item.Cells[4].Text = sysMgr.GetDeptCodeName(e.Item.Cells[4].Text);
            string zt = e.Item.Cells[7].Text;
            if (zt == "end")
            {
                e.Item.Cells[7].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[7].Text = state;
            }

            string isgk = e.Item.Cells[8].Text;
            if (isgk == "1")
            {
                e.Item.Cells[8].Text = "是";
            }
            else
            {
                e.Item.Cells[8].Text = "否";
            }

            string gkDep = e.Item.Cells[9].Text;
            if (gkDep != "&nbsp;" && !string.IsNullOrEmpty(gkDep))
            {
                e.Item.Cells[9].Text = sysMgr.GetDeptCodeName(gkDep);
            }
        }
    }
    
}
