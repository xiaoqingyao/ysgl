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

public partial class webBill_bxgl_yksqDetailPrint : System.Web.UI.Page
{
    string billCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {

        billCode = Request["billCode"];
        if (!string.IsNullOrEmpty(billCode))
        {
            BindData();
        }
    }
    
     private void BindData(){
     
     DataTable dt=server.GetDataTable("select * from bill_yksq where billCode='"+billCode+"'",null);
     if (dt.Rows.Count>0)
     { 
         date.Text =Convert.ToDateTime( dt.Rows[0]["ddefine0"]).ToString("yyyy年MM月dd日");
         dept.Text = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptCode='" + dt.Rows[0]["billDept"].ToString() + "'"); 
         yt.Text = dt.Rows[0]["yt"].ToString();
         je.Text =string.Format("{0:N}",Convert.ToDecimal(dt.Rows[0]["je"]));  
         dh.Text = dt.Rows[0]["rkCodes"].ToString();
         jbr.Text = server.GetCellValue("select  userName from bill_users where usercode='" + dt.Rows[0]["jbr"].ToString() + "'"); 
     }
      
     
     
     }
}
