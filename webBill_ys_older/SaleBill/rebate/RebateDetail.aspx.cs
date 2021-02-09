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

public partial class SaleBill_rebate_RebateDetail : System.Web.UI.Page
{
    string streditsmg = "";
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

                string str_sql = @"select a.*,(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.DeptCode)as deptName,
                (select '['+yskmCode+']'+yskmMc from bill_yskm where yskmCode= a.SaleFeeTypeCode )as feename,
                (case Status when '0' then '普通' when '1' then '财务确认' end)as astatus,
                (case RebatesType when '0' then '期初分配' when '1' then '销售提成' when '2' then '配置项' end) as rtype,
                (case ControlItemCode  when (select Code from T_SaleProcess where Code=a.ControlItemCode) 
                                 then (select  '['+Code+']'+PName from T_SaleProcess where Code=a.ControlItemCode) 
                                when (select Code from  T_ControlItem where Code=a.ControlItemCode)
                                 then (select'['+Code+']'+ CName from  T_ControlItem where Code=a.ControlItemCode)
                                end)as feekz
                 from T_SaleFeeAllocationNote a where 1=1  and Status<>'D' and a.Nid='" + strcode + "'";
                DataSet ds = server.GetDataSet(str_sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    this.lbbgtime.Text = ds.Tables[0].Rows[0]["ActionDate"].ToString();
                    this.lbdept.Text = ds.Tables[0].Rows[0]["deptName"].ToString();
                    this.lbcar.Text = ds.Tables[0].Rows[0]["TruckCode"].ToString();
                    this.lbcartype.Text = ds.Tables[0].Rows[0]["TruckTypeCode"].ToString();

                    this.lbfeetype.Text = ds.Tables[0].Rows[0]["feename"].ToString();
                    this.lbfeecz.Text = ds.Tables[0].Rows[0]["feekz"].ToString();

                    this.txtfee.Text = ds.Tables[0].Rows[0]["Fee"].ToString();
                    HiddenField1.Value= ds.Tables[0].Rows[0]["Fee"].ToString();
                    this.lbtype.Text = ds.Tables[0].Rows[0]["rtype"].ToString();

                    this.lbstatus.Text = ds.Tables[0].Rows[0]["astatus"].ToString();

                    this.lbbz.Text = ds.Tables[0].Rows[0]["Remark"].ToString();
                    HiddenField2.Value= ds.Tables[0].Rows[0]["ActionNote"].ToString();


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
            if (this.txtfee.Text != null && this.txtfee.Text != "")
            {
                dcfee = decimal.Parse(this.txtfee.Text.Trim());
            }
            else
            {
                dcfee = decimal.Parse("0");
            }
            
            streditsmg +=HiddenField2.Value+"[" + Session["userCode"].ToString() + "]" + Session["userName"].ToString() + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "将" + HiddenField1.Value.ToString()+ "改为" + dcfee;
            str_sql = "update T_SaleFeeAllocationNote set Fee ='" + dcfee + "',ActionNote='" + streditsmg + "' where Nid='" + strcode + "'";
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
