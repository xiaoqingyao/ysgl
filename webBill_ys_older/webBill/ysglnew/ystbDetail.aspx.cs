using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;

public partial class webBill_ysglnew_ystbDetail : System.Web.UI.Page
{
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        if (Request.QueryString["deptCode"] != null)
        {
            string deptcode = Request.QueryString["deptCode"].ToString();
            //Ladept.Text= server.GetCellValue("select deptname from bill_departments where deptcode='" + (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim()) + "'");
            if (Request.QueryString["billCode"] != null)
            {

                string billCode = Request.QueryString["billCode"].ToString();
                if (deptcode != "")
                {
                    try
                    {
                        string nd = billCode.Substring(1, 4);
                        if (nd != "")
                        {
                            IList<YsgcTb> ysMainTable = bll.GetMainTable(deptcode, nd, "", "02", new string[] { "1", "5" }, "", "");

                            if (ysMainTable == null)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('出错');", true);
                            }
                            else
                            {
                                GridView1.DataSource = ysMainTable;
                                GridView1.DataBind();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('" + ex + "');", true);
                    }
                }
            }
        }
    }
    protected void btn_Tb_Click(object sender, EventArgs e)
    {

        try
        {
            string nd = txtysgc.Text.Trim().Substring(1, 4);
            if (nd != "")
            {
                IList<YsgcTb> ysMainTable = bll.GetMainTable("", nd, "", "", new string[] { "1", "5" }, "", "");
                if (ysMainTable == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('找不到年份的预算过程！或者没有设置相对应的预算科目！');", true);
                }
                else
                {
                    IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
                    if (sysConfig["MonthOrQuarter"] == "1")
                    {
                        GridView1.Visible = false;
                        GridView2.Visible = true;
                        GridView2.DataSource = ysMainTable;
                        GridView2.DataBind();
                    }
                    else
                    {
                        GridView2.Visible = false;
                        GridView1.Visible = true;
                        GridView1.DataSource = ysMainTable;
                        GridView1.DataBind();
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('程序出错！ 请确认预算参数是否正确！必要联系管理员');", true);
        }

    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
        if (deptCode != null)
        {
            string nd = txtysgc.Text.Substring(1, 4);
            IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(nd);
            IList<YsgcTb> TbMain = new List<YsgcTb>();

            if (sysConfig["MonthOrQuarter"] == "1")
            {
                for (int k = 0; k < GridView2.Rows.Count; k++)
                {
                    string hiddkmbh = (GridView2.Rows[k].FindControl("HiddenKmbh") as HiddenField).Value;
                    string spring = (GridView2.Rows[k].FindControl("txtspring") as TextBox).Text.Trim();
                    string summer = (GridView2.Rows[k].FindControl("txtsummer") as TextBox).Text.Trim();
                    string autumn = (GridView2.Rows[k].FindControl("txtautumn") as TextBox).Text.Trim();
                    string winter = (GridView2.Rows[k].FindControl("txtwinter") as TextBox).Text.Trim();
                    string year = (GridView2.Rows[k].FindControl("txtyear") as TextBox).Text.Trim();
                    YsgcTb ys = new YsgcTb();
                    ys.kmbh = hiddkmbh;
                    ys.spring = spring;
                    ys.summer = summer;
                    ys.autumn = autumn;
                    ys.winter = winter;
                    ys.year = year;
                    TbMain.Add(ys);
                }
            }
            if (sysConfig["MonthOrQuarter"] == "2")
            {
                for (int s = 0; s < GridView1.Rows.Count; s++)
                {
                    string hiddkmbh = (GridView1.Rows[s].FindControl("HiddenKmbh") as HiddenField).Value; //科目编号
                    string January = (GridView1.Rows[s].FindControl("txtJanuary") as TextBox).Text.Trim();
                    string February = (GridView1.Rows[s].FindControl("txtFebruary") as TextBox).Text.Trim();
                    string march = (GridView1.Rows[s].FindControl("txtmarch") as TextBox).Text.Trim();
                    string April = (GridView1.Rows[s].FindControl("txtApril") as TextBox).Text.Trim();
                    string May = (GridView1.Rows[s].FindControl("txtMay") as TextBox).Text.Trim();
                    string June = (GridView1.Rows[s].FindControl("txtJune") as TextBox).Text.Trim();
                    string July = (GridView1.Rows[s].FindControl("txtJuly") as TextBox).Text.Trim();
                    string August = (GridView1.Rows[s].FindControl("txtAugust") as TextBox).Text.Trim();
                    string September = (GridView1.Rows[s].FindControl("txtSeptember") as TextBox).Text.Trim();
                    string October = (GridView1.Rows[s].FindControl("txtOctober") as TextBox).Text.Trim();
                    string November = (GridView1.Rows[s].FindControl("txtNovember") as TextBox).Text.Trim();
                    string December = (GridView1.Rows[s].FindControl("txtDecember") as TextBox).Text.Trim();
                    string year = (GridView1.Rows[s].FindControl("txtyear") as TextBox).Text.Trim();
                    YsgcTb ys = new YsgcTb();
                    ys.kmbh = hiddkmbh;
                    ys.January = January;
                    ys.February = February;
                    ys.march = march;
                    ys.April = April;
                    ys.May = May;
                    ys.June = June;
                    ys.July = July;
                    ys.August = August;
                    ys.September = September;
                    ys.October = October;
                    ys.November = November;
                    ys.December = December;
                    ys.year = year;
                    TbMain.Add(ys);
                }
            }
            //02是财务填报  表   部门编号
            string usercode = Session["userCode"].ToString().Trim();
            string flowid = "ys";//因为这个页面没有启用，所以暂时写死
            if (bll.Addtb(TbMain, deptCode, "", nd, usercode, "-1", flowid,""))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功');", true);
            }


        }
    }
}
