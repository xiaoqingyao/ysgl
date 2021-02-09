using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Bll.UserProperty;

public partial class webBill_DeskMessage_MyMessageList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PaginationPrepare();
            GridBind();
        }
    }


    private void PaginationPrepare()
    {
        DeskManager deskMgr = new DeskManager();
        string userCode = Convert.ToString(Session["userCode"]);
        int rows = deskMgr.GetReaderCount(userCode);
        PaginationToGV1.RowsCount = rows.ToString();
        if (rows > 0)
        {
            PaginationToGV1.PageIndex = "1";
        }
    }

    private void GridBind()
    {
        string userCode = Convert.ToString(Session["userCode"]);
        DeskManager deskMgr = new DeskManager();
        int pageIndex = 0;
        if (!string.IsNullOrEmpty(PaginationToGV1.PageIndex))
        {
            pageIndex = Convert.ToInt32(PaginationToGV1.PageIndex);
        }
        int[] pagerows = ComputeRow(pageIndex);
        GridView1.DataSource = deskMgr.GetMessageByReader(userCode, pagerows[0], pagerows[1]);
        GridView1.DataBind();
    }


    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        GridBind();
    }



    private int[] ComputeRow(int pageIndex)
    {
        int[] ret = new int[2];
        int pagRows = Convert.ToInt32(ConfigurationManager.AppSettings["ItemNumPerPage"]);
        ret[0] = (pageIndex - 1) * pagRows;
        ret[1] = pageIndex * pagRows;
        return ret;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header)
        {
            string userCode = Convert.ToString(Session["userCode"]);
            string billCode = e.Row.Cells[0].Text;
            DeskManager deskMgr = new DeskManager();
            e.Row.Cells[5].Text = deskMgr.GetMessageState(billCode, userCode);
        }
    }
    protected void btn_detail_Click(object sender, EventArgs e)
    {
        string code = hf_code.Value;
        Response.Redirect("NewsForLook.aspx?code=" + code);
    }
}
