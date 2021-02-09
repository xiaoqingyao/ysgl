using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_bxgl_ybbxSetYskm : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

    public DataSet dsHsxm;
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

    public void BindData()
    {
        dsHsxm = server.GetDataSet("select * from bill_ybbxmxb_hsxm where kmmxGuid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "'");

        string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
        dept = dept.Substring(1, dept.IndexOf("]") - 1);

        string sql = "select *,isnull((select je from bill_ybbxmxb_hsxm where kmmxGuid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "' and bill_ybbxmxb_hsxm.xmcode=bill_xm.xmcode),0) as je from bill_xm where  xmdept='" + dept + "' and isnull(xmStatus,'1')='1'";

        DataSet temp = server.GetDataSet(sql);

        this.myGrid.DataSource = temp;

        this.myGrid.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        //if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //{
        //    for (int i = 0; i <= dsHsxm.Tables[0].Rows.Count - 1; i++)
        //    {
        //        if (e.Item.Cells[2].Text.ToString().Trim() == dsHsxm.Tables[0].Rows[i]["xmCode"].ToString().Trim())
        //        {
        //            TextBox txt = (TextBox)e.Item.FindControl("TextBox2");
        //            txt.Text = double.Parse(dsHsxm.Tables[0].Rows[i]["je"].ToString().Trim()).ToString("0.00");
        //        }
        //    }   
        //}
    }
    protected void btn_dele_Click(object sender, EventArgs e)
    {
        string sql = "delete from bill_ybbxmxb_hsxm where kmmxGuid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "'";

        server.ExecuteNonQuery(sql);

        this.BindData();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //string sql = "delete from bill_ybbxmxb_hsxm where kmmxGuid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "'";
        //list.Add(sql);

        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            TextBox txt = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                //if (txt.Text.ToString().Trim() != "")
                //{
                try
                {
                    //double.Parse(txt.Text.ToString().Trim());
                    list.Add("insert into bill_ybbxmxb_hsxm select '" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "','" + (new GuidHelper()).getNewGuid() + "','" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "','0' from bill_xm where xmcode='" + this.myGrid.Items[i].Cells[1].Text.ToString().Trim() + "' and xmcode not in (select xmcode from bill_ybbxmxb_hsxm where kmmxGuid='" + Page.Request.QueryString["mxGuid"].ToString().Trim() + "')");
                }
                catch { }
                //}
            }
        }

        if (server.ExecuteNonQuerysArray(list)!=-1)
        {
            ClientScript.RegisterStartupScript(this.GetType(),"","alert('保存成功！');self.close();",true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.BindData();
    }
}
