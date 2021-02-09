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
using System.Collections.Generic;

public partial class BillBgsq_bgsqEdit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../inde.aspx','_self';", true);
            return;
        }
        if (string.IsNullOrEmpty(Request["type"]))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.location.href='../Index.aspx','_self';", true);
            return;
        }
        if (!IsPostBack)
        {
            BindDDL();
            if (Request["type"] == "add")
            {
                CreateLscgCode();
                txt_sj.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                 txt_cbr.Text = server.GetCellValue("select  '['+usercode+']'+userName from bill_users  where usercode='" + Session["userCode"].ToString() + "'");
                txt_dept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode=(select userdept from bill_users where usercode='" + Session["userCode"].ToString() + "')");

            }
            else if (Request["type"] == "edit" && !string.IsNullOrEmpty(Request["billCode"]))
            {
                BindData();
            }
        }
    }

    private void BindDDL()
    {
        DataSet temp = server.GetDataSet("select * from bill_dataDic where dicType='03' order by dicCode");
        ddlType.DataTextField = "dicName";
        ddlType.DataValueField = "dicCode";
        ddlType.DataSource = temp;
        ddlType.DataBind();
    }
    public void CreateLscgCode()
    {
        string lscgCode = (new billCoding()).getLscgCode();
        if (lscgCode == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成编号错误,请与开发商联系！');", true);
        }
        else
        {
            this.txt_cgbh.Text = lscgCode;
        }
    }
    private void BindData()
    {
        string code = Convert.ToString(Request["billCode"]);
        DataTable dt = server.GetDataTable("select * from bill_lscg where cgbh='" + code + "'", null);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txt_cgbh.Text = ObjectToStr(dr["cgbh"]);
            txt_sj.Text = Convert.ToDateTime(dr["sj"]).ToString("yyyy-MM-dd");
            txt_cbr.Text = server.GetCellValue("select  '['+usercode+']'+userName from bill_users  where usercode='" + ObjectToStr(dr["cbr"]) + "'"); 
            txt_yjfy.Text = ObjectToStr(dr["yjfy"]);
            txt_dept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + ObjectToStr(dr["cgDept"]) + "'");
            ddlType.SelectedValue = ObjectToStr(dr["cglb"]);
            txt_content.Text = ObjectToStr(dr["zynr"]);
            txt_sm.Text = ObjectToStr(dr["sm"]);
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {


        //beg发布时间
        string sj = "";
        if (this.txt_sj.Text.Trim() != "")
        {
            DateTime date = DateTime.MinValue;
            bool flag = DateTime.TryParse(this.txt_sj.Text.ToString(), out date);
            if (flag)
            {
                sj = date.ToString();
            }
            else
            {
                Response.Write("<script>alert('日期格式错误！');</script>");
                return;
            }
        }
        else
        {
            Response.Write("<script>alert('请填写发布日期！');</script>");
            return;
        }
        //end发布时间

        string str_stepid = "-1";
        string str_billuser = Session["userCode"].ToString().Trim();
        string str_billdate = sj;
        string str_billdept = server.GetCellValue("select userdept from bill_users where usercode='" + str_billuser + "'");
        string bm = txt_dept.Text.Trim();
        bm = bm.Substring(1, bm.IndexOf("]") - 1);

        string type = Request["type"];
        List<string> list = new List<string>();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select cgbh from bill_lscg where cgbh='" + txt_cgbh.Text.Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                this.CreateLscgCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的报告申请申请已存在,系统已重新生成,请保存！');", true);
                return;
            }

            list.Add("insert into bill_lscg(cgbh,sj,cgdept,cglb,zynr,sm,cbr,spyj01,spyj02,spyj03,spyj04,spyj05,spyj06,spyj07,spyj08,yjfy) values('" + txt_cgbh.Text.Trim() + "','" + str_billdate + "','" + bm + "','" + ddlType.SelectedValue + "','" + txt_content.Text.Trim() + "','" + txt_sm.Text.Trim() + "','" + Session["userCode"].ToString().Trim() + "','','','','','','','','','" + txt_yjfy.Text.Trim() + "') ");
            //申明主表添加
            list.Add("insert into bill_main(looptimes,billType,billcode,billname,flowid,stepid,billuser,billdate,billdept,billje) values(1,'1','" + txt_cgbh.Text.Trim() + "','','lscg','" + str_stepid + "','" + str_billuser + "','" + str_billdate + "','" + str_billdept + "','" + txt_yjfy.Text.Trim() + "')");

        }
        else //编辑
        {
            //修改单据时
            list.Add("update bill_lscg set  sj='" + str_billdate + "',cgdept='" + bm + "',cglb='" + ddlType.SelectedValue + "',zynr='" + txt_content.Text + "',sm='" + txt_sm.Text.Trim() + "',cbr='" + Session["userCode"].ToString().Trim() + "',yjfy='" + txt_yjfy.Text.Trim() + "' WHERE cgbh='" + Request.QueryString["billCode"].ToString() + "'");
            list.Add("update bill_main set  billname='' , billuser='" + str_billuser + "',billdate='" + str_billdate + "',billdept='" + str_billdept + "',billje='" + txt_yjfy.Text.Trim() + "',stepid='" + str_stepid + "' where flowid='lscg' and  billcode='" + Request.QueryString["billCode"].ToString() + "'");

        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.location.href='bgsqList.aspx';", true);
        }
    }

    private string ObjectToStr(object obj)
    {
        if (obj == null || Convert.ToString(obj) == string.Empty)
        {
            return "";
        }
        else
        {
            return obj.ToString();
        }
    }

}
