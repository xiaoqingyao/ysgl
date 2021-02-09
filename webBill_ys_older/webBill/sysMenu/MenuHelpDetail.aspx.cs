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
using Bll;

public partial class webBill_sysMenu_MenuHelpDetail : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    sysMenuHelpBLL bllhelp = new sysMenuHelpBLL();
    string menuid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        menuid = Convert.ToString(Request.QueryString["id"]);
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }   
        else if (!IsPostBack)
        {
          
            if (string.IsNullOrEmpty(menuid))
            {
                this.Button1.Visible = false;
                this.lblUserInfo.Text = "请选择相关菜单后进行编辑...";
            }
            else 
            {
                string name = server.GetCellValue(" select showname from bill_sysMenu where menuid='" + menuid + "'");
                h_title.InnerText = "[" + menuid + "]" + name;
                this.lblUserInfo.Text = "当前菜单：[" + menuid + "]" + name;
                Bind();
            }
        }
    }

    private void Bind()
    {
        FCKeditor1.Value = bllhelp.GetContent(menuid);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (bllhelp.Add(menuid, FCKeditor1.Value) > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('保存成功！');", true);
        }
        else {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "aaa", "alert('保存失败！');", true);
        }
    }
}
