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

public partial class webBill_bxgl_bxdprint_dz : System.Web.UI.Page
{
    string billcode = "";//单据编号
    string dydj = "";//单据类型 ybbx :借款申请：yksq
    public string je = "0";//单据金额
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
                string deptcodename = modelMain[0].BillDept.ToString();
                string strdeptsql = "select deptname from dbo.bill_departments where deptcode=" + deptcodename + "";
                deptcodename = server.GetCellValue(strdeptsql);
                lblsqrq.Text = modelMain[0].Note1.ToString();
                lblDept.Text = deptcodename;
                lblykrq.Text = ((DateTime)modelMain[0].BillDate).ToString("yyyy-MM-dd");
                lblsqr.Text = modelMain[0].BillUser.ToString();
            }

            Bill_Ybbxmxb modelybbxmx = new YbbxDal().GetYbbx(modelMain[0].BillCode);       //一般报销明细
            if (modelybbxmx != null)
            {
                lblkxyt.Text = modelybbxmx.Bxzy;
                if (!string.IsNullOrEmpty(modelybbxmx.Ykfs))
                {
                    lblykfs.Text = modelybbxmx.Ykfs;
                }
                string[] arr = modelybbxmx.Bxrzh.Split(new string[] { "|&|" }, StringSplitOptions.None);
                if (arr.Length > 1)
                {
                    if (arr.Length == 3)
                    {
                        lbl_khyh.Text = arr[0];
                        lbl_yhzh.Text = arr[1];
                        lbl_skdw.Text = arr[2];
                    }
                    else if (arr.Length == 2)
                    {
                        lbl_khyh.Text = arr[0];
                        lbl_yhzh.Text = arr[1];
                    }


                }
            }
            //项目明细
            string strsql = @"select (select yskmmc from bill_yskm where yskmcode=item.fykm)as km,(select deptname from dbo.bill_departments where deptcode=main.billdept) as deptname,
* from Bill_Ybbxmxb m, Bill_Ybbxmxb_Fykm item,bill_main main
 where m.billcode=item.billcode and main.billcode=m.billcode and (main.billcode='" + billcode + "' or main.billcode in (select billcode from bill_main where billname='" + billcode + "'))";
            DataTable dt = new DataTable();
            dt = server.GetDataTable(strsql, null);
            Repeater1.DataSource = dt;
            Repeater1.DataBind();

            //审批流
            string strsplsql = @"  select 
                         (select dicname from bill_datadic where dictype='05' and diccode=(select userPosition from bill_users where usercode=itmes.checkuser))as zw,      ( case when  itmes.rdState='1' then '正在审批' when  itmes.rdState='0' then '等待' when  itmes.rdState='2' then '同意' when  itmes.rdState='3' then '否决' end) as sqzt,*
                           from workflowrecord m,workflowrecords itmes where m.recordid=itmes.recordid and (billcode='" + billcode + "' or billcode in (select billcode from bill_main where billname='" + billcode + "'))  order by stepid";
            DataTable splldt = new DataTable();
            splldt = server.GetDataTable(strsplsql, null);
            StringBuilder arry = new StringBuilder();

            if (splldt != null)
            {
                int everowscount = 0;
                arry.Append("<tr>");
                for (int i = 0; i < splldt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(splldt.Rows[i]["zw"])))
                    {
                        arry.Append("<td> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>");
                    }
                    else
                    {
                        arry.Append("<td> " + Convert.ToString(splldt.Rows[i]["zw"]) + "</td>");
                    }
                    everowscount++;
                }
                for (int i = 0; i < 8 - everowscount; i++)
                {
                    arry.Append("<td> &nbsp; &nbsp; &nbsp;</td>");
                }
                arry.Append("</tr>");
                arry.Append("<tr>");
                everowscount = 0;
                for (int i = 0; i < splldt.Rows.Count; i++)
                {
                    arry.Append("<td style='text-align: left'>意见：" + Convert.ToString(splldt.Rows[i]["sqzt"]) + "</td>");
                    everowscount++;
                }
                for (int i = 0; i < 8 - everowscount; i++)
                {
                    arry.Append("<td> &nbsp; &nbsp; &nbsp;</td>");
                }
                arry.Append("</tr>");
                arry.Append("<tr>");
                everowscount = 0;
                for (int i = 0; i < splldt.Rows.Count; i++)
                {
                    arry.Append("<td style='text-align: left'>签字：" + Convert.ToString(splldt.Rows[i]["checkuser"]) + "</td>");
                    everowscount++;
                }
                for (int i = 0; i < 8 - everowscount; i++)
                {
                    arry.Append("<td> &nbsp; &nbsp; &nbsp;</td>");
                }
                arry.Append("</tr>");
                arry.Append("<tr>");
                everowscount = 0;
                for (int i = 0; i < splldt.Rows.Count; i++)
                {
                    string checkdt = splldt.Rows[i]["checkdate"].ToString();
                    if (!string.IsNullOrEmpty(checkdt))
                    {
                        checkdt = Convert.ToDateTime(checkdt).ToString("yyyy-MM-dd");
                    }

                    arry.Append("<td style='text-align: left'>日期：" + checkdt + "</td>");
                    everowscount++;
                }
                for (int i = 0; i < 8 - everowscount; i++)
                {
                    arry.Append("<td> &nbsp; &nbsp; &nbsp;</td>");
                }
                arry.Append("</tr>");
                if (arry.Length > 1)
                {
                    ltl_sqzt.Text = arry.ToString();
                }
            }
        }
    }

}