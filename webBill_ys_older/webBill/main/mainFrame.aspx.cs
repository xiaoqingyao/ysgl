using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class main_mainFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "v", "window.open('../../webBill.aspx','_top');", true);
        }
        this.Title = ConfigurationManager.AppSettings["CustomTitle"].ToString() + " 预算管理系统";
        ////注册
        //object objregistermark_date = System.Configuration.ConfigurationManager.AppSettings["RegistDate"];
        //DateTime dtReg;
        //if (objregistermark_date != null)
        //{
        //    dtReg = DateTime.Parse(objregistermark_date.ToString());
        //    DateTime strnowdate = DateTime.Now;
        //    if (strnowdate > dtReg)
        //    {
        //        this.Title += "  试用版已到期";
        //    }
        //}




    }

}
