using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class webBill_tjbb_dz_zcfzb_result : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindZhangTao();
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
        //ddlZt.Items.Insert(0, (new ListItem("-选择帐套-", "")));
        //ddlZt.Items.Insert(1, (new ListItem("集团财务", "001")));
        //ddlZt.Items.Insert(2, (new ListItem("济南财务", "002")));
    }
    protected void ddlZt_SelectedIndexChanged(object sender, EventArgs e)
    {
        initData();
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
            Label txtNc = this.form1.FindControl("nc_" + i) as Label;
            Label txtqmys = this.form1.FindControl("qmys_" + i) as Label;
            Label txtqmjs = this.form1.FindControl("qmjs_" + i) as Label;
            if (txtNc != null)
            {
                string sqlTemp = string.Format("select * from dz_zcfzb where flg='zcfzb' and  zth='" + zth + "' and id='{0}'", i);
                DataTable dtrel = server.GetDataTable(sqlTemp, null);
                if (dtrel.Rows.Count > 0)
                {
                    decimal dencs = 0;
                    decimal deqmys = 0;
                    decimal deqmjs = 0;
                    if (decimal.TryParse(dtrel.Rows[0]["qm_ys"].ToString(), out deqmys))
                    {
                        txtqmys.Text = deqmys.ToString("N");
                    }
                    else
                    {
                        txtqmys.Text = "";
                    }
                    if (decimal.TryParse(dtrel.Rows[0]["qm_js"].ToString(), out deqmjs))
                    {
                        txtqmjs.Text = deqmjs.ToString("N");
                    }
                    else
                    {
                        txtqmjs.Text = "";
                    }
                    if (decimal.TryParse(dtrel.Rows[0]["ncs"].ToString(), out dencs))
                    {
                        txtNc.Text = dencs.ToString("N");
                    }
                    else
                    {
                        txtNc.Text = "";
                    }
                    //如果设置了汇总关系 就调用存储过程计算
                    string kmbhs = dtrel.Rows[0]["kmbhs"].ToString();
                    if (!kmbhs.Equals(""))
                    {
                        if (kmbhs.IndexOf(",") > -1)//科目汇总的
                        {
                            string kssj = Request.QueryString["kssj"].ToString();
                            string jzsj = Request.QueryString["jzsj"].ToString();
                            dtrel = server.GetDataTable("exec dz_ysjs '" + kssj + "','" + jzsj + "','" + kmbhs + "','" + zth + "'", null);
                            txtqmys.Text = decimal.Parse(dtrel.Rows[0]["ys"].ToString()).ToString("N");
                            txtqmjs.Text = decimal.Parse(dtrel.Rows[0]["js"].ToString()).ToString("N");//string.Format("{0:F}", dtrel.Rows[0]["js"]);
                        }
                        else if (kmbhs.IndexOf("=") > -1)//本表计算的
                        {
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
                            decimal endVal_ncs = 0;//年初数
                            decimal endVal_Qmys = 0;//期末预算
                            decimal endVal_Qmjs = 0;//期末决算
                            for (int j = 0; j < dic.Count; j++)
                            {
                                //Response.Write(dic[j].key+":"+dic[j].value+"<br/>");
                                #region 获取控件值并计算
                                //年初数
                                decimal deNcs = 0;
                                Label lbeNcs = this.form1.FindControl("nc_" + dic[j].value) as Label;
                                if (lbeNcs != null)
                                {
                                    if (decimal.TryParse(lbeNcs.Text, out deNcs))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_ncs += deNcs;
                                        }
                                        else
                                        {
                                            endVal_ncs -= deNcs;
                                        }
                                    }
                                }
                                //期末预算
                                decimal deQmys = 0;
                                Label lbeQmys = this.form1.FindControl("qmys_" + dic[j].value) as Label;
                                if (lbeQmys != null)
                                {
                                    if (decimal.TryParse(lbeQmys.Text, out deQmys))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_Qmys += deQmys;
                                        }
                                        else
                                        {
                                            endVal_Qmys -= deQmys;
                                        }
                                    }
                                }
                                //期末决算
                                decimal deQmjs = 0;
                                Label lbeQmjs = this.form1.FindControl("qmjs_" + dic[j].value) as Label;
                                if (lbeQmjs != null)
                                {
                                    if (decimal.TryParse(lbeQmjs.Text, out deQmjs))
                                    {
                                        if (dic[j].key.Equals("+"))
                                        {
                                            endVal_Qmjs += deQmjs;
                                        }
                                        else
                                        {
                                            endVal_Qmjs -= deQmjs;
                                        }
                                    }
                                }
                                #endregion
                            }
                            txtNc.Text = endVal_ncs.ToString("N");
                            txtqmys.Text = endVal_Qmys.ToString("N");
                            txtqmjs.Text = endVal_Qmjs.ToString("N");
                            //foreach (KeyValuePair<string, string> item in dic)
                            //{
                            //    Response.Write(item.Key + ":" + item.Value + "<br/>");
                            //}
                            #endregion
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
    public class keyvalue
    {
        public string key;
        public string value;
    }
}