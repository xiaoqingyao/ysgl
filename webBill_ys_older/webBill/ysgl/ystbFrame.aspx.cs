﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using WorkFlowLibrary.WorkFlowBll;

public partial class ysgl_ystbFrame : System.Web.UI.Page
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
                this.Label1.Text = server.GetCellValue("select deptname from bill_departments where deptcode='" + (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim()) + "'") + " 已填报预算过程";

                //this.lblShlc.Text = (new workFlowLibrary.workFlow()).getShlcWord("ys");
                this.BindDataGrid();
            }
        }
    }
    void BindDataGrid()
    {
        //string deptCodes = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());

        //string sql = "select billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select deptname from bill_departments where deptcode=billdept) as billDept,(select username from bill_users where usercode=billuser) as billUser,billDate,stepid from bill_main where flowid='ys' and billCode in (select billCode from bill_ysmxb where ysdept='"+deptCodes+"' and yskm in (select yskmcode from bill_yskm where tblx='01'))";


        //sql += " and billDept='" + deptCodes + "' and billType='1' ";

        //sql += " order by billdate desc ";

        string sql = @" 
 select billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName
 ,(select deptname from bill_departments where deptcode=billdept) as billDept,
 (select username from bill_users where usercode=billuser) as billUser,billDate,stepid 
 from bill_main where flowid='ys' 
 and billCode in  (select billCode from bill_ysmxb where  yskm in (select yskmcode from bill_yskm where tblx='01'))
  and billDept  in (select objectID from bill_userright where rightType='2' and userCode='" + Session["userCode"].ToString() + "')    and billType='1' order by billdate desc ";

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
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystbSelectYsgc.aspx");
    }



    //不用了,提交之前先进行检测
    protected void Button4_Click(object sender, EventArgs e)
    {

        string billCode = "";
        int count = 0;
        string shyj = "";
        string stepID_ID = "";
        string deptcode = "";

        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[7].Text.ToString().Trim();
                deptcode = this.myGrid.Items[i].Cells[8].Text.ToString().Trim();

                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待操作的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能选择一项！');", true);
        }
        else
        {
            if (stepID_ID != "-1" && stepID_ID != "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算已进入审核流程,无须再次提交！');", true);
            }
            else
            {
                string cwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billCode + "' and yskm in (select yskmcode from bill_yskm where tblx='02')");
                string dwtb = server.GetCellValue("select count(1) from bill_ysmxb where billCode='" + billCode + "' and yskm in (select yskmcode from bill_yskm where tblx='01')");

                string cwtbkm = server.GetCellValue("select count(1) from bill_yskm_dept where deptCode='" + deptcode + "' and yskmcode in (select yskmcode from bill_yskm where tblx='02')");


                if (cwtb == "0" && !(cwtbkm == "0"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少财务填报部分,不能提交！');", true);
                    return;
                } if (dwtb == "0")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算过程缺少部门填报部分,不能提交！');", true);
                    return;
                }

                if (server.ExecuteNonQuery("update bill_main set stepID='begin',loopTimes=loopTimes+1 where flowID='ys' and billCode='" + billCode + "'") == -1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('提交成功！');", true);
                    this.BindDataGrid();
                }
            }
        }
    }


    protected void Button6_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }
    protected void btn_look_Click(object sender, EventArgs e)
    {
        string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        Response.Redirect("cwtbDetail.aspx?from=ystbFrame&billCode=" + choosebill.Value + "&deptCode=" + deptCode);
    }
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        Response.Redirect("ystbEdit.aspx?billCode=" + choosebill.Value);
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header)
        {
            string billcode = e.Item.Cells[0].Text;
            WorkFlowRecordManager bll = new WorkFlowRecordManager();

            if (e.Item.Cells[5].Text == "end")
            {
                e.Item.Cells[5].Text = "审批通过";
            }
            else
            {
                string state = bll.WFState(billcode);
                e.Item.Cells[5].Text = state;
            }
        }
    }
}
