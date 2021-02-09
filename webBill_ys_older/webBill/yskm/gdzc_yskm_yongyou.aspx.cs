using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class webBill_yskm_gdzc_yskm_yongyou : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        ClientScript.RegisterArrayDeclaration("availableTagsDept", GetDeoptAll());

        if (!IsPostBack)
        {
            bindData();
            hfDept.Value = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
            this.ddlCk.DataSource = server.GetDataTable("select * from V_WareHouse", null);
            this.ddlCk.DataTextField = "cwhname";
            this.ddlCk.DataValueField = "cwhcode";
            this.ddlCk.DataBind();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string dept = txtDept.Text.Trim();
        string Type = nType.SelectedValue;
        string ckCode = this.ddlCk.SelectedValue.Trim();
        string ckName = this.ddlCk.SelectedItem.Text.Trim();
        string Yskm = txtYskm.Text.Trim();
        if (string.IsNullOrEmpty(Type) || string.IsNullOrEmpty(ckCode) || string.IsNullOrEmpty(ckName) || string.IsNullOrEmpty(Yskm) || string.IsNullOrEmpty(dept))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('必填项不能为空');", true);
            return;
        }

        string deptCode = CutVal(dept);
        string deptName = GetName(dept);
        string yskmCode = CutVal(Yskm);
        string yskmName = GetName(Yskm);


        if (IsAdded(ckCode, deptCode, yskmCode))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('部门" + deptCode + "仓库" + ckCode + "和本系统预算科目编号" + yskmCode + "已做过对应关系');", true);
            return;
        }
        int row = server.ExecuteNonQuery("insert into gdzc_yskm_yongyou (ntype,deptCode,deptName,ckCode,ckName,yskmcode,yskmname) values ('" + Type + "','" + deptCode + "','" + deptName + "','" + ckCode + "','" + ckName + "','" + yskmCode + "','" + yskmName + "') ", null);
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

    private bool IsAdded(string deptcode, string ckCode, string ysCode)
    {

        string sql = "select count(*) from gdzc_yskm_yongyou where deptcode='" + deptcode + "' and yskmcode='" + ysCode + "' and ckCode='" + ckCode + "' ";
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
        if (string.IsNullOrEmpty(nType.SelectedValue))
        {
            sqlList.Add("delete from gdzc_yskm_yongyou ");
        }
        else
        {
            sqlList.Add("delete from gdzc_yskm_yongyou where ntype='" + this.nType.SelectedValue + "'");

        }

        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string type = myGrid.Items[i].Cells[0].Text.Trim();
            string strDept = myGrid.Items[i].Cells[2].Text.Trim();
            string strCk = myGrid.Items[i].Cells[3].Text.Trim();
            TextBox tb = myGrid.Items[i].Cells[4].FindControl("txtYskm") as TextBox;
            string strKm = tb.Text.Trim();

            string deptCode = CutVal(strDept);
            string deptName = GetName(strDept);
            string ckCode = CutVal(strCk);
            string ckName = GetName(strCk);
            string yskmCode = CutVal(strKm);
            string yskmName = GetName(strKm);

            if (tempList.Contains(deptCode + ckCode + yskmCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败,第" + (i + 1) + "行已做过对应');", true);
                return;
            }
            tempList.Add(deptCode + ckCode + yskmCode);
            sqlList.Add("insert into gdzc_yskm_yongyou (ntype,deptCode,deptName,ckCode,ckName,yskmcode,yskmname) values ('" + type + "','" + deptCode + "','" + deptName + "','" + ckCode + "','" + ckName + "','" + yskmCode + "','" + yskmName + "') ");
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
        string sql = " select ntype,(case ntype when '1' then '卫材、药品' when '2' then '固定资产' else '' end ) as typeName,'['+ckcode+']'+ckname as  ckCode,'['+deptCode+']'+deptName  as deptCode, case '['+yskmcode+']'+yskmname  when '[]' then '' else '['+yskmcode+']'+yskmname end  as yskmcode  from	 gdzc_yskm_yongyou where 1=1 ";

        string temp = nType.SelectedValue;
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and ntype='" + temp + "'";
        }

        if (CheckBox1.Checked)
        {
            sql += " and isnull(yskmcode,'')='' ";
        }
        DataTable dt = server.GetDataTable(sql, null);
        myGrid.DataSource = dt;
        myGrid.DataBind();
    }



    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }

    private string GetDeoptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptname as dept  from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["dept"]));
            arry.Append("',");
        }
        if (arry.Length > 1)
        {
            string script = arry.ToString().Substring(0, arry.Length - 1);
            return script;
        }
        else
        {
            return "";
        }



    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        bindData();
    }
}
