using Bll;
using Bll.newysgl;
using Bll.UserProperty;
using Dal.newysgl;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysgl_yd_ystzDetail : BasePage
{
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    string deptcode = string.Empty;//部门编号
    string ctrl = "";//页面类型 add edit look 
    public string deptname = string.Empty;
    public string billcode = string.Empty;//如果是修改的话，单据主键
    string flowid = "ystz";
    protected void Page_Load(object sender, EventArgs e)
    {
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
        }
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
            if (ctrl == "edit" || ctrl.Equals("look"))
            {
                string nd = new sqlHelper.sqlHelper().GetCellValue("select top 1 left(gcbh,4) from bill_ysmxb where billcode ='" + billcode + "'");
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
            bindData();
        }
    }
    private void bindData()
    {
        YsglMainBll bllMain = new YsglMainBll();
        string nd = this.ddlNd.SelectedValue;

        IList<Models.YsgcTb> list = bllMain.GetMainTable(deptcode, nd, this.ddlYsType.SelectedValue, "01", new string[] { "1", "2", "3", "4", "5" },"","end");
        this.GridView1.DataSource = list;
        this.GridView1.DataBind();
        if (ctrl != "add")
        {
            DataTable dtysmx = getdtysmx();
            this.GridView2.DataSource = dtysmx;
            this.GridView2.DataBind();
        }

        ConfigBLL configbll = new ConfigBLL();
        string strkzgcbhtz = configbll.GetValueByKey("kzgcbhtz");
        if (!string.IsNullOrEmpty(strkzgcbhtz) && strkzgcbhtz == "N")//设置预算调整是否控制结束了的预算过程不允许进行调整
        {
            RowsBound();
        }
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

            if (gc.IsState(gcbh.year, deptcode, flowid,""))//年度
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
            float f1 = 0;
            float.TryParse(((TextBox)e.Row.Cells[2].FindControl("txtJanuary")).Text.Trim(), out f1);
            float f2 = 0;
            float.TryParse(((TextBox)e.Row.Cells[4].FindControl("txtFebruary")).Text.Trim(), out f2);
            float f3 = 0;
            float.TryParse(((TextBox)e.Row.Cells[6].FindControl("txtmarch")).Text.Trim(), out f3);
            float f4 = 0;
            float.TryParse(((TextBox)e.Row.Cells[8].FindControl("txtApril")).Text.Trim(), out f4);
            float f5 = 0;
            float.TryParse(((TextBox)e.Row.Cells[10].FindControl("txtMay")).Text.Trim(), out f5);
            float f6 = 0;
            float.TryParse(((TextBox)e.Row.Cells[12].FindControl("txtJune")).Text.Trim(), out f6);
            float f7 = 0;
            float.TryParse(((TextBox)e.Row.Cells[14].FindControl("txtJuly")).Text.Trim(), out f7);
            float f8 = 0;
            float.TryParse(((TextBox)e.Row.Cells[16].FindControl("txtAugust")).Text.Trim(), out f8);
            float f9 = 0;
            float.TryParse(((TextBox)e.Row.Cells[18].FindControl("txtSeptember")).Text.Trim(), out f9);
            float f10 = 0;
            float.TryParse(((TextBox)e.Row.Cells[20].FindControl("txtOctober")).Text.Trim(), out f10);
            float f11 = 0;
            float.TryParse(((TextBox)e.Row.Cells[22].FindControl("txtNovember")).Text.Trim(), out f11);
            float f12 = 0;
            float.TryParse(((TextBox)e.Row.Cells[24].FindControl("txtDecember")).Text.Trim(), out f12);
            float floatYear = f1 + f2 + f3 + f4 + f5 + f6 + f7 + f8 + f9 + f10 + f11 + f12;
            ((TextBox)e.Row.Cells[26].FindControl("txtYear")).Text = floatYear.ToString();
            ((TextBox)e.Row.Cells[27].FindControl("txtYear_to")).Text = floatYear.ToString();
            //控制非末级科目行  隐藏文本框 改变颜色
            string kmbh = ((HiddenField)e.Row.Cells[26].FindControl("HiddenKmbh")).Value;
            if (!sysmanager.GetYskmIsmj(kmbh).Equals("0"))
            {
                ((TextBox)e.Row.Cells[2].FindControl("txtJanuary")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[3].FindControl("txtJanuary_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[4].FindControl("txtFebruary")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[5].FindControl("txtFebruary_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[6].FindControl("txtmarch")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[7].FindControl("txtmarch_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[8].FindControl("txtApril")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[9].FindControl("txtApril_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[10].FindControl("txtMay")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[11].FindControl("txtMay_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[12].FindControl("txtJune")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[13].FindControl("txtJune_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[14].FindControl("txtJuly")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[15].FindControl("txtJuly_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[16].FindControl("txtAugust")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[17].FindControl("txtAugust_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[18].FindControl("txtSeptember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[19].FindControl("txtSeptember_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[20].FindControl("txtOctober")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[21].FindControl("txtOctober_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[22].FindControl("txtNovember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[23].FindControl("txtNovember_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[24].FindControl("txtDecember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[25].FindControl("txtDecember_to")).CssClass = "basehidden";
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
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
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
        //if (ctrl == "add")
        //{
        //    ConfigBLL configbll = new ConfigBLL();

        //    string strtzcs = configbll.GetValueByKey("ystzcs");//设置预算调整次数  

        //    if (!string.IsNullOrEmpty(strtzcs))
        //    {
        //        string datenow = DateTime.Now.ToString("yyyy-MM");
        //        string strdept = deptcode;
        //        string strsql = @" select  count(*)  from bill_main m  where billcode in (select billcode from Bill_Ysmxb ) and flowid='ystz' and convert(varchar(7),convert(varchar(10),billdate,120)) ='" + datenow + "' and  billdept='" + strdept + "'  and dydj='" + ddlYsType.SelectedValue + "'";
        //        int strcount = Convert.ToInt32(server.GetCellValue(strsql));
        //        int inttzcs = Convert.ToInt32(strtzcs);
        //        if (strcount >= inttzcs)
        //        {
        //            ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('本月已经做过调整不允许多次调整，系统将跳转至列表页……');window.location.href='ystz.aspx'", true);
        //            return;
        //        }
        //    }
        //}

        //因为增加了调整说明  所以要重新循环gridview2
        List<Bill_Ysmxb> lstYsmxNew = new List<Bill_Ysmxb>();
        for (int i = 0; i < GridView2.Rows.Count; i++)
        {
            Bill_Ysmxb ysmx = new Bill_Ysmxb();
            ysmx.BillCode = lstYsmx[0].BillCode;
            ysmx.Gcbh = GridView2.Rows[i].Cells[4].Text.Trim();
            ysmx.YsDept = lstYsmx[0].YsDept;
            ysmx.Ysje = decimal.Parse(GridView2.Rows[i].Cells[2].Text.Trim());
            ysmx.Yskm = GridView2.Rows[i].Cells[5].Text.Trim();
            ysmx.YsType = "3";
            ysmx.Sm = ((TextBox)GridView2.Rows[i].Cells[3].FindControl("tzShuoMing")).Text.Trim();
            lstYsmxNew.Add(ysmx);
        }
        Bill_Main main = new Bill_Main();
        main.BillCode = lstYsmx[0].BillCode;
        main.BillDate = DateTime.Now;
        main.BillDept = deptcode;
        main.BillJe = lstYsmx.Where(p => p.Ysje > 0).Sum(p => p.Ysje);
        main.BillName = "预算调整单";
        main.Dydj = this.ddlYsType.SelectedValue;//存储预算科目类型 01 02 03……
        main.BillType = "3";
        main.BillUser = Session["userCode"].ToString();
        main.FlowId = "ystz";
        main.StepId = "-1";
        main.BillName2 = this.txtZy.Text.Trim();
        YsManager ysMgr = new YsManager();
        ysMgr.InsertYsmx(lstYsmxNew, main);
        ClientScript.RegisterStartupScript(this.GetType(), "a", "alert('保存成功，系统将跳转至列表页……');window.location.href='ystz.aspx'", true);
    }
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
        string strsql = @" select a.gcbh as Gcbh ,(select xmmc from bill_ysgc where gcbh=a.gcbh) as GcMc,a.yskm as Yskm,(select yskmmc from bill_yskm where yskmcode=a.yskm) as YskmMc,a.ysje as Ysje,sm from   Bill_Ysmxb a   where billcode='" + billcode + "'";
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
        //检测平衡数
        int rows = GridView1.Rows.Count;
        for (int i = 0; i < rows; i++)
        {
            string hdPingHeng = ((HiddenField)GridView1.Rows[i].Cells[28].FindControl("hdPingHengShu")).Value;
            if (!string.IsNullOrEmpty(hdPingHeng) && hdPingHeng != "0")
            {
                showMessage("对不起，第" + (i + 1) + "行调整差额不等于0，表示调整不平衡，请先修改后再保存。", false, "");
                return null;
            }
        }

        List<Models.Bill_Ysmxb> lstYsmx = new List<Models.Bill_Ysmxb>();
        if (ctrl == "add" && string.IsNullOrEmpty(billcode))
        {
            billcode = new GuidHelper().getNewGuid();
        }

        for (int i = 0; i < rows; i++)
        {
            //预算科目编号
            string yskmcode = ((HiddenField)GridView1.Rows[i].Cells[29].FindControl("HiddenKmbh")).Value;
            string yskmmc = GridView1.Rows[i].Cells[1].Text.Trim();
            #region  获取各月份预算
            string txtYs1 = ((TextBox)GridView1.Rows[i].Cells[2].FindControl("txtJanuary")).Text.Trim();
            string txtYs2 = ((TextBox)GridView1.Rows[i].Cells[4].FindControl("txtFebruary")).Text.Trim();
            string txtYs3 = ((TextBox)GridView1.Rows[i].Cells[6].FindControl("txtmarch")).Text.Trim();
            string txtYs4 = ((TextBox)GridView1.Rows[i].Cells[8].FindControl("txtApril")).Text.Trim();
            string txtYs5 = ((TextBox)GridView1.Rows[i].Cells[10].FindControl("txtMay")).Text.Trim();
            string txtYs6 = ((TextBox)GridView1.Rows[i].Cells[12].FindControl("txtJune")).Text.Trim();
            string txtYs7 = ((TextBox)GridView1.Rows[i].Cells[14].FindControl("txtJuly")).Text.Trim();
            string txtYs8 = ((TextBox)GridView1.Rows[i].Cells[16].FindControl("txtAugust")).Text.Trim();
            string txtYs9 = ((TextBox)GridView1.Rows[i].Cells[18].FindControl("txtSeptember")).Text.Trim();
            string txtYs10 = ((TextBox)GridView1.Rows[i].Cells[20].FindControl("txtOctober")).Text.Trim();
            string txtYs11 = ((TextBox)GridView1.Rows[i].Cells[22].FindControl("txtNovember")).Text.Trim();
            string txtYs12 = ((TextBox)GridView1.Rows[i].Cells[24].FindControl("txtDecember")).Text.Trim();
            float fYs1 = 0;
            float.TryParse(txtYs1, out fYs1);
            float fYs2 = 0;
            float.TryParse(txtYs2, out fYs2);
            float fYs3 = 0;
            float.TryParse(txtYs3, out fYs3);
            float fYs4 = 0;
            float.TryParse(txtYs4, out fYs4);
            float fYs5 = 0;
            float.TryParse(txtYs5, out fYs5);
            float fYs6 = 0;
            float.TryParse(txtYs6, out fYs6);
            float fYs7 = 0;
            float.TryParse(txtYs7, out fYs7);
            float fYs8 = 0;
            float.TryParse(txtYs8, out fYs8);
            float fYs9 = 0;
            float.TryParse(txtYs9, out fYs9);
            float fYs10 = 0;
            float.TryParse(txtYs10, out fYs10);
            float fYs11 = 0;
            float.TryParse(txtYs11, out fYs11);
            float fYs12 = 0;
            float.TryParse(txtYs12, out fYs12);
            #endregion

            #region 获取调整后的预算
            string txtTz1 = ((TextBox)GridView1.Rows[i].Cells[3].FindControl("txtJanuary_to")).Text.Trim();
            string txtTz2 = ((TextBox)GridView1.Rows[i].Cells[5].FindControl("txtFebruary_to")).Text.Trim();
            string txtTz3 = ((TextBox)GridView1.Rows[i].Cells[7].FindControl("txtmarch_to")).Text.Trim();
            string txtTz4 = ((TextBox)GridView1.Rows[i].Cells[9].FindControl("txtApril_to")).Text.Trim();
            string txtTz5 = ((TextBox)GridView1.Rows[i].Cells[11].FindControl("txtMay_to")).Text.Trim();
            string txtTz6 = ((TextBox)GridView1.Rows[i].Cells[13].FindControl("txtJune_to")).Text.Trim();
            string txtTz7 = ((TextBox)GridView1.Rows[i].Cells[15].FindControl("txtJuly_to")).Text.Trim();
            string txtTz8 = ((TextBox)GridView1.Rows[i].Cells[17].FindControl("txtAugust_to")).Text.Trim();
            string txtTz9 = ((TextBox)GridView1.Rows[i].Cells[19].FindControl("txtSeptember_to")).Text.Trim();
            string txtTz10 = ((TextBox)GridView1.Rows[i].Cells[21].FindControl("txtOctober_to")).Text.Trim();
            string txtTz11 = ((TextBox)GridView1.Rows[i].Cells[23].FindControl("txtNovember_to")).Text.Trim();
            string txtTz12 = ((TextBox)GridView1.Rows[i].Cells[25].FindControl("txtDecember_to")).Text.Trim();
            float fTz1 = 0;
            float.TryParse(txtTz1, out fTz1);
            float fTz2 = 0;
            float.TryParse(txtTz2, out fTz2);
            float fTz3 = 0;
            float.TryParse(txtTz3, out fTz3);
            float fTz4 = 0;
            float.TryParse(txtTz4, out fTz4);
            float fTz5 = 0;
            float.TryParse(txtTz5, out fTz5);
            float fTz6 = 0;
            float.TryParse(txtTz6, out fTz6);
            float fTz7 = 0;
            float.TryParse(txtTz7, out fTz7);
            float fTz8 = 0;
            float.TryParse(txtTz8, out fTz8);
            float fTz9 = 0;
            float.TryParse(txtTz9, out fTz9);
            float fTz10 = 0;
            float.TryParse(txtTz10, out fTz10);
            float fTz11 = 0;
            float.TryParse(txtTz11, out fTz11);
            float fTz12 = 0;
            float.TryParse(txtTz12, out fTz12);
            #endregion

            //对比计算调整差异情况
            float fCy1 = fTz1 - fYs1;
            float fCy2 = fTz2 - fYs2;
            float fCy3 = fTz3 - fYs3;
            float fCy4 = fTz4 - fYs4;
            float fCy5 = fTz5 - fYs5;
            float fCy6 = fTz6 - fYs6;
            float fCy7 = fTz7 - fYs7;
            float fCy8 = fTz8 - fYs8;
            float fCy9 = fTz9 - fYs9;
            float fCy10 = fTz10 - fYs10;
            float fCy11 = fTz11 - fYs11;
            float fCy12 = fTz12 - fYs12;

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
                tcHeader[2].Attributes.Add("colspan", "2");
                tcHeader[2].Text = "一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[3].Attributes.Add("colspan", "2");
                tcHeader[3].Text = "二月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[4].Attributes.Add("colspan", "2");
                tcHeader[4].Text = "三月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[5].Attributes.Add("colspan", "2");
                tcHeader[5].Text = "四月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[6].Attributes.Add("colspan", "2");
                tcHeader[6].Text = "五月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[7].Attributes.Add("colspan", "2");
                tcHeader[7].Text = "六月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[8].Attributes.Add("colspan", "2");
                tcHeader[8].Text = "七月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[9].Attributes.Add("colspan", "2");
                tcHeader[9].Text = "八月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[10].Attributes.Add("colspan", "2");
                tcHeader[10].Text = "九月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[11].Attributes.Add("colspan", "2");
                tcHeader[11].Text = "十月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[12].Attributes.Add("colspan", "2");
                tcHeader[12].Text = "十一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[13].Attributes.Add("colspan", "2");
                tcHeader[13].Text = "十二月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[14].Attributes.Add("colspan", "2");
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
                tcHeader[18].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[19].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[20].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[21].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[22].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[23].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[24].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[25].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[26].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[27].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[28].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[29].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[30].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[31].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[32].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[33].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[34].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[35].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[36].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[37].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[38].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[39].Text = "调整后预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[40].Text = "当前预算";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[41].Text = "调整后预算";
                break;
        }
    }
}