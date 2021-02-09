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

public partial class SaleBill_BorrowMoney_FundHKDetail : BasePage
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
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }

            if (!string.IsNullOrEmpty(Request["Code"]))
            {
                Code = Request["Code"];
                strBillCode = server.GetCellValue("select loancode from T_ReturnNote where billcode='" + Code+"'");
            }
            
            if (!IsPostBack)
            {
                bindData();
                if (strCtrl=="look")
                {
                    btn_bc.Visible=false;
                    txt_hkje.Enabled = false;
                    txtReason.Disabled = true;
                    trPz.Visible = true;
                    btn_pz.Visible=false;
                    txt_pz.Disabled=true;
                }
                else if(strCtrl=="pz")
                {
                    trPz.Visible=true;
                    btn_bc.Visible = false;
                    txt_hkje.Enabled = false;
                    txtReason.Disabled = true;
                }
            }

        }
    }

    private void bindData()
    {
        BindModel();
        GridView1.DataSource = getbycodemodel(strBillCode);
        GridView1.DataBind();
    }
    private object getbycodemodel(string strBillCode)
    {
        string strLoaner = SubString(txtloanName.Text.Trim());
        string strsql = "";

        int count = Convert.ToInt32(server.GetCellValue("select count(*) from  T_ReturnNote where  loancode ='" + strBillCode + "'"));
        if (count == 0)
        {
            return null;
        }
        strsql = "select  loancode,je ,case ltype when '2' then '现金' when '1' then '单据冲减'  end as ltype,billcode,ldate ,note1 from T_ReturnNote where  loancode ='" + strBillCode + "' ";
        return server.RunQueryCmdToTable(strsql);
    }
    public void BindModel()
    {
        T_LoanList modeljk = loanbll.GetModel(strBillCode);
        if (modeljk != null)
        {
            decimal ycj = 0;
            decimal je = 0;
            lbjkcode.Text = server.GetCellValue("select billCode from T_ReturnNote where billcode='" + Code+"'"); ;
            txtloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.LoanCode + "'");
            txtlb.Text = server.GetCellValue("select dicname  from bill_datadic where dictype='20' and diccode='" + modeljk.NOTE6 + "' ");
            txtdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            txtjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            txtaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            txtmoney.Text = Convert.ToDecimal(modeljk.LoanMoney).ToString("N02");
            txtjkts.Text = modeljk.NOTE4;
            txtjkdh.Text=modeljk.Listid;
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
            txt_hkje.Text = hfold.Value = server.GetCellValue("select isnull(je,0) from t_returnnote where billcode='" + Code+"'");
            txtReason.Value = server.GetCellValue("select note2 from t_returnnote where billcode='" + Code + "'");
            hfje.Value = je.ToString();
            hfycj.Value = ycj.ToString();
            txt_pz.Value = server.GetCellValue("select isnull(note3,'') from t_returnnote where listid="+Request["id"]);

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
        if (string.IsNullOrEmpty(hkje))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请填写还款金额！');", true);
            return;
        }

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        decimal cjmoney = Convert.ToDecimal(hkje);
        decimal oldmeney = Convert.ToDecimal(hfold.Value.Trim());
        cjmoney = cjmoney - oldmeney;

        // list.Add(" update T_loanList set note3=convert( decimal(18,2),isnull(note3,0))+("+cjmoney+") where listid='"+strBillCode+"'");
        list.Add(" update t_returnnote set je=" + hkje + ",note2='"+this.txtReason.Value.Trim()+"' where billcode='" + Code + "'");
        list.Add(" update bill_main set billje=" + hkje + " where billcode='" + Request["code"] + "'");

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }
    
    protected void btn_pz_Click(object sender,EventArgs e)
    {
        string pzcode=txt_pz.Value;
        int row=server.ExecuteNonQuery("update T_ReturnNote set note3='"+pzcode+"' where listid="+Request["id"]);
        if (row>0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true); 
        }
        else
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
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
