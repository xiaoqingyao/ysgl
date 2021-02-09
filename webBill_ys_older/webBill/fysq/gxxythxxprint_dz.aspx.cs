using Dal.Bills;
using Dal.FeiYong_DZ;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_fysq_gxxythxxprint_dz : System.Web.UI.Page
{
    string billcode = "";//单据编号
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    //   public string je = "0";//单据金额
    dz_zcgzsqdDal zcgzdal = new dz_zcgzsqdDal();
    string strflg = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        object objbillcode = Request["billcode"];

        if (objbillcode != null)
        {
            billcode = objbillcode.ToString();

            //je = server.GetCellValue("select sum(billje) from bill_main where billname='" + billcode + "'");
        }
        object objflg = Request["flg"];
        if (objflg != null)
        {
            strflg = objflg.ToString();
        }
        if (!string.IsNullOrEmpty(strflg) && strflg == "f")
        {
            lbl_tilte.Text = "关系学员特惠信息表";

        }
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(billcode))
            {

                string strsql = @"select * from bill_gxxythxx_dz gx where billcode='" + billcode + "'";
                DataTable dt = server.GetDataTable(strsql, null);

                if (dt != null)
                {
                    txt_fx.Text = dt.Rows[0]["fenxiao"].ToString();
                    txt_xyxm.Text = dt.Rows[0]["xyxm"].ToString();
                    txt_nj.Text = dt.Rows[0]["nianji"].ToString();
                    txt_bmkc.Text = dt.Rows[0]["bmkc"].ToString();
                    txt_ysf.Text = dt.Rows[0]["ysf"].ToString();
                    txt_xxyh.Text = dt.Rows[0]["xhyh"].ToString();
                    txt_youhui.Text = dt.Rows[0]["youhui"].ToString();
                    txt_zengsong1.Text = dt.Rows[0]["zengsong1"].ToString();
                    txt_zengsong2.Text = dt.Rows[0]["zengsong2"].ToString();
                    txt_beizhu.Text = dt.Rows[0]["beizhu"].ToString();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败')", true);
                }
            }
            //审批流
            int flg = 0;

            string strsplsql = @"  select 
                                            (select dicname from bill_datadic where dictype='05' and diccode=(select userPosition from bill_users where usercode=itmes.checkuser))as zw,      ( case when  itmes.rdState='1' then '正在审批' when  itmes.rdState='0' then '等待' when  itmes.rdState='2' then '同意' when  itmes.rdState='3' then '否决' end) as sqzt,*
                                    from workflowrecord m,workflowrecords itmes 
                                    where m.recordid=itmes.recordid and (billcode='" + billcode + "' or billcode in (select billcode from bill_main where billname='" + billcode + "'))  order by stepid";
            DataTable splldt = new DataTable();
            splldt = server.GetDataTable(strsplsql, null);
            if (splldt.Rows.Count == 0)
            {
                return;
            }
            StringBuilder sbSpl = new StringBuilder("<tr  style='height:40px'>");
            flg = 0;
            for (int i = 1; i < splldt.Rows.Count + 1; i++)
            {
                flg++;
                string checkuser = splldt.Rows[i - 1]["checkuser"].ToString();
                sbSpl.Append("<td>" + checkuser + "(" + splldt.Rows[i - 1]["sqzt"].ToString() + ")签字：" + checkuser + "</td>");
                //每三列加一个tr标记
                if (i > 1 && i % 7 == 0)
                {
                    sbSpl.Append("</tr><tr  style='height:40px'>");
                    flg = 0;
                }
                //如果转到最后了也没有正好凑够6列
                if (i == splldt.Rows.Count && flg < 7 && flg != 0)
                {
                    for (int j = 0; j < 7 - flg; j++)
                    {
                        sbSpl.Append("<td></td>");
                    }
                }
            }

            string relSpl = sbSpl.ToString();
            if (flg == 0)
            {
                relSpl = relSpl.Substring(0, relSpl.Length - 25);
            }
            else
            {
                relSpl = relSpl + "</tr>";
            }

            lalSpl.Text = relSpl;


        }
    }
}