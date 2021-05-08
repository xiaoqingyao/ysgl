using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_xtsz_Handover : BasePage
{
	private sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
	protected void Page_Load(object sender, EventArgs e)
    {
		base.ClientScript.RegisterArrayDeclaration("avaiusertb", this.GetUsersAll());
	}

    protected void sure_Click(object sender, EventArgs e)
    {
		string text = this.txtFrom.Text.Trim();
		string text2 = this.txtTo.Text.Trim();
		text = text.Substring(1, text.IndexOf("]") - 1);
		text2 = text2.Substring(1, text2.IndexOf("]") - 1);
		DataTable dataTable = this.server.GetDataTable(string.Concat(new string[]
		{
			"select usercode from bill_users where usercode like '%",
			text,
			"%' and usercode!='",
			text,
			"'"
		}), null);
		if (dataTable.Rows.Count > 1)
		{
			foreach (DataRow dataRow in dataTable.Rows)
			{
				string str = dataRow[0].ToString();
				object obj = this.server.ExecuteScalar("select count(*) from workflowstep where checkcode like '%" + str + "%'");
				if (int.Parse(obj.ToString()) > 0)
				{
					base.showMessage("全公司人员与交接人员有名字覆盖情况，如（张三丰与张三），所以不能用该功能，只能手动去改审批流。", false, "");
					return;
				}
			}
		}
		DataTable dataTable2 = this.server.GetDataTable(string.Concat(new string[]
		{
			"select usercode from bill_users where usercode like '%",
			text2,
			"%' and usercode!='",
			text2,
			"'"
		}), null);
		if (dataTable2.Rows.Count > 1)
		{
			foreach (DataRow dataRow2 in dataTable2.Rows)
			{
				string str2 = dataRow2[0].ToString();
				object obj2 = this.server.ExecuteScalar("select count(*) from workflowstep where checkcode like '%" + str2 + "%'");
				if (int.Parse(obj2.ToString()) > 0)
				{
					base.showMessage("全公司人员与被交接人员有名字覆盖情况，如（张三丰与张三），所以不能用该功能，只能手动去改审批流。", false, "");
					return;
				}
			}
		}
		string sqlQueryString = string.Concat(new string[]
		{
			"update [dbo].[workflowstep] set checkcode =  replace(checkcode,'",
			text,
			"','",
			text2,
			"')  where checkcode like '%",
			text,
			"%' "
		});
		this.server.ExecuteNonQuery(sqlQueryString);
		base.showMessage("处理完毕！", false, "");
	}
	private string GetUsersAll()
	{
		DataSet dataSet = this.server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
		StringBuilder stringBuilder = new StringBuilder();
		foreach (DataRow dataRow in dataSet.Tables[0].Rows)
		{
			stringBuilder.Append("'");
			stringBuilder.Append(Convert.ToString(dataRow["usercodename"]));
			stringBuilder.Append("',");
		}
		if (stringBuilder.Length > 1)
		{
			return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
		}
		return "";
	}
}