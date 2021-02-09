using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Dal.SysDictionary;
using Bll.FeeApplication;
using Models;
using System.Text;
using Bll;
using Bll.Bills;

public partial class travelApplicationPrint : System.Web.UI.Page
{
    bill_travelApplicationBLL bllTravelApplication = new bill_travelApplicationBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    string strReportBillCode = "";
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
            if (!IsPostBack)
            {
                this.bindData();
                this.initControl();
            }
        }
    }

    #region 页面数据绑定

    private void initControl()
    {
        //如果是查看页面显示各个领导签字，编辑页面不显示
        bool showManagerMind = false;
        if (strCtrl.Equals("View"))
        {
            showManagerMind = true;
        }
        else
        {
            showManagerMind = false;
        }
        this.tr1.Visible = this.tr2.Visible = this.tr3.Visible = this.tr4.Visible = showManagerMind;
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
            if (modelBillMain == null || lstmodelTravelApplication == null || lstmodelTravelApplication.Count == 0)
            {
                throw new Exception("对不起，单据相关数据丢失！");
            }
            this.lbeAppPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);
            this.lbeBillCode.Text = modelBillMain.BillCode;
            this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
            this.lbeAppDate.Text = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.lbeFeePlan.Text = lstmodelTravelApplication[0].needAmount.ToString();
            this.lbePlanDate.Text = lstmodelTravelApplication[0].travelDate;
            this.lbeReasion.Text = lstmodelTravelApplication[0].reasion;
            this.lbeTransport.Text = lstmodelTravelApplication[0].Transport;
            this.lbeTravelAddress.Text = lstmodelTravelApplication[0].arrdess;
            this.lbeWorkPlan.Text = lstmodelTravelApplication[0].travelplan;
            if (lstmodelTravelApplication[0].MoreThanStandard == 1)
            {
                IsOutStradard.Text = "是";
            }
            else
            {
                IsOutStradard.Text = "否";
            }
            this.lbeTravelType.Text = this.getTravelTypeByCode(lstmodelTravelApplication[0].typecode);
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
                user.UserPosition = dtRel.Rows[0]["userPosition"].ToString();
                listUser.Add(user);
            }
            this.DataGrid1.DataSource = listUser;
            this.DataGrid1.DataBind();
            #region 为出差报告单赋值
            strReportBillCode = new bill_travelApplicationBLL().GetReportCodeByAppCode(strBillCode);
            if (!strReportBillCode.Equals(""))
            {
                Bill_Main modelBillMain2 = bllBillMain.GetModel(strReportBillCode);
                Bill_TravelReport modelTravelReport = new Bill_TravelReportBLL().GetModel(strReportBillCode);
                this.txtRel.Text = modelTravelReport.Result;
                this.txtRepDate.Text = new PublicServiceBLL().cutDt(modelBillMain2.BillDate.ToString());
                this.txtTravelProcess.Text = modelTravelReport.TravelProcess;
                this.txtWorkProcess.Text = modelTravelReport.WorkProcess;
                this.lbeReportBillCode.Text = modelBillMain2.BillCode;
                this.lbeDeptForReport.Text = this.getShowDeptMsgByCode(modelBillMain2.BillUser);
                this.lbeRepPersion.Text = this.getShowUserByCode(modelBillMain2.BillUser);
            }
            else {
                this.tr21.Visible =
                    this.tr22.Visible =
                    this.tr23.Visible =
                    this.tr24.Visible =
                    this.tr25.Visible = this.tr26.Visible = false;
            }
            #endregion
        }
    }
    #endregion


    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }

    /// <summary>
    /// 通过user的code获取DT
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DataTable returnUserMsgDt(string userCode)
    {
        return server.GetDataSet("select userName,userPosition,(select deptName from bill_departments where deptCode=bill_users.userDept) as userDept from bill_users  where userCode='" + userCode + "'").Tables[0];//userStatus='1' and 
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
    /// <summary>
    /// 通过出差类型编号获取出差类型
    /// </summary>
    /// <param name="strTypeCode"></param>
    /// <returns></returns>
    private string getTravelTypeByCode(string strTypeCode) {
        if (strTypeCode.Equals(""))
        {
            return "";
        }
        return server.GetCellValue("select dicName from bill_dataDic where dicType='06' and dicCode='" + strTypeCode + "'");
    }
}
