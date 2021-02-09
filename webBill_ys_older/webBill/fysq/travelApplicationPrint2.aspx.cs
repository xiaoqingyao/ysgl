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
using Bll.FeeApplication;
using Bll.Bills;
using Models;
using System.Collections.Generic;
using Bll;

public partial class webBill_fysq_travelApplicationPrint2 : System.Web.UI.Page
{
    bill_travelApplicationBLL bllTravelApplication = new bill_travelApplicationBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    Bill_TravelReportBLL bllTravelReport = new Bill_TravelReportBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
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
                initControl();
                bindData();
            }
        }
    }
    private void initControl()
    {

    }
    private void bindData()
    {

        if (strCtrl.Equals("View"))
        {
            if (strBillCode.Equals(""))
            {
                return;
            }
            IList<Bill_TravelApplication> lstmodelTravelApplication = bllTravelApplication.GetListByBillCode(strBillCode);
            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            getmodel(lstmodelTravelApplication, modelBillMain);
            //报告单
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(lstmodelTravelApplication[0].ReportCode);
            if (modelTravelReport != null)
            {
                object objType = server.ExecuteScalar("select '['+dicCode+']'+dicName from bill_dataDic where dicType='11' and dicCode='" + modelTravelReport.Note1 + "'");
                if (objType != null)
                {
                    this.ddlReportType.Text = objType.ToString();
                }
                this.txtRepDate.Text = modelTravelReport.MainCode;
                object objRepPersion = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelBillMain.BillUser + "'");
                this.lbeRepPersion.Text = objRepPersion.ToString();
                this.txtRepDate.Text = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
                this.txtTitle.Text = modelTravelReport.TravelProcess;
                this.txtContent.Text = modelTravelReport.WorkProcess;
                object objdeptManager = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelTravelReport.Note2 + "'");
                if (objdeptManager != null)
                {
                    this.txtdeptManager.Text = objdeptManager.ToString();
                }
                object objsendManager = server.ExecuteScalar("select '['+userCode+']'+userName from bill_users where usercode='" + modelTravelReport.Note3 + "'");
                if (objsendManager != null)
                {
                    this.txtsendDeptManager.Text = objsendManager.ToString();
                }
                this.txtspecialCheck.Text = modelTravelReport.Note4;
                this.txtReportCode.Text = modelTravelReport.MainCode;
            }
            else {
                tr_rep_1.Visible = tr_rep_2.Visible = tr_rep_3.Visible
                    = tr_rep_4.Visible
                    = tr_rep_5.Visible
                    = tr_rep_6.Visible
                    = tr_rep_7.Visible
                    = tr_rep_8.Visible
                    = tr_rep_9.Visible
                    = tr_rep_10.Visible
                    = tr_rep_11.Visible=false;
            }
        }
        object objZhiwu = server.GetCellValue("select dicName from bill_datadic where dicType='05' and dicCode=(select userposition from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "')");
        this.lbeZhiWu.Text = objZhiwu == null ? "未设置" : objZhiwu.ToString();
    }

    private void getmodel(IList<Bill_TravelApplication> lstmodelTravelApplication, Bill_Main modelBillMain)
    {

        this.lbeAppPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);

        this.lbeDept.Text = this.getShowNamebyDeptCode(modelBillMain.BillDept);
        this.lbesendDept.Text = getShowNamebyDeptCode(lstmodelTravelApplication[0].sendDept);
        this.txtAppDate.InnerText = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
        this.txtPlanDate.Text = lstmodelTravelApplication[0].travelDate;
        this.txtReasion.Text = lstmodelTravelApplication[0].reasion;
        this.txtTransport.Text = lstmodelTravelApplication[0].Transport;
        this.txtTravelAddress.Text = lstmodelTravelApplication[0].arrdess;
        this.txtWorkPlan.Text = lstmodelTravelApplication[0].travelplan;
        if (lstmodelTravelApplication[0].MoreThanStandard == 1)
        {
            this.rdbYes.Checked = true;
            this.rdbNo.Checked = false;
        }
        else
        {
            this.rdbYes.Checked = false;
            this.rdbNo.Checked = true;
        }
        //各个费用
        this.jiaotongfei.InnerText = lstmodelTravelApplication[0].jiaotongfei.ToString();
        this.zhusufei.InnerText = lstmodelTravelApplication[0].zhusufei.ToString();
        this.yewuzhaodaifei.InnerText = lstmodelTravelApplication[0].yewuzhaodaifei.ToString();
        this.huiyifei.InnerText = lstmodelTravelApplication[0].huiyifei.ToString();
        this.yinshuafei.InnerText = lstmodelTravelApplication[0].yinshuafei.ToString();
        this.qitafei.InnerText = lstmodelTravelApplication[0].qitafei.ToString();

        //为DataGrid赋值
        IList<Bill_Users> listUser = new List<Bill_Users>();
        int iCount = lstmodelTravelApplication.Count;
        for (int i = 0; i < iCount; i++)
        {
            string strUserCode = lstmodelTravelApplication[i].travelPersionCode;
            DataTable dtRel = returnUserMsgDt(strUserCode);
            if (dtRel == null || dtRel.Rows.Count == 0)
            {
                throw new Exception("没有找到出差对应人员，可能基础数据被删除！");
            }
            Bill_Users user = new Bill_Users();
            user.UserCode = strUserCode;
            user.UserName = dtRel.Rows[0]["userName"].ToString();
            user.UserDept = dtRel.Rows[0]["userDept"].ToString();
            user.UserPosition = dtRel.Rows[0]["dicCodeName"].ToString();
            listUser.Add(user);
        }
        this.DataGrid1.DataSource = listUser;
        this.DataGrid1.DataBind();
    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowUserByCode(string userCode)
    {
        return server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + userCode + "'");
    }
    private string getShowNamebyDeptCode(string strDeptCode)
    {
        return server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strDeptCode + "'");
    }
    /// <summary>
    /// 通过user的code获取DT
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DataTable returnUserMsgDt(string userCode)
    {
        return server.GetDataSet(@"select userName,userPosition,
(select deptName from bill_departments where deptCode=a.userDept) as userDept,
(select  '['+dicCode+']'+dicName from bill_dataDic where dicType='05' and dicCode=a.userPosition)
as dicCodeName from bill_users a  where userCode='" + userCode + "'").Tables[0];//userStatus='1' and 
    }
}
