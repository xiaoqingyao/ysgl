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

public partial class webBill_select_selectYksq : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strNowDeptCode = "";
    string strNowDeptName = "";
    DataTable dtuserRightDept = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        strNowDeptCode = server.GetCellValue("select deptcode from bill_departments where deptcode=(select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "')");
        string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");

        strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");

        if (!IsPostBack)
        {
            //    dtuserRightDept = server.GetDataTable("select deptCode,deptName from bill_departments where   deptCode in (" + strDeptCodes + ") and deptCode not in (" + strNowDeptCode + ") order by deptcode", null);

            //    //获取人员管理下的部门
            //    if (strDeptCodes != "")
            //    {
            //        if (dtuserRightDept.Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dtuserRightDept.Rows.Count; i++)
            //            {
            //                ListItem li = new ListItem();
            //                li.Text = "[" + dtuserRightDept.Rows[i]["deptCode"].ToString().Trim() + "]" + dtuserRightDept.Rows[i]["deptName"].ToString().Trim();
            //                li.Value = dtuserRightDept.Rows[i]["deptCode"].ToString().Trim();
            //                this.drp_dept.Items.Add(li);
            //            }

            //        }

            //    }
            TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
            tNode.NavigateUrl = "";
            tNode.Target = "";
            tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
            this.TreeView1.Nodes.Add(tNode);
            (new Departments()).BindOffice(tNode, "", "", "", false, "../Resources/images/treeview/", "", false, "", "", "");
            //bindData();
        }
    }
    private void bindData()
    {
        if (this.TreeView1.SelectedNode == null)
        {
            return;
        }
        string deptCode = this.TreeView1.SelectedNode.Value;//this.drp_dept.SelectedValue;
        if (!string.IsNullOrEmpty(deptCode))
        {
            lbl_dept.Text = this.TreeView1.SelectedNode.Text;
        }
        string sql = @" select  (select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept)as deptname,
        billName,billDate,billDept ,billJe  from bill_main main where flowid='ybbx'
        and billdept = '" + deptCode + "'  and isnull(note5,0)<>1";

        //and  billName not in (select yksq_code  from dz_yksq_bxd) 
        if (!string.IsNullOrEmpty(Request["isdz"]) && Request["isdz"] == "1")
        {
            sql = @"  select  (select '['+deptCode+']'+deptName from bill_departments where deptCode=billDept)as deptname,
            billName,billDate,billDept ,SUM(billJe) as billje ,(select bxzy from bill_ybbxmxb where billcode =(select top 1 billcode from bill_main where billname=main.billname)) as zy
             from bill_main main where flowid='ybbx' and stepid='end' and billdept = '" + deptCode + "'  and  billName not in (select yksq_code  from dz_yksq_bxd)  group by billName,billDate,billDept";
        }


        GridView1.DataSource = server.GetDataTable(sql, null);
        GridView1.DataBind();
    }
    protected void myGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {

        //if (e.Item.ItemType != ListItemType.Header)
        //{
        //    string billname = e.Item.Cells[1].Text;
        //    string strsqlzy = @"select bxzy from bill_ybbxmxb where billCode =(select top 1 billCode from bill_main where billName='" + billname + "')";
        //    string strzy = server.GetCellValue(strsqlzy,null);
        //    e.Item.Cells[5].Text = strzy;
        //}
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        //Session["ykcode"] = "";
        //Session["ykje"] = "";

        //for (int i = 0; i < GridView1.Rows.Count; i++)
        //{
        //    JsonRet temp = new JsonRet();
        //    CheckBox ck = GridView1.Rows[i].Cells[0].FindControl("CheckBox1") as CheckBox;
        //    if (ck.Checked)
        //    {
        //        Session["ykcode"] += GridView1.Rows[i].Cells[1].Text + ",";
        //        Session["ykje"] += GridView1.Rows[i].Cells[4].Text+",";
        //        //temp.Yksqcode = GridView1.Rows[i].Cells[1].Text;
        //        //temp.yksqje = GridView1.Rows[i].Cells[4].Text;

        //    }
        //}
        bindData();
    }

    /// <summary>
    /// 确定选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Button1_Click(object sender, EventArgs e)
    {

        List<JsonRet> retlist = new List<JsonRet>();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            List<JsonRet> lstTemp = new List<JsonRet>();
            CheckBox ck = GridView1.Rows[i].Cells[0].FindControl("CheckBox1") as CheckBox;
            if (ck.Checked)
            {
                string Yksqcode = GridView1.Rows[i].Cells[1].Text;
                string yksqje = GridView1.Rows[i].Cells[4].Text;
                GetYkMx(Yksqcode, yksqje, retlist);
                //if (lstTemp != null)
                //{
                //    for (int j = 0; j < lstTemp.Count; j++)
                //    {
                //        retlist.Add(lstTemp[j]);
                //    }
                //}
            }
        }
        JavaScriptSerializer jserializer = new JavaScriptSerializer();
        string script = jserializer.Serialize(retlist);
        script=script.Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\t", " ").Replace("\r", " ").Replace("\n", "<br/>");
        ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
        if (retlist.Count == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择用款申请单!');", true);
        }
    }

    protected void drp_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData();
    }
    private void GetYkMx(string yksqCode, string je, List<JsonRet> lst)
    {

        string sql = "select (select top 1 bxzy from bill_ybbxmxb where billcode=main.billcode) as bxzy,(select top 1 bxsm from bill_ybbxmxb where billcode=main.billcode) as bxsm,(select '['+deptcode+']'+deptname from bill_departments where deptcode=main.gkdept) as deptname,(select dydj from bill_yskm where yskmcode=fykm.fykm) as dydj,(select '['+yskmcode+']'+yskmmc from bill_yskm where yskmcode=fykm.fykm) as kmname,* from bill_ybbxmxb_fykm fykm,bill_main main where fykm.billcode=main.billcode and main.billcode in (select billcode from bill_main where billname='" + yksqCode + "') and fykm.je!=0 ";

        DataTable dt = server.GetDataTable(sql, null);
        YsManager ysmgr = new YsManager();

        if (dt != null && dt.Rows.Count > 0)
        {
            string gcbh = ysmgr.GetYsgcCode(DateTime.Parse(dt.Rows[0]["billDate"].ToString()));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                JsonRet temp = new JsonRet();
                temp.Yksqcode = yksqCode;
                temp.yksqje = je;
                temp.je = dt.Rows[i]["je"].ToString();
                temp.se = "0.00";
                string gkdept = dt.Rows[i]["gkdept"].ToString();//归口部门
                string kmbh = dt.Rows[i]["fykm"].ToString();//预算科目编号
                string kmmc = dt.Rows[i]["kmname"].ToString();//预算科目编号
                string deptname = dt.Rows[i]["deptname"].ToString();//部门名称
                temp.dept = deptname;
                temp.Yscode = kmmc;
                string dydj = dt.Rows[i]["dydj"].ToString();//对应单据
                decimal ysje = ysje = ysmgr.GetYueYs(gcbh, gkdept, kmbh);//预算金额
                decimal hfje = ysmgr.GetYueHf(gcbh, gkdept, kmbh, dydj);//花费金额
                decimal syje = ysje - hfje;//剩余金额
                temp.Ysje = ysje.ToString();//预算金额
                temp.Syje = syje.ToString();//剩余金额
                temp.bxzy = dt.Rows[i]["bxzy"].ToString();//报销摘要
                temp.bxsm = dt.Rows[i]["bxsm"].ToString();//报销摘要
                lst.Add(temp);
            }
        }
    }
}
class JsonRet
{
    /// <summary>
    /// 用款申请单号
    /// </summary>
    public string Yksqcode { get; set; }
    /// <summary>
    /// 单据金额
    /// </summary>
    public string yksqje { get; set; }
    /// <summary>
    /// 预算科目编号
    /// </summary>
    public string Yscode { get; set; }
    /// <summary>
    /// 部门名称
    /// </summary>
    public string dept { get; set; }
    /// <summary>
    /// 预算金额
    /// </summary>
    public string Ysje { get; set; }
    /// <summary>
    /// 剩余金额
    /// </summary>
    public string Syje { get; set; }
    /// <summary>
    /// 是否项目核算
    /// </summary>
    public string XiangMuHeSuan = "";
    /// <summary>
    /// 金额
    /// </summary>
    public string je { get; set; }
    /// <summary>
    /// 税额
    /// </summary>
    public string se { get; set; }
    /// <summary>
    /// 报销摘要
    /// </summary>
    public string bxzy = "";
    /// <summary>
    /// 报销说明
    /// </summary>
    public string bxsm = "";

}