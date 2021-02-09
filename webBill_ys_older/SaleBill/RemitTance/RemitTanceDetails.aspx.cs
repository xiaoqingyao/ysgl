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
using Bll.Sepecial;
using Bll.Bills;
using Models;
using System.Collections.Generic;
using System.Text;
using Dal.SysDictionary;
using Bll.SaleBill;

public partial class SaleBill_RemitTance_RemitTanceDetails : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    RemittanceBll spebll = new RemittanceBll();
    BillMainBLL bllBillMain = new BillMainBLL();
    V_HuikuanLeiBie bhklbBll = new V_HuikuanLeiBie();
    ViewBLL viewbll = new ViewBLL();
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
        }
    }
    private void initControl()
    {
        DataSet temp = bhklbBll.getall();
        if (temp != null)
        {
            this.txtremitancetype.DataSource = temp;
            this.txtremitancetype.DataValueField = "lb_id";
            this.txtremitancetype.DataTextField = "kxlb";
            this.txtremitancetype.DataBind();
        }
        //TextBox txtAppDate2 = this.FindControl("txtAppDate") as TextBox;
        //txtAppDate2.Attributes.Add("readonly", "true");
        //DataSet temp = server.GetDataSet("select * from bill_departments where deptStatus='1'");
        //if (temp != null)
        //{
        //    this.ddlTravelType.DataSource = temp;
        //    this.ddlTravelType.DataTextField = "deptName";
        //    this.ddlTravelType.DataValueField = "deptCode";
        //    this.ddlTravelType.DataBind();
        //}

    }
    private string GetdetpAll()
    {
        DataSet ds = server.GetDataSet("select deptCode, '['+deptCode+']'+deptName as dtname from  bill_departments where  IsSell='Y'");
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

    private void bindData()
    {
        this.initControl();
        if (strCtrl == "Add")
        {
            IList<T_Remittance> cxb = new List<T_Remittance>();
            cxb.Add(new T_Remittance());
            GridView1.DataSource = cxb;
            GridView1.DataBind();
            //有效日期默认当天，申请日期默认当天
            DateTime dt = System.DateTime.Now;
            this.txtAppDate.Text = dt.ToString("yyyy-MM-dd");
            this.txtordercodedate.Text = dt.ToString("yyyy-MM-dd");


            //报告单号
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("tsfl", strneed, 1, 6);


            //查询所在部门，是二级部门则显示，不是则另显示
            //string strDept = server.GetCellValue("select  isnull(sjdeptcode,'') from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "') ") ;
            //是
            if (isTopDept("y", Session["userCode"].ToString().Trim()))
            {
                string dept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //string deptcode = server.GetCellValue("select deptCode from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                this.txtdeptname.Text = dept;
                if (dept != null && dept != "")
                {
                    txtdeptname.Text = dept;

                }
            }
            else
            {
                //所在部门
                string Dept = server.GetCellValue("select '['+deptCode+']'+ deptName from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //string strcodes = server.GetCellValue("select deptCode from bill_departments where  IsSell='Y'and  deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
                //上级部门 
                string sjDept = server.GetCellValue("select '['+deptCode+']'+deptName from bill_departments where  IsSell='Y'and  deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "'))");


                this.txtdeptname.Text = Dept;
                if (Dept != null && Dept != "")
                {
                    txtdeptname.Text = Dept.ToString();

                }
            }

            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;

        }
        else if (strCtrl == "Edit" || strCtrl == "Atal")
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
            this.txtorderCode.Enabled = false;
            this.btn_bc.Visible = false;

        }
        else if (strCtrl == "look")
        {
            getmodel();
            getenbel();
            this.btn_ok.Visible = false;
            this.btn_cancel.Visible = false;
            this.btn_bc.Visible = false;

        }
    }
    public void getenbel()
    {
        this.txtAppDate.Enabled = false;
        this.txtdeptname.Enabled = false;
        //this.ddlTravelType.Enabled = false;
        
        this.txtremitnumber.Enabled = false;
        this.txtTruckCode.Enabled = false;
        this.txtmoney.Enabled = false;
        this.txtremitancetype.Enabled = false;
        this.txtordercodedate.Enabled = false;
        this.txtremitanceuse.Enabled = false;
        this.Iframe2.Visible = false;

    }

    /// <summary>
    /// 获取model
    /// </summary>
    public void getmodel()
    {
        IList<T_Remittance> remitemode = spebll.GetListByBillCode(strBillCode);
        Bill_Main modelBillMain = bllBillMain.GetModel(strBillCode);
        if (remitemode.Count>0)
        {
            this.txtAppDate.Text = remitemode[0].RemittanceDate.Trim();
            if (remitemode[0].PaymentDeptCode.ToString().Trim() != "" && remitemode[0].PaymentDeptCode.ToString().Trim() != null)
            {
                string strdeptcode = remitemode[0].PaymentDeptCode.ToString().Trim();
                string strsqldeptname = "select '['+deptCode+']'+ deptName from bill_departments where deptCode='" + remitemode[0].PaymentDeptCode.ToString().Trim() + "'";
                string strcodename = server.GetCellValue(strsqldeptname);
                if (strcodename != null && strcodename != "")
                {
                    this.txtdeptname.Text = strcodename;
                }
            }
            else
            {
                this.txtdeptname.Text = '[' + remitemode[0].PaymentDeptCode.ToString().Trim() + ']' + remitemode[0].PaymentDeptName.ToString().Trim();
            }
            this.txtremitnumber.Text = remitemode[0].RemittanceNumber.Trim();
            if (remitemode[0].RemittanceType.Trim() != null && remitemode[0].RemittanceType.Trim() != "")
            {
                string strtype = remitemode[0].RemittanceType.Trim();
                this.txtremitancetype.SelectedItem.Text = strtype;
            }
            if (remitemode[0].Accessories.ToString() != null && remitemode[0].Accessories.ToString() != "")
            {
                //this.TextBox3.Text = remitemode[0].Accessories.ToString();
                string strfilename = this.HiddenField2.Value.ToString();
                string strAppTemp = string.Format("<a href=\"../../../webBill/" + remitemode[0].Accessories.ToString() + " \" target='_blank'>点击查看附件</a>");
                this.lbfj.Text = strAppTemp;

            }
            else
            {
                this.lbfj.Text = "";
            }
            this.txtremitanceuse.SelectedValue = remitemode[0].RemittanceUse.Trim();
            this.txtordercodedate.Text = remitemode[0].OrderCodeDate.Trim();
        }
        
        //this.ddlTravelType.SelectedItem.Text = remitemode[0].PaymentDeptName.ToString().Trim();
        // this.lbldept.Text = remitemode[0].PaymentDeptName.ToString().Trim();
        //   this.ddlTravelType.SelectedValue = remitemode[0].PaymentDeptCode.Trim();
        this.txtorderCode.Text = "";
        
        this.txtTruckCode.Text = "";
        if (strBillCode != null && strBillCode != "")
        {
            string strsqlbillJe = "select billJe from dbo.bill_main where billCode='" + strBillCode + "'";
            string strmoneys = server.GetCellValue(strsqlbillJe);
             this.txtmoney.Text =strmoneys;
        }
        else
        {
            this.txtmoney.Text = "0.00";
        }
        List<T_Remittance> cxblist = new List<T_Remittance>();
        if (remitemode.Count == 0)
        {
            cxblist.Add(new T_Remittance());
        }
        else
        {
            foreach (var i in remitemode)
            {
                T_Remittance cx = new T_Remittance();
                cx.OrderCode = i.OrderCode;
                cx.TruckCode = i.TruckCode;
                cx.NOTE1 = i.NOTE1;
                cxblist.Add(cx);
            }
        }
        GridView1.DataSource = cxblist;
        GridView1.DataBind();

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
            sql = " select count(1) from (select deptCode from bill_departments where IsSell='Y'and sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where  IsSell='Y'and  sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
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
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);

    }
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        T_Remittance cksjmodel = new T_Remittance();
        Bill_Main modelMainBill = new Bill_Main();
        string strappdate = this.txtAppDate.Text.Trim();
        string strorderdate = this.txtordercodedate.Text.Trim();
        string strdeptname = this.txtdeptname.Text.Trim();
        string strmoney = this.txtmoney.Text.Trim();
        string strmumber = this.txtremitnumber.Text.Trim();
        string strremitancetype = this.txtremitancetype.Text.Trim();
        string struser = this.txtremitanceuse.SelectedValue.Trim();
        string cjh = txtTruckCode.Text.Trim();
        string ddh = txtorderCode.Text.Trim();
        //string strsystime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        //string strusercode=
        string strAccessories = "";
        if (HiddenField2.Value.Trim() != null && HiddenField2.Value.Trim() != "")
        {
            strAccessories = HiddenField2.Value.Trim();

        }

        string str_billuser = Session["userCode"].ToString().Trim();
        //添加


        if (strCtrl.Equals("Atal") || strCtrl.Equals("Edit"))
        {

            if (!strBillCode.Equals(""))
            {
                cksjmodel = spebll.GetModel(strBillCode);
                modelMainBill = bllBillMain.GetModel(strBillCode);
                spebll.Delete(strBillCode);
            }

        }
        else
        {
            string strnid = new DataDicDal().GetYbbxBillName("cksj", DateTime.Now.ToString("yyyyMMdd"), 1);

            modelMainBill.BillCode = strnid;

            modelMainBill.StepId = "-1";
        }

        modelMainBill.BillName = "车款上缴报告";
        modelMainBill.BillType = "";
        modelMainBill.BillDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd").Trim());
        modelMainBill.BillDept = strdeptname.Substring(1, strdeptname.IndexOf("]") - 1).Trim();
        modelMainBill.BillUser = str_billuser;
        modelMainBill.FlowId = "cksj";
        modelMainBill.GkDept = "";//车架号
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

        //车款上缴报告表
        IList<T_Remittance> cksjLists = new List<T_Remittance>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                cksjmodel = new T_Remittance();
                cksjmodel.NID = modelMainBill.BillCode;
                cksjmodel.OrderCode = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                cksjmodel.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                cksjmodel.NOTE1 = GetGwStr(GridView1.Rows[s].Cells[3].Text);

                if (strdeptname != "")
                {
                    string deptcode = strdeptname;
                    string deptname = strdeptname;
                    deptcode = deptcode.Substring(1, deptcode.IndexOf("]") - 1).Trim();
                    cksjmodel.PaymentDeptCode = deptcode;
                    cksjmodel.PaymentDeptName = deptname.Substring(deptname.IndexOf("]") + 1);
                }

                cksjmodel.RemittanceNumber = strmumber;

                cksjmodel.RemittanceType = strremitancetype;
                cksjmodel.RemittanceMoney = decimal.Parse("0");
                cksjmodel.RemittanceDate = strappdate;
                cksjmodel.OrderCodeDate = strorderdate;
                cksjmodel.RemittanceUse = struser;

                cksjmodel.Accessories = strAccessories;
                cksjmodel.SystemDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                cksjmodel.SystemuserCode = Session["userCode"].ToString().Trim();

                cksjLists.Add(cksjmodel);
            }
        }
        if (cksjLists.Count <= 0)//如果没有明细
        {
            cksjmodel = new T_Remittance();
            cksjmodel.NID = modelMainBill.BillCode;
            if (strdeptname != "")
            {
                string deptcode = strdeptname;
                string deptname = strdeptname;
                deptcode = deptcode.Substring(1, deptcode.IndexOf("]") - 1).Trim();
                cksjmodel.PaymentDeptCode = deptcode;
                cksjmodel.PaymentDeptName = deptname.Substring(deptname.IndexOf("]") + 1);
            }
            cksjmodel.RemittanceNumber = strmumber;

            cksjmodel.RemittanceType = strremitancetype;
            cksjmodel.RemittanceMoney = decimal.Parse("0");
            cksjmodel.RemittanceDate = strappdate;
            cksjmodel.OrderCodeDate = strorderdate;
            cksjmodel.RemittanceUse = struser;

            cksjmodel.Accessories = strAccessories;
            cksjmodel.SystemDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            cksjmodel.SystemuserCode = Session["userCode"].ToString().Trim();

            cksjLists.Add(cksjmodel);
        }
        string strMsg = "";
        if (!spebll.AddNote(cksjLists, out strMsg))
        {
            throw new Exception(strMsg);
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('明细至少要有一行！');", true);
        //    return;
        //}
    }

    /// <summary>
    /// 添加到
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button2_Click(object sender, EventArgs e)
    {
        string strappdate = this.txtAppDate.Text.Trim();
        string strorderdate = this.txtordercodedate.Text.Trim();
        string strdeptname = this.txtdeptname.Text.Trim();
        string strmoney = this.txtmoney.Text.Trim();
        string strmumber = this.txtremitnumber.Text.Trim();
        string strremitancetype = this.txtremitancetype.Text.Trim();
        string struser = this.txtremitanceuse.SelectedValue.Trim();
        string cjh = txtTruckCode.Text.Trim();
        string ddh = txtorderCode.Text.Trim();
        string jxs = txtjxs.Text.Trim();

        string strAccessories = "";
        if (HiddenField2.Value.Trim() != null && HiddenField2.Value.Trim() != "")
        {
            strAccessories = HiddenField2.Value.Trim();

        }

        IList<T_Remittance> cxblist = new List<T_Remittance>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            T_Remittance licmodel = new T_Remittance();

            if (GridView1.Rows[s].Cells[1].Text != "" && GridView1.Rows[s].Cells[1].Text != "&nbsp;")
            {
                licmodel.OrderCode = GetGwStr(GridView1.Rows[s].Cells[1].Text);
                if (cjh == GetGwStr(GridView1.Rows[s].Cells[2].Text))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + cjh + "车架号已经存在！');", true);
                    this.txtTruckCode.Text = "";
                    this.txtorderCode.Text = "";
                    this.txtjxs.Text = "";
                    return;

                }
                licmodel.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                licmodel.NOTE1 = GetGwStr(GridView1.Rows[s].Cells[3].Text);

                cxblist.Add(licmodel);
            }
        }



        T_Remittance thiscx = new T_Remittance();
        thiscx.TruckCode = cjh;
        thiscx.OrderCode = ddh;
        thiscx.NOTE1 = jxs;
        cxblist.Add(thiscx);
        GridView1.DataSource = cxblist;
        GridView1.DataBind();
        txtTruckCode.Text = "";
        txtorderCode.Text = "";
        txtjxs.Text = "";
    }
    protected void btn_Del_Click(object sender, EventArgs e)
    {

        List<T_Remittance> cxblist = new List<T_Remittance>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            System.Web.UI.WebControls.CheckBox check = GridView1.Rows[s].FindControl("CheckBox2") as System.Web.UI.WebControls.CheckBox;
            if (check.Checked == false)
            {
                T_Remittance cx = new T_Remittance();
                cx.OrderCode = GetGwStr(GridView1.Rows[s].Cells[1].Text);

                cx.TruckCode = GetGwStr(GridView1.Rows[s].Cells[2].Text);
                cx.NOTE1 = GetGwStr(GridView1.Rows[s].Cells[3].Text);

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
    /// <summary>
    /// 根据底盘号获取订单号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.txtTruckCode.Text.Trim() != "")
        {
            DataTable temp = viewbll.getDt(txtTruckCode.Text.Trim());
            if (temp != null && temp.Rows.Count > 0)
            {
                this.txtorderCode.Text = temp.Rows[0]["ddh"].ToString().Trim();
                this.txtjxs.Text = temp.Rows[0]["yhmc"].ToString().Trim();
            }
        }
    }

    /// <summary>
    /// 对应销售一线通
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_DuiYing_Click(object sender, EventArgs e) {
        int iRows = this.GridView1.Rows.Count;
        int index = -1;
        for (int i = 0; i < iRows; i++)
        {
            CheckBox checkbox = this.GridView1.Rows[i].FindControl("CheckBox2") as CheckBox;
            if (checkbox==null)
            {
                continue;
            }
            if (checkbox.Checked)
            {
                index = i;
                break;
            }
        }
        string strCompanyMsg=txtdeptname.Text.Trim();
        string strCompanuName = strCompanyMsg.Substring(strCompanyMsg.IndexOf("]")+1);
        //经销商
        string strJingXiaoShang ="";
        if (index!=-1)
        {
            strJingXiaoShang = this.GridView1.Rows[index].Cells[3].Text;
        }
       
        //回款类别
        string strHuiKuanLeiBie = this.txtremitancetype.SelectedValue;
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('RemitTanceNote.aspx?JXS=" + strJingXiaoShang + "&FGS=" + strCompanuName + "&HKLB="+strHuiKuanLeiBie+"');", true);

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
}
