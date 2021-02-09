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

public partial class webBill_cwgl_PingZhengBuLu : System.Web.UI.Page
{
    private string strBillCode = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        object objBillCode = Request["BillCode"];
        if (objBillCode == null)
        {
            showMessage("重要参数丢失，请回到列表页选择要操作的记录后重试！", true, "");
            return;
        }
        strBillCode = objBillCode.ToString();
        if (!IsPostBack)
        {
            initpage();
        }
    }
    /// <summary>
    /// 初始化页面
    /// </summary>
    private void initpage()
    {
        this.txtDate.Attributes.Add("onfocus", "javascript:setday(this);");
        string strSql = "select pzcode,pzdate,sfgf from bill_ybbxmxb where billCode='" + strBillCode + "'";
        DataTable dtRel = server.GetDataTable(strSql, null);
        if (dtRel.Rows.Count > 0)
        {
            DateTime dt;
            if (DateTime.TryParse(dtRel.Rows[0]["pzdate"].ToString(), out dt))
            {
                this.txtDate.Text = dt.ToString("yyyy-MM-dd");
                if (txtDate.Text=="1900-01-01")
                {
                    txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.txtPingZhengHao.Text = dtRel.Rows[0]["pzcode"].ToString();
        }
        if (dtRel.Rows[0]["sfgf"].Equals("1"))
        {
            showMessage("该记录已经给付，不允许修改凭证!", false, "");
            this.txtPingZhengHao.Enabled = this.txtDate.Enabled = this.btn_Save.Enabled = false;
        }
        DataTable dtZhangtao = server.GetDataTable("select dicCode,dicName from bill_dataDic where dicType='10'", null);
        this.ddlZhangTao.DataSource = dtZhangtao;
        this.ddlZhangTao.DataTextField = "dicName";
        this.ddlZhangTao.DataValueField = "dicCode";
        this.ddlZhangTao.DataBind();
        this.ddlZhangTao.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-选择帐套-", ""));
        if (dtZhangtao==null||dtZhangtao.Rows.Count<=0)
        {
            showMessage("请先到数据字典维护帐套信息！", true, "");
            this.txtPingZhengHao.Enabled = this.txtDate.Enabled = this.btn_Save.Enabled = false;
            return;
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        string strDate = this.txtDate.Text.Trim();
        DateTime dt;
        if (!strDate.Equals(""))
        {
            if (!DateTime.TryParse(strDate, out dt))
            {
                showMessage("日期输入不合法！", false, "");
                return;
            }
        }
        string strZhangtao = this.ddlZhangTao.SelectedValue;
        if (strZhangtao.Equals(""))
        {
            showMessage("请先选择帐套！", false, "");
            return;
        }
        string strPzCode = this.txtPingZhengHao.Text.Trim();
        if (string.IsNullOrEmpty(strPzCode))
        {
            //showMessage("凭证号不能为空！", false, ""); return;
            strDate = "";
        }
        if (strBillCode.Equals(""))
        {
            showMessage("重要参数丢失！", false, ""); return;
        }
        if (new Bll.newysgl.bill_ysmxbBll().SetPingZheng(string.Format("'{0}'", strBillCode), strPzCode,strZhangtao, strDate) > 0)
        {
            showMessage("录入成功！", true, "");
        }
        else
        {
            showMessage("录入失败，请联系管理员解决！", false, "");
        }
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
