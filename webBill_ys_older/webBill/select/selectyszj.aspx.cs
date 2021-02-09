using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using Bll;
using Dal.Bills;
using System.Data;

public partial class webBill_select_selectyszj : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    DataTable dtuserRightDept = new DataTable();
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }


        if (!IsPostBack)
        {

            bindData();
        }
    }
    private void bindData()
    {

        string sql = @" select  (select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept)as deptname,billcode,
                            (select '['+xmCode+']'+xmName from bill_xm where xmCode=note3 ) as xm,
                            (select userName from bill_users where userCode=billUser)as zdr,                      
                            billName,billDate,billDept ,billJe  
                        from bill_main main 
                        where flowid='yszj'  and  billcode not in (select yksq_code  from dz_yksq_bxd where note1='zjhz') and stepID='end' ";


        if (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"].ToString() == "1")
        {
            if (!string.IsNullOrEmpty(Request["xmcode"]))
            {
                sql += " and isnull(note3,'')='" + Request["xmcode"].ToString() + "'";

            }
        }
        else
        {
            sql += " and isnull(note3,'')=''";
        }

        GridView1.DataSource = server.GetDataTable(sql, null);
        GridView1.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["isxm"]) && Request["isxm"].ToString() == "1")
        {
            e.Item.Cells[6].CssClass = "hiddenbill";
        }
    }


    /// <summary>
    /// 确定选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {

        string strcode = "";

        for (int i = 0; i < GridView1.Rows.Count; i++)
        {

            CheckBox ck = GridView1.Rows[i].Cells[0].FindControl("CheckBox1") as CheckBox;
            if (ck.Checked)
            {
                strcode += GridView1.Rows[i].Cells[5].Text + ";";

            }
        }
        strcode = strcode.Substring(0, strcode.Length - 1);
        JavaScriptSerializer jserializer = new JavaScriptSerializer();
        string script = jserializer.Serialize(strcode);
        script = script.Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\t", " ").Replace("\r", " ").Replace("\n", "<br/>");
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
        if (!string.IsNullOrEmpty(strcode))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择需要汇总的追加单!');", true);
        }
    }

    protected void drp_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
    //    private void GetYkMx(string yksqCode, string je, List<JsonRet> lst)
    //    {

    //        string sql = @"select (select top 1 bxzy from bill_ybbxmxb where billcode=main.billcode) as bxzy,(select top 1 bxsm from bill_ybbxmxb where billcode=main.billcode) as bxsm,
    //            (select '['+deptcode+']'+deptname from bill_departments where deptcode=main.gkdept) as deptname,(select dydj from bill_yskm where yskmcode=fykm.fykm) as dydj
    //            ,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=fykm.fykm) as kmname,* from bill_ybbxmxb_fykm fykm,bill_main main where fykm.billcode=main.billcode and 
    //                main.billcode in (select billcode from bill_main where billname='" + yksqCode + "') and fykm.je!=0 ";

    //        DataTable dt = server.GetDataTable(sql, null);
    //        YsManager ysmgr = new YsManager();

    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(dt.Rows[0]["billDate"].ToString()));
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                JsonRet temp = new JsonRet();
    //                temp.Yksqcode = yksqCode;
    //                temp.yksqje = je;
    //                temp.je = dt.Rows[i]["je"].ToString();
    //                temp.se = "0.00";
    //                string gkdept = dt.Rows[i]["gkdept"].ToString();//归口部门
    //                string kmbh = dt.Rows[i]["fykm"].ToString();//预算科目编号
    //                string kmmc = dt.Rows[i]["kmname"].ToString();//预算科目编号
    //                string deptname = dt.Rows[i]["deptname"].ToString();//部门名称
    //                temp.dept = deptname;
    //                temp.Yscode = kmmc;
    //                string dydj = dt.Rows[i]["dydj"].ToString();//对应单据
    //                decimal ysje = ysje = ysmgr.GetYueYs(gcbh, gkdept, kmbh);//预算金额
    //                decimal hfje = ysmgr.GetYueHf(gcbh, gkdept, kmbh, dydj);//花费金额
    //                decimal syje = ysje - hfje;//剩余金额
    //                temp.Ysje = ysje.ToString();//预算金额
    //                temp.Syje = syje.ToString();//剩余金额
    //                temp.bxzy = dt.Rows[i]["bxzy"].ToString();//报销摘要
    //                temp.bxsm = dt.Rows[i]["bxsm"].ToString();//报销摘要
    //                lst.Add(temp);
    //            }
    //        }
    //    }
}
//class JsonRet
//{
//    /// <summary>
//    /// 用款申请单号
//    /// </summary>
//    public string Yksqcode { get; set; }
//    /// <summary>
//    /// 单据金额
//    /// </summary>
//    public string yksqje { get; set; }
//    /// <summary>
//    /// 预算科目编号
//    /// </summary>
//    public string Yscode { get; set; }
//    /// <summary>
//    /// 部门名称
//    /// </summary>
//    public string dept { get; set; }
//    /// <summary>
//    /// 预算金额
//    /// </summary>
//    public string Ysje { get; set; }
//    /// <summary>
//    /// 剩余金额
//    /// </summary>
//    public string Syje { get; set; }
//    /// <summary>
//    /// 是否项目核算
//    /// </summary>
//    public string XiangMuHeSuan = "";
//    /// <summary>
//    /// 金额
//    /// </summary>
//    public string je { get; set; }
//    /// <summary>
//    /// 税额
//    /// </summary>
//    public string se { get; set; }
//    /// <summary>
//    /// 报销摘要
//    /// </summary>
//    public string bxzy = "";
//    /// <summary>
//    /// 报销说明
//    /// </summary>
//    public string bxsm = "";

//}