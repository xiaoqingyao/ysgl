using System;
using System.Collections.Generic;

using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Bills_billList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            DataSet temp = server.GetDataSet("select * from bill_userGroup");
            this.DropDownList1.DataTextField = "groupName";
            this.DropDownList1.DataValueField = "groupID";
            this.DropDownList1.DataSource = temp;
            this.DropDownList1.DataBind();

            temp = server.GetDataSet("select * from billType order by billTypeID");
            this.DropDownList2.DataTextField = "billTypeName";
            this.DropDownList2.DataValueField = "billTypeID";
            this.DropDownList2.DataSource = temp;
            this.DropDownList2.DataBind();


            this.bindData();
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }

    public void bindData()
    {
        string groupID = this.DropDownList1.SelectedItem.Value;//组编号
        string billTypeID = this.DropDownList2.SelectedItem.Value;//单据类型

        string sql = "select * from bill_workFlow where billType='" + billTypeID + "' and flowID in (select flowID from bill_workFlowGroup where opUserGroup='" + groupID + "' and billType='" + billTypeID + "')";
        DataSet temp = server.GetDataSet(sql);//获取当前组 当前单据类型有权审核的流程步骤 以及金额上下限

        this.GridView1.DataSource = temp;
        this.GridView1.DataBind();

        if (temp.Tables[0].Rows.Count == 1)
        {
            string billType = billTypeID;
            string beginJe = temp.Tables[0].Rows[0]["beginJe"].ToString().Trim();
            string wkStep = temp.Tables[0].Rows[0]["frontFlowID"].ToString().Trim();


            string resultSQL = "select * from bills where billTypeID='" + billTypeID + "' and wkFlowID='" + wkStep + "'";

            if (beginJe != "")
            {
                resultSQL += " and hjje>" + beginJe;
            }

            temp = server.GetDataSet(resultSQL);
            this.GridView2.DataSource = temp;
            this.GridView2.DataBind();
        }
    }

    /// <summary>
    /// 审核通过：根据当前状态和金额范围决定下一步状态
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAccess_Click(object sender, EventArgs e)
    {
        List<string> arrList = new List<string>();
        foreach (GridViewRow r in this.GridView2.Rows)
        {
            CheckBox chk = (CheckBox)r.FindControl("CheckBox1");
            if (chk.Checked)
            {
                string billCode = r.Cells[1].Text.ToString().Trim();//单据编号
                string billTypeID = r.Cells[3].Text.ToString().Trim();//单据类型编号
                string wkFlowID = r.Cells[4].Text.ToString().Trim();//当前步骤
                string hjje = r.Cells[5].Text.ToString().Trim();//合计金额

                //判断当前单据类型 当前金额的单据 是否需要下一步审批
                DataSet temp = server.GetDataSet("select * from bill_workFlow where billType='" + billTypeID + "' and frontFlowID>" + wkFlowID + " and beginJe<" + hjje + " order by flowID");

                if (temp.Tables[0].Rows.Count == 0)
                {
                    //没有后续的蛇皮流程 直接结束该单据的流程
                    arrList.Add("update bills set wkFlowID=-3 where billTypeID='" + billTypeID + "' and billCode='" + billCode + "'");
                    //记录审核记录
                    //arrList.Add();
                }
                else
                {
                    //有后续的审核流程
                    string newWkFlowID = temp.Tables[0].Rows[0]["frontFlowID"].ToString().Trim();
                    arrList.Add("update bills set wkFlowID=" + newWkFlowID + " where billTypeID='" + billTypeID + "' and billCode='" + billCode + "'");
                    //记录审核记录
                    //arrList.Add();
                }
            }
        }
        if (server.ExecuteNonQuerysArray(arrList) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('failure!');", true);
        }
        else
        {
            this.bindData();
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('success!');", true);
        }
    }
    protected void btnForbidden_Click(object sender, EventArgs e)
    {

        List<string> arrList = new List<string>();
        foreach (GridViewRow r in this.GridView2.Rows)
        {
            CheckBox chk = (CheckBox)r.FindControl("CheckBox1");
            if (chk.Checked)
            {
                string billCode = r.Cells[1].Text.ToString().Trim();//单据编号
                string billTypeID = r.Cells[3].Text.ToString().Trim();//单据类型编号
                string wkFlowID = r.Cells[4].Text.ToString().Trim();//当前步骤
                string hjje = r.Cells[5].Text.ToString().Trim();//合计金额

                arrList.Add("update bills set wkFlowID=-2 where billTypeID='" + billTypeID + "' and billCode='" + billCode + "'");

            }
        }
        if (server.ExecuteNonQuerysArray(arrList) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('failure!');", true);
        }
        else
        {
            this.bindData();
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('success!');", true);
        }
    }
}