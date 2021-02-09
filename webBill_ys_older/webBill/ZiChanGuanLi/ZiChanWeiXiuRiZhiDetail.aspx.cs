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
using System.Collections.Generic;
using Models;
using Bll.UserProperty;
using System.Text;

public partial class webBill_ZiChanGuanLi_ZiChanWeiXiuRiZhiDetail : System.Web.UI.Page
{
    string strbillcode = "";
    string strtype = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ZiChan_WeiXiuRiZhiBll wxrzbll = new ZiChan_WeiXiuRiZhiBll();
    ReportApplicationBLL reportbll = new ReportApplicationBLL();
    SysManager smgr = new SysManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            object objRequsttype = Request["Ctrl"];
            if (Request["Code"] != null && Request["Code"].ToString() != "")
            {
                strbillcode = Request["Code"].ToString();
            }
            if (objRequsttype != null && !string.IsNullOrEmpty(objRequsttype.ToString()))
            {
                strtype = objRequsttype.ToString();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("zcTags", GetzcAll());
            ClientScript.RegisterArrayDeclaration("userTags", GetUsersAll());
            //类别
            IList<Bill_DataDic> listwxlb = smgr.GetDicByType("09");
            int itypeCount = listwxlb.Count;
            if (itypeCount > 0)
            {
                for (int i = 0; i < itypeCount; i++)
                {
                    this.ddlSalewxlb.Items.Add(new ListItem("[" + listwxlb[i].DicCode + "]" + listwxlb[i].DicName, "[" + listwxlb[i].DicCode + "]" + listwxlb[i].DicName));
                }

                //this.lblDeptMsg.Text = this.ddlSaleDept.SelectedItem.Text.Trim();
            }

            if (!IsPostBack)
            {
                bindData();
            }
        }

    }

    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as yhnames from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["yhnames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    private string GetzcAll()
    {
        DataSet ds = server.GetDataSet("select '['+ZiChanCode+']'+ZiChanName as zcnames  from ZiChan_Jilu");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["zcnames"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;
    }

    private void bindData()
    {
        if (strtype == "Add")
        {
            string usercode = Convert.ToString(Session["userCode"]);
            UserMessage um = new UserMessage(usercode);
            txtwxname.Text = "[" + um.Users.UserCode + "]" + um.Users.UserName;
            Bill_Departments dept = um.GetRootDept();
            txtwxdeptname.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
            //Bll.PublicServiceBLL pusbll = new Bll.PublicServiceBLL();
            //string strneed = DateTime.Now.ToString("yyyyMMdd");
            //string strcode = pusbll.GetBillCode("wxrz", strneed, 1, 6);
            //this.lblcode.Text = strcode;
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
        Models.ZiChan_WeiXiuRiZhi model = new Models.ZiChan_WeiXiuRiZhi();
        if (strbillcode != "" && strbillcode != null)
        {
            model = wxrzbll.GetModel(int.Parse(strbillcode));
            if (model.ZiChanCode != null && model.ZiChanCode != "")
            {
                string strzcode = server.GetCellValue("select '['+ZiChanCode+']'+ZiChanName as zcnames  from ZiChan_Jilu  where ZiChanCode='" + model.ZiChanCode + "'");
                this.txtzccode.Text = strzcode;
            }
            if (model.WeiXiuRenCode!=null&&model.WeiXiuRenCode!="")
            {
                string strwxname = server.GetCellValue("select '['+userCode+']'+userName as yhnames from bill_users where userCode='"+model.WeiXiuRenCode+"'");
                this.txtwxname.Text = strwxname;
            }
            if (model.WeiXiuBuMenCode!=null&&model.WeiXiuBuMenCode!="")
            {
                string strdeptcode = server.GetCellValue("select '['+deptcode+']'+deptname as dept  from bill_departments where deptcode='"+model.WeiXiuBuMenCode+"'");
                this.txtwxdeptname.Text = strdeptcode;
            }
            if (model.WeiXiuTypeCode!=null&&model.WeiXiuTypeCode!="")
            {
                 string strtypecode = server.GetCellValue("select '['+dicCode+']'+dicName  from bill_dataDic where dicType='09' and dicCode='"+model.WeiXiuTypeCode+"'");
                 this.ddlSalewxlb.SelectedValue = strtypecode;
            }
           
            this.txtwxmoney.Text = model.WeiXiuJinE.ToString();
            // this.txtwxlb.Text = model.WeiXiuTypeCode;
           // this.dropzjfs.SelectedValue = model.ShiFouShenPi.ToString();
            this.txtreportexplain.Text = model.BeiZhu;
        }

    }

    protected void btnsave_Click(object sender, EventArgs e)
    {

        Models.ZiChan_WeiXiuRiZhi model = new Models.ZiChan_WeiXiuRiZhi();

        string strzccode = this.txtzccode.Text;
        try
        {
            strzccode = strzccode.Substring(1, strzccode.IndexOf("]") - 1);
        }
        catch (Exception)
        {
            strzccode = "";
           
        }
        if (strzccode.Equals(""))//资产编号
        {
            showMessage("资产编号不能为空！", false, "");
            return;
           
        }
        model.ZiChanCode = strzccode;

        string strusercode = this.txtwxname.Text;
        try
        {
            strusercode = strusercode.Substring(1, strusercode.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strusercode="";
        }
          
            model.WeiXiuRenCode = strusercode;
        

        model.ShiFouShenPi = "0";//是否审批

        //维修部门
        string strdeptcode = this.txtwxdeptname.Text;
        try
        {
            strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        }
        catch (Exception)
        {

            strdeptcode="";
        }

         model.WeiXiuBuMenCode = strdeptcode;
        
        if (ddlSalewxlb.Items.Count > 0)//维修类别
        {
            string strDicType = this.ddlSalewxlb.SelectedValue;
            try
            {
                strDicType = strDicType.Substring(1, strDicType.IndexOf("]") - 1);
            }
            catch (Exception)
            {

                strDicType="";
            }


            model.WeiXiuTypeCode = strDicType;
        }
        string strwxmoney = this.txtwxmoney.Text;
        if (strwxmoney.Equals(""))
        {
            showMessage("维修金额不能为空",false,"");
            return;
        }
        
        model.WeiXiuJinE = decimal.Parse(this.txtwxmoney.Text);
        model.XiTongShiJian = DateTime.Now.ToString("yyyy-MM-dd");//系统时间
        model.BeiZhu = this.txtreportexplain.Text;//备注

        
        if (strtype == "Add")
        {
            int row = wxrzbll.Add(model);
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
            model.listid = int.Parse(strbillcode);
            int row = wxrzbll.Add(model);
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
    private string GetDeoptAll()
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
