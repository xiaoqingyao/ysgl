using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysgl_fyysbmfj : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
    string strNowDeptCode = "";
    string strNowDeptName = "";
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

                string selectndsql = "select distinct  nian from  dbo.bill_ysgc order by nian desc";
                DataTable selectdt = server.GetDataTable(selectndsql, null);
                drpSelectNd.DataSource = selectdt;
                drpSelectNd.DataTextField = "nian";
                drpSelectNd.DataValueField = "nian";
                drpSelectNd.DataBind();
                this.BindDataGrid();
            }
        }
    }
    public bool isTopDept(string strus, string usercode)
    {
        string sql = "";
        if (strus == "y")
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode=(select userdept from bill_users where usercode='" + usercode + "')";
        }
        else
        {
            sql = " select count(1) from (select deptCode from bill_departments where sjDeptCode=(select deptCode from bill_departments where isnull(sjDeptCode,'')='') or isnull(sjDeptCode,'')='')a where deptCode='" + usercode + "'";
        }
        if (server.GetCellValue(sql) == "1")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void BindDataGrid()
    {
        ////获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        //int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        ////ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        //int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
        ////获取pagesize 每页的高度
        //int ipagesize = arrpage[2];
        ////总的符合条件的记录数
        //int icount = 0;
        ////----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>
        DataTable dtrel = GetData();
        ////给分页控件赋值 告诉分页控件 当前页显示的行数
        //this.ucPager.PageSize = ipagesize;
        ////告诉分页控件 所有的记录数
        //this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();
    }
    private DataTable GetData()
    {
        if (!string.IsNullOrEmpty(drpSelectNd.SelectedValue) && drpSelectNd.SelectedValue != "0")
        {


            string strjesql = "select zje from bill_Srmb where nd='" + drpSelectNd.SelectedValue + "' and note0='fy' ";
            string strzje = server.GetCellValue(strjesql);
            decimal deczje = 0;
            if (!string.IsNullOrEmpty(strzje))
            {
                deczje = decimal.Parse(strzje);
                lb_zje.Text = "总费用金额为：" + deczje.ToString("N2");
            }

            string strsql = "select * from bill_deptfj where nd='" + drpSelectNd.SelectedValue + "'";

            DataTable dt = server.GetDataTable(strsql, null);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                string sql = "exec  [bill_pro_ndysfjbm]'" + drpSelectNd.SelectedValue + "'";
                return server.GetDataTable(sql, null);
            }




            //DataSet temp = server.GetDataSet(sql);

            //string strsqlcount = "select count(*) from ( {0} ) t";
            //strsqlcount = string.Format(strsqlcount, sql);
            //count = int.Parse(server.GetCellValue(strsqlcount));

            //string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2} ";
            //strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);

        }
        else
        {
            return null;
        }
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }
    /// <summary>
    /// 保存    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string strnd = drpSelectNd.SelectedValue;
        List<string> list = new List<string>();
        if (!string.IsNullOrEmpty(strnd))
        {
            list.Add(" delete bill_deptfj where nd='" + strnd + "'");
        }
        if (myGrid.Items.Count > 0)
        {
            for (int i = 0; i < myGrid.Items.Count; i++)
            {
                string strdeptcode = myGrid.Items[i].Cells[0].Text;
                string strdeptname = myGrid.Items[i].Cells[1].Text;
                string strbl = myGrid.Items[i].Cells[2].Text;
                TextBox txt_je = myGrid.Items[i].Cells[3].FindControl("txt_fjje") as TextBox;
                string strje = txt_je.Text;// myGrid.Items[i].Cells[3].Text;
                strje = strje.Replace(",", "");
                list.Add(" insert into bill_deptfj (nd,deptcode,deptname,bl,xjje) values('" + strnd + "','" + strdeptcode + "','" + strdeptname + "','" + strbl + "','" + strje + "')");
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('没有要保存的数据！');", true);
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
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }

    }
    decimal zje = 0;//总金额

    protected void myGrid_DataBinding(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item)
        {
            TextBox txt_je = e.Item.Cells[3].FindControl("txt_fjje") as TextBox;
            string strzje = txt_je.Text;//e.Item.Cells[3].Text;
            if (!string.IsNullOrEmpty(strzje))
            {
                zje += decimal.Parse(strzje);
            }
        }
        if (e.Item.ItemType == ListItemType.Footer)
        {
            e.Item.Cells[0].Text = "合计：";
            e.Item.Cells[3].Text = zje.ToString("N4");
            e.Item.Cells[3].CssClass = "myGridItemRight";
        }
    }
}
