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

public partial class travelApplicationDetails : System.Web.UI.Page
{
    bill_travelApplicationBLL bllTravelApplication = new bill_travelApplicationBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
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
            if (objCtrl!=null)
            {
                strCtrl = objCtrl.ToString();
            }
            object objCode = Request["Code"];
            if (objCode!=null)
            {
                strBillCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();
                this.initControl();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        }
    }

    #region 页面数据绑定

    private void initControl() {
        //TextBox txtAppDate2 = this.FindControl("txtTravelReport") as TextBox;
        //txtAppDate2.Attributes.Add("readonly", "true");
        DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='06' order by dicCode");
        if (temp!=null)
        {
            this.ddlTravelType.DataSource = temp;
            this.ddlTravelType.DataTextField = "dicName";
            this.ddlTravelType.DataValueField = "dicCode";
            this.ddlTravelType.DataBind();
        }
        //如果是查看页面显示各个领导签字，编辑页面不显示
        bool showManagerMind=false;
        if (strCtrl.Equals("View"))
        {
             showManagerMind= true;
        }
        else { 
            showManagerMind= false;
        }
        this.tr1.Visible = this.tr2.Visible = this.tr3.Visible = this.tr4.Visible = false;
        this.btn_ok.Visible = btn_cancel.Visible =  showManagerMind;
        this.btn_bc.Visible =!showManagerMind;
        //附加报告单
        if (strCtrl.Equals("AddBill") || strCtrl.Equals("View"))
        {
            //禁用输入控件
            txtAppDate.Disabled = true;
            this.ddlTravelType.Enabled = false;
            btn_insert.Disabled=true;
            btn_Del.Enabled = false;
            this.txtTravelAddress.Enabled =
                this.txtPlanDate.Enabled =
                this.txtFeePlan.Enabled =
                this.txtTransport.Enabled =
                this.rdbYes.Enabled =
                this.lbeAppPersion.Enabled=
                this.rdbNo.Enabled = false;
            if (strCtrl.Equals("View"))
            {
                btn_bc.Visible = false;
                btn_ok.Visible = false;
                btn_cancel.Visible = false;

            }
        }
        if (strCtrl.Equals("look"))
        {
            btn_bc.Visible = false;
            btn_ok.Visible = true;
            btn_cancel.Visible = true;
        }
        else {
            trAddBill.Visible = false;
        }
    }

    private void bindData()
    {
        this.txtAppDate.Attributes.Add("onfocus", "javascript:setday(this);");
        if (strCtrl == "Add")
        {
            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            if (isTopDept("y", Session["userCode"].ToString().Trim()))
            {
                string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                this.lbeDept.Text = dept;
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                this.lbeDept.Text = Dept;
            }
            DateTime dt = System.DateTime.Now;
            this.txtAppDate.Value = dt.ToShortDateString();

            this.lbeAppPersion.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");

            this.CreateCode();
        }
        else if (strCtrl == "Edit" || strCtrl.Equals("View") || strCtrl.Equals("audit") || strCtrl.Equals("AddBill") || strCtrl.Equals("look"))
        {

            if (strBillCode.Equals(""))
            {
                return;
            }
            getmodel();
        }
        else { }
    }
    public void getmodel() 
    {
        IList<Bill_TravelApplication> lstmodelTravelApplication = bllTravelApplication.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        //if (modelBillMain==null||lstmodelTravelApplication==null||lstmodelTravelApplication.Count==0)
        //{
        //   // ClientScript.RegisterStartupScript(this.GetType(), "", "alert('对不起，单据相关数据丢失！')");
        //    return;  		 
        //}
        this.lbeAppPersion.Text = this.getShowUserByCode(modelBillMain.BillUser);
        this.lbeBillCode.Text = modelBillMain.BillCode;
        this.lbeDept.Text = this.getShowDeptMsgByCode(modelBillMain.BillUser);
        this.txtAppDate.Value = new PublicServiceBLL().cutDt(modelBillMain.BillDate.ToString());
        this.txtFeePlan.Text = lstmodelTravelApplication[0].needAmount.ToString();
        this.txtPlanDate.Text = lstmodelTravelApplication[0].travelDate;
        this.txtReasion.Text = lstmodelTravelApplication[0].reasion;
        this.txtTransport.Text = lstmodelTravelApplication[0].Transport;
        this.txtTravelAddress.Text = lstmodelTravelApplication[0].arrdess;
        this.txtWorkPlan.Text = lstmodelTravelApplication[0].travelplan;
        this.txtTravelReport.Text = lstmodelTravelApplication[0].ReportCode;
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
            throw new Exception("未出差类型赋值时出现异常，查看是否在数据字典中删除了对应的出差类型！");
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
            user.UserPosition = dtRel.Rows[0]["dicCodeName"].ToString();
            listUser.Add(user);
        }
        this.DataGrid1.DataSource = listUser;
        this.DataGrid1.DataBind();
        //如果是查看 就不让保存
        if (strCtrl.Equals("View"))
        {
            this.btn_bc.Visible = false;
            //如果单据审核通过 不显示审核驳回和审核通过
            if (modelBillMain.StepId == "end")
            {
                btn_ok.Visible = btn_cancel.Visible = false;
            }
        } 
        if (strCtrl.Equals("look"))
        {
            btn_bc.Visible = false;
            btn_ok.Visible = true;
            btn_cancel.Visible = true;

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
    #endregion

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdate = this.txtAppDate.Value.ToString().Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string bm = this.lbeDept.Text.Trim();
        //申请表
        Bill_TravelApplication modelTravelApplication = new Bill_TravelApplication();
        //主表
        Bill_Main modelMainBill = new Bill_Main();
        string strMsg = "";
        try
        {
            ////附加单据
            //if (strCtrl.Equals("AddBill"))
            //{
                
            //    string strRepCode = this.txtTravelReport.Text.Trim();
            //    if (strRepCode.Equals(""))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败，原因：附件单号不能为空！');", true);
            //        return;
            //    }
            //    int iaRel = bllTravelApplication.AddBill(strBillCode, strRepCode,out strMsg);
            //    if (iaRel > 0)
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
            //    }
            //    else {
            //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败，原因：" + strMsg + "！');", true);
            //    }
            //    return;
            //}
            //添加修改
            if (strCtrl.Equals("Update"))
            {
                if (strBillCode.Equals(""))
                {
                    throw new Exception("单号丢失！");
                }
                modelTravelApplication = bllTravelApplication.GetModel(strBillCode);
            }
            else
            {
                //添加
                modelMainBill.BillName = "出差管理单";
                modelMainBill.BillType = "";
                modelMainBill.BillUser = str_billuser;
                modelMainBill.FlowId = "ccsq";
                modelMainBill.GkDept = "";
                modelMainBill.IsGk = "";
                modelMainBill.LoopTimes = 0;
                modelMainBill.StepId = "-1";
            }
            //出差申请单表
            modelTravelApplication.arrdess = this.txtTravelAddress.Text.Trim();
            modelTravelApplication.maincode = this.lbeBillCode.Text.Trim();
            modelTravelApplication.MoreThanStandard = this.rdbYes.Checked ? 1 : 0;
            modelTravelApplication.needAmount = int.Parse(this.txtFeePlan.Text.Trim());
            modelTravelApplication.reasion = this.txtReasion.Text.Trim();
            modelTravelApplication.Transport = this.txtTransport.Text.Trim();
            modelTravelApplication.travelDate = this.txtPlanDate.Text.Trim();
            modelTravelApplication.travelPersionCode = this.getPersionStr();
            modelTravelApplication.travelplan = this.txtWorkPlan.Text.Trim();
            if (this.ddlTravelType.SelectedValue == null)
            {
                throw new Exception("出差类型不能为空！");
            }
            modelTravelApplication.typecode = this.ddlTravelType.SelectedValue.Trim();

            modelMainBill.BillCode = this.lbeBillCode.Text.Trim();

            DateTime dtBillDate;
            bool boBillDate = DateTime.TryParse(str_billdate, out dtBillDate);
            if (boBillDate)
            {
                modelMainBill.BillDate = dtBillDate;
            }
            else
            {
                throw new Exception("日期格式输入不正确！");
            }
            string strBillDept = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
            if (string.IsNullOrEmpty(strBillDept))
            {
                throw new Exception("未发现人员所在单位！");
            }
            modelMainBill.BillDept = strBillDept;
            modelMainBill.BillJe = int.Parse(this.txtFeePlan.Text.Trim());
            
            int iRel = bllTravelApplication.AddNote(modelMainBill, modelTravelApplication, out strMsg);
            if (iRel < 1)
            {
                throw new Exception(strMsg);
            }
            else {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,原因：" + ex.Message + "');", true);
        }
    }
    /// <summary>
    /// 添加子项
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Server_Click(object sender, EventArgs e) {
        if (string.IsNullOrEmpty(this.hdUerCode.Value))
        {
            return;
        }
        string strnewUserCode = this.hdUerCode.Value.ToString();
        IList<Bill_Users> listUser = returnListUser();
        string[] arrStrUserCode=strnewUserCode.Split(new string[]{"|&|"},StringSplitOptions.RemoveEmptyEntries);
        int iArrLength = arrStrUserCode.Length;
        for (int i = 0; i < iArrLength; i++)
        {
            strnewUserCode = new PublicServiceBLL().SubCode(arrStrUserCode[i]);
            DataTable dtRel = returnUserMsgDt(strnewUserCode);
            if (dtRel != null && dtRel.Rows.Count > 0)
            {
                bool bo = true;
                Bill_Users user = new Bill_Users();
                user.UserCode = strnewUserCode;
                user.UserName = dtRel.Rows[0]["userName"].ToString();
                user.UserDept = dtRel.Rows[0]["userDept"].ToString();
                user.UserPosition = getPositionNameByCode(dtRel.Rows[0]["userPosition"].ToString());
                for (int j = 0; j < listUser.Count; j++)
                {
                    if (listUser[j].UserCode.Equals(user.UserCode))
                    {
                        bo = false;
                        break;
                    }
                }
                if (bo)
                {
                    listUser.Add(user);
                }
            }
        }
        this.DataGrid1.DataSource = listUser;
        this.DataGrid1.DataBind();
    }
    private string getPositionNameByCode(string strCode) {
        string sql = "select dicName from bill_dataDic where dicType='05' and dicCode='" + strCode + "'";
        string strName = server.GetCellValue(sql);
        return strName;
    }
    /// <summary>
    /// 删除子项
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Del_Click(object sender, EventArgs e)
    {
        IList<Bill_Users> listUser = new List<Bill_Users>();
        int iCount = this.DataGrid1.Items.Count;
        for (int i = 0; i < iCount; i++)
        {
            CheckBox chk = (CheckBox)this.DataGrid1.Items[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                continue;
            }
            Bill_Users user = new Bill_Users();
            user.UserCode = DataGrid1.Items[i].Cells[1].Text;
            user.UserName = DataGrid1.Items[i].Cells[2].Text;
            user.UserDept = DataGrid1.Items[i].Cells[3].Text;
            user.UserPosition = DataGrid1.Items[i].Cells[4].Text;
            listUser.Add(user);
        }
        this.DataGrid1.DataSource = listUser;
        this.DataGrid1.DataBind();
    }
    /// <summary>
    /// 获取人员code拼接字符串
    /// </summary>
    /// <returns></returns>
    private string getPersionStr()
    {
        int iCount = this.DataGrid1.Items.Count;
        StringBuilder persionstrsb=new StringBuilder();
        for (int i = 0; i < iCount; i++)
        {
            persionstrsb.Append("|&|");
            string persioncode = this.DataGrid1.Items[i].Cells[1].Text.Trim();
            persionstrsb.Append(persioncode);
        }
        return persionstrsb.ToString();
    }
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
    /// 生成编号
    /// </summary>
    private void CreateCode()
    {
        string lscgCode = new DataDicDal().GetYbbxBillName("ccsq", DateTime.Now.ToString("yyyyMMdd"), 1);
        if (lscgCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_bc.Visible = false;
        }
        else
        {
            this.lbeBillCode.Text = lscgCode;
        }
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
    /// 返回datagrid中的数据组成的人员信息集合
    /// </summary>
    /// <returns></returns>
    private IList<Bill_Users> returnListUser() {
        IList<Bill_Users> listUser = new List<Bill_Users>();
        int iRows = this.DataGrid1.Items.Count;
        for (int i = 0; i < iRows; i++)
        {
            Bill_Users user = new Bill_Users();
            user.UserCode=DataGrid1.Items[i].Cells[1].Text;
            user.UserName = DataGrid1.Items[i].Cells[2].Text;
            user.UserDept = DataGrid1.Items[i].Cells[3].Text;
            user.UserPosition = DataGrid1.Items[i].Cells[4].Text;

            listUser.Add(user);
        }
        return listUser;
    }
    /// <summary>
    /// 通过user的code获取DT
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DataTable returnUserMsgDt(string userCode) {
        return server.GetDataSet(@"select userName,userPosition,
(select deptName from bill_departments where deptCode=a.userDept) as userDept,
(select  '['+dicCode+']'+dicName from bill_dataDic where dicType='05' and dicCode=a.userPosition)
as dicCodeName from bill_users a  where userCode='" + userCode + "'").Tables[0];//userStatus='1' and 
    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowDeptMsgByCode(string userCode) {
       return  server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + userCode + "')");
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
