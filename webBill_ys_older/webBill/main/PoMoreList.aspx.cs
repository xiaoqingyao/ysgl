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
using Bll.UserProperty;
using WorkFlowLibrary.WorkFlowBll;
using System.Text;

public partial class webBill_main_PoMoreList : System.Web.UI.Page
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
                BingDDL();
            }
        }
    }

    private void BingDDL()
    {
        string sql = " select * from mainworkflow where flowid in (select distinct flowID  from bill_main  )";
        DataTable dt= server.GetDataTable(sql,null);
        if (dt.Rows.Count >0)
        {
            ddltype.DataTextField = "flowName";
            ddltype.DataValueField = "flowid";
            ddltype.DataSource = dt;
            ddltype.DataBind();
            ddltype.Items.Insert(0, new ListItem( "--全部--","0"));
        }
    }


    void bindData()
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            string sql = @"select  billName,flowID,billuser,isGk,gkDept,(select bxzy from bill_ybbxmxb 
where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,stepid,billDept,billCode,
(select xmmc from bill_ysgc where gcbh=billName) as billName,
(select '['+usercode+']'+username from bill_users where usercode=billuser) as billUserName,
(select djmc from bill_djlx where djbh=bill_main.flowid) as billType,
convert(varchar(10),billdate,20) as billdate ,billje from bill_main where (billUser='" + Session["userCode"].ToString().Trim() + "' or billCode in (select billCode from bill_ybbxmxb where bxr='" + Session["userCode"].ToString().Trim() + "')) ";
            //if (this.TextBox1.Text.ToString().Trim() != "")
            //{
            //    sql += " and (billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
            //}

            //申请开始日期
            if (txb_sqrqbegin.Text != "")
            {
                sql += " and  billDate >=cast ('" + txb_sqrqbegin.Text + "' as datetime  ) ";
            }
            //申请结束日期
            if (txb_sqrqend.Text != "")
            {
                sql += " and  billDate <=cast ('" + txb_sqrqend.Text + "' as datetime  ) ";
            }

            if (!string.IsNullOrEmpty(ddltype.SelectedValue)&&ddltype.SelectedValue!="0")
            {
                sql += "  and flowID ='"+ddltype.SelectedValue+"'";
            }
            sql += "  and stepid<>'end' order by billdate desc";
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

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strbillcode = e.Item.Cells[0].Text.ToString().Trim();
            string strurl = @"";
            SysManager sysMgr = new SysManager();

            string zt = e.Item.Cells[2].Text;
            if (zt == "end")
            {
                e.Item.Cells[2].Text = "审批通过";
            }
            else
            {
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[2].Text = state;
            }

            string strpotype = e.Item.Cells[3].Text.ToString().Trim();
            if (strpotype != "")
            {
                if (strpotype == "ybbx")//一般报销单
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "yzsq")//借款申请单
                {
                    strurl = "../../SaleBill/BorrowMoney/LoanListDetails.aspx?Ctrl=View&Code=" + strbillcode;
                }
                else if (strpotype == "yszj")//预算追加
                {
                    strurl = "../ysgl/yszjEdit.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "ystz")//预算调整
                {
                    strurl = "../ysgl/YstzDetailNew.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "kmystz")//科目预算调整
                {
                    strurl = "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + strbillcode;
                }
                else if (strpotype == "cksj")//车款上缴报告
                {
                    strurl = "../../SaleBill/RemitTance/RemitTanceDetails.aspx?Ctrl=look&Code=" + strbillcode;
                }
                else if (strpotype == "qtbx")//其他报销
                {
                    strurl = "../bxgl/bxDetailFinal.aspx?type=look&djtype=qtbx&billCode=" + strbillcode;
                }
                else if (strpotype == "ccbg")//出差报告单
                {
                    strurl = "../fysq/travelReportDetail.aspx?Ctrl=View&Code=" + strbillcode;
                }
                else if (strpotype == "ccsq")//出差申请单
                {
                    strurl = "../fysq/travelApplicationDetails2.aspx?Ctrl=View&Code=" + strbillcode;

                }
                else if (strpotype == "ys")//预算填报
                {
                    strurl = "../ysgl/cwtbDetail.aspx?from=lookDialog&billCode=" + strbillcode;
                }
                else if (strpotype == "cgsp")//采购审批单
                {
                    strurl = "../fysq/cgspDetail.aspx?type=look&cgbh=" + strbillcode;
                }
                else if (strpotype == "yzsq")//借款申请单
                {
                    strurl = "../fysq/cgspDetail.aspx?type=look&cgbh=" + strbillcode;
                }
                else if (strpotype == "kmystz")//科目预算调整单
                {
                    strurl = "../ysgl/KmYstzDetail.aspx?Ctrl=View&billCode=" + strbillcode;
                }
                else if (strpotype == "kpsq")//开票申请 
                {
                    strurl = "../../SaleBill/kpsq/KpsqDetails.aspx?type=look&bh=" + strbillcode;
                }
                else if (strpotype == "lscg")//临时采购  
                {
                    strurl = "../fysq/lscgDetail.aspx?type=audit&cgbh=" + strbillcode;
                }
                else if (strpotype == "cgzjjh")//采购资金计划单
                {
                    strurl = "../cgzj/cgzjDetail.aspx?type=look&par=" + strbillcode;
                }

                else if (strpotype == "xmzf")//项目支付 
                {
                    strurl = "../xmzf/xmzfsqDetail.aspx?type=look&billCode=" + strbillcode;
                }
                else if (strpotype == "cgzjfk")//采购资金付款单
                {

                }
                else if (strpotype == "xmys")//项目预算单
                {

                }
                else if(strpotype=="gkbx")
                {
                    strurl = "../bxgl/bxDetailForGK.aspx?type=look&billCode=" + server.GetCellValue("select billName from bill_main where billCode='"+strbillcode+"'");
                }
                //e.Row.Cells[1].Text = @"<a href=# onclick='openDetail(" + strurl + ");'>" + e.Row.Cells[1].Text.ToString().Trim() + "</a>";
                string strtext = e.Item.Cells[1].Text;
                string strZhaiYao = e.Item.Cells[5].Text.Trim();
                strZhaiYao = strZhaiYao.Replace("&nbsp;", "");
                string strZhaiYaoShow = strZhaiYao.Length > 9 ? strZhaiYao.Substring(0, 6) + "..." : strZhaiYao;
                if (strZhaiYao.Length > 0)
                {
                    strZhaiYaoShow = "_" + strZhaiYaoShow;
                }
               // e.Item.Cells[1].Text = "<a href=\"#\" onclick=\"openDetail('" + strurl + "')\">" + strtext + "</a>";
                (e.Item.FindControl("Literal1") as Literal).Text = strurl;
            }
          
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }
}
