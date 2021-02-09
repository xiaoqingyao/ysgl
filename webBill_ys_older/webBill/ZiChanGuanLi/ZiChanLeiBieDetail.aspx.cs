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
using Bll.Zichan;
using Models;
using System.Text;
using Dal.SysDictionary;

public partial class ZiChan_ZiChanGuanLi_ZiChanLeiBieDetail : System.Web.UI.Page
{
    string strCode = "";//类型编号
    string strCtrl = "";//操作类型 Add=添加 Edit=修改
    ZiChan_LeibieBll ZCLBBll = new ZiChan_LeibieBll();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        object objCode = Page.Request["Code"];
        if (objCode != null)
        {
            strCode = objCode.ToString();
        }
        object objCtrl = Page.Request["Ctrl"];
        if (objCtrl != null)
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
        ZiChan_Leibie modelLeibie = new ZiChan_Leibie();
        if (strCtrl.Equals("Add"))
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");

            string strcode = pusbll.GetBillCode("ZCLB", strneed, 1, 6);
            this.txtCode.Text = strcode;
            HiddenFieldCode.Value = strcode;
            this.txtshiyongqixian.Text = "1";
            if (strCode != "")
            {
                modelLeibie = ZCLBBll.GetModel(strCode);
                this.txtParent.Text = "[" + strCode + "]" + modelLeibie.LeibieName;
            }
        }
        else if (strCtrl.Equals("Edit"))
        {
            if (strCode.Equals(""))
            {
                showMessage("关键字丢失！", true, "");
            }
            else
            {
                this.txtCode.Enabled = false;
                modelLeibie = ZCLBBll.GetModel(strCode);
                this.txtCode.Text = modelLeibie.LeibieCode.ToString();
                this.txtName.Text = modelLeibie.LeibieName;
                this.txtshiyongqixian.Text = modelLeibie.ShiYongQiXian.ToString();
                this.txtjiliangdanwei.Text = modelLeibie.JiLiangDanWei.ToString();
                this.txtbeizhu.Text = modelLeibie.BeiZhu.ToString().Trim();

                ZiChan_Leibie modelParent = ZCLBBll.GetModel(modelLeibie.ParentCode);
                if (modelParent != null)
                {
                    this.txtParent.Text = string.Format("[{0}]{1}", modelParent.LeibieCode.ToString(), modelParent.LeibieName);
                }
                else
                {
                    this.chbRoot.Checked = true;
                }
            }
        }
        else { }
    }

    /// <summary>
    /// 确定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Yes_Click(object sender, EventArgs e)
    {
        ZiChan_Leibie modelLeibie = new ZiChan_Leibie();

        if (strCtrl.Equals("Edit"))
        {
            modelLeibie = ZCLBBll.GetModel(strCode);
            //如果是修改则重新调用值

        }

        modelLeibie.LeibieCode = this.txtCode.Text.Trim();
        string strParentCode = this.txtParent.Text.Trim();




       

        //if (strZengJianFangShicode.Equals(""))
        //{
        //    showMessage("增减方式不能为空！", false, "");
        //    return;
        //}
        //model.ZengJianFangShiCode = strZengJianFangShicode;



        if (chbRoot.Checked)
        {
            modelLeibie.ParentCode = "0";
        }
        else
        {
            //if (strParentCode.Equals(""))
            //{
            //    showMessage("如果该节点不作为根节点则上级单位不能为空！", false, "");
            //    return;
            //}
            //else
            //{
                try
                {
                    strParentCode = strParentCode.Substring(1, strParentCode.IndexOf("]") - 1);
                }
                catch (Exception)
                {
                    strParentCode = "";
                }
            //}
            if (strParentCode.Equals(""))
            {
                showMessage("如果该节点不作为根节点则上级单位不能为空！", false, "");
                return;
            }
            modelLeibie.ParentCode = strParentCode;
        }


        string strjldw = this.txtjiliangdanwei.Text;



        if (strjldw.Equals(""))
        {
            showMessage("计量单位不能为空！", false, "");
            return;
        }
        modelLeibie.JiLiangDanWei = strjldw;
        modelLeibie.LeibieName = this.txtName.Text.Trim();
        modelLeibie.BeiZhu = this.txtbeizhu.Text.Trim();
        if (modelLeibie.ParentCode.Equals(modelLeibie.LeibieCode))
        {
            showMessage("该节点本身不能作为父节点", false, "");
        }
        if (this.txtshiyongqixian.Text != "")
        {
            modelLeibie.ShiYongQiXian = int.Parse(this.txtshiyongqixian.Text);
        }
        else
        {
            modelLeibie.ShiYongQiXian = 1;
        }
        
        if (strCtrl.Equals("Edit"))
        {
            string streditmasge = "";
            int rows = ZCLBBll.Upd(modelLeibie, out streditmasge);
            if (rows > 0)
            {
                //if (!modelLeibie.ParentCode.Equals("") && !modelLeibie.ParentCode.Equals("0"))
                //{
                //    int row = ZCLBBll.changeParentStatus(modelLeibie.ParentCode);
                //}
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
            if (ZCLBBll.IsTruckCode(strcode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('编号已存在，建议用系统编号！');", true);
                this.txtCode.Text = HiddenFieldCode.Value.ToString().Trim();
                return;

            }


            int iRel = ZCLBBll.Add(modelLeibie, out strMsg);
            if (iRel > 0)
            {
                //if (!modelLeibie.ParentCode.Equals("") && !modelLeibie.ParentCode.Equals("0"))
                //{
                //    int row = ZCLBBll.changeParentStatus(modelLeibie.ParentCode);
                //}
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

    /// <summary>
    /// 获取所有类别
    /// </summary>
    /// <returns></returns>
    private string getAllType()
    {
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
        DataSet ds = server.GetDataSet("select '['+CAST(LeibieCode AS varchar(100)) +']'+LeibieName as kemu from  ZiChan_Leibie");
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
