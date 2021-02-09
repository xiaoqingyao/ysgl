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
using Bll;

/// <summary>
/// 出差报告单打印
/// </summary>
public partial class webBill_fysq_travelReportprint2 : System.Web.UI.Page
{
    BillMainBLL bllBillMain = new BillMainBLL();
    Bill_TravelReportBLL bllTravelReport = new Bill_TravelReportBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objCtrl = Request["Ctrl"];
        if (objCtrl != null)
        {
            strCtrl = objCtrl.ToString();
        }
        object objCode = Request["Code"];
        if (objCode != null)
        {
            strBillCode = objCode.ToString();
        }
        if (!IsPostBack)
        {
            this.bindData();
        }
    }
    private void bindData()
    {
        if (strCtrl.Equals("print"))
        {
            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(strBillCode);
            object objType = server.ExecuteScalar("select '['+dicCode+']'+dicName from bill_dataDic where dicType='11' and dicCode='" + modelTravelReport.Note1 + "'").ToString();
            if (objType!=null)
            {
                this.ddlReportType.Text = objType.ToString();
            }
            this.txtRepDate.Text = modelTravelReport.MainCode;
            object objRepPersion = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelBillMain.BillUser + "'").ToString();
            this.lbeRepPersion.Text = objRepPersion.ToString();
            this.txtRepDate.Text = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.txtTitle.Text = modelTravelReport.TravelProcess;
            this.txtContent.Text = modelTravelReport.WorkProcess;
            object objdeptManager = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelTravelReport.Note2 + "'").ToString();
            if (objdeptManager!=null)
            {
                this.txtdeptManager.Text = objdeptManager.ToString();
            }
            object objsendManager = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelTravelReport.Note3 + "'").ToString();
            if (objsendManager!=null)
            {
                this.txtsendDeptManager.Text = objsendManager.ToString();
            }
            
            this.txtspecialCheck.Text = modelTravelReport.Note4;
            this.txtReportCode.Text = modelTravelReport.MainCode;
        }
    }
}
