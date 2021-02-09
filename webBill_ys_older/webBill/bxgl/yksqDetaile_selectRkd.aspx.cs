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

public partial class webBill_bxgl_yksqDetaile_selectRkd : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string type = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            txtLoanDateFrm.Value = DateTime.Now.ToString("yyyy-MM") + "-01";
            txtLoanDateTo.Value = DateTime.Now.ToString("yyyy-MM-dd");
            bindData();

        }
    }

    void bindData()
    {


        type = Request["type"];
        if (type == "m")
        {
            GetSetSelect();
        }
        else
            myGrid.Columns[1].HeaderText = "选择";

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 105);
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
        bool isSet;
        string[] dbName = new Bll.Bills.bill_yksqBll().GetDbNames(out isSet);
        if (!isSet)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请先配置用款申请配置项！');", true);
            count = 0;
            return null;
        }
        string sql = @" select Row_Number()over(order by a.ID desc) as crow, a.*,item.je  from " + dbName[0] + "  as a inner join (select ID,isnull(SUM(ipprice),0) as je from " + dbName[1] + "  group by ID) item  on a.ID=item.ID   and isnull(a.cDefine13,'')=''";

        string temp = txtLoanDateFrm.Value.Trim();
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and a.dDate>='" + temp + "'";
        }
        temp = this.txtLoanDateTo.Value.Trim();
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and a.dDate<='" + temp + "'";
        }

        temp = txtCode.Value.Trim();
        if (!string.IsNullOrEmpty(temp))
        {
            sql += " and a.ID  like '%" + temp + "%'";
        }


        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }

    protected void btn_SaveChoose_Click(object sender, EventArgs e)
    {
        decimal je = 0;
        string str = GetSetSelect();

        if (str == "未选择" || str.Length == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择！');", true);
            return;
        }
        je = GetSumJe(str);
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"" + str + "|" + je + "\";self.close();", true);
    }
    protected void btn_query_Click(object sender, EventArgs e)
    {
        bindData();
    }
    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        bindData();
    }

    private decimal GetSumJe(string codes)
    {
        string ret = "";
        string[] code = codes.Split(',');
        for (int i = 0; i < code.Length; i++)
        {
            ret += "" + code[i] + ",";
        }
        if (ret.Length <= 1)
        {
            return 0;
        }
        ret = ret.Substring(0, ret.Length - 1);
        bool isSet;
        string[] dbName = new Bll.Bills.bill_yksqBll().GetDbNames(out isSet);
        if (!isSet)
        {
            return 0;
        }
        string sql = @" select convert(decimal(18,2),isnull(sum(item.je),0))    from " + dbName[0] + "  as a inner join (select ID,SUM(ipprice) as je from " + dbName[1] + "  group by ID) item  on a.ID=item.ID   and a.ID in (" + ret + ")";
        return Convert.ToDecimal(server.GetCellValue(sql, null));
    }
    private string GetSetSelect()
    {

        string strGys = Label1.Text.Trim();
        
        strGys = strGys == "未选择" ? "" : strGys;
        string ret = "";
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            DataGridItem row = myGrid.Items[i];
            CheckBox cb = row.FindControl("ckDept") as CheckBox;
            if (cb.Checked)
            {
                string code = row.Cells[1].Text;
                if (strGys.IndexOf(code)==-1)
                {
                     ret += code + ",";
                }
               
            }
        }

        if (ret.Length > 1)
            ret = strGys.Length > 0 ? ret + strGys : ret.Substring(0, ret.Length - 1);
        else
            ret = strGys;
        Label1.Text = string.IsNullOrEmpty(ret) ? "未选择" : ret;
        txtje.Text = string.IsNullOrEmpty(ret) ? "" : GetSumJe(ret).ToString();
        return ret;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        this.Label1.Text =  txtje.Text="";
    }
}
