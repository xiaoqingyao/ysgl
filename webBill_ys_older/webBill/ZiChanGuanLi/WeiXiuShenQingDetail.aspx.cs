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
using Bll.UserProperty;
using Models;
using System.Collections.Generic;

/// <summary>
/// 维修申请明细页
/// Edit by Lvcc
/// </summary>
public partial class webBill_ZiChanGuanLi_WeiXiuShenQingDetail : System.Web.UI.Page
{
    string strCtrl = "";
    string strCode = "";
    DataTable dtWeiXiuType = new DataTable();
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            object objCode = Request["Code"];
            if (objCode!=null)
            {
                strCode = objCode.ToString();
            }
            object objCtrl = Request["Ctrl"];
            if (objCtrl == null)
            {
                showMessage("重要参数丢失,请回到列表页刷新后重试！", true, "");return;
            }
            else
            {
                strCtrl = objCtrl.ToString();
                if ((strCtrl.Equals("Edit")||strCtrl.Equals("View"))&&strCode.Equals(""))
                {
                    showMessage("重要参数丢失,请回到列表页刷新后重试！", true, ""); return;
                }
            }
            string strsql = "select dicCode,dicName from bill_datadic where dictype='09'";
            dtWeiXiuType = server.GetDataTable(strsql, null);
            if (!IsPostBack)
            {
                initContrl();
                bindData();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        }
    }
    /// <summary>
    /// 初始化控件
    /// </summary>
    private void initContrl()
    {
        if (strCtrl.Equals("Add") || strCtrl.Equals("Edit"))
        {
            btn_ok.Visible = btn_cancel.Visible = false;//审核按钮
        }
        else if (strCtrl.Equals("View"))
        {
            btn_bc.Visible = btn_insert.Visible = btn_Del.Visible = false;//明细按钮
            this.btn_ok.Visible = this.btn_cancel.Visible = false;
            txtAppPersion.ReadOnly = this.txtAppDate.ReadOnly = txtShuoMing.ReadOnly = true;
        }
        else { }
    }
    /// <summary>
    /// 绑定数据
    /// </summary>
    private void bindData()
    {
        if (strCtrl.Equals("Add"))
        {
            string strUserCode = Session["userCode"].ToString();
            UserMessage userManager = new UserMessage(strUserCode);
            txtJingBanPersion.Text = this.txtAppPersion.Text = "[" + userManager.Users.UserCode + "]" + userManager.Users.UserName;
            txtAppDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.lblDept.Text = "[" + userManager.GetRootDept().DeptCode + "]" + userManager.GetRootDept().DeptName;
            List<ZiChan_Jilu> lstModel = new List<ZiChan_Jilu>();
            this.DataGrid1.DataSource = lstModel;
            this.DataGrid1.DataBind();
        }
        else if (strCtrl.Equals("View") || strCtrl.Equals("Edit"))
        {
            if (strCode.Equals(""))
	        {
                showMessage("重要参数丢失，请刷新后重试！",true,"no");return;
	        }
            Bill_Main modelBillMian = new Bll.Bills.BillMainBLL().GetModel(strCode);
            UserMessage userManager=new UserMessage(modelBillMian.BillUser);
            this.txtAppPersion.Text = "[" + userManager.Users.UserCode + "]" + userManager.Users.UserName;
            this.lblDept.Text = "[" + userManager.GetRootDept().DeptCode + "]" + userManager.GetRootDept().DeptName;
            this.txtAppDate.Text = ((DateTime)modelBillMian.BillDate).ToString("yyyy-MM-dd");
            IList<ZiChan_WeiXiuShenQing> lstmodelWeiXiuShenQing = new Bll.Zichan.ZiChan_WeiXiuShenQingBLL().GetListModel(strCode);
            if (lstmodelWeiXiuShenQing == null||lstmodelWeiXiuShenQing.Count==0)
            {
                showMessage("没有获取到维修申请的对象，请刷新后重试！", true, "no"); return;
            }
            userManager = new UserMessage(lstmodelWeiXiuShenQing[0].JingBanRenCode);
            this.txtJingBanPersion.Text = "[" + userManager.Users.UserCode + "]" + userManager.Users.UserName;
            this.txtShuoMing.Text = lstmodelWeiXiuShenQing[0].ShuoMing;

            //绑定明细
            List<ZiChan_Jilu> listModelZiChanJilu = new List<ZiChan_Jilu>();
            for (int i = 0; i < lstmodelWeiXiuShenQing.Count; i++)
            {
                ZiChan_Jilu modelZiChanJiLu = new Bll.Zichan.ZiChan_JiluBll().GetModel(lstmodelWeiXiuShenQing[i].ZiChanCode);
                if (modelZiChanJiLu!=null)
                {
                    modelZiChanJiLu.Note1 = lstmodelWeiXiuShenQing[i].WeiXiuTypeCode;
                    modelZiChanJiLu.Note2 = lstmodelWeiXiuShenQing[i].YuJiJinE.ToString();
                    listModelZiChanJilu.Add(modelZiChanJiLu);
                }
            }
            DataGrid1.DataSource = listModelZiChanJilu;
            DataGrid1.DataBind();
        }
        else { }
    }
    /// <summary>
    /// 添加明细
    /// </summary>
    protected void btnAdd_Server_Click(object sender, EventArgs e)
    {
        string strZiChanCodes = this.hdZiChanCodes.Value;
        string strappUserCode = this.txtAppPersion.Text.Trim();
        try
        {
            strappUserCode = strappUserCode.Substring(1, strappUserCode.IndexOf("]")-1);
        }
        catch (Exception)
        {
            strappUserCode = "";
        }
        if (!strappUserCode.Equals(""))
        {
            UserMessage userManager = new UserMessage(strappUserCode);
            Bill_Departments modelDept=userManager.GetRootDept();
            this.lblDept.Text = "[" + modelDept.DeptCode + "]" + modelDept.DeptName;
        }
        if (string.IsNullOrEmpty(strZiChanCodes))
        {
            return;
        }
        string[] arrZiChanCodes = strZiChanCodes.Split(new string[] { "|&|" }, StringSplitOptions.RemoveEmptyEntries);
        if (arrZiChanCodes.Length == 0)
        {
            return;
        }
        List<ZiChan_Jilu> lstModel = new List<ZiChan_Jilu>();
        for (int i = 0; i < arrZiChanCodes.Length; i++)
        {
            Bll.Zichan.ZiChan_JiluBll bllZiChanJiLu = new Bll.Zichan.ZiChan_JiluBll();
            lstModel.Add(bllZiChanJiLu.GetModel(arrZiChanCodes[i]));
        }
        this.DataGrid1.DataSource = lstModel;
        this.DataGrid1.DataBind();
    }
    /// <summary>
    /// 删除明细
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_Del_Click(object sender, EventArgs e)
    {
        List<ZiChan_Jilu> lstModel = new List<ZiChan_Jilu>();
        int iCount = this.DataGrid1.Items.Count;
        for (int i = 0; i < iCount; i++)
        {
            CheckBox chb = this.DataGrid1.Items[i].Cells[0].FindControl("cbSelect") as CheckBox;
            if (chb == null || chb.Checked)
            {
                continue;
            }
            string strZiChanCode = this.DataGrid1.Items[i].Cells[1].Text.Trim();
            string strZiChanName = this.DataGrid1.Items[i].Cells[2].Text.Trim();
            string strLeiBie = this.DataGrid1.Items[i].Cells[3].Text.Trim();
            string strGuiGeXingHao = this.DataGrid1.Items[i].Cells[4].Text.Trim();
            string strWeiXiuLeiBie = "";
            DropDownList ddlWeiXiuLeiBie = this.DataGrid1.Items[i].Cells[5].FindControl("ddlWeiXiuLeiBie") as DropDownList;
            if (ddlWeiXiuLeiBie != null)
            {
                strWeiXiuLeiBie = ddlWeiXiuLeiBie.SelectedValue.Trim();
            }
            decimal Je = 0;//预计金额
            TextBox txtNeedJe = this.DataGrid1.Items[i].Cells[4].FindControl("txtNeedJe") as TextBox;
            if (txtNeedJe != null)
            {
                string strNeedJe = txtNeedJe.Text.Trim();
                if (!strNeedJe.Equals(""))
                {
                    decimal.TryParse(strNeedJe, out Je);
                }
            }
            ZiChan_Jilu modelZiChanJilu = new ZiChan_Jilu();
            modelZiChanJilu.ZiChanCode = strZiChanCode;
            modelZiChanJilu.ZiChanName = strZiChanName;
            modelZiChanJilu.LeiBieCode = strLeiBie;
            modelZiChanJilu.GuiGeXingHao = strGuiGeXingHao;
            modelZiChanJilu.Note1 = strWeiXiuLeiBie;//维修类别
            modelZiChanJilu.Note2 = Je.ToString();
            lstModel.Add(modelZiChanJilu);
        }
        this.DataGrid1.DataSource = lstModel;
        this.DataGrid1.DataBind();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {
        int iMingXiCount = this.DataGrid1.Items.Count;
        if (iMingXiCount == 0)
        {
            showMessage("资产维修明细不能为空！", false, ""); return;
        }
        string strAppPersion = this.txtAppPersion.Text.Trim();
        if (strAppPersion.Equals(""))
        {
            showMessage("申请人不能为空！", false, ""); return;
        }
        string strAppDept = this.lblDept.Text.Trim();
        string strAppDate = this.txtAppDate.Text.Trim();
        if (strAppDate.Equals(""))
        {
            showMessage("申请日期不能为空！", false, ""); return;
        }
        string strJingBanRen = this.txtJingBanPersion.Text.Trim();
        if (strJingBanRen.Equals(""))
        {
            showMessage("经办人不能为空！", false, ""); return;
        }
        string strShuoMing = this.txtShuoMing.Text.Trim();
        if (strShuoMing.Equals(""))
        {
            showMessage("申请说明不能为空！", false, ""); return;
        }
        //单据编号
        string strBillCode = "";
        if (this.strCtrl.Equals("Add"))
        {
            strBillCode = new Bll.PublicServiceBLL().GetBillCode("wxsq", DateTime.Now.ToString("yyyyMMdd"), 1, 5);
        }
        else if (strCtrl.Equals("Edit"))
        {
            if (!strCode.Equals(""))
            {
                strBillCode = strCode;
            }
           
        }
        else {
            showMessage("未知参数："+strCode, false, ""); return;
        }

        if (strBillCode.Equals(""))
        {
            showMessage("生成单号失败，请联系开发商解决！", false, ""); return;
        }
        //做modellist
        List<ZiChan_WeiXiuShenQing> lstmodelWeiXiuShenQing = new List<ZiChan_WeiXiuShenQing>();
        for (int i = 0; i < iMingXiCount; i++)
        {
            
            ZiChan_WeiXiuShenQing modelWeiXiuShenQing  = new ZiChan_WeiXiuShenQing();
            modelWeiXiuShenQing.JingBanRenCode = strJingBanRen.Substring(1, strJingBanRen.IndexOf("]") - 1); ;
            modelWeiXiuShenQing.MainCode = strBillCode;
            modelWeiXiuShenQing.ShenQingRenCode = strAppPersion.Substring(1, strAppPersion.IndexOf("]") - 1);
            modelWeiXiuShenQing.ShuoMing = strShuoMing;
            DropDownList ddlWeiXiuType = this.DataGrid1.Items[i].Cells[5].FindControl("ddlWeiXiuLeiBie") as DropDownList;
            if (ddlWeiXiuType != null)
            {
                modelWeiXiuShenQing.WeiXiuTypeCode = ddlWeiXiuType.SelectedValue.Trim();
            }
            else
            {
                showMessage("添加明细时出现未知错误，请联系开发商解决！", false, ""); break;
            }
            TextBox txtYjje = this.DataGrid1.Items[i].Cells[6].FindControl("txtNeedJe") as TextBox;
            if (txtYjje != null)
            {
                decimal deyjje = 0;
                if (decimal.TryParse(txtYjje.Text, out deyjje))
                {
                    modelWeiXiuShenQing.YuJiJinE = deyjje;
                }
                else
                {
                    showMessage("第" + (i + 1) + "行预计金额格式不正确！", false, ""); break;
                }
            }
            else
            {
                showMessage("添加明细时出现未知错误，请联系开发商解决！", false, ""); break;
            }
            string strZiChanCode = this.DataGrid1.Items[i].Cells[1].Text.Trim();
            strZiChanCode = strZiChanCode.Replace("&nbsp;", "");
            if (strZiChanCode.Equals(""))
            {
                showMessage("第" + (i + 1) + "行没有找到资产编号，请检查该资产的详细信息！", false, ""); break;
            }
            modelWeiXiuShenQing.ZiChanCode = strZiChanCode;
            modelWeiXiuShenQing.Note1 = strAppDate;
            string strdeptcodes;
            try
            {
                strdeptcodes = strAppDept.Substring(1, strAppDept.IndexOf("]") - 1);
            }
            catch (Exception)
            {

                strdeptcodes="";
            }
            
            modelWeiXiuShenQing.Note2 = strdeptcodes;
            lstmodelWeiXiuShenQing.Add(modelWeiXiuShenQing);
        }
        string strmsg = "";
        int iRel = new Bll.Zichan.ZiChan_WeiXiuShenQingBLL().Add(lstmodelWeiXiuShenQing, out strmsg);
        if (iRel <= 0)
        {
            showMessage("保存失败，原因：" + strmsg, true, "no");
        }
        else
        {
            showMessage("保存成功！", true, "yes");
        }
    }
    protected void DataGrid1_OnItemCommand(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Pager)
        {
            //维修类别绑定
            DropDownList ddlWeiXiuType = e.Item.Cells[5].FindControl("ddlWeiXiuLeiBie") as DropDownList;
            if (ddlWeiXiuType != null && dtWeiXiuType != null)
            {
                ddlWeiXiuType.DataSource = dtWeiXiuType;
                ddlWeiXiuType.DataTextField = "dicName";
                ddlWeiXiuType.DataValueField = "dicName";
                ddlWeiXiuType.DataBind();
            }
            //选中维修类别
            string strWeixiuType = e.Item.Cells[7].Text.Trim();
            strWeixiuType = strWeixiuType.Replace("&nbsp;", "");
            if (ddlWeiXiuType != null && !strWeixiuType.Equals(""))
            {
                ddlWeiXiuType.SelectedValue = strWeixiuType;
            }
            //预计金额
            string strYjje = e.Item.Cells[8].Text.Trim();
            strWeixiuType = strWeixiuType.Replace("&nbsp;", "");
            TextBox txtYjje = e.Item.Cells[8].FindControl("txtNeedJe") as TextBox;
            if (txtYjje != null && !strWeixiuType.Equals(""))
            {
                txtYjje.Text = strYjje;
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
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }
}
