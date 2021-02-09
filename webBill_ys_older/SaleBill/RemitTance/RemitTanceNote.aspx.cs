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
using System.Text;

public partial class SaleBill_RemitTance_RemitTanceNote : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strJingXiaoShang = "";
    string strFenGongsi = "";
    string strHuiKuanLeiBie = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objJingXiaoShang = Request["JXS"];
            if (objJingXiaoShang != null)
            {
                strJingXiaoShang = objJingXiaoShang.ToString();
            }
            object objFenGongSi = Request["FGS"];
            if (objFenGongSi != null)
            {
                strFenGongsi = objFenGongSi.ToString();
            }
            object objHuiKuanLeiBie = Request["HKLB"];
            if (objHuiKuanLeiBie != null)
            {
                strHuiKuanLeiBie = objHuiKuanLeiBie.ToString();
            }
            if (!IsPostBack)
            {
                initControl();
                BindData();
            }
        }

    }

    private void initControl()
    {
        DataSet temp = new V_HuikuanLeiBie().getall();
        if (temp != null)
        {
            this.ddlHuiKuanLeiBie.DataSource = temp;
            this.ddlHuiKuanLeiBie.DataValueField = "lb_id";
            this.ddlHuiKuanLeiBie.DataTextField = "kxlb";
            this.ddlHuiKuanLeiBie.DataBind();
        }
        this.ddlHuiKuanLeiBie.Items.Insert(0, new ListItem("--全部--", ""));

        this.txtCompanyName.Text = strFenGongsi;
        this.txtJXSName.Text = strJingXiaoShang;
        //DateTime dt = DateTime.Now;
        DateTime dtOperate = DateTime.Now.AddMonths(-5);
        //int iYear = dtOperate.Year;
        //int iMonth = dtOperate.Month;
        //int iDay = dtOperate.Day;
        this.txtOperateDateFrm.Text = dtOperate.ToString("yyyy-MM-dd");
        this.txtOperateDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");

        this.txtJingBanRiQiQ.Text = dtOperate.ToString("yyyy-MM-dd");
        this.txtJingBanRiQiZhi.Text = DateTime.Now.ToString("yyyy-MM-dd");
        if (strHuiKuanLeiBie != "")
        {
            this.ddlHuiKuanLeiBie.SelectedValue = strHuiKuanLeiBie;
        }
    }

    private void BindData()
    {
        #region 查询条件
        StringBuilder sqlSb = new StringBuilder();
        string strCompanyName = this.txtCompanyName.Text.Trim();
        if (!strCompanyName.Equals(""))
        {
            sqlSb.Append(" and ssfgsMc like '%" + strCompanyName + "%'");
        }
        string strJXSName = this.txtJXSName.Text.Trim();
        if (!strJXSName.Equals(""))
        {
            sqlSb.Append(" and dwmc like '%" + strJXSName + "%'");
        }
        string strHuiKuanLeiBie = this.ddlHuiKuanLeiBie.SelectedValue;
        if (!strHuiKuanLeiBie.Equals(""))
        {
            sqlSb.Append(" and hklb='" + strHuiKuanLeiBie + "'");
        }
        string strOperateDateFrm = this.txtOperateDateFrm.Text.Trim();
        if (!strOperateDateFrm.Equals(""))
        {
            sqlSb.Append(" and operdate>='" + strOperateDateFrm + "'");
        }
        string strOperateDateTo = this.txtOperateDateTo.Text.Trim();
        if (!strOperateDateTo.Equals(""))
        {
            sqlSb.Append(" and operdate<='" + strOperateDateTo + "'");
        }
        string strAmountFrm = this.txtAmoumtFrm.Text.Trim();
        if (!strAmountFrm.Equals(""))
        {
            sqlSb.Append(" and hkje>=" + strAmountFrm + "");
        }
        string strAmountTo = this.txtAmountTo.Text.Trim();
        if (!strAmountTo.Equals(""))
        {
            sqlSb.Append(" and hkje<=" + strAmountTo + "");
        }
        string strJingBanRiQiQ = this.txtJingBanRiQiQ.Text.Trim();
        if (!strJingBanRiQiQ.Equals(""))
        {
            sqlSb.Append(" and jbrq>='" + strJingBanRiQiQ + "'");
        }
        string strJingBanRiQiZhi = this.txtJingBanRiQiZhi.Text.Trim();
        if (!strJingBanRiQiZhi.Equals(""))
        {
            sqlSb.Append(" and jbrq<='" + strJingBanRiQiZhi + "'");
        }
        string strJBR = this.txtJingBanRen.Text.Trim();
        if (!strJBR.Equals(""))
        {
            sqlSb.Append(" and jbr like '%" + strJBR + "%'");
        }
        string strOperater = this.txtOperater.Text.Trim();
        if (!strOperater.Equals(""))
        {
            sqlSb.Append(" and oper like '%" + strOperater + "%'");
        }
        #endregion
        this.GridView1.DataSource = new V_HuiKuanMingXiBLL().GetAllHuiKuanNote(sqlSb.ToString());
        this.GridView1.DataBind();
    }

    protected void btn_cx_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
