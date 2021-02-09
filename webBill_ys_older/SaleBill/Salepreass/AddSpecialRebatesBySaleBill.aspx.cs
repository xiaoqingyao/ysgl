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

public partial class SaleBill_Salepreass_AddSpecialRebatesBySaleBill : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelect_Click(object sender, EventArgs e) {
        string strSaleCode = this.txtBillCode.Text.Trim();
        this.GridView1.DataSource = new Bll.SaleBill.ViewBLL().GetDtBySaleCode(strSaleCode);
        this.GridView1.DataBind();
    }
    /// <summary>
    /// queding
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_sure_Click(object sender, EventArgs e) {
        int iRow = this.GridView1.Rows.Count;
        int iCheckCount = 0;
        string strEnd = string.Empty;
        for (int i = 0; i < iRow; i++)
        {
            CheckBox ckb = this.GridView1.Rows[i].Cells[0].FindControl("chk_SelectedHeader") as CheckBox;
            if (ckb==null)
            {
                continue;
            }
            if (ckb.Checked)
            {
                string strEve = "";
                iCheckCount++;
                string strDdh = this.GridView1.Rows[i].Cells[1].Text.Trim();//订单号
                if (strDdh.Equals(""))
                {
                    showMessage("数据错误：第" + (i + 1).ToString() + "行存在订单号为空的记录！", false, "");
                    return;
                }
                string strCjh = this.GridView1.Rows[i].Cells[2].Text.Trim();//车架号
                if (strCjh.Equals(""))
                {
                    showMessage("数据错误：第" + (i + 1).ToString() + "行存在车架号为空的记录！", false, "");
                    return;
                }
                //标准返利
                TextBox txtStandardSaleAmount = this.GridView1.Rows[i].Cells[3].FindControl("txtStandardSaleAmount") as TextBox;
                string strStandardSaleAmount = "";
                if (txtStandardSaleAmount == null)
                {
                    showMessage("第" + (i + 1).ToString() + "行没有找到标准返利！", false, "");
                    return;
                }
                else {
                    strStandardSaleAmount = txtStandardSaleAmount.Text.Trim();
                    if (strStandardSaleAmount.Equals(""))
                    {
                        showMessage("数据错误：第" + (i + 1).ToString() + "行存在标准返利为空的记录！", false, ""); return;
                    }
                    int iStandardSaleAmount = 0;
                    if (!int.TryParse(strStandardSaleAmount, out iStandardSaleAmount)) {
                        showMessage("数据错误：第" + (i + 1).ToString() + "行标准返利值数据类型不正确，请填写阿拉伯数字！", false, ""); return; 
                    }
                }
                //特殊返利
                TextBox txtExceedStandardPoint = this.GridView1.Rows[i].Cells[4].FindControl("txtExceedStandardPoint") as TextBox;
                string strExceedStandardPoint = "";
                if (txtExceedStandardPoint == null)
                {
                    showMessage("第" + (i + 1).ToString() + "行没有找到特殊返利！", false, ""); return;
                }
                else
                {
                    strExceedStandardPoint = txtExceedStandardPoint.Text.Trim();
                    if (strExceedStandardPoint.Equals(""))
                    {
                        showMessage("数据错误：第" + (i + 1).ToString() + "行存在特殊为空的记录！", false, ""); return;
                    }
                    int iExceedStandardPoint = 0;
                    if (!int.TryParse(strExceedStandardPoint, out iExceedStandardPoint))
                    {
                        showMessage("数据错误：第" + (i + 1).ToString() + "行标准返利值数据类型不正确，请填写阿拉伯数字！", false, ""); return;
                    }
                }
                //原因
                TextBox txtExplain = this.GridView1.Rows[i].Cells[5].FindControl("txtExplain") as TextBox;
                string strExplain = "";
                if (txtExplain == null)
                {
                    showMessage("第" + (i + 1).ToString() + "行没有找到申请原因！", false, ""); return;
                }
                else
                {
                    strExplain = txtExplain.Text;
                    strExplain = strExplain.Equals("") ? "未填写" : strExplain;
                }
                strEve += string.Format("{0}|&|{1}|&|{2}|&|{3}|&|{4}", strDdh, strCjh, strStandardSaleAmount, strExceedStandardPoint, strExplain);
                strEnd += strEve+"|*|";
            }
        }
        if (iCheckCount == 0 || strEnd.Equals(""))
        {
            showMessage("请勾选要选中的记录！", false, ""); return;
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + strEnd + "\";self.close();", true);
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
