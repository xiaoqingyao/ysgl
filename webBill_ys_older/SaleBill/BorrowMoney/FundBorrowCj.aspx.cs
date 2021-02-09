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

public partial class SaleBill_BorrowMoney_FundBorrowCj : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();

    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
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
                strBillCode = objCode.ToString();
                txtlb.Text = strBillCode;
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
        GridView1.DataSource = getbycodemodel(strBillCode);
        GridView1.DataBind();
    }

    private object getbycodemodel(string strBillCode)
    {
        string strLoaner = SubString(txtloanName.Text.Trim());
        bool boChongJianByDept = new Bll.ConfigBLL().GetValueByKey("ChongJianByPersionOrDept").Equals("1");
        string strsql = "";
        if (boChongJianByDept)//通过部门冲减
        {
            Bll.UserProperty.UserMessage user = new Bll.UserProperty.UserMessage(strLoaner);
            string strdept = user.GetRootDept().DeptCode;
            strsql = @"select b.*,a.billname, a.billUser ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=b.fykm) as fykmname  from bill_main a inner join bill_ybbxmxb_fykm  b on a.billCode=b.billCode and a.billDept='" + strdept + "' and b.status<>'2'  and b.je!=0  and a.stepid='end' and a.billDate >='" + txtaddtime.Text.Trim() + "'";
        }
        else
        {//通过人员冲减
            strsql = @"select b.*,  a.billname, a.billUser ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=b.fykm) as fykmname  from bill_main a inner join bill_ybbxmxb_fykm  b on a.billCode=b.billCode and a. billUser='" + strLoaner + "' and b.status<>'2' and b.je!=0 and a.stepid='end' and a.billDate>='" + txtaddtime.Text.Trim() + "'";
        }
        DataTable dt = server.RunQueryCmdToTable(strsql);
        if (dt != null)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }
    public void BindModel()
    {
        T_LoanList modeljk = loanbll.GetModel(strBillCode);
        if (modeljk != null)
        {

            txtloanName.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode ='" + modeljk.LoanCode + "'");
            lbjkcode.Text = server.GetCellValue("select dicname  from bill_datadic where dictype='20' and diccode='" + modeljk.NOTE6 + "' ");
            txtdeptname.Text = server.GetCellValue("select '['+deptcode+']'+deptname from bill_departments where deptcode ='" + modeljk.LoanDeptCode + "'");
            txtjksj.Text = Convert.ToDateTime(modeljk.LoanDate).ToString("yyyy-MM-dd");//借款日期
            txtaddtime.Text = Convert.ToDateTime(modeljk.LoanSystime).ToString("yyyy-MM-dd");
            txtmoney.Text = modeljk.LoanMoney.ToString();
            txtjkts.Text = modeljk.NOTE4;
            txtycj.Text = modeljk.NOTE3.ToString();
        }

    }
    /// <summary>
    /// 冲减操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        decimal cjmoney = 0;
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        string strBxdCode = "";//存入数据库
        string strBxdCode2 = "";//组建sql
        if (this.GridView1.Rows.Count <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('没有合适的报销单可冲减！');", true);
            bindData();
            return;
        }
        for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.GridView1.Rows[i].FindControl("CheckBox2");
            if (chk.Checked)
            {
                strBxdCode += this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + ",";
                strBxdCode2 += "'" + this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "',";
                if (this.GridView1.Rows[i].Cells[3].Text.ToString().Trim() != null)
                {
                    cjmoney += decimal.Parse(this.GridView1.Rows[i].Cells[3].Text.ToString().Trim());
                }
            }
        }
        strBxdCode2 = strBxdCode2.Substring(0, strBxdCode2.Length - 1);
        strBxdCode = strBxdCode.Substring(0, strBxdCode.Length - 1);
        decimal deLoanMoney = decimal.Parse(txtmoney.Text.Trim());
        decimal zcjmoney = cjmoney;
        if (!string.IsNullOrEmpty(txtycj.Text))
        {
            zcjmoney = decimal.Parse(txtycj.Text.Trim()) + cjmoney;
        }
        if (deLoanMoney <= zcjmoney)
        {
            // NOTE3=cast(note3 as decimal(18,2))+100 
            list.Add("update T_LoanList set Status='2',CJCode='" + strBxdCode + "', NOTE3=cast(isnull(NOTE3,0) as decimal(18,2))+" + cjmoney + " where Listid='" + Request["Code"].ToString() + "'");
        }
        else
        {
            list.Add("update T_LoanList set Status='3',CJCode='" + strBxdCode + "', NOTE3=cast(isnull(NOTE3,0) as decimal(18,2))+" + cjmoney + "  where Listid='" + Request["Code"].ToString() + "'");
        }


        list.Add("update bill_ybbxmxb_fykm set status='2' where billCode in (" + strBxdCode2 + ")");
        list.Add("update bill_ybbxmxb set sfdk='1'  where billCode in (" + strBxdCode2 + ")");

        string strcode = strBxdCode;
        string[] attrcode = strcode.Split(',');
        if (attrcode.Length > 0)
        {
            for (int i = 0; i < attrcode.Length; i++)
            {
                list.Add("  insert T_ReturnNote(ltype,loancode,billcode,ldate,je,usercode,note1) values('1','" + txtlb.Text.Trim() + "','" + attrcode[i] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + cjmoney + ",'" + Session["userCode"].ToString() + "','1') ");

            }
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('冲减失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }
}
