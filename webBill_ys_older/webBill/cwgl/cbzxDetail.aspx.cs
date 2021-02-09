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

public partial class webBill_cwgl_cbzxDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            if (!IsPostBack)
            {
                bindDate();
            }
        }
    }

    private void bindDate()
    {
        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            this.CreateGroupCode();
        }
        else
        {
            string sql = "select * from bill_cbzx where zxCode='" + Page.Request.QueryString["zxCode"].ToString().Trim() + "'";
            DataSet temp = server.GetDataSet(sql);
            if (temp.Tables[0].Rows.Count == 1)
            {
                this.txtGroupID.Text = temp.Tables[0].Rows[0]["zxCode"].ToString();
                this.txtGroupID.ReadOnly = true;
                txb_groupname.Text = temp.Tables[0].Rows[0]["zxName"].ToString();
            }
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        string str_sql = "";
        string str_name = txb_groupname.Text;


        string type = Page.Request.QueryString["type"].ToString().Trim();
        if (type == "add")
        {
            DataSet temp = server.GetDataSet("select zxCode from bill_cbzx where zxCode='" + this.txtGroupID.Text.ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                this.CreateGroupCode();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的成本中心已存在,系统已重新生成,请保存！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            str_sql = "insert bill_cbzx (zxCode,zxName)values('" + this.txtGroupID.Text.ToString().Trim() + "','" + this.txb_groupname.Text.ToString().Trim() + "')";
        }
        else
        {
            str_sql = "update bill_cbzx set zxName='" + str_name + "' where zxCode='" + this.txtGroupID.Text.ToString().Trim() + "'";
        }

        if (server.ExecuteNonQuery(str_sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
    }

    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "self.close();", true);
    }


    protected void btnAgain_Click(object sender, EventArgs e)
    {
        this.CreateGroupCode();
    }
    public void CreateGroupCode()
    {
        string groupID = (new billCoding()).getCbzxCode();
        if (groupID == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('生成成本中心编号错误,请与开发商联系！');", true);
            this.btn_save.Visible = false;
        }
        else
        {
            this.txtGroupID.Text = groupID;
        }
    }
}
