using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_MyWorkFlow_BillShDetail : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
            //if (!IsPostBack)
            //{
                BindDataGrid();
           // }
        //}
       
    }
    void BindDataGrid()
    {
       
        DataTable dtrel = GetData();
      
        //----------给gridview赋值
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();
    }
    private DataTable GetData()
    {
        string strBillCode = "";
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            strBillCode = Request["billCode"].ToString();
        }
        List<SqlParameter> lstParameter = new List<SqlParameter>();
        StringBuilder sb = new StringBuilder();
        sb.Append(@" select Row_Number()over(order by w.flowid asc,ws.stepid asc) as crow,w.billcode,billtype,w.flowid,isEdit,w.rdState as wrdstate, stepid,steptext,recordtype,
                  isnull((select '['+usercode+']'+username from bill_users where usercode=ws.checkuser),checkuser) as checkuser,
                  isnull((select '['+usercode+']'+username from bill_users where usercode=ws.finaluser),finaluser) as finaluser,
                  ws.rdstate as wsrdstate,mind, convert(varchar(10),ws.checkdate,121) as checkdate, ws.checkdate as checkdate1,checktype   
                  from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid  ");
        sb.Append(" and  billCode  = @billCode ");
        lstParameter.Add(new SqlParameter("@billCode ", strBillCode));
        return server.GetDataTable(sb.ToString(),lstParameter.ToArray());
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string flowtype = e.Row.Cells[1].Text.ToString();
            string rdStatus = e.Row.Cells[3].Text.ToString();
            string mind = e.Row.Cells[4].Text.ToString();


            string txtStatus = "";
            string txtFlowType = "";

            if (rdStatus == "1")
            {
                txtStatus = "正在进行";
            }
            else if (rdStatus == "0")
            {
                txtStatus = "等待";
            }
            else if (rdStatus == "2")
            {
                txtStatus = "审核通过";
            }
            else if (rdStatus == "3")
            {
                txtStatus = "驳回";
            }
            
            e.Row.Cells[1].Text = txtFlowType;
            e.Row.Cells[3].Text = txtStatus;



            string yj = "";
           
            string[] arr = mind.Split(new string[] { "|&amp;|" }, StringSplitOptions.None);
            if (arr.Length == 0)
            {
                return;
            }
            else if (arr.Length == 1)
            {
                yj = arr[0];
            }
          
           
            e.Row.Cells[4].Text = yj;
         

        }
    }

}