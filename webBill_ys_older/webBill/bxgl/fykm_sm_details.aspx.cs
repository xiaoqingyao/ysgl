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
using System.Data.SqlClient;

public partial class webBill_bxgl_fykm_sm_details : System.Web.UI.Page
{
    string strfykm = "";
    string strsm = "";
    sqlHelper.sqlHelper sqlhelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            if (Request["txtfykm"] != null && Request["txtsm"] != null)
            {
                strfykm = Request["txtfykm"].ToString();
                strsm = Request["txtsm"].ToString();
                if (string.IsNullOrEmpty(strfykm) || string.IsNullOrEmpty(strsm))
                {
                    Response.Write("<script>alert('请填写费用科目和报销说明');</script>");
                    return;
                }
                try
                {
                    string strsql = @"insert into bill_datadic(dictype,diccode,dicname) values('19',@diccode,@dicname) ";
                  
                    int irel = sqlhelper.ExecuteNonQuery(strsql,  new SqlParameter[] { new SqlParameter("@diccode", strfykm), new SqlParameter("@dicname", strsm) });
                    if (irel > 0)
                    {
                        Response.Write("<script>alert('保存成功！');window.close();</script>");
                    }
                    else
                    {
                        throw new Exception("未知错误");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('保存失败，原因：" + ex.Message + "');window.close();</script>");
                }

            }
            else
            {
                Response.Write("<script>alert('请填写费用科目和报销说明');</script>");
            }
        }

    }
}
