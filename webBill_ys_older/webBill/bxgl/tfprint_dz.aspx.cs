using Dal.Bills;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_bxgl_tfprint_dz : System.Web.UI.Page
{
    string billcode = "";//单据编号
    string dydj = "";//单据类型 ybbx :用款申请：yksq
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string je = "0";//单据金额
    protected void Page_Load(object sender, EventArgs e)
    {
        object objbillcode = Request["billcode"];
        object objtype = Request["dydj"];
        if (objbillcode != null)
        {
            billcode = objbillcode.ToString();
            je = server.GetCellValue("select sum(billje) from bill_main where billname='" + billcode + "'");
        }
        if (objtype != null)
        {
            dydj = objtype.ToString();
        }
        if (!IsPostBack)
        {
            if (billcode.Equals("") || dydj.Equals(""))
            {
                Response.Write("参数不完整，请联系管理员解决。");
                return;
            }
            IList<Bill_Main> modelMain = new MainDal().GetMainsByBillName(billcode);//主表

            if (modelMain != null)
            {
                lblykrq.Text = ((DateTime)modelMain[0].BillDate).ToString("yyyy-MM-dd");
            }
            Bill_Ybbxmxb modelybbxmx = new YbbxDal().GetYbbx(modelMain[0].BillCode);       //一般报销明细

            if (modelybbxmx != null)
            {
                lblZy.Text = modelybbxmx.Bxzy;
                //lblkxyt.Text = modelybbxmx.Bxzy;
                string[] arr = modelybbxmx.Bxrzh.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arr.Length > 1)
                {
                    if (arr.Length == 3)
                    {
                        lbl_khyh.Text = arr[0];
                        lbl_yhzh.Text = arr[1];
                      
                    }
                    else if (arr.Length == 2)
                    {
                        lbl_khyh.Text = arr[0];
                        lbl_yhzh.Text = arr[1];
                    }


                }

                string[] arrxyxx = modelybbxmx.note0.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arrxyxx.Length > 1)
                {
                    if (!string.IsNullOrEmpty(arrxyxx[0]))
                    {
                        txt_szfx.Text = arrxyxx[0];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[1]))
                    {
                        txt_xyxm.Text = arrxyxx[1];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[2]))
                    {
                        txt_sznj.Text = arrxyxx[2];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[3]))
                    {
                        txt_xybh.Text = arrxyxx[3];
                    }
                    if (!string.IsNullOrEmpty(arrxyxx[4]))
                    {
                        txt_qdsj.Text = arrxyxx[4];
                    }
                }
                //报销费用

                string[] arrbxfy = modelybbxmx.note1.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arrbxfy.Length > 1)
                {
                    if (!string.IsNullOrEmpty(arrbxfy[0]))
                    {
                        lbl_xyfdfy.Text = arrbxfy[0];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[1]))
                    {
                        lbl_yxfks.Text = arrbxfy[1];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[2]))
                    {
                        lbl_dyksdj.Text = arrbxfy[2];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[3]))
                    {
                        lbl_yxffy.Text = arrbxfy[3];
                    }
                    if (!string.IsNullOrEmpty(arrbxfy[4]))
                    {
                        lbl_ykqtfy.Text = arrbxfy[4];
                    }
                }
              
            }
            //组建费用项目的明细html
            //项目明细
            string strsql = @"select (select yskmmc from bill_yskm where yskmcode=item.fykm)as km,(select deptname from dbo.bill_departments where deptcode=main.billdept) as deptname,
                                * from Bill_Ybbxmxb m, Bill_Ybbxmxb_Fykm item,bill_main main
                                where m.billcode=item.billcode and main.billcode=m.billcode and (main.billcode='" + billcode + "' or main.billcode in (select billcode from bill_main where billname='" + billcode + "' ) )";
            DataTable dt = server.GetDataTable(strsql, null);
            StringBuilder sbFykm = new StringBuilder("<tr>");
            int flg = 0;
            for (int i = 1; i < dt.Rows.Count + 1; i++)
            {
                flg++;
                sbFykm.Append(string.Format("<td >{0}</td><td >{1}</td>", dt.Rows[i - 1]["km"].ToString(), dt.Rows[i - 1]["je"]));
                //每三列加一个tr标记
                if (i > 1 && i % 3 == 0)
                {
                    sbFykm.Append("</tr><tr>");
                    flg = 0;
                }
                //如果转到最后了也没有正好凑够三列
                if (i == dt.Rows.Count && flg < 3 && flg != 0)
                {
                    for (int j = 0; j < 3 - flg; j++)
                    {
                        sbFykm.Append("<td></td><td></td>");
                    }
                }
            }
            string relFykm = sbFykm.ToString();
            if (flg == 0)
            {
                relFykm = relFykm.Substring(0, relFykm.Length - 25);
            }
            else
            {
                relFykm = relFykm + "</tr>";
            }
            lalFyxm.Text = relFykm;



        
            //审批流
            string strsplsql = @"  select 
                                                 (select dicname from bill_datadic where dictype='05' and diccode=(select userPosition from bill_users where usercode=itmes.checkuser))as zw,      ( case when  itmes.rdState='1' then '正在审批' when  itmes.rdState='0' then '等待' when  itmes.rdState='2' then '同意' when  itmes.rdState='3' then '否决' end) as sqzt,*
                                                   from workflowrecord m,workflowrecords itmes where m.recordid=itmes.recordid and (billcode='" + billcode + "' or billcode in (select billcode from bill_main where billname='" + billcode + "'))  order by stepid";
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
                if (i > 1 && i % 6 == 0)
                {
                    sbSpl.Append("</tr><tr  style='height:40px'>");
                    flg = 0;
                }
                //如果转到最后了也没有正好凑够6列
                if (i == splldt.Rows.Count && flg < 6 && flg != 0)
                {
                    for (int j = 0; j < 6 - flg; j++)
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