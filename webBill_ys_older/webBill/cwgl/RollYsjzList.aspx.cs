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
using Bll.UserProperty;
using System.Data.SqlClient;

public partial class webBill_cwgl_RollYsjzList : System.Web.UI.Page
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
                BindData();

            }
        }
    }

    private void BindData()
    {
        string nian = Request["nian"];
        string ysgc = Request["ysgc"];
        YsManager ysmgr = new YsManager();
        string[] temp = ysmgr.GetYsYearMonth(ysgc);
        string yf = nian + "-" + temp[2];
        string usercode = Convert.ToString(Session["userCode"]);
        string guid = (new GuidHelper()).getNewGuid();
        SqlParameter[] sps = { 
                             new SqlParameter("@yf",yf),
                                 new SqlParameter("@guid",guid),
                                 new SqlParameter("@userCode",usercode),
                                 new SqlParameter ("@hsdo","0")
                             };
        DataSet ds = server.ExecuteProcedure("pro_yj_js", sps);
        myGrid.DataSource = ds;
        myGrid.DataBind();

    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        string nian = Request["nian"];
        string ysgc = Request["ysgc"];
        YsManager ysmgr = new YsManager();
        string[] temp = ysmgr.GetYsYearMonth(ysgc);
        string yf = nian + "-" + temp[2];
        string usercode = Convert.ToString(Session["userCode"]);
        string guid = (new GuidHelper()).getNewGuid();
        string scale = "1";
        if (!string.IsNullOrEmpty(txt_percentage.Text.Trim()))
        {
            scale = txt_percentage.Text.Trim();
        }
        
        SqlParameter[] sps = { 
                             new SqlParameter("@yf",yf),
                                 new SqlParameter("@guid",guid),
                                 new SqlParameter("@userCode",usercode),
                                 new SqlParameter("@percentage",scale)
                             };
      server.ExecuteProc("pro_yj_zd", sps);
      ClientScript.RegisterStartupScript(this.GetType(), "","alert('结转成功！');window.location.href='RollYsgcList.aspx';", true);

    }
}
