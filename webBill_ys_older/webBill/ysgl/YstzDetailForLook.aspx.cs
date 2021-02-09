using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;

public partial class webBill_ysgl_YstzDetailForLook : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindToGrid();
        }
    }

    private void DataBindToGrid()
    {
        string billCode = Request.Params["billCode"];
        YsManager ysMgr = new YsManager();
        IList<Bill_Ysmxb> list = ysMgr.GetYsmxByCode(billCode);
        GridView1.DataSource = list;
        GridView1.DataBind();
        string type = Request.Params["type"];
    }


    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            SysManager sysMgr = new SysManager();
            YsManager ysMgr = new YsManager();
            string tzje = e.Row.Cells[6].Text;
            if (Convert.ToDecimal(tzje) < 0)
            {
                e.Row.Cells[0].Text = "调整出";
            }
            else
            {
                e.Row.Cells[0].Text = "调整入";
            }

            string deptCode = e.Row.Cells[2].Text;
            e.Row.Cells[2].Text = sysMgr.GetDeptCodeName(deptCode);

            string gcbh = e.Row.Cells[1].Text;
            e.Row.Cells[1].Text = ysMgr.GetYsgcCodeName(gcbh);

            string ysKm = e.Row.Cells[3].Text;
            e.Row.Cells[3].Text = sysMgr.GetYskmNameCode(ysKm);

            e.Row.Cells[4].Text =Convert.ToString(ysMgr.GetYueYs(gcbh, deptCode, ysKm));
            e.Row.Cells[5].Text = Convert.ToString(Convert.ToDecimal(e.Row.Cells[4].Text) - ysMgr.GetYueHf(gcbh, deptCode, ysKm));
        }
    }
}
