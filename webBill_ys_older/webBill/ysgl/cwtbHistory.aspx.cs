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

public partial class webBill_ysgl_cwtbHistory : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Page.Request.QueryString["from"].ToString().Trim() == "cwtbAdd")
            {
                
            }

            this.bindData();
        }
    }

    public void bindData()
    {
        string deptCode = Page.Request.QueryString["deptCode"].ToString().Trim();
        string gcbh=Request.QueryString["gcbh"].ToString().Trim();
        DataSet gcInfo = server.GetDataSet("select * from bill_ysgc where gcbh='" + gcbh + "'");

        string qnGcbh = server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and yue='" + gcInfo.Tables[0].Rows[0]["yue"].ToString() + "' and ystype='" + gcInfo.Tables[0].Rows[0]["ystype"].ToString() + "'");
        this.Label1.Text = "预算过程：" + server.GetCellValue("select xmmc from bill_ysgc where gcbh='" + qnGcbh + "'") + " 填报单位：" + server.GetCellValue("select deptname from bill_departments where deptcode='" + (new billCoding()).GetDeptLevel2(deptCode) + "'");

        if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "0")//年预算
        {
            this.myGrid.Visible = true;
            DataSet temp = server.GetDataSet("select yskmCode,yskmBm,replicate('&nbsp;&nbsp;',len(yskmCode)-2)+yskmmc as yskmmc,tbsm,(case tblx when '01' then '单位填报' when '02' then '<font color=red>财务填报</font>' end) as tblx,ysje from bill_ysmxb,bill_yskm where bill_yskm.yskmCode=bill_ysmxb.yskm and ysType='1' and gcbh='" + qnGcbh + "'");
            this.myGrid.DataSource = temp;
            this.myGrid.DataBind();
        }
        else if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "1")//季度预算
        {
            this.DataGrid1.Visible = true;
            DataSet temp = server.GetDataSet("exec bill_pro_ysb_history_jd '" + deptCode + "','" + gcbh + "','" + qnGcbh + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='"+(int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and ystype='0'") + "'");
            this.DataGrid1.DataSource = temp;
            this.DataGrid1.DataBind();
        }
        else if (gcInfo.Tables[0].Rows[0]["ysType"].ToString().Trim() == "2")//月预算
        {
            string nian = gcInfo.Tables[0].Rows[0]["nian"].ToString().Trim();
            int yue = int.Parse(gcInfo.Tables[0].Rows[0]["yue"].ToString().Trim());
            string jidu = "";
            string months1 = "";
            string months2 = "";
            string months3 = "";
            if (yue >= 1 && yue <= 3)
            {
                jidu = "一";
                months1 = nian + "01";
                months2 = nian + "02";
                months3 = nian + "03";
            }
            else if (yue >= 4 && yue <= 6)
            {
                jidu = "二";
                months1 = nian + "04";
                months2 = nian + "05";
                months3 = nian + "06";
            }
            else if (yue >= 7 && yue <= 9)
            {
                jidu = "三";
                months1 = nian + "07";
                months2 = nian + "08";
                months3 = nian + "09";
            }
            else if (yue >= 10 && yue <= 12)
            {
                jidu = "四";
                months1 = nian + "10";
                months2 = nian + "11";
                months3 = nian + "12";
            }


            this.DataGrid2.Visible = true;
            DataSet temp = server.GetDataSet("exec bill_pro_ysb_history_yue '" + deptCode + "','" + gcbh + "','" + qnGcbh + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and ystype='0'") + "','" + server.GetCellValue("select gcbh from bill_ysgc where nian='" + (int.Parse(gcInfo.Tables[0].Rows[0]["nian"].ToString()) - 1) + "' and yue='" + jidu + "' and ystype='1'") + "'," + months1 + "," + months2 + "," + months3 + "");
            this.DataGrid2.DataSource = temp;
            this.DataGrid2.DataBind();
        }
    }
}
