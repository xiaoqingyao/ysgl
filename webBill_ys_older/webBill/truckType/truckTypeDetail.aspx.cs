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
using System.Text;
using Bll.TruckType;
using Models;
using Dal.SysDictionary;

public partial class webBill_truckType_truckTypeDetail : System.Web.UI.Page
{
    string strCode = "";//类型编号
    string strCtrl = "";//操作类型 Add=添加 Edit=修改
    TruckTypeBLL bllTruckTypeBll = new TruckTypeBLL();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        object objCode = Page.Request["Code"];
        if (objCode!=null)
        {
            strCode = objCode.ToString();
        }
        object objCtrl = Page.Request["Ctrl"];
        if (objCtrl!=null)
        {
            strCtrl = objCtrl.ToString();
        }
        if (!IsPostBack)
        {
            initControl();
        }
        ClientScript.RegisterArrayDeclaration("availableTags", getAllType());
    }

    /// <summary>
    /// 初始化控件
    /// </summary>
    private void initControl()
    {
        //this.txtCode.Enabled = false;
        T_truckType modelTruckType=new T_truckType();
        if (strCtrl.Equals("Add"))
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");

            string strcode = pusbll.GetBillCode("ClLX", strneed, 1, 6);
            this.txtCode.Text = strcode;
            HiddenFieldCode.Value = strcode;
            //this.txtCode.Text = CreateCode();
            if (strCode!="")
            {
                modelTruckType = bllTruckTypeBll.GetModel(strCode);
                this.txtParent.Text = "["+strCode+"]"+modelTruckType.typeName;
            }
        }
        else if (strCtrl.Equals("Edit"))
        {
            if (strCode.Equals(""))
            {
                showMessage("关键字丢失！", true, "");
            }
            else {
                this.txtCode.Enabled = false;
                modelTruckType = bllTruckTypeBll.GetModel(strCode);
                this.txtCode.Text = modelTruckType.typeCode.ToString();
                this.txtName.Text = modelTruckType.typeName;
                this.txtHigherPerPoint.Text = modelTruckType.HigherPerPoint.ToString();
                this.txtRebatePoint.Text = modelTruckType.RebatePoint.ToString();
                this.txtDeductionPoint.Text = modelTruckType.DeductionPoint.ToString();
                T_truckType modelParentTurck = bllTruckTypeBll.GetModel(modelTruckType.parentCode);
                if (modelParentTurck != null)
                {
                    this.txtParent.Text = string.Format("[{0}]{1}", modelParentTurck.typeCode.ToString(), modelParentTurck.typeName);
                }
                else {
                    this.chbRoot.Checked = true;
                }
                this.ddlStatus.SelectedValue = modelTruckType.status;
            }
        }else{}
    }

    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Yes_Click(object sender, EventArgs e) {
        T_truckType modelTruckType = new T_truckType();
      
        if (strCtrl.Equals("Edit"))
        {
            modelTruckType = bllTruckTypeBll.GetModel(strCode);
            //如果是修改则重新调用值

        }
       
        modelTruckType.typeCode = this.txtCode.Text.Trim();
        modelTruckType.IsLastNode = bllTruckTypeBll.HasChildren(modelTruckType.typeCode) ? "0" : "1";
        string strParentCode=this.txtParent.Text.Trim();
        if (chbRoot.Checked)
        {
            modelTruckType.parentCode = "0";
        }
        else {
            if (strParentCode.Equals(""))
            {
                showMessage("如果该节点不作为根节点则上级单位不能为空！", false, "");
                return;
            }
            modelTruckType.parentCode = strParentCode.Substring(1, strParentCode.IndexOf(']') - 1);
        }
        modelTruckType.status = this.ddlStatus.SelectedValue;
        modelTruckType.typeName = this.txtName.Text;
        if (modelTruckType.parentCode.Equals(modelTruckType.typeCode))
        {
            showMessage("该节点本身不能作为父节点",false,"");
        }
        if (this.txtHigherPerPoint.Text!="")
        {
            modelTruckType.HigherPerPoint = float.Parse(this.txtHigherPerPoint.Text);
        }
        else
        {
            modelTruckType.HigherPerPoint =1;
        }
        if (txtRebatePoint.Text!="")
        {
            modelTruckType.RebatePoint = float.Parse(this.txtRebatePoint.Text);
        }
        else
        {
            modelTruckType.RebatePoint = 1;
        }
        if (txtDeductionPoint.Text!="")
        {
            modelTruckType.DeductionPoint = float.Parse(this.txtDeductionPoint.Text);
        }
        else
        {
            modelTruckType.DeductionPoint = 1;
        }
        if (strCtrl.Equals("Edit"))
        {
            string streditmasge = "";
            int rows = bllTruckTypeBll.Upd(modelTruckType,out streditmasge);
            if (rows>0)
            {
                if (!modelTruckType.parentCode.Equals("") && !modelTruckType.parentCode.Equals("0"))
                {
                    int row = bllTruckTypeBll.changeParentStatus(modelTruckType.parentCode);
                }
                 showMessage("修改成功！", true, "success");
            }
            else
            {
                showMessage("修改失败，原因：" + streditmasge, false, "");

            }
        }
        else if (strCtrl.Equals("Add"))
        {
            string strMsg = "";
            //判断该类型是否存在
           
            string strcode = this.txtCode.Text.Trim();
            if (bllTruckTypeBll.IsTruckCode(strcode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('编号已存在，建议用系统编号！');", true);
                this.txtCode.Text = HiddenFieldCode.Value.ToString().Trim();
                return;
                
            }
          
            
            int iRel = bllTruckTypeBll.Add(modelTruckType, out strMsg);
            if (iRel > 0)
            {
                if (!modelTruckType.parentCode.Equals("") && !modelTruckType.parentCode.Equals("0"))
                {
                    int row = bllTruckTypeBll.changeParentStatus(modelTruckType.parentCode);
                }
                showMessage("保存成功！", true, "success");
            }
            else
            {
                showMessage("保存失败，原因：" + strMsg, false, "");
            }
        }
       
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg,bool isExit,string strReturnVal) {
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

    /// <summary>
    /// 获取所有类别
    /// </summary>
    /// <returns></returns>
    private string getAllType() {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        DataSet ds = server.GetDataSet("select '['+CAST(typeCode AS varchar(100)) +']'+typeName as kemu from  T_truckType");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kemu"]));
            arry.Append("',");
        }
        if (arry.Length>1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }
       
    }
    /// <summary>
    /// 生成编号
    /// </summary>
    private string CreateCode()
    {
        string strCode = new DataDicDal().GetYbbxBillName("", DateTime.Now.ToString("yyyyMMdd"), 1, 3);
        if (strCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
        }
        return strCode;
    }
}
