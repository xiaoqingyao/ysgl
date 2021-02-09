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

public partial class webBill_ysglnew_BenefityskmLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
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
                DrpSelectBid();
                BindDataGrid();
            }
        }
    }

    private void DrpSelectBid()
    {
        string selectndsql = "select distinct annual from bill_ys_benefitpro order by annual desc ";
        DataTable selectdt = server.GetDataTable(selectndsql, null);
       
        for (int i = 0; i < selectdt.Rows.Count; i++)
        {
            drpSelectNd.Items.Add(new ListItem(selectdt.Rows[i]["annual"].ToString(), selectdt.Rows[i]["annual"].ToString()));
        }
        if (selectdt.Rows.Count > 0)
        {
            drpSelectNd.SelectedValue = selectdt.Rows[0]["annual"].ToString();
        }
    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    private void BindDataGrid()
    {
        TreeNode tNode = new TreeNode(drpSelectNd.SelectedValue.ToString().Trim()+"项目大类", "00");
        tNode.NavigateUrl = "BenefityskmList.aspx?BenefitCode=''&nd=" + drpSelectNd.SelectedValue.ToString();
        tNode.Target = "list";
        tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
        this.TreeView1.Nodes.Clear();
        this.TreeView1.Nodes.Add(tNode);
        string sql = "select procode,proname,yslb from bill_ys_benefitpro where fillintype = '明细汇总' and annual='" + drpSelectNd.SelectedValue.ToString().Trim() + "'  and status <>'0' order by procode";

        DataSet temp = server.GetDataSet(sql);
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["procode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["proname"].ToString().Trim(), temp.Tables[0].Rows[i]["procode"].ToString().Trim());
            sNode.NavigateUrl = "BenefityskmList.aspx?BenefitCode=" + temp.Tables[0].Rows[i]["procode"].ToString().Trim()+"&nd="+drpSelectNd.SelectedValue.ToString()+"&yslb="+temp.Tables[0].Rows[i]["yslb"].ToString();
            sNode.Target = "list";
            sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
            tNode.ChildNodes.Add(sNode);
        }
        this.TreeView1.ExpandAll();
        //(new Departments()).BindOffice(tNode, "deptList.aspx", "list", "", false, "../Resources/images/treeview/", "", false, "", "", "");
                
    }
}
