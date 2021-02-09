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
using System.Data.OleDb;
using System.IO;

public partial class webBill_yskm_GKDeptFenJieBiLi : System.Web.UI.Page
{
    sqlHelper.sqlHelper sqlHelper = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true); return;
        }
        if (!IsPostBack)
        {
            BindData();
        }
    }
    /// <summary>
    /// 未回发绑定数据
    /// </summary>
    private void BindData()
    {
        //绑定部门 and deptCode in (select objectID from bill_userRight where rightType='2' and userCode='" + Session["userCode"].ToString().Trim() + "')
        string strbinddeptsql = "select ('['+deptcode+']'+deptname)as showname,deptcode from bill_Departments where deptcode in (select distinct deptcode from bill_yskm_gkdept) ";
        if (Session["userCode"].ToString().Trim() != "admin")
        {
            strbinddeptsql += " and (deptCode in (select objectID from bill_userRight where rightType='2'and userCode='" + Session["userCode"].ToString().Trim() + "') or sjdeptcode in (select objectID from bill_userRight where rightType='2'and userCode='" + Session["userCode"].ToString().Trim() + "'))";
        }
        DataTable dtDept = sqlHelper.GetDataTable(strbinddeptsql, null);
        if (dtDept != null)
        {
            int iRowCount = dtDept.Rows.Count;
            for (int i = 0; i < iRowCount; i++)
            {
                DataRow dr = dtDept.Rows[i];
                TreeNode tn = new TreeNode();
                tn.Text = dr["showname"].ToString();
                tn.Value = dr["deptcode"].ToString();
                tn.ImageUrl = "../../webBill/Resources/Images/treeView/treeNode.gif";
                treeViewGkDept.Nodes[0].ChildNodes.Add(tn);
            }
        }
        //绑定年度
        string strbindyear = "select distinct nian from bill_ysgc order by nian desc";
        DataTable dtyear = sqlHelper.GetDataTable(strbindyear, null);
        this.ddlYear.DataSource = dtyear;
        this.ddlYear.DataTextField = "nian";
        this.ddlYear.DataValueField = "nian";
        this.ddlYear.DataBind();
    }
    ///// <summary>
    ///// 年度切换
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}
    /// <summary>
    /// 当选中部门的时候
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewGkDept_SelectedNodeChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        lblNowKmForBz.Text = "未选择";
        string strdeptcode = treeViewGkDept.SelectedNode.Value;
        string strdeptname = treeViewGkDept.SelectedNode.Text;
        this.lblNowDeptForKm.Text = strdeptname;
        lblNowDeptForBz.Text = strdeptname;

        string strbindyskmsql = "select yskmcode,yskmBm,('['+yskmcode+']'+yskmmc) as yskmshowname,yskmmc from bill_yskm where yskmcode in (select yskmcode from bill_yskm_gkdept where deptcode =@deptcode)";

        DataTable dtKm = sqlHelper.GetDataTable(strbindyskmsql, new SqlParameter[] { new SqlParameter("@deptcode", strdeptcode) });
        if (dtKm == null || dtKm.Rows.Count <= 0)
        {
            showMessage("请开启预算过程。");
        }
        else
        {
            treeViewFeeType.Nodes[0].ChildNodes.Clear();
            DataSet ds = new DataSet();
            ds.Tables.Add(dtKm);
            int iCount = dtKm.Rows.Count;
            for (int i = 0; i < iCount; i++)
            {
                DataRow dr = dtKm.Rows[i];
                if (dr["yskmcode"].ToString().Length == 2)
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = dr["yskmshowname"].ToString();
                    tn.Value = dr["yskmcode"].ToString();
                    tn.ImageUrl = "../../webBill/Resources/Images/treeView/treeNode.gif";
                    treeViewFeeType.Nodes[0].ChildNodes.Add(tn);
                    new yskm().BindYskm_No2(tn, ds, "", "", "", false);
                }
                treeViewFeeType.ExpandAll();
            }
        }
        showGridViewContent("", "", "");
    }
    /// <summary>
    /// 当选中费用科目的时候
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void treeViewFeeType_SelectedNodeChanged(object sender, EventArgs e)
    {
        lblmsg.Text = "";
        string stryskmcode = treeViewFeeType.SelectedNode.Value;
        string stryskmname = treeViewFeeType.SelectedNode.Text;

        this.lblNowKmForBz.Text = stryskmname;

        string stryear = this.ddlYear.SelectedValue;
        string strdept = this.treeViewGkDept.SelectedNode.Value.Trim();
        this.lblNowDeptForBz.Text = this.treeViewGkDept.SelectedNode.Text.Trim();
        showGridViewContent(stryear, strdept, stryskmcode);
    }
    private void showGridViewContent(string stryear, string strdept, string stryskmcode)
    {
        DataTable dtRel = new DataTable();
        if (stryear.Equals(""))
        {
        }
        else if (strdept.Equals(""))
        {
        }
        else if (stryskmcode.Equals(""))
        {
        }
        else
        {
            dtRel = new Bll.bill_gkfjbiliBLL().GetDt(stryear, strdept, stryskmcode);
        }
        this.GridView1.DataSource = dtRel;
        this.GridView1.DataBind();
    }
    decimal deTotalBili = 0;
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer && e.Row.RowType != DataControlRowType.EmptyDataRow)
        {
            string strbl = e.Row.Cells[3].Text.Trim();
            strbl = strbl.Replace("&nbsp;", "");

            double dbje = 0;
            if (!double.TryParse(strbl, out dbje))
            {
                return;
            }

            if (!strbl.Equals(""))
            {
                //填充textbox
                TextBox txtbl = e.Row.Cells[2].FindControl("bl") as TextBox;
                if (dbje == 0)
                {
                    txtbl.Text = "0.00";
                }
                if (txtbl != null)
                {
                    txtbl.Text = strbl;
                }
                //计算合计
                decimal deevebili = 0;
                if (decimal.TryParse(strbl, out deevebili))
                {
                    deTotalBili += deevebili;
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[0].Text = "合计：";
            e.Row.Cells[2].Text = deTotalBili.ToString();
        }
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string strnd = this.ddlYear.SelectedValue.Trim();
        if (strnd.Equals(""))
        {
            showMessage("请先选择年度。"); return;
        }
        else if (this.treeViewGkDept.SelectedNode == null)
        {
            showMessage("请先选择归口部门。"); return;
        }
        else if (this.treeViewFeeType.SelectedNode == null)
        {
            showMessage("请先选择预算科目。"); return;
        }
        else { }

        string strdeptcode = this.treeViewGkDept.SelectedNode.Value;
        string stryskmcode = this.treeViewFeeType.SelectedNode.Value;
        List<string> lstSql = new List<string>();
        lstSql.Add("delete from bill_gkfjbili where nian='" + strnd + "' and gkdeptcode='" + strdeptcode + "' and yskmcode='" + stryskmcode + "' ");
        int iGridviewRowCount = this.GridView1.Rows.Count;
        for (int i = 0; i < iGridviewRowCount; i++)
        {
            TextBox tbBili = this.GridView1.Rows[i].Cells[2].FindControl("bl") as TextBox;
            if (tbBili == null)
            {
                continue;
            }
            string strBili = tbBili.Text.Trim();
            bool bohasbaifen = false;
            if (strBili.IndexOf("%") != -1)
            {
                strBili = strBili.Replace("%", "");
                bohasbaifen = true;
            }
            if (strBili.Equals(""))
            {
                strBili = "0";
            }
            double bdBili = 0;
            bool bo = double.TryParse(strBili, out bdBili);
            if (!bo)
            {
                showMessage("第" + i + "行输入格式不正确。");
                break;
            }
            //分解到的部门编号
            string strfjdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            strfjdeptcode = strfjdeptcode.Replace("&nbsp;", "");
            if (strfjdeptcode.Equals(""))
            {
                continue;
            }
            if (bohasbaifen)
            {
                bdBili = bdBili / 100;
            }
            string straddsql = "insert into bill_gkfjbili(nian,gkdeptcode,yskmcode,fjdeptcode,fjbl) values('" + strnd + "','" + strdeptcode + "','" + stryskmcode + "','" + strfjdeptcode + "','" + bdBili.ToString("0.000000") + "')";
            lstSql.Add(straddsql);
        }
        if (lstSql.Count > 0)
        {
            try
            {
                int irel = sqlHelper.ExecuteNonQuerysArray(lstSql);
                if (irel > -1)
                {
                    lblmsg.Text = "保存成功。";
                }
                else { throw new Exception("未知原因。"); }
            }
            catch (Exception ex)
            {
                showMessage("保存失败，原因：" + ex.Message);
            }


        }
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

    /// <summary>
    /// 导入excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_reload_Click(object sender, EventArgs e)
    {
        object objdt = Session["nowfjbldt"];
        if (objdt == null)
        {
            return;
        }
        string strDeptCode = this.lblNowDeptForBz.Text.Trim();
        string strKmCode = this.lblNowKmForBz.Text.Trim();
        if (strDeptCode.Equals("未选择") || strDeptCode.Equals(""))
        {
            showMessage("请先选择部门。"); return;
        }
        else if (strKmCode.Equals("未选择") || strKmCode.Equals(""))
        {
            showMessage("请先选择科目。"); return;
        }
        strDeptCode = strDeptCode.Substring(1, strDeptCode.IndexOf("]") - 1);
        strKmCode = strKmCode.Substring(1, strKmCode.IndexOf("]") - 1);

        DataTable dt = (DataTable)objdt;
        if (dt == null || dt.Rows.Count <= 0)
        {
            return;
        }
        if (!dt.Rows[0]["gkbmbh"].Equals(strDeptCode))
        {
            showMessage("请确定导入的文件与当前选择的部门或预算科目信息一致。"); return;
        }
        else if (!dt.Rows[0]["yskmbh"].Equals(strKmCode))
        {
            showMessage("请确定导入的文件与当前选择的部门或预算科目信息一致。"); return;
        }
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            string strfjdeptcode = this.GridView1.Rows[i].Cells[0].Text.Trim();
            TextBox txtBili = this.GridView1.Rows[i].Cells[3].FindControl("bl") as TextBox;
            if (txtBili == null)
            {
                continue;
            }
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                Decimal dbbili = 0;
                if (dt.Rows[j]["fjbmbh"].Equals(strfjdeptcode))
                {
                    string fjbl = dt.Rows[j]["fjbl"].ToString();
                    if (!fjbl.Equals(""))
                    {
                        dbbili += Convert.ToDecimal(Decimal.Parse(fjbl, System.Globalization.NumberStyles.Float));
                        if (dbbili == 0)
                        {
                            txtBili.Text = "0";
                        }
                        else
                        {
                            txtBili.Text = dbbili.ToString();
                        }
                        break;
                    }
                }
            }
            if (txtBili.Text == "")
            {
                txtBili.Text = "0.00";
            }
        }
        Session.Remove("nowfjbldt");
    }
    /// <summary>
    /// 导出excel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btn_outExcel_Click(object sender, EventArgs e)
    {
        string strYear = "";
        if (this.ddlYear.SelectedValue != null)
        {
            strYear = this.ddlYear.SelectedValue;
        }
        else
        {
            showMessage("尚未开启年度预算过程。"); return;
        }
        string strDeptCode = this.lblNowDeptForBz.Text.Trim();
        string strKmCode = this.lblNowKmForBz.Text.Trim();
        if (strDeptCode.Equals("未选择") || strDeptCode.Equals(""))
        {
            showMessage("请先选择部门。"); return;
        }
        else if (strKmCode.Equals("未选择") || strKmCode.Equals(""))
        {
            showMessage("请先选择科目。"); return;
        }
        else { }
        strDeptCode = strDeptCode.Substring(1, strDeptCode.IndexOf("]") - 1);
        strKmCode = strKmCode.Substring(1, strKmCode.IndexOf("]") - 1);

        DataTable dtRel = new Bll.bill_gkfjbiliBLL().GetDt(strYear, strDeptCode, strKmCode);
        if (dtRel == null)
        {
            return;
        }
        //临时文件    
        string tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //if (File.Exists(tempFile))
        //{
        //    File.Delete(tempFile);
        //    tempFile = string.Format("{0}/{1}.xls", System.Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid());
        //}
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + tempFile + ";Extended Properties=Excel 8.0");
        using (con)
        {
            con.Open();
            //创建Sheet   
            OleDbCommand cmdCreate = new OleDbCommand("CREATE TABLE Sheet1 ([归口部门编号] VarChar,[归口部门名称] VarChar,[预算科目编号] VarChar,[预算科目名称] VarChar,[分解部门编号] VarChar,[分解部门名称] VarChar,[分解比例] VarChar)", con);
            cmdCreate.ExecuteNonQuery();
            //插入数据     
            for (int i = 0; i < dtRel.Rows.Count; i++)
            {
                DataRow dr = dtRel.Rows[i];
                using (OleDbCommand cmd = new OleDbCommand(@"INSERT INTO [Sheet1$] VALUES(@gkbmbh,@gkbmmc,@kmbh,@kmmc,@fjbmbh,@fjbmmc,@fjbl)", con))
                {
                    cmd.Parameters.AddWithValue("@gkbmbh", dr["gkdeptcode"]);
                    cmd.Parameters.AddWithValue("@gkbmmc", dr["gkdeptname"]);
                    cmd.Parameters.AddWithValue("@kmbh", dr["yskmcode"]);
                    cmd.Parameters.AddWithValue("@kmmc", dr["yskmmc"]);
                    cmd.Parameters.AddWithValue("@fjbmbh", dr["deptcode"]);
                    cmd.Parameters.AddWithValue("@fjbmmc", dr["deptname"]);
                    cmd.Parameters.AddWithValue("@fjbl", dr["fjbl"]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        Response.ContentType = "application/ms-excel";
        Response.Charset = "utf-8";
        Response.AppendHeader("Content-Disposition", "attachment;filename=" + strDeptCode + "-" + strKmCode + ".xls");
        Response.BinaryWrite(File.ReadAllBytes(tempFile));
        Response.End();
        File.Delete(tempFile);
    }
}
