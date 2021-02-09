using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_shouru_sr_kmdy_dz : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }


        if (!IsPostBack)
        {
            bindData();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Type = ddlType.SelectedValue;

        string OutName = txtOutName.Text.Trim();
        string Yskm = txtYskm.Text.Trim();
        if (string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(OutName) || string.IsNullOrEmpty(Yskm))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('必填项不能为空');", true);
            return;
        }

        string yskmCode = CutVal(Yskm);
        string yskmName = GetName(Yskm);



        int row = server.ExecuteNonQuery(@"insert into sr_kmdy_dz (atype,outname,yskmcode,yskmname)
                                values ('" + Type + "','" + OutName + "','" + yskmCode + "','" + yskmName + "') ", null);
        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功');", true);
            bindData();
            txtYskm.Text = "";


        }
        else
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败');", true);
            return;
        }



    }

    private bool IsAdded(string outCode, string ysCode)
    {

        string sql = "select count(*) from sr_kmdy_dz where yskmcode='" + ysCode + "' ";
        int row = Convert.ToInt32(server.GetCellValue(sql));

        if (row > 0)
            return true;
        else
            return false;

    }

    protected void btnSaveAll_Click(object sender, EventArgs e)
    {
        IList<string> sqlList = new List<string>();
        List<string> tempList = new List<string>();
        //string ddlstratype = ddlType.SelectedValue;
        //if (true)
        //{

        //}


        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string type = myGrid.Items[i].Cells[0].Text.Trim();
            string strOut = myGrid.Items[i].Cells[1].Text.Trim();
            string stratype = myGrid.Items[i].Cells[3].Text.Trim();
            TextBox tb = myGrid.Items[i].Cells[2].FindControl("txtYskm") as TextBox;
            string strKm = tb.Text.Trim();

            string outName = strOut;
            string yskmCode = CutVal(strKm);
            string yskmName = GetName(strKm);
            if (!string.IsNullOrEmpty(yskmCode))
            {
                if (tempList.Contains(outName + yskmCode))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,第" + (i + 1) + "行已做过对应');", true);
                    return;
                }
            }


            tempList.Add(outName + yskmCode);
            sqlList.Add("delete from sr_kmdy_dz where atype='" + stratype + "' and outname='" + outName + "'");
            sqlList.Add(@"insert into sr_kmdy_dz (atype,outname,yskmcode,yskmname) values ('" + stratype + "','" + outName + "','" + yskmCode + "','" + yskmName + "') ");
        }
        int row = server.ExecuteNonQuerysArray(sqlList.ToList());

        if (row > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功');", true);
            bindData();

        }
        else
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败');", true);
            return;
        }
    }
    private void bindData()
    {
        string sql = @"select atype,outname 
          ,case '['+yskmcode+']'+yskmname  when '[]' then '' else '['+yskmcode+']'+yskmname end  as yskmcode  
            from	 sr_kmdy_dz where 1=1  ";

        string temp = ddlType.SelectedValue;
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and atype='" + temp + "'";
        }
        string strzt = ddlzt.SelectedValue;
        if (strzt == "已设置")
        {
            sql += " and yskmcode!='' ";
        }
        else if (strzt == "未设置")
        {
            sql += "  and (yskmcode='' or yskmcode is null)";
        }
        DataTable dt = server.GetDataTable(sql, null);
        myGrid.DataSource = dt;
        myGrid.DataBind();
    }
    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
}