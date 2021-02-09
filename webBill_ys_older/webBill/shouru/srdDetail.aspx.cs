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
using System.Text;
using Bll;
using System.Collections.Generic;
using Bll.UserProperty;
using Models;
using Dal.Bills;
using System.Data.SqlClient;

public partial class webBill_bxgl_bxDetailForGK : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    AddChangyong addcy = new AddChangyong();

    bool hasSaleRebate = false;
    ConfigBLL bllConfig = new ConfigBLL();
    string strChecking = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        ClientScript.RegisterArrayDeclaration("allzhaiyao", getAllZhaiYao());
        ClientScript.RegisterArrayDeclaration("allzh", getAllZh());
        ClientScript.RegisterArrayDeclaration("allphone", getAllphone());
        ClientScript.RegisterArrayDeclaration("allbxr2", getAllBxr2());
        ClientScript.RegisterArrayDeclaration("allkhh", getAllkhh());//开户行
        //是否启用销售提成模块
        hasSaleRebate = bllConfig.GetValueByKey("HasSaleRebate").Equals("1");
        if (hasSaleRebate)
        {
            this.hdHasSaleRebate.Value = "1";
            thAvailableAmount.Visible = thSaleAmount.Visible = true;
            thLeavingAmount.Visible = false;
        }
        else
        {
            this.hdHasSaleRebate.Value = "0";
            thAvailableAmount.Visible = thSaleAmount.Visible = false;
            thLeavingAmount.Visible = true;
        }
        if (!IsPostBack)
        {
            string strType = Request.QueryString["type"];
            if (Page.Request.QueryString["djtype"] == null)
            {
                this.hd_djtype.Value = "srd";
                this.lbdjmc.Text = "收入明细单";
            }
            else
            {
                this.hd_djtype.Value = "srd";//Page.Request.QueryString["djtype"].ToString().Trim();
                this.lbdjmc.Text = "收入明细单";
            }
            object objChecking = Request["checking"];//shenhe
            if (objChecking != null)
            {
                strChecking = objChecking.ToString();
            }
            //是否启用销售提成模块

            IList<Bill_DataDic> list = (new SysManager()).GetDicByType("02");
            drpBxmxlx.DataTextField = "DicName";
            drpBxmxlx.DataValueField = "DicCode";
            drpBxmxlx.DataSource = list;
            drpBxmxlx.DataBind();
            if (Page.Request.QueryString["type"] == null || Page.Request.QueryString["type"].ToString().Trim() == "add")
            {
                string usercode = Convert.ToString(Session["userCode"]);
                UserMessage um = new UserMessage(usercode);
                txtJbr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                txtguikoukeshi.Value = "[" + um.GetDept().DeptCode + "]" + um.GetDept().DeptName;
                txtBxr.Value = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                Bill_Departments dept = um.GetRootDept();
                txtDept.Value = "[" + dept.DeptCode + "]" + dept.DeptName;

                txtSqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
                HiddenField1.Value = (new GuidHelper()).getNewGuid();
                SysManager sysMgr = new SysManager();
                Label1.Text = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);

                //获取条码
                //if (!string.IsNullOrEmpty(this.Label1.Text))
                //{
                //    string strcode = this.Label1.Text.Trim();

                //   // this.Label2.Text = Get128CodeString(strcode);

                //    idimg.Src = "../../tmpc.aspx?strcode=" + strcode;
                //}

                HiddenField1.Value = Label1.Text;
                #region 报销人是否是上一个
                string strCurrentOrLastForYbbx = new Bll.ConfigBLL().GetValueByKey("CurrentOrLastForYbbx");
                if (strCurrentOrLastForYbbx.Equals("0") && Session["LastBXR"] != null && Session["LastBXR"].ToString() != "")
                {
                    um = new UserMessage(Session["LastBXR"].ToString());
                    txtBxr.Value = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                    dept = um.GetRootDept();
                    txtDept.Value = "[" + dept.DeptCode + "]" + dept.DeptName;
                }
                #endregion
            }
            else if (Request.QueryString["type"] == "edit" || Request.QueryString["type"] == "look" || Page.Request.QueryString["type"].ToString().Trim() == "audit")
            {
                if (Request.QueryString["type"] == "look")
                {
                    btn_test.Visible = false;
                    btn_save_commit.Visible = false;
                    btn_ok.Visible = false;
                    btn_cancel.Visible = false;
                }
                //this.txtSqrq.ReadOnly = true;
                string billCode = Request.QueryString["billCode"];
                HiddenField1.Value = billCode;

                BillManager mgr = new BillManager();
                IList<Bill_Main> lstmain = mgr.GetMainsByBillName(billCode);
                if (lstmain == null || lstmain.Count <= 0)
                {
                    return;
                }
                Bill_Ybbxmxb ybbx = mgr.GetYbbx(lstmain[0].BillCode);

                Label1.Text = lstmain[0].BillName;
                UserMessage billuser = new UserMessage(lstmain[0].BillUser);
                txtJbr.Text = billuser.GetName();//这个是归口管理员 
                txtbxr2.Text = ybbx.Bxr2;
                UserMessage bxr = new UserMessage(ybbx.Bxr);
                txtBxr.Value = bxr.GetName();//经办人
                txtDept.Value = "[" + bxr.GetDept().DeptCode + "]" + bxr.GetDept().DeptName;

                Bill_Departments depts = billuser.GetRootDept();
                txtguikoukeshi.Value = "[" + depts.DeptCode + "]" + depts.DeptName;
                txtSqrq.Text = lstmain[0].BillDate.Value.ToString("yyyy-MM-dd");
                drpBxmxlx.SelectedValue = ybbx.Bxmxlx;
                txtBxzy.Text = ybbx.Bxzy;
                txtBxsm.Text = ybbx.Bxsm;
                string[] arr = ybbx.Bxrzh.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arr.Length > 1)
                {
                    txtbxzh.Text = arr[1];
                    txt_khh.Value = arr[0];
                }

                txtbxrdh.Text = ybbx.Bxrphone;
                this.btnaddbxrdh.Enabled = false;
                this.btnaddbxzh.Enabled = false;
                this.btnAddTOChangyong.Enabled = false;

                string isgk = lstmain[0].IsGk;
                if (string.IsNullOrEmpty(isgk) || isgk == "0")
                {
                    rb_can.Checked = true;
                    rb_ok.Checked = false;
                    selectbxdept.Attributes.Add("display", "none");
                    fykmname.Attributes.Add("display", "none");
                }
                else
                {
                    //归口报销
                    rb_can.Checked = false;
                    rb_ok.Checked = true;
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sbDept = new StringBuilder();
                StringBuilder sbXm = new StringBuilder();
                SysManager sysMgr = new SysManager();
                int i = 1;
                foreach (Bill_Ybbxmxb_Fykm fykm in ybbx.KmList)
                {
                    YsManager ysmgr = new YsManager();
                    DateTime dt = Convert.ToDateTime(txtSqrq.Text);

                    string deptCode = "";
                    if (lstmain[0].IsGk == "1")
                    {
                        deptCode = lstmain[0].GkDept;
                    }
                    else
                    {
                        deptCode = billuser.GetRootDept().DeptCode;
                    }
                    string kmCode = fykm.Fykm;

                    //根据配置查看是否开启月度预算。
                    string nd = txtSqrq.Text.Substring(0, 4);
                    string config = (new SysManager()).GetsysConfigBynd(nd)["MonthOrQuarter"];

                    //根据备注文件取得预算过程号
                    string gcbh = ysmgr.GetYsgcCode(dt);
                    // gcbh;
                    //if (config == "1")
                    //{
                    //    //季度
                    //    gcbh = gcCode[1];
                    //}
                    //else if (config == "0")
                    //{
                    //    //年度
                    //    gcbh = gcCode[0];
                    //}
                    //else
                    //{
                    //    //月度
                    //    gcbh = gcCode[2];
                    //}

                    decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                    decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                    decimal syje = ysje - hfje;
                    decimal Tcje = 0;
                    if (hasSaleRebate) { Tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, kmCode); }
                    decimal Kyje = syje + Tcje;
                    //roll  beg  --2014-03-24注掉
                    //bool IsRollCtrl = new ConfigBLL().GetValueByKey("IsRollCtrl").Equals("1");
                    //decimal rollje = 0;
                    //if (IsRollCtrl)
                    //{
                    //    rollje = ysmgr.GetRollSy(gcbh, deptCode, kmCode);
                    //}
                    //syje += rollje;
                    //Kyje += rollje;
                    //end
                    string kmname = fykm.Fykm;
                    kmname = sysMgr.GetYskmNameCode(kmname);
                    string je = fykm.Je.ToString("F02");
                    string se = fykm.Se.ToString("F02");
                    string strKeMuShiFouXiangMuHeSuan = new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).XmHs.Equals("1") ? "是" : "否";

                    sb.Append("<tr id=\"tr_").Append(i.ToString()).Append("\" >");

                    sb.Append("<td>");
                    sb.Append(kmname);
                    sb.Append("</td>");

                    Bill_Departments deptmodel = new SysManager().GetDeptByCode(deptCode);
                    sb.Append("<td>");
                    sb.Append("[" + deptmodel.DeptCode + "]" + deptmodel.DeptName);
                    sb.Append("</td>");

                    sb.Append("<td>");
                    sb.Append(ysje);
                    sb.Append("</td>");

                    if (!hasSaleRebate)
                    {
                        sb.Append("<td>");
                        sb.Append(syje);
                        sb.Append("</td>");
                    }
                    else
                    {
                        sb.Append("<td>");
                        sb.Append(Tcje);
                        sb.Append("</td>");

                        sb.Append("<td>");
                        sb.Append(Kyje);
                        sb.Append("</td>");
                    }
                    //使用比例
                    sb.Append("<td>");
                    if (ysje - syje > 1)
                    {
                        sb.Append((Math.Round((ysje - syje) / ysje * 100, 2)).ToString() + "%");
                    }
                    else
                    {
                        sb.Append("0%");
                    }

                    sb.Append("</td>");

                    sb.Append("<td>");
                    sb.Append("<input type=\"text\" class=\"baseText ysje\" onblur=\"htjeChange();\" value=\"");
                    sb.Append(je.ToString()).Append("\" />");
                    sb.Append("</td>");

                    sb.Append("<td  style='display:none'>");
                    sb.Append("<input type=\"text\" class=\"baseText ysse\" onblur=\"htjeChange();\" value=\"");
                    sb.Append(se.ToString()).Append("\" />");
                    sb.Append("</td>");

                    sb.Append("</tr>");

                    sbDept.Append("<ul id=\"bm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");
                    foreach (Bill_Ybbxmxb_Fykm_Dept dept in fykm.DeptList)
                    {
                        string deptcode = dept.DeptCode;
                        deptcode = sysMgr.GetDeptCodeName(deptcode);
                        string depje = dept.Je.ToString();
                        sbDept.Append("<li><span>" + deptcode + ":</span><input type='text' value='" + depje + "' /></li>");
                    }
                    sbDept.Append("</ul>");


                    sbXm.Append("<ul id=\"xm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");
                    foreach (Bill_Ybbxmxb_Hsxm xm in fykm.XmList)
                    {
                        string xmcode = xm.XmCode;
                        xmcode = sysMgr.GetXmCodeName(xmcode);
                        string xmje = xm.Je.ToString();
                        sbXm.Append("<li><span>" + xmcode + ":</span><input type='text' value='" + xmje + "' /></li>");
                    }
                    sbXm.Append("</ul>");

                    i++;
                }

                StringBuilder fysqSb = new StringBuilder();
                string sql = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_cgsp b where a.flowid='cgsp' and a.billCode=b.cgbh ";
                string sql2 = "select (select dicname from bill_datadic where diccode=b.cglb and dictype='03') as cglb,b.sj,b.sm,b.yjfy as cgze,a.billCode,(select deptName from bill_departments where deptCode=b.cgDept) as cgDept,(select userName from bill_users where userCode=b.cbr) as  cbr,'审批通过' as spzt from bill_main a,bill_lscg b where a.flowid='lscg' and a.billCode=b.cgbh ";
                string sqlCCSQ = @"select(select dicname from bill_datadic where diccode=b.typecode and dictype='06') 
                as cclb,a.billDate,(select deptName from bill_departments where deptCode=a.billDept)
                 as Dept,(select userName from bill_users where userCode=a.billUser)
                 as  billUser ,a.billJe,'审批通过' as spzt,b.reasion,a.billCode
               from bill_main a,bill_travelApplication b where a.billCode=b.maincode";

                foreach (Bill_Ybbx_Fysq fysq in ybbx.FysqList)
                {
                    int iNum = 1;
                    if (fysq.SqCode.Substring(0, 4) == "ccsq")//出差申请
                    {
                        DataSet ds = server.GetDataSet(sqlCCSQ + " and a.billcode='" + fysq.SqCode + "'");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            fysqSb.Append("<tr>");
                            fysqSb.Append("<td><input id='radio" + iNum + "' type='radio' name='myrad' onclick='radCheck(this);' /></td>");
                            fysqSb.Append("<td>");
                            fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["Dept"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["billUser"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(new PublicServiceBLL().cutDt(ds.Tables[0].Rows[0]["billDate"].ToString()));
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["cclb"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["billJe"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["reasion"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("<td>&nbsp;");
                            fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                            fysqSb.Append("</td>");
                            fysqSb.Append("</tr>");
                        }
                    }
                    else if (fysq.SqCode.Substring(0, 2) == "cg")
                    {
                        DataSet ds = server.GetDataSet(sql + "and billcode='" + fysq.SqCode + "'");
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                            return;
                        }
                        fysqSb.Append("<tr>");
                        fysqSb.Append("<td><input id='radio" + iNum + "' type='radio' name='myrad' onclick='radCheck(this);'/></td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>&nbsp;");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>&nbsp;");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("</tr>");
                    }
                    else if (fysq.SqCode.Substring(0, 2) == "ls")
                    {
                        DataSet ds = server.GetDataSet(sql2 + "and billcode='" + fysq.SqCode + "'");
                        if (ds.Tables[0].Rows.Count <= 0)
                        {
                            return;
                        }
                        fysqSb.Append("<tr>");
                        fysqSb.Append("<td><input id='radio" + iNum + "' type='radio' name='myrad'onclick='radCheck(this);' /></td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["billCode"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgDept"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cbr"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sj"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cglb"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["cgze"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["sm"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["spzt"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("</tr>");
                    }
                    iNum++;
                }


                td_dept.InnerHtml = sbDept.ToString();
                td_xm.InnerHtml = sbXm.ToString();
                this.body_fykm.InnerHtml = sb.ToString();
                tb_fysq.InnerHtml = fysqSb.ToString();

                if (strType == "look" || strType == "audit")
                {
                    this.txtJbr.Enabled = false;
                    this.txtSqrq.Enabled = false;
                    this.txtBxzy.Enabled = false;
                    this.drpBxmxlx.Enabled = false;
                    this.txtBxsm.Enabled = false;
                    this.txtbxrdh.Enabled = false;
                    this.txtbxzh.Enabled = false;

                    btn_test.Visible = false;
                    btnAddFykm.Visible = false;
                    btn_save_commit.Visible = false;

                }
            }
            if (strType == "audit" && strChecking == "true")
            {
                this.btn_ok.Visible = true;
                this.btn_cancel.Visible = true;
            }
            else
            {
                this.btn_ok.Visible = this.btn_cancel.Visible = false;
            }
            //通过配置项添加附加单据

            if (bllConfig.GetModuleDisabled("HasBGSQ"))
            {
                this.selectBill.Items.Add(new ListItem("报告单", "bg"));
            }
            if (bllConfig.GetModuleDisabled("HasCGSP"))
            {
                this.selectBill.Items.Add(new ListItem("采购单", "cg"));
            }
            if (bllConfig.GetModuleDisabled("HasCCSQ"))
            {
                this.selectBill.Items.Add(new ListItem("出差单", "cc"));
            }
            if (bllConfig.GetValueByKey("HasTravelReport").Equals("1"))
            {
                this.hdHsCCBG.Value = "1";

            }

        }
    }
    /// <summary>
    /// 报销人账号
    /// </summary>
    /// <returns></returns>
    private string getAllZh()
    {
        string strbxr = this.txtBxr.Value.Trim();
        try
        {
            strbxr = strbxr.Substring(1, strbxr.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strbxr = "";
        }
        string strselectsql = "";
        SqlParameter[] arr;
        if (strbxr.Equals(""))
        {
            strselectsql = "select dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "12") };
        }
        else
        {
            strselectsql = "select dicname from bill_datadic where dictype='12' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strbxr) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }



    /// <summary>
    /// 报销人电话
    /// </summary>
    /// <returns></returns>
    private string getAllphone()
    {
        string strphone = this.txtbxrdh.Text.Trim();

        string strselectsql = "";
        SqlParameter[] arr;
        if (strphone.Equals(""))
        {
            strselectsql = "select dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "13") };
        }
        else
        {
            strselectsql = "select dicname from bill_datadic where dictype='13' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strphone) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;

    }




    /// <summary>
    /// 生成一维码字符串
    /// </summary>
    /// <param name="inputData"></param>
    /// <returns></returns>
    private string Get128CodeString(string inputData)
    {
        string result;
        int checksum = 104;
        for (int ii = 0; ii < inputData.Length; ii++)
        {
            if (inputData[ii] >= 32)
            {
                checksum += (inputData[ii] - 32) * (ii + 1);
            }
            else
            {
                checksum += (inputData[ii] + 64) * (ii + 1);
            }
        }
        checksum = checksum % 103;
        if (checksum < 95)
        {
            checksum += 32;
        }
        else
        {
            checksum += 100;
        }
        result = Convert.ToChar(204) + inputData.ToString() + Convert.ToChar(checksum) + Convert.ToChar(206);


        return result;
    }
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users where userStatus='1' ");
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
    /// 获取所有常用摘要 14
    /// </summary>
    /// <returns></returns>
    private string getAllZhaiYao()
    {
        string strdept = this.txtDept.Value.Trim();
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        string strselectsql = "";
        SqlParameter[] arr;
        if (strdept.Equals(""))
        {
            strselectsql = "select dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "14") };
        }
        else
        {
            strselectsql = "select dicname from bill_datadic where dictype='14' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strdept) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }
    /// <summary>
    /// 获取报销人或单位
    /// </summary>
    /// <returns></returns>
    private string getAllBxr2()
    {
        string strdept = this.txtDept.Value.Trim();
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        string strselectsql = "";
        SqlParameter[] arr;
        if (strdept.Equals(""))
        {
            strselectsql = "select dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "15") };
        }
        else
        {
            strselectsql = "select dicname from bill_datadic where dictype='15' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strdept) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }
    /// <summary>
    /// 获取所有开户行 16
    /// </summary>
    /// <returns></returns>
    private string getAllkhh()
    {
        string strdept = this.txtDept.Value.Trim();
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        string strselectsql = "";
        SqlParameter[] arr;
        if (strdept.Equals(""))
        {
            strselectsql = "select dicname from bill_datadic where dictype=@dictype  ";
            arr = new SqlParameter[] { new SqlParameter("@dictype", "16") };
        }
        else
        {
            strselectsql = "select dicname from bill_datadic where dictype='16' and diccode=@diccode ";
            arr = new SqlParameter[] { new SqlParameter("@diccode", strdept) };
        }
        DataSet ds = server.GetDataSet(strselectsql, arr);
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dicname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 0)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        return script;
    }
    protected void btn_kmselect_Click(object sender, EventArgs e)
    {
        //this.fykm.InnerHtml = "";
        string str = hf_kmselect.Value;
        string[] km = str.Split('|');

        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < km.Length; i++)
        {
            sb.Append("<h3><a href='#'>" + km[i] + "</a></h3><div><ul><li style='height:36px'>[金额] <input type='text' value='0.00' class='ysje' onblur='htjeChange()' id='je" + i + "' /><input type='button' value='部门选择' id='bmch" + i + "' onclick='bmChoose(this)' /><input type='button' value='项目选择' id='xmch" + i + "' onclick='xmChoose(this)' /> </li><li style='height:36px'>[税额] <input type='text' onblur='htjeChange()' class='ysse' value='0.00' id='se" + i + "' /></li><li><div id='dv_bm" + i + "'>[使用部门]</div></li><li><div id='dv_xm" + i + "'>[科目项目]</div></li></ul></div>");
        }
        //$("#fykm").append(innerval);
        //fykm.InnerHtml = sb.ToString();

        //txtSqrq.ReadOnly = true;
    }
    /// <summary>
    /// 报销摘要添加到常用 14
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddTOChangyong_Click(object sender, EventArgs e)
    {

        string strzy = txtBxzy.Text.Trim();//摘要
        string strdept = txtDept.Value.Trim();//部门
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        if (strdept.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先选择所在部门');", true);
            return;
        }

        int iRel = addcy.intRowAdd("14", strdept, strzy);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }
    /// <summary>
    /// 添加报销人到常用  15
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddbxrTOChangyong_Click(object sender, EventArgs e)
    {
        string strbxr2 = txtbxr2.Text.Trim();//摘要
        string strdept = txtDept.Value.Trim();//部门
        try
        {
            strdept = strdept.Substring(1, strdept.IndexOf(']') - 1);
        }
        catch (Exception)
        {
            strdept = "";
        }
        if (strdept.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先选择报销人');", true);
            return;
        }

        int iRel = addcy.intRowAdd("15", strdept, strbxr2);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }

    /// <summary>
    /// 添加报销人常用账号 12
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnaddbxzh_Click(object sender, EventArgs e)
    {
        string strzh = this.txtbxzh.Text.Trim();
        string strbxr = this.txtBxr.Value.Trim();
        try
        {
            strbxr = strbxr.Substring(1, strbxr.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strbxr = "";
        }

        if (strbxr.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择报销人')", true);
            return;
        }
        int iRel = addcy.intRowAdd("12", strbxr, strzh);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }
    /// <summary>
    /// 添加常用报销人电话 13
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnaddbxrdh_Click(object sender, EventArgs e)
    {
        string strdh = this.txtbxrdh.Text.Trim();
        string strbxr = this.txtBxr.Value.Trim();
        try
        {
            strbxr = strbxr.Substring(1, strbxr.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strbxr = "";
        }

        if (strbxr.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择报销人')", true);
            return;
        }
        int iRel = addcy.intRowAdd("13", strbxr, strdh);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }
    /// <summary>
    /// 开户行添加到常用
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnaddbxrkhh_Click(object sender, EventArgs e)
    {
        string strkhh = this.txt_khh.Value.Trim();
        string strbxr = this.txtBxr.Value.Trim();
        try
        {
            strbxr = strbxr.Substring(1, strbxr.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strbxr = "";
        }

        if (strbxr.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择报销人')", true);
            return;
        }
        int iRel = addcy.intRowAdd("16", strbxr, strkhh);
        if (iRel <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败。');", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功');", true);
        }
    }
}
