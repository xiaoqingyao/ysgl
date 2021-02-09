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
using Models;
using Bll.Bills;
using Bll.FeeApplication;
using Bll;
using System.Collections.Generic;
using System.Text;
using Dal.SysDictionary;

public partial class webBill_fysq_travelReportprint : System.Web.UI.Page
{
    BillMainBLL bllBillMain = new BillMainBLL();
    Bill_TravelReportBLL bllTravelReport = new Bill_TravelReportBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    string strAppCode = "";//申请单单号  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
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
            }
            object objAppCode = Request["AppCode"];
            if (objAppCode != null)
            {
                strAppCode = objAppCode.ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();
                //this.initControl();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        }
    }
    #region 页面控件/数据绑定

    private void bindData()
    {
      
      
         if (strCtrl == "print")
        {
            if (!strAppCode.Equals(""))
            {
                //申请单号不为空
                strBillCode = new bill_travelApplicationBLL().GetReportCodeByAppCode(strAppCode);
            }
            else if (!strBillCode.Equals(""))
            {
                //报告单号不为空
                strAppCode = new bill_travelApplicationBLL().GetAppCodeByReportCode(strBillCode);
            }
            else
            {
                return;
            }
            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(strBillCode);

            this.txtRel.Text = modelTravelReport.Result;
            this.txtRepDate.Text = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.txtTravelProcess.Text = modelTravelReport.TravelProcess;
            this.txtWorkProcess.Text = modelTravelReport.WorkProcess;
            this.lbeBillCode.Text = modelBillMain.BillCode;
            this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
            this.lbeRepPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);
           
        }
        else if (strCtrl.Equals("look"))
        {
            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(strBillCode);

            this.txtRel.Text = modelTravelReport.Result;
            this.txtRepDate.Text = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.txtTravelProcess.Text = modelTravelReport.TravelProcess;
            this.txtWorkProcess.Text = modelTravelReport.WorkProcess;
            this.lbeBillCode.Text = modelBillMain.BillCode;
            this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
            this.lbeRepPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);
         
        }
        //为出差申请单的信息赋值

        if (!strAppCode.Equals(""))
        {
            #region 为出差申请单赋值
            IList<Bill_TravelApplication> lstmodelTravelApplication = new bill_travelApplicationBLL().GetListByBillCode(strAppCode);
            Bill_Main modelBillMain2 = bllBillMain.GetModel(strAppCode);
           

          
            #endregion
        }
    }
    #endregion
  
   
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowDeptMsgByCode(string userCode)
    {
        return server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + userCode + "')");
    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowUserByCode(string userCode)
    {
        return server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + userCode + "'");
    }
    
}
