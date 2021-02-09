using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_bill_bmsrysList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindData();
        }
    }
    void bindData()
    {

        string strsql = @"select deptCode,deptname,replicate('　　',len(deptCode)-2)+deptname as deptMc from bill_departments where deptcode!='000001'  order by deptCode";
        DataSet temp = server.GetDataSet(strsql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {

        List<string> list = new List<string>();
        decimal yszje = 0;
        string errorMsg = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            string deptcode = this.myGrid.Items[i].Cells[0].Text.Trim();
            string deptname = this.myGrid.Items[i].Cells[1].Text.Trim();
            TextBox txtysje = (TextBox)this.myGrid.Items[i].FindControl("TextBox2");
            TextBox txtjsje = (TextBox)this.myGrid.Items[i].FindControl("txtjsje");


            try
            {
                decimal ysje = decimal.Parse(txtysje.Text.ToString().Trim());

                if (txtysje.Text == "0.00")
                { }
                else
                {

                    yszje += ysje;

                    //写入预算明细
                    list.Add("delete [bill_bmsrys] where deptcode='" + deptcode + "' ");
                    list.Add("insert  into  [bill_bmsrys](deptcode,deptname,srysje,srjsje) values('" + deptcode + "','" + deptname + "','" + txtysje.Text + "','" + txtjsje.Text + "')");

                }
            }
            catch { }
        }
        if (!string.IsNullOrEmpty(errorMsg))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "aa", "alert('" + errorMsg + "');self.close();", true);
            return;
        }
        //else if (yszje == 0)
        //{
        //    Response.Write("<script>alert('总金额为0，保存失败！')</script>");
        //    return;
        //}

        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);//window.open('yszjList.aspx','_self');
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        // Response.Redirect("yszjFrame.aspx?deptcode=" + strdeptcode + "&page=yszj");
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            TextBox zjtext = e.Item.FindControl("TextBox2") as TextBox;
            TextBox txtjsje = e.Item.FindControl("txtjsje") as TextBox;
            string deptcode = Convert.ToString(e.Item.Cells[0].Text.Trim());

            string strysje = server.GetCellValue("select top 1 srysje  from [bill_bmsrys] where deptcode='" + deptcode + "'");
            string strjsje = server.GetCellValue("select top 1 srjsje  from [bill_bmsrys] where deptcode='" + deptcode + "'");
            if (!string.IsNullOrEmpty(strysje))
            {
                zjtext.Text = strysje;
            }
            if (!string.IsNullOrEmpty(strjsje))
            {
                txtjsje.Text = strjsje;
            }
            int count = Convert.ToInt32(server.GetCellValue(@" select (select count(*) from bill_departments where deptCode like b.deptCode+'%' and len(deptCode)>len(b.deptCode))as ss  from bill_departments b
	              where deptCode in( select deptCode from bill_departments) and deptcode!='000001' and deptcode='" + deptcode + "'"));
            if (count != 0)
            {
                zjtext.Enabled = false;
                txtjsje.Enabled = false;
            }

        }
    }
}