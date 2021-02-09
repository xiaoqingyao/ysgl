using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;

public partial class webBill_ystz_ystzfp_Edit : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string kmbh = Request.Params["kmcode"];
            string bmbh = Request.Params["deptcode"];
            string djbh = Request.Params["billcode"];
            string je = Request.Params["ysje"];

            ViewState["kmcode"] = kmbh;
            ViewState["deptcode"] = bmbh;
            ViewState["billcode"] = djbh;
            ViewState["ysje"] = je;

            DataSet ds = server.GetDataSet("select * from bill_ysbl");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                StringBuilder insert = new StringBuilder();

                for (int i = 1; i <= 12; i++)
                {
                    insert.Append(" insert into bill_ysbl(yf,bl) values ");
                    insert.Append(" (");
                    insert.Append("'" + Convert.ToString(i) + "',");
                    insert.Append("0");
                    insert.Append(") ");
                }
                server.ExecuteNonQuery(insert.ToString());
            }
            ds = server.GetDataSet("select * from bill_ysbl order by yf");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                switch (Convert.ToString(dr["yf"]))
                {
                    case "1":
                        tb_01.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "2":
                        tb_02.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "3":
                        tb_03.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "4":
                        tb_04.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "5":
                        tb_05.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "6":
                        tb_06.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "7":
                        tb_07.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "8":
                        tb_08.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "9":
                        tb_09.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "10":
                        tb_10.Text = Convert.ToString(dr["bl"]);
                        break;
                    case "11":
                        tb_11.Text = Convert.ToString(dr["bl"]);
                        break;
                    default:
                        tb_12.Text = Convert.ToString(dr["bl"]);
                        break;
                }
            }
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        StringBuilder sql = new StringBuilder();
        sql.Append("delete bill_ysbl");

        decimal temp = Convert.ToDecimal(tb_01.Text) +
                       Convert.ToDecimal(tb_02.Text) +
                       Convert.ToDecimal(tb_03.Text) +
                       Convert.ToDecimal(tb_04.Text) +
                       Convert.ToDecimal(tb_05.Text) +
                       Convert.ToDecimal(tb_06.Text) +
                       Convert.ToDecimal(tb_07.Text) +
                       Convert.ToDecimal(tb_08.Text) +
                       Convert.ToDecimal(tb_09.Text) +
                       Convert.ToDecimal(tb_10.Text) +
                       Convert.ToDecimal(tb_11.Text) +
                       Convert.ToDecimal(tb_12.Text);
        if (temp != 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "输入值的合计必须是一!");
            return;
        }

        for (int i = 1; i <= 12; i++)
        {
            sql.Append(" insert into bill_ysbl(yf,bl) values ");
            sql.Append(" (");
            sql.Append("'" + Convert.ToString(i) + "',");

            switch (i)
            {
                case 1:
                    sql.Append(tb_01.Text);
                    break;
                case 2:
                    sql.Append(tb_02.Text);
                    break;
                case 3:
                    sql.Append(tb_03.Text);
                    break;
                case 4:
                    sql.Append(tb_04.Text);
                    break;
                case 5:
                    sql.Append(tb_05.Text);
                    break;
                case 6:
                    sql.Append(tb_06.Text);
                    break;
                case 7:
                    sql.Append(tb_07.Text);
                    break;
                case 8:
                    sql.Append(tb_08.Text);
                    break;
                case 9:
                    sql.Append(tb_09.Text);
                    break;
                case 10:
                    sql.Append(tb_10.Text);
                    break;
                case 11:
                    sql.Append(tb_11.Text);
                    break;
                default:
                    sql.Append(tb_12.Text);
                    break;
            }
                 
            sql.Append("0");
            sql.Append(") ");
        }
        server.ExecuteNonQuery(sql.ToString());

        string kmbh =Convert.ToString(ViewState["kmcode"]);
        string bmbh = Convert.ToString(ViewState["deptcode"]);
        string djbh = Convert.ToString(ViewState["billcode"]);
        string je = Convert.ToString(ViewState["ysje"]);
        string date = (Convert.ToString(DateTime.Now)).Substring(0, 4) + "0001";
        SqlParameter[] sps = {
                                 new SqlParameter("@cllb","0"),
                                 new SqlParameter("@gcbh",date),
                                 new SqlParameter("@bmbh",bmbh),
                                 new SqlParameter("@yskm",kmbh),
                                 new SqlParameter("@xgje",je),
                             };
        server.ExecuteProc("bill_pro_ystz", sps);
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script>window.close();</script>");

    }
}
