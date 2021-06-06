﻿using System;
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
using System.Data.SqlClient;

public partial class webBill_bxgl_jkDetailForDz : System.Web.UI.Page
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

        thLeavingAmount.Visible = true;

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
            //控制审批意见区域的显示与隐藏
            if (Page.Request.QueryString["type"].ToString().Trim() == "audit")
            {
                tr_shyj.Visible = true;
                tr_shyj_history.Visible = true;
                //显示历史驳回意见
                DataTable dtHisMind = server.GetDataTable("select (select '['+usercode+']'+userName from bill_users where usercode=bill_ReturnHistory.usercode)as username,* from bill_ReturnHistory where billcode='" + Request.QueryString["billCode"] + "' order by dt desc", null);
                if (dtHisMind.Rows.Count == 0)
                {
                    this.txt_shyj_History.InnerHtml = "无";
                }
                else
                {
                    StringBuilder sbMind = new StringBuilder();
                    sbMind.Append("<table class='auditTable'>");
                    for (int i = 0; i < dtHisMind.Rows.Count; i++)
                    {
                        sbMind.Append("<tr>");
                        sbMind.Append("<td style='width:30px;'>" + (i + 1) + "</td>");
                        sbMind.Append("<td style='color:red; font-weight:bolder;width:100px; '>审批驳回</td>");
                        sbMind.Append("<td style='width:150px;'>" + dtHisMind.Rows[i]["username"].ToString() + "</td>");
                        sbMind.Append("<td style='width:200px;'>驳回时间：" + dtHisMind.Rows[i]["dt"].ToString() + "</td>");

                        sbMind.Append("<td>驳回意见：" + dtHisMind.Rows[i]["mind"].ToString() + "</td>");
                        sbMind.Append("</tr>");
                    }
                    sbMind.Append("</table>");
                    this.txt_shyj_History.InnerHtml = sbMind.ToString();
                }
            }
            else
            {
                tr_shyj_history.Visible = false;
                tr_shyj.Visible = false;
            }
            binddata();



            //销售提成金额是否显示用配置项配置
            string strType = Request.QueryString["type"];


            if (Page.Request.QueryString["djtype"] == null)
            {
                this.hd_djtype.Value = "tfsq";
            }
            else
            {
                this.hd_djtype.Value = Page.Request.QueryString["djtype"].ToString().Trim();
            }

            //dydj是指预算类型 根据预算类型加载不同的决算单明细类型
            //string strdictype = "02";
            //if (!string.IsNullOrEmpty(Request["dydj"]))
            //{
            //    string strdydjsql = "select diccode from bill_dataDic where note3='" + Request["dydj"].ToString() + "'";
            //    strdictype = server.GetCellValue(strdydjsql);
            //}
            //if (Request["dydj"]=="02"&&Request["djlx"]=="qkd")
            //{
            //    string strdydjsql = "select diccode from bill_dataDic where note3='06'";
            //    strdictype = server.GetCellValue(strdydjsql);

            //}
            //hddictype.Value = "03";

            //2014-04-28 beg

            if (!string.IsNullOrEmpty(Request["dydj"]))
            {
                string strflowid = maindal.getJSFlowId(Request["dydj"].ToString());
                this.hd_djtype.Value = strflowid;
            }

            if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "02")
            {
                this.hd_djtype.Value = "tfbx";
            }




            object objChecking = Request["checking"];
            if (objChecking != null)
            {
                strChecking = objChecking.ToString();
            }


            if (Page.Request.QueryString["type"] == null || Page.Request.QueryString["type"].ToString().Trim() == "add")
            {
                if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
                    return;
                }
                btn_lookpic.Visible = false;
                string usercode = Convert.ToString(Session["userCode"]);
                UserMessage um = new UserMessage(usercode);
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


                txtSqrq.Text = DateTime.Now.ToString("yyyy-MM-dd");
                SysManager sysMgr = new SysManager();
                //if (!string.IsNullOrEmpty(strdydj))
                //{
                //    Label1.Text = sysMgr.GetYbbxBillName(strdydj, DateTime.Now.ToString("yyyMMdd"), 1);
                //}
                //Label1.Text = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);
                HiddenField1.Value = sysMgr.GetYbbxBillName("TF", DateTime.Now.ToString("yyyMMdd"), 1);// (new GuidHelper()).getNewGuid();


            }
            else if (Request.QueryString["type"] == "edit" || Request.QueryString["type"] == "look" || Page.Request.QueryString["type"].ToString().Trim() == "audit" || Request.QueryString["type"] == "mark")
            {
                //this.txtSqrq.ReadOnly = true;
                string billCode = Request.QueryString["billCode"];
                HiddenField1.Value = billCode;
                BillManager mgr = new BillManager();

                IList<Bill_Main> lstmain = mgr.GetMainsByBillName(billCode);
                Bill_Ybbxmxb ybbx = mgr.GetYbbx(lstmain[0].BillCode);
                if (lstmain == null && lstmain.Count <= 0)
                {
                    return;
                }


                UserMessage billuser = new UserMessage(lstmain[0].BillUser);
                txtBxr.Value = billuser.GetName();
                UserMessage bxr = new UserMessage(ybbx.Bxr);


                //录入信息
                string[] arrxyxx = ybbx.note0.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arrxyxx.Length > 1)
                {
                    if (!string.IsNullOrEmpty(arrxyxx[0]))
                    {
                        txt_stuschool.Text = arrxyxx[0];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[1]))
                    {
                        txt_stuname.Text = arrxyxx[1];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[2]))
                    {
                        txt_stuclass.Text = arrxyxx[2];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[3]))
                    {
                        txt_stucode.Text = arrxyxx[3];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[4]))
                    {
                        txt_qdtime.Text = arrxyxx[4];
                    }
                }
                //报销费用

                string[] arrbxfy = ybbx.note1.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arrbxfy.Length > 1)
                {
                    if (!string.IsNullOrEmpty(arrbxfy[0]))
                    {
                        txt_xyfdfy.Text = arrbxfy[0];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[1]))
                    {
                        txt_yxfks.Text = arrbxfy[1];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[2]))
                    {
                        txt_dyksdj.Text = arrbxfy[2];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[3]))
                    {
                        txt_yxffy.Text = arrbxfy[3];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[4]))
                    {
                        txt_ykqtfy.Text = arrbxfy[4];
                    }
                }
                //判断是不是预算到末级
                this.txt_teacher.Text = ybbx.note2;

                string strismj = new ConfigBLL().GetValueByKey("deptjc");
                if (!string.IsNullOrEmpty(strismj) && strismj == "Y")//如果是末级
                {
                    DepartmentBLL deptbll = new DepartmentBLL();

                    string strdeptcode = lstmain[0].BillDept;
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




                txtSqrq.Text = lstmain[0].BillDate.Value.ToString("yyyy-MM-dd");

                txtBxzy.Text = ybbx.Bxzy;
                txtBxsm.Text = ybbx.Bxsm;

                txt_djs.Text = Convert.ToString(ybbx.Bxdjs);
                string[] arr = ybbx.Bxrzh.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arr.Length > 1)
                {
                    if (arr.Length == 3)
                    {
                        txtbxzh.Text = arr[1];
                        txt_khh.Value = arr[0];

                    }
                    else if (arr.Length == 2)
                    {
                        txtbxzh.Text = arr[1];
                        txt_khh.Value = arr[0];
                    }


                }

                string isgk = lstmain[0].IsGk;
                if (string.IsNullOrEmpty(isgk) || isgk == "0")
                {
                    rb_can.Checked = true;
                    rb_ok.Checked = false;
                }
                else
                {
                    rb_can.Checked = false;
                    rb_ok.Checked = true;
                    Bill_Departments gkDept = (new SysManager()).GetDeptByCode(lstmain[0].GkDept);
                    txt_gk.Value = "[" + gkDept.DeptCode + "]" + gkDept.DeptName;
                }

                StringBuilder sb = new StringBuilder();
                StringBuilder sbDept = new StringBuilder();
                StringBuilder sbXm = new StringBuilder();
                SysManager sysMgr = new SysManager();


                //显示费用报销科目
                int i = 1;
                foreach (Bill_Main main in lstmain)
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
                        deptCode = billuser.GetDept().DeptCode;
                    }

                    IList<Bill_Ybbxmxb_Fykm> fykm = new YbbxDal().GetFykm(main.BillCode);
                    for (int j = 0; j < fykm.Count; j++)
                    {
                        string kmCode = fykm[j].Fykm;
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
                            hfje = ysmgr.GetYueHf_tf(gcbh, deptCode, kmCode, strdydj);//花费金额    
                        }
                        // hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                        decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                        decimal syje = ysje - hfje;
                        decimal Tcje = 0;//提成金额
                        if (hasSaleRebate) { Tcje = ysmgr.getEffectiveSaleRebateAmount(deptCode, kmCode); }
                        decimal Kyje = syje + Tcje;//可用金额
                        if (syje < 0)
                        {
                            syje = 0;
                        }
                        string kmname = fykm[j].Fykm;
                        kmname = sysMgr.GetYskmNameCode(kmname);
                        string je = fykm[j].Je.ToString("F02");
                        string se = fykm[j].Se.ToString("F02");
                        string deptname = new SysManager().GetDeptCodeName(deptCode);
                        string strKeMuShiFouXiangMuHeSuan = new Dal.SysDictionary.YskmDal().GetYskmByCode(kmCode).XmHs.Equals("1") ? "是" : "否";//获取项目核算科目

                        sb.Append("<tr id=\"tr_").Append(i.ToString()).Append("\" >");

                        sb.Append("<td>");
                        sb.Append(kmname);
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(deptname);
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
                            sb.Append("<td>");
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
                        foreach (Bill_Ybbxmxb_Fykm_Dept dept in fykm[j].DeptList)
                        {
                            string deptcode = dept.DeptCode;
                            deptcode = sysMgr.GetDeptCodeName(deptcode);
                            string depje = dept.Je.ToString();
                            sbDept.Append("<li><span>" + deptcode + ":</span><input type='text' value='" + depje + "' /></li>");
                        }
                        sbDept.Append("</ul>");

                        sbXm.Append("<ul  id=\"xm_").Append(i.ToString()).Append("\" class=\"hiddenbill\">");

                        if (fykm[0].XmList.Count > 0)
                        {
                            foreach (Bill_Ybbxmxb_Hsxm xm in fykm[j].XmList)
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

                                        int index = fykm[j].XmList.IndexOf(xm); //index 为索引值
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

                                        if (index == fykm[j].XmList.Count - 1)
                                            sbXm.Append("</tbody></table></li>");
                                    }
                                }
                            }
                        }
                        sbXm.Append("</ul>");
                        i++;
                    }
                }
              
                td_dept.InnerHtml = sbDept.ToString();
                td_xm.InnerHtml = sbXm.ToString();
                this.body_fykm.InnerHtml = sb.ToString();


                if (strType == "look" || strType == "audit" || strType == "mark")
                {

                    this.txtSqrq.Enabled = false;
                    this.txtBxzy.Enabled = false;

                  //  this.txtBxsm.Enabled = false;
                    this.txt_djs.Enabled = false;
                    this.rb_can.Enabled = false;
                    this.rb_ok.Enabled = false;
                    //txt_khh.Disabled = true;
                    //txtbxzh.Enabled = false;

                    //txt_khh.Disabled = true;
                    btn_test.Visible = false;

                    btnAddFykm.Visible = false;
                    btnDelFykm.Visible = false;

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


            if (!string.IsNullOrEmpty(Request["billCode"]))
            {
                DataTable shdt = GetData();
                if (shdt != null)
                {
                    StringBuilder sbxx = new StringBuilder();
                    sbxx.Append("<table class='auditTable'>");
                    for (int i = 0; i < shdt.Rows.Count; i++)
                    {
                        sbxx.Append("<tr>");
                        string state = shdt.Rows[i]["wsrdstate"].ToString();
                        sbxx.Append("<td style='width:30px;'>" + (i + 1) + "</td>");
                        if (state == "审核通过")
                        {
                            sbxx.Append("<td style='color:green; font-weight:bolder;width:100px; '>" + shdt.Rows[i]["wsrdstate"].ToString() + "</td>");
                        }
                        else if (state == "正在进行")
                        {
                            sbxx.Append("<td style='font-weight:bolder; width:100px;'>" + shdt.Rows[i]["wsrdstate"].ToString() + "</td>");
                        }
                        else
                        {
                            sbxx.Append("<td style='width:100px;'>" + shdt.Rows[i]["wsrdstate"].ToString() + "</td>");
                        }
                        sbxx.Append("<td style='width:150px;'>" + shdt.Rows[i]["checkuser"].ToString() + "</td>");
                        sbxx.Append("<td style='width:200px;'>审批时间：" + shdt.Rows[i]["checkdate1"].ToString() + "</td>");
                        sbxx.Append("<td >审批意见：</td>");
                        sbxx.Append("<td >" + shdt.Rows[i]["mind"].ToString() + "</td>");
                        sbxx.Append("</tr>");
                    }
                    sbxx.Append("</table>");
                    this.txt_shxx_history.InnerHtml = sbxx.ToString();
                }
            }

        }
    }

    /// <summary>
    /// 审核意见
    /// </summary>
    /// <returns></returns>
    private DataTable GetData()
    {
        string strBillCode = "";
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            strBillCode = Request["billCode"].ToString();
        }
        List<SqlParameter> lstParameter = new List<SqlParameter>();
        StringBuilder sb = new StringBuilder();
        sb.Append(@" select Row_Number()over(order by w.flowid asc,ws.stepid asc) as crow,w.billcode,billtype,w.flowid,isEdit,
                   w.rdState as wrdstate, 
                   stepid,steptext,recordtype, isnull((select '['+usercode+']'+username from bill_users where usercode=ws.checkuser),checkuser) as checkuser,
                  isnull((select '['+usercode+']'+username from bill_users where usercode=ws.finaluser),finaluser) as finaluser,
                 (case when  ws.rdstate  ='1' then '正在进行'  when  ws.rdstate  ='0' then '等待' when  ws.rdstate  ='2' then'审核通过' when  ws.rdstate  ='3' then '驳回'  end)as wsrdstate,
                  mind, convert(varchar(10),ws.checkdate,121) as checkdate, ws.checkdate as checkdate1,checktype   
                  from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid and  billCode  = @billCode  order by stepid  ");
 
        lstParameter.Add(new SqlParameter("@billCode ", strBillCode));
        return server.GetDataTable(sb.ToString(), lstParameter.ToArray());
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void binddata()
    {
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            string fujian = server.GetCellValue("select top 1 fujian from bill_ybbxmxb where billCode =(select top 1 billcode from bill_main where billname='" + Request["billCode"] + "')");
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                for (int i = 0; i < arrname.Count() - 1; i++)
                {
                    filenames.InnerHtml += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>已上传附件：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a><a onclick='delfj(this);'>删除</a><span style='display:none'><input type='text' class='fujianurl' value='" + arrfile[i] + "'/><input type='text' class='fujianname' value='" + arrname[i] + "'/></span></div>";
                }
                //string[] arrTemp = fujian.Split('|');
                //Lafilename.Text = "我的附件";
                //upLoadFiles.Visible = false;
                //btn_sc.Text = "新增附件";
                //string[] arrname = arrTemp[0].Split(';');
                //string[] arrfile = arrTemp[1].Split(';');
                //for (int i = 0; i < arrname.Count() - 1; i++)
                //{
                //    Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";
                //}
                //Lafilename.Text = arrTemp[0];//显示名
                //hidfilnename.Value = arrTemp[0];
                //hiddFileDz.Value = arrTemp[1];
            }
            else
            {
                //如果没有附件的话
                //btn_sc.Text = "上 传";
                //Lafilename.Text = "";
                //hidfilnename.Value = "";
                //upLoadFiles.Visible = true;
                //hiddFileDz.Value = "";
            }
        }

    }
    protected void btnScdj_Click(object sender, EventArgs e)
    {
        //string filePath = "";
        //string Name = "";
        //string name = "";
        //string exname = "";
        //if (upLoadFiles.Visible == true)
        //{
        //    if (upLoadFiles.PostedFile.FileName == "")
        //    {
        //        laFilexx.Text = "请选择文件";
        //        return;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            filePath = upLoadFiles.PostedFile.FileName;
        //            Name = this.upLoadFiles.PostedFile.FileName;
        //            name = System.IO.Path.GetFileName(Name).Split('.')[0];
        //            exname = System.IO.Path.GetExtension(Name);
        //            if (isOK(exname))
        //            {
        //                string filename = filePath.Substring(filePath.LastIndexOf("\\") + 1);
        //                string fileSn = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //                ////转换成绝对地址,
        //                string serverpath = Server.MapPath(@"~\Uploads\ybbx\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
        //                ////转换成与相对地址,相对地址为将来访问图片提供
        //                string relativepath = @"~\Uploads\ybbx\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
        //                ////绝对地址用来将上传文件夹保存到服务器的具体路下。
        //                if (!Directory.Exists(Server.MapPath(@"~\Uploads\ybbx\")))
        //                {
        //                    Directory.CreateDirectory(Server.MapPath(@"~\Uploads\ybbx\"));
        //                }
        //                upLoadFiles.PostedFile.SaveAs(serverpath);
        //                ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
        //                hiddFileDz.Value += relativepath + ";";
        //                Lafilename.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件" + filename + "：</span></div>";

        //                hidfilnename.Value += filename + ";";
        //                laFilexx.Text = "上传成功";
        //                //  btn_sc.Text = "修改附件";
        //                //upLoadFiles.Visible = false;
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('文件类型不合法');</script>");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            laFilexx.Text = ex.ToString();
        //        }
        //    }
        //}
        //else
        //{
        //    btn_sc.Text = "上传";
        //    upLoadFiles.Visible = true;
        //}
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
        string str = hf_kmselect.Value;
        string[] km = str.Split('|');

        StringBuilder sb = new StringBuilder();
        for (var i = 0; i < km.Length; i++)
        {
            sb.Append("<h3><a href='#'>" + km[i] + "</a></h3><div><ul><li style='height:36px'>[金额] <input type='text' value='0.00' class='ysje' onblur='htjeChange()' id='je" + i + "' /><input type='button' value='部门选择' id='bmch" + i + "' onclick='bmChoose(this)' /><input type='button' value='项目选择' id='xmch" + i + "' onclick='xmChoose(this)' /> </li><li style='height:36px'>[税额] <input type='text' onblur='htjeChange()' class='ysse' value='0.00' id='se" + i + "' /></li><li><div id='dv_bm" + i + "'>[使用部门]</div></li><li><div id='dv_xm" + i + "'>[科目项目]</div></li></ul></div>");
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

    /// <summary>
    /// 查看所有附件图片
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_lookpic_Click(object sender, EventArgs e)
    {
        string strbillcode = "";
        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            strbillcode = Request["billCode"].ToString();
            Response.Redirect("lookpic.aspx?billcode=" + strbillcode);
        }
    }
}