using Dal.FeiYong_DZ;
using Dal.SysDictionary;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_fysq_ZcgzsqDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    dz_zcgzsqdDal zcgzdal = new dz_zcgzsqdDal();
    string strCtrl = "";
    string strbillcode = "";

    string flg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!string.IsNullOrEmpty(Request["flg"]))
            {
                flg = Request["flg"].ToString();
                if (flg == "f")
                {
                    lbl_bd.Text = "新建总部物品申购单";
                    lbl_title.Text = "物品申购单";
                }
            }
            if (Request["Code"] != null && Request["Code"].ToString() != "")
            {
                strbillcode = Request["Code"].ToString();
            }
            else
            {
                Databind();
            }
            if (Request["Ctrl"] != null && Request["Ctrl"].ToString() != "")
            {
                strCtrl = Request["Ctrl"].ToString();


                if (Page.Request.QueryString["ctrl"].ToString().Trim() == "audit")
                {
                    tr_shyj.Visible = true;
                    tr_shyj_history.Visible = true;
                    //显示历史驳回意见
                    DataTable dtHisMind = server.GetDataTable("select * from bill_ReturnHistory where billcode='" + Request.QueryString["Code"] + "' order by dt desc", null);
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
                    tr_shyj.Visible = false;
                    tr_shyj.Visible = btn_ok.Visible = btn_cancel.Visible = false;
                }


                if (!string.IsNullOrEmpty(strCtrl) && strCtrl == "Add")
                {
                    string strflowid = "zcgz";
                         
                    if (flg=="f")
                    {
                        strflowid = "fzcgz";
                    }
                    txt_swbh.Text = new DataDicDal().GetYbbxBillName(strflowid, DateTime.Now.ToString("yyyyMMdd"), 1);

                }
                else
                {
                    if (strCtrl == "View")
                    {
                        this.btn_save.Visible = false;
                    }
                    Databind();
                }
            }

        }
    }
    /// <summary>
    /// 给控件赋值
    /// </summary>
    void Databind()
    {
        if (!string.IsNullOrEmpty(strCtrl) && strCtrl != "Add" && (!string.IsNullOrEmpty(strbillcode)))
        {
            dz_zcgzsqd model = new dz_zcgzsqd();
            model = zcgzdal.GetModel(strbillcode);
            if (model != null)
            {
                txt_swbh.Text = model.swbh;
                drp_zydj.SelectedValue = model.zydj;
                txt_sqsy.Text = model.sqsy;
                txt_tsbz.Text = model.tsbz;
                txt_bh.Text = model.bh;
                txt_sqsj.Text = model.sqsy;
                txt_wpmc.Text = model.wpmc;
                txt_ggsl.Text = model.ggsl;
                txt_yt.Text = model.yt;
                txt_sybm.Text = model.sybm;
                txt_xyrq.Text = model.xyrq;
                txt_gjjz.Text = model.gjjz;
                txt_gzbz.Text = model.gzbz;
                txt_zje.Text = model.zje.ToString();
                txt_sgbmfzr.Text = model.sgbmfzr;
                txt_nqbyj.Text = model.nqbyj;
                txt_cwbyj.Text = model.cwbyj;
                txt_rzxzbyj.Text = model.rzxzyj;
                txt_xzbyj.Text = model.xzbyj;
                txt_sgbrq.Text = model.sgbmrq;
                txt_nqbrq.Text = model.nqbrq;
                txt_cwbrq.Text = model.cwbrq;
                txt_rzxrrq.Text = model.rzxzrq;
                txt_xzbrq.Text = model.xzbrq;
                txt_sqjs.Text = model.sqjs.ToString();

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败')", true);
            }
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {

        dz_zcgzsqd zcgzmodel = new dz_zcgzsqd();
        Bill_Main mainmodel = new Bill_Main();

        //1.验证是否为空
        //2. 保存
        string strswbh = txt_swbh.Text;
        if (!string.IsNullOrEmpty(strswbh))
        {
            mainmodel.BillCode = strswbh;
            mainmodel.BillName = "资产购置申请单";
            zcgzmodel.swbh = strswbh;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('事物编号不能为空')", true);
            return;
        }
        mainmodel.BillDate = DateTime.Now;
        if (!string.IsNullOrEmpty(Session["userCode"].ToString().Trim()))
        {
            string dept = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
            mainmodel.BillDept = dept;
            mainmodel.BillUser = Session["userCode"].ToString();
        }

        if (flg == "f")
        {
            mainmodel.FlowId = "fzcgz";
        }
        else
        {
            mainmodel.FlowId = "zcgz";
        }

        mainmodel.StepId = "-1";


        string strzydj = drp_zydj.SelectedValue;
        if (!string.IsNullOrEmpty(strzydj))
        {
            zcgzmodel.zydj = strzydj;
        }
        string strsqsy = txt_sqsy.Text;
        if (!string.IsNullOrEmpty(strsqsy))
        {
            zcgzmodel.sqsy = strsqsy;
        }
        else
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请事由不能为空')", true);
            return;
        }

        string strtsbz = txt_tsbz.Text;
        if (!string.IsNullOrEmpty(strtsbz))
        {
            zcgzmodel.tsbz = strtsbz;
        }
        string strbh = txt_bh.Text;
        if (!string.IsNullOrEmpty(strbh))
        {
            zcgzmodel.bh = strbh;
        }
        string strsqsj = txt_sqsj.Text;
        if (!string.IsNullOrEmpty(strsqsj))
        {
            zcgzmodel.sqsj = strsqsj;
        }
        string strwpmc = txt_wpmc.Text;
        if (!string.IsNullOrEmpty(strwpmc))
        {
            zcgzmodel.wpmc = strwpmc;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('物品名称不能为空')", true);
            return;
        }
        string strggsl = txt_ggsl.Text;
        if (!string.IsNullOrEmpty(strggsl))
        {
            zcgzmodel.ggsl = strggsl;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('规格数量不能为空')", true);
            return;
        }
        string stryt = txt_yt.Text;
        if (!string.IsNullOrEmpty(stryt))
        {
            zcgzmodel.yt = stryt;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('用途不能为空')", true);
            return;
        }
        string strsybm = txt_sybm.Text;
        if (!string.IsNullOrEmpty(strsybm))
        {
            zcgzmodel.sybm = strsybm;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('使用部门不能为空')", true);
            return;
        }
        string strxyrq = txt_xyrq.Text;
        if (!string.IsNullOrEmpty(strxyrq))
        {
            zcgzmodel.xyrq = strxyrq;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('需用日期不能为空')", true);
            return;
        }
        string strgjjz = txt_gjjz.Text;
        if (!string.IsNullOrEmpty(strgjjz))
        {
            zcgzmodel.gjjz = strgjjz;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('估计价值不能为空')", true);
            return;
        }
        string strgzbz = txt_gzbz.Text;
        if (!string.IsNullOrEmpty(strgzbz))
        {
            zcgzmodel.gzbz = strgzbz;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('购置备注不能为空')", true);
            return;
        }
        string strzje = txt_zje.Text;
        if (!string.IsNullOrEmpty(strzje))
        {
            zcgzmodel.zje = decimal.Parse(strzje);
            mainmodel.BillJe = decimal.Parse(strzje);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('总金额不能为空')", true);
            return;
        }
        string strsqjs = txt_sqjs.Text;
        if (!string.IsNullOrEmpty(strsqjs))
        {
            zcgzmodel.sqjs = decimal.Parse(strsqjs);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('申请件数不能为空')", true);
            return;
        }
        string strsgbmfzr = txt_sgbmfzr.Text;
        if (!string.IsNullOrEmpty(strsgbmfzr))
        {
            zcgzmodel.sgbmfzr = strsgbmfzr;
        }
        string strnqbyj = txt_nqbyj.Text;
        if (!string.IsNullOrEmpty(strnqbyj))
        {
            zcgzmodel.nqbyj = strnqbyj;
        }
        string strcwbyj = txt_cwbyj.Text;
        if (!string.IsNullOrEmpty(strcwbyj))
        {
            zcgzmodel.cwbyj = strcwbyj;
        }
        string strrzxzbyj = txt_rzxzbyj.Text;
        if (!string.IsNullOrEmpty(strrzxzbyj))
        {
            zcgzmodel.rzxzyj = strrzxzbyj;
        }
        string strxzbyj = txt_xzbyj.Text;
        if (!string.IsNullOrEmpty(strxzbyj))
        {
            zcgzmodel.xzbyj = strxzbyj;
        }
        string strsgbrq = txt_sgbrq.Text;
        if (!string.IsNullOrEmpty(strsgbrq))
        {
            zcgzmodel.sgbmrq = strsgbrq;
        }
        string strnqbrq = txt_nqbrq.Text;
        if (!string.IsNullOrEmpty(strnqbrq))
        {
            zcgzmodel.nqbrq = strnqbrq;
        }
        string strcwbrq = txt_cwbrq.Text;
        if (!string.IsNullOrEmpty(strcwbrq))
        {
            zcgzmodel.cwbrq = strcwbrq;
        }
        string strrzxzrq = txt_rzxrrq.Text;
        if (!string.IsNullOrEmpty(strrzxzrq))
        {
            zcgzmodel.rzxzrq = strrzxzrq;
        }
        string strxzbrq = txt_xzbrq.Text;
        if (!string.IsNullOrEmpty(strxzbrq))
        {
            zcgzmodel.xzbrq = strxzbrq;
        }



        int intRow = zcgzdal.Add(mainmodel, zcgzmodel);
        if (intRow > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);

        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败')", true);
        }
    }
}