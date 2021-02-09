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
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Bll;

public partial class webBill_cwgl_ybbxcwms : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            if (!IsPostBack)
            {
                this.BindDataGrid();
            }
        }
    }
    void BindDataGrid()
    {
        //        //string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        //        ////string selfDeptCode = (new Departments()).GetZgDepartments(Session["userCode"].ToString().Trim());

        //        //string billCodes = (new workFlowLibrary.workFlow()).getRightStepBills("ybbx", Session["userGroup"].ToString().Trim(), Session["userCode"].ToString().Trim(), deptCodes);

        //        string sql = @"select * from( select (select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,
        //stepid as stepID_ID,billDept,stepid,billCode,(select xmmc from bill_ysgc where gcbh=billName) as billName,(select username from bill_users where usercode=billuser) as billUser,
        //billdate,billje from bill_main  where stepid ='-1' and flowID='ybbx' and billcode not in(select billcode from workflowrecord))t";


        //        /*
        //        if (this.TextBox1.Text.ToString().Trim() != "")
        //        {
        //            sql += " and (billCode like '%" + this.TextBox1.Text.ToString().Trim() + "%')";
        //        }
        //         */


        //        DataSet temp = server.GetDataSet(sql);

        //        if (temp.Tables[0].Rows.Count == 0)
        //        {
        //            temp = null;
        //        }
        //        this.myGrid.DataSource = temp;
        //        this.myGrid.DataBind();


        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 100);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>


        //查询条件
        List<SqlParameter> listSp = new List<SqlParameter>();
        string strCondition = "";


        icount = 0;
        DataTable dtrel = GetData(arrpage[0], arrpage[1], listSp, strCondition, out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount;
        //----------给gridview赋值

        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
        if (dtrel.Rows.Count == 0 || dtrel == null)
        {
            this.ucPager.Visible = false;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagefrm"></param>
    /// <param name="pageto"></param>
    /// <param name="paramter"></param>
    /// <param name="strCondition"></param>
    /// <returns></returns>
    private DataTable GetData(int pagefrm, int pageto, List<SqlParameter> paramter, string strCondition, out int count)
    {
        // by a.billDate and billcode not in(select billcode from workflowrecord)
        string sql = @"select  Row_Number()over(order by billdate desc) as crow,* from( select (select bxzy from bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode) as bxzy,
stepid as stepID_ID,billDept,stepid,billCode, billName,(select username from bill_users where usercode=billuser) as billUser,flowID,
billdate,billje from bill_main  where stepid ='-1' and flowID in('srd','ybbx','zcgzbx','chly','wlfk')  {0})t";
        //查询条件
        if (this.TextBox1.Text.ToString().Trim() != "")
        {
            strCondition += " and (billName like '%" + this.TextBox1.Text.ToString().Trim() + "%') ";
        }

        string strusercode = (new PublicServiceBLL()).SubSting(this.txtloannamecode.Text.Trim());
        if (!string.IsNullOrEmpty(strusercode))
        {
            strCondition += " and billuser='" + strusercode + "'";
        }

        string strdeptcode = (new PublicServiceBLL()).SubSting(this.txtLoanDeptCode.Text.Trim());
        if (!string.IsNullOrEmpty(strdeptcode))
        {
            strCondition += " and (billDept='" + strdeptcode + "' or substring(billdept,1,2)='" + strdeptcode + "')";
        }
        if (txtLoanDateFrm.Text.Trim() != "")
        {
            strCondition += " and billdate>='" + this.txtLoanDateFrm.Text.Trim() + "'";
        }
        if (this.txtLoanDateTo.Text.Trim() != "")
        {
            strCondition += " and billdate<='" + this.txtLoanDateTo.Text.Trim() + "'";
        }
        sql = string.Format(sql, strCondition);

        //获取条数
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        if (paramter != null)
        {
            count = int.Parse(server.GetCellValue(strsqlcount, paramter.ToArray()));
        }
        else
        {
            count = int.Parse(server.GetCellValue(strsqlcount));
        }

        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by billdate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);

        return server.GetDataTable(strsqlframe, paramter.ToArray());
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        string billCode = "";
        int count = 0;
        string shyj = "";
        string stepID_ID = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                billCode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                stepID_ID = this.myGrid.Items[i].Cells[8].Text.ToString().Trim();
                shyj = ((TextBox)this.myGrid.Items[i].FindControl("TextBox1")).Text.ToString().Trim();
                count += 1;
            }
        }

        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待审核的项！');", true);
        }
        else if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('每次只能审核一项！');", true);
        }
        else
        {



            server.ExecuteScalar("delete from workflowrecords where recordid =(select recordid from workflowrecord where billcode='" + billCode + "')");
            server.ExecuteScalar("delete from workflowrecord where billcode='" + billCode + "'");
            server.ExecuteScalar("update bill_main set stepid='end' where billcode='" + billCode + "'");

            this.BindDataGrid();
        }
    }
    protected void Button3_Click1(object sender, EventArgs e)
    {
        string billGuid = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                count += 1;
                billGuid = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
            }
        }
        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多个报销单！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您未选择待查看的报销单！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>openDetail('../bxgl/bxDetailNew.aspx?type=look&billCode=" + billGuid + "');</script>");
        }
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            e.Item.Cells[5].Text = (new billCoding()).getDeptLevel2Name(e.Item.Cells[5].Text.ToString().Trim());
            string strdjlx = e.Item.Cells[11].Text;
            if (!string.IsNullOrEmpty(strdjlx))
            {
                if (strdjlx == "srd")
                {
                    e.Item.Cells[11].Text = "收入报告单";
                }
                if (strdjlx == "zcgzbx")
                {
                    e.Item.Cells[11].Text = "资产购置报销单";
                }
                if (strdjlx == "chly")
                {
                    e.Item.Cells[11].Text = "存货领用单";
                }
                if (strdjlx == "wlfk")
                {
                    e.Item.Cells[11].Text = "往来付款单";

                }
                if (strdjlx == "ybbx")
                {
                    e.Item.Cells[11].Text = "一般报销单";
                }

            }
            if (e.Item.Cells[7].Text == "-1")
            {
                e.Item.Cells[7].Text = "未提交";
            }
        }
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
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
    private string GetUsersAll()
    {
        DataSet ds = server.GetDataSet("select '['+userCode+']'+userName as usercodename from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["usercodename"]));
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
    protected void Button2_Click(object sender, EventArgs e)
    {
        BindDataGrid();
    }
}
