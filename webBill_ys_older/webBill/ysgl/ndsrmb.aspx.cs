using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysgl_ndsrmb : BasePage
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
                string selectndsql = "select nian,xmmc from bill_ysgc where   yue='' order by nian desc";
                DataTable selectdt = server.GetDataTable(selectndsql, null);

                drpSelectNd.DataSource = selectdt;
                drpSelectNd.DataTextField = "xmmc";
                drpSelectNd.DataValueField = "nian";

                drpSelectNd.DataBind();
                this.drpSelectNd.Items.Insert(0, new ListItem("--全部--", ""));

                this.BindDataGrid();
            }
        }
    }
    public void BindDataGrid()
    {
        string strsql = "select * from bill_Srmb where note0='sr'  ";
        if (!string.IsNullOrEmpty(drpSelectNd.SelectedValue))
        {

            string strjesql = "select zje from bill_Srmb where nd='" + drpSelectNd.SelectedValue + "' and note0='sr'";
            string strzje = server.GetCellValue(strjesql);
            if (!string.IsNullOrEmpty(strzje))
            {
                txtje.Text = strzje;
            }
            else
            {
                txtje.Text = "";
            }

            strsql += " and nd='" + drpSelectNd.SelectedValue + "'";

        }
        strsql += " order by nd desc";
        DataTable dt = server.GetDataTable(strsql, null);
        this.myGrid.DataSource = dt;
        this.myGrid.DataBind();
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string strnd = drpSelectNd.SelectedValue;
        //if (!string.IsNullOrEmpty(strnd))
        //{
        //    string strsql = "select zje from bill_Srmb where nd='" + strnd + "'";
        //    string strzje = server.GetCellValue(strsql);
        //    if (!string.IsNullOrEmpty(strzje))
        //    {
        //        txtje.Text = strzje;
        //    }
        //}

        BindDataGrid();
    }



    /// <summary>
    /// 保存设置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_sz_Click(object sender, EventArgs e)
    {
        List<string> list = new List<string>();
        string strnd = drpSelectNd.SelectedValue;
        string strzje = txtje.Text;
        if (!string.IsNullOrEmpty(strnd) && !string.IsNullOrEmpty(strzje))
        {
            list.Add("delete bill_Srmb where nd='" + strnd + "' and note0='sr'");
            list.Add("   insert bill_Srmb (nd,zje,note0) values('" + strnd + "','" + strzje + "','sr') ");

        }
        if (string.IsNullOrEmpty(strnd))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择设置年度！');", true);
        }
        if (list.Count > 0)
        {
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
                this.BindDataGrid();
            }
        }

       
    }
}