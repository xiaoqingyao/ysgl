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
using Bll.UserProperty;
using System.Text;
using Bll;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class webBill_cwgl_Ybbxgf : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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


                hd_isedit.Value = ConfigurationManager.AppSettings["GfjeEditable"].ToString().Trim();
                object objgfafterpingzheng = ConfigurationManager.AppSettings["GfAfterPingZheng"];
                if (objgfafterpingzheng != null && !string.IsNullOrEmpty(objgfafterpingzheng.ToString()) && objgfafterpingzheng.Equals("1"))
                {
                    this.hd_GfAfterPingzheng.Value = "1";
                }
                this.BindDataGrid();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetUserAll());
            ClientScript.RegisterArrayDeclaration("availableTags2", GetDeptAll());
        }
    }

    void BindDataGrid()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 100);
        //获取pagesize 每页的高度
        int ipagesize = arrpage[2];
        //总的符合条件的记录数
        int icount = 0;
        //----------通过后台方法计算  正规的后台方法应该参照gldrp   应该传入（rownumfrm,rownumto,strSqlWhere,list<sqlparameter>,out iallcount）返回值是List<model>

        #region

        string stemp1, stemp2;
        List<SqlParameter> listSp = new List<SqlParameter>();
        string strCondition = "";
        stemp1 = this.find_txt_djcode.Text.ToString().Trim();
        if (stemp1 != "")
        {
            strCondition += " and (bill_main.billName like @billName )";
            listSp.Add(new SqlParameter("@billName", "%" + stemp1 + "%"));
        }

        stemp1 = this.find_txt_user.Text.ToString().Trim();
        if (stemp1 != "")
        {
            stemp1 = (new PublicServiceBLL()).SubSting(stemp1);
            strCondition += " and (bill_main.billuser =@billuser)";
            listSp.Add(new SqlParameter("@billuser", stemp1));
        }

        stemp1 = this.find_txt_dept.Text.ToString().Trim();
        if (stemp1 != "")
        {
            stemp1 = (new PublicServiceBLL()).SubSting(stemp1);
            strCondition += " and (bill_main.billDept = @billDept)";
            listSp.Add(new SqlParameter("@billDept", stemp1));
        }

        string strMoney1 = this.find_txt_money1.Text.ToString().Trim();
        decimal deMoney1 = 0;
        if (decimal.TryParse(strMoney1, out deMoney1))
        {
            strCondition += " and (bill_main.billje >= @begje) ";
            listSp.Add(new SqlParameter("@begje", deMoney1));
        }
        string strMoney2 = this.find_txt_money2.Text.ToString().Trim();
        decimal deMoney2 = 0;
        if (decimal.TryParse(strMoney2, out deMoney2))
        {
            strCondition += " and (bill_main.billje <=@endje) ";
            listSp.Add(new SqlParameter("@endje", deMoney2));
        }

        stemp1 = this.find_txt_time1.Text.ToString().Trim();
        stemp2 = this.find_txt_time2.Text.ToString().Trim();
        if (stemp1 != "")
        {
            strCondition += " and (convert(varchar(10),bill_main.billdate,121) >=@begtime)";
            listSp.Add(new SqlParameter("@begtime", stemp1));
        }
        if (stemp2 != "")
        {
            strCondition += "and (convert(varchar(10),bill_main.billdate,121) <= @endtime)";
            listSp.Add(new SqlParameter("@endtime", stemp2));

        }
        string strgfstatus = isGF.SelectedValue.Trim();
        if (strgfstatus != null && !strgfstatus.Equals(""))
        {
            strCondition += " and  isnull(bill_ybbxmxb.sfgf,'1')=@sfgf";
            listSp.Add(new SqlParameter("@sfgf", strgfstatus));
        }

        //根据webconfig 读取 是否控制凭证
        object objgfafterpingzheng = ConfigurationManager.AppSettings["GfAfterPingZheng"];
        if (objgfafterpingzheng != null && !string.IsNullOrEmpty(objgfafterpingzheng.ToString()) && objgfafterpingzheng.Equals("1"))
        {
            strCondition += " and isnull(bill_ybbxmxb.pzcode,'')!='' ";
        }



        #endregion
        icount = getcount();
        DataTable dtrel = GetData(arrpage[0], arrpage[1], listSp, strCondition);
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

    private int getcount()
    {
        int count = 0;

        string strsqlcount = @"select count(*)
from bill_main,bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode ";

        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        strsqlcount += "  and billDept in (" + deptCodes + ") and flowID in('srd','ybbx','zcgzbx','chly','wlfk') and stepID='end' and isnull(bill_ybbxmxb.guazhang,'0')!='1'";//and isnull(bill_ybbxmxb.pzcode,'')!='' and isnull(bill_ybbxmxb.pzdate,'')!=''


        List<SqlParameter> listSp = new List<SqlParameter>();
        #region 查询条件
        string stemp1, stemp2;

        stemp1 = this.find_txt_djcode.Text.ToString().Trim();
        if (stemp1 != "")
        {
            strsqlcount += " and (bill_main.billName like @billName )";
            listSp.Add(new SqlParameter("@billName", "%" + stemp1 + "%"));
        }

        stemp1 = this.find_txt_user.Text.ToString().Trim();
        if (stemp1 != "")
        {
            stemp1 = (new PublicServiceBLL()).SubSting(stemp1);
            strsqlcount += " and (bill_main.billuser =@billuser)";
            listSp.Add(new SqlParameter("@billuser", stemp1));
        }

        stemp1 = txt_khh.Text.Trim();
        if (!string.IsNullOrEmpty(stemp1))
        {
            strsqlcount += " and substring(bxrzh,0,charindex('&',bxrzh)-1) like @khh";
            listSp.Add(new SqlParameter("@khh","%"+stemp1+"%"));
        }


        stemp1 = txt_zh.Text.Trim();
        if (!string.IsNullOrEmpty(stemp1))
        {
            strsqlcount += " and substring(bxrzh,charindex('&',bxrzh)+2,len(bxrzh)) like @zh";
            listSp.Add(new SqlParameter("@zh", "%" + stemp1 + "%"));
        }

        stemp1 = this.find_txt_dept.Text.ToString().Trim();
        if (stemp1 != "")
        {
            stemp1 = (new PublicServiceBLL()).SubSting(stemp1);
            strsqlcount += " and (bill_main.billDept = @billDept)";
            listSp.Add(new SqlParameter("@billDept", stemp1));
        }

        string strMoney1 = this.find_txt_money1.Text.ToString().Trim();
        decimal deMoney1 = 0;
        if (decimal.TryParse(strMoney1, out deMoney1))
        {
            strsqlcount += " and (bill_main.billje >= @begje) ";
            listSp.Add(new SqlParameter("@begje", deMoney1));
        }
        string strMoney2 = this.find_txt_money2.Text.ToString().Trim();
        decimal deMoney2 = 0;
        if (decimal.TryParse(strMoney2, out deMoney2))
        {
            strsqlcount += " and (bill_main.billje <=@endje) ";
            listSp.Add(new SqlParameter("@endje", deMoney2));
        }

        stemp1 = this.find_txt_time1.Text.ToString().Trim();
        stemp2 = this.find_txt_time2.Text.ToString().Trim();
        if (stemp1 != "")
        {
            strsqlcount += " and (convert(varchar(10),bill_main.billdate,121) >=@begtime)";
            listSp.Add(new SqlParameter("@begtime", stemp1));
        }
        if (stemp2 != "")
        {
            strsqlcount += "and (convert(varchar(10),bill_main.billdate,121) <= @endtime)";
            listSp.Add(new SqlParameter("@endtime", stemp2));

        }
        string strgfstatus = isGF.SelectedValue.Trim();
        if (strgfstatus != null && !strgfstatus.Equals(""))
        {
            strsqlcount += " and  isnull(bill_ybbxmxb.sfgf,'1')=@sfgf";
            listSp.Add(new SqlParameter("@sfgf", strgfstatus));
        }

        //根据webconfig 读取 是否控制凭证
        object objgfafterpingzheng = ConfigurationManager.AppSettings["GfAfterPingZheng"];
        if (objgfafterpingzheng != null && !string.IsNullOrEmpty(objgfafterpingzheng.ToString()) && objgfafterpingzheng.Equals("1"))
        {
            strsqlcount += " and isnull(bill_ybbxmxb.pzcode,'')!='' ";
        }


        #endregion

        return count = int.Parse(server.GetCellValue(strsqlcount, listSp.ToArray()));

    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="pagefrm"></param>
    /// <param name="pageto"></param>
    /// <param name="paramter"></param>
    /// <param name="strCondition"></param>
    /// <returns></returns>
    private DataTable GetData(int pagefrm, int pageto, List<SqlParameter> paramter, string strCondition)
    {
        // by a.billDate and isnull(sfdk,'0')='0'
        string sql = @"select Row_Number()over(order by billdate desc) as crow,stepid,bxrzh,
substring(bxrzh,charindex('&',bxrzh)+2,len(bxrzh)) as zh,
substring(bxrzh,0,charindex('&',bxrzh)-1) as khh
,(select deptName from bill_departments where deptCode=billDept) as billDept,bill_main.billCode,billName,flowID,
(select username from bill_users where usercode=billuser) as billUser,billdate,billje,pzcode,CONVERT(varchar(10), pzdate, 121 )as pzdate,sfgf
from bill_main,bill_ybbxmxb where bill_ybbxmxb.billCode=bill_main.billCode  and  sfdk is null 
 ";
        string deptCodes = (new Departments()).GetUserRightDepartments(Session["userCode"].ToString().Trim(), "");
        sql += "  and billDept in (" + deptCodes + ") and flowID in('srd','ybbx','zcgzbx','chly','wlfk')  and stepID='end' and isnull(bill_ybbxmxb.guazhang,'0')!='1'";//and isnull(bill_ybbxmxb.pzcode,'')!='' and isnull(bill_ybbxmxb.pzdate,'')!=''

        sql += strCondition;
        string strsqlframe = "select * from ({0}) t where t.crow>{1} and t.crow<={2} order by billdate desc";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, paramter.ToArray());
    }


    protected void Button4_Click(object sender, EventArgs e)
    {
        this.BindDataGrid();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string billCode = hd_billCode.Value;

        string sql = "update bill_ybbxmxb set sfgf='1',gfr='" + Session["userCode"].ToString().Trim() + "',gfsj=getdate() where billCode='" + billCode + "'";
        if (server.ExecuteNonQuery(sql) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('给付失败！');", true);
        }
        else
        {
            this.BindDataGrid();
        }

    }

    //mxl
    protected void btn_summit_Click(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(this.GetType(), "", "alert('Fuck！');", true);

        this.BindDataGrid();
    }

    //mxl
    private string GetUserAll()
    {
        DataSet ds = server.GetDataSet("select '['+usercode+']'+username as username from bill_users");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["username"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    private string GetDeptAll()
    {
        DataSet ds = server.GetDataSet("select '['+deptcode+']'+deptName as deptName from bill_departments");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["deptName"]));
            arry.Append("',");
        }
        string script = arry.ToString().Substring(0, arry.Length - 1);

        return script;

    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            string strdjlx = e.Item.Cells[10].Text;
            if (!string.IsNullOrEmpty(strdjlx))
            {
                if (strdjlx == "srd")
                {
                    e.Item.Cells[10].Text = "收入报告单";
                }
                if (strdjlx == "zcgzbx")
                {
                    e.Item.Cells[10].Text = "资产购置报销单";
                }
                if (strdjlx == "chly")
                {
                    e.Item.Cells[10].Text = "存货领用单";
                }
                if (strdjlx == "wlfk")
                {
                    e.Item.Cells[10].Text = "往来付款单";

                }
                if (strdjlx == "ybbx")
                {
                    e.Item.Cells[10].Text = "一般报销单";
                }

            }
            string zt = e.Item.Cells[6].Text;
            if (zt == "end")
            {
                e.Item.Cells[6].Text = "审批通过";
            }
            string sfgf = e.Item.Cells[9].Text;
            if (sfgf.Equals("1"))
            {
                e.Item.Cells[9].Text = "已给付";
            }
            else
            {
                e.Item.Cells[9].Text = "未给付";
            }
           
        }
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }


}