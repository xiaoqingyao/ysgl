using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Bll.UserProperty;
using System.Text;

public partial class webBill_bxgl_SelectBxDeptForGk : System.Web.UI.Page
{
    string stryskmcode = "";//预算科目编号
    string strbxdate = "";//报销时间
    string strgkdept = "";//归口部门
    string stryskmname = "";//预算科目名称
    string strysgc = "";//预算过程编号
    YsManager ysmgr = new YsManager();
    sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        object objYskmcode = Request["yskmname"];
        if (objYskmcode != null)
        {
            stryskmname = objYskmcode.ToString();
            stryskmcode = stryskmname.Substring(1, stryskmname.IndexOf("]") - 1);
        }
        else
        {
            showMessage("预算科目不能为空。");
        }
        object objbxdate = Request["bxdate"];
        if (objbxdate != null)
        {
            strbxdate = objbxdate.ToString();
            DateTime dt;
            if (!DateTime.TryParse(strbxdate, out dt))
            {
                showMessage("报销日期格式不正确。");
                return;
            }

            strysgc = ysmgr.GetYsgcCode(dt);
        }
        object objgkdept = Request["deptcode"];
        if (objgkdept != null)
        {
            strgkdept = objgkdept.ToString();
        }
        if (!IsPostBack)
        {
            bindData();
        }
        //ClientScript.RegisterArrayDeclaration("availableTags", getalldept());
    }
    private void bindData()
    {
        lblDept.Text = strgkdept;
        lblBxsj.Text = strbxdate;
        lblYskm.Text = stryskmname;

        //绑定部门
        if (!IsPostBack)
        {
            TreeNode tNode = new TreeNode("【" + Session["userName"].ToString().Trim() + "】管理单位", "00");
            tNode.NavigateUrl = "#";
            tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
            this.TreeView1.Nodes.Add(tNode);

            //(new Departments()).BindOffice(tNode, "", "", "All", false, "../Resources/images/treeview/", "", false, "", "", "");
            DataTable ds = sqlHelper.GetDataTable("select '['+deptcode+']'+deptname as deptname,deptcode from bill_departments where deptcode in  (select deptcode from bill_yskm_gkdept ) and deptstatus='1' and (deptcode in ( select objectID from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "') or sjdeptcode in ( select objectID from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "'))", null);
            if (ds != null && ds.Rows.Count > 0)
            {
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    TreeNode tnode1 = new TreeNode(ds.Rows[i]["deptname"].ToString());
                    tnode1.NavigateUrl = "";
                    tnode1.Value = ds.Rows[i]["deptcode"].ToString();
                    tnode1.ImageUrl = "../Resources/Images/treeView/office.gif";
                    tnode1.Target = "";
                    tNode.ChildNodes.Add(tnode1);
                }
            }
        }

        //string strsql = "select depts.deptcode,depts.deptname from bill_departments depts inner join (select deptcode from  bill_yskm_dept where yskmcode=@yskmcode) yskmdept on depts.deptcode=yskmdept.deptcode ";
        //SqlParameter[] sparr = { new SqlParameter("@yskmcode", stryskmcode) };
        //DataTable dtRel = sqlHelper.GetDataTable(strsql, sparr);
        //this.GridView1.DataSource = dtRel;
        //this.GridView1.DataBind();

        //上一归口部门的科目的报销部门
        string strsql = @"select distinct depts.deptcode,depts.deptname,'0' as bxje,'0' as shuie from bill_departments depts inner join (
                    select gkdept from bill_main main inner join 
                      bill_ybbxmxb_fykm fykm on main.billcode=fykm.billcode
                    where main.flowid='gkbx' and main.isgk='1' and main.billdept=@gkdept and fykm.fykm=@fykm) a on depts.deptcode=a.gkdept";
        SqlParameter[] sparr = { new SqlParameter("@gkdept", strgkdept.Substring(1, strgkdept.IndexOf(']') - 1)), new SqlParameter("@fykm", stryskmcode) };
        DataTable dtrel = sqlHelper.GetDataTable(strsql, sparr);
        this.GridView1.DataSource = dtrel;
        this.GridView1.DataBind();

    }

    protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {
            string strdeptcode = e.Row.Cells[0].Text.Trim();
            strdeptcode = strdeptcode.Replace("&nbsp;", "");
            decimal deysje = ysmgr.GetYueYs(strysgc, strdeptcode, stryskmcode);//预算金额
            e.Row.Cells[2].Text = deysje.ToString();

            decimal dehfje = ysmgr.GetYueHf(strysgc, strdeptcode, stryskmcode);//花费金额
            decimal desyje = deysje - dehfje;//剩余金额
            e.Row.Cells[3].Text = desyje.ToString();

            string strsybl = "0%";
            strsybl = deysje == 0 ? "0%" : (Math.Round((deysje - desyje) / deysje * 100, 2)).ToString() + "%";
            e.Row.Cells[4].Text = strsybl;

        }
    }
    /// <summary>
    /// 选择部门
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void TreeView1_OnSelectedNodeChanged(object sender, EventArgs e)
    {
        string strnewdept = TreeView1.SelectedNode.Value;
        string strnewdeptname = TreeView1.SelectedNode.Text;
        DataTable dtSource = new DataTable();
        dtSource.Columns.Add("deptcode");
        dtSource.Columns.Add("deptname");
        dtSource.Columns.Add("bxje");
        dtSource.Columns.Add("shuie");
        int iRows = this.GridView1.Rows.Count;
        bool boExit = false;
        for (int i = 0; i < iRows; i++)
        {
            string strdept = GridView1.Rows[i].Cells[0].Text.Trim();
            if (strdept.Equals(strnewdept))
            {
                return;
            }
            string strdeptname = GridView1.Rows[i].Cells[1].Text.Trim();
            TextBox txtbxje = GridView1.Rows[i].Cells[4].FindControl("txtbxje") as TextBox;
            string strbxje = "0";
            if (txtbxje != null)
            {
                strbxje = txtbxje.Text.Trim();
            }
            TextBox txtshuie = GridView1.Rows[i].Cells[5].FindControl("shuie") as TextBox;
            string strshuie = "0";
            if (txtshuie != null)
            {
                strshuie = txtshuie.Text.Trim();
            }
            DataRow dr = dtSource.NewRow();
            dr["deptcode"] = strdept;
            dr["deptname"] = strdeptname;
            dr["bxje"] = strbxje;
            dr["shuie"] = strshuie;
            dtSource.Rows.Add(dr);
        }
        DataRow ndr = dtSource.NewRow();
        ndr["deptcode"] = strnewdept;
        try
        {
            strnewdeptname = strnewdeptname.Substring(strnewdeptname.IndexOf(']') + 1);
        }
        catch (Exception)
        {
        }
        ndr["deptname"] = strnewdeptname;
        ndr["bxje"] = "0.00";
        ndr["shuie"] = "0.00";
        dtSource.Rows.Add(ndr);
        this.GridView1.DataSource = dtSource;
        this.GridView1.DataBind();
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_save_Click(object sender, EventArgs e)
    {
        int iRows = this.GridView1.Rows.Count;
        IList<JsonRet> retlist = new List<JsonRet>();
        for (int i = 0; i < iRows; i++)
        {
            GridViewRow gr = this.GridView1.Rows[i];
            TextBox txtBxje = gr.Cells[4].FindControl("txtbxje") as TextBox;
            if (txtBxje == null)
            {
                continue;
            }
            else if (txtBxje.Text.Trim().Equals("0"))
            {
                continue;
            }
            else
            {
                JsonRet temp = new JsonRet();

                decimal bxje = 0;
                string strje = txtBxje.Text.Trim();//报销金额
                if (strje.Equals(""))
                {
                    continue;
                }
                else
                {
                    if (!decimal.TryParse(strje, out bxje))
                    {
                        showMessage("第" + (i + 1) + "行报销金额格式不正确。");
                        return;
                    }
                }
                temp.bxje = bxje;
                string strsyje = gr.Cells[3].Text.Trim();//剩余金额
                string strysje = gr.Cells[2].Text.Trim();//预算金额
                string strdeptname = "[" + gr.Cells[0].Text.Trim() + "]" + gr.Cells[1].Text.Trim();
                decimal deshuie = 0;
                TextBox txtShuie = gr.Cells[5].FindControl("txtse") as TextBox;
                if (txtShuie != null)
                {
                    string strse = txtShuie.Text.Trim();
                    if (!strse.Equals(""))
                    {
                        if (!decimal.TryParse(strse, out deshuie))
                        {
                            showMessage("第" + (i + 1) + "行税额格式不正确。");
                            return;
                        }
                    }
                }
                temp.se = deshuie;
                temp.Yscode = stryskmname;
                temp.dept = strdeptname;
                temp.Ysje = decimal.Parse(strysje);
                temp.Syje = decimal.Parse(strsyje);
                string strbl = temp.Ysje == 0 ? "0%" : Math.Round(((temp.Ysje - temp.Syje) / temp.Ysje * 100), 2).ToString() + "%";
                temp.sybl = strbl;
                if (temp.Syje < bxje + deshuie)
                {
                    showMessage("第" + (i + 1) + "行报销金额超出了剩余金额。");
                    return;
                }
                retlist.Add(temp);
            }
        }
        if (retlist.Count > 0)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string script = jserializer.Serialize(retlist);
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
        }
        else
        {
            showMessage("请直接填写报销部门的报销金额。");
        }
    }

    class JsonRet
    {
        public string Yscode { get; set; }
        public decimal Ysje { get; set; }
        public decimal Syje { get; set; }
        /// <summary>
        /// 销售提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Tcje { get; set; }
        /// <summary>
        /// 可用金额 预算剩余金额+费用提成金额  如果T_config没有配置系统使用该功能，则该字段无用
        /// </summary>
        public decimal Kyje { get; set; }
        /// <summary>
        /// 项目核算
        /// </summary>
        public string XiangMuHeSuan { get; set; }
        public string dept { get; set; }
        /// <summary>
        /// 报销金额
        /// </summary>
        public decimal bxje { get; set; }
        /// <summary>
        /// 税额
        /// </summary>
        public decimal se { get; set; }

        /// <summary>
        /// 使用比例
        /// </summary>
        public string sybl { get; set; }
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    /// <param name="strMsg">提示的信息</param>
    private void showMessage(string strMsg)
    {
        string strScript = "alert('" + strMsg + "');";
        ClientScript.RegisterStartupScript(this.GetType(), "", strScript, true);
    }

    //private string getalldept()
    //{
    //    DataSet ds = sqlHelper.GetDataSet("select '['+deptcode+']'+deptname as deptname from bill_departments where deptcode in  (select deptcode from bill_yskm_gkdept ) and deptstatus='1'");
    //    StringBuilder arry = new StringBuilder();
    //    foreach (DataRow dr in ds.Tables[0].Rows)
    //    {
    //        arry.Append("'");
    //        arry.Append(Convert.ToString(dr["deptname"]));
    //        arry.Append("',");
    //    }
    //    string script = arry.ToString().Substring(0, arry.Length - 1);

    //    return script;

    //}
}
