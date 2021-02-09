using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Dal.SysDictionary;
using Bll.FeeApplication;
using Models;
using System.Text;
using Bll;
using Bll.Bills;

public partial class webBill_fysq_aqjdDetail : System.Web.UI.Page
{

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strCtrl = "";
    string strBillCode = "";
    string strUserCode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            strUserCode = Session["userCode"].ToString().Trim();
            object objCtrl = Request["Ctrl"];
            if (objCtrl != null)
            {
                strCtrl = objCtrl.ToString();
            }
            object objCode = Request["Code"];
            if (objCode != null)
            {
                strBillCode = objCode.ToString();
            }
            if (!IsPostBack)
            {
                switch (strCtrl)
                {
                    case "Add":
                        this.CreateQjdCode();
                        txtAppDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                        lbeAppPersion.Text = strUserCode;
                        btn_ok.Visible = false;
                        btn_cancel.Visible = false;
                        break;
                    case "View":
                        btn_ok.Visible = false;
                        btn_cancel.Visible = false;
                        btn_bc.Visible = false;
                        this.bindData();

                        break;
                    case "Update":
                        btn_ok.Visible = false;
                        btn_cancel.Visible = false;
                        this.bindData();

                        break;
                    default:
                        break;
                }


            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
        }
    }



    private void bindData()
    {
        string sql = "select * from bill_qjd where id=" + strBillCode;
        DataTable temp = server.GetDataSet(sql).Tables[0];
        if (temp.Rows.Count > 0)
        {
            if (temp.Rows[0]["begtime"] != null)
            {
                txtLoanDateFrm.Text = temp.Rows[0]["begtime"].ToString();
            }
            if (temp.Rows[0]["endtime"] != null)
            {
                txtLoanDateTo.Text = temp.Rows[0]["endtime"].ToString();
            }
            if (temp.Rows[0]["days"] != null)
            {
                txtDays.Text = temp.Rows[0]["days"].ToString();
            }
            if (temp.Rows[0]["reason"] != null)
            {
                txtReason.Text = temp.Rows[0]["reason"].ToString();
            }
            if (temp.Rows[0]["usercode"] != null)
            {
                lbeAppPersion.Text = temp.Rows[0]["usercode"].ToString();
            }
            if (temp.Rows[0]["reportdate"] != null)
            {
                txtAppDate.Value = temp.Rows[0]["reportdate"].ToString();
            }

        }
       
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

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.CreateQjdCode();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_bc_Click(object sender, EventArgs e)
    {

        string str_stepid = "-1";
        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");

        string addtime = txtAppDate.Value.Trim();
        string begtime = txtLoanDateFrm.Text.Trim();
        string endtime = txtLoanDateTo.Text.Trim();
       string days= txtDays.Text.Trim();
       string appuser = SubSting(lbeAppPersion.Text.Trim());
        
        string reason=txtReason.Text;

        DateTime dt1; bool dtBillDate = DateTime.TryParse(addtime, out dt1);
        bool boolbeg = DateTime.TryParse(begtime, out dt1);
        bool boolend=DateTime.TryParse(endtime, out dt1);
        float f1; bool booldays = float.TryParse(days,out f1);
        try
        {
           
            // 如果 s 参数成功转换，则为 true；否则为 false。
            if ( !dtBillDate||!boolbeg||!boolend)
            {            
                Response.Write("<script>alert(日期输入不合法！);</script>");
            }
            else if (!booldays)
            {
                Response.Write("<script>alert(天数输入不合法！);</script>");
            }
            else
            {
                System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
                if (strCtrl == "Add")
                {
                    DataSet temp = server.GetDataSet("select cgbh from bill_lscg where cgbh='" + this.lblCgbh.Text.ToString().Trim() + "'");
                    if (temp.Tables[0].Rows.Count != 0)
                    {
                        this.CreateQjdCode();
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的报告申请申请已存在,系统已重新生成,请保存！');", true);
                        this.Button1.Visible = true;
                        return;
                    }

                    list.Add("insert into bill_main(looptimes,billType,billcode,billname,flowid,stepid,billuser,billdate,billdept,billje) values(1,'1','" + this.lblCgbh.Text.ToString().Trim() + "','','qjd','" + str_stepid + "','" + str_billuser + "','" + addtime + "','" + str_billdept + "','0')");
            
                    list.Add("insert into bill_qjd (id,begtime,endtime,days,reason,usercode,reportdate) values('"+this.lblCgbh.Text.ToString().Trim()+"','" + begtime + "','" + endtime + "'," + days + ",'" + reason + "','" + appuser + "','" + addtime + "')");
                }
                else if (strCtrl == "Update")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("update bill_qjd set (");
                    sb.Append("begtime='" + endtime + "', ");
                    sb.Append("endtime='" + begtime + "', ");
                    sb.Append("days=" + days + ", ");
                    sb.Append("reason='" + reason + "', ");
                    sb.Append("usercode='" + appuser + "', ");
                    sb.Append("reportdate='" + addtime + "') ");
                    sb.Append(" where id=" + strBillCode);
                    list.Add(sb.ToString());
                    list.Add("update bill_main set  billname='' , billuser='" + str_billuser + "',billdate='" + addtime + "',billdept='" + str_billdept + "',billje='0',stepid='" + str_stepid + "' where flowid='qjd' and  billcode='" + strBillCode + "'");

                }
                if (server.ExecuteNonQuerysArray(list) == -1)
                {

                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存设置失败！');", true);
                }
                else
                {

                    //ClientScript.RegisterStartupScript(this.GetType(), "doOpen", "SuccessOk();", true);

                    ClientScript.RegisterStartupScript(this.GetType(), "aa", "window.close();", true);
                }
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,原因：" + ex.Message + "');", true);
        }
    }

    public void CreateQjdCode()
    {
        string Code = (new billCoding()).getQjdCode();
        if (Code == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
            this.btn_bc.Visible = false;
        }
        else
        {
            this.lblCgbh.Text = Code;
        }
    }

    public string SubSting(string longStr)
    {

        try
        {
            string result = "";
            if (!string.IsNullOrEmpty(longStr) && longStr.Length > 1 && longStr.IndexOf("[") != -1 && longStr.IndexOf("]") != -1)
            {
                int i = longStr.LastIndexOf("]");
                result = longStr.Substring(1, i - 1);
            }
            else
            {
                result = longStr;
            }
            return result;
        }
        catch (Exception e)
        {

            throw e;
        }
    }
    /// <summary>
    /// 返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_fh_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }




    /// <summary>
    /// 通过user的code获取DT
    /// </summary>
    /// <param name="userCode"></param>
    /// <returns></returns>
    private DataTable returnUserMsgDt(string userCode)
    {
        return server.GetDataSet(@"select userName,userPosition,
(select deptName from bill_departments where deptCode=a.userDept) as userDept,
(select  '['+dicCode+']'+dicName from bill_dataDic where dicType='05' and dicCode=a.userPosition)
as dicCodeName from bill_users a  where userCode='" + userCode + "'").Tables[0];//userStatus='1' and 
    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowDeptMsgByCode(string userCode)
    {
        return server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + userCode + "')");
    }
    /// <summary>
    /// 通过人员编号 返回用于显示的信息
    /// </summary>
    /// <returns></returns>
    private string getShowUserByCode(string userCode)
    {
        return server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + userCode + "'");
    }
}