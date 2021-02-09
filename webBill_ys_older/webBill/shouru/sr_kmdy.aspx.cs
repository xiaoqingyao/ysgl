using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class webBill_shouru_sr_kmdy : BasePage
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
            hfDept.Value = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Type = ddlType.SelectedValue;
        string OutCode = txtOutCode.Text.Trim();
        string OutName = txtOutName.Text.Trim();
        string Yskm = txtYskm.Text.Trim();
        if (string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(OutCode) || string.IsNullOrEmpty(OutName) || string.IsNullOrEmpty(Yskm))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('必填项不能为空');", true);
            return;
        }

        string yskmCode = CutVal(Yskm);
        string yskmName = GetName(Yskm);


        if (IsAdded(OutCode, yskmCode))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('外部编号" + OutCode + "和本系统预算科目编号" + yskmCode + "已做过对应关系');", true);
            return;
        }
        int row = server.ExecuteNonQuery("insert into sr_kmdy (dytype,outcode,outname,yskmcode,yskmname) values ('" + Type + "','" + OutCode + "','" + OutName + "','" + yskmCode + "','" + yskmName + "') ", null);
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

        string sql = "select count(*) from sr_kmdy where outcode='" + outCode + "' and yskmcode='" + ysCode + "' ";
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
        sqlList.Add("delete from sr_kmdy where dytype='" + this.ddlType.SelectedValue + "'");
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string type = myGrid.Items[i].Cells[0].Text.Trim();
            string strOut = myGrid.Items[i].Cells[2].Text.Trim();
            TextBox tb = myGrid.Items[i].Cells[3].FindControl("txtYskm") as TextBox;
            string strKm = tb.Text.Trim();
            string outCode = CutVal(strOut);
            string outName = GetName(strOut);
            string yskmCode = CutVal(strKm);
            string yskmName = GetName(strKm);

            if (tempList.Contains(outCode + yskmCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,第" + (i + 1) + "行已做过对应');", true);
                return;
            }
            tempList.Add(outCode + yskmCode);
            sqlList.Add("insert into sr_kmdy (dytype,outcode,outname,yskmcode,yskmname) values ('" + type + "','" + outCode + "','" + outName + "','" + yskmCode + "','" + yskmName + "') ");
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
        string sql = "select dytype,(case dytype when '502' then '预交金' when '503' then '出院结算' when  '504' then' 住院收入'  when   '505' then '门诊收入'  when   '506' then '门诊挂号收费' else '' end ) as dytypeName,'['+outcode+']'+outname as  outcode,  case '['+yskmcode+']'+yskmname  when '[]' then '' else '['+yskmcode+']'+yskmname end  as yskmcode  from	 sr_kmdy where 1=1  ";

        string temp = ddlType.SelectedValue;
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and dytype='" + temp + "'";
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
