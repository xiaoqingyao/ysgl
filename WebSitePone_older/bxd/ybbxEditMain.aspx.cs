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
using Bll.UserProperty;
using System.Collections.Generic;
using Models;
using webBillLibrary;

public partial class bxd_ybbxEditMain : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    //protected string strdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Login.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            //strdate = DateTime.Now.ToString("yyyy-MM-dd");
            SysManager sysMgr = new SysManager();
            txtBillCode.Text = sysMgr.GetYbbxBillName("", DateTime.Now.ToString("yyyMMdd"), 1);
            string strType = Request.QueryString["type"];
            if (Page.Request.QueryString["djtype"] == null)
            {
                this.hd_djtype.Value = "ybbx";
                hfdydj.Value = "02";
                this.lbdjmc.Text = "费用报销单→表头信息";

            }
            else
            {
                this.hd_djtype.Value = Page.Request.QueryString["djtype"].ToString().Trim();
                this.lbdjmc.Text = "其他报销单";
                hfdydj.Value = "02";
            }


            IList<Bill_DataDic> list = (new SysManager()).GetDicByType("02");
            ddlBxmxlx.DataSource = list;
            ddlBxmxlx.DataTextField = "DicName";
            ddlBxmxlx.DataValueField = "DicCode";
            ddlBxmxlx.DataBind();

            //2014-04-28 beg
            if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "sr")
            {
                this.hd_djtype.Value = Page.Request.QueryString["dydj"].ToString().Trim();
                this.lbdjmc.Text = "收入单";
                ddlBxmxlx.SelectedValue = "04";
            }
            //2014-04-28 end

            //2014-04-29 beg
            if (!string.IsNullOrEmpty(Request["dydj"]) && Request["dydj"] == "gdzcgz")
            {
                this.hd_djtype.Value = Page.Request.QueryString["dydj"].ToString().Trim();
                hfdydj.Value = "03";
                this.lbdjmc.Text = "固定资产购置单";
                ddlBxmxlx.SelectedValue = "05";
            }
            //2014-04-29 end

          
            txtBillDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtZdr.Text = server.GetCellValue("select '['+usercode+']'+userName  from bill_users where userCode='" + Session["userCode"] + "'");
            txtBillDept.Text = server.GetCellValue("select '['+a.deptCode+']'+a.deptName  as dept from bill_departments a,bill_users b where a.deptCode=b.userDept and b.userCode='" + Session["userCode"] + "'");
            BindGk();
        }
    }


    private void BindGk()
    {
        DataTable dt = server.GetDataTable("select deptCode,'['+deptCode+']'+deptName as deptName from bill_departments where sjDeptCode=(select top 1 deptCode  from bill_departments where isnull(sjDeptCode,'')='')", null);
        ddlGkDept.DataSource = dt;
        ddlGkDept.DataTextField = "deptName";
        ddlGkDept.DataValueField = "deptCode";
        ddlGkDept.DataBind();
        ddlGkDept.Items.Insert(0, new ListItem("请选择", ""));
    }

    //protected void Button1_Click(object sender, EventArgs e)
    //{
    //    txtBxzy.Text = Request["a"];

    //    //Response.Write(Request["a"]);
    //}

    protected void btnSave_Click(object sender, EventArgs e)
    {

        string ysGuid = (new GuidHelper()).getNewGuid();
        string billName = txtBillCode.Text.Trim();
        string flowid = hd_djtype.Value;
        string billUser = (string)Session["userCode"];
        string date = txtBillDate.Text.Trim();
        //beg发布时间
        if (this.txtBillDate.Text.Trim() != "")
        {
            DateTime date1 = DateTime.MinValue;
            bool flag = DateTime.TryParse(this.txtBillDate.Text.ToString(), out date1);
            if (flag)
            {
                date = date1.ToString("yyyy-MM-dd");
            }
            else
            {
                Response.Write("<script>alert('日期格式错误！');</script>");
                txtBillDate.Focus();
                return;
            }
        }
        else
        {
            Response.Write("<script>alert('请填制单日期！');</script>");
            return;
        }
        //end发布时间
        

        
        string dept = PubMethod.SubString(txtBillDept.Text.Trim());
        string isgk = ddlIsGk.SelectedValue == "是" ? "1" : "0";
        string gkdept = isgk == "0" ? dept : ddlGkDept.SelectedValue;
        string bxlx = ddlBxmxlx.SelectedValue;
        string bxzy = txtBxzy.Text.Trim();
        string bxsm = txtBxsm.Text.Trim();
        string sql = string.Format("insert into ph_main ([billCode],[billName]  ,[flowID] ,[stepID] ,[billUser] ,[bxr] ,[billDate] ,[billDept] ,[loopTimes] ,[isgk] ,[gkdept] ,[bxmxlx] ,[bxzy] ,[bxsm])   values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", ysGuid, billName, flowid, "-1", billUser, billUser, date, dept, "0", isgk, gkdept, bxlx, bxzy, bxsm);
        if (server.ExecuteNonQuery(sql) > 0)
        {
            Response.Redirect("ybbxEditItem.aspx?billCode=" + ysGuid);
        }

    }
}
