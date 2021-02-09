using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using System.Configuration;

public partial class webBill_DeskMessage_NewsList : System.Web.UI.Page
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
        int rows = deskMgr.GetNewsCount();
        PaginationToGV1.RowsCount = rows.ToString();
        if (rows > 0)
        {
            PaginationToGV1.PageIndex = "1";
        }
    }

    private void GridBind()
    {
        DeskManager deskMgr = new DeskManager();
        int pageIndex = 0;
        if(!string.IsNullOrEmpty(PaginationToGV1.PageIndex))
        {
            pageIndex = Convert.ToInt32(PaginationToGV1.PageIndex);
        }
        int[] pagerows = ComputeRow(pageIndex);
        GridView1.DataSource = deskMgr.GetNews(pagerows[0], pagerows[1]);
        GridView1.DataBind();
    }

    protected void btn_insert_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewsDetails.aspx?type=news&action=add");
    }

    protected void PaginationToGV1_GvBind(object sender, EventArgs e)
    {
        GridBind();
    }

    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string code = hf_code.Value;
        Response.Redirect("NewsDetails.aspx?type=news&action=edit&code=" + code);
    }

    protected void btn_delete_Click(object sender, EventArgs e)
    {
        string code = hf_code.Value;
        DeskManager deskMgr = new DeskManager();
        deskMgr.Delete(code);
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功!')", true);
    }



    private int[] ComputeRow(int pageIndex)
    {
        int[] ret = new int[2];
        int pagRows = Convert.ToInt32(ConfigurationManager.AppSettings["ItemNumPerPage"]);
        ret[0] = (pageIndex - 1) * pagRows;
        ret[1] = pageIndex * pagRows;
        return ret;
    }
}
