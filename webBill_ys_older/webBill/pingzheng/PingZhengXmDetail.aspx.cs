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
using Dal.SysDictionary;
using System.Text;
using Models;
using Bll.PingZheng;

public partial class webBill_pingzheng_PingZhengXmDetail : System.Web.UI.Page
{
    string strCtrl = "";
    string strCode = "";
    PingZheng_XMBLL bllPingZhengXm = new PingZheng_XMBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        object objCode = Request["Code"];
        if (objCode != null)
        {
            strCode = objCode.ToString();
        }
        object objCtrl = Request["Ctrl"];
        if (objCtrl != null)
        {
            strCtrl = objCtrl.ToString();
        }
        if (!IsPostBack)
        {
            initPage();
        }
        ClientScript.RegisterArrayDeclaration("availableTags", getAllType());
    }

    private void initPage()
    {
        if (strCtrl.Equals(""))
        {
            this.showMessage("关键参数丢失！", true, "");
            return;
        }
        if (!strCode.Equals(""))
        {
            bill_pingzheng_xmModel modelPingzhengXmModel = new bill_pingzheng_xmModel();
            modelPingzhengXmModel = bllPingZhengXm.GetModel(strCode);
            this.txtCode.Text = modelPingzhengXmModel.xmCode;
            this.txtName.Text = modelPingzhengXmModel.xmName;
            if (modelPingzhengXmModel.parentCode.Equals("0"))
            {
                this.ChIsRootNode.Checked = true;
            }
            else
            {
                this.txtParent.Text = string.Format("[{0}]{1}", modelPingzhengXmModel.parentCode, modelPingzhengXmModel.parentName);
            }
            this.cbIsDefault.Checked = modelPingzhengXmModel.isDefault.Equals("1");
            this.rdStatus.SelectedValue = modelPingzhengXmModel.Status;
            this.txtcitem_class.Text = modelPingzhengXmModel.Note1;
            this.txtcitem_id.Text = modelPingzhengXmModel.Note2;
        }
        if (strCtrl.Equals("Add"))
        {
            this.txtCode.Text = this.CreateCode();
        }
        else if (strCtrl.Equals("Edit"))
        {
            if (strCode.Equals(""))
            {
                this.showMessage("关键参数丢失！", true, "");
                return;
            }
        }

    }
    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Yes_Click(object sender, EventArgs e)
    {
        bill_pingzheng_xmModel modelPingZhengXmModel = new bill_pingzheng_xmModel();
        if (!strCode.Equals(""))
        {
            modelPingZhengXmModel = bllPingZhengXm.GetModel(strCode);
        }
        modelPingZhengXmModel.isDefault = this.cbIsDefault.Checked ? "1" : "0";
        modelPingZhengXmModel.Status = this.rdStatus.SelectedValue;
        modelPingZhengXmModel.xmCode = this.txtCode.Text.Trim();
        string strName = this.txtName.Text.Trim();
        if (strName.Equals(""))
        {
            showMessage("必须填写名称！", false, ""); return;
        }
        modelPingZhengXmModel.xmName = strName;
        if (ChIsRootNode.Checked)
        {
            modelPingZhengXmModel.parentCode = "0";
            modelPingZhengXmModel.parentName = "";
        }
        else
        {
            string strParentCode = this.txtParent.Text.Trim();
            try
            {
                modelPingZhengXmModel.parentCode = strParentCode.Substring(1, strParentCode.IndexOf(']') - 1);
                modelPingZhengXmModel.parentName = strParentCode.Substring(strParentCode.IndexOf(']') + 1);
            }
            catch (Exception)
            {
                showMessage("必须选择上级类型！", false, ""); return;
            }
        }
        modelPingZhengXmModel.Note1 = this.txtcitem_class.Text.Trim();
        modelPingZhengXmModel.Note2 = this.txtcitem_id.Text.Trim();
        string strMsg = "";
        if (bllPingZhengXm.Add(modelPingZhengXmModel, out strMsg) > 0)
        {
            showMessage("保存成功！", true, "");
        }
        else
        {
            showMessage("保存失败，原因：" + strMsg, true, "");
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
    private string CreateCode()
    {
        string strCode = new DataDicDal().GetYbbxBillName("N", DateTime.Now.ToString("yyyyMMdd"), 1, 4);
        if (strCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
        }
        return strCode;
    }
    /// <summary>
    /// 获取所有类别
    /// </summary>
    /// <returns></returns>
    private string getAllType()
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        DataSet ds = server.GetDataSet("select '['+xmCode+']'+xmName as kemu from  bill_pingzheng_xm");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kemu"]));
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
}
