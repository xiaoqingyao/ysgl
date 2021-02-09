using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class webBill_ysglnew_LrbBmfjLeft : System.Web.UI.Page
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
                TreeBind();
            }
            ClientScript.RegisterArrayDeclaration("availableTags", GetkmAll());
        }
    }

    private void DrpSelectBid()
    {
        string selectndsql = "select distinct  nian from  dbo.bill_ysgc order by nian desc";
        DataTable selectdt = server.GetDataTable(selectndsql, null);

        for (int i = 0; i < selectdt.Rows.Count; i++)
        {
            drpSelectNd.Items.Add(new ListItem(selectdt.Rows[i]["nian"].ToString(), selectdt.Rows[i]["nian"].ToString()));
        }
        if (selectdt.Rows.Count > 0)
        {
            drpSelectNd.SelectedValue = selectdt.Rows[0]["nian"].ToString();
        }
    }

    private void TreeBind()
    {
        TreeNode tNode = new TreeNode("[" + drpSelectNd.SelectedValue + "]" + "科目", "00");
        tNode.NavigateUrl = "LrbBmfjList.aspx?nd=" + drpSelectNd.SelectedValue;
        tNode.Target = "list";
        tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
        this.TreeView1.Nodes.Clear();
        this.TreeView1.Nodes.Add(tNode);
        string sql = " select distinct kmcode,(select yskmmc from bill_yskm where bill_yskm.yskmcode=bill_ys_xmfjlrb.kmcode  )as kmname from bill_ys_xmfjlrb  where left(procode,4) = '" + drpSelectNd.SelectedValue + "' and kmcode not in (select yskmcode from bill_yskm where kmStatus!='1') ";

        DataSet temp = server.GetDataSet(sql);

        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["kmcode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["kmname"].ToString().Trim(), temp.Tables[0].Rows[i]["kmcode"].ToString().Trim());
            sNode.NavigateUrl = "LrbBmfjList.aspx?yskm=" + temp.Tables[0].Rows[i]["kmcode"].ToString().Trim() + "&nd=" + drpSelectNd.SelectedValue;
            sNode.Target = "list";
            sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
            sNode.Value = temp.Tables[0].Rows[i]["kmcode"].ToString().Trim();
            tNode.ChildNodes.Add(sNode);
        }
        TreeView1.ExpandAll();
    }
    private string GetkmAll()
    {
        find_txt_km.Text = "";
        DataSet ds = server.GetDataSet("select distinct '['+kmcode+']'+(select yskmmc from bill_yskm where bill_yskm.yskmcode=bill_ys_xmfjlrb.kmcode  )as kmname from bill_ys_xmfjlrb  where left(procode,4)  = '" + drpSelectNd.SelectedValue + "' ");
        StringBuilder arry = new StringBuilder();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arry.Append("'");
            arry.Append(Convert.ToString(dr["kmname"]));
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 1)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }
        

        return script;

    }
    protected void drpSelectNd_SelectedIndexChanged(object sender, EventArgs e)
    {
        TreeBind();
        GetkmAll();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Txtchange(object sender, EventArgs e)
    {

        string strkmcode = this.hdkm.Value.Trim();
        if (strkmcode.Equals(""))
        {
            return;
        }
        this.find_txt_km.Text = strkmcode;
        strkmcode = strkmcode.Substring(1, strkmcode.IndexOf("]") - 1);

        TreeNode tNode = new TreeNode("[" + drpSelectNd.SelectedValue + "]" + "科目", "00");
        tNode.NavigateUrl = "LrbBmfjList.aspx";
        tNode.Target = "list";
        tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
        this.TreeView1.Nodes.Clear();
        this.TreeView1.Nodes.Add(tNode);
        string sql = @" select distinct kmcode,(select yskmmc from bill_yskm 
where bill_yskm.yskmcode=bill_ys_xmfjlrb.kmcode  )as kmname,
(select distinct '['+kmcode+']'+(select yskmmc from bill_yskm 
where bill_yskm.yskmcode=bill_ys_xmfjlrb.kmcode  )as kmname)as kmcodename from bill_ys_xmfjlrb  where left(procode,4) = '" + drpSelectNd.SelectedValue + "' and kmcode='" + strkmcode + "'";

        DataSet temp = server.GetDataSet(sql);

        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["kmcode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["kmname"].ToString().Trim(), temp.Tables[0].Rows[i]["kmcode"].ToString().Trim());
            sNode.NavigateUrl = "LrbBmfjList.aspx?yskm=" + temp.Tables[0].Rows[i]["kmcode"].ToString().Trim() + "&nd=" + drpSelectNd.SelectedValue;
            sNode.Target = "list";
            sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
            sNode.Value = temp.Tables[0].Rows[i]["kmcode"].ToString().Trim();
            tNode.ChildNodes.Add(sNode);

        }
        TreeView1.ExpandAll();
    }
}
