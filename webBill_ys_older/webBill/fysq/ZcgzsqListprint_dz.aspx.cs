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

public partial class webBill_fysq_ZcgzsqListprint_dz : System.Web.UI.Page
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
            lbl_tilte.Text = "物品申购单";
           
        }
        if (!IsPostBack)
        {
            if (billcode.Equals("") )
            {
                Response.Write("参数不完整，请联系管理员解决。");
                return;
            }
           Bill_Main main = new MainDal().GetMainByCode(billcode);;//主表

           if (main != null)
            {
                this.txtdate.Text = ((DateTime)main.BillDate).ToString("yyyy-MM-dd");

               
            }
           dz_zcgzsqd model = new dz_zcgzsqd();
           model = zcgzdal.GetModel(billcode);

           if (model != null)
           {
               txt_bh.Text = model.bh;
               txt_wpmc.Text = model.wpmc;
               txt_ggsl.Text = model.ggsl;
               txt_yt.Text = model.yt;
               txt_sybm.Text = model.sybm;
               txt_xyrq.Text = model.xyrq;
               txt_gjjz.Text = model.gjjz;
               txt_gzbz.Text = model.gzbz;
               txt_zje.Text = model.zje.ToString();
               txt_sqjs.Text = model.sqjs.ToString();

           }
           else
           {
               ClientScript.RegisterStartupScript(this.GetType(), "", "alert('获取数据失败')", true);
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
                    for (int j = 0; j <7 - flg; j++)
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