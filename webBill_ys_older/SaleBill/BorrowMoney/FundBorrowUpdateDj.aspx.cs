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
using System.Collections.Generic;

public partial class SaleBill_BorrowMoney_FundBorrowUpdateDj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                string je = server.GetCellValue("select billJe from bill_main where billcode='" + Request["billCode"] + "'");
                Label1.Text = je;
                TextBox1.Text = je;
            }
        }
    }

    protected void Button1_click(object sender, EventArgs e)
    {
        string strje = TextBox1.Text.Trim();
        string billCode = Request["billCode"];
        if (string.IsNullOrEmpty(strje))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请输入金额！');", true);
            return;
        }
        decimal je;
        decimal.TryParse(strje, out je);
        if (je == 0 && strje != "0")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('输入金额不合法！');", true);
            return;
        }



        IList<string> list = new List<string>();
        list.Add("update T_LoanList set LoanMoney='" + je.ToString() + "' where listid='" + billCode + "'");
        list.Add("update bill_main set billJe='" + je.ToString() + "' where billCode='" + billCode + "'");
        int row = server.ExecuteNonQuerys(list.ToArray());
        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改成功！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
}
