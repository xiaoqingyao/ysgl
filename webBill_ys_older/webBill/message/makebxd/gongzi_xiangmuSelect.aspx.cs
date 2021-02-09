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
using System.Text;
using System.Data.SqlClient;

public partial class webBill_makebxd_gongzi_xiangmuSelect : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string nameArrStr = "岗位工资,薪级工资,护士10,护龄,住房补贴,基本工资,保健,独子,院外,生活护理费,奖励性绩效工资,基础性绩效工资,电话费补助,监察办案补贴,返聘费,人才津贴,补工资,房租,扣基资,公积金,失业金,养老保险,医疗保险,扣税,合养保险,合医保险";
    DataTable dt;
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

    /// <summary>
    /// 数据填充
    /// </summary>
    private void BindDataGrid()
    {
        dt = this.CreateDataTable();
        myGrid.DataSource = dt;
        myGrid.DataBind();
    }


    /// <summary>
    /// 自定义DataTable，并为之添加数据
    /// </summary>
    /// <returns></returns>
    private DataTable CreateDataTable()
    {
        dt = new DataTable();
        dt.Columns.Add("gzmxname", typeof(string));
        dt.Columns.Add("ischeck", typeof(string));
        string[] arr = nameArrStr.Split(',');
        for (int i = 0; i < arr.Length; i++)
        {
            dt.Rows.Add(new object[] { arr[i], "0" });
        }
        return dt;
    }

    /// <summary>
    /// 保存（将数据插入到对应关系表格）
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        list.Add("delete from bill_gzxmdy where yskmCode='" + Request.QueryString["yskmCode"].ToString() + "'");
        string dyname = "";
        string names = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
            if (cbox.Checked == true)
            {
                dyname = myGrid.Items[i].Cells[1].Text;
                names += dyname + ",";
                list.Add("insert into bill_gzxmdy(yskmCode,dyName) values('" + Request.QueryString["yskmCode"].ToString() + "','" + dyname + "')");
            }
        }
        if (names.Length - 1 > 0)
        {
            names = names.Substring(0, names.Length - 1);
        }

        if (server.ExecuteNonQuerysArray(list) == -1)
        {

            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存设置失败！');", true);
        }
        else
        {

            //ClientScript.RegisterStartupScript(this.GetType(), "doOpen", "SuccessOk();", true);

            ClientScript.RegisterStartupScript(this.GetType(), "aa", "closeParent('" + names + "');window.close();", true);
        }


        //System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //list.Add("delete from bill_dept_ywzg where deptCode='" + Page.Request.QueryString["deptCode"].ToString().Trim() + "'");
        //string userCode = "";
        //for (int i = 0; i < myGrid.Items.Count; i++)
        //{
        //    CheckBox cbox = (CheckBox)myGrid.Items[i].FindControl("CheckBox1");
        //    if (cbox.Checked == true)
        //    {
        //        userCode = myGrid.Items[i].Cells[1].Text;
        //        list.Add("insert into bill_dept_ywzg(deptCode,userCode) values('" + Page.Request.QueryString["deptCode"].ToString().Trim() + "','" + userCode + "')");
        //    }
        //}

        //if (server.ExecuteNonQuerysArray(list) == -1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存设置失败！');", true);
        //}
        //else
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存完成！');self.close();", true);
        //}
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {


        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string s = e.Item.Cells[1].Text;
            string yskmCode = "";
            if (!string.IsNullOrEmpty(Request.QueryString["yskmCode"]))
            {
                yskmCode = Request.QueryString["yskmCode"].ToString();
                string s2 = "select * from bill_gzxmdy where yskmCode='" + yskmCode + "' and  dyName='" + s + "'";
                DataTable temp = server.GetDataSet(s2).Tables[0]; ;
                if (temp.Rows.Count > 0)
                {
                    CheckBox cb = (CheckBox)e.Item.FindControl("CheckBox1");
                    cb.Checked = true;
                }
            }
        }
    }
}
