using Bll;
using Bll.Bills;
using Bll.FeeApplication;
using Bll.UserProperty;
using Dal.SysDictionary;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_fysq_DzfpDetail : System.Web.UI.Page
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
                this.initControl();
                this.bindData();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
            ClientScript.RegisterArrayDeclaration("availableTagsdept", GetdetpAll());
        }
    }

    #region 页面数据绑定

    private void initControl()
    {

        if (strCtrl.Equals("look"))
        {
            //btn_bc.Visible = false;

        }

    }
    private void bindData()
    {

        if (strCtrl == "Add")
        {
            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            if (isTopDept("y", Session["userCode"].ToString().Trim()))
            {
                string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                this.lbeDept.Text = dept;
              //  this.lbesendDept.Text = dept;
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");
                this.lbeDept.Text = Dept;
                //this.lbesendDept.Text = Dept;
            }
            DateTime dt = System.DateTime.Now;
            this.txtSqrq.Text = dt.ToString("yyyy-MM-dd");
            this.lbeAppPersion.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'");
             txtbillcode.Text=   new DataDicDal().GetYbbxBillName("dzfp", DateTime.Now.ToString("yyyyMMdd"), 1);
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
        object objZhiwu = server.GetCellValue("select dicName from bill_datadic where dicType='05' and dicCode=(select userposition from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "')");

    }
    public void getmodel()
    {
        string strsql = @" select a.billDept as showdeptcode,a.billName,a.stepID,a.billDate,a.billJe,
                (select '['+userCode+']'+userName from bill_users where userCode=a.billUser) as billUser,b.* 
                from bill_main a ,bill_fpfj  b
                 where a.billCode=b.billCode
                 and a.billCode='" + strBillCode + "' ";
        DataTable dtzb = server.GetDataTable(strsql, null);
        if (dtzb.Rows.Count > 0)
        {
           // lbesendDept.Text =  dtzb.Rows[0]["deptname"].ToString();
            txtSqrq.Text = dtzb.Rows[0]["fprq"].ToString();
            lbeAppPersion.Text = dtzb.Rows[0]["fpusername"].ToString();
            lbeDept.Text = dtzb.Rows[0]["deptname"].ToString();
            txtbz.Text = dtzb.Rows[0]["bz"].ToString();
            txtbillcode.Text = dtzb.Rows[0]["billcode"].ToString();
        }



        StringBuilder sb = new StringBuilder();

        string strzbsql = @"select * from bill_fpfjs where billcode='" + strBillCode + "'";
        DataTable dtzbs = new DataTable();
        dtzbs = server.GetDataTable(strzbsql, null);
        for (int i = 0; i < dtzbs.Rows.Count; i++)
        {
            sb.Append("<tr id=\"tr_").Append(i.ToString()).Append("\" >");

            sb.Append("<td>");
            sb.Append("<input type=\"text\" class=\"baseText \" onblur=\"htjeChange();\" value=\"");
            sb.Append(dtzbs.Rows[i]["fph"].ToString()).Append("\" />"); ;
            sb.Append("</td>");

            sb.Append("<td>");
            sb.Append("<input type=\"text\" class=\"baseText \" onblur=\"htjeChange();\" value=\"");
            sb.Append(dtzbs.Rows[i]["fpdw"].ToString()).Append("\" />"); ;
            sb.Append("</td>");

            sb.Append("<td>");
            sb.Append("<input type=\"text\" class=\"baseText ysje\" onblur=\"htjeChange();\" value=\"");
            sb.Append(dtzbs.Rows[i]["fpje"].ToString()).Append("\" />");
            sb.Append("</td>");

            sb.Append("<td>");
            sb.Append("<input type=\"text\" class=\"baseText \" onblur=\"htjeChange();\" value=\"");
            sb.Append(dtzbs.Rows[i]["bz"].ToString()).Append("\" />");
            sb.Append("</td>");
            sb.Append("</tr>");

            this.body_fpmx.InnerHtml = sb.ToString();
        }

        //如果是查看 就不让保存
        if (strCtrl.Equals("View"))
        {
           // this.btn_bc.Visible = false;

        }
        if (strCtrl.Equals("look"))
        {
           // btn_bc.Visible = false;
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
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dtname"]));
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
        //string str_billuser = lbeAppPersion.Text.Trim();
        //try
        //{
        //    str_billuser = str_billuser.Substring(1, str_billuser.IndexOf(']') - 1);
        //}
        //catch (Exception)
        //{
        //    showMessage("出差人的输入格式不正确！", false, "");
        //    return;
        //}
        //string str_billdate = this.txtSqrq.Text.Trim();
        //string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        //string bm = this.lbeDept.Text.Trim();
        ////电子发票附件

        ////主表
        //Bill_Main modelMainBill = new Bill_Main();
        //string strMsg = "";
        //try
        //{
        //    string billCode = "";
        //    if (string.IsNullOrEmpty(strBillCode))
        //    {
        //        billCode = new DataDicDal().GetYbbxBillName("dzfp", DateTime.Now.ToString("yyyyMMdd"), 1);
        //    }
        //    else
        //    {
        //        billCode = strBillCode;
        //    }

        //    //添加修改
        //    if (strCtrl.Equals("Edit"))
        //    {
        //        if (strBillCode.Equals(""))
        //        {
        //            throw new Exception("单号丢失！");
        //        }

        //        modelMainBill = bllBillMain.GetModel(strBillCode);
        //    }
        //    else
        //    {

        //        //添加
        //        modelMainBill.BillName = "电子发票单";
        //        modelMainBill.BillType = "";
        //        modelMainBill.BillUser = str_billuser;
        //        modelMainBill.FlowId = "dzfp";
        //        modelMainBill.GkDept = "";
        //        modelMainBill.IsGk = "";
        //        modelMainBill.LoopTimes = 0;
        //        modelMainBill.StepId = "end";
        //    }
        //    //出差申请单表


        //    string strAppPersion = this.lbeAppPersion.Text.Trim();
        //    if (strAppPersion.Equals(""))
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请人不能为空！');", true);
        //        return;
        //    }

        //    modelMainBill.BillCode = this.txtSqrq.Text.Trim();

        //    DateTime dtBillDate;
        //    bool boBillDate = DateTime.TryParse(str_billdate, out dtBillDate);
        //    if (boBillDate)
        //    {
        //        modelMainBill.BillDate = dtBillDate;
        //    }
        //    else
        //    {
        //        throw new Exception("日期格式输入不正确！");
        //    }
        //    UserMessage user = new UserMessage(modelMainBill.BillUser);
        //    string strBillDept = "";
        //    //判断是否预算到末级
        //    strBillDept = user.GetDept().DeptCode;
        //    if (string.IsNullOrEmpty(strBillDept))
        //    {
        //        throw new Exception("未发现人员所在单位！");
        //    }
        //    modelMainBill.BillDept = strBillDept;
        //    //====电子发票主表信息

        //    string strfprq = txtSqrq.Text;
        //    string strdeptCode = "";
        //    string strdeptname = "";
        //    if (!string.IsNullOrEmpty(lbesendDept.Text))
        //    {
        //        strdeptCode = lbesendDept.Text.Substring(1, lbesendDept.Text.IndexOf(']') - 1);
        //        strdeptname = lbesendDept.Text.Substring(lbesendDept.Text.IndexOf(']') + 1);
        //    }
        //    string fpusercode = "";
        //    string fpusername = "";

        //    if (!string.IsNullOrEmpty(lbeAppPersion.Text))
        //    {
        //        fpusercode = lbeAppPersion.Text.Substring(1, lbeAppPersion.Text.IndexOf(']') - 1);
        //        fpusername = lbeAppPersion.Text.Substring(lbeAppPersion.Text.IndexOf(']') + 1);
        //    }
        //    string strbz = txtbz.Text;


        //    //===电子发票子表信息

          
        //    if (iRel < 1)
        //    {
        //        throw new Exception(strMsg);
        //    }
        //    else
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,原因：" + ex.Message + "');", true);
        //}
    }
   
    /// <summary>
    /// 添加子项
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Server_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(this.hdUerCode.Value))
        {
            return;
        }
        string strnewUserCode = this.hdUerCode.Value.ToString();

        string[] arrStrUserCode = strnewUserCode.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries);
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

            }
        }

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
           // this.btn_bc.Visible = false;
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
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowDeptMsgByCode(string userCode)
    {
        return server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + userCode + "')");
    }
    private string getShowNamebyDeptCode(string strDeptCode)
    {
        return server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + strDeptCode + "'");
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