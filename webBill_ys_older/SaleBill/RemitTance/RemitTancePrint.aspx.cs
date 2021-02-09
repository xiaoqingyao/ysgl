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
using Models;
using Bll.Sepecial;
using Bll.Bills;

public partial class SaleBill_RemitTance_RemitTancePrint : System.Web.UI.Page
{
    RemittanceBll spebll = new RemittanceBll();
    BillMainBLL bllBillMain = new BillMainBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                //
                //Ctrl=print&Code=" + billcode
                if (Request["Ctrl"].ToString() == "print" && Request["Code"].ToString() != null && Request["Code"].ToString() != "")
                {
                    getmodel();
                }
            }
        }
       
    }
    public void getmodel()
    {

        string strBillCode = "";

        if (Request["Code"].ToString() != null && Request["Code"].ToString() != "")
        {
            strBillCode = Request["Code"].ToString();
        }
        if (Request["je"].ToString()!=null&&Request["je"].ToString()!="")
        {
            this.lblremitmoney.Text = Request["je"].ToString();
        }
        else
        {
            this.lblremitmoney.Text = "0.00";
        }
        IList<T_Remittance> remitemode = spebll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        this.lbldept.Text = remitemode[0].PaymentDeptName.Trim();
        this.lblodercode.Text = new RemittanceBll().getBillOrderCode(strBillCode);
        this.lblremitnumber.Text = remitemode[0].RemittanceNumber.Trim();
        this.lbltruckcode.Text = new RemittanceBll().getBillTruckCode(strBillCode);
        this.lblremdata.Text = remitemode[0].RemittanceDate.Trim();
        this.lbljxs.Text = remitemode[0].NOTE1.Trim();
        this.lblremtype.Text = remitemode[0].RemittanceType.Trim();
        this.lblremituser.Text = remitemode[0].RemittanceUse.Trim();
        this.lblordertime.Text = remitemode[0].OrderCodeDate.ToString();
        this.lbljidw.Text = remitemode[0].PaymentDeptName.Trim();
        if (remitemode[0].Accessories.ToString() != null && remitemode[0].Accessories.ToString() != "")
        {

            string strAppTemp = string.Format("<a href=\"../../../webBill/" + remitemode[0].Accessories.ToString() + " \" target='_blank'>附件</a>");
            this.lblremitfj.Text = strAppTemp;

        }

    }
}
