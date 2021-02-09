using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class webBill_sjzd_sjzdList : BasePage
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string dicType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objtype = Request["dicType"];
        if (objtype != null)
        {
            dicType = objtype.ToString();
        }
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                DataDicList();
            }
        }
    }

    protected void DataDicList()
    {

        //获取窗口的高度    获取的方法是前台的jquery获取后赋值给隐藏域，这个地方获取到的高度应该是回发的高度，第一次获取到的高度是通过mainnew页面get方法传过来的参数 在basepage中处理的
        int ipagesheight = int.Parse(this.hdwindowheight.Value.Equals("") ? "400" : this.hdwindowheight.Value);
        //ComputeRow是basepage提供的 计算出来的三个参数 分别是 rownumfrm  rownumto pagesize
        int[] arrpage = ComputeRow(ucPager.CurrentPageIndex, ipagesheight, 70);
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
        string str_sql = "select Row_Number()over(order by dicCode) as crow , dictype,diccode,dicname,(case isnull(cjys,'1') when '1' then '是' when 0 then '否' else cjys end) as cjys,(case isnull(cys,'1') when '1' then '是' else '否' end) as cys ,(case isnull(cdj,'0') when '0' then '否' when '1' then '是'  end) as cdj,(case isnull(isSys,'0') when '0' then '否' when '1' then '是' end) as isSys from bill_dataDic where 1=1 ";

        str_sql += " and dictype='" + dicType + "'";
        this.Label1.Text = server.GetCellValue("select dicName from bill_dataDic where dicCode='" + dicType + "' and dicType='00'");

        //查询条件
        if (txb_where.Text.Trim() != "")
        {
            str_sql += " and dicname like '%" + txb_where.Text.Trim() + "%'";
        }
        string strsqlcount = "select count(*) from ( {0} ) t";
        strsqlcount = string.Format(strsqlcount, str_sql);
        count = int.Parse(server.GetCellValue(strsqlcount));

        string strsqlframe = "select * from ( {0} ) t where t.crow>{1} and t.crow<={2}";
        strsqlframe = string.Format(strsqlframe, str_sql, pagefrm, pageto);
        return server.GetDataTable(strsqlframe, null);
    }



    protected void UcfarPager1_PageChanged(object sender, EventArgs e)
    {
        DataDicList();
    }


    #region 修改
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        string strisSys = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                diccode = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                strisSys = this.myGrid.Items[i].Cells[7].Text.ToString().Trim();
                count += 1;
            }
        }

        if (count > 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('您选择了多条数据！');", true);
        }
        else if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择待修改的数据！');", true);
        }
        else
        {
            if (strisSys == "是")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('系统设置项不允许修改。')", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sjzdDetail02.aspx?type=edit&dictype=" + dicType + "&diccode=" + diccode + "');", true);

            }
        }
    }
    #endregion

    #region 删除
    protected void btn_del_Click(object sender, EventArgs e)
    {
        string diccode = "";
        int count = 0;
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked == true)
            {
                diccode += "'" + this.myGrid.Items[i].Cells[2].Text.ToString().Trim() + "',";
                count += 1;
            }
        }
        if (count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择要删除的数据！');", true);
        }
        else
        {
            diccode = diccode.Substring(0, diccode.Length - 1);
            if (Page.Request.QueryString["dictype"].ToString().Trim() == "01")
            {
                DataSet temp = server.GetDataSet("select jkdjlx from bill_fysq where jkdjlx in (" + diccode + ")");
                if (temp.Tables[0].Rows.Count == 0)
                { }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('待删除的数据字典正在使用中！');", true);
                    return;
                }
            }
            else if (Page.Request.QueryString["dictype"].ToString().Trim() == "02")
            {
                DataSet temp = server.GetDataSet("select bxmxlx from bill_ybbxmxb where bxmxlx in (" + diccode + ")");
                if (temp.Tables[0].Rows.Count == 0)
                { }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('待删除的数据字典正在使用中！');", true);
                    return;
                }
            }
            System.Collections.Generic.List<string> list = new List<string>();

            list.Add("delete from bill_dataDic where diccode in (" + diccode + ") and dictype='02'");
            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('删除成功！');", true);
                this.DataDicList();
            }
        }

    }
    #endregion

    #region 查询
    protected void btn_sel_Click(object sender, EventArgs e)
    {
        DataDicList();
    }
    #endregion

    protected void btn_add_Click(object sender, EventArgs e)
    {
        if (dicType.Equals(""))
        {
            showMessage("参数丢失，请刷新后重试！", false, ""); return;
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", "openDetail('sjzdDetail02.aspx?type=add&dictype=" + dicType + "&diccode=');", true);
    }

    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e == null)
        {
            return;
        }
        bool boHasBudgetControl = new Bll.ConfigBLL().GetModuleDisabled("HasBudgetControl");
        //如果配置为不启用预算管理 则禁用冲减预算和控制预算的控制项目 edit by lvcc
        if (dicType.Equals("02"))
        {
            if (!boHasBudgetControl)
            {
                e.Item.Cells[4].Style.Add("display", "none");
                e.Item.Cells[5].Style.Add("display", "none");
            }
        }
        else if (dicType.Equals("10"))
        {
            e.Item.Cells[5].Style.Add("display", "none");
            if (e.Item.ItemType == ListItemType.Header)
            {
                e.Item.Cells[4].Text = "默认获取编号";
                e.Item.Cells[6].Text = "是否默认";
            }
        }
        else { }
    }
    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    /// <param name="isExit">提示后是否退出</param>
    /// <param name="strReturnVal">返回值</param>
    private void showMessage(string strMsg, bool isExit, string strReturnVal)
    {
        string strScript = "alert('" + strMsg + "');";
        if (!strReturnVal.Equals(""))
        {
            strScript += "window.returnValue=\"" + strReturnVal + "\";";
        }
        if (isExit)
        {
            strScript += "self.close();";
        }
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }
}
