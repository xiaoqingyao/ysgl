using Bll.UserProperty;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_ysglnew_Cwtbhz_List : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Bll.newysgl.YsglMainBll bll = new Bll.newysgl.YsglMainBll();
    string xmcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string strdept = Request["deptCode"];
        string strnd = Request["nian"];
        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            xmcode = Request["xmcode"].ToString();
        }
        string strsql = "";
        if (!string.IsNullOrEmpty(xmcode))
        {
            strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "','" + xmcode + "' ";
        }
        else
        {
            strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "','' ";
        }


        if (!string.IsNullOrEmpty(strdept))
        {
            strdept = server.GetCellValue(" select deptname from dbo.bill_departments where deptcode='" + strdept + "' ");
        }

        string cainianText = server.GetCellValue("select xmmc from bill_ysgc where nian='" + strnd + "' and ystype='0'");
        DataTable dt = new DataTable();
        lbl_masege.Text = strdept + cainianText + "汇总";
        if (!string.IsNullOrEmpty(strsql))
        {
            dt = server.GetDataTable(strsql, null);
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
        RowsBound();
    }
    private void RowsBound()
    {
        string deptcode = Request.QueryString["deptCode"].ToString();
        SysManager sysmanager = new SysManager();

        YsgcTb gcbh = bll.GetgcbhByNd(Request.QueryString["nian"].ToString()); // 获取预算过程编号
        IDictionary<string, string> sysConfig = new Bll.UserProperty.SysManager().GetsysConfigBynd(Request.QueryString["nian"].ToString());
        Dal.Bills.YsgcDal gc = new Dal.Bills.YsgcDal();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            GridView1.Rows[i].Cells[0].Text = (i + 1).ToString();                                          //行号
            string hiddkmbh = (GridView1.Rows[i].FindControl("HiddenKmbh") as HiddenField).Value;          //科目编号
            if (sysmanager.GetYskmIsmj(hiddkmbh) != "0")                                                   //将非末级的文本框隐藏
            {
                GridView1.Rows[i].CssClass = "unEdit";
            }
        }

        //ClientScript.RegisterStartupScript(this.GetType(), "", "EnbleTxt();", true);          //将背景为#DEDEDE的TD内的textbox设置为不可用

    }
    decimal deyi = 0, degk1 = 0, deer = 0, degk2 = 0, desan = 0, degk3 = 0, desi = 0, degk4 = 0, dewu = 0, degk5 = 0, deliu = 0, degk6 = 0, deqi = 0, degk7 = 0, deba = 0, degk8 = 0, dejiu = 0, degk9 = 0, deshi = 0, degk10 = 0, deshiyi = 0, degk11 = 0, deshier = 0, degk12 = 0, denian = 0, degknain = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            #region 合计行
            decimal decnian = 0;
            string txtnian = e.Row.Cells[2].Text;

            if (txtnian != null)
            {
                if (decimal.TryParse(txtnian, out decnian))
                {
                    denian += decnian;
                }
            }
            decimal gkdecnian = 0;
            string gktxtnian = e.Row.Cells[3].Text;

            if (gktxtnian != null)
            {


                if (decimal.TryParse(gktxtnian, out gkdecnian))
                {
                    degknain += gkdecnian;
                }
            }

            decimal decyue1 = 0;
            string txtyue1 = e.Row.Cells[4].Text;
            if (txtyue1 != null)
            {

                if (decimal.TryParse(txtyue1, out decyue1))
                {
                    deyi += decyue1;
                }
            }
            decimal decyue2 = 0;
            string txtyue2 = e.Row.Cells[6].Text;
            if (txtyue2 != null)
            {

                if (decimal.TryParse(txtyue2, out decyue2))
                {
                    deer += decyue2;
                }
            }
            decimal decyue3 = 0;
            string txtyue3 = e.Row.Cells[8].Text;
            if (txtyue3 != null)
            {
                if (decimal.TryParse(txtyue3, out decyue3))
                {
                    desan += decyue3;
                }
            }


            decimal decyue4 = 0;
            string txtyue4 = e.Row.Cells[10].Text;
            if (txtyue4 != null)
            {

                if (decimal.TryParse(txtyue4, out decyue4))
                {
                    desi += decyue4;
                }
            }
            decimal decyue5 = 0;
            string txtyue5 = e.Row.Cells[12].Text;
            if (txtyue5 != null)
            {

                if (decimal.TryParse(txtyue5, out decyue5))
                {
                    dewu += decyue5;
                }
            }
            decimal decyue6 = 0;
            string txtyue6 = e.Row.Cells[14].Text;
            if (txtyue6 != null)
            {

                if (decimal.TryParse(txtyue6, out decyue6))
                {
                    deliu += decyue6;
                }
            }
            decimal decyue7 = 0;
            string txtyue7 = e.Row.Cells[16].Text;
            if (txtyue7 != null)
            {

                if (decimal.TryParse(txtyue7, out decyue7))
                {
                    deqi += decyue7;
                }
            }
            decimal decyue8 = 0;
            string txtyue8 = e.Row.Cells[18].Text;
            if (txtyue8 != null)
            {

                if (decimal.TryParse(txtyue8, out decyue8))
                {
                    deba += decyue8;
                }
            }
            decimal decyue9 = 0;
            string txtyue9 = e.Row.Cells[20].Text;
            if (txtyue9 != null)
            {

                if (decimal.TryParse(txtyue9, out decyue9))
                {
                    dejiu += decyue9;
                }
            }
            decimal decyue10 = 0;
            string txtyue10 = e.Row.Cells[22].Text;
            if (txtyue10 != null)
            {

                if (decimal.TryParse(txtyue10, out decyue10))
                {
                    deshi += decyue10;
                }
            }
            decimal decyue11 = 0;
            string txtyue11 = e.Row.Cells[24].Text;
            if (txtyue11 != null)
            {

                if (decimal.TryParse(txtyue11, out decyue11))
                {
                    deshiyi += decyue11;
                }
            }
            decimal decyue12 = 0;
            string txtyue12 = e.Row.Cells[26].Text;
            if (txtyue12 != null)
            {

                if (decimal.TryParse(txtyue12, out decyue12))
                {
                    deshier += decyue12;
                }
            }



            decimal gkdecyue1 = 0;
            string gktxtyue1 = e.Row.Cells[5].Text;
            if (gktxtyue1 != null)
            {

                if (decimal.TryParse(gktxtyue1, out gkdecyue1))
                {
                    degk1 += gkdecyue1;
                }
            }
            decimal gkdecyue2 = 0;
            string gktxtyue2 = e.Row.Cells[7].Text;
            if (gktxtyue2 != null)
            {

                if (decimal.TryParse(gktxtyue2, out gkdecyue2))
                {
                    degk2 += gkdecyue2;
                }
            }
            decimal gkdecyue3 = 0;
            string gktxtyue3 = e.Row.Cells[9].Text;
            if (gktxtyue3 != null)
            {
                if (decimal.TryParse(gktxtyue3, out gkdecyue3))
                {
                    degk3 += gkdecyue3;
                }
            }


            decimal gkdecyue4 = 0;
            string gktxtyue4 = e.Row.Cells[11].Text;
            if (gktxtyue4 != null)
            {

                if (decimal.TryParse(gktxtyue4, out gkdecyue4))
                {
                    degk4 += gkdecyue4;
                }
            }
            decimal gkdecyue5 = 0;
            string gktxtyue5 = e.Row.Cells[13].Text;
            if (gktxtyue5 != null)
            {

                if (decimal.TryParse(gktxtyue5, out gkdecyue5))
                {
                    degk5 += gkdecyue5;
                }
            }
            decimal gkdecyue6 = 0;
            string gktxtyue6 = e.Row.Cells[15].Text;
            if (gktxtyue6 != null)
            {

                if (decimal.TryParse(gktxtyue6, out gkdecyue6))
                {
                    degk6 += gkdecyue6;
                }
            }
            decimal gkdecyue7 = 0;
            string gktxtyue7 = e.Row.Cells[17].Text;
            if (gktxtyue7 != null)
            {

                if (decimal.TryParse(gktxtyue7, out gkdecyue7))
                {
                    degk7 += gkdecyue7;
                }
            }
            decimal gkdecyue8 = 0;
            string gktxtyue8 = e.Row.Cells[19].Text;
            if (gktxtyue8 != null)
            {

                if (decimal.TryParse(gktxtyue8, out gkdecyue8))
                {
                    degk8 += gkdecyue8;
                }
            }
            decimal gkdecyue9 = 0;
            string gktxtyue9 = e.Row.Cells[21].Text;
            if (gktxtyue9 != null)
            {

                if (decimal.TryParse(gktxtyue9, out gkdecyue9))
                {
                    degk9 += gkdecyue9;
                }
            }
            decimal gkdecyue10 = 0;
            string gktxtyue10 = e.Row.Cells[23].Text;
            if (gktxtyue10 != null)
            {

                if (decimal.TryParse(txtyue10, out gkdecyue10))
                {
                    degk10 += gkdecyue10;
                }
            }
            decimal gkdecyue11 = 0;
            string gktxtyue11 = e.Row.Cells[25].Text;
            if (gktxtyue11 != null)
            {

                if (decimal.TryParse(gktxtyue11, out gkdecyue11))
                {
                    degk11 += gkdecyue11;
                }
            }
            decimal gkdecyue12 = 0;
            string gktxtyue12 = e.Row.Cells[27].Text;
            if (gktxtyue12 != null)
            {

                if (decimal.TryParse(gktxtyue12, out gkdecyue12))
                {
                    degk12 += gkdecyue12;
                }
            }
            #endregion
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[1].Text = "合计：";
            e.Row.Cells[2].Text = denian.ToString("N2");
            e.Row.Cells[2].CssClass = "myGridItemRight";
            e.Row.Cells[3].Text = degknain.ToString("N2");
            e.Row.Cells[3].CssClass = "myGridItemRight";
            e.Row.Cells[4].Text = deyi.ToString("N2");
            e.Row.Cells[4].CssClass = "myGridItemRight";
            e.Row.Cells[6].Text = deer.ToString("N2");
            e.Row.Cells[6].CssClass = "myGridItemRight";
            e.Row.Cells[8].Text = desan.ToString("N2");
            e.Row.Cells[8].CssClass = "myGridItemRight";

            e.Row.Cells[10].Text = desi.ToString("N2");
            e.Row.Cells[10].CssClass = "myGridItemRight";
            e.Row.Cells[12].Text = dewu.ToString("N2");
            e.Row.Cells[12].CssClass = "myGridItemRight";
            e.Row.Cells[14].Text = deliu.ToString("N2");
            e.Row.Cells[14].CssClass = "myGridItemRight";

            e.Row.Cells[16].Text = deqi.ToString("N2");
            e.Row.Cells[16].CssClass = "myGridItemRight";
            e.Row.Cells[18].Text = deba.ToString("N2");
            e.Row.Cells[18].CssClass = "myGridItemRight";
            e.Row.Cells[20].Text = dejiu.ToString("N2");
            e.Row.Cells[20].CssClass = "myGridItemRight";
            e.Row.Cells[22].Text = deshi.ToString("N2");
            e.Row.Cells[22].CssClass = "myGridItemRight";
            e.Row.Cells[24].Text = deshiyi.ToString("N2");
            e.Row.Cells[24].CssClass = "myGridItemRight";
            e.Row.Cells[26].Text = deshier.ToString("N2");
            e.Row.Cells[26].CssClass = "myGridItemRight";

            e.Row.Cells[5].Text = degk1.ToString("N2");
            e.Row.Cells[5].CssClass = "myGridItemRight";
            e.Row.Cells[7].Text = degk2.ToString("N2");
            e.Row.Cells[7].CssClass = "myGridItemRight";
            e.Row.Cells[9].Text = degk3.ToString("N2");
            e.Row.Cells[9].CssClass = "myGridItemRight";

            e.Row.Cells[11].Text = degk4.ToString("N2");
            e.Row.Cells[11].CssClass = "myGridItemRight";
            e.Row.Cells[13].Text = degk5.ToString("N2");
            e.Row.Cells[13].CssClass = "myGridItemRight";
            e.Row.Cells[15].Text = degk6.ToString("N2");
            e.Row.Cells[15].CssClass = "myGridItemRight";
            e.Row.Cells[17].Text = degk7.ToString("N2");
            e.Row.Cells[17].CssClass = "myGridItemRight";
            e.Row.Cells[19].Text = degk8.ToString("N2");
            e.Row.Cells[19].CssClass = "myGridItemRight";
            e.Row.Cells[21].Text = degk9.ToString("N2");
            e.Row.Cells[21].CssClass = "myGridItemRight";

            e.Row.Cells[23].Text = degk10.ToString("N2");
            e.Row.Cells[23].CssClass = "myGridItemRight";
            e.Row.Cells[25].Text = degk11.ToString("N2");
            e.Row.Cells[25].CssClass = "myGridItemRight";
            e.Row.Cells[27].Text = degk12.ToString("N2");
            e.Row.Cells[2].CssClass = "myGridItemRight";


        }
    }

    protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                //总表头
                TableCellCollection tcHeader = e.Row.Cells;
                tcHeader.Clear();

                //第一行表头
                tcHeader.Add(new TableHeaderCell());
                tcHeader[0].Attributes.Add("rowspan", "2");
                tcHeader[0].Text = "序号";

                tcHeader.Add(new TableHeaderCell());
                tcHeader[1].Attributes.Add("rowspan", "2");
                tcHeader[1].Text = "预算科目";

                tcHeader.Add(new TableHeaderCell());
                tcHeader[2].Attributes.Add("colspan", "2");
                tcHeader[2].Text = "年度";

                tcHeader.Add(new TableHeaderCell());
                tcHeader[3].Attributes.Add("colspan", "2");
                tcHeader[3].Text = "一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[4].Attributes.Add("colspan", "2");
                tcHeader[4].Text = "二月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[5].Attributes.Add("colspan", "2");
                tcHeader[5].Text = "三月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[6].Attributes.Add("colspan", "2");
                tcHeader[6].Text = "四月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[7].Attributes.Add("colspan", "2");
                tcHeader[7].Text = "五月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[8].Attributes.Add("colspan", "2");
                tcHeader[8].Text = "六月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[9].Attributes.Add("colspan", "2");
                tcHeader[9].Text = "七月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[10].Attributes.Add("colspan", "2");
                tcHeader[10].Text = "八月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[11].Attributes.Add("colspan", "2");
                tcHeader[11].Text = "九月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[12].Attributes.Add("colspan", "2");
                tcHeader[12].Text = "十月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[13].Attributes.Add("colspan", "2");
                tcHeader[13].Text = "十一月份";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[14].Attributes.Add("colspan", "2");
                tcHeader[14].Text = "十二月份</th></tr><tr id='secondtr'>";
                //第二行
                tcHeader.Add(new TableHeaderCell());
                tcHeader[15].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[16].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[17].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[18].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[19].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[20].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[21].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[22].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[23].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[24].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[25].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[26].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[27].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[28].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[29].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[30].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[31].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[32].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[33].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[34].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[35].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[36].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[37].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[38].Text = "本部门填报数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[39].Text = "分校汇总数";
                tcHeader.Add(new TableHeaderCell());
                tcHeader[40].Text = "本部门填报数";
                break;
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        string strdept = Request["deptCode"];
        string strnd = Request["nian"];

        if (!string.IsNullOrEmpty(Request["xmcode"]))
        {
            xmcode = Request["xmcode"].ToString();
        }
        string strsql = "";
        if (!string.IsNullOrEmpty(xmcode))
        {
            strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "','" + xmcode + "' ";
        }
        else
        {
            strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "','' ";
        }

        // string strsql = " exec [pro_gkdeptys_hz] '" + strdept + "','" + strnd + "' ";

        if (!string.IsNullOrEmpty(strdept))
        {
            strdept = server.GetCellValue(" select deptname from dbo.bill_departments where deptcode='" + strdept + "' ");
        }

        //  string cainianText = server.GetCellValue("select xmmc from bill_ysgc where nian='" + strnd + "' and ystype='0'");
        // lbl_masege.Text = strdept + cainianText + "汇总";

        DataTable dt = server.GetDataTable(strsql, null);
        ExcelHelper excl = new ExcelHelper();
        excl.ExpExcel(dt, GridView1, lbl_masege.Text.Trim());
    }
}