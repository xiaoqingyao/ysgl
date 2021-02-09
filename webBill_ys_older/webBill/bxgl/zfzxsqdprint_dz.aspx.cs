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

public partial class webBill_bxgl_zfzxsqdprint_dz : System.Web.UI.Page
{
    string billcode = "";//单据编号
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
 //   public string je = "0";//单据金额

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
        if (!string.IsNullOrEmpty(strflg) && strflg == "n")
        {
            lbl_tilte.Text = "区域内转费转校申请单";
            lbl_zcfx.Text = "转出分校";
            lbl_zrfx.Text = "转入分校";
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

                //this.txtdept = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + main.BillDept + "'");

                //this.txtSm.Text = main.BillName2;

                //this.txtsqr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + main.BillUser + "'");
               
            }
            bill_zfzxsqd_dz mxmodel = new bill_zfzxsqd_dz();
            bill_zfzxsqd_dzDal mxdal = new bill_zfzxsqd_dzDal();

            mxmodel = mxdal.GetmodelByCode(billcode);
            if (mxmodel != null)
            {
                txt_zcqyfx.Text = mxmodel.zcfx;
                txt_zrfx.Text = mxmodel.zrfx;
                txt_xyxm.Text = mxmodel.xyxm;
                txt_nianji.Text = mxmodel.nianji;
                txt_yxyfdfy.Text = mxmodel.yxyfdfy.ToString();
                txt_ybmkc.Text = mxmodel.ybmkc;
                txt_ykcxsyh.Text = mxmodel.ykcxsyh;
                txt_yxfks.Text = mxmodel.yxfks.ToString();
                txt_dyksdj.Text = mxmodel.dyksdj.ToString();
                txt_yxffy.Text = mxmodel.yxffy.ToString();
                txt_ykqtfy.Text = mxmodel.ykqtfy.ToString();
                txt_syje.Text = mxmodel.syjexx.ToString();
                txt_syjedx.Text = mxmodel.syjedx;
                txtSm.Text = mxmodel.zfyy;
                txt_xbxs.Text = mxmodel.xbxs;
                txt_xbjje.Text = mxmodel.xbjje.ToString();
                txt_xbjjedx.Text = mxmodel.xbjjedx;


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
                if (i > 1 && i % 8 == 0)
                {
                    sbSpl.Append("</tr><tr  style='height:40px'>");
                    flg = 0;
                }
                //如果转到最后了也没有正好凑够6列
                if (i == splldt.Rows.Count && flg < 8 && flg != 0)
                {
                    for (int j = 0; j <8 - flg; j++)
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