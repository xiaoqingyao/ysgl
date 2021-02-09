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
                string str_sql = "select diccode,dicname,isnull(cdj,'0') as cdj from bill_dataDic where diccode ='" + strcode + "' and dictype='" + strtype + "' ";
                DataSet ds = server.GetDataSet(str_sql);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    txt_diccode.Text = ds.Tables[0].Rows[0]["diccode"].ToString();
                    txb_dicname.Text = ds.Tables[0].Rows[0]["dicname"].ToString();
                    DropDownList2.SelectedValue = ds.Tables[0].Rows[0]["cdj"].ToString(); ;
                }
            }
        }
    }
    #endregion

    #region 保存
    protected void btn_save_Click(object sender, EventArgs e)
    {

        string str_sql = "";
        if (Request.QueryString["type"].ToString() == "add")
        {

            DataSet temp = server.GetDataSet("select dicCode from bill_datadic where dicCode='" + this.txt_diccode.Text.ToString().Trim() + "' and dicType='" + Page.Request.QueryString["dictype"].ToString().Trim() + "'");
            if (temp.Tables[0].Rows.Count != 0)
            {
                this.CreateDicCode(Request.QueryString["dictype"].ToString());
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('该编号的数据字典已存在,系统已重新生成,请保存！');", true);
                this.btnAgain.Visible = true;
                return;
            }
            str_sql = "insert into bill_dataDic(dictype,diccode,dicname,cdj) values('" + ddl_dictype.SelectedValue + "','" + txt_diccode.Text.Trim() + "','" + txb_dicname.Text.Trim() + "','" + DropDownList2.SelectedValue + "');";

        }
        else
        {
            str_sql = "update bill_dataDic set dicname ='" + txb_dicname.Text.Trim() + "',cdj='" + DropDownList2.SelectedValue + "' where dictype='" + ddl_dictype.SelectedValue + "' and diccode='" + txt_diccode.Text.Trim() + "'";
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
}
