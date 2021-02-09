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
using Bll.ReportAppBLL;
using System.Text;

public partial class ZiChan_ZiChanGuanLi_ZiChan_JiluDetail : System.Web.UI.Page
{
    string strbillcode = "";
    string strtype = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ZiChan_JiluBll zcjlbll = new ZiChan_JiluBll();
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
            ClientScript.RegisterArrayDeclaration("deptAll", GetDeptAll());
            ClientScript.RegisterArrayDeclaration("availableTagssyzk", GetsyzkAll());
            ClientScript.RegisterArrayDeclaration("availableTagszjfs", GetZjfsAll());
            ClientScript.RegisterArrayDeclaration("availableTagszclb", GetZclbAll());
            this.txtQiYongDate.Attributes.Add("onfocus", "javascript:setday(this);");

        }

    }

    private void bindData()
    {
        if (strtype == "Add")
        {
            Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            string strneed = DateTime.Now.ToString("yyyyMMdd");
            string strcode = pusbll.GetBillCode("ZCJL", strneed, 1, 6);
            this.lblcode.Text = strcode;
        }
        if (strbillcode != null && strbillcode != "")
        {
            if (strtype == "Edit")
            {
                getmodel();
                txtShiYongZhuangKuangCode.Enabled = false;
                txtShiYongBuMenCode.Enabled = false;
                txtWeiZhi.Enabled = false;
            }
            if (strtype == "look")
            {
                getmodel();
                this.btnsave.Visible = false;
            }

        }

    }
    /// <summary>
    /// 部门
    /// </summary>
    /// <returns></returns>
    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments where IsSell='Y'");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 增减方式
    /// </summary>
    /// <returns></returns>
    private string GetZjfsAll()
    {
        DataSet ds = server.GetDataSet("select '['+FangshiCode+']'+Fangshiname as fsname from ZiChan_ZengJianFangShi ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["fsname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }
    /// <summary>
    /// 使用状况
    /// </summary>
    /// <returns></returns>
    private string GetsyzkAll()
    {
        DataSet ds = server.GetDataSet("select '['+ZhuangKuangCode+']'+ZhuangKuangName as syzkname from ZiChan_ShiYongZhuangKuang  ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["syzkname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;
    }
    /// <summary>
    /// 资产类别
    /// </summary>
    /// <returns></returns>
    private string GetZclbAll()
    {
        DataSet ds = server.GetDataSet("select '['+LeibieCode+']'+LeibieName as lbname from  ZiChan_Leibie  ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["lbname"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);
        return script;

    }

    public void getmodel()
    {
        Models.ZiChan_Jilu model = new Models.ZiChan_Jilu();
        if (strbillcode != "" && strbillcode != null)
        {
            model = zcjlbll.GetModel(strbillcode);
            this.lblcode.Text = strbillcode;
            this.txtreportname.Text = model.ZiChanName;
            if (model.ZengJianFangShiCode != null && model.ZengJianFangShiCode != "")
            {
                string sql = "select '['+FangshiCode+']'+Fangshiname from ZiChan_ZengJianFangShi where FangshiCode='" + model.ZengJianFangShiCode + "'";
                string strzjfscode = server.GetCellValue(sql);
                this.txtZengJianFangShiCode.Text = strzjfscode;
            }
            if (model.ShiYongZhuangKuangCode != null && model.ShiYongZhuangKuangCode != "")
            {
                string sql = "select '['+ZhuangKuangCode+']'+ZhuangKuangName from ZiChan_ShiYongZhuangKuang where ZhuangKuangCode='" + model.ShiYongZhuangKuangCode + "'";
                string strsyzkcode = server.GetCellValue(sql);
                this.txtShiYongZhuangKuangCode.Text = strsyzkcode;
            }
            this.txtGuiGeXingHao.Text = model.GuiGeXingHao;
            this.txtShiYongQiXian.Text = model.ShiYongQiXian.ToString();
            this.txtYuanZhi.Text = model.YuanZhi.ToString();
            if (model.ShiYongBuMenCode != null && model.ShiYongBuMenCode != "")
            {
                string sql = "select '['+deptCode+']'+deptName from bill_departments where deptCode='" + model.ShiYongBuMenCode + "'";
                string strsybmcode = server.GetCellValue(sql);
                this.txtShiYongBuMenCode.Text = strsybmcode;
            }

            this.txtWeiZhi.Text = model.WeiZhi;
            if (model.CaiGouBuMenCode != null && model.CaiGouBuMenCode != "")
            {
                string sql = "select '['+deptCode+']'+deptName from bill_departments where deptCode='" + model.CaiGouBuMenCode + "'";
                string strsybmcode = server.GetCellValue(sql);
                this.txtCaiGouBuMenCode.Text = strsybmcode;
            }
            this.txtQiYongDate.Text = model.QiYongDate;
            this.txtreportexplain.Text = model.BeiZhu;


            if (model.LeiBieCode != null && model.LeiBieCode != "")
            {
                string sql = "select '['+LeibieCode+']'+LeibieName from  ZiChan_Leibie where LeibieCode ='" + model.LeiBieCode + "'";
                string strzclbcode = server.GetCellValue(sql);
                this.txtleibiecode.Text = strzclbcode;
            }

        }

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

        Models.ZiChan_Jilu model = new Models.ZiChan_Jilu();
        model.ZiChanCode = this.lblcode.Text.Trim();
        model.BeiZhu = this.txtreportexplain.Text;
        string strzcname = this.txtreportname.Text.Trim();
        if (strzcname.Equals(""))
        {
            showMessage("资产名称不能为空", false, "");
            return;
        }
        model.ZiChanName = strzcname;
        string strlbcode = this.txtleibiecode.Text.Trim();
        try
        {
            strlbcode = strlbcode.Substring(1, strlbcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
          
            strlbcode = "";
            this.txtleibiecode.Text = "";
        }
        if (strlbcode.Equals(""))//资产类别
        {
            showMessage("资产类别不能为空！", false, "");
            return;
        }

        model.LeiBieCode = strlbcode;

        string strZengJianFangShicode = this.txtZengJianFangShiCode.Text.Trim();
        try
        {
            strZengJianFangShicode = strZengJianFangShicode.Substring(1, strZengJianFangShicode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            this.txtZengJianFangShiCode.Text = "";
            strZengJianFangShicode = "";
        }
        if (strZengJianFangShicode.Equals(""))
        {
            showMessage("增减方式不能为空！", false, "");
            return;
        }
        model.ZengJianFangShiCode = strZengJianFangShicode;



        string strsyzkcode = this.txtShiYongZhuangKuangCode.Text;

        try
        {
            strsyzkcode = strsyzkcode.Substring(1, strsyzkcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strsyzkcode = "";
        }
        //if (strsyzkcode.Equals(""))//使用状况
        //{
        //    showMessage("使用状况不能为空！", false, "");

        //    return;
        //}
        model.ShiYongZhuangKuangCode = strsyzkcode;

        string strShiYongQiXian = this.txtShiYongQiXian.Text.Trim();
        if (!strShiYongQiXian.Equals(""))
        {
            int iShiYongQiXian = 0;
            if (int.TryParse(strShiYongQiXian, out iShiYongQiXian))
            {
                model.ShiYongQiXian = iShiYongQiXian;
            }
            else
            {
                showMessage("使用期限输入不合法！", false, ""); return;
            }
        }
        else
        {
            model.ShiYongQiXian = 0;
        }

        string stryz = this.txtYuanZhi.Text;
        if (!stryz.Equals(""))
        {
            decimal isyz = 0;
            if (decimal.TryParse(stryz, out isyz))
            {
                model.YuanZhi = isyz;
            }
            else
            {
                showMessage("原值输入不合法！", false, ""); return;
            }
        }
        else
        {
            model.YuanZhi = 0;
        }

        model.WeiZhi = this.txtWeiZhi.Text;

        model.QiYongDate = this.txtQiYongDate.Text;
        model.LuRuRenCode = Session["userCode"].ToString();
        model.LuRuDate = DateTime.Now.ToString("yyyy-MM-dd");





        model.GuiGeXingHao = this.txtGuiGeXingHao.Text;
        model.BeiZhu = this.txtreportexplain.Text;

        string strsydeptcode = this.txtShiYongBuMenCode.Text;

        try
        {
            strsydeptcode = strsydeptcode.Substring(1, strsydeptcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strsydeptcode = "";
        }
        
        model.ShiYongBuMenCode = strsydeptcode;



        string strcgdeptcode = this.txtCaiGouBuMenCode.Text.Trim();
        try
        {
            strcgdeptcode = strcgdeptcode.Substring(1, strcgdeptcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strcgdeptcode = "";
        }
       

        model.CaiGouBuMenCode = strcgdeptcode;







        int row = zcjlbll.Add(model);
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
