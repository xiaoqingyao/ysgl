using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;

public partial class webBill_cwgl_Ysyj : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ddl_year.SelectedValue = DateTime.Now.Year.ToString();
            Bind();
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        QueryManger query = new QueryManger();

        string month = this.hd_billCode.Value.Trim();
            //ddl_year.SelectedValue + "-" + ddl_month.SelectedValue;
        string userCode = Session["userCode"].ToString();
        string guid = new GuidHelper().getNewGuid();

        string checkStr = query.CheckYj(month);
        if (checkStr.Length > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + checkStr + "')", true);
            return;
        }

        query.ProYj(month, userCode, guid);
        
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('月结成功!')", true);
        Bind();
    }

    private void Bind()
    {
        string strnf = this.ddl_year.SelectedValue;
        QueryManger query = new QueryManger();
        myGrid.DataSource = query.GetYsTable(strnf);
        myGrid.DataBind();
    }
    protected void btn_select_Click(object sender, EventArgs e)
    {
        Bind();
    }
    protected void btn_qx_Click(object sender, EventArgs e)
    {
        QueryManger query = new QueryManger();

        string month = this.hd_billCode.Value.Trim();

        string str= query.qxyj(month);

        if (str != "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + str + "')", true);
            return;
        }
        else
        {
            Bind();
        }
    }
}
