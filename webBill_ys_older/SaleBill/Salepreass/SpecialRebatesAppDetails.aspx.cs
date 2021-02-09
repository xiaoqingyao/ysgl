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
using Bll;
using Models;
using System.Collections.Generic;
using Bll.Bills;
using System.Text;
using Bll.SaleBill;
using Bll.TruckType;

public partial class SaleBill_Salepreass_SpecialRebatesAppDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    SpecialRebatesAppBLL spebll = new SpecialRebatesAppBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
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
                this.bindData();

            }
            ClientScript.RegisterArrayDeclaration("deptAll", GetDeptAll());

        }
    }

    private void bindData()
    {

        this.txtAppDate.Attributes.Add("onfocus", "javascript:setday(this);");
        this.txtbgtime.Attributes.Add("onfocus", "javascript:setday(this);");
        this.txtendtime.Attributes.Add("onfocus", "javascript:setday(this);");
        //this.initControl();
        if (strCtrl == "Add")
        {
            IList<T_SpecialRebatesAppmode> cxb = new List<T_SpecialRebatesAppmode>();
            cxb.Add(new T_SpecialRebatesAppmode());
            GridView1.DataSource = cxb;
            GridView1.DataBind();
            //有效日期默认当天，申请日期默认当天
            DateTime dt = System.DateTime.Now;
            this.txtAppDate.Text = dt.ToShortDateString();
            this.txtbgtime.Text = dt.ToShortDateString();

            //报告单号
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("tsfl", strneed, 1, 6);
            this.lbeBillCode.Text = strcode;
            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            if (isTopDept("y", Session["userCode"].ToString().Trim()))
            {
                string dept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                // string deptcode = server.GetCellValue("select deptCode  from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //this.lbeDept.Text = dept;
                if (dept != null && dept != "")
                {
                    ddlTravelType.Text = dept;
                    // ddlTravelType.SelectedValue = deptcode;
                }
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                //string strcodes = server.GetCellValue("select deptCode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //上级部门
                string sjDept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "')) and IsSell='Y'");

                //this.lbeDept.Text = Dept;
                if (sjDept != null && sjDept != "")
                {
                    ddlTravelType.Text = sjDept.ToString();
                    //if (strcodes != null && strcodes != "")
                    //{
                    //    ddlTravelType.SelectedValue = strcodes;
                    //}
                }
            }

            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
            this.tr1.Visible = false;
        }
        else if (strCtrl == "Edit")
        {
            if (strBillCode.Equals(""))
            {
                return;
            }
            getmodel();

        }
        else if (strCtrl == "View")
        {
            getmodel();
            getenbel();
            this.tryy.Visible = false;
            this.tridmoney.Visible = false;
            // this.ckid.Visible = false;


        }
        else if (strCtrl == "look")
        {
            getmodel();
            getenbel();
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
            this.btn_bc.Visible = false;
            this.tryy.Visible = false;
            this.tridmoney.Visible = false;
            // this.ckid.Visible = false;

        }
    }
    public void getenbel()
    {
        this.txtAppDate.Enabled = false;//.Attributes.Add("readonly", "true");
        this.ddlTravelType.Enabled = false;
        this.txtReasion.Enabled = false;
        this.txtWorkPlan.Enabled = false;
        this.txtbgtime.Enabled = false;
        this.txtendtime.Enabled = false;
        this.txtFeePlan.Enabled = false;
        this.txtTransport.Enabled = false;
        this.TextBox1.Enabled = false;
        this.TextBox2.Enabled = false;
        this.TextBox3.Enabled = false;
        this.Iframe2.Attributes.Add("readonly", "true");

    }

    /// <summary>
    /// 通过订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ReBindItem(object sender, EventArgs e) {
        string strVal = this.hdSelectBySaleBillVal.Value.Trim();
        if (string.IsNullOrEmpty(strVal))
        {
            return;
        }
        IList<T_SpecialRebatesAppmode> cxblist = new List<T_SpecialRebatesAppmode>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            T_SpecialRebatesAppmode licmodel = new T_SpecialRebatesAppmode();
            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                licmodel.Note1 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                licmodel.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                TextBox txtStandardSaleAmount = GridView1.Rows[s].Cells[3].FindControl("txtStandardSaleAmount") as TextBox;
                licmodel.StandardSaleAmount = decimal.Parse(GetGwStr(txtStandardSaleAmount.Text));
                TextBox txtExceedStandardPoint = GridView1.Rows[s].Cells[4].FindControl("txtExceedStandardPoint") as TextBox;
                licmodel.ExceedStandardPoint = decimal.Parse(GetGwStr(txtExceedStandardPoint.Text));
                TextBox txtExplain = GridView1.Rows[s].Cells[5].FindControl("txtExplain") as TextBox;
                licmodel.Explain = GetGwStr(txtExplain.Text);
                cxblist.Add(licmodel);
            }
        }
        string[] arrEve1=strVal.Split(new string[]{"|*|"},StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < arrEve1.Length; i++)
        {
            string strEve1Val = arrEve1[i];
            string[] arrEve2 = strEve1Val.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrEve2.Length<5)
            {
                continue;
            }
            T_SpecialRebatesAppmode model = new T_SpecialRebatesAppmode();
            model.Note1 = GetGwStr(arrEve2[0]);
            model.TruckCode = GetGwStr(arrEve2[1]);
            model.StandardSaleAmount = decimal.Parse(arrEve2[2]);
            model.ExceedStandardPoint = decimal.Parse(arrEve2[3]);
            model.Explain = GetGwStr(arrEve2[4]);
            cxblist.Add(model);
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
    }
    /// <summary>
    /// 获取model
    /// </summary>
    public void getmodel()
    {
        IList<T_SpecialRebatesAppmode> spemode = spebll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);

        this.txtAppDate.Text = spemode[0].AppDate.ToString();
        this.lbeBillCode.Text = spemode[0].Code.ToString();
        if (spemode[0].SaleDeptCode.ToString() != null && spemode[0].SaleDeptCode.ToString() != "")
        {
            string sql = "select '['+deptCode+']'+deptName from bill_departments where deptcode ='" + spemode[0].SaleDeptCode.ToString() + "'";
            string strdeptcode = server.GetCellValue(sql);
            this.ddlTravelType.Text = strdeptcode;
        }

        // this.TextBox1.Text = spemode[0].Explain.ToString();
        //this.txtReasion.Text = spemode[0].Note1.ToString();//订单号
        this.txtWorkPlan.Text = spemode[0].TruckCount.ToString();
        //this.txtFeePlan.Text = spemode[0].TruckCode.ToString();//底牌号=车架号
        //this.txtTransport.Text = spemode[0].StandardSaleAmount.ToString();

        // this.TextBox2.Text = spemode[0].ExceedStandardPoint.ToString();
        this.txtbgtime.Text = spemode[0].EffectiveDateFrm.ToString();
        this.txtendtime.Text = spemode[0].EffectiveDateTo.ToString();
        if (spemode[0].CheckAttachment.ToString() != null && spemode[0].CheckAttachment.ToString() != "")
        {
            this.txtFileUrl.Text = spemode[0].CheckAttachment.ToString();
            string strfilename = this.HiddenField1.Value.ToString();
            string strAppTemp = string.Format("<a href=\"../../../webBill/" + spemode[0].CheckAttachment.ToString() + " \" target='_blank'>审批附件</a>");
            this.lblspfj.Text = strAppTemp;

        }
        if (spemode[0].Attachment.ToString() != null && spemode[0].Attachment.ToString() != "")
        {

            string strfilename2 = this.HiddenField2.Value.ToString();
            string strAppTemp1 = string.Format("<a href=\"../../../webBill/" + spemode[0].Attachment.ToString() + "\" target='_blank'>附件</a>");
            this.TextBox3.Text = strAppTemp1;

        }

        List<T_SpecialRebatesAppmode> cxblist = new List<T_SpecialRebatesAppmode>();

        if (spemode.Count == 0)
        {
            cxblist.Add(new T_SpecialRebatesAppmode());
        }
        else
        {
            foreach (var i in spemode)
            {
                T_SpecialRebatesAppmode cx = new T_SpecialRebatesAppmode();
                cx.Note1 = i.Note1;
                cx.TruckCode = i.TruckCode;
                cx.Explain = i.Explain;
                cx.ExceedStandardPoint = i.ExceedStandardPoint;
                cx.StandardSaleAmount = i.StandardSaleAmount;
                cxblist.Add(cx);
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();


        //显示审批意见

        string sqlspyj = "select a.billCode, a.recordid,b.mind as shenpimind,b.steptext as shenpiren from dbo.workflowrecord a,dbo.workflowrecords b where a.billCode='" + strBillCode + "' and b.recordid=a.recordid";
        DataTable dt = server.RunQueryCmdToTable(sqlspyj);
        this.Repeater1.DataSource = dt;
        this.Repeater1.DataBind();
        if (dt.Rows.Count < 1)
        {
            this.tr1.Visible = false;
            this.Repeater1.Visible = false;
            this.trrepeat.Visible = false;

        }


        //如果是查看 就不让保存
        if (strCtrl.Equals("View"))
        {
            this.btn_bc.Visible = false;
            this.tryy.Visible = false;
            this.tridmoney.Visible = false;
            // this.ckid.Visible = false;
        }
        if (strCtrl.Equals("Edit"))
        {
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
        }

    }
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
    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    protected void btn_bc_Click(object sender, EventArgs e)
    {


        ////添加

        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdate = this.txtAppDate.Text.ToString().Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        // string bm = this.lbeDept.Text.Trim();
        //返利申请表
        SpecialRebatesAppBLL spebll = new SpecialRebatesAppBLL();
        T_SpecialRebatesAppmode spemode = new T_SpecialRebatesAppmode();
        //主表
        Bill_Main modelMainBill = new Bill_Main();

        if (strCtrl.Equals("Edit"))
        {
            if (strBillCode.Equals(""))
            {
                throw new Exception("单号丢失！");
            }
            else
            {
                spemode = spebll.GetModel(strBillCode);
                modelMainBill = bllBillMain.GetModel(strBillCode);
                spebll.Delete(strBillCode);
            }


            if (this.HiddenField2.Value == "" || this.HiddenField2.Value == null)
            {

                spemode.Attachment = this.TextBox3.Text;

            }


        }

        modelMainBill.BillCode = this.lbeBillCode.Text.Trim();
        modelMainBill.BillDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd").Trim());
        modelMainBill.BillDept = this.ddlTravelType.Text.Substring(1, this.ddlTravelType.Text.IndexOf("]") - 1).Trim();
        modelMainBill.BillName = "tzcfld";
        modelMainBill.BillType = "";
        modelMainBill.BillUser = str_billuser;
        modelMainBill.FlowId = "tsfl";
        modelMainBill.GkDept = "";
        modelMainBill.IsGk = "";
        modelMainBill.LoopTimes = 0;
        modelMainBill.StepId = "-1";



        //返利单申请表


        IList<T_SpecialRebatesAppmode> speciaLists = new List<T_SpecialRebatesAppmode>();

        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                spemode = new T_SpecialRebatesAppmode();
                spemode.Code = modelMainBill.BillCode;

                
                spemode.Note1 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                spemode.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);


                spemode.BillMainCode = modelMainBill.BillCode;
                spemode.AppDate = str_billdate;

                TextBox txtStandardSaleAmount = this.GridView1.Rows[s].Cells[3].FindControl("txtStandardSaleAmount") as TextBox;
                string strStandardSaleAmount = txtStandardSaleAmount.Text.Trim();
                if (strStandardSaleAmount.Equals(""))
                {
                    throw new Exception("正常返利不能为空！");
                }
                spemode.StandardSaleAmount = decimal.Parse(strStandardSaleAmount);//正常返利
                spemode.SysDateTime = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");//系统时间
                spemode.SysPersionCode = Session["userCode"].ToString().Trim();
                spemode.TruckCount = int.Parse(this.txtWorkPlan.Text);

                TextBox txtExplain = this.GridView1.Rows[s].Cells[5].FindControl("txtExplain") as TextBox;
                string strExplain = txtExplain.Text.Trim();

                spemode.Explain = GetGwStr(strExplain);//申请返利原因

                if (this.ddlTravelType.Text == null && this.ddlTravelType.Text == "")//申请部门
                {
                    throw new Exception("申请部门不能为空！");
                }
                else
                {
                    string strcode = this.ddlTravelType.Text.ToString();
                    spemode.SaleDeptCode = strcode.Substring(1, strcode.IndexOf("]") - 1);
                }
                //spemode.Explain = this.txtAppcontent.Text;
                TextBox txtExceedStandardPoint = this.GridView1.Rows[s].Cells[4].FindControl("txtExceedStandardPoint") as TextBox;
                string strExceedStandardPoint = txtExceedStandardPoint.Text.Trim();
                if (GetGwStr(strExceedStandardPoint) != null && GetGwStr(strExceedStandardPoint) != "")
                {
                    spemode.ExceedStandardPoint = decimal.Parse(GetGwStr(strExceedStandardPoint));
                }
                else
                {
                    spemode.ExceedStandardPoint = 0;
                }

                spemode.EffectiveDateTo = this.txtendtime.Text.ToString();
                spemode.EffectiveDateFrm = this.txtbgtime.Text.ToString();
                spemode.CheckAttachment = this.txtFileUrl.Text.ToString();
                spemode.BillMainCode = this.lbeBillCode.Text;
                if (this.HiddenField2.Value.ToString() != null && this.HiddenField2.Value.ToString() != "")
                {
                    spemode.Attachment = this.HiddenField2.Value.ToString();
                }

                spemode.AppDate = this.txtAppDate.Text.ToString();

                speciaLists.Add(spemode);

            }


        }
        if (speciaLists.Count > 0)
        {
            string strMsg = "";
            //int iRel = ;
            int iMainRel = bllBillMain.Add(modelMainBill);

            if (iMainRel <= 0)
            {
                throw new Exception("向主表插入数据时发生未知错误！");
            }
            if (!spebll.AddNotelist(speciaLists, out strMsg))
            {
                throw new Exception(strMsg);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('明细至少要有一行！');", true);
            return;

        }



        //    spemode.Code = this.lbeBillCode.Text.Trim();//单号
        //    spemode.StandardSaleAmount = decimal.Parse(this.txtTransport.Text);//正常返利
        //    spemode.SysDateTime = DateTime.Now.ToString("yy-MM-dd hh:mm:ss");//系统时间
        //    spemode.SysPersionCode = Session["userCode"].ToString().Trim();

        //    //判断是否有相同的车架号

        //    spemode.TruckCode = this.txtFeePlan.Text;
        //    spemode.TruckCount = int.Parse(this.txtWorkPlan.Text);
        //    spemode.Note1 = this.txtReasion.Text;//订单号
        //    spemode.Explain = this.TextBox1.Text;//申请返利原因

        //    if (this.ddlTravelType.Text == null && this.ddlTravelType.Text == "")//申请部门
        //    {
        //        throw new Exception("申请部门不能为空！");
        //    }
        //    else
        //    {
        //        string strcode = this.ddlTravelType.Text.ToString();
        //        spemode.SaleDeptCode = strcode.Substring(1, strcode.IndexOf("]") - 1);
        //    }
        //    //spemode.Explain = this.txtAppcontent.Text;
        //    if (this.TextBox2.Text != null && this.TextBox2.Text != "")
        //    {
        //        spemode.ExceedStandardPoint = decimal.Parse(this.TextBox2.Text);
        //    }
        //    else
        //    {
        //        spemode.ExceedStandardPoint = 0;
        //    }

        //    spemode.EffectiveDateTo = this.txtendtime.Text.ToString();
        //    spemode.EffectiveDateFrm = this.txtbgtime.Text.ToString();
        //    spemode.CheckAttachment = this.txtFileUrl.Text.ToString();
        //    spemode.BillMainCode = this.lbeBillCode.Text;
        //    if (this.HiddenField2.Value.ToString() != null && this.HiddenField2.Value.ToString() != "")
        //    {
        //        spemode.Attachment = this.HiddenField2.Value.ToString();
        //    }

        //    spemode.AppDate = this.txtAppDate.Text.ToString();


        //    modelMainBill.BillCode = this.lbeBillCode.Text.Trim();

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
        //    string strBillDept = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        //    if (string.IsNullOrEmpty(strBillDept))
        //    {
        //        throw new Exception("未发现人员所在单位！");
        //    }
        //    string strmaincode = this.ddlTravelType.Text.ToString();
        //    modelMainBill.BillDept = strmaincode.Substring(1, strmaincode.IndexOf("]") - 1);

        //    modelMainBill.BillJe = decimal.Parse(this.TextBox2.Text.Trim());
        //    string strMsg = "";
        //    int iRel = spebll.AddNote(modelMainBill, spemode, out strMsg);
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
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！,原因：" + ex.Message + "');", true);
        //}
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        ViewBLL viewbll = new ViewBLL();
        string strcarCode = "";
        if (txtFeePlan.Text != "")
        {
            strcarCode = txtFeePlan.Text.Trim();
            DataTable dt = viewbll.getDt(strcarCode);
            if (dt.Rows.Count > 0)
            {
                this.txtReasion.Text = dt.Rows[0]["ddh"].ToString();

            }

        }


    }
    /// <summary>
    /// 添加到列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {

        string cjh = txtFeePlan.Text.Trim();
        string ddh = txtReasion.Text.Trim();
        string zcmoney = txtTransport.Text.Trim();
        string ccmoney = TextBox2.Text.Trim();
        string strex = TextBox1.Text.Trim();
        string strcxh = "";

        ViewBLL viewbll = new ViewBLL();
        string strcarCode = "";
        if (txtFeePlan.Text != "")
        {
            strcarCode = txtFeePlan.Text.Trim();
            DataTable dt = viewbll.getDt(strcarCode);
            if (dt.Rows.Count > 0)
            {
                ddh = dt.Rows[0]["ddh"].ToString();
                strcxh = dt.Rows[0]["cxh"].ToString();
            }

        }

        // 通过车架号找车型号 如果找不到“无效底盘号（车架号）”
        if (strcxh == null || strcxh == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('无效底盘号！');", true);
            this.txtFeePlan.Text = "";
            this.txtReasion.Text = "";
            this.txtTransport.Text = "";
            this.TextBox1.Text = "";
            this.TextBox2.Text = "";
            return;
        }
        else//再通过车型号找是否已经设置车辆类型对应 如果没有“改内部车型号未设置过车辆类型对应，系统已记录该车型号，请到车辆类型对应模块设置对应关系”
        {
            string strTruckTypeNum = new T_TruckTypeCorrespondBLL().GetTruckTypeNumByFactCode(strcxh);
            Bll.Bills.T_BillingApplicationBll bll = new Bll.Bills.T_BillingApplicationBll();
            if (strTruckTypeNum != null && strTruckTypeNum != "")//如果有对应
            {
                //根据部门

                string strdeptcode = ddlTravelType.Text.Trim();
                if (strdeptcode != null && strdeptcode != "")
                {
                    strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门不能为空！');", true);
                    return;
                }

                //根据时间
                string strdatetime = DateTime.Now.ToString("yyyy-MM-dd");

                if (!bll.isExistrebate(strTruckTypeNum, strdatetime, strdeptcode))//判断对应上的车辆类型号有效日期   部门  是否有返利标准
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('编号为" + strTruckTypeNum.Trim() + "的车辆类型没有制定返利标准！');", true);
                }
            }
            else
            {
                //将车型号添加到对应表中
                T_TruckTypeCorrespond model = new T_TruckTypeCorrespond();
                T_TruckTypeCorrespondBLL trckbll = new T_TruckTypeCorrespondBLL();
                string msg = "";
                model.factTruckType = strcxh;
                //判断是否已经有改车型号了
                if (!trckbll.isExistfact(strcxh))
                {
                    int row = trckbll.Add(model, out msg);
                    if (row <= 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('系统记录该车型号失败！');", true);
                    }
                }
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车架号对应的内部车型号" + strcxh.Trim() + "未设置过车辆类型对应，系统将该车型号添加到车辆类型对应的设置界面，请到对应界面设置！');", true);
                return;
            }
        }


        if (txtFeePlan.Text.Trim() != "" && txtFeePlan.Text.Trim() != null)
        {
            if (spebll.IsKpsqTruckCode(txtFeePlan.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车架号做过开票申请！');", true);
                this.txtFeePlan.Text = "";
                this.txtReasion.Text = "";
                this.txtTransport.Text = "";
                this.TextBox1.Text = "";
                this.TextBox2.Text = "";
                return;
            }
            if (spebll.IsSpecialRebatesTruckCode(txtFeePlan.Text.Trim()))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车架号已经申请过特殊返利！');", true);
                this.txtFeePlan.Text = "";
                this.txtReasion.Text = "";
                this.txtTransport.Text = "";
                this.TextBox1.Text = "";
                this.TextBox2.Text = "";
                return;
            }
        }
        if (ddh == null || ddh == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该车辆没有订单号！');", true);
            this.txtFeePlan.Text = "";
            this.txtReasion.Text = "";
            this.txtTransport.Text = "";
            this.TextBox1.Text = "";
            this.TextBox2.Text = "";
            return;
        }
        if (ccmoney == null || ccmoney == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('超出返利标准不能为空！');", true);

            return;
        }
        if (zcmoney == null || zcmoney == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('正常返利不能为空！');", true);
            return;
        }
        IList<T_SpecialRebatesAppmode> cxblist = new List<T_SpecialRebatesAppmode>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            T_SpecialRebatesAppmode licmodel = new T_SpecialRebatesAppmode();

            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                licmodel.Note1 = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                if (cjh == GetGwStr(GridView1.Rows[s].Cells[2].Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + cjh + "车架号已经存在！');", true);
                    this.txtFeePlan.Text = "";
                    this.txtReasion.Text = "";
                    this.txtTransport.Text = "";
                    this.TextBox1.Text = "";
                    this.TextBox2.Text = "";

                    return;

                }
                licmodel.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                if (zcmoney != "")
                {
                    TextBox txtStandardSaleAmount = GridView1.Rows[s].Cells[3].FindControl("txtStandardSaleAmount") as TextBox;
                    if (txtStandardSaleAmount!=null)
                    {
                        licmodel.StandardSaleAmount = decimal.Parse(GetGwStr(txtStandardSaleAmount.Text.Trim()));
                    }
                }
                else
                {
                    licmodel.StandardSaleAmount = decimal.Parse("0");
                }
                if (ccmoney != "")
                {
                    TextBox txtExceedStandardPoint = GridView1.Rows[s].Cells[4].FindControl("txtExceedStandardPoint") as TextBox;
                    licmodel.ExceedStandardPoint = decimal.Parse(GetGwStr(txtExceedStandardPoint.Text.Trim()));

                }
                else
                {
                    licmodel.ExceedStandardPoint = decimal.Parse("0");

                }
                TextBox txtExplain = GridView1.Rows[s].Cells[5].FindControl("txtExplain") as TextBox;
                licmodel.Explain = GetGwStr(txtExplain.Text);
                cxblist.Add(licmodel);
            }
        }

        T_SpecialRebatesAppmode thiscx = new T_SpecialRebatesAppmode();
        thiscx.TruckCode = cjh;

        thiscx.Note1 = ddh;
        thiscx.Explain = strex;
        if (ccmoney != "")
        {
            thiscx.ExceedStandardPoint = decimal.Parse(ccmoney);
        }
        else
        {
            thiscx.ExceedStandardPoint = decimal.Parse("0");
        }
        if (zcmoney != "")
        {
            thiscx.StandardSaleAmount = decimal.Parse(zcmoney);
        }
        else
        {
            thiscx.StandardSaleAmount = decimal.Parse("0");
        }
        cxblist.Add(thiscx);
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
        txtFeePlan.Text = "";
        txtTransport.Text = "";
        TextBox2.Text = "";
        TextBox1.Text = "";
        txtReasion.Text = "";
    }
    protected void btn_Del_Click(object sender, EventArgs e)
    {

        List<T_SpecialRebatesAppmode> cxblist = new List<T_SpecialRebatesAppmode>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            System.Web.UI.WebControls.CheckBox check = GridView1.Rows[s].FindControl("CheckBox2") as System.Web.UI.WebControls.CheckBox;
            if (check.Checked == false)
            {
                T_SpecialRebatesAppmode cx = new T_SpecialRebatesAppmode();
                cx.Note1 = GetGwStr(GridView1.Rows[s].Cells[1].Text);

                cx.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                cx.StandardSaleAmount = decimal.Parse(GetGwStr(GridView1.Rows[s].Cells[3].Text));
                cx.ExceedStandardPoint = decimal.Parse(GetGwStr(GridView1.Rows[s].Cells[4].Text));
                cx.Explain = GetGwStr(GridView1.Rows[s].Cells[5].Text);
                cxblist.Add(cx);
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
    }
    protected string GetGwStr(string str)
    {
        if (str == "&nbsp;")
        {
            return "";
        }
        else
        {
            return str;
        }
    }
}
