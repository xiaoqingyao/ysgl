using Bll;
using Bll.newysgl;
using Bll.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysgl_yd_ystzDetail_dz : BasePage
{
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    YsManager ysmgr = new YsManager();
    string deptcode = string.Empty;//部门编号
    string ctrl = "";//页面类型 add edit look 
    public string deptname = string.Empty;
    public string billcode = string.Empty;//如果是修改的话，单据主键
    string flowid = "ystz";
    string stepid = "-1";
    
    List<string> lstYskmsNoYs = new List<string>();//不占用预算的预算科目编号
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        #region 获取url参数

        object objDeptCode = Request["deptcode"];
        if (objDeptCode != null)
        {
            deptcode = objDeptCode.ToString();
            deptname = new sqlHelper.sqlHelper().GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + deptcode + "'");
        }
        object objCtrl = Request["type"];
        if (objCtrl != null)
        {
            ctrl = objCtrl.ToString();
        }
        object objBillCode = Request["billCode"];
        if (objBillCode != null)
        {
            billcode = objBillCode.ToString();
            //根据billcode 获取对应的deptcode
            string strsql = "select billdept from bill_main where billcode='" + billcode + "'";
            deptcode = server.GetCellValue(strsql);
            deptname = new sqlHelper.sqlHelper().GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + deptcode + "'");
            stepid = new sqlHelper.sqlHelper().GetCellValue("select stepid from bill_main where billcode='" + billcode + "'");
        }
        #endregion
        #region
        getYskmsNoYs();
        #endregion
        if (!IsPostBack)
        {
            //initControl
            //年度
            ddlNd.DataSource = new sqlHelper.sqlHelper().GetDataTable("select nian,xmmc from bill_ysgc where   yue='' order by nian desc", null);
            this.ddlNd.DataTextField = "xmmc";
            this.ddlNd.DataValueField = "nian";
            this.ddlNd.DataBind();
            this.ddlNd.SelectedValue = DateTime.Now.Year.ToString();
            //预算类型
            this.ddlYsType.DataSource = new sqlHelper.sqlHelper().GetDataTable("select * from bill_dataDic where dictype='18'", null);
            this.ddlYsType.DataTextField = "dicName";
            this.ddlYsType.DataValueField = "dicCode";
            this.ddlYsType.DataBind();
            this.ddlYsType.SelectedValue = "02";//默认费用预算

            if (ctrl.Equals("look"))
            {
                string nd = new sqlHelper.sqlHelper().GetCellValue("select top 1 left(gcbh,4) from bill_ysmxb where billcode ='" + billcode + "'");
                this.btnSave.Visible = false;
                if (string.IsNullOrEmpty(nd))
                {
                    showMessage("对不起，参数失效，没有找到对应的年度", false, "");
                    return;
                }
                else
                {
                    this.ddlNd.SelectedValue = nd;
                }
                //预算类型
                string ystype = new sqlHelper.sqlHelper().GetCellValue("select dydj from bill_main where billcode='" + billcode + "'");
                if (string.IsNullOrEmpty(ystype))
                {
                    showMessage("对不起，参数失效，没有找到对应的预算类型", false, "");
                    return;
                }
                else
                {
                    this.ddlYsType.SelectedValue = ystype;
                }
                this.ddlYsType.Enabled = false;
                this.txtZy.Text = new sqlHelper.sqlHelper().GetCellValue("select billName2 from bill_main where billcode='" + billcode + "'");
            }
            else if (ctrl == "audit")
            {

                btn_fh.Visible = btnSave.Visible = btnTzMx.Visible = false;
            }

            //隐藏当前预算
            if (ctrl != "add" && ctrl != "edit")
            {

                //  this.GridView1.Visible = false;
                msg.Visible = false;
                Lafilename.Visible = false;
                btn_sc.Visible = false;
                lblfj.Visible = false;
            }
            else
            {
                this.GridView1.Visible = true;
                this.btn_excelmx.Visible = false;
            }
            if (ctrl == "audit")
            {
                div_shyj.Visible = btn_ok.Visible = btn_cancel.Visible = true;
            }
            else
            {
                div_shyj.Visible = btn_ok.Visible = btn_cancel.Visible = false;
            }

            //控制审批意见区域的显示与隐藏
            if (Page.Request.QueryString["type"].ToString().Trim() == "look" || Request["type"] == "audit")
            {

                tr_shyj_history.Visible = true;
                //显示历史驳回意见
                DataTable dtHisMind = server.GetDataTable("select * from bill_ReturnHistory where billcode='" + Request.QueryString["billCode"] + "' order by dt desc", null);
                if (dtHisMind.Rows.Count == 0)
                {
                    this.txt_shyj_History.InnerHtml = "无";
                }
                else
                {
                    StringBuilder sbMind = new StringBuilder();
                    for (int i = 0; i < dtHisMind.Rows.Count; i++)
                    {
                        sbMind.Append("<br/>");

                        sbMind.Append("&nbsp;&nbsp;驳回人：");
                        sbMind.Append(dtHisMind.Rows[i]["usercode"].ToString());
                        sbMind.Append("&nbsp;&nbsp;驳回时间：");
                        sbMind.Append(dtHisMind.Rows[i]["dt"].ToString());
                        sbMind.Append("<br/>");
                        sbMind.Append("&nbsp;&nbsp;驳回意见：");
                        sbMind.Append(dtHisMind.Rows[i]["mind"].ToString());
                        sbMind.Append("<br/>");
                        sbMind.Append("<hr/>");
                    }
                    this.txt_shyj_History.InnerHtml = sbMind.ToString();
                }
            }
            else
            {
                tr_shyj_history.Visible = false;

            }
            bindData();
        }
    }
    private void bindData()
    {
        DataTable dtysmx;//预算调整明细
        if (ctrl != "add")
        {
            dtysmx = getdtysmx();
            this.GridView2.DataSource = dtysmx;
            this.GridView2.DataBind();

        }

        if (ctrl.Equals("add") || ctrl.Equals("edit"))
        {

            YsglMainBll bllMain = new YsglMainBll();
            string nd = this.ddlNd.SelectedValue;
            IList<Models.YsgcTb> list = bllMain.GetMainTable(deptcode, nd, this.ddlYsType.SelectedValue, "01", new string[] { "1", "2", "3", "4", "5", "8" }, "all", "end");


            this.GridView1.DataSource = list;
            this.GridView1.DataBind();
        }

        if (!string.IsNullOrEmpty(Request["billCode"]))
        {
            string fujian = server.GetCellValue("select top 1 note2 from bill_main where billcode='" + Request["billCode"] + "'");
            if (!string.IsNullOrEmpty(fujian))
            {
                string[] arrTemp = fujian.Split('|');
                Lafilename.Text = "我的附件";
                upLoadFiles.Visible = false;
                btn_sc.Text = "新增附件";
                string[] arrname = arrTemp[0].Split(';');
                string[] arrfile = arrTemp[1].Split(';');
                for (int i = 0; i < arrname.Length - 1; i++)
                {
                    Literal1.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件" + (i + 1) + "：</span><a href='../../AFrame/download.aspx?filename=" + Server.UrlEncode(arrname[i]) + "&filepath=" + Server.UrlEncode(arrfile[i]) + "' target='_blank'>" + arrname[i] + "下载;</a></div>";
                }
                Lafilename.Text = arrTemp[0];//显示名
                hidfilnename.Value = arrTemp[0];
                hiddFileDz.Value = arrTemp[1];
            }
            else
            {
                //如果没有附件的话
                btn_sc.Text = "上 传";
                Lafilename.Text = "";
                hidfilnename.Value = "";
                upLoadFiles.Visible = true;
                hiddFileDz.Value = "";
            }

            DataTable shdt = GetData();
            if (shdt != null)
            {
                StringBuilder sbxx = new StringBuilder();
                for (int i = 0; i < shdt.Rows.Count; i++)
                {
                    sbxx.Append("<br/>");

                    sbxx.Append("&nbsp;&nbsp;审批人：");
                    sbxx.Append(shdt.Rows[i]["checkuser"].ToString());
                    sbxx.Append("&nbsp;&nbsp;审批状态：");
                    sbxx.Append(shdt.Rows[i]["wsrdstate"].ToString());
                    sbxx.Append("&nbsp;&nbsp;审批时间：");
                    sbxx.Append(shdt.Rows[i]["checkdate1"].ToString());
                    sbxx.Append("<br/>");
                    sbxx.Append("&nbsp;&nbsp;审批意见：");
                    sbxx.Append(shdt.Rows[i]["mind"].ToString());
                    sbxx.Append("<br/>");
                    sbxx.Append("<hr/>");
                }
                this.txt_shxx_history.InnerHtml = sbxx.ToString();
            }
        }
        ConfigBLL configbll = new ConfigBLL();
        string strkzgcbhtz = configbll.GetValueByKey("kzgcbhtz");
        if (!string.IsNullOrEmpty(strkzgcbhtz) && strkzgcbhtz == "N")//设置预算调整是否控制结束了的预算过程不允许进行调整
        {
            RowsBound();
        }

    }
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
                  from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid  ");
        sb.Append(" and  billCode  = @billCode ");
        lstParameter.Add(new SqlParameter("@billCode ", strBillCode));
        return server.GetDataTable(sb.ToString(), lstParameter.ToArray());
    }
    /// <summary>
    /// 控制预算过程结束了 或者审核通过的不允许再调整
    /// </summary>
    private void RowsBound()
    {

        SysManager sysmanager = new SysManager();

        YsgcTb gcbh = bll.GetgcbhByNd(this.ddlNd.SelectedValue); // 获取预算过程编号
        IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(this.ddlNd.SelectedValue);
        Dal.Bills.YsgcDal gc = new Dal.Bills.YsgcDal();


        if (sysConfig["MonthOrQuarter"] == "2")//季度预算
        {
            #region 将月度不可用的改变背景色

            if (gc.IsState(gcbh.year, deptcode, flowid, ""))//年度
            {
                GridView1.Columns[2].ItemStyle.CssClass = "rightReadOnly";
            }

            if (gc.IsState(gcbh.January, deptcode, flowid, ""))//1月
            {
                GridView1.Columns[3].ItemStyle.CssClass = "rightReadOnly";

            }
            if (gc.IsState(gcbh.February, deptcode, flowid, ""))
            {
                GridView1.Columns[5].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.march, deptcode, flowid, ""))
            {
                GridView1.Columns[7].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.April, deptcode, flowid, ""))
            {
                GridView1.Columns[9].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.May, deptcode, flowid, ""))
            {
                GridView1.Columns[11].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.June, deptcode, flowid, ""))
            {
                GridView1.Columns[13].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.July, deptcode, flowid, ""))
            {
                GridView1.Columns[15].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.August, deptcode, flowid, ""))
            {
                GridView1.Columns[17].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.September, deptcode, flowid, ""))
            {
                GridView1.Columns[19].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.October, deptcode, flowid, ""))
            {
                GridView1.Columns[21].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.November, deptcode, flowid, ""))
            {
                GridView1.Columns[23].ItemStyle.CssClass = "rightReadOnly";
            }
            if (gc.IsState(gcbh.December, deptcode, flowid, ""))
            {
                GridView1.Columns[25].ItemStyle.CssClass = "rightReadOnly";
            }
            #endregion
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "EnbleTxt();", true);          //将背景为#DEDEDE的TD内的textbox设置为不可用
    }
    SysManager sysmanager = new SysManager();
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            //因为年度总预算不一定等于分解下去的总预算，所以年度预算要重新计算
            decimal f1 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[2].FindControl("txtJanuary")).Text.Trim(), out f1);
            decimal f2 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[5].FindControl("txtFebruary")).Text.Trim(), out f2);
            decimal f3 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[8].FindControl("txtmarch")).Text.Trim(), out f3);
            decimal f4 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[11].FindControl("txtApril")).Text.Trim(), out f4);
            decimal f5 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[14].FindControl("txtMay")).Text.Trim(), out f5);
            decimal f6 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[17].FindControl("txtJune")).Text.Trim(), out f6);
            decimal f7 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[20].FindControl("txtJuly")).Text.Trim(), out f7);
            decimal f8 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[23].FindControl("txtAugust")).Text.Trim(), out f8);
            decimal f9 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[26].FindControl("txtSeptember")).Text.Trim(), out f9);
            decimal f10 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[29].FindControl("txtOctober")).Text.Trim(), out f10);
            decimal f11 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[32].FindControl("txtNovember")).Text.Trim(), out f11);
            decimal f12 = 0;
            decimal.TryParse(((TextBox)e.Row.Cells[35].FindControl("txtDecember")).Text.Trim(), out f12);
            decimal floatYear = f1 + f2 + f3 + f4 + f5 + f6 + f7 + f8 + f9 + f10 + f11 + f12;
            ((TextBox)e.Row.Cells[38].FindControl("txtYear")).Text = floatYear.ToString();
            ((TextBox)e.Row.Cells[39].FindControl("txtYear_to")).Text = floatYear.ToString();
            //控制非末级科目行  隐藏文本框 改变颜色
            string kmbh = ((HiddenField)e.Row.Cells[42].FindControl("HiddenKmbh")).Value;
            if (!sysmanager.GetYskmIsmj(kmbh).Equals("0"))
            {
                ((TextBox)e.Row.Cells[2].FindControl("txtJanuary")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[3].FindControl("txtJanuary_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[5].FindControl("txtFebruary")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[6].FindControl("txtFebruary_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[8].FindControl("txtmarch")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[9].FindControl("txtmarch_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[11].FindControl("txtApril")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[12].FindControl("txtApril_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[14].FindControl("txtMay")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[15].FindControl("txtMay_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[17].FindControl("txtJune")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[18].FindControl("txtJune_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[20].FindControl("txtJuly")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[21].FindControl("txtJuly_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[23].FindControl("txtAugust")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[24].FindControl("txtAugust_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[26].FindControl("txtSeptember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[27].FindControl("txtSeptember_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[29].FindControl("txtOctober")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[30].FindControl("txtOctober_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[32].FindControl("txtNovember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[33].FindControl("txtNovember_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[35].FindControl("txtDecember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[36].FindControl("txtDecember_to")).CssClass = "basehidden";
                e.Row.CssClass = "unEdit";
            }
            //显示层级结构
            string yskmmc = e.Row.Cells[1].Text.Trim();
            int length = kmbh.Length - 2;
            for (int i = 0; i < length; i++)
            {
                yskmmc = "&nbsp;&nbsp;" + yskmmc;
            }
            e.Row.Cells[1].Text = yskmmc;

            //如果是退费的科目，将其隐藏
            if (yskmmc.IndexOf("退费") > 0)//退费不能调整 因为退费报销单可以超预算  其他科目虽然有不占用预算的，但是他们受预算的控制，所以还得调整 但是他们的共同属性都是不占用预算  不好控制 先暂时写死
            {
                e.Row.CssClass = "hiddenbill";
            }
            //如果是退费等不占用预算的科目，将其隐藏
            //if (lstYskmsNoYs.Contains(kmbh))
            //{
            //    e.Row.CssClass = "hiddenbill";
            //}
        }
    }

   


    protected void btnSave_Click(object sender, EventArgs e)
    {
        decimal deczys = 0;// decimal.Parse(lblzys.Text);//总预算数
        //循环调整表 计算调整了的总和
        decimal dectzys = 0;
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            TextBox txtyear = (TextBox)GridView1.Rows[i].Cells[38].FindControl("txtYear");
            TextBox txtYear_to = (TextBox)GridView1.Rows[i].Cells[39].FindControl("txtYear_to");
            if (!string.IsNullOrEmpty(txtYear_to.Text.Trim()))//调整总金额
            {
                dectzys += decimal.Parse(txtYear_to.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtyear.Text.ToString().Trim()))
            {
                deczys += decimal.Parse(txtyear.Text.ToString().Trim());
            }

            //TextBox txttzje1 = (TextBox)GridView1.Rows[i].Cells[3].FindControl("txtJanuary_to");
        }

        if (dectzys > deczys)
        {
            showMessage("调整总金额不能大于预算总金额。", false, "", "window.scrollTo(0,document.body.scrollHeight);");
            return;
        }
        List<Bill_Ysmxb> lstYsmx = getTzMx();
        //如果没有显示调整明细，直接显示并滚动过去
        int mxRows = GridView2.Rows.Count;
        if (mxRows == 0)
        {
            this.GridView2.DataSource = lstYsmx;
            this.GridView2.DataBind();
            showMessage("请填写调整说明后保存。", false, "", "window.scrollTo(0,document.body.scrollHeight);");
            return;
        }
        //检测每行都输入了调整原因
        for (int i = 0; i < mxRows; i++)
        {
            string mxShuoMing = ((TextBox)GridView2.Rows[i].Cells[3].FindControl("tzShuoMing")).Text.Trim();
            if (string.IsNullOrEmpty(mxShuoMing))
            {
                showMessage("请填写第" + (i + 1) + "行的调整明细说明", false, "", "window.scrollTo(0,document.body.scrollHeight);");
                return;
            }
        }
        if (ctrl.Equals("add") && lstYsmx == null || lstYsmx.Count <= 0)
        {
            return;
        }
        //暂时不控制次数 代码保留
        if (ctrl == "add")
        {
            ConfigBLL configbll = new ConfigBLL();

            string strtzcs = configbll.GetValueByKey("ystzcs");//设置预算调整次数  

            if (!string.IsNullOrEmpty(strtzcs))
            {
                string datenow = DateTime.Now.ToString("yyyy-MM");
                string strdept = deptcode;
                string strsql = @" select  count(*)  from bill_main m  where billcode in (select billcode from Bill_Ysmxb ) and flowid='ystz' and convert(varchar(7),convert(varchar(10),billdate,120)) ='" + datenow + "' and  billdept='" + strdept + "'  and dydj='" + ddlYsType.SelectedValue + "'";
                int strcount = Convert.ToInt32(server.GetCellValue(strsql));
                int inttzcs = Convert.ToInt32(strtzcs);
                if (strcount >= inttzcs)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('本月已经做过调整不允许多次调整，系统将跳转至列表页……');window.location.href='ystz.aspx?isdz=1'", true);
                    return;
                }
            }
        }

        //因为增加了调整说明  所以要重新循环gridview2
        List<Bill_Ysmxb> lstYsmxNew = new List<Bill_Ysmxb>();
        decimal decphs = 0;
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            Bill_Ysmxb ysmx = new Bill_Ysmxb();
            ysmx.BillCode = lstYsmx[0].BillCode;
            ysmx.Gcbh = GridView2.Rows[i].Cells[7].Text.Trim();

            ysmx.YsDept = lstYsmx[0].YsDept;
            ysmx.Ysje = decimal.Parse(GridView2.Rows[i].Cells[9].Text.Trim());
            decphs += ysmx.Ysje;
            ysmx.Yskm = GridView2.Rows[i].Cells[8].Text.Trim();
            ysmx.YsType = "3";
            ysmx.Sm = ((TextBox)GridView2.Rows[i].Cells[6].FindControl("tzShuoMing")).Text.Trim();
            lstYsmxNew.Add(ysmx);
        }
        if (decphs != 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('调出调入金额不平衡，相差" + decphs.ToString() + "不能保存，请检查后再保存。');", true);
            return;
        }

        //添加附件
        string fjname = hidfilnename.Value;
        string fjurl = hiddFileDz.Value;
        var fujian = "";
        if (!string.IsNullOrEmpty(fjname) && !string.IsNullOrEmpty(fjurl))
        {
            fujian = Server.UrlDecode(fjname + "|" + fjurl);
        }
        Bill_Main main = new Bill_Main();
        main.BillCode = lstYsmx[0].BillCode;
        main.BillDate = DateTime.Now;
        main.BillDept = deptcode;
        decimal decje = (lstYsmx.Where(p => p.Ysje > 0 && p.Gcbh.IndexOf("0001") <= 0).Sum(p => p.Ysje));
        main.BillJe = decje;
        main.BillName = "预算调整单";
        main.Dydj = this.ddlYsType.SelectedValue;//存储预算科目类型 01 02 03……
        main.BillType = "3";
        main.BillUser = Session["userCode"].ToString();
        main.FlowId = "ystz";
        main.StepId = "-1";
        main.BillName2 = this.txtZy.Text.Trim();
        main.Note2 = fujian;


        YsManager ysMgr = new YsManager();
        ysMgr.InsertYsmx(lstYsmxNew, main);
        ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('保存成功，系统将跳转至列表页……');window.location.href='ystz.aspx?isdz=1'", true);
    }
    protected void btnTzMx_Click(object sender, EventArgs e)
    {

        if (ctrl != "add" && !string.IsNullOrEmpty(billcode))
        {
            DataTable dtysmx = getdtysmx();
            this.GridView2.DataSource = dtysmx;
        }
        else
        {
            List<Bill_Ysmxb> lstYsmx = getTzMx();
            this.GridView2.DataSource = lstYsmx;
        }


        this.GridView2.DataBind();
    }
    public DataTable getdtysmx()
    {
        string strsql = @" select a.gcbh as Gcbh ,(select xmmc from bill_ysgc where gcbh=a.gcbh) as GcMc,a.yskm as Yskm
                        ,(select yskmmc from bill_yskm where yskmcode=a.yskm) as YskmMc,a.ysje as Ysje,                       
                        (
                         select isnull(sum(ysje),0) from bill_ysmxb,bill_main 
                            where gcbh=a.gcbh and ysdept=a.ysDept and yskm=a.yskm
                            and bill_main.billcode=bill_ysmxb.billcode and stepid='end'
                        )as ys,
                        ( case when ysje>0 then '调整入'else  '调整出' end) as type
                        ,sm from   Bill_Ysmxb a   where billcode='" + billcode + "'";
        DataTable dtysmx = new DataTable();
        dtysmx = server.GetDataTable(strsql, null);
        return dtysmx;
    }
    /// <summary>
    /// 获取调整明细
    /// </summary>
    /// <returns></returns>
    private List<Bill_Ysmxb> getTzMx()
    {
        int rows = GridView1.Rows.Count;

        List<Models.Bill_Ysmxb> lstYsmx = new List<Models.Bill_Ysmxb>();
        if (ctrl == "add" && string.IsNullOrEmpty(billcode))
        {
            billcode = new GuidHelper().getNewGuid();
        }

        for (int i = 0; i < rows; i++)
        {
            //预算科目编号
            string yskmcode = ((HiddenField)GridView1.Rows[i].Cells[42].FindControl("HiddenKmbh")).Value;
            string yskmmc = GridView1.Rows[i].Cells[1].Text.Trim();
            #region  获取各月份预算
            string txtYs1 = ((TextBox)GridView1.Rows[i].Cells[2].FindControl("txtJanuary")).Text.Trim();
            string txtYs2 = ((TextBox)GridView1.Rows[i].Cells[5].FindControl("txtFebruary")).Text.Trim();
            string txtYs3 = ((TextBox)GridView1.Rows[i].Cells[8].FindControl("txtmarch")).Text.Trim();
            string txtYs4 = ((TextBox)GridView1.Rows[i].Cells[11].FindControl("txtApril")).Text.Trim();
            string txtYs5 = ((TextBox)GridView1.Rows[i].Cells[14].FindControl("txtMay")).Text.Trim();
            string txtYs6 = ((TextBox)GridView1.Rows[i].Cells[17].FindControl("txtJune")).Text.Trim();
            string txtYs7 = ((TextBox)GridView1.Rows[i].Cells[20].FindControl("txtJuly")).Text.Trim();
            string txtYs8 = ((TextBox)GridView1.Rows[i].Cells[23].FindControl("txtAugust")).Text.Trim();
            string txtYs9 = ((TextBox)GridView1.Rows[i].Cells[27].FindControl("txtSeptember")).Text.Trim();
            string txtYs10 = ((TextBox)GridView1.Rows[i].Cells[30].FindControl("txtOctober")).Text.Trim();
            string txtYs11 = ((TextBox)GridView1.Rows[i].Cells[33].FindControl("txtNovember")).Text.Trim();
            string txtYs12 = ((TextBox)GridView1.Rows[i].Cells[37].FindControl("txtDecember")).Text.Trim();
            string txtYsYear = ((TextBox)GridView1.Rows[i].Cells[38].FindControl("txtYear")).Text.Trim();
            decimal fYs1 = 0;
            decimal.TryParse(txtYs1, out fYs1);
            decimal fYs2 = 0;
            decimal.TryParse(txtYs2, out fYs2);
            decimal fYs3 = 0;
            decimal.TryParse(txtYs3, out fYs3);
            decimal fYs4 = 0;
            decimal.TryParse(txtYs4, out fYs4);
            decimal fYs5 = 0;
            decimal.TryParse(txtYs5, out fYs5);
            decimal fYs6 = 0;
            decimal.TryParse(txtYs6, out fYs6);
            decimal fYs7 = 0;
            decimal.TryParse(txtYs7, out fYs7);
            decimal fYs8 = 0;
            decimal.TryParse(txtYs8, out fYs8);
            decimal fYs9 = 0;
            decimal.TryParse(txtYs9, out fYs9);
            decimal fYs10 = 0;
            decimal.TryParse(txtYs10, out fYs10);
            decimal fYs11 = 0;
            decimal.TryParse(txtYs11, out fYs11);
            decimal fYs12 = 0;
            decimal.TryParse(txtYs12, out fYs12);
            decimal fYsYear = 0;
            decimal.TryParse(txtYsYear, out fYsYear);
            #endregion
            #region 获取调整后的预算
            string txtTz1 = ((TextBox)GridView1.Rows[i].Cells[3].FindControl("txtJanuary_to")).Text.Trim();
            string txtTz2 = ((TextBox)GridView1.Rows[i].Cells[6].FindControl("txtFebruary_to")).Text.Trim();
            string txtTz3 = ((TextBox)GridView1.Rows[i].Cells[9].FindControl("txtmarch_to")).Text.Trim();
            string txtTz4 = ((TextBox)GridView1.Rows[i].Cells[12].FindControl("txtApril_to")).Text.Trim();
            string txtTz5 = ((TextBox)GridView1.Rows[i].Cells[15].FindControl("txtMay_to")).Text.Trim();
            string txtTz6 = ((TextBox)GridView1.Rows[i].Cells[18].FindControl("txtJune_to")).Text.Trim();
            string txtTz7 = ((TextBox)GridView1.Rows[i].Cells[21].FindControl("txtJuly_to")).Text.Trim();
            string txtTz8 = ((TextBox)GridView1.Rows[i].Cells[24].FindControl("txtAugust_to")).Text.Trim();
            string txtTz9 = ((TextBox)GridView1.Rows[i].Cells[27].FindControl("txtSeptember_to")).Text.Trim();
            string txtTz10 = ((TextBox)GridView1.Rows[i].Cells[30].FindControl("txtOctober_to")).Text.Trim();
            string txtTz11 = ((TextBox)GridView1.Rows[i].Cells[33].FindControl("txtNovember_to")).Text.Trim();
            string txtTz12 = ((TextBox)GridView1.Rows[i].Cells[36].FindControl("txtDecember_to")).Text.Trim();
            string txtTzYear = ((TextBox)GridView1.Rows[i].Cells[39].FindControl("txtYear_to")).Text.Trim();
            decimal fTz1 = 0;
            decimal.TryParse(txtTz1, out fTz1);
            decimal fTz2 = 0;
            decimal.TryParse(txtTz2, out fTz2);
            decimal fTz3 = 0;
            decimal.TryParse(txtTz3, out fTz3);
            decimal fTz4 = 0;
            decimal.TryParse(txtTz4, out fTz4);
            decimal fTz5 = 0;
            decimal.TryParse(txtTz5, out fTz5);
            decimal fTz6 = 0;
            decimal.TryParse(txtTz6, out fTz6);
            decimal fTz7 = 0;
            decimal.TryParse(txtTz7, out fTz7);
            decimal fTz8 = 0;
            decimal.TryParse(txtTz8, out fTz8);
            decimal fTz9 = 0;
            decimal.TryParse(txtTz9, out fTz9);
            decimal fTz10 = 0;
            decimal.TryParse(txtTz10, out fTz10);
            decimal fTz11 = 0;
            decimal.TryParse(txtTz11, out fTz11);
            decimal fTz12 = 0;
            decimal.TryParse(txtTz12, out fTz12);
            decimal fTzYear = 0;
            decimal.TryParse(txtTzYear, out fTzYear);
            #endregion

            //对比计算调整差异情况
            decimal fCy1 = fTz1 - fYs1;
            decimal fCy2 = fTz2 - fYs2;
            decimal fCy3 = fTz3 - fYs3;
            decimal fCy4 = fTz4 - fYs4;
            decimal fCy5 = fTz5 - fYs5;
            decimal fCy6 = fTz6 - fYs6;
            decimal fCy7 = fTz7 - fYs7;
            decimal fCy8 = fTz8 - fYs8;
            decimal fCy9 = fTz9 - fYs9;
            decimal fCy10 = fTz10 - fYs10;
            decimal fCy11 = fTz11 - fYs11;
            decimal fCy12 = fTz12 - fYs12;
            decimal fCyYear = fTzYear - fYsYear;
            IList<Bill_Ysgc> ysgclist = new Dal.Bills.YsgcDal().GetYsgcByNian(this.ddlNd.SelectedValue);
            #region 根据每个月的差异 生成预算调整明细
            if (fCy1 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "1").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy1;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy2 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "2").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy2;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy3 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "3").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy3;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy4 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "4").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy4;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy5 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "5").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy5;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy6 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "6").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy6;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy7 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "7").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy7;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy8 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "8").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy8;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy9 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "9").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy9;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy10 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "10").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy10;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy11 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "11").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy11;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCy12 != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "2" && p.Yue == "12").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCy12;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            if (fCyYear != 0)
            {
                Models.Bill_Ysmxb tzMx = new Models.Bill_Ysmxb();
                tzMx.BillCode = billcode;
                var ysgc = ysgclist.Where(p => p.YsType == "0" && p.Yue == "").First();
                tzMx.Gcbh = ysgc.Gcbh;
                tzMx.GcMc = ysgc.Xmmc;
                tzMx.YsDept = deptcode;
                tzMx.Ysje = (decimal)fCyYear;
                tzMx.Yskm = yskmcode;
                tzMx.YskmMc = yskmmc;
                tzMx.YsType = "3";
                lstYsmx.Add(tzMx);
            }
            #endregion


        }
        return lstYsmx;
    }
    protected void ddlYsType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                //总表头
                TableCellCollection tcHeader = e.Row.Cells;
                tcHeader.Clear();

                //第一行表头
                tcHeader.Add(new TableHeaderCell());
                tcHeader[0].Attributes.Add("rowspan", "2");
                tcHeader[0].Text = "序号";

                tcHeader.Add(new TableHeaderCell());
                tcHeader[1].Attributes.Add("rowspan", "2");
                tcHeader[1].Text = "预算科目";

                tcHeader.Add(new TableHeaderCell());
                tcHeader[2].Attributes.Add("colspan", "3");
                tcHeader[2].Text = "一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[3].Attributes.Add("colspan", "3");
                tcHeader[3].Text = "二月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[4].Attributes.Add("colspan", "3");
                tcHeader[4].Text = "三月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[5].Attributes.Add("colspan", "3");
                tcHeader[5].Text = "四月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[6].Attributes.Add("colspan", "3");
                tcHeader[6].Text = "五月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[7].Attributes.Add("colspan", "3");
                tcHeader[7].Text = "六月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[8].Attributes.Add("colspan", "3");
                tcHeader[8].Text = "七月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[9].Attributes.Add("colspan", "3");
                tcHeader[9].Text = "八月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[10].Attributes.Add("colspan", "3");
                tcHeader[10].Text = "九月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[11].Attributes.Add("colspan", "3");
                tcHeader[11].Text = "十月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[12].Attributes.Add("colspan", "3");
                tcHeader[12].Text = "十一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[13].Attributes.Add("colspan", "3");
                tcHeader[13].Text = "十二月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[14].Attributes.Add("colspan", "3");
                tcHeader[14].Text = "年度";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[15].Attributes.Add("rowspan", "2");
                tcHeader[15].Text = "调整差额</th></tr><tr id='secondtr'>";
                //第二行
                tcHeader.Add(new TableHeaderCell());
                tcHeader[16].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[17].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[18].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[19].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[20].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[21].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[22].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[23].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[24].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[25].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[26].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[27].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[28].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[29].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[30].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[31].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[32].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[33].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[34].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[35].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[36].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[37].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[38].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[39].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[40].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[41].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[42].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[43].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[44].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[45].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[46].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[47].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[48].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[49].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[50].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[51].Text = "报销金额";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[52].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[53].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[54].Text = "报销金额";
                //第三行
                break;
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
                        string serverpath = Server.MapPath(@"~\Uploads\ystz\") + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////转换成与相对地址,相对地址为将来访问图片提供
                        string relativepath = @"~\Uploads\ystz\" + fileSn + "-" + filename;//.Substring(filename.LastIndexOf("."));
                        ////绝对地址用来将上传文件夹保存到服务器的具体路下。
                        if (!Directory.Exists(Server.MapPath(@"~\Uploads\ystz\")))
                        {
                            Directory.CreateDirectory(Server.MapPath(@"~\Uploads\ystz\"));
                        }
                        upLoadFiles.PostedFile.SaveAs(serverpath);
                        ////把相对路径的地址保存到页面hdImageUrl的value值上以供保存值时使用。
                        hiddFileDz.Value += relativepath + ";";
                        Lafilename.Text += "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件" + filename + "：</span></div>";

                        hidfilnename.Value += filename + ";";
                        laFilexx.Text = "上传成功";
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
            upLoadFiles.Visible = true;

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

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string tzje = e.Row.Cells[9].Text.Trim();
            if (decimal.Parse(tzje) > 0)
            {
                e.Row.Cells[2].Text = "调整入";
                e.Row.Cells[3].Text = "";
            }
            else
            {
                e.Row.Cells[2].Text = "调整出";
                e.Row.Cells[4].Text = "";
            }
            //计算调整后金额
            string gcbh = e.Row.Cells[7].Text.Trim();
            string yskm = e.Row.Cells[8].Text.Trim();
            decimal deYs = ysmgr.GetYueYs(gcbh, deptcode, yskm);
            decimal deTzh = 0;//调整后预算
            if (stepid.Equals("-1"))
            {
                deTzh = deYs + decimal.Parse(tzje);
            }
            else
            {
                deTzh = deYs;
            }
            e.Row.Cells[5].Text = deTzh.ToString("N2");
        }
    }
    void getYskmsNoYs()
    {
        string sql = "select yskm.yskmcode from bill_yskm yskm,bill_yskm_dept dept where yskm.yskmcode=dept.yskmcode and dept.deptcode='" + deptcode + "' and  iszyys='0'";
        DataTable dt = server.GetDataTable(sql, null);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            lstYskmsNoYs.Add(dt.Rows[i][0].ToString());
        }
    }
    protected void btn_excelmx_Click(object sender, EventArgs e)
    {
        //string strhtml = txtHtml.Text;

        ////导出
        ////输出的应用类型
        //Response.ContentType = "application/vnd.ms-excel";
        //string content = strhtml;
        ////设定编码方式，若输出的excel有乱码，可优先从编码方面解决Response.Charset = "gb2312";
        //Response.ContentEncoding = System.Text.Encoding.UTF8;
        ////关闭ViewState，此属性在Page中
        //EnableViewState = false;
        ////判断是否是火狐
        //bool isFireFox = false;
        //if (Request.ServerVariables["http_user_agent"].ToLower().IndexOf("firefox") != -1)
        //{
        //    isFireFox = true;
        //}
        ////filenames是自定义的文件名
        //string filename = "tzmx.xls";
        //Response.ContentType = "application/vnd.ms-excel;charset=UTF-8";
        //if (isFireFox)
        //{
        //    filename = "\"" + filename + "\"";
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
        //}
        //else
        //{
        //    HttpUtility.UrlEncode(filename, System.Text.Encoding.UTF8);
        //    Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
        //}
        ////content是步骤1的html，注意是string类型
        //Response.Write(content);
        //Response.End();

        DataTable dtmx = getdtysmx();
        Dictionary<string, string> dic = new Dictionary<String, String>();
      //  dic.Add("Gcbh", "过程编号");
        dic.Add("GcMc", "过程名称");
        dic.Add("Yskm", "预算科目编号");
        dic.Add("YskmMc", "预算科目名称");
        dic.Add("type", "调整类型");
        dic.Add("Ysje", "调整金额");
        dic.Add("ys", "调整后的预算金额");
        dic.Add("sm", "说明");
        if (dtmx != null)
        {
            new ExcelHelper().ExpExcel(dtmx, "ExportFile", dic);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('导出失败。');", true);
            return;
        }

    }
}