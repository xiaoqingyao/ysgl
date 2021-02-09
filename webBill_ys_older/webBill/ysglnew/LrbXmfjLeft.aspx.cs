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
using System.Collections.Generic;
using Dal;

public partial class webBill_ysglnew_LrbXmfjLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    Dal.newysgl.Xmlr dal = new Dal.newysgl.Xmlr();
    string strdydj = "02";
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
                ConfigDal configdal = new ConfigDal();
                string strqmys=configdal.GetValueByKey("Qmys");
                if (!string.IsNullOrEmpty(strqmys) && strqmys=="Y")//是否为全面预算
                {
                    if (Request["dydj"] != null && Request["dydj"].ToString() != "")
                    {
                        strdydj = Request["dydj"].ToString();
                    }
                   
                }
                if (!string.IsNullOrEmpty(strdydj))
                {
                    (new yskm()).BindYskmbydydj(this.TreeView1.Nodes[0], "LrbXmfj.aspx", "list", "../Resources/Images/treeView/treeNode.gif", false, "'1'",strdydj);
                }
                else
                {
                    (new yskm()).BindYskm(this.TreeView1.Nodes[0], "LrbXmfj.aspx", "list", "../Resources/Images/treeView/treeNode.gif", false, "'1'");

                }
              //  DrpSelectBid();
            }
        }
    }

    //private void TreeBind()
    //{
    //    TreeNode tNode = new TreeNode("[" + drpNd.SelectedValue + "]" + "科目", "00");
    //    tNode.NavigateUrl = "LrbXmfj.aspx";
    //    tNode.Target = "list";
    //    tNode.ImageUrl = "../Resources/Images/treeView/treeHome.gif";
    //    this.TreeView1.Nodes.Clear();
    //    this.TreeView1.Nodes.Add(tNode);//select * from bill_yskm
    //    string sql = "select * from bill_yskm ";//where yskmcode in(select kmcode from bill_ys_xmfjlrb where annual='" + drpNd.SelectedValue + "')";
    //    //string sql = " select distinct kmcode,(select yskmmc from bill_yskm where bill_yskm.yskmcode=bill_ys_xmfjlrb.kmcode  )as kmname from bill_ys_xmfjlrb  where left(procode,4) = '" + drpNd.SelectedValue + "' ";

    //    DataSet temp = server.GetDataSet(sql);

    //    for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
    //    {
    //        TreeNode sNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim());
    //        sNode.NavigateUrl = "LrbXmfj.aspx?kmCode=" + temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim() + "&nd=" + drpNd.SelectedValue;
    //        sNode.Target = "list";
    //        sNode.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
    //        sNode.Value = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();


    //        tNode.ChildNodes.Add(sNode);
    //    }
    //    TreeView1.ExpandAll();
    //}
    //private void DrpSelectBid()
    //{
    //    List<string> ndlist = dal.GetNdByxmLrb("1");
    //    if (ndlist.Count > 0)
    //    {
    //        foreach (var i in ndlist)
    //        {
    //            drpNd.Items.Add(new ListItem(i, i));
    //        }
    //    }
    //}

}
