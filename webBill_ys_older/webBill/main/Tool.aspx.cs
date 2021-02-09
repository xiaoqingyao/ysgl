using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;

public partial class webBill_main_Tool : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string userCode = Convert.ToString(Session["userCode"]);
            UserMessage userMsg = new UserMessage(userCode);
            userName.InnerHtml = "[" + userMsg.Users.UserCode + "]" + userMsg.Users.UserName;
            deptName.InnerHtml = userMsg.GetRootDept().DeptName + "--" + userMsg.GetDept().DeptName;
        }
    }
}
