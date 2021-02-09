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
using System.Data.SqlClient;
using Dal;

public partial class webBill_ysgl_YsImportList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        //    return;
        //}
        if (!IsPostBack)
        {
            Bind();
        }
    }
    private void Bind()
    {
        this.ddlnd.DataSource = server.GetDataTable("select distinct nian from bill_ysgc order by nian desc", null);
        this.ddlnd.DataTextField = "nian";
        this.ddlnd.DataValueField = "nian";
        this.ddlnd.DataBind();
        GridView1.DataSource = new DataTable();
        GridView1.DataBind();
    }
    protected void hdpostback_Click(object sender, EventArgs e)
    {
        //从导入页面中获取session
        string strid = Convert.ToString(Session["ysdt"]);
        Session.Contents.Remove("ysdt");
        DataTable dt = server.GetDataTable("exec pro_ys_import_chuli '" + strid + "'", null);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string strid = new GuidHelper().getNewGuid();
        string insql = " insert into bill_ys_import (id,deptcode,deptname,yskmcode,yskmmc,yi,er,san,si,wu,liu,qi,ba,jiu,shi,shiyi,shier,nian)values(@id,@deptcode,@deptname,@yskkmcode,@yskmmc,@yi,@er,@san,@si,@wu,@liu,@qi,@ba,@jiu,@shi,@shiyi,@shier,@nian)";
        using (SqlConnection conn = new SqlConnection(DataHelper.constr))
        {
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {

                    string deptcode = GridView1.Rows[i].Cells[1].Text.Trim().Replace("&nbps;", "");
                    string deptname = GridView1.Rows[i].Cells[2].Text.Trim().Replace("&nbps;", "");
                    string kmcode = GridView1.Rows[i].Cells[3].Text.Trim().Replace("&nbps;", "");
                    string kmmc = GridView1.Rows[i].Cells[4].Text.Trim().Replace("&nbps;", "");
                    string yi = GridView1.Rows[i].Cells[5].Text.Trim().Replace("&nbps;", "");
                    decimal deyi = 0;
                    if (!decimal.TryParse(yi, out deyi))
                    {
                        showMessage("第" + (i + 1) + "行一月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string er = GridView1.Rows[i].Cells[6].Text.Trim().Replace("&nbps;", "");
                    decimal deer = 0;
                    if (!decimal.TryParse(er, out deer))
                    {
                        showMessage("第" + (i + 1) + "行二月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string san = GridView1.Rows[i].Cells[7].Text.Trim().Replace("&nbps;", "");
                    decimal desan = 0;
                    if (!decimal.TryParse(san, out desan))
                    {
                        showMessage("第" + (i + 1) + "行三月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    decimal desi = 0;
                    string si = GridView1.Rows[i].Cells[8].Text.Trim().Replace("&nbps;", "");
                    if (!decimal.TryParse(si, out desi))
                    {
                        showMessage("第" + (i + 1) + "行四月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string wu = GridView1.Rows[i].Cells[9].Text.Trim().Replace("&nbps;", "");
                    decimal dewu = 0;
                    if (!decimal.TryParse(wu, out dewu))
                    {
                        showMessage("第" + (i + 1) + "行五月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string liu = GridView1.Rows[i].Cells[10].Text.Trim().Replace("&nbps;", "");
                    decimal deliu = 0;
                    if (!decimal.TryParse(liu, out deliu))
                    {
                        showMessage("第" + (i + 1) + "行六月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string qi = GridView1.Rows[i].Cells[11].Text.Trim().Replace("&nbps;", "");
                    decimal deqi = 0;
                    if (!decimal.TryParse(qi, out deqi))
                    {
                        showMessage("第" + (i + 1) + "行七月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string ba = GridView1.Rows[i].Cells[12].Text.Trim().Replace("&nbps;", "");
                    decimal deba = 0;
                    if (!decimal.TryParse(ba, out deba))
                    {
                        showMessage("第" + (i + 1) + "行八月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string jiu = GridView1.Rows[i].Cells[13].Text.Trim().Replace("&nbps;", "");
                    decimal dejiu = 0;
                    if (!decimal.TryParse(jiu, out dejiu))
                    {
                        showMessage("第" + (i + 1) + "行九月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string shi = GridView1.Rows[i].Cells[14].Text.Trim().Replace("&nbps;", "");
                    decimal deshi = 0;
                    if (!decimal.TryParse(shi, out deshi))
                    {
                        showMessage("第" + (i + 1) + "行十月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string shiyi = GridView1.Rows[i].Cells[15].Text.Trim().Replace("&nbps;", "");
                    decimal deshiyi = 0;
                    if (!decimal.TryParse(shiyi, out deshiyi))
                    {
                        showMessage("第" + (i + 1) + "行十一月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string shier = GridView1.Rows[i].Cells[16].Text.Trim().Replace("&nbps;", "");
                    decimal deshier = 0;
                    if (!decimal.TryParse(shier, out deshier))
                    {
                        showMessage("第" + (i + 1) + "行十二月份预算金额必须为阿拉伯数字", false, ""); return;
                    }
                    string sums = GridView1.Rows[i].Cells[17].Text.Trim().Replace("&nbps;", "");
                    decimal desums = 0;
                    if (!decimal.TryParse(sums, out desums))
                    {
                        showMessage("第" + (i + 1) + "行合计金额必须为阿拉伯数字", false, ""); return;
                    }
                    SqlParameter[] inparamter = {
                                                    new SqlParameter("@id",strid),
                                                        new SqlParameter("@deptcode",deptcode),
                                                          new SqlParameter("@deptname",deptname),
                                                          new SqlParameter("@yskkmcode",kmcode),
                                                          new SqlParameter("@yskmmc",kmmc),
                                                          new SqlParameter("@yi",deyi),
                                                          new SqlParameter("@er",deer),
                                                          new SqlParameter("@san",desan),
                                                          new SqlParameter("@si",desi),
                                                          new SqlParameter("@wu",dewu),
                                                          new SqlParameter("@liu",deliu),
                                                          new SqlParameter("@qi",deqi),
                                                          new SqlParameter("@ba",deba),
                                                          new SqlParameter("@jiu",dejiu),
                                                          new SqlParameter("@shi",deshi),
                                                          new SqlParameter("@shiyi",deshiyi),
                                                          new SqlParameter("@shier",deshier),
                                                          new SqlParameter("@nian",desums) };
                    DataHelper.ExcuteNonQuery(insql, tran, inparamter, false);
                }
                tran.Commit();
                if (DataHelper.ExcuteNonQuery("exec pro_ys_import '" + strid + "','" + ddlnd.SelectedValue + "','" + Session["userCode"].ToString() + "'", null, false) >= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算数导入成功！');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算数导入失败！');", true);
                }
            }
            catch
            {
                tran.Rollback();
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('预算数导入失败！');", true);
                throw;
            }
        }
    }

    decimal deyi, deer, desan, desi, dewu, deliu, deqi, deba, dejiu, deshi, deshiyi, deshier, denian;
    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
        {
            #region 计算列合计
            decimal deyieve = 0;
            string stryieve = e.Row.Cells[5].Text.Trim();
            if (decimal.TryParse(stryieve, out deyieve))
            {
                deyi += deyieve;
            }
            decimal deereve = 0;
            string strereve = e.Row.Cells[6].Text.Trim();
            if (decimal.TryParse(strereve, out deereve))
            {
                deer += deereve;
            }
            decimal desaneve = 0;
            string strsaneve = e.Row.Cells[7].Text.Trim();
            if (decimal.TryParse(strsaneve, out desaneve))
            {
                desan += desaneve;
            }
            decimal desieve = 0;
            string strsieve = e.Row.Cells[8].Text.Trim();
            if (decimal.TryParse(strsieve, out desieve))
            {
                desi += desieve;
            }
            decimal dewueve = 0;
            string strwueve = e.Row.Cells[9].Text.Trim();
            if (decimal.TryParse(strwueve, out dewueve))
            {
                dewu += dewueve;
            }
            decimal deliueve = 0;
            string strliueve = e.Row.Cells[10].Text.Trim();
            if (decimal.TryParse(strliueve, out deliueve))
            {
                deliu += deliueve;
            }
            decimal deqieve = 0;
            string strqieve = e.Row.Cells[11].Text.Trim();
            if (decimal.TryParse(strqieve, out deqieve))
            {
                deqi += deqieve;
            }
            decimal debaeve = 0;
            string strbaeve = e.Row.Cells[12].Text.Trim();
            if (decimal.TryParse(strbaeve, out debaeve))
            {
                deba += debaeve;
            }
            decimal dejiueve = 0;
            string strjiueve = e.Row.Cells[13].Text.Trim();
            if (decimal.TryParse(strjiueve, out dejiueve))
            {
                dejiu += dejiueve;
            }
            decimal deshieve = 0;
            string strshieve = e.Row.Cells[14].Text.Trim();
            if (decimal.TryParse(strshieve, out deshieve))
            {
                deshi += deshieve;
            }
            decimal deshiyieve = 0;
            string strshiyieve = e.Row.Cells[15].Text.Trim();
            if (decimal.TryParse(strshiyieve, out deshiyieve))
            {
                deshiyi += deshiyieve;
            }
            decimal deshiereve = 0;
            string strshiereve = e.Row.Cells[16].Text.Trim();
            if (decimal.TryParse(strshiereve, out deshiereve))
            {
                deshier += deshiereve;
            }
            decimal denianeve = 0;
            string strnianeve = e.Row.Cells[17].Text.Trim();
            if (decimal.TryParse(strnianeve, out denianeve))
            {
                denian += denianeve;
            }
            #endregion
            
            #region 如果有匹配失败的，添加样式
            string strdeptcode = e.Row.Cells[1].Text.Trim();
            string strdeptname = e.Row.Cells[2].Text.Trim();
            string stryskmcode = e.Row.Cells[3].Text.Trim();
            string stryskmname = e.Row.Cells[4].Text.Trim();
            if (strdeptcode.IndexOf("失败") > 0 || strdeptname.IndexOf("失败") > 0 || stryskmcode.IndexOf("失败") > 0 || stryskmname.IndexOf("失败") > 0)
            {
                e.Row.CssClass = "errorrow";
            }
            #endregion
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";

            e.Row.Cells[5].Text = deyi.ToString("0.00");
            e.Row.Cells[6].Text = deer.ToString("0.00");
            e.Row.Cells[7].Text = desan.ToString("0.00");
            e.Row.Cells[8].Text = desi.ToString("0.00");
            e.Row.Cells[9].Text = dewu.ToString("0.00");
            e.Row.Cells[10].Text = deliu.ToString("0.00");
            e.Row.Cells[11].Text = deqi.ToString("0.00");
            e.Row.Cells[12].Text = deba.ToString("0.00");
            e.Row.Cells[13].Text = dejiu.ToString("0.00");
            e.Row.Cells[14].Text = deshi.ToString("0.00");
            e.Row.Cells[15].Text = deshiyi.ToString("0.00");
            e.Row.Cells[16].Text = deshier.ToString("0.00");
            e.Row.Cells[17].Text = denian.ToString("0.00");
            e.Row.Cells[0].Style.Add("text-align", "right");
            e.Row.Cells[5].Style.Add("text-align", "right");
            e.Row.Cells[6].Style.Add("text-align", "right");
            e.Row.Cells[7].Style.Add("text-align", "right");
            e.Row.Cells[8].Style.Add("text-align", "right");
            e.Row.Cells[9].Style.Add("text-align", "right");
            e.Row.Cells[10].Style.Add("text-align", "right");
            e.Row.Cells[11].Style.Add("text-align", "right");
            e.Row.Cells[12].Style.Add("text-align", "right");
            e.Row.Cells[13].Style.Add("text-align", "right");
            e.Row.Cells[14].Style.Add("text-align", "right");
            e.Row.Cells[15].Style.Add("text-align", "right");
            e.Row.Cells[16].Style.Add("text-align", "right");
            e.Row.Cells[17].Style.Add("text-align", "right");
        }
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
