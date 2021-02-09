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
using Bll.TruckType;

public partial class webBill_truckType_truckTypeList : System.Web.UI.Page
{
    TruckTypeBLL bllTruckType = new TruckTypeBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else {
            bindData();
        }
    }
    private void bindData(){
        TreeListView.TreeImageFolder = "../Resources/Images/TreeListView/";
        this.tlm.RootNodeFlag = "0";
        this.tlm.DataSource = bllTruckType.GetAll();
        this.tlm.DataBind();
    }
    protected void btn_delete_Click(object serder,EventArgs e) {
        string strCode = this.hdDelCode.Value;
        string msg="";
        int iRel = new TruckTypeBLL().Delete(strCode,out msg);
        if (iRel > 0)
        {
            showMessage("删除成功！",false,"");
        }
        else {
            showMessage("删除失败：原因："+msg,false,"");
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
