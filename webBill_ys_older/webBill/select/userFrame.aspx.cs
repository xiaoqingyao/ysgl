using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class user_userFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strFlg = "";
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        object objFlg = Request["Flg"];
        if (objFlg!=null)
        {
            strFlg = objFlg.ToString();
        }
        this.list.Attributes.Remove("src");
        this.list.Attributes.Add("src", "userList.aspx?deptCode=&Flg=" + strFlg);
        this.left.Attributes.Remove("src");
        this.left.Attributes.Add("src", "userLeft.aspx?Flg=" + strFlg);
    }
}
