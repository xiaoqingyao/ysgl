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

public partial class webBill_makebxd_gongzi_xiangmuduiying : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //}
        //else
        //{
        if (!IsPostBack)
        {
            this.BindDataGrid();
        }
        //}
    }

    private void BindDataGrid()
    {
        string sql = "select * from bill_yskm where yskmCode like '01%' and yskmCode not in ('01')";
        DataSet temp = server.GetDataSet(sql);
        myGrid.DataSource = temp;
        myGrid.DataBind();

    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            System.Web.UI.HtmlControls.HtmlInputButton btnSelect = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSetMx");
            string kmcode = Convert.ToString(e.Item.Cells[0].Text).Trim();
            HtmlInputText tb = e.Item.FindControl("txt_mingxi") as HtmlInputText;

            if (!string.IsNullOrEmpty(kmcode))
            {
                string strSql = "select dyName from  bill_gzxmdy where yskmCode='" + kmcode + "'";
                DataTable dt = server.GetDataTable(strSql, null);
                string strtb = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strtb += Convert.ToString(dt.Rows[i]["dyName"]) + ",";
                }
                if (!string.IsNullOrEmpty(strtb) && strtb.Length > 1)
                {
                    tb.Value = strtb.Substring(0, strtb.Length - 1);
                }

            }
            if (btnSelect == null)
            { }
            else
            {
                string temp = e.Item.Cells[0].Text;
                hf_yskmCode.Value = temp;
                btnSelect.Attributes.Add("onclick", "openSelect('" + temp + "',this);");

            }
        }
    }
}