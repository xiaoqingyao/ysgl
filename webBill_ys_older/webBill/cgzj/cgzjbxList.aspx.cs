﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Bll.UserProperty;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_cgzj_cgzjbxList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    my_fzl.bindClss bindCl = new my_fzl.bindClss();
    string strGongyingshangbh = "";
    string strDateFrm = "";
    string strDateTo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["gysbh"]!=null)
            {
                strGongyingshangbh = Request["gysbh"].ToString();
            }
            if (Request["datefrom"]!=null) 
            {
                strDateFrm=Request["datefrom"].ToString();
            }
             if (Request["dateto"]!=null)
            {
                strDateTo = Request["dateto"].ToString();
            }
            this.txb_sqrqbegin.Attributes.Add("onfocus", "javascript:setday(this);");
            this.txb_sqrqend.Attributes.Add("onfocus", "javascript:setday(this);");

            if (!IsPostBack)
            {
                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("cgzjfk");
                initControl();
                BindDataGrid();

            }
        }
    }
    private void initControl() {
        if (!strGongyingshangbh.Equals(""))
        {
            this.Button1.Visible =
                this.btn_edit.Visible =
                    this.btn_summit.Visible =
                this.btn_replace.Visible = false;
            this.spdelete.Visible = false;
            btnReturn.Visible = true;
        }
        else {
            btnReturn.Visible = false;
        }
        if (!strDateFrm.Equals(""))
        {
            this.txb_sqrqbegin.Text = strDateFrm;
        }
        if (!strDateTo.Equals(""))
        {
            this.txb_sqrqend.Text = strDateTo;
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
    protected void BindDataGrid()
    {
        string sql = "select billdate,billname,billje,a.billCode,billDept,(select userName from bill_users where userCode=billuser) as  billuser,stepid from bill_main a where a.flowid='cgzjfk' ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and billDept in (" + deptCodes + ")";

        #region 查询条件

        //申请开始日期
        if (txb_sqrqbegin.Text != "")
        {
            sql += " and  a.billDate >cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
        }
        else
        {
            sql += " and  left(CONVERT(varchar(10),a.billDate,121),7) =left(CONVERT(varchar(10),getdate(),121),7) ";
        }
        //申请结束日期
        if (txb_sqrqend.Text != "")
        {
            sql += " and  a.billDate <cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
        }
        if (!strGongyingshangbh.Equals(""))
        {
            sql += " and a.billCode in (select billcode from bill_cgzjfk where gysbh='"+strGongyingshangbh+"')";
        }

        #endregion

        sql += " order by billDate desc ";

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

    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            //e.Item.Cells[2].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[2].Text.ToString().Trim());
            SysManager sysMgr = new SysManager();
            e.Item.Cells[1].Text = sysMgr.GetDeptCodeName(e.Item.Cells[1].Text);
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[6].Text = state;
            }

        }
    }
    protected void btnReturn_Click(object sender, EventArgs e) {
        Response.Redirect("../tjbb/CaiGouZiJinTongJi.aspx");
    }
    


}
