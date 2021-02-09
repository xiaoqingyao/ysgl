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
using Dal.SysDictionary;
using Models;
using System.Collections.Generic;
using Bll;
using Bll.FeeApplication;
using System.Text;
using Bll.UserProperty;

public partial class webBill_fysq_travelReportDetail2 : System.Web.UI.Page
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
                this.initControl();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        }
    }
    #region 页面控件/数据绑定
    private void initControl()
    {
        bool boIsView = strCtrl.Equals("View");
        this.btn_ok.Visible = btn_cancel.Visible = this.tr1.Visible = tr2.Visible = tr3.Visible = tr4.Visible = boIsView;
        this.btn_bc.Visible = !boIsView;
        if (strCtrl.Equals("Add") || strCtrl.Equals("Edit"))
        {
            txtTitle.Enabled = txtContent.Enabled = true;
        }
        else if (strCtrl.Equals("View"))
        {
            this.btn_ok.Visible = btn_cancel.Visible = btn_bc.Visible = false;
            this.ddlReportType.Enabled = false;
            txtTitle.Enabled = txtContent.Enabled = false;
        }
        else if (true)
        {
            this.btn_ok.Visible = false;
            btn_cancel.Visible = true;
            btn_ok.Visible = true;
        }
        else
        {
            txtTitle.Enabled = txtContent.Enabled = false;
        }
        if (strCtrl.Equals("Edit"))
        {
            ddlReportType.Enabled = false;
        }
    }
    private void bindData()
    {
        DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='06' order by dicCode");
        if (temp != null)
        {
            this.ddlTravelType.DataSource = temp;
            this.ddlTravelType.DataTextField = "dicName";
            this.ddlTravelType.DataValueField = "dicCode";
            this.ddlTravelType.DataBind();
        }
        DataTable dtReportType = server.GetDataTable("select * from bill_dataDic where dicType='11' order by dicCode", null);
        if (dtReportType != null)
        {
            this.ddlReportType.DataSource = dtReportType;
            this.ddlReportType.DataTextField = "dicName";
            this.ddlReportType.DataValueField = "dicCode";
            this.ddlReportType.DataBind();
        }
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
        IList<Bill_TravelApplication> lstmodelTravelApplication = new bill_travelApplicationBLL().GetListByBillCode(strAppCode);
        Bill_Main modelBillMain2 = bllBillMain.GetModel(strAppCode);
        if (lstmodelTravelApplication.Count == 0)
        {
            return;
        }
        if (strCtrl.Equals("Add"))
        {
            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            //if (isTopDept("y", Session["userCode"].ToString().Trim()))
            //{
            //    string dept2 = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    this.lbeDept.Text = dept2;
            //}
            //else
            //{
            //    //所在部门
            //    string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    //上级部门
            //    string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
            //    this.lbeDept.Text = Dept;
            //}
            DateTime dt = System.DateTime.Now;
            this.txtRepDate.Value = dt.ToString("yyyy-MM-dd");
            this.lbeRepPersion.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + modelBillMain2.BillUser + "'");
            this.CreateCode();
            string strGetsendDeptmanager = "select '['+usercode+']'+userName from bill_users where usercode=(select top 1 usercode from bill_dept_ywzg where deptcode='" + lstmodelTravelApplication[0].sendDept + "')";
            object objsenddeptmanager = server.GetCellValue(strGetsendDeptmanager, null);
            if (objsenddeptmanager != null)
            {
                this.txtsendDeptManager.Text = objsenddeptmanager.ToString();
            }
            string strGetDeptmanager = "select '['+usercode+']'+userName from bill_users where usercode=(select top 1 usercode from bill_dept_ywzg where deptcode='" + modelBillMain2.BillDept + "')";
            object objdeptmanager = server.GetCellValue(strGetDeptmanager, null);
            if (objdeptmanager != null)
            {
                this.txtdeptManager.Text = objdeptmanager.ToString();
            }
        }
        else if (strCtrl == "Edit" || strCtrl.Equals("View"))
        {

            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(strBillCode);

            //this.txtRel.Text = modelTravelReport.Result;
            this.txtRepDate.Value = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.txtTitle.Text = modelTravelReport.TravelProcess;
            this.txtContent.Text = modelTravelReport.WorkProcess;
            //this.lbeBillCode.Text = modelBillMain.BillCode;
            this.txtReportCode.Text = modelBillMain.BillCode;
            //this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
            this.lbeRepPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);

            this.txtdeptManager.Text = this.getShowUserByCode(modelTravelReport.Note2);
            this.txtsendDeptManager.Text = this.getShowUserByCode(modelTravelReport.Note3);
            this.txtspecialCheck.Text = modelTravelReport.Note4;
            this.Lafilename.Text = modelTravelReport.AttachmentName;
            this.hiddFileDz.Value = modelTravelReport.Attachment;
            if (modelTravelReport.Attachment != "")
            {
                Literal1.Text = "<a href='../../Uploads/ccbg/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
            }
            if (strCtrl.Equals("View"))
            {
                this.btn_bc.Visible = false;
                this.btn_ok.Visible = false;
                this.btn_cancel.Visible = false;
            }
            try
            {
                this.ddlReportType.SelectedValue = modelTravelReport.Note1.Trim();
            }
            catch (Exception)
            {
                throw new Exception("为报告类型赋值时出现异常，查看是否在数据字典中删除了对应的出差类型！");
            }
        }
        else if (strCtrl.Equals("look"))
        {
            Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
            Bill_TravelReport modelTravelReport = this.bllTravelReport.GetModel(strBillCode);

            //this.txtRel.Text = modelTravelReport.Result;
            this.txtRepDate.Value = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
            this.txtTitle.Text = modelTravelReport.TravelProcess;

            this.txtContent.Text = modelTravelReport.WorkProcess;
            //this.lbeBillCode.Text = modelBillMain.BillCode;
            txtReportCode.Text = modelBillMain.BillCode;
            //this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
            this.lbeRepPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);
            this.Lafilename.Text = modelTravelReport.AttachmentName;
            this.hiddFileDz.Value = modelTravelReport.Attachment;
            if (modelTravelReport.Attachment != "")
            {
                Literal1.Text = "<a href='../Uploads/ccbg/" + System.IO.Path.GetFileName(hiddFileDz.Value) + @"' target='_blank'  >下载</a> ";
            }
            this.btn_bc.Visible = false;
            this.btn_ok.Visible = true;
            this.btn_cancel.Visible = true;
        }
        //为出差申请单的信息赋值

        #region 为出差申请单赋值
        this.lbeAppCode.Text = modelBillMain2.BillCode;
        string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + modelBillMain2.BillDept + "'");
        this.lbeAppDept.Text = dept;
        this.lbeAppPersion.Text = this.getShowUserByCode(modelBillMain2.BillUser);
        this.lbeAppCode.Text = modelBillMain2.BillCode;
        //this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain2.BillUser);
        this.txtAppDate.Value = modelBillMain2.BillDate.ToString(); //new PublicServiceBLL().cutDt(modelBillMain2.BillDate.ToString());
        this.txtFeePlan.Text = lstmodelTravelApplication[0].needAmount.ToString();
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
        try
        {
            this.ddlTravelType.SelectedValue = lstmodelTravelApplication[0].typecode;
        }
        catch (Exception)
        {
            throw new Exception("为出差类型赋值时出现异常，查看是否在数据字典中删除了对应的出差类型！");
        }

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
            user.UserPosition = getPositionNameByCode(dtRel.Rows[0]["userPosition"].ToString());
            listUser.Add(user);
        }
        this.DataGrid1.DataSource = listUser;
        this.DataGrid1.DataBind();
        #endregion
    }
    #endregion
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        //报告表
        Bill_TravelReport modelTravelReport = new Bill_TravelReport();
        Bill_Main modelBillMain = new Bill_Main();
        try
        {
            if (strCtrl.Equals("Edit"))
            {
                if (strBillCode.Equals(""))
                {
                    throw new Exception("单号丢失！");
                }
                modelTravelReport = bllTravelReport.GetModel(strBillCode);
                modelBillMain = bllBillMain.GetModel(strBillCode);
            }
            else
            {
                //添加
                string strbillusercode = this.lbeRepPersion.Text.Trim();
                strbillusercode = strbillusercode.Substring(1, strbillusercode.IndexOf("]") - 1);
                modelBillMain.BillName = "出差报告单";
                modelBillMain.BillType = "";
                modelBillMain.BillUser = strbillusercode;
                modelBillMain.FlowId = "ccbg";
                modelBillMain.GkDept = "";
                modelBillMain.IsGk = "";
                modelBillMain.LoopTimes = 0;
                modelBillMain.StepId = "-1";
            }
            string strDeptManager = this.txtdeptManager.Text.Trim();
            string strsendDeptManager = this.txtsendDeptManager.Text.Trim();
            try
            {
                strDeptManager = strDeptManager.Substring(1, strDeptManager.IndexOf(']') - 1);
                strsendDeptManager = strsendDeptManager.Substring(1, strsendDeptManager.IndexOf(']') - 1);
            }
            catch (Exception EX)
            {
                showMessage("部门负责人格式不正确！", false, "");
                return;
            }
            modelTravelReport.Note2 = strDeptManager;
            modelTravelReport.Note3 = strsendDeptManager;
            modelTravelReport.Note1 = this.ddlReportType.SelectedValue;
            modelTravelReport.Note4 = this.txtspecialCheck.Text.Trim();
            modelTravelReport.Attachment = this.hiddFileDz.Value;
            modelTravelReport.AttachmentName = this.Lafilename.Text.Trim();
            //modelBillMain.BillCode = this.lbeBillCode.Text.Trim();
            string strcode = CreateCode();
            if (strcode.Equals(""))
            {
                showMessage("未获取到出差报告单号！", false, "");
                return;
            }
            modelBillMain.BillCode = strcode;//this.txtReportCode.Text.Trim();
            modelTravelReport.MainCode = modelBillMain.BillCode;
            string str_billdate = this.txtRepDate.Value.ToString().Trim();
            DateTime dtBillDate;
            bool boBillDate = DateTime.TryParse(str_billdate, out dtBillDate);
            if (boBillDate)
            {
                modelBillMain.BillDate = dtBillDate;
            }
            else
            {
                throw new Exception("日期格式输入不正确！");
            }
            string strusercode = this.lbeRepPersion.Text.Trim();
            strusercode = strusercode.Substring(1, strusercode.IndexOf("]") - 1);
            string strBillDept = server.GetCellValue("select userDept from bill_users where userCode='" + strusercode.Trim() + "'");
            if (string.IsNullOrEmpty(strBillDept))
            {
                throw new Exception("未发现人员所在单位！");
            }
            modelBillMain.BillDept = strBillDept;
            modelBillMain.BillJe = 0;
            //modelTravelReport.Result = this.txtRel.Text.Trim();
            modelTravelReport.TravelProcess = this.txtTitle.Text.Trim();
            modelTravelReport.WorkProcess = this.txtContent.Text.Trim();

            if (!strBillCode.Equals(""))
            {
                //报告单号不为空 chachu shenqingdanhao
                strAppCode = new bill_travelApplicationBLL().GetAppCodeByReportCode(strBillCode);
            }

            //添加到数据库
            string strMsg = "";
            int iRel = bllTravelReport.Insert(modelBillMain, modelTravelReport, out strMsg);
            if (iRel < 1)
            {
                throw new Exception(strMsg);
            }
            else
            {
                //将报告单绑定到出差申请单
                //if (strCtrl.Equals("Add"))
                //{

                string strMsg2 = "";
                if (new bill_travelApplicationBLL().AddBill(strAppCode, modelBillMain.BillCode, out strMsg2) < 1)
                {
                    throw new Exception("将报告单绑定到出差申请单失败，原因：" + strMsg2);
                }
                else
                {
                    showMessage("保存成功！", true, "1");
                }
                //}
                //else
                //{
                //    showMessage("保存成功！", true, "1");
                //}
            }
        }
        catch (Exception ex)
        {
            showMessage("保存失败，原因：" + ex.Message, true, "");
        }
    }

    /// <summary>
    /// 上传
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_sc_Click(object sender, EventArgs e)
    {
        if (FileUpload1.Visible == true)
        {
            string script;
            if (FileUpload1.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择要上传的附件";
                return;
            }
            else
            {
                try
                {
                    string filePath = FileUpload1.PostedFile.FileName;
                    string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                    string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    ////转换成绝对地址,
                    string serverpath = Server.MapPath(@"~\Uploads\ccbg\") + fileSn + filename.Substring(filename.LastIndexOf("."));
                    ////转换成与相对地址,相对地址为将来访问图片提供
                    string relativepath = @"~\Uploads\ccbg\" + fileSn + filename.Substring(filename.LastIndexOf("."));
                    ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                    FileUpload1.PostedFile.SaveAs(serverpath);
                    ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                    hiddFileDz.Value = relativepath;
                    Lafilename.Text = filename;
                    laFilexx.Text = "上传成功";
                    btn_sc.Text = "修改附件";
                    FileUpload1.Visible = false;
                }
                catch (Exception ex)
                {
                    laFilexx.Text = ex.ToString();
                }
            }
        }
        else
        {
            btn_sc.Text = "上传";
            laFilexx.Text = "";
            FileUpload1.Visible = true;
            Lafilename.Text = "";
        }
    }

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
    protected void ddlReportTypeSelectedIndexChanged(object sender, EventArgs e)
    {
        string strCode = this.txtReportCode.Text.Trim();
        string strTypeCode = this.ddlReportType.SelectedValue;
        if (strCode.Equals("") || strTypeCode.Equals(""))
        {
            return;
        }
        strCode = strTypeCode + strCode.Substring(2);
        this.txtReportCode.Text = strCode;
    }
    /// <summary>
    /// 查询是不是二级单位
    /// </summary>
    /// <param name="strus">是人员CODE？y:n</param>
    /// <param name="usercode">人员CODE</param>
    /// <returns></returns>
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 生成编号
    /// </summary>
    private string CreateCode()
    {
        //string Code = new DataDicDal().GetYbbxBillName("ccbg", DateTime.Now.ToString("yyyyMMdd"), 1);
        //if (Code == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
        //    this.btn_bc.Visible = false;
        //}
        //else
        //{
        //    this.lbeBillCode.Text = Code;
        //}
        if (strAppCode.Equals(""))
        {
            showMessage("出差申请单号不能为空！", true, ""); return "";
        }
        string strGetJianma = "select deptJianma from bill_departments where deptcode=(select billdept from bill_main where billcode='" + strAppCode + "')";
        object objRel = server.GetCellValue(strGetJianma);
        if (objRel == null)
        {
            showMessage("请先去部门管理维护部门简码，如财务部：CW！", true, ""); return "";
        }
        string strJianma = objRel.ToString();
        if (strJianma.Equals(""))
        {
            showMessage("请先去部门管理维护部门简码，如财务部：CW！", true, ""); return "";
        }
        if (ddlReportType.Items.Count <= 0)
        {
            showMessage("请先去维护出差类型！", true, ""); return "";
        }
        string strType = this.ddlReportType.SelectedValue;
        string strTime = DateTime.Now.ToString("yyyyMM");
        string strGetMaxNum = "select right('00000'+isnull(cast(substring(max(maincode),len(max(maincode))-2,3)+1 as varchar(10)),'1'),3) from bill_travelReport where maincode like '%" + strType + strJianma + strTime + "%'";
        objRel = server.GetCellValue(strGetMaxNum, null);
        if (objRel == null)
        {
            return "";
        }
        else
        {
            string strRel = strType + strJianma + strTime + objRel.ToString();
            string strcheckexit = "select isnull(count(*),0) from bill_travelReport where maincode='" + strRel + "'";
            object objExit = server.GetCellValue(strcheckexit, null);
            if (int.Parse(objExit.ToString()) > 0)
            {
                CreateCode();
            }
            this.txtReportCode.Text = strRel;
            return strRel;
        }
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
    /// 通过user的code获取DT
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DataTable returnUserMsgDt(string userCode)
    {
        return server.GetDataSet("select userName,userPosition,(select deptName from bill_departments where deptCode=bill_users.userDept) as userDept from bill_users  where userCode='" + userCode + "'").Tables[0];//userStatus='1' and 
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }
    private string getPositionNameByCode(string strCode)
    {
        string sql = "select dicName from bill_dataDic where dicType='05' and dicCode='" + strCode + "'";
        string strName = server.GetCellValue(sql);
        return strName;
    }
}
