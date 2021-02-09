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
using Bll.ReportAppBLL;
using Bll.Zichan;

public partial class ZiChan_ZiChanGuanLi_zjfsDetail : System.Web.UI.Page
{
    string strbillcode = "";
    string strtype = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ZengJianFangShiBll zjfsbll = new ZengJianFangShiBll();
    ReportApplicationBLL reportbll = new ReportApplicationBLL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objRequest = Request["Code"];
            object objRequsttype = Request["Ctrl"];
            if (objRequest != null && !string.IsNullOrEmpty(objRequest.ToString()))
            {
                strbillcode = objRequest.ToString();
            }
            if (objRequsttype != null && !string.IsNullOrEmpty(objRequsttype.ToString()))
            {
                strtype = objRequsttype.ToString();
            }

            if (!IsPostBack)
            {
                bindData();
            }
            //ClientScript.RegisterArrayDeclaration("availableTagsdt", GetdetpAll());
            //ClientScript.RegisterArrayDeclaration("availableTagsuser", GetuserAll());
            //this.txtreportdate.Attributes.Add("onfocus", "javascript:setday(this);");

        }

    }

    private void bindData()
    {
        if (strtype == "Add")
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("zjfs", strneed, 1, 6);
            this.lblcode.Text = strcode;
        }
        if (strbillcode != null && strbillcode != "")
        {
            if (strtype == "Edit")
            {
                getmodel();
            }
            if (strtype == "look")
            {
                 getmodel();
                 this.btnsave.Visible = false;
            }

        }
       
    }

    public void getmodel()
    {
        Models.ZiChan_ZengJianFangShi model = new Models.ZiChan_ZengJianFangShi();
        if (strbillcode != "" && strbillcode != null)
        {
            model = zjfsbll.GetModel(strbillcode);
            this.lblcode.Text = strbillcode;
            this.txtreportname.Text = model.Fangshiname;
            this.dropzjfs.SelectedValue = model.ZjType.ToString();
            this.txtreportexplain.Text = model.BeiZhu;
        }

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

        Models.ZiChan_ZengJianFangShi model = new Models.ZiChan_ZengJianFangShi();
        model.FangshiCode = this.lblcode.Text.Trim();
        model.BeiZhu = this.txtreportexplain.Text;
        model.ZjType = this.dropzjfs.SelectedValue.ToString().Trim();
        string strfsname = this.txtreportname.Text;
        if (strfsname.Equals(""))
        {
            showMessage("增减方式名称不能为空！", false, "");
        }
        model.Fangshiname = strfsname;
        int row = zjfsbll.Add(model);
        if (strtype == "Add")
        {
            if (row > -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加成功！');window.returnValue=\"sucess\";self.close();", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('添加失败！');window.returnValue=\"sucess\";self.close();", true);

            }

        }
        else
        {
            if (row > -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改成功！');window.returnValue=\"sucess\";self.close();", true);

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('修改失败！');window.returnValue=\"sucess\";self.close();", true);

            }
        }

        // this.txtnid.Text = strcode;
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
