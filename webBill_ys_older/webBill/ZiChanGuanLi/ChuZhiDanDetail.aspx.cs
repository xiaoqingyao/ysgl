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
using Bll.SaleBill;
using Bll.Zichan;
using Models;
using System.Text;
using System.Collections.Generic;
using Bll.UserProperty;
using Dal.SysDictionary;

public partial class webBill_ZiChanGuanLi_ChuZhiDanDetail : System.Web.UI.Page
{
    ChuZhiDanBll bll = new ChuZhiDanBll();
    Bll.Bills.BillMainBLL bllmain = new Bll.Bills.BillMainBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ViewBLL viewbll = new ViewBLL();
    SysManager smgr = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetNoStore();
            if (!IsPostBack)
            {
                Bind();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetuserAll());
            ClientScript.RegisterArrayDeclaration("avzcTags", GetZcCodeAll());
            if (this.ddlSalewxlb.Items.Count <= 0 || this.ddlSalewxlb.SelectedIndex > -1)
            {
                this.bindByType(this.ddlSalewxlb.SelectedValue);
            }
        }
    }


    //页面一进入时的绑定
    private void Bind()
    {

        if (Request.QueryString["type"] != null)
        {
            string type = Request.QueryString["type"].ToString();
            if (type == "Add")
            {
                ZiChan_ChuZhiDan model = new ZiChan_ChuZhiDan();


                txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请编号

                btn_Shtg.Visible = false;
                btn_Shbh.Visible = false;

                if (isTopDept("y", Session["userCode"].ToString().Trim()))
                {
                    string dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                    this.lasqbm.Text = dept;

                }
                else
                {
                    //所在部门
                    string Dept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "') and IsSell='Y'");
                    //上级部门
                    string sjDept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString().Trim() + "')) and IsSell='Y'");
                    this.lasqbm.Text = Dept;
                }
                //生成编号
                string lscgCode = new DataDicDal().GetYbbxBillName("zcczd", DateTime.Now.ToString("yyyyMMdd"), 1);
                if (lscgCode == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
                    this.btn_bc.Visible = false;
                }
                else
                {
                    this.laSqbh.Text = lscgCode;
                }
                //经办人
                UserMessage user = new UserMessage(Session["userCode"].ToString());
                this.txtjbname.Text = "[" + user.Users.UserCode + "]" + user.Users.UserName;
            }
            if (type == "Edit")
            {
                if (Request.QueryString["bh"] != null)
                {
                    getmodel();
                    btn_Shbh.Visible = false;
                    btn_Shtg.Visible = false;
                }
            }
            if (type == "look" || type == "addInvoice")
            {
                if (Request.QueryString["bh"] != null)
                {
                    getmodel();
                }
                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;
                this.txtchangeh.Enabled = false;
                this.txtchangeq.Enabled = false;
                this.txtchangetime.Enabled = false;
                this.txtjbname.Enabled = false;
                this.txtzcCode.Enabled = false;
                this.btn_bc.Visible = false;
                this.txtsqname.Enabled = false;
                this.ddlSalewxlb.Enabled = false;
                this.btn_Shbh.Visible = false;
                this.btn_Shtg.Visible = false;
            }
            if (type == "audit")
            {
                if (Request.QueryString["bh"] != null)
                {
                    getmodel();
                }
                this.txtsqrq.Enabled = false;
                this.txtExplina.Enabled = false;

                this.btn_Shbh.Visible = true;
                this.btn_Shtg.Visible = true;
                this.btn_bc.Visible = false;
            }
            else
            {
                this.btn_Shbh.Visible = false;
                this.btn_Shtg.Visible = false;
            }
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
            sql = " select count(1) from (select deptCode from bill_departments where IsSell='Y'and sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "') ";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where IsSell='Y'and sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
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
    /// 获取model
    /// </summary>
    public void getmodel()
    {
        string bh = Request.QueryString["bh"].ToString();
        Bill_Main billmain = bllmain.GetModel(bh);
        ZiChan_ChuZhiDan zcmodel = new ZiChan_ChuZhiDan();
        zcmodel = bll.getmodel(bh);

        laSqbh.Text = billmain.BillCode;//处置单号
        txtsqrq.Text = Convert.ToDateTime(billmain.BillDate).ToString("yyyy-MM-dd");//申请单号
        if (billmain.BillDept != null && billmain.BillDept != "")
        {
            string strsql = "select '['+deptcode+']'+deptName from bill_departments where deptcode='" + billmain.BillDept + "'";
            string strsqname = server.GetCellValue(strsql);
            lasqbm.Text = strsqname;
        }
        if (zcmodel.ShenQingRenCode != null && zcmodel.ShenQingRenCode != "")//申请人
        {
            string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.ShenQingRenCode + "'";
            string strsqname = server.GetCellValue(strsql);
            txtsqname.Text = strsqname;
        }
        if (zcmodel.ZiChanCode != null && zcmodel.ZiChanCode != "")//资产
        {
            string strsql = "select '['+ZiChanCode+']'+ZiChanName as zcnames  from ZiChan_Jilu  where ZiChanCode='" + zcmodel.ZiChanCode + "'";
            string strsqname = server.GetCellValue(strsql);
            txtzcCode.Text = strsqname;
        }
        if (zcmodel.JingBanRen != null && zcmodel.JingBanRen != "")//经办人
        {

            string strsql = "select '['+userCode+']'+userName as usernamecode from bill_users where userCode='" + zcmodel.JingBanRen + "'";
            string strjbname = server.GetCellValue(strsql);
            txtjbname.Text = strjbname;
        }
        if (zcmodel.BianDongType != null && zcmodel.BianDongType != "")
        {
            //string strtypecode = server.GetCellValue("select '['+DicCode + ']'+DicName  from bill_dataDic where dicType='08' and dicCode='" + zcmodel.BianDongType + "'");
            //ddlSalewxlb.SelectedValue = strtypecode;
            this.ddlSalewxlb.SelectedValue = zcmodel.BianDongType;
        }
        else
        {
            showMessage("变动类型没有获取到，程序出错！", true, ""); return;
        }
        this.txtchangetime.Text = zcmodel.BianDongDate.ToString();

        string strQian = zcmodel.Qian;
        string strHou = zcmodel.Hou;
        switch (zcmodel.BianDongType)
        {
            case "01": break;//位置
            case "02": strQian = new DepartmentManager(strQian).ToString(); strHou = new DepartmentManager(strHou).ToString(); break;
            case "03": strQian = new ZiChan_ShiYongZhuangKuang(strQian).ToString(); strHou = new ZiChan_ShiYongZhuangKuang(strHou).ToString(); break;
            case "04": strQian = new ZiChan_Leibie(strQian).ToString(); strHou = new ZiChan_Leibie(strHou).ToString(); break;//类别
            default: showMessage("未知的变动类别！", true, "");
                break;
        }
        this.txtchangeq.Text = strQian;
        this.txtchangeh.Text = strHou;
        this.txtExplina.Text = zcmodel.ShuoMing;
        this.txtAddressBefore.Text = zcmodel.Note1;
        this.txtAddressAfter.Text = zcmodel.Note2;
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["type"] != null)
        {
            string type = Request.QueryString["type"].ToString();
            if (type == "Add")
            {
                Bill_Main main = new Bill_Main();
                main.BillCode = laSqbh.Text;
                main.BillName = "资产处置单";
                main.FlowId = "zcczd";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();

                main.BillDate = Convert.ToDateTime(txtsqrq.Text);
                string strjbname = lasqbm.Text.ToString();
                try
                {
                    strjbname = strjbname.Substring(1, strjbname.IndexOf("]") - 1);
                }
                catch (Exception)
                {

                    strjbname = "";
                }

                if (!strjbname.Equals(""))//申请部门
                {

                    main.BillDept = strjbname;
                }

                ZiChan_ChuZhiDan model = new ZiChan_ChuZhiDan();
                model.MainCode = laSqbh.Text.Trim();
                txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请日期
                model.ShuoMing = this.txtExplina.Text.ToString();//说明
                string strjbnames = txtjbname.Text.ToString();
                try
                {
                    strjbnames = strjbnames.Substring(1, strjbnames.IndexOf("]") - 1);
                }
                catch (Exception)
                {

                    strjbnames = "";
                }



                model.JingBanRen = strjbname;

                string strChangeTime = this.txtchangetime.Text.Trim();
                DateTime dtChangTime;
                if (!DateTime.TryParse(strChangeTime, out dtChangTime))
                {
                    showMessage("请输入合法变动日期！", false, ""); return;
                }
                model.BianDongDate = dtChangTime.ToString("yyyy-MM-dd");
                if (txtsqname.Text.Trim() != "")//申请人
                {
                    string strsqname = txtsqname.Text.Trim();
                    try
                    {
                        strsqname = strsqname.Substring(1, strsqname.IndexOf("]") - 1);
                        model.ShenQingRenCode = strsqname;
                    }
                    catch (Exception)
                    {
                        showMessage("请输入合法申请人信息！", false, ""); return;
                    }
                }
                else
                {
                    showMessage("请选择申请人！", false, ""); return;
                }
                string strZcCode = this.txtzcCode.Text.Trim();
                if (strZcCode != "")//资产code
                {
                    try
                    {
                        strZcCode = strZcCode.Substring(1, strZcCode.IndexOf("]") - 1);
                        model.ZiChanCode = strZcCode;
                    }
                    catch (Exception)
                    {
                        showMessage("请输入合法资产信息！", false, ""); return;
                    }
                }
                else { showMessage("请选择资产！", false, ""); return; }
                if (this.ddlSalewxlb.SelectedValue != "" && this.ddlSalewxlb.SelectedValue != null)//变动类别
                {
                    string strbdlb = this.ddlSalewxlb.SelectedValue;
                    model.BianDongType = strbdlb;
                }

                //变动前后
                string strQian = this.txtchangeq.Text.Trim();
                string strHou = this.txtchangeh.Text.Trim();
                if (this.ddlSalewxlb.SelectedValue != "01")
                {
                    try
                    {
                        strHou = strHou.Substring(1, strHou.IndexOf("]") - 1);
                        strQian = strQian.Substring(1, strQian.IndexOf("]") - 1);
                    }
                    catch (Exception)
                    {
                        showMessage("请输入合法的变动前后信息！", false, ""); return;
                    }
                }
                model.Qian = strHou;
                model.Hou = strQian;

                //如果是部门变动  note1 note2 分别存储部门变动带来的位置变动
                if (this.ddlSalewxlb.SelectedValue.Equals("02"))
                {
                    string strAdressBefore = this.txtAddressBefore.Text.Trim();
                    string strAdressAfter = this.txtAddressAfter.Text.Trim();
                    if (strAdressAfter.Equals(""))
                    {
                        showMessage("部门变动请重新输入资产位置！", false, ""); return;
                    }
                    else
                    {
                        model.Note1 = strAdressBefore;
                        model.Note2 = strAdressAfter;
                    }
                }
                else
                {
                    model.Note1 = "";
                    model.Note2 = "";
                }
                model.Note3 = "";
                model.Note4 = "";
                model.Note5 = "";
                model.Note6 = "";
                model.Note7 = "";
                model.Note8 = "";
                model.Note9 = "";
                model.Note10 = "";
                if (bll.Edit(main, model))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
                }
            }
            if (type == "Edit" || type == "addInvoice")
            {
                Bill_Main main = new Bill_Main();
                main.BillCode = laSqbh.Text;
                main.BillName = "资产处置单";
                main.FlowId = "zcczd";
                main.StepId = "-1";
                main.BillUser = Session["userCode"].ToString().Trim();
                main.BillDate = Convert.ToDateTime(txtsqrq.Text);


                string strjbname = lasqbm.Text.ToString();
                try
                {
                    strjbname = strjbname.Substring(1, strjbname.IndexOf("]") - 1);
                }
                catch (Exception)
                {

                    strjbname = "";
                }


                if (!strjbname.Equals(""))//申请部门
                {

                    main.BillDept = strjbname;
                }
                ZiChan_ChuZhiDan sqmodel = new ZiChan_ChuZhiDan();
                if (Request.QueryString["bh"].ToString() != null && Request.QueryString["bh"].ToString() != "")
                {
                    sqmodel.MainCode = laSqbh.Text;
                    if (type == "Edit")
                    {
                        int row = bll.Delete(Request.QueryString["bh"].ToString());
                    }
                    txtsqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");//申请时间
                    sqmodel.ShuoMing = this.txtExplina.Text.ToString();//说明

                    string strjbnames = txtjbname.Text.ToString();
                    try
                    {
                        strjbnames = strjbnames.Substring(1, strjbname.IndexOf("]") - 1);
                    }
                    catch (Exception)
                    {

                        strjbnames = "";
                    }

                    sqmodel.JingBanRen = strjbnames;

                    if (txtsqname.Text.Trim() != "")//申请人
                    {
                        string strsqname = txtsqname.Text.Trim();
                        try
                        {
                            strsqname = strsqname.Substring(1, strsqname.IndexOf("]") - 1);
                            sqmodel.ShenQingRenCode = strsqname;
                        }
                        catch (Exception)
                        {
                            showMessage("请输入合法申请人信息！", false, ""); return;
                        }
                    }
                    else
                    {
                        showMessage("请选择申请人！", false, ""); return;
                    }
                    string strZcCode = this.txtzcCode.Text.Trim();
                    if (strZcCode != "")//资产code
                    {
                        try
                        {
                            strZcCode = strZcCode.Substring(1, strZcCode.IndexOf("]") - 1);
                            sqmodel.ZiChanCode = strZcCode;
                        }
                        catch (Exception)
                        {
                            showMessage("请输入合法资产信息！", false, ""); return;
                        }
                    }
                    else { showMessage("请选择资产！", false, ""); return; }
                    if (this.ddlSalewxlb.SelectedValue != "" && this.ddlSalewxlb.SelectedValue != null)//变动类别
                    {
                        string strbdlb = this.ddlSalewxlb.SelectedValue;
                        strbdlb = strbdlb.Substring(1, strbdlb.IndexOf("]") - 1);
                        sqmodel.BianDongType = strbdlb;
                    }
                    sqmodel.BianDongDate = this.txtchangetime.Text;
                    //变动前后
                    string strQian = this.txtchangeq.Text.Trim();
                    string strHou = this.txtchangeh.Text.Trim();
                    if (this.ddlSalewxlb.SelectedValue != "01")
                    {
                        try
                        {
                            strHou = strHou.Substring(1, strHou.IndexOf("]") - 1);
                            strQian = strQian.Substring(1, strQian.IndexOf("]") - 1);
                        }
                        catch (Exception)
                        {
                            showMessage("请输入合法的变动前后信息！", false, ""); return;
                        }
                    }
                    sqmodel.Qian = strHou;
                    sqmodel.Hou = strQian;
                    //如果是部门变动  note1 note2 分别存储部门变动带来的位置变动
                    if (this.ddlSalewxlb.SelectedValue.Equals("02"))
                    {
                        string strAdressBefore = this.txtAddressBefore.Text.Trim();
                        string strAdressAfter = this.txtAddressAfter.Text.Trim();
                        if (strAdressAfter.Equals(""))
                        {
                            showMessage("部门变动请重新输入资产位置！", false, ""); return;
                        }
                        else
                        {
                            sqmodel.Note1 = strAdressBefore;
                            sqmodel.Note2 = strAdressAfter;
                        }
                    }
                    else
                    {
                        sqmodel.Note1 = "";
                        sqmodel.Note2 = "";
                    }
                    sqmodel.Note3 = "";
                    sqmodel.Note4 = "";
                    sqmodel.Note5 = "";
                    sqmodel.Note6 = "";
                    sqmodel.Note7 = "";
                    sqmodel.Note8 = "";
                    sqmodel.Note9 = "";
                    sqmodel.Note10 = "";
                }
                if (bll.Edit(main, sqmodel))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue='1';window.close();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
                }
            }
        }
    }
    /// <summary>
    /// 判断是不是空格
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
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
    /// 变动类别选择切换
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlSalewxlb_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtchangeq.Text = txtchangeh.Text = "";
        if (this.ddlSalewxlb.SelectedValue == null)
        {
            return;
        }
        biandongbefore.Text = this.ddlSalewxlb.SelectedItem.Text + "前：";
        this.biandongafter.Text = this.ddlSalewxlb.SelectedItem.Text + "后：";
        if (this.ddlSalewxlb.SelectedValue.Equals("02"))
        {
            this.trAddressAfter.Visible = true;
            this.trAddressBefore.Visible = true;
        }
        else
        {
            this.trAddressAfter.Visible = false;
            this.trAddressBefore.Visible = false;
        }
        bindByType(this.ddlSalewxlb.SelectedValue);
        showZiChanMsg();
    }

    /// <summary>
    /// 卡片变化后触发后台的事件  以绑定基本信息
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShowmsg_Click(object sender, EventArgs e)
    {
        showZiChanMsg();
    }
    /// <summary>
    /// 通过变动类别 为变动前和变动后注册可选的数据
    /// </summary>
    /// <param name="strType"></param>
    private void bindByType(string strType)
    {
        if (string.IsNullOrEmpty(strType))
        {
            return;
        }

        switch (strType)
        {
            case "01": ClientScript.RegisterArrayDeclaration("availableTagsBefore", "''");
                ClientScript.RegisterArrayDeclaration("availableTagsAfter", "''");
                break;//位置变动
            case "02": ClientScript.RegisterArrayDeclaration("availableTagsBefore", GetDept());
                ClientScript.RegisterArrayDeclaration("availableTagsAfter", GetDept()); ; break;//部门变动
            case "03": ClientScript.RegisterArrayDeclaration("availableTagsBefore", GetShiYongZhuangKuang());
                ClientScript.RegisterArrayDeclaration("availableTagsAfter", GetShiYongZhuangKuang()); ; break;//使用状况
            case "04": ClientScript.RegisterArrayDeclaration("availableTagsBefore", GetLeiBie());
                ClientScript.RegisterArrayDeclaration("availableTagsAfter", GetLeiBie()); ; break;//类别变动
            default:
                break;
        }
    }
    /// <summary>
    /// 通过资产[code]name和变动类别  在页面中渲染资产的信息
    /// </summary>
    /// <param name="strZiChanCode"></param>
    private void showZiChanMsg()
    {
        if (this.ddlSalewxlb.SelectedValue == null || this.txtzcCode.Text.Trim().Equals(""))
        {
            return;
        }
        string strZiChanCode = this.hdZiChanCodeForShowMsg.Value;
        string strBianDongLeiBie = this.ddlSalewxlb.SelectedValue;
        try
        {
            strZiChanCode = strZiChanCode.Substring(1, strZiChanCode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            return;
        }
        if (strZiChanCode.Equals(""))
        {
            return;
        }
        Models.ZiChan_Jilu modelZiChanJiLu = new Bll.Zichan.ZiChan_JiluBll().GetModel(strZiChanCode);
        if (modelZiChanJiLu != null)
        {
            switch (strBianDongLeiBie)
            {
                case "01": this.txtchangeq.Text = modelZiChanJiLu.WeiZhi; break;//位置
                case "02": this.txtchangeq.Text = new DepartmentManager(modelZiChanJiLu.ShiYongBuMenCode).ToString(); this.txtAddressBefore.Text = modelZiChanJiLu.WeiZhi; this.txtchangeh.Text = ""; break;//部门
                case "03": this.txtchangeq.Text = new ZiChan_ShiYongZhuangKuang(modelZiChanJiLu.ShiYongZhuangKuangCode).ToString(); this.txtchangeh.Text = ""; break;//使用状况
                case "04": this.txtchangeq.Text = new ZiChan_Leibie(modelZiChanJiLu.LeiBieCode).ToString(); this.txtchangeh.Text = ""; ; break;//类别
                default:
                    break;
            }
        }
        else
        {
            showMessage("对不起，没有找到对应的资产卡片！", false, "");
        }
    }
    #region 私有
    /// <summary>
    /// 获取资产
    /// </summary>
    /// <returns></returns>
    private string GetZcCodeAll()
    {
        DataSet ds = server.GetDataSet("select '['+ZiChanCode+']'+ZiChanName as zcnames  from ZiChan_Jilu ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["zcnames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 获取用户名
    /// </summary>
    /// <returns></returns>
    private string GetuserAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usernamecode from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usernamecode"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 获取类别
    /// </summary>
    /// <returns></returns>
    private string GetLeiBie()
    {
        DataSet ds = server.GetDataSet("select '['+LeibieCode+']'+LeibieName as LeiBieName from ZiChan_Leibie");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["LeiBieName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 获取部门
    /// </summary>
    /// <returns></returns>
    private string GetDept()
    {
        DataSet ds = server.GetDataSet("select '['+deptCode+']'+deptName as deptShowName from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptShowName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 获取使用状况
    /// </summary>
    /// <returns></returns>
    private string GetShiYongZhuangKuang()
    {
        DataSet ds = server.GetDataSet("select '['+ZhuangKuangCode+']'+ZhuangKuangName as ShowName from ZiChan_ShiYongZhuangKuang");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["ShowName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
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
    #endregion
}
