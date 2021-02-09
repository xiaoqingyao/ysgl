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
using System.Collections.Generic;

public partial class SaleBill_BorrowMoney_LoanListPrint : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    string strBillCode = "";
    string strCtrl = "";
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

                if (Request["Ctrl"].ToString() != "" && Request["Code"].ToString() != null && Request["Code"].ToString() != "")
                {
                    strBillCode = Request["Code"].ToString();
                    strCtrl = Request["Ctrl"].ToString();
                    datebind();
                }
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public void datebind()
    {

        if (strCtrl == "print" || strCtrl == "loancj")
        {
            if (strBillCode != "" && strBillCode != null)
            {
                getmodel();
                if (strCtrl == "loancj")
                {
                    this.divprint.Visible = false;

                    trcjcz.Visible = true;
                    this.trgride.Visible = true;

                    GridView1.DataSource = getbycodemodel(strBillCode);
                    GridView1.DataBind();
                    for (int i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        this.GridView1.Rows[i].Cells[2].Text = "<a href=# onclick=\"openDetail('" + this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "');\">" + this.GridView1.Rows[i].Cells[2].Text.ToString().Trim() + "</a>";
                    }
                }
                if (strCtrl == "print")
                {
                    this.divprint.Visible = true;
                    this.trcjcz.Visible = false;
                    this.trgride.Visible = false;
                }

            }

        }

    }

    public DataTable getbycodemodel(string billcode)
    {
        string strKeBaoXiaoCode = server.GetCellValue("select NOTE2 from T_LoanList where listid='" + billcode + "'");
        string[] arrKeBaoXiaoCode = strKeBaoXiaoCode.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string strRel = "";
        for (int i = 0; i < arrKeBaoXiaoCode.Length; i++)
        {
            strRel += "'";
            strRel += arrKeBaoXiaoCode[i];
            strRel += "',";
        }
        if (strRel != "")
        {
            strRel = strRel.Substring(0, strRel.Length - 1);
        }
        //可报销科目
        if (strRel.Equals(""))
        {
            return null;
        }
        //借款人
        string strLoaner = lbloanName.Text;
        if (strLoaner == "")
        {
            return null;
        }
        strLoaner = strLoaner.Substring(1, strLoaner.IndexOf("]") - 1);

        bool boChongJianByDept = new Bll.ConfigBLL().GetValueByKey("ChongJianByPersionOrDept").Equals("1");
        string strsql = "";
        if (boChongJianByDept)//通过部门冲减
        {
            Bll.UserProperty.UserMessage user = new Bll.UserProperty.UserMessage(strLoaner);
            string strdept = user.GetRootDept().DeptCode;
            strsql = @"select b.*,a.billname, a.billUser ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=b.fykm) as fykmname  from bill_main a inner join bill_ybbxmxb_fykm  b on a.billCode=b.billCode and a.billDept='" + strdept + "' and b.status<>'2' and fykm in(" + strRel + ") and b.je!=0  and a.stepid='end'";
        }
        else
        {//通过人员冲减
            strsql = @"select b.*,  a.billname, a.billUser ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=b.fykm) as fykmname  from bill_main a inner join bill_ybbxmxb_fykm  b on a.billCode=b.billCode and a. billUser='" + strLoaner + "' and b.status<>'2' and fykm in(" + strRel + ") and b.je!=0 and a.stepid='end'";
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
            datebind();
            return;
        }
        for (int i = 0; i <= this.GridView1.Rows.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.GridView1.Rows[i].FindControl("CheckBox2");
            if (chk.Checked)
            {
                strBxdCode += this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + ",";
                strBxdCode2 += "'";
                strBxdCode2 += this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "',";
                if (this.GridView1.Rows[i].Cells[3].Text.ToString().Trim() != null)
                {
                    cjmoney += decimal.Parse(this.GridView1.Rows[i].Cells[3].Text.ToString().Trim());
                }
            }
        }

        strBxdCode2 = strBxdCode2.Substring(0, strBxdCode2.Length - 1);
        strBxdCode = strBxdCode.Substring(0, strBxdCode.Length - 1);
        decimal deLoanMoney = decimal.Parse(lblmoney.Text.Trim());
        decimal zcjmoney = cjmoney;
        if (!string.IsNullOrEmpty(lblycjje.Text))
        {
            zcjmoney = decimal.Parse(lblycjje.Text.Trim()) + cjmoney;
        }

        if (deLoanMoney <= zcjmoney)
        {//借款金额小于等于已经冲减金额加上将要冲减金额时 状态改为2
            // NOTE3=cast(note3 as decimal(18,2))+100 
            list.Add("update T_LoanList set Status='2',CJCode='" + strBxdCode + "', NOTE3=cast(isnull(NOTE3,0) as decimal(18,2))+" + cjmoney + " where Listid='" + Request["Code"].ToString() + "'");
        }
        else
        {
            list.Add("update T_LoanList set Status='3',CJCode='" + strBxdCode + "', NOTE3=cast(isnull(NOTE3,0) as decimal(18,2))+" + cjmoney + "  where Listid='" + Request["Code"].ToString() + "'");
        }
        list.Add("update bill_ybbxmxb_fykm set status='2' where billCode in (" + strBxdCode2 + ")");
        string strcode = strBxdCode;
        string[] attrcode = strcode.Split(',');
        if (attrcode.Length > 0)
        {
            for (int i = 0; i < attrcode.Length; i++)
            {
                list.Add("  insert T_ReturnNote(ltype,loancode,billcode,ldate,je,usercode) values('1','" + lblodercode.Text + "','" + attrcode[i] + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + cjmoney + ",'" + Session["userCode"].ToString() + "') ");

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
    /// <summary>
    /// 获取model
    /// </summary>
    public void getmodel()
    {


        if (Request["Code"].ToString() != null && Request["Code"].ToString() != "")
        {
            strBillCode = Request["Code"].ToString();
        }
        if (Request["je"].ToString() != null && Request["je"].ToString() != "")
        {
            this.lblmoney.Text = Request["je"].ToString();
        }
        else
        {
            this.lblmoney.Text = "0.00";
        }
        IList<T_LoanList> loanModel = loanbll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        if (loanModel[0].LoanDeptCode.Trim() != "" && loanModel[0].LoanDeptCode.Trim() != null)
        {
            string strdeptname = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  deptcode='" + loanModel[0].LoanDeptCode.Trim() + "'");
            this.lbldept.Text = strdeptname;//借款人单位
        }

        this.lblodercode.Text = loanModel[0].Listid.ToString();
        if (loanModel[0].LoanCode != "" && loanModel[0].LoanCode != null)
        {
            string strsqlloanName = server.GetCellValue("select '['+userCode+']'+userName from bill_users where userCode='" + loanModel[0].LoanCode + "'");
            this.lbloanName.Text = strsqlloanName.Trim();

        }

        this.lblloandate.Text = loanModel[0].LoanDate;
        this.lblmoney.Text = loanModel[0].LoanMoney.ToString().Trim();
        if (loanModel[0].NOTE2 != "" && loanModel[0].NOTE2 != null)
        {
            string strsqlyskm = server.GetCellValue("select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode='" + loanModel[0].NOTE2 + "'");
            this.lbkmcode.Text = strsqlyskm.Trim();
        }
        if (loanModel[0].Status == "3")
        {
            this.lblStatus.Text = "冲减中";
        }
        else if (loanModel[0].Status == "1")
        {
            this.lblStatus.Text = "借款";
        }
        else if (loanModel[0].Status == "2")
        {
            this.lblStatus.Text = "结算完毕";

        }
        else
        {
            this.lblStatus.Text = "未知状态";
        }

        if (loanModel[0].SettleType == "0")
        {
            this.lbljstype.Text = "现金";
        }
        else if (loanModel[0].SettleType == "1")
        {
            this.lbljstype.Text = "单据冲减";
        }
        else
        {
            this.lbljstype.Text = "";

        }
        if (modelBillMain.StepId.ToString() == "-1")
        {
            this.lblSPStatus.Text = "未提交";
        }
        else if (modelBillMain.StepId.ToString() == "end")
        {
            this.lblSPStatus.Text = "审核通过";
        }
        else
        {
            this.lblSPStatus.Text = "";
        }
        this.lblCjPocode.Text = loanModel[0].CJCode.Trim();
        this.lblycjje.Text = loanModel[0].NOTE3.Trim();
        if (loanModel[0].ResponsibleCode != "" && loanModel[0].ResponsibleCode != null)
        {
            string strRepName = server.GetCellValue("select '['+userCode+']'+userName from bill_users where userCode='" + loanModel[0].ResponsibleCode.Trim() + "'");
            this.lblRepsonName.Text = strRepName;
        }

        this.lblRepsonDate.Text = loanModel[0].ResponsibleDate.Trim();



    }
}
