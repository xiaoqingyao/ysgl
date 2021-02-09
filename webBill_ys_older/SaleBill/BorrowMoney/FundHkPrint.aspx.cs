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
using Bll.Bills;
using Models;
using WorkFlowLibrary.WorkFlowBll;

public partial class SaleBill_BorrowMoney_FundHkPrint : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();

    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    string Code = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
           

            if (!string.IsNullOrEmpty(Request["Code"]))
            {
                Code = Request["Code"];
                strBillCode = server.GetCellValue("select loancode from T_ReturnNote where billcode='" + Code + "'");
            }

            if (!IsPostBack)
            {
                bindData();
            }

        }
    }

 
    public void bindData()
    {
        T_LoanList modeljk = loanbll.GetModel(strBillCode);
        if (modeljk != null)
        {
            decimal ycj = 0;
            decimal je = 0;
            lbjkcode.Text = server.GetCellValue("select billCode from T_ReturnNote where billcode='" + Code + "'"); ;
            txtloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.LoanCode + "'");
            txtlb.Text = server.GetCellValue("select dicname  from bill_datadic where dictype='20' and diccode='" + modeljk.NOTE6 + "' ");
            txtdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            txtjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            txtaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            txtmoney.Text = Convert.ToDecimal(modeljk.LoanMoney).ToString("N02");
            txtjkts.Text = modeljk.NOTE4;
            txtjkdh.Text = modeljk.Listid;
            je = Convert.ToDecimal(modeljk.LoanMoney);
            if (!string.IsNullOrEmpty(modeljk.NOTE3))
            {
                lbycj.Text = Convert.ToDecimal(modeljk.NOTE3).ToString("N02");
                ycj = Convert.ToDecimal(modeljk.NOTE3);
            }
            else
            {
                lbycj.Text = "0.00";
            }
            txt_hkje.Text = server.GetCellValue("select isnull(je,0) from t_returnnote where billcode='" + Code + "'");
            txtReason.Text = server.GetCellValue("select note2 from t_returnnote where billcode='" + Code + "'");
        
            txt_pz.Value = server.GetCellValue("select isnull(note3,'') from t_returnnote where listid=" + Request["id"]);

        }

    }
}
