using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_xjllb_set : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
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
        sqls.Add("delete from dz_zcfzb where zth='" + this.ddlZt.SelectedValue + "' and flg='xjllb'");
        for (int i = 0; i < 65; i++)
        {
            object objval = Request["bqys_" + i];
            if (objval == null)
            {
                continue;
            }
            string bqys = Request["bqys_" + i].ToString();
            string bqjs = Request["bqjs_" + i].ToString();
            string ljys = Request["ljys_" + i].ToString();
            string ljjs = Request["ljjs_" + i].ToString();
            string kmbhs = "";

            if (bqys.IndexOf(",")>-1||bqys.IndexOf("=") > -1)
            {
                kmbhs = bqys;
                bqys = "";
                bqjs = "";
                ljys = "";
                ljjs = "";
            }
            else if (bqjs.IndexOf(",") > -1 || bqjs.IndexOf("=") > -1)
            {
                kmbhs = bqjs;
                bqys = "";
                bqjs = "";
                ljys = "";
                ljjs = "";
            }
            else if (ljys.IndexOf(",") > -1||ljys.IndexOf("=") > -1)
            {
                kmbhs = ljys;
                bqys = "";
                bqjs = "";
                ljys = "";
                ljjs = "";
            }
            else if (ljjs.IndexOf(",") > -1 || ljjs.IndexOf("=") > -1)
            {
                kmbhs = ljjs;
                bqys = "";
                bqjs = "";
                ljys = "";
                ljjs = "";
            }


            string sqltemp = "insert into dz_zcfzb(zth,id,bqys,bqjs,ljys,ljjs,kmbhs,flg) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')";
            sqltemp = string.Format(sqltemp, zth, i, bqys, bqjs, ljys, ljjs, kmbhs, "xjllb");
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
            TextBox bqys = this.form1.FindControl("bqys_" + i) as TextBox;
            TextBox bqjs = this.form1.FindControl("bqjs_" + i) as TextBox;
            TextBox ljys = this.form1.FindControl("ljys_" + i) as TextBox;
            TextBox ljjs = this.form1.FindControl("ljjs_" + i) as TextBox;
            if (bqys != null)
            {
                string sqlTemp = string.Format("select * from dz_zcfzb where flg='xjllb' and   zth='" + zth + "' and id='{0}'", i);
                DataTable dtrel = server.GetDataTable(sqlTemp, null);
                if (dtrel.Rows.Count > 0)
                {
                    bqys.Text = dtrel.Rows[0]["bqys"].ToString();
                    bqjs.Text = dtrel.Rows[0]["bqjs"].ToString();
                    ljys.Text = dtrel.Rows[0]["ljys"].ToString();
                    ljjs.Text = dtrel.Rows[0]["ljjs"].ToString();
                    string kmbhs = dtrel.Rows[0]["kmbhs"].ToString();
                    if (!kmbhs.Equals(""))
                    {
                        bqys.Text = kmbhs;
                        bqjs.Text = kmbhs;
                        ljys.Text = kmbhs;
                        ljjs.Text = kmbhs;
                    }
                }
                else
                {
                    bqys.Text = "";
                    bqjs.Text = "";
                    ljys.Text = "";
                    ljjs.Text = "";
                }
            }
        }
    }
    protected void ddlZt_SelectedIndexChanged(object sender, EventArgs e)
    {
        initData();
    }
}