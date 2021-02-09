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

public partial class webBill_sysMenu_MenuList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            initControl();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    private void initControl()
    {
        string sql = @"select menuid,menuName,showName,menustate from bill_sysMenu where 1=1 and  (ISNULL(menustate, '') = '') ";
        string strWhere = this.txtWhere.Text.Trim();
        if (!string.IsNullOrEmpty(strWhere))
        {
            sql += " and (menuName like '%" + strWhere + "%' or menuid='" + strWhere + "' or showName like '%" + strWhere + "%')";
        }
        sql += "order by menuid ";
        DataTable dtRel = server.GetDataSet(sql).Tables[0];
        this.myGrid.DataSource = dtRel;
        this.myGrid.DataBind();
    }
    /// <summary>
    /// 查询
    /// </summary>
    protected void btn_sele_Click(object sender,EventArgs e) {
       initControl();
    }
}
