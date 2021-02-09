using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Models;

public partial class webBill_ysglnew_Lrbmx : System.Web.UI.Page
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
                DrpSelectBid();
                Bind();
            }
        }
    }
    private void DrpSelectBid()
    {
        string selectndsql = "select distinct nd from bill_SysConfig order by nd desc ";
        DataTable selectdt = server.GetDataTable(selectndsql, null);
        for (int i = 0; i < selectdt.Rows.Count; i++)
        {
            drpNd.Items.Add(new ListItem(selectdt.Rows[i]["nd"].ToString(), selectdt.Rows[i]["nd"].ToString()));
        }
        if (selectdt.Rows.Count > 0)
        {
            drpNd.SelectedValue = selectdt.Rows[0]["nd"].ToString();
        }
    }
    private void Bind()
    {
        if (drpNd.Items.Count > 0)
        {
            string cxsql = "select procode,proname,calculatype,fillintype,annual,je  from bill_ys_benefitpro where 1=1 and annual = '" + drpNd.SelectedValue + "' ";
            GridView1.DataSource = server.GetDataTable(cxsql, null);
            GridView1.DataBind();
            RowsBound();
        }

    }

    private void RowsBound()
    {
        IDictionary<string, string> dic = new Bll.UserProperty.SysManager().GetsysConfigBynd(drpNd.SelectedValue);
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            GridView1.Rows[s].Cells[0].Text = (s + 1).ToString();
            string lrfs = GridView1.Rows[s].Cells[5].Text;
            TextBox txtje = GridView1.Rows[s].FindControl("txt_je") as TextBox;
            if (lrfs != "直接录入")
            {
                txtje.Visible = false;
            }
            else if (lrfs == "明细汇总")
            {
                string procode = GridView1.Rows[s].Cells[2].Text;
                string getzjesql = "";
                if (dic["ystbfs"] == "1")
                {
                    getzjesql = "select sum(budgetmoney) from bill_ys_xmfjlrb where procode = '" + procode + "' and annual = '" + drpNd.SelectedValue + "'";
                }
                else
                {
                    getzjesql = @"  select  sum(ysje)  from   bill_ysmxb a ,bill_ys_benefits_yskm b  
                                    where  a.gcbh = '" + drpNd.SelectedValue + "0001" + @"' 
                                    and  a.ysdept=b.deptcode 
                                    and procode = '" + procode + @"'
                                    and a.yskm=b.yskmcode ";
                }
                object xje = server.ExecuteScalar(getzjesql);
                txtje.Text = Convert.ToDecimal(xje == DBNull.Value ? "0" : xje).ToString("N02");
            }  else if (lrfs == "本表计算")
                {
                    decimal bbjsje = 0;
                    if (GridView1.Rows[0].Cells[5].Text != "直接录入")
                    {
                        bbjsje = Convert.ToDecimal(GridView1.Rows[0].Cells[6].Text == "" ? "0" : GridView1.Rows[0].Cells[6].Text);
                    }
                    else
                    {
                        string je = (GridView1.Rows[0].FindControl("txt_je") as TextBox).Text;
                        bbjsje = Convert.ToDecimal(je == "" ? "0" : je);
                    }
                    for (int i = 1; i < s; i++)
                    {
                        string lrfss = GridView1.Rows[i].Cells[5].Text;
                        string jsfs = GridView1.Rows[i].Cells[4].Text;
                        if (lrfss == "明细汇总" && jsfs != "不计算")
                        {
                            if (jsfs == "加")
                            {
                                bbjsje += Convert.ToDecimal(GridView1.Rows[i].Cells[6].Text == "" ? "0" : GridView1.Rows[i].Cells[6].Text);
                            }
                            else
                            {
                                bbjsje -= Convert.ToDecimal(GridView1.Rows[i].Cells[6].Text == "" ? "0" : GridView1.Rows[i].Cells[6].Text);
                            }
                        }
                        if (lrfss == "本表计算" && jsfs != "不计算")
                        {
                            if (jsfs == "加")
                            {
                                bbjsje += Convert.ToDecimal(GridView1.Rows[i].Cells[6].Text);
                            }
                            else
                            {
                                bbjsje -= Convert.ToDecimal(GridView1.Rows[i].Cells[6].Text);
                            }
                        }
                        if (lrfss == "直接录入" && jsfs != "不计算")
                        {
                            string jes = (GridView1.Rows[i].FindControl("txt_je") as TextBox).Text;
                            if (jsfs == "加")
                            {
                                bbjsje += Convert.ToDecimal(jes == "" ? "0" : jes);
                            }
                            else
                            {
                                bbjsje -= Convert.ToDecimal(jes == "" ? "0" : jes);
                            }
                        }
                    }
                    GridView1.Rows[s].Cells[6].Text = Convert.ToDecimal(bbjsje).ToString("N02");
                }
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        IList<bill_ys_benefitpro> LrbList = new List<bill_ys_benefitpro>();
        for (int s = 0; s < GridView1.Rows.Count; s++)
        {
            if (GridView1.Rows[s].Cells[5].Text == "直接录入")
            {
                string jes = (GridView1.Rows[s].FindControl("txt_je") as TextBox).Text;
                bill_ys_benefitpro lrb = new bill_ys_benefitpro();
                lrb.annual = GridView1.Rows[s].Cells[1].Text;
                lrb.procode = GridView1.Rows[s].Cells[2].Text;
                lrb.je = Convert.ToDecimal(jes == "" ? "0" : jes);
                LrbList.Add(lrb);
            }
        }
        if (new Dal.newysgl.LrbDal().InsertJe(LrbList))
        {
            Bind();
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
    }
    protected void drpNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        Bind();
    }
}
