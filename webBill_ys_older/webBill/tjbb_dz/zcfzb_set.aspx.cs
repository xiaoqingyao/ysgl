using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_zcfzb_set : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //string[] names = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "15", "16", "18", "19", "20", "21", "22", "23", "24", "25", "27", "28", "29", "31", "32", "33", "", "", "", };
        if (!IsPostBack)
        {
            bindZhangTao();
            //string[] names = Request.Form.AllKeys;
            initData();
        }
    }
    private void bindZhangTao()
    {

        string strlinkdbname = new Bll.ConfigBLL().GetValueByKey("pingzhengdblinkname");

        string strselectsql = @"select dsname as db_data,iYear,
                                        cast(cAcc_Num as varchar(50))+'|*|'+dsname as tval,
                                        * from [{0}].UFTSystem.dbo.EAP_Account where iYear>='2014' order by iYear desc";
        strselectsql = string.Format(strselectsql, strlinkdbname);

        this.ddlZt.DataSource = server.GetDataTable(strselectsql, null);
        this.ddlZt.DataTextField = "companyname";
        this.ddlZt.DataValueField = "db_data";
        this.ddlZt.DataBind();
        //this.ddlZt.Items.Insert(0, (new ListItem("-选择帐套-", "")));
        //this.ddlZt.Items.Insert(1, (new ListItem("集团财务", "001")));
        //this.ddlZt.Items.Insert(2, (new ListItem("济南财务", "002")));
    }
    protected void btn_Save_Click(object sender, EventArgs e)
    {
        List<string> sqls = new List<string>();
        string zth = this.ddlZt.SelectedValue;
        if (zth.Equals(""))
        {
            Response.Write("必须选择账套");
            return;
        }
        sqls.Add("delete from dz_zcfzb where zth='" + this.ddlZt.SelectedValue + "'  and flg='zcfzb'");
        for (int i = 0; i < 65; i++)
        {
            object objval = Request["nc_" + i];
            if (objval == null)
            {
                continue;
            }
            string ncs = Request["nc_" + i].ToString();
            string qmys = Request["qmys_" + i].ToString();
            string qmjs = Request["qmjs_" + i].ToString();
            string kmbhs = "";
            if ((qmys.Length > 0 && (qmys.IndexOf(",") > -1 || qmys.IndexOf("=") > -1)) || (qmjs.Length > 0 && (qmjs.IndexOf(",") > -1 || qmjs.IndexOf("=") > -1)) || (ncs.Length > 0 && ncs.IndexOf("=") > -1))
            {
                if (qmys.IndexOf(",") > -1 || qmys.IndexOf("=") > -1)
                {
                    kmbhs = qmys;
                    qmys = "";
                    qmjs = "";
                    ncs = "";
                }
                else if (qmjs.IndexOf(",") > -1 || qmjs.IndexOf("=") > -1)
                {
                    kmbhs = qmjs;
                    qmys = "";
                    qmjs = "";
                    ncs = "";
                }
                else if (ncs.IndexOf("=") > -1)
                {
                    kmbhs = ncs;
                    qmys = "";
                    qmjs = "";
                    ncs = "";
                }
            }

            string sqltemp = "insert into dz_zcfzb(zth,id,ncs,qm_ys,qm_js,kmbhs,flg) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
            sqltemp = string.Format(sqltemp, zth, i, ncs, qmys, qmjs, kmbhs, "zcfzb");
            //if (i == 3)
            //{
            //    Response.Write(qmys);
            //    Response.Write(qmjs);
            //    Response.Write(kmbhs);
            //    Response.Write(sqltemp);
            //}
            sqls.Add(sqltemp);
        }
        new sqlHelper.sqlHelper().ExecuteNonQuerys(sqls.ToArray());
        ClientScript.RegisterStartupScript(this.GetType(), "", "alert('设置成功')", true);
    }

    private void initData()
    {
        string zth = this.ddlZt.SelectedValue;
        //if (zth.Equals(""))
        //{
        //    Response.Write("对不起，请先选择账套");
        //    return;
        //}
        sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();

        for (int i = 1; i < 65; i++)
        {
            TextBox txtNc = this.form1.FindControl("nc_" + i) as TextBox;
            TextBox txtqmys = this.form1.FindControl("qmys_" + i) as TextBox;
            TextBox txtqmjs = this.form1.FindControl("qmjs_" + i) as TextBox;
            if (txtNc != null)
            {
                string sqlTemp = string.Format("select * from dz_zcfzb where flg='zcfzb' and   zth='" + zth + "' and id='{0}'", i);
                DataTable dtrel = server.GetDataTable(sqlTemp, null);
                if (dtrel.Rows.Count > 0)
                {
                    txtNc.Text = dtrel.Rows[0]["ncs"].ToString();
                    txtqmys.Text = dtrel.Rows[0]["qm_ys"].ToString();
                    txtqmjs.Text = dtrel.Rows[0]["qm_js"].ToString();
                    string kmbhs = dtrel.Rows[0]["kmbhs"].ToString();
                    if (!kmbhs.Equals(""))
                    {
                        txtqmys.Text = kmbhs;
                        txtqmjs.Text = kmbhs;
                        if (kmbhs.IndexOf("=") > -1)
                        {
                            txtNc.Text = kmbhs;
                        }
                    }
                }
                else
                {
                    txtNc.Text = "";
                    txtqmys.Text = "";
                    txtqmjs.Text = "";
                }
            }
        }
    }
    protected void ddlZt_SelectedIndexChanged(object sender, EventArgs e)
    {
        initData();
    }
}