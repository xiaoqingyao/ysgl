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

public partial class webBill_sysMenu_MenuDetails : System.Web.UI.Page
{
    string strID = "";
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        if (Request["id"]!=null)
        {
            strID = Request["id"].ToString();
        }
        if (!IsPostBack)
        {
            initControl();
        }
    }

    private void initControl()
    {
        if (strID!="")
        {
            string strSql = "select menuName,showName,menuOrder from bill_sysMenu where menuid='" + strID + "'";
            DataTable dt = server.GetDataSet(strSql).Tables[0];
            this.lbeName.Text = dt.Rows[0]["menuName"].ToString();
            this.txtMyName.Text = dt.Rows[0]["showName"].ToString();
            this.txtMenuOrder.Text = dt.Rows[0]["menuOrder"].ToString();
        }
    }


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        if (strID.Equals(""))
        {
            return;
        }
        string strMyName = this.txtMyName.Text.Trim();
        string strmenuOrder = this.txtMenuOrder.Text.Trim();
        string strUpSql = "update  bill_sysMenu set showName='" + strMyName + "',menuOrder='" + strmenuOrder + "' where menuid='" + strID + "'";
        int iRel = server.ExecuteNonQuery(strUpSql);
        if (iRel > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');window.returnValue=\"sucess\";self.close();", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');window.returnValue=\"failed\";self.close();", true);
        }
    }
}
