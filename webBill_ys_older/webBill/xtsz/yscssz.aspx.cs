using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Models;
using System.Text.RegularExpressions;

public partial class webBill_xtsz_yscssz : System.Web.UI.Page
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
                Bind(true);
            }
        }

    }

    private void Bind(bool ispock)
    {
        string SysConfig = " select * from dbo.bill_SysConfig  ";
        DataTable SysConfigDt = server.GetDataTable(SysConfig, null);
        if (ispock)
        {
            //删除所有项
            for (int l = drpNd.Items.Count - 1; l > -1; l--)
            {
                drpNd.Items.Remove(drpNd.Items[l]);
            }

            DataRow[] drpNws = SysConfigDt.Select("ConfigName='ystbfs'", " nd desc ");
            DataRow[] TopNd = SysConfigDt.Select("", " nd desc ");
            if (TopNd.Count() > 0)
            {
                string NewYear = (Convert.ToInt32(TopNd[0]["nd"]) + 1).ToString();
                drpNd.Items.Add(new ListItem("[未设置]" + NewYear, NewYear));
            }
            else
            {
                return;
            }
            for (int s = 0; s < drpNws.Count(); s++)
            {
                drpNd.Items.Add(new ListItem(drpNws[s]["nd"].ToString(), drpNws[s]["nd"].ToString()));
            }
            drpNd.SelectedValue = TopNd[0]["nd"].ToString();
        }
        DataRow[] YearBudget = SysConfigDt.Select("ConfigName='YearBudget' and nd='" + drpNd.SelectedValue + "' ");
        DataRow[] ystbfs = SysConfigDt.Select("ConfigName='ystbfs' and nd='" + drpNd.SelectedValue + "' ");
        DataRow[] ndStatus = SysConfigDt.Select("ConfigName='ndStatus' and nd='" + drpNd.SelectedValue + "' ");

        if (YearBudget.Count() > 0)
        {
            if (YearBudget[0]["ConfigValue"].ToString() == "1")//是否启动年度预算
            {

                CheckYear.Checked = true;
                DataRow[] MonthOrQuarter = SysConfigDt.Select("ConfigName='MonthOrQuarter'  and nd='" + drpNd.SelectedValue + "'  ");
                if (MonthOrQuarter.Count() > 0)
                {
                    if (MonthOrQuarter[0]["ConfigValue"].ToString() == "2")//判断是月度预算还是季度预算 1是季度 2是月度
                    {
                        CheckSeason.Checked = false;
                        CheckMonth.Checked = true;
                    }
                    if (MonthOrQuarter[0]["ConfigValue"].ToString() == "1")
                    {
                        CheckMonth.Checked = false;
                        CheckSeason.Checked = true;
                    }
                }
            }
        }
        if (ystbfs.Count() > 0)
        {
            if (ystbfs[0]["ConfigValue"].ToString() == "0")
            {
                Checkbmsj.Checked = true;
                Checkfztb.Checked = false;
            }
            else
            {
                Checkfztb.Checked = true;
                Checkbmsj.Checked = false;
            }
        }
        if (YearBudget[0]["ConfigValue"].ToString() == "1")
        {
            GridView1.DataSource = GetListByMonthorSeons(CheckSeason.Checked ? "Season" : "Month");
            GridView1.DataBind();
        }

        if (ndStatus.Count() > 0 && ndStatus[0]["ConfigValue"].ToString() == "0")
        {

            rdbClose.Checked = true;
            rdbOn.Checked = false;

        }
        else
        {
            rdbClose.Checked = false;
            rdbOn.Checked = true;
        }
    }
    protected void CheckYear_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckYear.Checked)
        {
            GridView1.DataSource = GetListByMonthorSeons(CheckSeason.Checked ? "Season" : "Month"); ;
            GridView1.DataBind();
            CheckMonth.Enabled = true;
            CheckSeason.Enabled = true;
        }
        else
        {
            CheckMonth.Checked = false;
            CheckSeason.Checked = false;
            CheckMonth.Enabled = false;
            CheckSeason.Enabled = false;
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }
    protected void CheckSeason_CheckedChanged(object sender, EventArgs e)
    {
        GridView1.DataSource = GetListByMonthorSeons(CheckSeason.Checked ? "Season" : "Month");
        GridView1.DataBind();
    }
    protected void CheckMonth_CheckedChanged(object sender, EventArgs e)
    {
        GridView1.DataSource = GetListByMonthorSeons(CheckSeason.Checked ? "Season" : "Month"); ;
        GridView1.DataBind();
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        //1.
        string strcnnd = drpNd.SelectedValue;
        string isYear = CheckYear.Checked ? "1" : "0";
        string isMonth = CheckSeason.Checked ? "1" : "2";
        string tbfs = Checkbmsj.Checked ? "0" : "1";
        //年度开关设置，editby zyl2015-01-06
        string ndStatus = rdbClose.Checked ? "0" : "1";
        list.Add("delete from bill_Cnpz where year_CN='" + strcnnd + "' ");
        //当选择预算分解填报时
        //隐藏利润表项目分解、利润表部门分解、部门分解金额确认
        string strsql = "";
        int row = 0;
        if (Checkbmsj.Checked)
        {
            strsql = @"update bill_sysMenu set menustate='D' where menuid in('0207','0208','0209')";
            row = server.ExecuteNonQuery(strsql);
        }
        else
        {
            strsql = @"update bill_sysMenu set menustate='' where menuid in('0207','0208','0209')";
            row = row = server.ExecuteNonQuery(strsql);
        }
        if (drpNd.SelectedItem.Text.Substring(0, 4) == "[未设置")
        {
            if (new Bll.ConfigBLL().AddYscs(isYear, isMonth, tbfs, drpNd.SelectedValue, ndStatus))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
                Bind(true);
            }
        }
        else
        {
            IList<bill_syspar> parList = new List<bill_syspar>();
            for (int s = 0; s < GridView1.Rows.Count; s++)
            {
                bill_syspar par = new bill_syspar();
                bill_syspar pars = new bill_syspar();
                TextBox txtsj = GridView1.Rows[s].FindControl("txtks") as TextBox;
                TextBox txtjs = GridView1.Rows[s].FindControl("txtjs") as TextBox;
               
                par.parname = GridView1.Rows[s].Cells[0].Text;
                par.parVal = txtsj.Text;
                pars.parname = GridView1.Rows[s].Cells[2].Text;
                pars.parVal = txtjs.Text;
                parList.Add(par);
                parList.Add(pars);

                string strcnny = "";
                if (s < 10)
                {
                    strcnny = strcnnd + "-0" + s.ToString();
                }
                else
                {
                    strcnny = strcnnd + "-"+s.ToString();
                }
                if (s > 0)
                {
                    list.Add("insert into bill_Cnpz (beg_time,end_time,year_moth,year_CN) values ('" + txtsj.Text + "','" + txtjs.Text + "','" + strcnny + "','" + strcnnd + "')");

                }


            }
            IList<Bill_SysMenu> menulist = new List<Bill_SysMenu>();
            //menulist.Add(CreateMenu("0304", "1"));//添加关闭菜单示例  第一个参数为菜单ID  第二个参数为状态
            //menulist.Add(CreateMenu("0302", "1"));
            //menulist.Add(CreateMenu("0303", "1"));
            //menulist.Add(CreateMenu("0305", "1"));

            //isYear 是否年度 isMonth 月度还是季度 tbfs填报方式  parList设置日期
            if (new Bll.ConfigBLL().SetYscs(isYear, isMonth, tbfs, parList, menulist, drpNd.SelectedValue, ndStatus))
            {
                if (server.ExecuteNonQuerysArray(list) == -1)//执行list中的sql
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
                }

                Bind(true);
            }
        }
    }
    public Bill_SysMenu CreateMenu(string menuid, string menustate)
    {
        Bill_SysMenu menu = new Bill_SysMenu();
        menu.MenuId = menuid;
        menu.MenuState = menustate;
        return menu;
    }
    public IList<yssj> GetListByMonthorSeons(string im)
    {
        string sysparSql = "select * from dbo.bill_syspar where 1=1  ";
        DataTable parTable = server.GetDataTable(sysparSql, null);
        IList<yssj> yssjlist = new List<yssj>();
        DataRow[] ndksrow = parTable.Select("  parname like '%年度预算开始%' ");
        DataRow[] ksrow = parTable.Select("  parname like '%月份预算开始%' ", " parname  ");
        DataRow[] jdksrow = parTable.Select("  parname like '%季度预算开始%' ");
        DataRow[] ndjsrow = parTable.Select("  parname like '%年度预算截止%' ");
        DataRow[] jsrow = parTable.Select("  parname like '%月份预算截止%' ", " parname  ");
        DataRow[] jdjsrow = parTable.Select("  parname like '%季度预算截止%' ");
        for (int s = 0; s < ndjsrow.Count(); s++)
        {
            yssj ys = new yssj();
            ys.kssj = ndksrow[s]["parval"].ToString();
            ys.ksmc = ndksrow[s]["parname"].ToString();
            ys.jssj = ndjsrow[s]["parval"].ToString();
            ys.jsmc = ndjsrow[s]["parname"].ToString();
            ys.xh = Convert.ToInt32(GetNumber(ys.ksmc));
            yssjlist.Add(ys);
        }
        if (im == "Month")
        {
            for (int i = 0; i < ksrow.Count(); i++)
            {
                yssj ys = new yssj();
                ys.kssj = ksrow[i]["parval"].ToString();
                ys.ksmc = ksrow[i]["parname"].ToString();
                ys.jssj = jsrow[i]["parval"].ToString();
                ys.jsmc = jsrow[i]["parname"].ToString();
                ys.xh = Convert.ToInt32(GetNumber(ys.ksmc));
                yssjlist.Add(ys);
            }
        }
        if (im == "Season")
        {
            for (int k = 0; k < jdksrow.Count(); k++)
            {
                yssj ys = new yssj();
                ys.kssj = jdksrow[k]["parval"].ToString();
                ys.ksmc = jdksrow[k]["parname"].ToString();
                ys.jssj = jdjsrow[k]["parval"].ToString();
                ys.jsmc = jdjsrow[k]["parname"].ToString();
                ys.xh = Convert.ToInt32(GetNumber(ys.ksmc));
                yssjlist.Add(ys);
            }
        }
        if (im == "Season")
        {

            var temp = from p in yssjlist
                       orderby GetNumber(p.ksmc.Substring(0, 2))
                       select p;
            IList<yssj> rl = new List<yssj>();
            foreach (var i in temp)
            {
                rl.Add(i);
            }
            return rl;
        }
        else
        {
            var temp = from p in yssjlist
                       orderby p.xh
                       select p;
            IList<yssj> rl = new List<yssj>();
            foreach (var i in temp)
            {
                rl.Add(i);
            }
            return rl;
        }
    }
    public string GetNumber(string str)
    {
        string result = "0";
        if (str != null && str != string.Empty)
        {
            // 正则表达式剔除非数字字符（不包含小数点.） 
            str = Regex.Replace(str, @"[^\d.\d]", "");
            // 如果是数字，则转换为decimal类型 
            if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                result = str;
            }
            else
            {
                result = "0";
            }
        }
        else
        {
            return "0";
        }
        return Convert.ToDecimal(result == "" ? "0" : result) < 10 ? "0" + result : result;
    }
    protected void drpNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpNd.SelectedItem.Text.Substring(0, 4) == "[未设置")
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            CheckYear.Checked = false;
            CheckSeason.Checked = false;
            Checkfztb.Checked = false;
            CheckMonth.Checked = false;
            Checkbmsj.Checked = false;
        }
        else
        {

            Bind(false);
        }
    }
}
public class yssj
{
    public string ksmc { get; set; }
    public string kssj { get; set; }
    public string jsmc { get; set; }
    public string jssj { get; set; }
    public int xh { get; set; }
}
