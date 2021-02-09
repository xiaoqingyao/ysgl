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

public partial class webBill_ysgl_yskmtjlist : System.Web.UI.Page
{
    string strdeptcode = "";
    string strkmcode = "";

    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {

            if (Request["deptcode"] != null && Request["deptcode"].ToString() != "")
            {
                strdeptcode = Request["deptcode"].ToString();
            }
            if (Request["kmcode"] != null && Request["kmcode"].ToString() != "")
            {
                strkmcode = Request["kmcode"].ToString();
            }
            if (!IsPostBack)
            {
                this.bindData();
            }
        }
    }

    void bindData()
    {

        string deptGuid = @"select a.*,
(select xmmc from bill_ysgc where gcbh=a.gcbh) as yuefen,
(select '['+deptCode+']'+ deptName from bill_departments where deptCode=a.ysDept ) as deptName,
(select '['+yskmCode+']'+ ysKmMc from bill_yskm where yskmCode=a.yskm ) as yskmName
from bill_ysmxb a where ysType='1' and ysDept='" + strdeptcode + "' and yskm='" + strkmcode + "'";

        DataTable temp = server.RunQueryCmdToTable(deptGuid);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();
    }
    decimal deHj = 0;
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strEve = e.Item.Cells[4].Text.Trim();
            decimal deEve = 0;
            if (decimal.TryParse(strEve, out deEve))
            {
                deHj += deEve;
            }
        }
        else if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].Text = "合计：";
            e.Item.Cells[0].Style.Add("text-align", "right");
            e.Item.Cells[4].Text = deHj.ToString();
            e.Item.Cells[4].Style.Add("text-align", "right");
        }
    }
}
