using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using Models;
using Bll.UserProperty;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public partial class webBill_ysglnew_BenefityskmList : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    public string ystbfs = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["nd"] != null)
                {
                    this.BindDataGrid();
                    //  Button1.Enabled = true;
                }
                else
                {
                    Button1.Enabled = false;
                }
            }
        }
    }

    public void BindDataGrid()
    {

        ystbfs = server.GetCellValue("select ConfigValue from dbo.bill_SysConfig where ConfigName='ystbfs' and nd='" + Request.QueryString["nd"].ToString() + "' ", null);
        if (ystbfs == "1")
        {
            myGrid.Columns[1].Visible = false;
        }
        else
        {
            myGrid.Columns[1].Visible = true;
        }


        SysManager sysMgr = new SysManager();

        string benefitCode = Request.Params["BenefitCode"];
        string stryslb = "";
        if ( !string.IsNullOrEmpty(Request["yslb"]))
        {
            stryslb = Request["yslb"].ToString();
        }
        if (string.IsNullOrEmpty(benefitCode) || benefitCode.Length <= 4)
        {
            Button1.Enabled = false;
        }
        else
        {
            string strshowxmname = server.GetCellValue("select '['+procode+']'+proname as showname from bill_ys_benefitpro where procode='" + benefitCode + "'");
            if (!string.IsNullOrEmpty(strshowxmname))
            {
                lb_msg.Text = "当前选择的项目为:" + strshowxmname;

            }
            Button1.Enabled = true;
        }
        string sql = "";
        if (ystbfs == "0")//0是部门汇总1是预算分解
        {
            sql = "select c.deptcode+' '+c.deptname as dept,b.yskmcode,b.yskmmc,b.tbsm,b.tblx from dbo.bill_yskm_dept a left join dbo.bill_yskm b on a.yskmcode=b.yskmCode left join dbo.bill_departments c on c.deptCode=a.deptcode where isnull(a.deptcode,'')<>'' {0} order by c.deptCode,b.yskmCode";
        }
        else
        {
            sql = "select '' as dept,yskmcode,yskmmc,tbsm,tblx from dbo.Bill_Yskm where kmstatus='1' {0} order by yskmcode";
        }

        string sqlappend = "";
        //如果预算类别不为空，则过滤要显示的预算科目
        if (!string.IsNullOrEmpty(stryslb))
        {
            sqlappend = " and dydj='" + stryslb + "'";
        }

        sql = string.Format(sql, sqlappend);



        DataTable dt = server.GetDataTable(sql, null);

        IList<Bill_Yskm> list = new List<Bill_Yskm>();
        foreach (DataRow dr in dt.Rows)
        {
            Bill_Yskm yskm = new Bill_Yskm();
            yskm.Tblx = Convert.ToString(dr["Tblx"]);
            yskm.Tbsm = Convert.ToString(dr["Tbsm"]);
            yskm.YskmCode = Convert.ToString(dr["YskmCode"]);
            yskm.YskmMc = Convert.ToString(dr["YskmMc"]);
            yskm.Dept = Convert.ToString(dr["Dept"]);
            list.Add(yskm);
        }
        sysMgr.SetEndYsbm(list);
        this.myGrid.DataSource = list;
        myGrid.DataBind();

        //if (ystbfs == "0")//0是部门汇总1是预算分解
        //{
        //    #region 如果是汇总填报  讲部门+部门编号对应的checkBox花对号

        //    List<SqlParameter> lstsp = new List<SqlParameter>();
        //    string strsqlappend = "";
        //    if (!benefitCode.Equals(""))
        //    {
        //        strsqlappend = " and procode=@benefitCode ";
        //        lstsp.Add(new SqlParameter("@benefitCode", benefitCode));
        //    }
        //    sql = "select yskmcode+deptcode as yskmcode from bill_ys_benefits_yskm where deptcode <> '' ";

        //    sql += strsqlappend;
        //    dt = server.GetDataTable(sql, lstsp.ToArray());
        //    string[] benefitYskm = new string[dt.Rows.Count];
        //    int i = 0;
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        benefitYskm[i] = Convert.ToString(dr["yskmcode"]);
        //        i++;
        //    }

        //    for (i = 0; i < myGrid.Items.Count; i++)
        //    {
        //        string dept = "";
        //        string yskm = "";
        //        if (ystbfs == "0")
        //        {
        //            dept = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
        //            dept = dept.Substring(0, dept.IndexOf(" "));
        //            yskm = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
        //        }
        //        else
        //        {
        //            yskm = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
        //        }
        //        var count = (from temp in benefitYskm
        //                     where temp == yskm + dept
        //                     select temp).Count();
        //        if (count > 0 && myGrid.Items[i].Cells[6].Text == "1")
        //        {
        //            ((CheckBox)myGrid.Items[i].FindControl("CheckBox1")).Checked = true;
        //        }
        //    }
        //    #endregion
        //}
        //else
        //{
        #region 如果是分解填报 仅仅把科目对应的话对号
        List<SqlParameter> lstsp = new List<SqlParameter>();
        string strsqlappend = "";
        if (!benefitCode.Equals(""))
        {
            strsqlappend += " and procode=@benefitCode";
            lstsp.Add(new SqlParameter("@benefitCode", benefitCode));
        }
        if (!string.IsNullOrEmpty(stryslb))
        {
            strsqlappend += " and yslb=@yslb";
            lstsp.Add(new SqlParameter("@yslb", stryslb));
        }

        string cxsql = "select yskmcode from bill_ys_benefits_yskm where deptcode = ''";
        cxsql += strsqlappend;

        DataTable dts = server.GetDataTable(cxsql, lstsp.ToArray());
        List<string> strs = new List<string>();
        foreach (DataRow i in dts.Rows)
        {
            strs.Add(i["yskmcode"].ToString());
        }
        for (int i = 0; i < myGrid.Items.Count; i++)
        {
            string yskm = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();

            if (strs.Where(p => p == yskm && yskm.Length > 2).Count() > 0 && this.myGrid.Items[i].BackColor != Color.Silver)
            {

                ((CheckBox)myGrid.Items[i].FindControl("CheckBox1")).Checked = true;
            }
        }

        #endregion
        //}
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
        string tempStr = "";
        string benefitcode = Page.Request.QueryString["BenefitCode"].ToString().Trim();
        string stryslb = Request["yslb"].ToString();
        list.Add("delete from bill_ys_benefits_yskm where procode='" + benefitcode + "' and yslb='" + stryslb + "' ");
        string deptcode = "";
        for (int i = 0; i <= this.myGrid.Items.Count - 1; i++)
        {
            CheckBox chk = (CheckBox)this.myGrid.Items[i].FindControl("CheckBox1");
            if (chk.Checked)
            {
                //tempStr = this.getFather(this.myGrid.Items[i].Cells[2].Text.ToString().Trim());
                tempStr = this.myGrid.Items[i].Cells[2].Text.ToString().Trim();
                deptcode = this.myGrid.Items[i].Cells[1].Text.ToString().Trim();
                if (deptcode.IndexOf(" ") < 0)
                {
                    deptcode = "";
                }
                else
                {
                    deptcode = deptcode.Substring(0, deptcode.IndexOf(" "));
                }
                list.Add("insert into bill_ys_benefits_yskm select '" + benefitcode + "',yskmCode,'" + deptcode.Trim() + "','" + stryslb + "' from bill_yskm where yskmCode in ('" + tempStr + "') and yskmCode not in (select yskmCode from bill_ys_benefits_yskm where procode='" + Page.Request.QueryString["BenefitCode"].ToString().Trim() + "' and deptcode='" + deptcode.Trim() + "' and yslb='" + stryslb + "')");
                tempStr = "";
            }
        }
        if (server.ExecuteNonQuerysArray(list) == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存成功！');", true);
        }
    }

    public string getFather(string pCode)
    {
        string code = pCode;
        string tempStr = "";
        if (code.Length == 2)
        {
            return "'" + code + "',";
        }
        else
        {
            int len = code.Length;
            while (len >= 4)
            {
                tempStr += "'" + code.Substring(0, len - 2) + "',";
                code = code.Substring(0, code.Length - 2);
                len = code.Length;
            }
        }
        return tempStr + "'" + pCode + "'";
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        {
            //if (e.Item.Cells[5].Text == "01")
            //{
            //    e.Item.Cells[5].Text = "单位填报";
            //}
            //else
            //{
            //    e.Item.Cells[5].Text = "财务填报";
            //    e.Item.Cells[5].CssClass = "cwtb";
            //}

            if (e.Item.Cells[6].Text == "0")
            {
                e.Item.BackColor = Color.Silver;
                ((CheckBox)e.Item.FindControl("CheckBox1")).Enabled = false;
            }

        }
    }
    protected void myGrid_rowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ystbfs == "0")
        {
            e.Row.Cells[1].Style.Add("Visible", "False");
        }
    }
}