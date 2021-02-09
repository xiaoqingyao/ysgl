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

public partial class SaleBill_Flsz_RebatesStandardedit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                bindDate();
            }
        }
    }

    #region 绑定信息
    private void bindDate()
    {
        if (Request.QueryString["type"].ToString() == "edit")
        {
            if (Request.QueryString["diccode"].ToString() != "")
            {
               
                string strcode = Request.QueryString["diccode"].ToString();
              
                string str_sql = @"select a.* ,(select deptName from bill_departments where deptCode=a.DeptCode )as deptname,
                (select yskmMc from bill_yskm where yskmCode=a.SaleFeeTypeCode)as feename,
                (select typeName from T_truckType where typeCode=a.TruckTypeCode) as caname,
                (case [Type] when '0' then '期初分配' when '1' then '销售提成' when '2' then '配置项' end )as alltype,
                (case ControlItemCode  when (select Code from T_SaleProcess where Code=a.ControlItemCode) 
                 then (select PName from T_SaleProcess where Code=a.ControlItemCode) 
                when (select Code from  T_ControlItem where Code=a.ControlItemCode)
                 then (select CName from  T_ControlItem where Code=a.ControlItemCode)
                end)as feekz,(case Status when '1' then '启用' when '0' then '禁用' when '2' then '财务确认通过' end) as astatus,
                (select userName from bill_users where userCode=a.AuditUserCode)as username
                 from dbo.T_RebatesStandard a where a.NID='"+strcode+"'";
                DataSet ds = server.GetDataSet(str_sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    this.lbcartype.Text = ds.Tables[0].Rows[0]["caname"].ToString();
                    this.lbdept.Text = ds.Tables[0].Rows[0]["deptname"].ToString();
                    this.lbfeetype.Text = ds.Tables[0].Rows[0]["feename"].ToString();
                    this.lbfeecz.Text = ds.Tables[0].Rows[0]["feekz"].ToString();
                    this.txtfee.Text = ds.Tables[0].Rows[0]["Fee"].ToString();
                    this.lbtype.Text = ds.Tables[0].Rows[0]["alltype"].ToString();
                    this.lbstatus.Text = ds.Tables[0].Rows[0]["astatus"].ToString();
                    this.lbatuilusername.Text = ds.Tables[0].Rows[0]["username"].ToString();
                    this.lbbgtime.Text = ds.Tables[0].Rows[0]["EffectiveDateFrm"].ToString();
                    this.lbedtime.Text = ds.Tables[0].Rows[0]["EffectiveDateTo"].ToString();
                    this.lbbz.Text = ds.Tables[0].Rows[0]["Remark"].ToString();


                 
                }
            }
        }
    }
    #endregion

    #region 保存
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string str_sql = "";
        string strcode = Request.QueryString["diccode"].ToString();
        if (Request.QueryString["type"].ToString() == "edit")
        {
            decimal dcfee;
            if (this.txtfee.Text!=null&&this.txtfee.Text!="")
            {
               dcfee = decimal.Parse(this.txtfee.Text.Trim());
            }
            else
            {
                dcfee = decimal.Parse("0");
            }
            str_sql = "update T_RebatesStandard set Fee ='"+ dcfee + "'where NID='" + strcode + "'";
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

  
}
