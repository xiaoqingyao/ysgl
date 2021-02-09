using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Models;
using System.Text;
using Bll.UserProperty;
using Bll;
using System.IO;
using Dal.UserProperty;
using Dal.Bills;

public partial class webBill_bxgl_bxDetailFinal : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    AddChangyong addcy = new AddChangyong();

    bool hasSaleRebate = false;
    ConfigBLL bllConfig = new ConfigBLL();
    MainDal maindal = new MainDal();
    string strChecking = "";
    public string title = "";
    public string hsxmModel = "";
    string strdydj = "02";//单据对应的预算类型  01收入 02费用  03固定资产  04存货 05往来 06请款单

    public string altMark = "";//标记的内容
    string strdjlx = "";
    string strfjdj = "";//是否有附加单据
    string djmxlx = "";//单据明细类型  如果是空则报销明细类型读取所有的项目   如果传入01则只读取数据字典中为01的报销明细类型
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
        ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeoptAll());
        //获取url参数 单据明细类型
        object objDjlx = Request["djmxlx"];
        if (objDjlx != null)
        {
            djmxlx = objDjlx.ToString();
        }
        //是否启用销售提成模块
        hasSaleRebate = bllConfig.GetValueByKey("HasSaleRebate").Equals("1");
        if (hasSaleRebate)//启用
        {
            this.hdHasSaleRebate.Value = "1";
            thAvailableAmount.Visible = thSaleAmount.Visible = true;
            thLeavingAmount.Visible = false;
        }
        else//没有启用销售提成模块
        {
            this.hdHasSaleRebate.Value = "0";
            thAvailableAmount.Visible = thSaleAmount.Visible = false;
            thLeavingAmount.Visible = true;
        }
        //根据配置项 是否显示项目核算 by wpp

        string strxmhs = server.GetCellValue("select isnull(avalue,'') from T_config where akey='Xmhs'");
        strfjdj = server.GetCellValue("select isnull(avalue,'') from T_config where akey='Fjdj'");

        if (!string.IsNullOrEmpty(strxmhs) && strxmhs == "N")//是否显示核算项目
        {
            thxmhs.Visible = false;
            hdxmhs.Value = "0";
        }
        else
        {
            thxmhs.Visible = true;
            hdxmhs.Value = "1";
        }

        hsxmModel = server.GetCellValue("select isnull(avalue,'') from T_config where akey='YbbxHsxmMode'");
        string strXstcje = server.GetCellValue("select isnull(avalue,'') from T_config where akey='Xstcje'");
        //获取对应单据
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdydj = Request["dydj"].ToString();
        }
        if (!string.IsNullOrEmpty(Request["djlx"]))
        {
            strdjlx = Request["djlx"];
        }
        if (!IsPostBack)
        {
            binddata();


            if (!string.IsNullOrEmpty(Request["isDz"]) && Request["isDz"] == "1")
            {
                tr_yh.Visible = true;
            }
            if (drp_xcn.SelectedValue == "是")
            {
                this.btn_yksq.Disabled = false;
            }
            else
            {
                btn_yksq.Disabled = true;
            }
            if (!string.IsNullOrEmpty(strXstcje) || strXstcje == "N")
            {
                thSaleAmount.Visible = false;
            }
            //销售提成金额是否显示用配置项配置
            string strType = Request.QueryString["type"];


            if (Page.Request.QueryString["djtype"] == null)
            {
                this.hd_djtype.Value = "ybbx";
                this.lbdjmc.Text = "费用报销单";
                title = "费用报销单";
            }
            else
            {
                this.hd_djtype.Value = Page.Request.QueryString["djtype"].ToString().Trim();
                this.lbdjmc.Text = "其他报销单";
                title = "其他报销单";
            }

            //dydj是指预算类型 根据预算类型加载不同的决算单明细类型
            string strdictype = "02";
            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                string strdydjsql = "select diccode from bill_dataDic where note3='" + Request["dydj"].ToString() + "'";
                strdictype = server.GetCellValue(strdydjsql);
            }
            //if (Request["dydj"]=="02"&&Request["djlx"]=="qkd")
            //{
            //    string strdydjsql = "select diccode from bill_dataDic where note3='06'";
            //    strdictype = server.GetCellValue(strdydjsql);

            //}
            hddictype.Value = strdictype;
            IList<Bill_DataDic> list = (new SysManager()).GetDicByType(strdictype);
            if (!string.IsNullOrEmpty(djmxlx))
            {
                list = (from c in list where c.DicCode == djmxlx select c).ToList();
            }
            drpBxmxlx.DataTextField = "DicName";
            drpBxmxlx.DataValueField = "DicCode";
            drpBxmxlx.DataSource = list;
            drpBxmxlx.DataBind();

            //2014-04-28 beg

            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                string strflowid = maindal.getJSFlowId(Request["dydj"].ToString());
                this.hd_djtype.Value = strflowid;
            }

            if ((!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "01"))
            {
                this.lbdjmc.Text = "收入报告单";
                drpBxmxlx.SelectedValue = "01";
                title = "收入报告单";
                lbl_Jbr.Text = "制单人";
                lblDate.Text = "收入日期";

                lb_bxmxlx.Text = "单据类型";
                lbl_bxzy.Text = "摘要";
                lbl_bxsm.Text = "备注";
                lbl_bxdj.Text = "附件";
                lbl_bxr.Text = "报告人";
                lblHsbm1.Text = "选择业绩归属";
                lblHsbm2.Text = "业绩归属";
                trGk.Style.Add("display", "none");
                trXm1.Style.Add("display", "none");
                trXm2.Style.Add("display", "none");
                fujiadj.Style.Add("display", "none");
                cgspInfo.Style.Add("display", "none");
            }


            //2014-04-28 end

            //2014-04-29 beg
            yklxtd1.Visible = false;
            yklxtd2.Visible = false;



            if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "02")
            {
                //if (!string.IsNullOrEmpty(strdjlx)&&strdjlx=="qkd")
                //{
                yklxtd1.Visible = true;
                yklxtd2.Visible = true;
                this.lbdjmc.Text = "用款申请单";
                drpBxmxlx.SelectedValue = "01";
                title = "用款申请单";
                lbl_Jbr.Text = "制单人";
                lblDate.Text = "用款时间";
                lbl_dept.Text = "用款部门";
                lb_bxmxlx.Text = "单据类型";
                lbl_bxzy.Text = "款项用途";
                lbl_bxsm.Text = "备注";
                lbl_bxdj.Text = "附件";
                lbl_bxr.Text = "申请人";
                lblHsbm1.Text = "选择核算部门";
                lblHsbm2.Text = "核算部门";
                //trGk.Style.Add("display", "none");
                // trbm1.Visible = false;
                // trbm2.Style.Add("display", "none");
                trXm1.Style.Add("display", "none");
                trXm2.Style.Add("display", "none");
                fujiadj.Style.Add("display", "none");
                cgspInfo.Style.Add("display", "none");
                tr_dysqd.Style.Add("display", "none");
                tr_sqdmx.Style.Add("display", "none");
                this.hd_djtype.Value = "ybbx";

                //}
                if (!string.IsNullOrEmpty(strfjdj) && strfjdj == "N")
                {

                    trfgx3.Style.Add("display", "none");
                    fujiadj.Style.Add("display", "none");
                    cgspInfo.Style.Add("display", "none");
                }
                lbl_djlx.Text = "申请类型";

            }
            if ((!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "06"))
            {
                title = "费用报销单";
                lbl_djs.Text = "附件数：";
                lbl_djlx.Text = "报销单类型";
                lbl_yskm.Text = "费用项目";
                btnAddFykm.Value = "选择费用项目";
                yklxtd1.Visible = true;
                yklxtd2.Visible = true;
                trXm1.Style.Add("display", "none");
                trXm2.Style.Add("display", "none");
                if (!string.IsNullOrEmpty(strfjdj) && strfjdj == "N")
                {
                    lblDate.Text = "报销日期";
                    trfgx3.Style.Add("display", "none");
                    fujiadj.Style.Add("display", "none");
                    cgspInfo.Style.Add("display", "none");
                }
            }
            if ((!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "03"))
            {

                this.lbdjmc.Text = "固定资产购置单";
                title = "固定资产购置单";
                drpBxmxlx.SelectedValue = "01";
                lb_bxmxlx.Text = "购置明细类型";
                lbl_bxzy.Text = "购置摘要";
                lbl_bxsm.Text = "购置说明";
                lbl_bxdj.Text = "购置单据";
                lbl_bxr.Text = "购置人";
            }
            if ((!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "04"))
            {

                this.lbdjmc.Text = "存货领料单";
                title = "存货领料单";
                drpBxmxlx.SelectedValue = "01";
                lb_bxmxlx.Text = "领料明细类型";
                lbl_bxzy.Text = "领料单摘要";
                lbl_bxsm.Text = "领料说明";
                lbl_bxdj.Text = "领料单据";
                lbl_bxr.Text = "领料人";
            }
            if ((!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "05"))
            {

                this.lbdjmc.Text = "往来付款单";
                title = "往来付款单";
                drpBxmxlx.SelectedValue = "01";
                lb_bxmxlx.Text = "付款明细类型";
                lbl_bxzy.Text = "付款摘要";
                lbl_bxsm.Text = "付款说明";
                lbl_bxdj.Text = "付款单据";
                lbl_bxr.Text = "付款人";

            }

            //2014-04-29 end

            //2015-02-07 采购支付单添加 edit by zyl begin dydj=cgzf
            if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "cgzf")
            {
                this.hd_djtype.Value = "cgzf";
                this.lbdjmc.Text = "采购支付";
                title = "采购支付";
                rb_ok.Checked = true;
                rb_can.Checked = false;
                rb_can.Enabled = false;
            }

            object objChecking = Request["checking"];
            if (objChecking != null)
            {
                strChecking = objChecking.ToString();
            }
            //是否启用销售提成模块

            if (Page.Request.QueryString["type"] == null || Page.Request.QueryString["type"].ToString().Trim() == "add")
            {
                string usercode = Convert.ToString(Session["userCode"]);
                UserMessage um = new UserMessage(usercode);
                txtJbr.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
                txtBxr.Value = "[" + um.Users.UserCode + "]" + um.Users.UserName;

                string strdeptjc = bllConfig.GetValueByKey("deptjc");//是否预算到末级

                if (!string.IsNullOrEmpty(strdeptjc) && strdeptjc == "Y")
                {
                    DepartmentDal depDal = new DepartmentDal();
                    string strdept = depDal.GetDeptByUser(usercode);
                    if (!string.IsNullOrEmpty(strdept))
                    {
                        txtDept.Value = strdept;
                    }
                }
                else
                {
                    Bill_Departments dept = um.GetRootDept();
                    txtDept.Value = "[" + dept.DeptCode + "]" + dept.DeptName;
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
                //Bill_Departments dept = um.GetRootDept();
                //txtDept.Value = "[" + dept.DeptCode + "]" + dept.DeptName;

                txtSqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
                HiddenField1.Value = (new GuidHelper()).getNewGuid();
                SysManager sysMgr = new SysManager();
                if (!string.IsNullOrEmpty(strdydj))
                {
                    Label1.Text = sysMgr.GetYbbxBillName(strdydj, DateTime.Now.ToString("yyyMMdd"), 1);
                }
                Label1.Text = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);

            }
            else if (Request.QueryString["type"] == "edit" || Request.QueryString["type"] == "look" || Page.Request.QueryString["type"].ToString().Trim() == "audit" || Request.QueryString["type"] == "mark")
            {


                //this.txtSqrq.ReadOnly = true;
                string billCode = Request.QueryString["billCode"];
                HiddenField1.Value = billCode;
                BillManager mgr = new BillManager();
                Bill_Ybbxmxb ybbx = mgr.GetYbbx(billCode);
                Bill_Main main = mgr.GetMainByCode(billCode);
                Label1.Text = main.BillName;
                UserMessage billuser = new UserMessage(main.BillUser);
                txtBxr.Value = billuser.GetName();
                UserMessage bxr = new UserMessage(ybbx.Bxr);
                txtJbr.Text = bxr.GetName();
                //判断是不是预算到末级

                string strismj = new ConfigBLL().GetValueByKey("deptjc");
                if (!string.IsNullOrEmpty(strismj) && strismj == "Y")//如果是末级
                {
                    DepartmentBLL deptbll = new DepartmentBLL();

                    string strdeptcode = main.BillDept;
                    string showdeptname = deptbll.GetShowNameByCode(strdeptcode);
                    if (!string.IsNullOrEmpty(showdeptname))
                    {
                        txtDept.Value = showdeptname;
                    }
                }
                else
                {
                    Bill_Departments depts = billuser.GetRootDept();
                    txtDept.Value = "[" + depts.DeptCode + "]" + depts.DeptName;
                }
                txtSqrq.Text = main.BillDate.Value.ToString("yyyy-MM-dd");
                drpBxmxlx.SelectedValue = ybbx.Bxmxlx;
                drp_xcn.SelectedValue = main.Note4;
                txtBxzy.Text = ybbx.Bxzy;
                txtBxsm.Text = ybbx.Bxsm;
                ddl_ykdlx.SelectedValue = ybbx.Sqlx;
                ddl_ykfs.SelectedValue = ybbx.Ykfs;
                txt_djs.Text = Convert.ToString(ybbx.Bxdjs);
                string[] arr = ybbx.Bxrzh.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arr.Length > 1)
                {
                    txtbxzh.Text = arr[1];
                    txt_khh.Value = arr[0];
                }
                string isgk = main.IsGk;
                if (string.IsNullOrEmpty(isgk) || isgk == "0")
                {
                    rb_can.Checked = true;
                    rb_ok.Checked = false;
                }
                else
                {
                    rb_can.Checked = false;
                    rb_ok.Checked = true;
                    Bill_Departments gkDept = (new SysManager()).GetDeptByCode(main.GkDept);
                    txt_gk.Value = "[" + gkDept.DeptCode + "]" + gkDept.DeptName;
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sbDept = new StringBuilder();
                StringBuilder sbXm = new StringBuilder();
                SysManager sysMgr = new SysManager();
                StringBuilder sbyksqd = new StringBuilder();//用款申请单
                string strsql = @" select (select billJe from bill_main where flowID='ybbx' and billName=dz_yksq_bxd.yksq_code) as billJe,*  from dz_yksq_bxd  where bxd_code='" + billCode + "' ";

                DataTable dtyksq = server.GetDataTable(strsql, null);
                if (dtyksq != null && dtyksq.Rows.Count != 0)
                {
                    for (int j = 0; j < dtyksq.Rows.Count; j++)
                    {
                        sbyksqd.Append("<tr id=\"tr_").Append(dtyksq.Rows[j]["yksq_code"].ToString()).Append("\" >");

                        sbyksqd.Append("<td>");
                        sbyksqd.Append(dtyksq.Rows[j]["yksq_code"].ToString());
                        sbyksqd.Append("</td>");

                        sbyksqd.Append("<td>");
                        sbyksqd.Append(dtyksq.Rows[j]["billJe"].ToString());
                        sbyksqd.Append("</td>");

                        sbyksqd.Append("<td><input type='button' style='text-align:center' class='baseButton' value='删除' onclick='del(this)'/></td>");

                        sbyksqd.Append("</tr>");
                    }
                }
                this.body_yksqmx.InnerHtml = sbyksqd.ToString();

                int i = 1;
                foreach (Bill_Ybbxmxb_Fykm fykm in ybbx.KmList)
                {
                    YsManager ysmgr = new YsManager();
                    DateTime dt = Convert.ToDateTime(txtSqrq.Text);

                    string deptCode = "";
                    if (main.IsGk == "1")
                    {
                        deptCode = main.GkDept;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strismj) && strismj == "Y")//如果是末级
                        { deptCode = billuser.GetDept().DeptCode; }
                        else
                        {
                            deptCode = billuser.GetRootDept().DeptCode;
                        }

                    }
                    string kmCode = fykm.Fykm;

                    //根据配置查看是否开启月度预算。
                    string nd = txtSqrq.Text.Substring(0, 4);

                    //根据备注文件取得预算过程号
                    string gcbh = ysmgr.GetYsgcCode(dt);
                    decimal hfje = 0;

                    //获取对应单据
                    if (!string.IsNullOrEmpty(Request["dydj"]))
                    {
                        strdydj = Request["dydj"].ToString();
                    }

                    if (!string.IsNullOrEmpty(strdydj))
                    {
                        hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                    }

                    // hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                    decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                    decimal syje = ysje - hfje;
                    decimal Tcje = 0;//提成金额
                    if (hasSaleRebate) { Tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, kmCode); }
                    decimal Kyje = syje + Tcje;//可用金额
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
                    string strKeMuShiFouXiangMuHeSuan = new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).XmHs.Equals("1") ? "是" : "否";//获取项目核算科目

                    sb.Append("<tr id=\"tr_").Append(i.ToString()).Append("\" >");

                    sb.Append("<td>");
                    sb.Append(kmname);
                    sb.Append("</td>");
                    if (string.IsNullOrEmpty(strxmhs) || strxmhs == "Y")//是否项目核算
                    {
                        sb.Append("<td>");
                        sb.Append(strKeMuShiFouXiangMuHeSuan);
                        sb.Append("</td>");
                    }
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
                        sb.Append("<td style='display:none'>");
                        sb.Append(Tcje);
                        sb.Append("</td>");

                        sb.Append("<td>");
                        sb.Append(Kyje);
                        sb.Append("</td>");
                    }

                    sb.Append("<td>");
                    sb.Append("<input type=\"text\" class=\"baseText ysje\" onblur=\"htjeChange();\" value=\"");
                    sb.Append(je.ToString()).Append("\" />");
                    sb.Append("</td>");

                    sb.Append("<td>");
                    sb.Append("<input type=\"text\" class=\"baseText ysse\" onblur=\"htjeChange();\" value=\"");
                    sb.Append(se.ToString()).Append("\" />");
                    sb.Append("</td>");

                    sb.Append("</tr>");

                    //mxl 由无序列表改为table


                    //sbDept.Append("<table id =\bm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");



                    sbDept.Append("<ul id=\"bm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");
                    foreach (Bill_Ybbxmxb_Fykm_Dept dept in fykm.DeptList)
                    {
                        string deptcode = dept.DeptCode;
                        deptcode = sysMgr.GetDeptCodeName(deptcode);
                        string depje = dept.Je.ToString();
                        sbDept.Append("<li><span>" + deptcode + ":</span><input type='text' value='" + depje + "' /></li>");
                    }
                    sbDept.Append("</ul>");

                    sbXm.Append("<ul  id=\"xm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");

                    if (fykm.XmList.Count > 0)
                    {
                        foreach (Bill_Ybbxmxb_Hsxm xm in fykm.XmList)
                        {
                            string xmcode = xm.XmCode;
                            //2014-06-30 核算项目模式配置项添加

                            if (hsxmModel != "1")
                            {
                                xmcode = sysMgr.GetXmCodeName(xmcode);
                                if (xmcode != null)
                                {
                                    string xmje = xm.Je.ToString();
                                    sbXm.Append("<li><span>" + xmcode + ":</span><input type='text' value='" + xmje + "' /></li>");
                                }
                            }
                            else
                            {

                                //2014-02-18
                                DataTable dt1 = server.GetDataSet("select a.xmCode,a.xmDept,a.je,a.isCtrl,a.nd,(select top 1 sjXm from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as sjXm,(select top 1 xmName from bill_xm where xmCode=a.xmCode and xmDept=a.xmDept) as XmName from bill_xm_dept_nd  a where a.xmCode='" + xmcode + "' and a.nd='" + nd + "'").Tables[0];


                                if (dt1.Rows.Count > 0)
                                {

                                    int index = fykm.XmList.IndexOf(xm); //index 为索引值
                                    if (index == 0)
                                    {
                                        sbXm.Append("<li class='li_xm'><table   class='myTable' style='width: 95%'>");
                                        sbXm.Append("<thead class='myGridHeader'  style='height:0px;' ><tr><th>核算项目</th><th>年度项目核算金额</th><th>是否控制项目金额</th><th>可核算金额</th><th>金额</th><tr></thead><tbody >");

                                    }

                                    string xmmc = "[" + dt1.Rows[0]["xmCode"] + "]" + dt1.Rows[0]["xmName"];
                                    string xmzje = Convert.ToString(dt1.Rows[0]["je"]);
                                    string ctrl = Convert.ToString(dt1.Rows[0]["isCtrl"]) == "1" ? "是" : "否";
                                    string xmsyje = "";
                                    string xmtbje = xm.Je.ToString();
                                    if (string.IsNullOrEmpty(Convert.ToString(xmzje)) || Convert.ToString(xmzje) == "0.00")
                                    {
                                        xmzje = "0.00";
                                        xmsyje = "0.00";
                                    }
                                    else
                                    {
                                        xmzje = string.Format("{0:N}", xmzje);
                                        string sqlje = "select isnull(SUM(je),'0') from bill_ybbxmxb_hsxm where xmCode='" + xmcode + "'";
                                        xmsyje = string.Format("{0:N}", (Convert.ToSingle(xmzje) - Convert.ToSingle(server.GetCellValue(sqlje))).ToString());
                                    }
                                    if (ctrl == "否")
                                    {
                                        xmsyje = "0.00";
                                    }
                                    sbXm.Append("<tr><td>" + xmmc + "</td><td>" + xmzje + "</td><td>" + ctrl + "</td><td>" + xmsyje + "</td><td><input type='text' class='baseText ' value='" + xmtbje + "' /></td></tr>");

                                    if (index == fykm.XmList.Count - 1)
                                        sbXm.Append("</tbody></table></li>");
                                }
                            }
                        }

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
                string sqldzfp = @" select a.billCode,a.billName,a.billJe,b.*,
	                (case stepid when '-1' then '未提交' when '0' then '审核退回' when 'begin' then '已提交' when 'end' then '审核通过' 
	                  else (select steptext from bill_workFlowStep where bill_workFlowStep.flowID='lscg' and bill_workFlowStep.stepid=a.stepid ) end) as stepID
                    from bill_main a,bill_fpfj b 
                    where a.flowid='dzfp' ";
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
                    if (fysq.SqCode.Substring(0, 2) == "cg")
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


                    if (fysq.SqCode.Substring(0, 2) == "ls")
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

                    if (fysq.SqCode.Substring(0, 4) == "dzfp")//电子发票
                    {
                        DataSet ds = server.GetDataSet(sqldzfp + "and a.billcode='" + fysq.SqCode + "'");
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
                        fysqSb.Append(ds.Tables[0].Rows[0]["deptname"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["fpusername"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["fprq"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append("");
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append(ds.Tables[0].Rows[0]["billJe"].ToString());
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append("");
                        fysqSb.Append("</td>");
                        fysqSb.Append("<td>");
                        fysqSb.Append("");
                        fysqSb.Append("</td>");
                        fysqSb.Append("</tr>");
                    }
                    iNum++;
                }


                td_dept.InnerHtml = sbDept.ToString();
                td_xm.InnerHtml = sbXm.ToString();
                this.body_fykm.InnerHtml = sb.ToString();
                tb_fysq.InnerHtml = fysqSb.ToString();

                if (strType == "look" || strType == "audit" || strType == "mark")
                {
                    this.txtJbr.Enabled = false;
                    this.txtSqrq.Enabled = false;
                    this.txtBxzy.Enabled = false;
                    this.drpBxmxlx.Enabled = false;
                    this.txtBxsm.Enabled = false;
                    this.txt_djs.Enabled = false;
                    this.rb_can.Enabled = false;
                    this.rb_ok.Enabled = false;
                    btn_sc.Visible = false;
                    txt_khh.Disabled = true;
                    txtbxzh.Enabled = false;
                    btn_test.Visible = false;
                    btnAddFykm.Visible = false;
                    if (strType == "audit" || strType == "mark")
                    {
                        txtbxzh.Text = "*****";
                        txt_khh.Value = "****************";
                        if (strType.Equals("mark"))
                        {
                            this.trMark.Visible = true;
                        }
                    }
                }
                //显示标记的内容
                DataTable dtMark = server.GetDataTable("select  mark from bill_Mark where billcode='" + billCode + "' and usercode='" + Session["usercode"].ToString() + "'", null);
                if (dtMark.Rows.Count > 0 && dtMark != null)
                {
                    altMark = dtMark.Rows[0][0].ToString();
                    this.markImg.Visible = true;
                }
                else
                {
                    //如果没有标记过 就不显示图片了
                    this.markImg.Visible = false;
                }
            }
            if ((strType == "audit" || strType == "mark") && strChecking == "true")
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
            if (bllConfig.GetModuleDisabled("HasFP"))
            {
                 this.selectBill.Items.Add(new ListItem("发票单", "fp"));
            }
            if (bllConfig.GetValueByKey("HasTravelReport").Equals("1"))
            {
                this.hdHsCCBG.Value = "1";

            }

        }
    }


    /// <summary>
    /// 绑定数据
    /// </summary>
    private void binddata()
    {
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            string fujian = server.GetCellValue("select fujian from bill_ybbxmxb where billCode ='" + Request["billCode"] + "'");
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                Lafilename.Text = "我的附件";
                upLoadFiles.Visible = false;
                btn_sc.Text = "修改附件";
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                
                for (int i = 0; i < arrname.Count() - 1; i++)
                {
                    Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";
                }
            }
            else
            {
                //如果没有附件的话
                btn_sc.Text = "上 传";
                Lafilename.Text = "";
                upLoadFiles.Visible = true;
                hiddFileDz.Value = "";
            }
        }

    }
    protected void btnScdj_Click(object sender, EventArgs e)
    {
        string filePath = "";
        string Name = "";
        string name = "";
        string exname = "";
        if (upLoadFiles.Visible == true)
        {
            string script;
            if (upLoadFiles.PostedFile.FileName == "")
            {
                laFilexx.Text = "请选择文件";
                return;
            }
            else
            {
                try
                {
                    filePath = upLoadFiles.PostedFile.FileName;
                    Name = this.upLoadFiles.PostedFile.FileName;
                    name = System.IO.Path.GetFileName(Name).Split('.')[0];
                    exname = System.IO.Path.GetExtension(Name);
                    if (isOK(exname))
                    {
                        string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                        string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        ////转换成绝对地址,
                        string serverpath = Server.MapPath(@"~\Uploads\ybbx\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\ybbx\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\ybbx\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\ybbx\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value += relativepath + ";";
                        Lafilename.Text += filename + ";";
                        laFilexx.Text = "上传成功";
                        //  btn_sc.Text = "修改附件";
                        //upLoadFiles.Visible = false;
                    }
                    else
                    {
                        Response.Write("<script>alert('文件类型不合法');</script>");
                    }
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
            upLoadFiles.Visible = true;
            Lafilename.Text = "";
        }
    }

    bool isOK(string exname)
    {
        if (exname.ToLower() == ".doc" || exname.ToLower() == ".docx" || exname.ToLower() == ".jpg" || exname.ToLower() == ".png" || exname.ToLower() == ".gif" || exname.ToLower() == ".xls" || exname.ToLower() == ".xlsx" || exname.ToLower() == ".zip" || exname.ToLower() == ".txt" || exname.ToLower() == ".pdf" || exname.ToLower() == ".rar" || exname.ToLower() == ".ppt")
        {
            return true;
        }
        else
        {
            return false;
        }

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
    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments  where sjdeptcode!='' and sjdeptcode!='000001'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
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
    protected void btnMark_Click(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        string mark = txtMark.Text.Trim();
        mark = mark + "（标记时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ")";

        string username = Session["userCode"].ToString();
        string billcode = Request.QueryString["billCode"];
        if (username.Equals("") || billcode.Equals(""))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('对不起，相关参数丢失，请关闭该页面重试。');", true);
            return;
        }
        server.ExecuteNonQuery("delete from bill_Mark where billcode='" + billcode + "' and usercode='" + username + "';insert into bill_Mark(billcode,usercode,mark) values('" + billcode + "','" + username + "','" + mark + "') ");
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('标记成功！');window.close();", true);
    }
    protected void drp_xcn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drp_xcn.SelectedValue == "是")
        {
            btn_yksq.Disabled = false;
        }
        else
        {
            btn_yksq.Disabled = true;
        }
    }
}