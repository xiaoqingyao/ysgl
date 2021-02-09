using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Bll;

public partial class webBill_shouru_zhuyuanshouru : System.Web.UI.Page
{
    sqlHelper.sqlHelper sqlhelper = new sqlHelper.sqlHelper();
    string businesscode = "504";//调用webservice的编号  住院收入
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        object objcode = Request["businesscode"];
        if (objcode != null)
        {
            businesscode = objcode.ToString();
        }
        if (!IsPostBack)
        {
            this.txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //根据类型不同 加载部门
            if (businesscode.Equals("504"))//住院收入
            {
                this.ddlDept.Items.Add(new ListItem("住院收入", "40902"));
                this.ddlDept.Items.Add(new ListItem("急诊收入", "40903"));
            }
            else if (businesscode.Equals("505"))//门诊收入
            {
                this.ddlDept.Items.Add(new ListItem("门诊收款", "40901"));
                this.ddlDept.Items.Add(new ListItem("急诊收款", "40903"));
                this.ddlDept.Items.Add(new ListItem("离退收款处(老专家门诊)", "40906"));
            }
            else if (businesscode.Equals("503"))//结算
            {
                this.ddlDept.Items.Add(new ListItem("出院结算", "40902"));
                this.ddlDept.Items.Add(new ListItem("急诊收款", "40903"));

            }
            else if (businesscode.Equals("502"))//收款
            {
                this.ddlDept.Items.Add(new ListItem("住院收款", "40902"));
                this.ddlDept.Items.Add(new ListItem("急诊收款", "40903"));

            }
            else if (businesscode.Equals("506"))//门诊挂号收费
            {
                this.ddlDept.Items.Add(new ListItem("门诊", "40901"));
                this.ddlDept.Items.Add(new ListItem("急诊", "40903"));
                this.ddlDept.Items.Add(new ListItem("离退收款处(老专家门诊)", "40906"));
            }
        }
    }
    protected void btnMakBxd_OnClick(object sender, EventArgs e)
    {
        //费用发生日期
        string strDate = txtDate.Text.Trim();
        //费用发生部门
        string strdeptcode = this.ddlDept.SelectedValue;
        //摘要
        string zhaiyao = string.Empty;
        string zhaiyao_dt = DateTime.Parse(strDate).Month.ToString() + "." + DateTime.Parse(strDate).Day.ToString();
        if (businesscode.Equals("504"))//住院收入
        {
            zhaiyao = strdeptcode.Equals("40902") ? "住院收入" : "急诊收入";
        }
        else if (businesscode.Equals("505"))//门诊收入
        {
            zhaiyao = this.ddlDept.SelectedItem.Text + "收入";
        }
        else if (businesscode.Equals("503"))//结算
        {
            zhaiyao = strdeptcode.Equals("40902") ? "出院结算" : "急诊收款结算";
        }
        else if (businesscode.Equals("502"))//收款
        {
            zhaiyao = "预收款";
        }
        else if (businesscode.Equals("506"))
        {
            zhaiyao = this.ddlDept.SelectedItem.Text + "收入";
        }
        zhaiyao = zhaiyao + zhaiyao_dt;


        //执行sql的list
        List<string> lstSqls = new List<string>();
        //唯一码
        string id = new GuidHelper().getNewGuid();
        //制单人
        string usercode = Session["usercode"].ToString();

        string strsqlTemp = "insert into sr_import_temp(id,imptype,dostatus,operno,opername,deptcode,deptname,orderby,orderbyname,classcode,classname,impdate,costs,charges,zdr,zhaiyao,note1) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')";
        //导入类型
        //string imptype = businesscode;
        //if (businesscode.Equals("504"))
        //{
        //    imptype = businesscode + "-" + this.ddlDept.SelectedValue;
        //}
        if (businesscode.Equals("504") || businesscode.Equals("505"))
        {
            int dtcount = 0;
            DataRow[] drs = getrows(businesscode, strDate, strdeptcode, out dtcount);
            if (dtcount == 0)
            {
                Response.Write("对不起，未发现当日数据。");
                return;
            }
            foreach (var dr in drs)
            {

                string strsql = string.Format(strsqlTemp, id, businesscode, "0", dr["operno"].ToString(), dr["opername"].ToString(), dr["deptcode"].ToString(), dr["deptname"].ToString(), dr["orderby"].ToString()
                    , dr["OrderName"].ToString(), dr["classcode"].ToString(), dr["classname"].ToString(), dr["Date"].ToString(), dr["costs"].ToString(), dr["charges"].ToString(), usercode, zhaiyao, "");
                lstSqls.Add(strsql);
            }
        }
        else if (businesscode.Equals("502") || businesscode.Equals("503"))
        {// 502 503的时候
            int dtcount = 0;
            DataRow[] drs = getrows(businesscode, strDate, strdeptcode, out dtcount);
            if (dtcount == 0)
            {
                Response.Write("对不起，未发现当日数据。");
                return;
            }
            foreach (var dr in drs)
            {
                string strsql = string.Format(strsqlTemp, id, businesscode, "0", dr["operno"].ToString(), dr["opername"].ToString(), dr["deptcode"].ToString(), dr["deptname"].ToString(), dr["deptcode"].ToString()
                    , dr["deptname"].ToString(), dr["payway"].ToString(), dr["payway"].ToString(), dr["Date"].ToString(), dr["amount"].ToString(), dr["amount"].ToString(), usercode, zhaiyao, "");
                lstSqls.Add(strsql);
            }
            //如果是502预收款（预交金） 还要把出院结算里的结账退款数据取过来（预交金外都应该算做结账退款  ）
            DataSet ds502 = new ShouRuHelper().getData("503", strDate);
            int dtcount502 = ds502.Tables.Count;
            DataRow[] drs502 = ds502.Tables[dtcount - 1].Select("PayWay<>'预交金' and PayWay<>'舍入金额' and DeptCode='" + strdeptcode + "'");
            if (drs502 != null || drs502.Length != 0)
            {
                foreach (var dr in drs502)
                {
                    string strsql = string.Format(strsqlTemp, id, businesscode, "0", dr["operno"].ToString(), dr["opername"].ToString(), dr["deptcode"].ToString(), dr["deptname"].ToString(), dr["deptcode"].ToString()
                        , dr["deptname"].ToString(), "结账退款", "结账退款", dr["Date"].ToString(), dr["amount"].ToString(), dr["amount"].ToString(), usercode, zhaiyao, "");
                    lstSqls.Add(strsql);
                }
            }
        }
        else if (businesscode.Equals("506"))//门诊挂号收费交款日报表-银行收款凭证
        {
            //先把505的收入明细放到sr_import_temp临时表里
            int dtcount = 0;
            DataRow[] drs = getrows("505", strDate, strdeptcode, out dtcount);
            if (dtcount == 0)
            {
                Response.Write("对不起，未发现当日数据。");
                return;
            }
            foreach (var dr in drs)
            {
                string strsql = string.Format(strsqlTemp, id, "505", "0", "", "", dr["deptcode"].ToString(), dr["deptname"].ToString(), dr["orderby"].ToString()
                    , dr["OrderName"].ToString(), dr["classcode"].ToString(), dr["classname"].ToString(), dr["Date"].ToString(), dr["costs"].ToString(), dr["charges"].ToString(), usercode, zhaiyao, "");
                lstSqls.Add(strsql);
            }
            //506中的支付类别明细放入sr_import_temp临时表里
            drs = getrows("506", strDate, strdeptcode, out dtcount);
            if (dtcount == 0)
            {
                Response.Write("对不起，未发现当日数据。");
                return;
            }
            foreach (var dr in drs)
            {
                string strsql = string.Format(strsqlTemp, id, businesscode, "0", "", "", dr["deptcode"].ToString(), dr["deptname"].ToString(), dr["deptcode"].ToString()
                    , dr["deptname"].ToString(), dr["PayWay"].ToString(), dr["PayWay"].ToString(), dr["Date"].ToString(), dr["Amount"].ToString(), dr["Amount"].ToString(), usercode, zhaiyao, "");
                lstSqls.Add(strsql);
            }

        }

        int irel = sqlhelper.ExecuteNonQuerysArray(lstSqls);
        if (irel < 0)
        {
            Response.Write("对不起，在获取数据源的时候出现错误，请联系管理员解决");
            return;
        }
        //调用存储过程
        string rel = sqlhelper.ExecuteScalar("exec sr_MakeSrd '" + id + "','" + businesscode + "'").ToString();
        if (rel.IndexOf("error") > -1)
        {
            string strErrorMsg = rel.Substring(6);
            Response.Write(strErrorMsg);
        }
        else if (rel.Equals("success"))
        {
            //验证通过 调用制单的存储过程
            rel = sqlhelper.ExecuteScalar("exec pro_makebxd '" + id + "','srd'").ToString();
            Response.Write("单据生成成功，对应单据号为：" + rel);
        }
        else
        {
            Response.Write("对不起，制单未成功，未知错误。");
        }
    }
    private DataRow[] getrows(string businesscode, string strDate, string deptcode, out int dtcount)
    {
        DataSet ds = new ShouRuHelper().getData(businesscode, strDate);
        dtcount = ds.Tables.Count;
        DataRow[] drs = ds.Tables[dtcount - 1].Select("DeptCode='"+deptcode+"'");
        if (drs == null || drs.Length == 0)
        {
            dtcount = 0;
        }
        return drs;
    }

}
