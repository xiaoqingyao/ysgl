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
using System.Text;
using Dal.SysDictionary;

public partial class SaleBill_BorrowMoney_LoanListDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    LoanListBLL loanbll = new LoanListBLL();
    BillMainBLL bllBillMain = new BillMainBLL();
    //V_HuikuanLeiBie bhklbBll = new V_HuikuanLeiBie();
    //ViewBLL viewbll = new ViewBLL();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
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
            ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
            ClientScript.RegisterArrayDeclaration("avaiusernamedt", GetuserAll());
            ClientScript.RegisterArrayDeclaration("availablekmdt", GetKmAll());
        }
    }
    /// <summary>
    /// 选择人员
    /// </summary>
    /// <returns></returns>
    public string GetuserAll()
    {
        DataSet ds = server.GetDataSet("select  '['+usercode+']'+ username as usercodename from bill_users ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
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
        string strsql = @"select b.*,  a.billname, a.billUser ,(select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode=b.fykm)as fykmname  from bill_main a inner join bill_ybbxmxb_fykm  b on a.    billCode=b.billCode and a. billUser='" + Session["userCode"].ToString().Trim() + "' and b.status='2'and fykm in(" + strRel + ")";
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
    /// 预算科目选择
    /// </summary>
    /// <returns></returns>
    public string GetKmAll()
    {
        DataSet ds = server.GetDataSet("select '['+yskmCode+']'+yskmMc as yskmCodeName from bill_yskm ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["yskmCodeName"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 部门选择
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
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
    }

    private void bindData()
    {
        this.trjs.Visible = false;
        this.txtdeptname.Enabled = false;
        this.txtResponsibleName.Enabled = false;
        this.txtAppDate.Visible = false;
        if (strCtrl == "Add")
        {
            this.trgride.Visible = false;
            this.trcjmx.Visible = false;
            //有效日期默认当天，申请日期默认当天
            DateTime dt = System.DateTime.Now;
            this.txtAppDate.Text = dt.ToString("yyyy-MM-dd");
            this.txtordercodedate.Text = dt.ToString("yyyy-MM-dd");
            if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                return;
            }
            else
            {
                string userCode = Session["userCode"].ToString();
                string strsqlrespcodename = "select  '['+usercode+']'+ username from bill_users where usercode='" + userCode + "'";
                string strrespcodename = server.GetCellValue(strsqlrespcodename);
                string strlastjkr = "";
                string strCurrentOrLastForYbbx = new Bll.ConfigBLL().GetValueByKey("CurrentOrLastForJK");
                if (Session["LastJKR"] != null && Session["LastJKR"].ToString() != "" && strCurrentOrLastForYbbx.Equals("0"))
                {
                    strlastjkr = Session["LastJKR"].ToString();
                    string strlastjkrname = server.GetCellValue("select  '['+usercode+']'+ username from bill_users where usercode='" + strlastjkr + "'");

                    txtloanName.Text = strlastjkrname;//借款人
                }
                else
                {
                    txtloanName.Text = strrespcodename;//借款人
                }
  
                if (strrespcodename != "" && strrespcodename != null)
                {
                    this.txtResponsibleName.Text = strrespcodename;//经办人
                    
                    string strloancode = strrespcodename.Substring(1, strrespcodename.IndexOf("]") - 1);
                    //string strdeptname = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  deptcode=(select userDept from bill_users where userCode='" + strloancode + "')");
                    Bill_Departments modelDept = new Bll.UserProperty.UserMessage(strloancode).GetRootDept();
                    this.txtdeptname.Text = "[" + modelDept.DeptCode + "]" + modelDept.DeptName;
                    this.HiddenField1.Value = "[" + modelDept.DeptCode + "]" + modelDept.DeptName;
                }
            }
            //报告单号
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("yzsq", strneed, 1, 6);
            this.lbjkcode.Text = strcode.Trim();

            ////查询所在部门，是二级部门则显示，不是则另显示
            ////string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            ////是
            //if (isTopDept("y", Session["userCode"].ToString().Trim()))
            //{
            //    string dept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    //string deptcode = server.GetCellValue("select deptCode from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    this.txtdeptname.Text = dept;
            //    if (dept != null && dept != "")
            //    {
            //        txtdeptname.Text = dept;

            //    }
            //}
            //else
            //{
            //    //所在部门
            //    string Dept = server.GetCellValue("select '['+deptCode+']'+ deptName from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    //string strcodes = server.GetCellValue("select deptCode from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            //    //上级部门 
            //    string sjDept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  IsSell='Y'and  deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");


            //    this.txtdeptname.Text = Dept;
            //    if (Dept != null && Dept != "")
            //    {
            //        txtdeptname.Text = Dept.ToString();

            //    }
            //}
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
        }
        else if (strCtrl == "Edit" || strCtrl == "Atal")
        {
            this.trgride.Visible = false;
            this.trcjmx.Visible = false;
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
            this.trgride.Visible = true;

            GridView1.DataSource = getbycodemodel(strBillCode);
            GridView1.DataBind();
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                this.GridView1.Rows[i].Cells[2].Text = "<a href=# onclick=\"openDetail('" + this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "');\">" + this.GridView1.Rows[i].Cells[2].Text.ToString().Trim() + "</a>";
            }
            this.btn_bc.Visible = false;
        }
        else if (strCtrl == "look")
        {
            getmodel();
            getenbel();
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
            this.btn_bc.Visible = false;
            this.trgride.Visible = true;

            GridView1.DataSource = getbycodemodel(strBillCode);
            GridView1.DataBind();
            for (int i = 0; i < this.GridView1.Rows.Count; i++)
            {
                this.GridView1.Rows[i].Cells[2].Text = "<a href=# onclick=\"openDetailbx('" + this.GridView1.Rows[i].Cells[1].Text.ToString().Trim() + "');\">" + this.GridView1.Rows[i].Cells[2].Text.ToString().Trim() + "</a>";
            }
        }
    }
    public void getenbel()
    {
        this.txtAppDate.Enabled = false;
        this.txtdeptname.Enabled = false;
        //this.ddlTravelType.Enabled = false;
        this.txtmoney.Enabled = false;
        this.txtordercodedate.Enabled = false;
        this.txtremitanceuse.Enabled = false;
    }

    /// <summary>
    /// 获取model
    /// </summary>
    public void getmodel()
    {
        IList<T_LoanList> loanemodel = loanbll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        if (loanemodel.Count > 0)
        {
            if (loanemodel[0].LoanCode.ToString() != "" && loanemodel[0].LoanCode != null)
            {
                string strsqlloanName = "select  '['+usercode+']'+ username from bill_users where usercode='" + loanemodel[0].LoanCode.ToString().Trim() + "'";
                string strloancodeName = server.GetCellValue(strsqlloanName);
                this.lbjkcode.Text = loanemodel[0].Listid.ToString();
                if (strloancodeName != "" && strloancodeName != null)
                {
                    this.txtloanName.Text = strloancodeName;//借款人
                }
            }

            this.txtAppDate.Text = loanemodel[0].ResponsibleDate.Trim();//经办时间
            this.txtcjCode.Text = loanemodel[0].CJCode.ToString();//冲减单号

            if (loanemodel[0].ResponsibleCode.ToString() != "" && loanemodel[0].ResponsibleCode != null)
            {
                string strsqlResponsName = "select  '['+usercode+']'+ username from bill_users where usercode='" + loanemodel[0].ResponsibleCode.ToString().Trim() + "'";
                string strResponscodeName = server.GetCellValue(strsqlResponsName);
                if (strResponscodeName != "" && strResponscodeName != null)
                {
                    this.txtResponsibleName.Text = strResponscodeName;//经办人
                }
            }
            if (loanemodel[0].NOTE2.ToString() != "" && loanemodel[0].NOTE2 != null)
            {
                string[] arrCodes = loanemodel[0].NOTE2.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string strShowStr = "";
                for (int i = 0; i < arrCodes.Length; i++)
                {
                    strShowStr+=server.GetCellValue("select '['+yskmCode+']'+yskmMc as yskmCodeName from bill_yskm where yskmCode='" + arrCodes[i] + "'");
                    strShowStr += ",";
                }
                this.txtloankm.Text=strShowStr.Substring(0,strShowStr.Length-1);
            }
            this.txtloanexplain.Text = loanemodel[0].LoanExplain.ToString();



            if (loanemodel[0].LoanDeptCode.ToString().Trim() != "" && loanemodel[0].LoanDeptCode.ToString().Trim() != null)
            {
                string strdeptcode = loanemodel[0].LoanDeptCode.ToString().Trim();
                string strsqldeptname = "select '['+deptCode+']'+ deptName from bill_departments where deptCode='" + loanemodel[0].LoanDeptCode.ToString().Trim() + "'";
                string strcodename = server.GetCellValue(strsqldeptname);
                if (strcodename != null && strcodename != "")
                {
                    this.txtdeptname.Text = strcodename;//借款部门
                    this.HiddenField1.Value = strcodename;
                }
            }



            this.txtremitanceuse.SelectedValue = loanemodel[0].SettleType.Trim();//结算方式
            this.txtordercodedate.Text = loanemodel[0].LoanDate.Trim();//借款日期
        }


        if (strBillCode != null && strBillCode != "")//借款金额
        {
            string strsqlbillJe = "select billJe from dbo.bill_main where billCode='" + strBillCode + "'";
            string strmoneys = server.GetCellValue(strsqlbillJe);
            this.txtmoney.Text = strmoneys;
        }
        else
        {
            this.txtmoney.Text = "0.00";
        }


        if (strCtrl.Equals("Edit"))
        {
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
        }

    }
   /// <summary>
   /// 保存
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        T_LoanList loanmodel = new T_LoanList();
        Bill_Main modelMainBill = new Bill_Main();
        string strappdate = this.txtAppDate.Text.Trim();//经办日期
        string strorderdate = this.txtordercodedate.Text.Trim();//借款日期
        string strdeptname = this.HiddenField1.Value.Trim();//借款部门
        string strmoney = this.txtmoney.Text.Trim();//借款金额
        string struser = this.txtremitanceuse.SelectedValue.Trim();//结算方式
        string ddh = this.lbjkcode.Text.Trim();
        string str_billuser = Session["userCode"].ToString().Trim();
        //添加
        if (strCtrl.Equals("Atal") || strCtrl.Equals("Edit"))
        {
            if (!strBillCode.Equals(""))
            {
                loanmodel = loanbll.GetModel(strBillCode);
                modelMainBill = bllBillMain.GetModel(strBillCode);
                loanbll.Delete(strBillCode);
            }
        }
        else
        {
            string strnid = new DataDicDal().GetYbbxBillName("yzsq", DateTime.Now.ToString("yyyyMMdd"), 1);
            modelMainBill.BillCode = strnid;
            modelMainBill.StepId = "-1";
        }
        modelMainBill.BillName = "借款申请单";
        modelMainBill.BillType = "";
        modelMainBill.BillDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd").Trim());
        modelMainBill.BillDept = strdeptname.Substring(1, strdeptname.IndexOf("]") - 1).Trim();
        modelMainBill.BillUser = str_billuser;
        modelMainBill.FlowId = "yzsq";
        modelMainBill.GkDept = "";
        modelMainBill.IsGk = "";
        modelMainBill.LoopTimes = 0;
        if (txtmoney.Text.Trim() != "")
        {
            modelMainBill.BillJe = decimal.Parse(txtmoney.Text.Trim());
        }
        else
        {
            modelMainBill.BillJe = decimal.Parse("0");
        }
        if (modelMainBill == null)
        {
            throw new Exception("主表或出差申请单模型不能为空");
        }
        //主表
        int iMainRel = bllBillMain.Add(modelMainBill);
        if (iMainRel <= 0)
        {
            throw new Exception("向主表插入数据时发生未知错误！");
        }
        //借款申请单
        IList<T_LoanList> LoanLists = new List<T_LoanList>();
        loanmodel = new T_LoanList();
        loanmodel.Listid = modelMainBill.BillCode;
        loanmodel.LoanCode = txtloanName.Text.Substring(1, txtloanName.Text.IndexOf("]") - 1).Trim();
        loanmodel.LoanDate = txtordercodedate.Text.Trim();
        if (strdeptname != "")
        {
            string deptcode = strdeptname;
            string deptname = strdeptname;
            deptcode = deptcode.Substring(1, deptcode.IndexOf("]") - 1).Trim();
            loanmodel.LoanDeptCode = deptcode;

        }
        if (txtloankm.Text.Trim() != "" && txtloankm.Text != null)
        {
            string strloankm = txtloankm.Text.Trim();
            string[] arrKm = strloankm.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string strkmRel = "";
            for (int i = 0; i < arrKm.Length; i++)
            {
                strkmRel += arrKm[i].Substring(1, arrKm[i].IndexOf("]") - 1);
                strkmRel += ",";
            }
            loanmodel.NOTE2 = strkmRel.Substring(0, strkmRel.Length - 1);
        }
        loanmodel.LoanExplain = txtloanexplain.Text;
        if (txtmoney.Text.Trim() != "")
        {
            loanmodel.LoanMoney = decimal.Parse(txtmoney.Text.Trim());
        }
        else
        {
            loanmodel.LoanMoney = decimal.Parse("0");
        }
        loanmodel.LoanSystime = DateTime.Now.ToString("yyyy-MM-dd");
        if (txtResponsibleName.Text.Trim() != "")
        {
            loanmodel.ResponsibleCode = txtResponsibleName.Text.Trim().Substring(1, txtResponsibleName.Text.Trim().IndexOf("]") - 1).Trim();
        }
        loanmodel.ResponsibleDate = txtAppDate.Text.ToString();
        loanmodel.ResponsibleSysTime = DateTime.Now.ToString("yyyy-MM-dd");
        loanmodel.Status = "1";
        int iRows = loanbll.AddModel(loanmodel);
        if (iRows > 0)
        {
            string strCurrentOrLastForYbbx = new Bll.ConfigBLL().GetValueByKey("CurrentOrLastForJK");
            if (strCurrentOrLastForYbbx.Equals("0"))
            {
                Session["LastJKR"] = loanmodel.LoanCode;
            }
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');", true);
            return;
        }
    }
}
