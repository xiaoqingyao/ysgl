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
using GoldMantis.Web.UI.WebControls;
using Bll.PingZheng;

public partial class webBill_pingzheng_PingZhengXmList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            bindData();
        }
    }

    private void bindData()
    {
        TreeListView.TreeImageFolder = "../Resources/Images/TreeListView/";
        this.tlm.RootNodeFlag = "0";
        this.tlm.DataSource = new PingZheng_XMBLL().GetAlltb();
        this.tlm.DataBind();
    }
    protected void btn_delete_Click(object serder, EventArgs e)
    {
        string strCode = this.hdDelCode.Value.Trim();
        string xmcode = this.hf_xmcode.Value.Trim();
        if (strCode.Equals(""))
        {
            showMessage("请先选中行!",false,"");
        }
        if (new PingZheng_XMBLL().ExistsNext(xmcode))
        {
            showMessage("请先选择该凭证项目的子级项目!",false,"");
            return;
        }
      
        string msg = "";
        int iRel = new PingZheng_XMBLL().Delete(strCode, out msg);
        if (iRel > 0)
        {
            showMessage("删除成功！", false, "");
        }
        else
        {
            showMessage("删除失败：原因：" + msg, false, "");
        }
        bindData();
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
