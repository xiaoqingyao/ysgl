using Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_ysgl_ystz_qt_dz : BasePage
{
	private sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

	private DataTable dtuserRightDept = new DataTable();

	private string strNowDeptCode = "";

	private string strNowDeptName = "";

	private bool boYstzNeedAudit = !new ConfigBLL().GetValueByKey("YstzNeedAudit").Equals("0");

	protected void Page_Load(object sender, EventArgs e)
    {
		if (this.Session["userCode"] == null || this.Session["userCode"].ToString().Trim() == "")
		{
			base.ClientScript.RegisterStartupScript(base.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
			return;
		}
		string usercode = this.Session["userCode"].ToString().Trim();
		string valueByKey = new ConfigBLL().GetValueByKey("deptjc");
		if (!this.isTopDept("y", usercode) && valueByKey == "Y")
		{
			this.strNowDeptCode = this.server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + this.Session["userCode"].ToString().Trim() + "')");
			this.strNowDeptName = this.server.GetCellValue("select deptName from bill_departments where deptcode=(select userDept from bill_users where userCode='" + this.Session["userCode"].ToString().Trim() + "')");
		}
		else
		{
			this.strNowDeptCode = this.server.GetCellValue("select deptcode from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + this.Session["userCode"].ToString().Trim() + "'))");
			this.strNowDeptName = this.server.GetCellValue("select deptName from bill_departments where deptcode = (select  sjdeptcode from bill_departments where deptcode=(select userdept from bill_users where usercode='" + this.Session["userCode"].ToString().Trim() + "'))");
		}
		string userRightDepartments = new Departments().GetUserRightDepartments(this.Session["userCode"].ToString().Trim(), "", "0");
		if (!string.IsNullOrEmpty(valueByKey) && valueByKey == "Y")
		{
			this.dtuserRightDept = this.server.GetDataTable(string.Concat(new string[]
			{
				"select deptCode,deptName from bill_departments where  deptCode in (",
				userRightDepartments,
				") and deptCode not in (",
				this.strNowDeptCode,
				") order by deptCode"
			}), null);
		}
		else
		{
			this.dtuserRightDept = this.server.GetDataTable(string.Concat(new string[]
			{
				"select deptCode,deptName from bill_departments where  sjdeptCode='000001' and deptCode in (",
				userRightDepartments,
				") and deptCode not in (",
				this.strNowDeptCode,
				") order by deptCode "
			}), null);
		}
		if (!base.IsPostBack)
		{
			if (!this.strNowDeptCode.Equals("") && userRightDepartments != "")
			{
				if (this.dtuserRightDept.Rows.Count > 0)
				{
					for (int i = 0; i < this.dtuserRightDept.Rows.Count; i++)
					{
						ListItem listItem = new ListItem();
						listItem.Text = "[" + this.dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + this.dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
						listItem.Value = this.dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
						this.LaDept.Items.Add(listItem);
					}
				}
				this.LaDept.Items.Insert(0, new ListItem("--全部--", ""));
				this.LaDept.Items.Insert(0, new ListItem("[" + this.strNowDeptCode + "]" + this.strNowDeptName, this.strNowDeptCode));
				this.LaDept.SelectedIndex = 0;
			}
			this.BindDataGrid();
		}
		this.hd_YstzNeedAudit.Value = (this.boYstzNeedAudit ? "1" : "0");
	}
	protected void LaDept_SelectedIndexChanged(object sender, EventArgs e)
	{
		this.BindDataGrid();
	}

	private void BindDataGrid()
	{
		int wdHeight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
		int[] array = base.ComputeRow(this.ucPager.CurrentPageIndex, wdHeight, 90);
		int pageSize = array[2];
		int num = 0;
		DataTable data = this.GetData(array[0], array[1], out num);
		this.ucPager.PageSize = pageSize;
		this.ucPager.RecordCount = ((num == 0) ? 1 : num);
		this.myGrid.DataSource = data;
		this.myGrid.DataBind();
	}

	private DataTable GetData(int pagefrm, int pageto, out int count)
	{
		string text = this.LaDept.Text.Trim();
		string text2 = "";
		if (!string.IsNullOrEmpty(text))
		{
			text2 = text2 + " and  billDept='" + text + "'";
		}
		if (!string.IsNullOrEmpty(this.ddlstatus.SelectedValue))
		{
			text2 = text2 + " and stepid='" + this.ddlstatus.SelectedValue + "'";
		}
		string arg = "select billJe ,billName2,(select deptname from bill_departments where deptcode=billdept) as billDept,stepid,billCode\r\n                ,(select username from bill_users where usercode=billuser) as billUser,billdate \r\n                ,( select xmmc from bill_ysgc \twhere convert(char(10),kssj,121)<=convert(char(10),billdate,121) and  convert(char(10),jzsj,121)>=convert(char(10),billdate,121) and ystype!='0') as gcmc \r\n                ,(select top 1   mind from workflowrecords where recordid=(select top 1 recordid from workflowrecord where billCode=bill_main.billCode) and rdstate='3') as mind\r\n            ,(select dicname from bill_dataDic where dictype='18' and diccode=isnull(bill_main.dydj,'02')) as dydjname\r\n            ,Row_Number()over( order by billDate desc) as crow from bill_main where flowID='ystz' and billname='其他预算调整单' " + text2;
		string text3 = "select count(*) from ( {0} ) t";
		text3 = string.Format(text3, arg);
		count = int.Parse(this.server.GetCellValue(text3));
		string text4 = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
		text4 = string.Format(text4, arg, pagefrm, pageto);
		return this.server.GetDataTable(text4, null);
	}

	protected void UcfarPager1_PageChanged(object sender, EventArgs e)
	{
		this.BindDataGrid();
	}

	public bool isTopDept(string strus, string usercode)
	{
		string sqlQueryString;
		if (strus == "y")
		{
			sqlQueryString = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
		}
		else
		{
			sqlQueryString = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
		}
		return this.server.GetCellValue(sqlQueryString) == "1";
	}

	protected void Button6_Click(object sender, EventArgs e)
	{
		this.BindDataGrid();
	}

	protected void btn_Export_Click(object sender, EventArgs e)
	{
		int num = 0;
		DataTable data = this.GetData(0, 99999999, out num);
		for (int i = 0; i < data.Rows.Count; i++)
		{
			string a = data.Rows[i]["stepid"].ToString();
			string billcode = data.Rows[i]["billCode"].ToString();
			WorkFlowRecordManager workFlowRecordManager = new WorkFlowRecordManager();
			if (a == "end")
			{
				data.Rows[i]["stepid"] = "审批通过";
			}
			else
			{
				string value = workFlowRecordManager.WFState(billcode);
				data.Rows[i]["stepid"] = value;
			}
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("billCode", "单据编号");
		dictionary.Add("gcmc", "目标预算过程");
		dictionary.Add("billDept", "填报单位");
		dictionary.Add("billUser", "制单人");
		dictionary.Add("billDate", "单据日期");
		dictionary.Add("billJe", "调整金额");
		dictionary.Add("stepid", "审批状态");
		dictionary.Add("billName2", "摘要");
		dictionary.Add("dydjname", "调整类型");
		dictionary.Add("mind", "驳回理由");
		new ExcelHelper().ExpExcel(data, "ExportFile", dictionary);
	}

	protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
	{
		if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
		{
			string text = e.Item.Cells[6].Text;
			if (text == "end")
			{
				e.Item.Cells[6].Text = "审批通过";
				return;
			}
			string text2 = e.Item.Cells[0].Text;
			WorkFlowRecordManager workFlowRecordManager = new WorkFlowRecordManager();
			string text3 = workFlowRecordManager.WFState(text2);
			e.Item.Cells[6].Text = text3;
		}
	}

	protected void ddlstatus_SelectedIndexChanged(object sender, EventArgs e)
	{
		this.BindDataGrid();
	}

}