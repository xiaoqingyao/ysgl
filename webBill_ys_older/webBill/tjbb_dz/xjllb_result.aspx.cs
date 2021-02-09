using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_xjllb_result : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindZhangTao();
            //string[] names = Request.Form.AllKeys;
            initData();
            this.lblDt.Text = Request["showdt"].ToString();
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
            Label bqys = this.form1.FindControl("bqys_" + i) as Label;
            Label bqjs = this.form1.FindControl("bqjs_" + i) as Label;
            Label ljys = this.form1.FindControl("ljys_" + i) as Label;
            Label ljjs = this.form1.FindControl("ljjs_" + i) as Label;
            if (bqys != null)
            {
                string sqlTemp = string.Format("select * from dz_zcfzb where flg='xjllb' and   zth='" + zth + "' and id='{0}'", i);
                DataTable dtrel = server.GetDataTable(sqlTemp, null);
                if (dtrel.Rows.Count > 0)
                {
                    decimal debqys = 0;
                    decimal debqjs = 0;
                    decimal deljys = 0;
                    decimal deljjs = 0;
                    if (decimal.TryParse(dtrel.Rows[0]["bqys"].ToString(), out debqys))
                    {
                        bqys.Text = debqys.ToString("N");
                    }
                    else {
                        bqys.Text = "";
                    }
                    if (decimal.TryParse(dtrel.Rows[0]["bqjs"].ToString(), out debqjs))
                    {
                        bqjs.Text = debqjs.ToString("N");
                    }
                    else
                    {
                        bqjs.Text = "";
                    }
                    if (decimal.TryParse(dtrel.Rows[0]["ljys"].ToString(), out deljys))
                    {
                        ljys.Text = deljys.ToString("N");
                    }
                    else
                    {
                        ljys.Text = "";
                    }
                    if (decimal.TryParse(dtrel.Rows[0]["ljjs"].ToString(), out deljjs))
                    {
                        ljjs.Text = deljjs.ToString("N");
                    }
                    else
                    {
                        ljjs.Text = "";
                    }
                    //如果设置了汇总关系 就调用存储过程计算
                    string kmbhs = dtrel.Rows[0]["kmbhs"].ToString();
                    if (!kmbhs.Equals(""))
                    {
                        if (kmbhs.IndexOf(",") > -1)
                        {
                            string kssj = Request.QueryString["kssj"].ToString();
                            string jzsj = Request.QueryString["jzsj"].ToString();
                            dtrel = server.GetDataTable("exec dz_ysjs '" + kssj + "','" + jzsj + "','" + kmbhs + "','" + zth + "'", null);
                            bqys.Text = decimal.Parse(dtrel.Rows[0]["ys"].ToString()).ToString("N");
                            bqjs.Text = decimal.Parse(dtrel.Rows[0]["js"].ToString()).ToString("N");
                            ljys.Text = decimal.Parse(dtrel.Rows[0]["ys_nd"].ToString()).ToString("N");
                            ljjs.Text = decimal.Parse(dtrel.Rows[0]["js_nd"].ToString()).ToString("N");
                        }
                        else if (kmbhs.IndexOf("=") > -1) {
                            #region 将公式拆分开 公式格式 =1+3-4+6……
                            kmbhs = kmbhs.Substring(1);
                            
                            string[] jia = kmbhs.Split(new string[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
                            //从第二个元素开始加负号
                           
                            
                            List<keyvalue> dic = new List<keyvalue>();
                            for (int j = 0; j < jia.Length; j++)
                            {
                                string temp = jia[j];
                                if (temp.IndexOf("-") > 0)//如果包含减号
                                {
                                    string[] jian = temp.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                                    dic.Add(new keyvalue() { key = "+", value = jian[0] });//第一个元素是+
                                    for (int k = 1; k < jian.Length; k++)
                                    {
                                        dic.Add(new keyvalue() { key = "-", value = jian[k] });
                                    }
                                }
                                else
                                {
                                    dic.Add(new keyvalue() { key = "+", value = jia[j] });
                                }
                            }
                            #endregion

                            #region 获取控件并计算值
                            decimal endVal_bqys = 0;//本期预算
                            decimal endVal_bqjs = 0;//本期决算
                            decimal endVal_ljys = 0;//累计预算
                            decimal endVal_ljjs = 0;//累计决算
                            for (int j = 0; j < dic.Count; j++)
                            {
                               //Response.Write(dic[j].key + ":" + dic[j].value+"<br/>");
                                //本期预算
                                decimal deBqys = 0;
                                Label lbeBqys = this.form1.FindControl("bqys_" + dic[j].value) as Label;
                                if (lbeBqys != null)
                                {
                                    if (decimal.TryParse(lbeBqys.Text, out deBqys))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_bqys += deBqys;
                                        }
                                        else
                                        {
                                            endVal_bqys -= deBqys;
                                        }
                                    }
                                }
                                //本期决算
                                decimal deBqjs = 0;
                                Label lbeBqjs = this.form1.FindControl("bqjs_" + dic[j].value) as Label;
                                if (lbeBqjs != null)
                                {
                                    if (decimal.TryParse(lbeBqjs.Text, out deBqjs))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_bqjs += deBqjs;
                                        }
                                        else
                                        {
                                            endVal_bqjs -= deBqjs;
                                        }
                                    }
                                }
                                //累计预算
                                decimal deLjys = 0;
                                Label lbeLjys = this.form1.FindControl("ljys_" + dic[j].value) as Label;
                                if (lbeLjys != null)
                                {
                                    if (decimal.TryParse(lbeLjys.Text, out deLjys))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_ljys += deLjys;
                                        }
                                        else
                                        {
                                            endVal_ljys -= deLjys;
                                        }
                                    }
                                }
                                //累计决算
                                decimal deLjjs = 0;
                                Label lbeLjjs = this.form1.FindControl("ljjs_" + dic[j].value) as Label;
                                if (lbeLjjs != null)
                                {
                                    if (decimal.TryParse(lbeLjjs.Text, out deLjjs))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_ljjs += deLjjs;
                                        }
                                        else
                                        {
                                            endVal_ljjs -= deLjjs;
                                        }
                                    }
                                }
                            }
                            bqys.Text = endVal_bqys.ToString("N");
                            bqjs.Text = endVal_bqjs.ToString("N");
                            ljys.Text = endVal_ljys.ToString("N");
                            ljjs.Text = endVal_ljjs.ToString("N");
                            #endregion
                        }
                    }
                }
                else {
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
    public class keyvalue
    {
        public string key;
        public string value;
    }
}