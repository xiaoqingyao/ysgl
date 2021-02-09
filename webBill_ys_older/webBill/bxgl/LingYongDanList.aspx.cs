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
using Bll.Bills;
using Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using Bll;
using System.Text;

public partial class webBill_bxgl_LingYongDanList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    bill_lydBll bllLyd = new bill_lydBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeoptAll());
            ClientScript.RegisterArrayDeclaration("avaiusertb", GetUsersAll());
            if (!IsPostBack)
            {

                this.bindData();
            }
        }
    }

    void bindData()
    {
        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 90);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>

        DataTable dtrel = GetData(arrpage[0], arrpage[1], out icount);
        //给分页控件赋值 告诉分页控件 当前页显示的行数
        this.ucPager.PageSize = ipagesize;
        //告诉分页控件 所有的记录数
        this.ucPager.RecordCount = icount == 0 ? 1 : icount;
        //----------给gridview赋值
        this.myGrid.DataSource = dtrel;
        this.myGrid.DataBind();

    }


    private DataTable GetData(int pagefrm, int pageto, out int count)
    {
        string sql = @"select guid,note1,note2,note3,note4,note5,note6,note7,note8,note9,note0,convert(varchar(10),lyDate,121) as lyDate,(select '['+userCode+']'+username from bill_users where usercode=lyr) lyr,(select '['+userCode+']'+username from bill_users where userCode=zdr) as zdr,(select '['+deptcode+']'+deptname from bill_departments where deptcode=lyDept) lyDept,je,sm,bz,zt,Row_Number()over(order by guid) as crow from bill_lyd";
        sql += GetParm();
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);

    }
    private string GetParm()
    {
        string temp = txtloannamecode.Text.Trim();//领用人
        StringBuilder sb = new StringBuilder();
        sb.Append("  where  1=1 ");
        if (!string.IsNullOrEmpty(temp))
        {
            sb.Append(" and lyr like  '%" + new PublicServiceBLL().SubSting(temp) + "%'");
        }
        temp = txtLoanDeptCode.Text.Trim();//领用部门
        if (!string.IsNullOrEmpty(temp))
        {
            sb.Append(" and lyDept like '%" + new PublicServiceBLL().SubSting(temp) + "%'");
          
        }
        temp = TextBox1.Text.Trim();//单据编号
        if (!string.IsNullOrEmpty(temp))
        {
            sb.Append(" and guid  like '%"+temp+"%'");  
        }
        temp = txtLoanDateFrm.Text.Trim();//领用时间
        if (!string.IsNullOrEmpty(temp))
        {
            sb.Append(" and  convert(varchar(10),lyDate,121)  >= '"+temp+"'");
        }
        temp = txtLoanDateTo.Text.Trim();
        if (!string.IsNullOrEmpty(temp))
        {
            sb.Append(" and  convert(varchar(10),lyDate,121) <= '"+temp+"'");
        }
        return sb.ToString();
    }

    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        bindData();
    }



    protected void Button4_Click(object sender, EventArgs e)
    {
        this.bindData();
    }


    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {



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

}
