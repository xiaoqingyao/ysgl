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

public partial class webBill_bxgl_ZijinShenqingprint_dz : BasePage
{
    string billcode = "";//单据编号
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        object objbillcode = Request["billcode"];
     
        if (objbillcode != null)
        {
            billcode = objbillcode.ToString();
           
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
                txt_yksj.Text = main.Note1;
                this.txtdept.Text = server.GetCellValue("select '['+deptcode+']'+deptName from bill_departments where deptcode='" + main.BillDept + "'");
                this.txtje.Text = main.BillJe.ToString();
                this.TextBox1.Text = main.Note2;
                this.txtSm.Text = main.BillName2;
                txt_ykfs.Text = main.Note4;
                this.txtsqr.Text = server.GetCellValue("select '['+usercode+']'+username from bill_users where usercode='" + main.BillUser + "'");


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
                if (i > 1 && i % 6 == 0)
                {
                    sbSpl.Append("</tr><tr  style='height:40px'>");
                    flg = 0;
                }
                //如果转到最后了也没有正好凑够6列
                if (i == splldt.Rows.Count && flg < 6 && flg != 0)
                {
                    for (int j = 0; j <6 - flg; j++)
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