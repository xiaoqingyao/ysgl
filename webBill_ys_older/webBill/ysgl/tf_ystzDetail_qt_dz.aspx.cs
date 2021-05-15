using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll;
using Bll.newysgl;
using Bll.UserProperty;
using Dal.Bills;
using Models;
using System.Data.SqlClient;
using System.IO;
using sqlHelper;
using System.Text;

public partial class webBill_ysgl_tf_ystzDetail_qt_dz : BasePage
{
    private YsglMainBll bll = new YsglMainBll();

    private sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    private YsManager ysmgr = new YsManager();

    private string deptcode = string.Empty;

    private string ctrl = "";

    public string deptname = string.Empty;

    public string billcode = string.Empty;

    private string flowid = "ystz";

    private string stepid = "-1";

    private SysManager sysmanager = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["userCode"] == null || this.Session["userCode"].ToString().Trim() == "")
        {
            base.ClientScript.RegisterStartupScript(base.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        object obj = base.Request["deptcode"];
        if (obj != null)
        {
            this.deptcode = obj.ToString();
            this.deptname = new sqlHelper.sqlHelper().GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + this.deptcode + "'");
        }
        object obj2 = base.Request["type"];
        if (obj2 != null)
        {
            this.ctrl = obj2.ToString();
        }
        object obj3 = base.Request["billCode"];
        if (obj3 != null)
        {
            this.billcode = obj3.ToString();
            string sqlQueryString = "select billdept from bill_main where billcode='" + this.billcode + "'";
            this.deptcode = this.server.GetCellValue(sqlQueryString);
            this.deptname = new sqlHelper.sqlHelper().GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + this.deptcode + "'");
            this.stepid = new sqlHelper.sqlHelper().GetCellValue("select stepid from bill_main where billcode='" + this.billcode + "'");
        }
        if (!base.IsPostBack)
        {
            this.ddlNd.DataSource = new sqlHelper.sqlHelper().GetDataTable("select nian,xmmc from bill_ysgc where   yue='' order by nian desc", null);
            this.ddlNd.DataTextField = "xmmc";
            this.ddlNd.DataValueField = "nian";
            this.ddlNd.DataBind();
            this.ddlYsType.DataSource = new sqlHelper.sqlHelper().GetDataTable("select * from bill_dataDic where dictype='18'", null);
            this.ddlYsType.DataTextField = "dicName";
            this.ddlYsType.DataValueField = "dicCode";
            this.ddlYsType.DataBind();
            this.ddlYsType.SelectedValue = "02";
            if (this.ctrl.Equals("look"))
            {
                string cellValue = new sqlHelper.sqlHelper().GetCellValue("select top 1 left(gcbh,4) from bill_ysmxb where billcode ='" + this.billcode + "'");
                this.btnSave.Visible = false;
                if (string.IsNullOrEmpty(cellValue))
                {
                    base.showMessage("对不起，参数失效，没有找到对应的年度", false, "");
                    return;
                }
                this.ddlNd.SelectedValue = cellValue;
                string cellValue2 = new sqlHelper.sqlHelper().GetCellValue("select dydj from bill_main where billcode='" + this.billcode + "'");
                if (string.IsNullOrEmpty(cellValue2))
                {
                    base.showMessage("对不起，参数失效，没有找到对应的预算类型", false, "");
                    return;
                }
                this.ddlYsType.SelectedValue = cellValue2;
                this.ddlYsType.Enabled = false;
                this.txtZy.Text = new sqlHelper.sqlHelper().GetCellValue("select billName2 from bill_main where billcode='" + this.billcode + "'");
            }
            else if (this.ctrl == "audit")
            {
                this.btn_fh.Visible = (this.btnSave.Visible = (this.btnTzMx.Visible = false));
            }
            if (this.ctrl != "add" && this.ctrl != "edit")
            {
                this.msg.Visible = false;
                this.Lafilename.Visible = false;
                this.btn_sc.Visible = false;
                this.lblfj.Visible = false;
            }
            else
            {
                this.GridView1.Visible = true;
                this.btn_excelmx.Visible = false;
            }
            if (this.ctrl == "audit")
            {
                this.div_shyj.Visible = (this.btn_ok.Visible = (this.btn_cancel.Visible = true));
            }
            else
            {
                this.div_shyj.Visible = (this.btn_ok.Visible = (this.btn_cancel.Visible = false));
            }
            if (this.Page.Request.QueryString["type"].ToString().Trim() == "look" || base.Request["type"] == "audit")
            {
                this.tr_shyj_history.Visible = true;
                DataTable dataTable = this.server.GetDataTable("select * from bill_ReturnHistory where billcode='" + base.Request.QueryString["billCode"] + "' order by dt desc", null);
                if (dataTable.Rows.Count == 0)
                {
                    this.txt_shyj_History.InnerHtml = "无";
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        stringBuilder.Append("<br/>");
                        stringBuilder.Append("&nbsp;&nbsp;驳回人：");
                        stringBuilder.Append(dataTable.Rows[i]["usercode"].ToString());
                        stringBuilder.Append("&nbsp;&nbsp;驳回时间：");
                        stringBuilder.Append(dataTable.Rows[i]["dt"].ToString());
                        stringBuilder.Append("<br/>");
                        stringBuilder.Append("&nbsp;&nbsp;驳回意见：");
                        stringBuilder.Append(dataTable.Rows[i]["mind"].ToString());
                        stringBuilder.Append("<br/>");
                        stringBuilder.Append("<hr/>");
                    }
                    this.txt_shyj_History.InnerHtml = stringBuilder.ToString();
                }
            }
            else
            {
                this.tr_shyj_history.Visible = false;
            }
            this.bindData();
        }
    }
    private void bindData()
    {
        if (this.ctrl != "add")
        {
            DataTable dataSource = this.getdtysmx();
            this.GridView2.DataSource = dataSource;
            this.GridView2.DataBind();
        }
        if (this.ctrl.Equals("add") || this.ctrl.Equals("edit"))
        {
            YsglMainBll ysglMainBll = new YsglMainBll();
            string selectedValue = this.ddlNd.SelectedValue;
            IList<YsgcTb> mainTable_qt = ysglMainBll.GetMainTable_qt(this.deptcode, selectedValue, this.ddlYsType.SelectedValue, "01", new string[]
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "8"
            }, "all", "end");
            this.GridView1.DataSource = mainTable_qt;
            this.GridView1.DataBind();
        }
        if (!string.IsNullOrEmpty(base.Request["billCode"]))
        {
            string cellValue = this.server.GetCellValue("select top 1 note2 from bill_main where billcode='" + base.Request["billCode"] + "'");
            if (!string.IsNullOrEmpty(cellValue))
            {
                string[] array = cellValue.Split(new char[]
                {
                    '|'
                });
                this.Lafilename.Text = "我的附件";
                this.upLoadFiles.Visible = false;
                this.btn_sc.Text = "新增附件";
                string[] array2 = array[0].Split(new char[]
                {
                    ';'
                });
                string[] array3 = array[1].Split(new char[]
                {
                    ';'
                });
                for (int i = 0; i < array2.Length - 1; i++)
                {
                    Literal expr_1C3 = this.Literal1;
                    object text = expr_1C3.Text;
                    expr_1C3.Text = string.Concat(new object[]
                    {
                        text,
                        "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>附件",
                        i + 1,
                        "：</span><a href='../../AFrame/download.aspx?filename=",
                        base.Server.UrlEncode(array2[i]),
                        "&filepath=",
                        base.Server.UrlEncode(array3[i]),
                        "' target='_blank'>",
                        array2[i],
                        "下载;</a></div>"
                    });
                }
                this.Lafilename.Text = array[0];
                this.hidfilnename.Value = array[0];
                this.hiddFileDz.Value = array[1];
            }
            else
            {
                this.btn_sc.Text = "上 传";
                this.Lafilename.Text = "";
                this.hidfilnename.Value = "";
                this.upLoadFiles.Visible = true;
                this.hiddFileDz.Value = "";
            }
            DataTable data = this.GetData();
            if (data != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int j = 0; j < data.Rows.Count; j++)
                {
                    stringBuilder.Append("<br/>");
                    stringBuilder.Append("&nbsp;&nbsp;审批人：");
                    stringBuilder.Append(data.Rows[j]["checkuser"].ToString());
                    stringBuilder.Append("&nbsp;&nbsp;审批状态：");
                    stringBuilder.Append(data.Rows[j]["wsrdstate"].ToString());
                    stringBuilder.Append("&nbsp;&nbsp;审批时间：");
                    stringBuilder.Append(data.Rows[j]["checkdate1"].ToString());
                    stringBuilder.Append("<br/>");
                    stringBuilder.Append("&nbsp;&nbsp;审批意见：");
                    stringBuilder.Append(data.Rows[j]["mind"].ToString());
                    stringBuilder.Append("<br/>");
                    stringBuilder.Append("<hr/>");
                }
                this.txt_shxx_history.InnerHtml = stringBuilder.ToString();
            }
        }
        ConfigBLL configBLL = new ConfigBLL();
        string valueByKey = configBLL.GetValueByKey("kzgcbhtz");
        if (!string.IsNullOrEmpty(valueByKey) && valueByKey == "N")
        {
            this.RowsBound();
        }
    }

    private DataTable GetData()
    {
        string value = "";
        if (!string.IsNullOrEmpty(base.Request["billCode"]))
        {
            value = base.Request["billCode"].ToString();
        }
        List<SqlParameter> list = new List<SqlParameter>();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" select Row_Number()over(order by w.flowid asc,ws.stepid asc) as crow,w.billcode,billtype,w.flowid,isEdit,\r\n                   w.rdState as wrdstate, \r\n                   stepid,steptext,recordtype, isnull((select '['+usercode+']'+username from bill_users where usercode=ws.checkuser),checkuser) as checkuser,\r\n                  isnull((select '['+usercode+']'+username from bill_users where usercode=ws.finaluser),finaluser) as finaluser,\r\n                 (case when  ws.rdstate  ='1' then '正在进行'  when  ws.rdstate  ='0' then '等待' when  ws.rdstate  ='2' then'审核通过' when  ws.rdstate  ='3' then '驳回'  end)as wsrdstate,\r\n                  mind, convert(varchar(10),ws.checkdate,121) as checkdate, ws.checkdate as checkdate1,checktype   \r\n                  from workflowrecord w inner join   workflowrecords ws  on w.recordid =ws.recordid  ");
        stringBuilder.Append(" and  billCode  = @billCode ");
        list.Add(new SqlParameter("@billCode ", value));
        return this.server.GetDataTable(stringBuilder.ToString(), list.ToArray());
    }

    private void RowsBound()
    {
        new SysManager();
        YsgcTb ysgcTb = this.bll.GetgcbhByNd(this.ddlNd.SelectedValue);
        IDictionary<string, string> dictionary = new SysManager().GetsysConfigBynd(this.ddlNd.SelectedValue);
        YsgcDal ysgcDal = new YsgcDal();
        if (dictionary["MonthOrQuarter"] == "2")
        {
            if (ysgcDal.IsState(ysgcTb.year, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[2].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.January, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[3].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.February, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[5].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.march, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[7].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.April, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[9].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.May, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[11].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.June, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[13].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.July, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[15].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.August, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[17].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.September, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[19].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.October, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[21].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.November, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[23].ItemStyle.CssClass = "rightReadOnly";
            }
            if (ysgcDal.IsState(ysgcTb.December, this.deptcode, this.flowid, ""))
            {
                this.GridView1.Columns[25].ItemStyle.CssClass = "rightReadOnly";
            }
        }
        base.ClientScript.RegisterStartupScript(base.GetType(), "", "EnbleTxt();", true);
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            decimal d = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[2].FindControl("txtJanuary")).Text.Trim(), out d);
            decimal d2 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[4].FindControl("txtFebruary")).Text.Trim(), out d2);
            decimal d3 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[6].FindControl("txtmarch")).Text.Trim(), out d3);
            decimal d4 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[8].FindControl("txtApril")).Text.Trim(), out d4);
            decimal d5 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[10].FindControl("txtMay")).Text.Trim(), out d5);
            decimal d6 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[12].FindControl("txtJune")).Text.Trim(), out d6);
            decimal d7 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[14].FindControl("txtJuly")).Text.Trim(), out d7);
            decimal d8 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[16].FindControl("txtAugust")).Text.Trim(), out d8);
            decimal d9 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[18].FindControl("txtSeptember")).Text.Trim(), out d9);
            decimal d10 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[20].FindControl("txtOctober")).Text.Trim(), out d10);
            decimal d11 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[22].FindControl("txtNovember")).Text.Trim(), out d11);
            decimal d12 = 0m;
            decimal.TryParse(((TextBox)e.Row.Cells[24].FindControl("txtDecember")).Text.Trim(), out d12);
            decimal num = d + d2 + d3 + d4 + d5 + d6 + d7 + d8 + d9 + d10 + d11 + d12;
            ((TextBox)e.Row.Cells[26].FindControl("txtYear")).Text = num.ToString();
            ((TextBox)e.Row.Cells[27].FindControl("txtYear_to")).Text = num.ToString();
            string value = ((HiddenField)e.Row.Cells[28].FindControl("HiddenKmbh")).Value;
            if (!this.sysmanager.GetYskmIsmj(value).Equals("0"))
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
                ((TextBox)e.Row.Cells[13].FindControl("txtNovember_to")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[24].FindControl("txtDecember")).CssClass = "basehidden";
                ((TextBox)e.Row.Cells[25].FindControl("txtDecember_to")).CssClass = "basehidden";
                e.Row.CssClass = "unEdit";
            }
            string text = e.Row.Cells[1].Text.Trim();
            int num2 = value.Length - 2;
            for (int i = 0; i < num2; i++)
            {
                text = "&nbsp;&nbsp;" + text;
            }
            e.Row.Cells[1].Text = text;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        decimal num = 0m;
        decimal d = 0m;
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            TextBox textBox = (TextBox)this.GridView1.Rows[i].Cells[26].FindControl("txtYear");
            TextBox textBox2 = (TextBox)this.GridView1.Rows[i].Cells[27].FindControl("txtYear_to");
            if (!string.IsNullOrEmpty(textBox2.Text.Trim()))
            {
                d += decimal.Parse(textBox2.Text.Trim());
            }
            if (!string.IsNullOrEmpty(textBox.Text.ToString().Trim()))
            {
                num += decimal.Parse(textBox.Text.ToString().Trim());
            }
        }
        if (d > num)
        {
            base.showMessage("调整总金额不能大于预算总金额。", false, "", "window.scrollTo(0,document.body.scrollHeight);");
            return;
        }
        bool flag = true;
        List<Bill_Ysmxb> tzMx = this.getTzMx(out flag);
        if (!flag)
        {
            return;
        }
        int count = this.GridView2.Rows.Count;
        if (count == 0)
        {
            this.GridView2.DataSource = tzMx;
            this.GridView2.DataBind();
            base.showMessage("请填写调整说明后保存。", false, "", "window.scrollTo(0,document.body.scrollHeight);");
            return;
        }
        for (int j = 0; j < count; j++)
        {
            string value = ((TextBox)this.GridView2.Rows[j].Cells[3].FindControl("tzShuoMing")).Text.Trim();
            if (string.IsNullOrEmpty(value))
            {
                base.showMessage("请填写第" + (j + 1) + "行的调整明细说明", false, "", "window.scrollTo(0,document.body.scrollHeight);");
                return;
            }
        }
        if ((this.ctrl.Equals("add") && tzMx == null) || tzMx.Count <= 0)
        {
            return;
        }
        List<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
        decimal d2 = 0m;
        for (int k = 0; k < this.GridView2.Rows.Count; k++)
        {
            Bill_Ysmxb bill_Ysmxb = new Bill_Ysmxb();
            bill_Ysmxb.BillCode = tzMx[0].BillCode;
            bill_Ysmxb.Gcbh = this.GridView2.Rows[k].Cells[7].Text.Trim();
            bill_Ysmxb.YsDept = tzMx[0].YsDept;
            bill_Ysmxb.Ysje = decimal.Parse(this.GridView2.Rows[k].Cells[9].Text.Trim());
            d2 += bill_Ysmxb.Ysje;
            bill_Ysmxb.Yskm = this.GridView2.Rows[k].Cells[8].Text.Trim();
            bill_Ysmxb.YsType = "3";
            bill_Ysmxb.Sm = ((TextBox)this.GridView2.Rows[k].Cells[6].FindControl("tzShuoMing")).Text.Trim();
            list.Add(bill_Ysmxb);
        }
        if (d2 != 0m)
        {
            base.ClientScript.RegisterStartupScript(base.GetType(), "a", "alert('调出调入金额不平衡，相差" + d2.ToString() + "不能保存，请检查后再保存。');", true);
            return;
        }
        string value2 = this.hidfilnename.Value;
        string value3 = this.hiddFileDz.Value;
        string note = "";
        if (!string.IsNullOrEmpty(value2) && !string.IsNullOrEmpty(value3))
        {
            note = base.Server.UrlDecode(value2 + "|" + value3);
        }
        Bill_Main bill_Main = new Bill_Main();
        bill_Main.BillCode = tzMx[0].BillCode;
        bill_Main.BillDate = new DateTime?(DateTime.Now);
        bill_Main.BillDept = this.deptcode;
        decimal num2 = (tzMx.Where(p => p.Ysje > 0 && !p.Gcbh.EndsWith("0001")).Sum(p => p.Ysje));

        if (num2 == 0m)
        {
            base.showMessage("对不起，调整金额不能为0，请联系管理员解决", false, "");
            return;
        }
        bill_Main.BillJe = num2;
        bill_Main.BillName = "其他预算调整单";
        bill_Main.Dydj = this.ddlYsType.SelectedValue;
        bill_Main.BillType = "3";
        bill_Main.BillUser = this.Session["userCode"].ToString();
        bill_Main.FlowId = "ystz";
        bill_Main.StepId = "-1";
        bill_Main.BillName2 = this.txtZy.Text.Trim();
        bill_Main.Note2 = note;
        YsManager ysManager = new YsManager();
        ysManager.InsertYsmx(list, bill_Main);
        base.ClientScript.RegisterStartupScript(base.GetType(), "a", "alert('保存成功，系统将跳转至列表页……');window.location.href='ystz_qt_dz.aspx?isdz=1'", true);
    }

    protected void btnTzMx_Click(object sender, EventArgs e)
    {
        if (this.ctrl != "add" && !string.IsNullOrEmpty(this.billcode))
        {
            DataTable dataSource = this.getdtysmx();
            this.GridView2.DataSource = dataSource;
        }
        else
        {
            bool flag = true;
            List<Bill_Ysmxb> tzMx = this.getTzMx(out flag);
            this.GridView2.DataSource = tzMx;
        }
        this.GridView2.DataBind();
    }

    public DataTable getdtysmx()
    {
        string sqlQueryString = " select a.gcbh as Gcbh ,(select xmmc from bill_ysgc where gcbh=a.gcbh) as GcMc,a.yskm as Yskm\r\n                        ,(select yskmmc from bill_yskm where yskmcode=a.yskm) as YskmMc,a.ysje as Ysje,                       \r\n                        (\r\n                         select isnull(sum(ysje),0) from bill_ysmxb,bill_main \r\n                            where gcbh=a.gcbh and ysdept=a.ysDept and yskm=a.yskm\r\n                            and bill_main.billcode=bill_ysmxb.billcode and stepid='end'\r\n                        )as ys,\r\n                        ( case when ysje>0 then '调整入'else  '调整出' end) as type\r\n                        ,sm from   Bill_Ysmxb a   where billcode='" + this.billcode + "'";
        DataTable dataTable = new DataTable();
        return this.server.GetDataTable(sqlQueryString, null);
    }

    private List<Bill_Ysmxb> getTzMx(out bool flg)
    {
        flg = true;
        int count = this.GridView1.Rows.Count;
        List<Bill_Ysmxb> list = new List<Bill_Ysmxb>();
        if (this.ctrl == "add" && string.IsNullOrEmpty(this.billcode))
        {
            this.billcode = new GuidHelper().getNewGuid();
        }
        for (int i = 0; i < count; i++)
        {
            string value = ((HiddenField)this.GridView1.Rows[i].Cells[29].FindControl("HiddenKmbh")).Value;
            string yskmMc = this.GridView1.Rows[i].Cells[1].Text.Trim();
            string s = ((TextBox)this.GridView1.Rows[i].Cells[2].FindControl("txtJanuary")).Text.Trim();
            string s2 = ((TextBox)this.GridView1.Rows[i].Cells[4].FindControl("txtFebruary")).Text.Trim();
            string s3 = ((TextBox)this.GridView1.Rows[i].Cells[6].FindControl("txtmarch")).Text.Trim();
            string s4 = ((TextBox)this.GridView1.Rows[i].Cells[8].FindControl("txtApril")).Text.Trim();
            string s5 = ((TextBox)this.GridView1.Rows[i].Cells[10].FindControl("txtMay")).Text.Trim();
            string s6 = ((TextBox)this.GridView1.Rows[i].Cells[12].FindControl("txtJune")).Text.Trim();
            string s7 = ((TextBox)this.GridView1.Rows[i].Cells[14].FindControl("txtJuly")).Text.Trim();
            string s8 = ((TextBox)this.GridView1.Rows[i].Cells[16].FindControl("txtAugust")).Text.Trim();
            string s9 = ((TextBox)this.GridView1.Rows[i].Cells[18].FindControl("txtSeptember")).Text.Trim();
            string s10 = ((TextBox)this.GridView1.Rows[i].Cells[20].FindControl("txtOctober")).Text.Trim();
            string s11 = ((TextBox)this.GridView1.Rows[i].Cells[22].FindControl("txtNovember")).Text.Trim();
            string s12 = ((TextBox)this.GridView1.Rows[i].Cells[24].FindControl("txtDecember")).Text.Trim();
            string s13 = ((TextBox)this.GridView1.Rows[i].Cells[26].FindControl("txtYear")).Text.Trim();
            decimal d = 0m;
            decimal.TryParse(s, out d);
            decimal d2 = 0m;
            decimal.TryParse(s2, out d2);
            decimal d3 = 0m;
            decimal.TryParse(s3, out d3);
            decimal d4 = 0m;
            decimal.TryParse(s4, out d4);
            decimal d5 = 0m;
            decimal.TryParse(s5, out d5);
            decimal d6 = 0m;
            decimal.TryParse(s6, out d6);
            decimal d7 = 0m;
            decimal.TryParse(s7, out d7);
            decimal d8 = 0m;
            decimal.TryParse(s8, out d8);
            decimal d9 = 0m;
            decimal.TryParse(s9, out d9);
            decimal d10 = 0m;
            decimal.TryParse(s10, out d10);
            decimal d11 = 0m;
            decimal.TryParse(s11, out d11);
            decimal d12 = 0m;
            decimal.TryParse(s12, out d12);
            decimal d13 = 0m;
            decimal.TryParse(s13, out d13);
            string s14 = ((TextBox)this.GridView1.Rows[i].Cells[3].FindControl("txtJanuary_to")).Text.Trim();
            string s15 = ((TextBox)this.GridView1.Rows[i].Cells[5].FindControl("txtFebruary_to")).Text.Trim();
            string s16 = ((TextBox)this.GridView1.Rows[i].Cells[7].FindControl("txtmarch_to")).Text.Trim();
            string s17 = ((TextBox)this.GridView1.Rows[i].Cells[9].FindControl("txtApril_to")).Text.Trim();
            string s18 = ((TextBox)this.GridView1.Rows[i].Cells[11].FindControl("txtMay_to")).Text.Trim();
            string s19 = ((TextBox)this.GridView1.Rows[i].Cells[13].FindControl("txtJune_to")).Text.Trim();
            string s20 = ((TextBox)this.GridView1.Rows[i].Cells[15].FindControl("txtJuly_to")).Text.Trim();
            string s21 = ((TextBox)this.GridView1.Rows[i].Cells[17].FindControl("txtAugust_to")).Text.Trim();
            string s22 = ((TextBox)this.GridView1.Rows[i].Cells[19].FindControl("txtSeptember_to")).Text.Trim();
            string s23 = ((TextBox)this.GridView1.Rows[i].Cells[21].FindControl("txtOctober_to")).Text.Trim();
            string s24 = ((TextBox)this.GridView1.Rows[i].Cells[23].FindControl("txtNovember_to")).Text.Trim();
            string s25 = ((TextBox)this.GridView1.Rows[i].Cells[25].FindControl("txtDecember_to")).Text.Trim();
            string s26 = ((TextBox)this.GridView1.Rows[i].Cells[27].FindControl("txtYear_to")).Text.Trim();
            decimal d14 = 0m;
            decimal.TryParse(s14, out d14);
            decimal d15 = 0m;
            decimal.TryParse(s15, out d15);
            decimal d16 = 0m;
            decimal.TryParse(s16, out d16);
            decimal d17 = 0m;
            decimal.TryParse(s17, out d17);
            decimal d18 = 0m;
            decimal.TryParse(s18, out d18);
            decimal d19 = 0m;
            decimal.TryParse(s19, out d19);
            decimal d20 = 0m;
            decimal.TryParse(s20, out d20);
            decimal d21 = 0m;
            decimal.TryParse(s21, out d21);
            decimal d22 = 0m;
            decimal.TryParse(s22, out d22);
            decimal d23 = 0m;
            decimal.TryParse(s23, out d23);
            decimal d24 = 0m;
            decimal.TryParse(s24, out d24);
            decimal d25 = 0m;
            decimal.TryParse(s25, out d25);
            decimal d26 = 0m;
            decimal.TryParse(s26, out d26);
            decimal num = d14 - d;
            decimal num2 = d15 - d2;
            decimal num3 = d16 - d3;
            decimal num4 = d17 - d4;
            decimal num5 = d18 - d5;
            decimal num6 = d19 - d6;
            decimal num7 = d20 - d7;
            decimal num8 = d21 - d8;
            decimal num9 = d22 - d9;
            decimal num10 = d23 - d10;
            decimal num11 = d24 - d11;
            decimal num12 = d25 - d12;
            decimal num13 = d26 - d13;
            IList<Bill_Ysgc> ysgcByNian = new YsgcDal().GetYsgcByNian(this.ddlNd.SelectedValue);
            if (num != 0m)
            {
                Bill_Ysmxb bill_Ysmxb = new Bill_Ysmxb();
                bill_Ysmxb.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc = (from p in ysgcByNian
                                       where p.YsType == "2" && p.Yue == "1"
                                       select p).First<Bill_Ysgc>();
                bill_Ysmxb.Gcbh = bill_Ysgc.Gcbh;
                bill_Ysmxb.GcMc = bill_Ysgc.Xmmc;
                bill_Ysmxb.YsDept = this.deptcode;
                bill_Ysmxb.Ysje = num;
                bill_Ysmxb.Yskm = value;
                bill_Ysmxb.YskmMc = yskmMc;
                bill_Ysmxb.YsType = "3";
                list.Add(bill_Ysmxb);
            }
            if (num2 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb2 = new Bill_Ysmxb();
                bill_Ysmxb2.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc2 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "2"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb2.Gcbh = bill_Ysgc2.Gcbh;
                bill_Ysmxb2.GcMc = bill_Ysgc2.Xmmc;
                bill_Ysmxb2.YsDept = this.deptcode;
                bill_Ysmxb2.Ysje = num2;
                bill_Ysmxb2.Yskm = value;
                bill_Ysmxb2.YskmMc = yskmMc;
                bill_Ysmxb2.YsType = "3";
                list.Add(bill_Ysmxb2);
            }
            if (num3 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb3 = new Bill_Ysmxb();
                bill_Ysmxb3.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc3 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "3"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb3.Gcbh = bill_Ysgc3.Gcbh;
                bill_Ysmxb3.GcMc = bill_Ysgc3.Xmmc;
                bill_Ysmxb3.YsDept = this.deptcode;
                bill_Ysmxb3.Ysje = num3;
                bill_Ysmxb3.Yskm = value;
                bill_Ysmxb3.YskmMc = yskmMc;
                bill_Ysmxb3.YsType = "3";
                list.Add(bill_Ysmxb3);
            }
            if (num4 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb4 = new Bill_Ysmxb();
                bill_Ysmxb4.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc4 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "4"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb4.Gcbh = bill_Ysgc4.Gcbh;
                bill_Ysmxb4.GcMc = bill_Ysgc4.Xmmc;
                bill_Ysmxb4.YsDept = this.deptcode;
                bill_Ysmxb4.Ysje = num4;
                bill_Ysmxb4.Yskm = value;
                bill_Ysmxb4.YskmMc = yskmMc;
                bill_Ysmxb4.YsType = "3";
                list.Add(bill_Ysmxb4);
            }
            if (num5 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb5 = new Bill_Ysmxb();
                bill_Ysmxb5.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc5 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "5"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb5.Gcbh = bill_Ysgc5.Gcbh;
                bill_Ysmxb5.GcMc = bill_Ysgc5.Xmmc;
                bill_Ysmxb5.YsDept = this.deptcode;
                bill_Ysmxb5.Ysje = num5;
                bill_Ysmxb5.Yskm = value;
                bill_Ysmxb5.YskmMc = yskmMc;
                bill_Ysmxb5.YsType = "3";
                list.Add(bill_Ysmxb5);
            }
            if (num6 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb6 = new Bill_Ysmxb();
                bill_Ysmxb6.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc6 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "6"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb6.Gcbh = bill_Ysgc6.Gcbh;
                bill_Ysmxb6.GcMc = bill_Ysgc6.Xmmc;
                bill_Ysmxb6.YsDept = this.deptcode;
                bill_Ysmxb6.Ysje = num6;
                bill_Ysmxb6.Yskm = value;
                bill_Ysmxb6.YskmMc = yskmMc;
                bill_Ysmxb6.YsType = "3";
                list.Add(bill_Ysmxb6);
            }
            if (num7 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb7 = new Bill_Ysmxb();
                bill_Ysmxb7.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc7 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "7"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb7.Gcbh = bill_Ysgc7.Gcbh;
                bill_Ysmxb7.GcMc = bill_Ysgc7.Xmmc;
                bill_Ysmxb7.YsDept = this.deptcode;
                bill_Ysmxb7.Ysje = num7;
                bill_Ysmxb7.Yskm = value;
                bill_Ysmxb7.YskmMc = yskmMc;
                bill_Ysmxb7.YsType = "3";
                list.Add(bill_Ysmxb7);
            }
            if (num8 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb8 = new Bill_Ysmxb();
                bill_Ysmxb8.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc8 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "8"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb8.Gcbh = bill_Ysgc8.Gcbh;
                bill_Ysmxb8.GcMc = bill_Ysgc8.Xmmc;
                bill_Ysmxb8.YsDept = this.deptcode;
                bill_Ysmxb8.Ysje = num8;
                bill_Ysmxb8.Yskm = value;
                bill_Ysmxb8.YskmMc = yskmMc;
                bill_Ysmxb8.YsType = "3";
                list.Add(bill_Ysmxb8);
            }
            if (num9 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb9 = new Bill_Ysmxb();
                bill_Ysmxb9.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc9 = (from p in ysgcByNian
                                        where p.YsType == "2" && p.Yue == "9"
                                        select p).First<Bill_Ysgc>();
                bill_Ysmxb9.Gcbh = bill_Ysgc9.Gcbh;
                bill_Ysmxb9.GcMc = bill_Ysgc9.Xmmc;
                bill_Ysmxb9.YsDept = this.deptcode;
                bill_Ysmxb9.Ysje = num9;
                bill_Ysmxb9.Yskm = value;
                bill_Ysmxb9.YskmMc = yskmMc;
                bill_Ysmxb9.YsType = "3";
                list.Add(bill_Ysmxb9);
            }
            if (num10 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb10 = new Bill_Ysmxb();
                bill_Ysmxb10.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc10 = (from p in ysgcByNian
                                         where p.YsType == "2" && p.Yue == "10"
                                         select p).First<Bill_Ysgc>();
                bill_Ysmxb10.Gcbh = bill_Ysgc10.Gcbh;
                bill_Ysmxb10.GcMc = bill_Ysgc10.Xmmc;
                bill_Ysmxb10.YsDept = this.deptcode;
                bill_Ysmxb10.Ysje = num10;
                bill_Ysmxb10.Yskm = value;
                bill_Ysmxb10.YskmMc = yskmMc;
                bill_Ysmxb10.YsType = "3";
                list.Add(bill_Ysmxb10);
            }
            if (num11 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb11 = new Bill_Ysmxb();
                bill_Ysmxb11.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc11 = (from p in ysgcByNian
                                         where p.YsType == "2" && p.Yue == "11"
                                         select p).First<Bill_Ysgc>();
                bill_Ysmxb11.Gcbh = bill_Ysgc11.Gcbh;
                bill_Ysmxb11.GcMc = bill_Ysgc11.Xmmc;
                bill_Ysmxb11.YsDept = this.deptcode;
                bill_Ysmxb11.Ysje = num11;
                bill_Ysmxb11.Yskm = value;
                bill_Ysmxb11.YskmMc = yskmMc;
                bill_Ysmxb11.YsType = "3";
                list.Add(bill_Ysmxb11);
            }
            if (num12 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb12 = new Bill_Ysmxb();
                bill_Ysmxb12.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc12 = (from p in ysgcByNian
                                         where p.YsType == "2" && p.Yue == "12"
                                         select p).First<Bill_Ysgc>();
                bill_Ysmxb12.Gcbh = bill_Ysgc12.Gcbh;
                bill_Ysmxb12.GcMc = bill_Ysgc12.Xmmc;
                bill_Ysmxb12.YsDept = this.deptcode;
                bill_Ysmxb12.Ysje = num12;
                bill_Ysmxb12.Yskm = value;
                bill_Ysmxb12.YskmMc = yskmMc;
                bill_Ysmxb12.YsType = "3";
                list.Add(bill_Ysmxb12);
            }
            if (num13 != 0m)
            {
                Bill_Ysmxb bill_Ysmxb13 = new Bill_Ysmxb();
                bill_Ysmxb13.BillCode = this.billcode;
                Bill_Ysgc bill_Ysgc13 = (from p in ysgcByNian
                                         where p.YsType == "0" && p.Yue == ""
                                         select p).First<Bill_Ysgc>();
                bill_Ysmxb13.Gcbh = bill_Ysgc13.Gcbh;
                bill_Ysmxb13.GcMc = bill_Ysgc13.Xmmc;
                bill_Ysmxb13.YsDept = this.deptcode;
                bill_Ysmxb13.Ysje = num13;
                bill_Ysmxb13.Yskm = value;
                bill_Ysmxb13.YskmMc = yskmMc;
                bill_Ysmxb13.YsType = "3";
                list.Add(bill_Ysmxb13);
            }
        }
        return list;
    }

    protected void ddlYsType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        DataControlRowType rowType = e.Row.RowType;
        if (rowType != DataControlRowType.Header)
        {
            return;
        }
        TableCellCollection cells = e.Row.Cells;
        cells.Clear();
        cells.Add(new TableHeaderCell());
        cells[0].Attributes.Add("rowspan", "2");
        cells[0].Text = "序号";
        cells.Add(new TableHeaderCell());
        cells[1].Attributes.Add("rowspan", "2");
        cells[1].Text = "预算科目";
        cells.Add(new TableHeaderCell());
        cells[2].Attributes.Add("colspan", "3");
        cells[2].Text = "一月份";
        cells.Add(new TableHeaderCell());
        cells[3].Attributes.Add("colspan", "3");
        cells[3].Text = "二月份";
        cells.Add(new TableHeaderCell());
        cells[4].Attributes.Add("colspan", "3");
        cells[4].Text = "三月份";
        cells.Add(new TableHeaderCell());
        cells[5].Attributes.Add("colspan", "3");
        cells[5].Text = "四月份";
        cells.Add(new TableHeaderCell());
        cells[6].Attributes.Add("colspan", "3");
        cells[6].Text = "五月份";
        cells.Add(new TableHeaderCell());
        cells[7].Attributes.Add("colspan", "3");
        cells[7].Text = "六月份";
        cells.Add(new TableHeaderCell());
        cells[8].Attributes.Add("colspan", "3");
        cells[8].Text = "七月份";
        cells.Add(new TableHeaderCell());
        cells[9].Attributes.Add("colspan", "3");
        cells[9].Text = "八月份";
        cells.Add(new TableHeaderCell());
        cells[10].Attributes.Add("colspan", "3");
        cells[10].Text = "九月份";
        cells.Add(new TableHeaderCell());
        cells[11].Attributes.Add("colspan", "3");
        cells[11].Text = "十月份";
        cells.Add(new TableHeaderCell());
        cells[12].Attributes.Add("colspan", "3");
        cells[12].Text = "十一月份";
        cells.Add(new TableHeaderCell());
        cells[13].Attributes.Add("colspan", "3");
        cells[13].Text = "十二月份";
        cells.Add(new TableHeaderCell());
        cells[14].Attributes.Add("colspan", "3");
        cells[14].Text = "年度";
        cells.Add(new TableHeaderCell());
        cells[15].Attributes.Add("rowspan", "2");
        cells[15].Text = "调整差额</th></tr><tr id='secondtr'>";
        cells.Add(new TableHeaderCell());
        cells[16].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[17].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[18].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[19].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[20].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[21].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[22].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[23].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[24].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[25].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[26].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[27].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[28].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[29].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[30].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[31].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[32].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[33].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[34].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[35].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[36].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[37].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[38].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[39].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[40].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[41].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[42].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[43].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[44].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[45].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[46].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[47].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[48].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[49].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[50].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[51].Text = "报销金额";
        cells.Add(new TableHeaderCell());
        cells[52].Text = "当前预算";
        cells.Add(new TableHeaderCell());
        cells[53].Text = "调整后预算";
        cells.Add(new TableHeaderCell());
        cells[54].Text = "报销金额";
    }

    protected void btnScdj_Click(object sender, EventArgs e)
    {
        if (this.upLoadFiles.Visible)
        {
            if (this.upLoadFiles.PostedFile.FileName == "")
            {
                this.laFilexx.Text = "请选择文件";
                return;
            }
            try
            {
                string fileName = this.upLoadFiles.PostedFile.FileName;
                string fileName2 = this.upLoadFiles.PostedFile.FileName;
                string arg_8E_0 = Path.GetFileName(fileName2).Split(new char[]
                {
                    '.'
                })[0];
                string extension = Path.GetExtension(fileName2);
                if (this.isOK(extension))
                {
                    string text = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                    string str = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    string filename = base.Server.MapPath("~\\Uploads\\ystz\\") + str + "-" + text;
                    string str2 = "~\\Uploads\\ystz\\" + str + "-" + text;
                    if (!Directory.Exists(base.Server.MapPath("~\\Uploads\\ystz\\")))
                    {
                        Directory.CreateDirectory(base.Server.MapPath("~\\Uploads\\ystz\\"));
                    }
                    this.upLoadFiles.PostedFile.SaveAs(filename);
                    HiddenField expr_143 = this.hiddFileDz;
                    expr_143.Value = expr_143.Value + str2 + ";";
                    Literal expr_160 = this.Lafilename;
                    expr_160.Text = expr_160.Text + "<div style=' border-bottom:1px dashed #CDCDCD; text-align:left;'>&nbsp;&nbsp;&nbsp;<span style='font-weight:700'>新附件" + text + "：</span></div>";
                    HiddenField expr_181 = this.hidfilnename;
                    expr_181.Value = expr_181.Value + text + ";";
                    this.laFilexx.Text = "上传成功";
                }
                else
                {
                    base.Response.Write("<script>alert('文件类型不合法');</script>");
                }
                return;
            }
            catch (Exception ex)
            {
                this.laFilexx.Text = ex.ToString();
                return;
            }
        }
        this.btn_sc.Text = "上传";
        this.upLoadFiles.Visible = true;
    }

    private bool isOK(string exname)
    {
        return exname.ToLower() == ".doc" || exname.ToLower() == ".docx" || exname.ToLower() == ".jpg" || exname.ToLower() == ".png" || exname.ToLower() == ".gif" || exname.ToLower() == ".xls" || exname.ToLower() == ".xlsx" || exname.ToLower() == ".zip" || exname.ToLower() == ".txt" || exname.ToLower() == ".pdf" || exname.ToLower() == ".rar" || exname.ToLower() == ".ppt";
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            string s = e.Row.Cells[9].Text.Trim();
            if (decimal.Parse(s) > 0m)
            {
                e.Row.Cells[2].Text = "调整入";
                e.Row.Cells[3].Text = "";
            }
            else
            {
                e.Row.Cells[2].Text = "调整出";
                e.Row.Cells[4].Text = "";
            }
            string gcbh = e.Row.Cells[7].Text.Trim();
            string kmCode = e.Row.Cells[8].Text.Trim();
            decimal yueYs = this.ysmgr.GetYueYs(gcbh, this.deptcode, kmCode);
            decimal num;
            if (this.stepid.Equals("-1"))
            {
                num = yueYs + decimal.Parse(s);
            }
            else
            {
                num = yueYs;
            }
            e.Row.Cells[5].Text = num.ToString("N2");
        }
    }

    protected void btn_excelmx_Click(object sender, EventArgs e)
    {
        DataTable dataTable = this.getdtysmx();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("GcMc", "过程名称");
        dictionary.Add("Yskm", "预算科目编号");
        dictionary.Add("YskmMc", "预算科目名称");
        dictionary.Add("type", "调整类型");
        dictionary.Add("Ysje", "调整金额");
        dictionary.Add("ys", "调整后的预算金额");
        dictionary.Add("sm", "说明");
        if (dataTable != null)
        {
            new ExcelHelper().ExpExcel(dataTable, "ExportFile", dictionary);
            return;
        }
        base.ClientScript.RegisterStartupScript(base.GetType(), "a", "alert('导出失败。');", true);
    }

    protected void ddlNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.bindData();
    }
}