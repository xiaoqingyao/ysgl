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
using Dal.Loan;

public partial class SaleBill_BorrowMoney_FundBorrowHK : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();

    string strCtrl = "";
    string strCode = "";//单据编号
    string strUserCode = "";
    string strhkje = "";//如果有传过来的还款金额就用这个金额作为默认还款金额
    string strBillCode = "";//对应报销单的编号
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
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            object objCode = Request["Code"];
            if (objCode != null)
            {
                strCode = objCode.ToString();
            }
            object objHkje = Request["je"];
            if (objHkje != null)
            {
                strhkje = objHkje.ToString();
            }
            //对应报销单的主键
            object objbillcode = Request["billcode"];
            if (objbillcode != null)
            {
                strBillCode = objbillcode.ToString();
                strBillCode = strBillCode.Substring(0, strBillCode.IndexOf("|*|"));
            }
            if (!IsPostBack)
            {
                bindData();
            }

        }
    }

    private void bindData()
    {
        BindModel();
        BindBills();
    }

    private void BindBills()
    {
        GridView1.DataSource = getbycodemodel(strCode);
        GridView1.DataBind();
    }

    private object getbycodemodel(string strCode)
    {
        string strLoaner = SubString(txtloanName.Text.Trim());
        string strsql = "";

        int count = Convert.ToInt32(server.GetCellValue("select count(*) from  T_ReturnNote where  loancode ='" + strCode + "'"));
        if (count == 0)
        {
            return null;
        }
        strsql = "select  loancode,je ,case ltype when '2' then '现金' when '1' then '单据冲减'  end as ltype,billcode,ldate,note1 from T_ReturnNote where  loancode ='" + strCode + "' ";
        return server.RunQueryCmdToTable(strsql);
    }
    public void BindModel()
    {
        T_LoanList modeljk = loanbll.GetModel(strCode);
        if (modeljk != null)
        {
            decimal ycj = 0;
            decimal je = 0;
            lbjkcode.Text = strCode;
            txtlb.Text = modeljk.Listid;
            txtloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.ResponsibleCode + "'");
            txtlb.Text = server.GetCellValue("select dicname  from bill_datadic where dictype='20' and diccode='" + modeljk.NOTE6 + "' ");
            txtdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            txtjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            txtaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            txtmoney.Text = Convert.ToDecimal(modeljk.LoanMoney).ToString("N02");
            txtjkts.Text = modeljk.NOTE4;
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
            txt_hkje.Text = strhkje.Equals("") ? (je - ycj).ToString() : strhkje;
            hfje.Value = je.ToString();
            hfycj.Value = ycj.ToString();
        }

    }
    /// <summary>
    /// 还款
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        string hkje = txt_hkje.Text.Trim();//还款金额
        decimal deHkje = 0;
        if (string.IsNullOrEmpty(hkje))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请填写还款金额！');", true);
            return;
        }
        if (!decimal.TryParse(hkje, out deHkje))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('还款金额输入不合法，请用阿拉伯数字表示！');", true);
            return;
        }
        decimal cjmoney = 0;
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        string date = DateTime.Now.ToString("yyyyMMdd");
        string mainCode = new Bll.PublicServiceBLL().GetBillCode("hksq", date, 1, 3);
        cjmoney = Convert.ToDecimal(hkje);
        decimal deLoanMoney = decimal.Parse(txtmoney.Text.Trim());

        string strstepid = strhkje.Equals("") ? "-1" : "end";
        string strstepid2 = strhkje.Equals("") ? "0" : "1";

        //resion
        string strReasion = txtReasion.Value.Trim();

        list.Add(" insert T_ReturnNote(ltype,loancode,billcode,ldate,je,usercode,note1,note2,note4) values('2','" + lbjkcode.Text.Trim() + "','" + mainCode + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + cjmoney + ",'" + Session["userCode"].ToString() + "','" + strstepid2 + "','" + strReasion + "','" + strBillCode + "') ");

        list.Add(" insert into bill_main(billcode,billname,flowid,stepid,billUser,billDate,billDept,billJe,LoopTimes,billName2) values('" + mainCode + "','还款申请单','hksq','" + strstepid + "','" + SubString(txtloanName.Text.Trim()) + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + SubString(txtdeptname.Text.Trim()) + "','" + hkje + "','0','" + lbjkcode.Text.Trim() + "')");

        //string loancode = new T_LoanListDal().GetMainCode(strCode);
        T_LoanList loan = new T_LoanListDal().GetModel(strCode);
        decimal ycj = string.IsNullOrEmpty(loan.NOTE3) ? 0 : Convert.ToDecimal(loan.NOTE3);
        string strzt = "1";
        if (loan.LoanMoney <= ycj + deHkje)
        {
            strzt = "2";
        }
        //如果有默认的还款金额传进来  那么就是生成审批通过的还款单
        if (!string.IsNullOrEmpty(strhkje))
        {
            list.Add("update  T_LoanList set  note3=cast(isnull(note3,0) as decimal(18,2))+" + hkje + " ,Status='" + strzt + "' where listid='" + strCode + "'");
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('还款失败！');", true);
        }
        else
        {
            string strscript = "alert('保存成功！');window.returnValue='back';self.close();";
            if (!string.IsNullOrEmpty(strhkje))
            {
                strscript = "alert('保存成功！');self.close();";
            }
            ClientScript.RegisterStartupScript(this.GetType(), "", strscript, true);
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string zt = e.Row.Cells[5].Text;
            string state = "";
            if (zt == "1")
            {
                state = "审批通过";
            }
            else
            {
                string billcode = e.Row.Cells[4].Text;
                WorkFlowRecordManager bll = new WorkFlowRecordManager();
                state = bll.WFState(billcode);
            }
            e.Row.Cells[5].Text = state;
        }
    }
}
