using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Models;
using System.Configuration;
using WorkFlowLibrary.WorkFlowBll;
using System.Text;
using Bll.Bills;

public partial class SaleBill_kpsq_KpsqList : System.Web.UI.Page
{
    Bll.Bills.BillMainBLL mainbll = new Bll.Bills.BillMainBLL();
    Bll.Bills.T_BillingApplicationBll bll = new Bll.Bills.T_BillingApplicationBll();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            if (!IsPostBack)
            {
                Bind(true);
               
            }
        }
    }

    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    private void Bind(bool ispock)
    {

        string sql = @"  select *,Row_Number()over(order by billCode desc) as crow from (select billcode ,flowid,billdept,
                         (select '['+deptCode+']'+ deptName from dbo.bill_departments where deptCode=billdept) as billdeptshow , billdate,
                         (select '['+userCode+']'+ userName from bill_users where userCode=billuser)   as billuser,stepid,
                        (select count(*) from workflowrecord where billcode=bill_main.billcode) as recordcount,
                        (select rdState from workflowrecord where billcode=bill_main.billcode) as rdState
                         from Bill_Main ) b
                         where 1=1 and flowid='kpsq' ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += " and billdept in (" + deptCodes + ") ";

       
        #region 查询操作

        //HiddenField1是是否查询  点击查询按钮HiddenField1变成1
        if (txb_sqrqbegin.Text != "" && HiddenField1.Value == "1")
        {
            sql += " and billdate >= '" + txb_sqrqbegin.Text + "' ";
        }
        if (txb_sqrqend.Text != "" && HiddenField1.Value == "1")
        {
            sql += "  and billdate <= '" + txb_sqrqbegin.Text + "'  ";
        }
        if (txtbh.Text != "" && HiddenField1.Value == "1")
        {
            sql += " and billcode like '%"+txtbh.Text+"%'  ";
        }
        if (HiddenField1.Value == "1" && txtdept.Text != "")
        {
            sql += " and billdept in ('" + txtdept.Text.Substring(1, txtdept.Text.IndexOf("]") - 1) + "')  ";
        }
        string strTruckCode = txtTruckCode.Text.Trim();
        if (!strTruckCode.Equals(""))
        {
            sql += " and billcode in (select code from T_BillingApplication where truckcode='" + strTruckCode + "') ";
        }
        //审核状态
        string strBillStatus = this.ddlBillStatus.SelectedValue.Trim();
        if (!strBillStatus.Equals(""))
        {
            switch (strBillStatus)
            {
                case "end": sql += " and stepID='end' "; break;
                case "-1": sql += " and stepID='-1' and recordcount=0"; break;//未提交
                case "0": sql += " and stepID='-1' and recordcount!=0 and rdState!='3' "; break;//审核中 
                case "1": sql += " and stepID='-1' and recordcount!=0 and rdState='3'"; break;//审核驳回
                default:
                    break;
            }
        }
        #endregion
        if (ispock)
        {
            //获取分页行数
            string countsql = "select count(*) from (" + sql + ") t";
            PaginationToGV1.RowsCount = server.ExecuteScalar(countsql).ToString();
            PaginationToGV1.PageIndex = "1";
        }

        int[] ArrRows = ComputeRow(Convert.ToInt32( PaginationToGV1.PageIndex==""?"1":PaginationToGV1.PageIndex));

        string cxsql = " select * from ( " + sql + ")t where t.crow >= " + ArrRows[0] + " and t.crow < " + ArrRows[1];

        DataTable dt = server.RunQueryCmdToTable(cxsql);
        myGrid.DataSource = dt;
        myGrid.DataBind();



    }
    //查询
    protected void btn_cx_Click(object sender, EventArgs e)
    {
        HiddenField1.Value = "1";
        Bind(true);
    }
    protected void PaginationToGV1_DataBinding(object sender, EventArgs e)
    {
        Bind(false);
    }
    protected int[] ComputeRow(int pageIndex)
    {
        int[] ret = new int[2];
        int pagRows = Convert.ToInt32(ConfigurationManager.AppSettings["ItemNumPerPage"]);
        ret[0] = (pageIndex - 1) * pagRows;
        ret[1] = pageIndex * pagRows;
        return ret;
    }
    //删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string bh = HiddenBh.Value;
        if (bh != "")
        {
            int row = bll.Delete(bh);
            if (row>0)
            {
                Bind(true);
            }
           
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string zt = e.Item.Cells[4].Text;
            if (zt == "end")
            {
                e.Item.Cells[4].Text = "审批通过";
            }
            else
            {   //状态(0,等待;1,正在执行;2,通过;3,废弃)
                string billcode = e.Item.Cells[0].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                string state = bll.WFState(billcode);
                e.Item.Cells[4].Text = state;
            }
            string strBillCode = e.Item.Cells[0].Text;
            e.Item.Cells[5].Text = new T_BillingApplicationBll().GetTruckStrByBillCode(strBillCode);
        }
    }
    //分页
    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        Bind(false);
    }
}
