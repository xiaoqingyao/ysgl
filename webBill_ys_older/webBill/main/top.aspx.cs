using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using cn.com.webxml.www;
using Bll;
using System.Data;

public partial class main_top : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            Bll.ConfigBLL bllConfig = new Bll.ConfigBLL();
            DataTable dt;
            //企业Logo图片配置
            dt = bllConfig.GetDtByKey("CompanyLogo");
            if (dt.Rows.Count > 0&&!string.IsNullOrEmpty( Convert.ToString(dt.Rows[0]["avalue"])))
            {
                img_logo.Src ="../../" +  Convert.ToString(dt.Rows[0]["avalue"]);
            }

            string userCode = Convert.ToString(Session["userCode"]);
            UserMessage userMsg = new UserMessage(userCode);
            userName.InnerHtml = "[" + userMsg.Users.UserCode + "]" + userMsg.Users.UserName;
            deptName.InnerHtml = userMsg.GetDept().DeptName;
            //try
            //{
            //    WeatherWebService weather = new WeatherWebService();
            //    string[] array = weather.getWeatherbyCityName("济南");
            //    sp_first.InnerHtml = "<img src=\"../Images/" + array[8] + "\" alt=\"\"/>今天:" + array[6].Split(' ')[1] + " " + array[5];
            //    sp_next.InnerHtml = "<img src=\"../Images/" + array[15] + "\" alt=\"\"/>明天:" + array[13].Split(' ')[1] + " " + array[12];
            //    sp_last.InnerHtml = "<img src=\"../Images/" + array[21] + "\" alt=\"\"/>后天:" + array[18].Split(' ')[1] + " " + array[17];
            //}
            //catch { }
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        bool boControlPoint = new ConfigBLL().GetValueByKey("ISControlPoint").Equals("1");
        if (!boControlPoint)
        {
            return;
        }
        if (Session["userCode"] == null)
        {
            return;
        }
        string usercode = Session["userCode"].ToString();
        if (new Bll.OnlineBLL().IsExit(usercode))
        {
            new Bll.OnlineBLL().UpLastTime(usercode);
        }
    }
}
