using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_sjzd_sjzdDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string dicType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            Response.Cache.SetSlidingExpiration(true);
            Response.Cache.SetNoStore();
            object objtype = Request["dictype"];
            if (objtype != null)
            {
                dicType = objtype.ToString();
            }
            else
            {
                showMessage("重要参数丢失！", false, ""); return;
            }
            if (!IsPostBack)
            {
                DataSet temp = server.GetDataSet("select '00' as diccode,'数据字典大类' as dicname  union select diccode,dicname from bill_dataDic where dicType='00' order by dicCode asc");
                this.ddl_dictype.DataTextField = "dicName";
                this.ddl_dictype.DataValueField = "dicCode";
                this.ddl_dictype.DataSource = temp;
                this.ddl_dictype.DataBind();
                bindDate();
            }
        }
    }

    #region 绑定信息
    private void bindDate()
    {
        //控制控件的显示与隐藏
        if (dicType.Equals("02")){
            trReceiver.Visible = trShiFouMoRen.Visible = false;
        }
        else if (dicType.Equals("10")) {
            trChongJianYs.Visible = trFuJiadj.Visible = trKongZhiYs.Visible = false;
        } else { }
        if (Request.QueryString["type"].ToString() == "add")
        { 
            ddl_dictype.SelectedValue = Request.QueryString["dictype"].ToString();
            this.CreateDicCode(Request.QueryString["dictype"].ToString());
        }
        else
        {
            this.btnAgain.Visible = false;
            this.txt_diccode.ReadOnly = true;
            if (Request.QueryString["diccode"].ToString() != "" && Request.QueryString["dictype"].ToString() != "")
            {
                string strtype= Request.QueryString["dictype"].ToString();
                string strcode = Request.QueryString["diccode"].ToString();
                ddl_dictype.SelectedValue = strtype;
                ddl_dictype.Enabled = false;
                string str_sql = "select diccode,dicname,cjys,cys,cdj from bill_dataDic where diccode ='" + strcode + "' and dictype='" + strtype + "' ";
                DataSet ds = server.GetDataSet(str_sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    txt_diccode.Text = ds.Tables[0].Rows[0]["diccode"].ToString();
                    txb_dicname.Text = ds.Tables[0].Rows[0]["dicname"].ToString();
                    this.DropDownList1.SelectedValue = ds.Tables[0].Rows[0]["cjys"].ToString();
                    this.DropDownList2.SelectedValue = ds.Tables[0].Rows[0]["cys"].ToString();
                    this.DropDownList4.SelectedValue = ds.Tables[0].Rows[0]["cdj"].ToString();
                    this.ddlShiFouMoRen.SelectedValue = ds.Tables[0].Rows[0]["cdj"].ToString();//是否默认（帐套）
                    this.txtReceiver.Text = ds.Tables[0].Rows[0]["cjys"].ToString();//默认获取编号（帐套）
                }
            }
        }
        //如果配置为不启用预算管理 则禁用冲减预算和控制预算的控制项目 edit by lvcc 20120904
        bool boHasBudgetControl = new Bll.ConfigBLL().GetModuleDisabled("HasBudgetControl");
        if (!boHasBudgetControl)
        {
            this.DropDownList1.SelectedValue = "0";
            this.DropDownList2.SelectedValue = "0";
            this.DropDownList1.Enabled = this.DropDownList2.Enabled = false;
        }
    }
    #endregion

    #region 保存
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string str_sql = "";
        string strDicCode = this.txt_diccode.Text.Trim();
        string strDicName = this.txb_dicname.Text.Trim();
        string strKongZhiYs = this.DropDownList2.SelectedValue;
        string strShiFouMoren = this.ddlShiFouMoRen.SelectedValue;
        string strSuoShuZidian = this.ddl_dictype.SelectedValue;
        string strCjys="";//冲减预算
        string strFdj = "";//附加单据
        if (dicType.Equals("02") || dicType.Equals("21") || dicType.Equals("22") || dicType.Equals("23") || dicType.Equals("24"))//如果是报销明细类型
        {
            strCjys=this.DropDownList1.SelectedValue;
            strFdj = this.DropDownList4.SelectedValue;
        }else if(dicType.Equals("10")){//如果是帐套的话  冲减预算这个字段存储默认接受编号
            strCjys=this.txtReceiver.Text.Trim();
            strFdj = this.ddlShiFouMoRen.SelectedValue;
        }else{
            showMessage("未定义字段!",false,"");return;
        }
        if (Request.QueryString["type"].ToString() == "add")
        {
            DataSet temp = server.GetDataSet("select dicCode from bill_datadic where dicCode='" + strDicCode + "' and dicType='" + dicType + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                this.CreateDicCode(Request.QueryString["dictype"].ToString());
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的数据字典已存在,系统已重新生成,请保存！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            str_sql = "insert into bill_dataDic(dictype,diccode,dicname,cjys,cys,cdj) values('" + strSuoShuZidian + "','" + strDicCode + "','" + strDicName + "','" + strCjys + "','" + strKongZhiYs + "','" + strFdj + "');";
        }
        else
        {
            str_sql = "update bill_dataDic set dicname ='" + strDicName + "',cjys='" + strCjys + "',cys='" + strKongZhiYs + "',cdj='" + strFdj + "' where dictype='" + strSuoShuZidian + "' and diccode='" + strDicCode + "'";
        }
        if (server.ExecuteNonQuery(str_sql) != -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
    #endregion

    #region 取消
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }
    #endregion

    protected void btnAgain_Click(object sender, EventArgs e)
    {

        this.CreateDicCode(Request.QueryString["dictype"].ToString());
    }

    public void CreateDicCode(string dicType)
    {
        string dicCode = (new billCoding()).getDicCode(dicType);
        if (dicCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txt_diccode.Text = dicCode;
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
