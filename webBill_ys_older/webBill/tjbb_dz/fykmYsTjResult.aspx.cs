﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Dal.Bills;
using Dal;

public partial class webBill_tjbb_dz_fykmYsTjResult : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    ConfigDal configdal = new ConfigDal();
    string strIsHuizong = "";//通过判断是否是汇总
    string strflowid = "ybbx";//相应的flowid 默认一般报销
    string strdydj = "02";//对应单据 默认02 费用
    string strismj = "";//是否预算到末级
    string kssj = "";
    string jzsj = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objIsHuizong = Request["ishz"];
        if (objIsHuizong == null || string.IsNullOrEmpty(objIsHuizong.ToString()))
        {
            strIsHuizong = objIsHuizong.ToString();
        }
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["dydj"]))//对应单据
            {
                strdydj = Request["dydj"].ToString();
                MainDal maindal = new MainDal();
                strflowid = maindal.getJSFlowId(strdydj);

            }
            string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
            string strshowname = "单位名称:";
            if (dept == "")
            { dept = "统计单位：所有单位"; }
            else
            {
                if (dept == "")
                {
                    strshowname += "所有单位,";
                }
                else
                {

                    dept = dept.Replace('|', ',');
                    string[] array = dept.Split(',');
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(array[i]))
                        {
                            strshowname += server.GetCellValue("select '['+deptcode+']'+deptname as showname from bill_departments where deptcode ='" + array[i] + "'") + ",";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(strshowname))
                {
                    strshowname = strshowname.Substring(0, strshowname.Length - 1);
                }
            }
            this.Label1.Text = "开始时间：" + Page.Request.QueryString["kssj"].ToString().Trim() + "  截止时间：" + Page.Request.QueryString["jzsj"].ToString().Trim() + "  " + strshowname;
            this.bindData();
        }
    }

    public void bindData()
    {
        //显示按照部门来的统计查询
        if (Request["ishz"] == "1")
        {
            // myGrid.Columns[0].Visible=false;
        }
        kssj = Request.QueryString["kssj"].ToString();
        jzsj = Request.QueryString["jzsj"].ToString();
        string dept = Request.QueryString["deptCode"].ToString();
        dept = dept.Replace('|', ',');
        if (string.IsNullOrEmpty(dept))
        {
            dept = "";
        }
        //string dept = Request.QueryString["deptCode"].ToString();
        //string iscn = configdal.GetValueByKey("CYLX");
        //if (!string.IsNullOrEmpty(iscn) && iscn == "Y")
        //{
        //    string strksny = DateTime.Parse(kssj).ToString("yyyy-MM");
        //    string strzrnkssj = server.GetCellValue("select beg_time from dbo.bill_Cnpz where year_moth ='" + strksny + "' ");


        //    if (!string.IsNullOrEmpty(strzrnkssj))
        //    {
        //        kssj = strzrnkssj;
        //    }

        //    string strjzny = DateTime.Parse(jzsj).ToString("yyyy-MM");
        //    string strzrnjzsj = server.GetCellValue("select end_time from dbo.bill_Cnpz where year_moth ='" + strjzny + "' ");
        //    if (!string.IsNullOrEmpty(strzrnjzsj))
        //    {
        //        jzsj = strzrnjzsj;
        //    }

        //}
        string strsql = "exec pro_bill_bxtj_yskm2 '" + Convert.ToDateTime(kssj) + "','" + DateTime.Parse(jzsj).ToString("yyyy-MM-dd") + " 23:59:59" + "','" + dept + "','" + Request["ishz"] + "','" + strdydj + "'";
       // Response.Write(strsql);
        DataSet temp = server.GetDataSet(strsql);
        this.myGrid.DataSource = temp;
        this.myGrid.DataBind();

    }

    protected void btn_sel_Click(object sender, EventArgs e)
    {
        this.bindData();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            if (e.Item.Cells[0].Text.ToString().Trim().IndexOf("合计") >= 0)
            {
                e.Item.CssClass = "heji";
            }
            else
            {
                string strdeptcode = e.Item.Cells[0].Text.Trim();
                if (Request["ishz"] == "1")
                {
                    strdeptcode = "";
                }
                else
                {
                    strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf(']') - 1);
                }

                string strtime = e.Item.Cells[13].Text;
                string yskm = e.Item.Cells[1].Text;
                if (!string.IsNullOrEmpty(strtime) && strtime != "&nbsp;")
                {
                    e.Item.Cells[5].Text = "<a href=# onclick=\"openDetail('../tjbb/deptYsTj_Dept.aspx','" + strtime + "','" + yskm + "','" + kssj + "','" + jzsj + "','" + strdeptcode + "','0','yksq_dz','" + strdydj + "');\">" + e.Item.Cells[5].Text.ToString().Trim() + "</a>";
                }
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["dydj"]))
        {
            strdydj = Request["dydj"].ToString();
        }
        Response.Redirect("deptYsTj.aspx?dydj=" + strdydj);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Response.ClearContent();

        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        myGrid.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {

    }
}
