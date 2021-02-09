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
using Models;
using Bll.UserProperty;

public partial class SaleBill_SaleFee_SaleDeptFeeLeft : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string strdatefrom = "";
    string strdateto = "";
    string strdeptcode = ""; 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            if (Request["datefrom"] != null)
            {
                strdatefrom = Request["datefrom"].ToString();
            } 
            if (Request["dateto"] != null)
            {
                strdateto = Request["dateto"].ToString();
            }
            if (Request["deptcode"] != null)
            {
                strdeptcode = Request["deptcode"].ToString();
            }
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(strdeptcode))
                {
                    initFeeType(strdeptcode);
                    treeViewFeeType.Nodes[0].NavigateUrl = "SaleDeptFeelist.aspx?yskmcode=0&deptcode=" + strdeptcode + "&datefrom=" + strdatefrom + "&dateto=" + strdateto;
                    treeViewFeeType.Nodes[0].Target = "list";
                }
            }
        }
    }
    private void initFeeType(string strDeptCode)
    {
        SysManager smgr = new SysManager();
        IList<string> kmlist = new List<string>();
        IList<Bill_Yskm> list1 = smgr.GetYskmByDep(strDeptCode);
        SysManager sysMgr = new SysManager();
        sysMgr.SetEndYsbm(list1);
        InserFeeTypeTree(treeViewFeeType.Nodes[0], list1, kmlist);
        this.treeViewFeeType.ExpandAll();
    }
    /// <summary>
    /// 费用类别加载
    /// </summary>
    /// <param name="tnd"></param>
    /// <param name="list"></param>
    /// <param name="kmlist"></param>
    private void InserFeeTypeTree(TreeNode tnd, IList<Bill_Yskm> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.YskmCode.Length == tnd.Value.Length + 2 && child.YskmCode.Substring(0, tnd.Value.Length) == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Yskm yskm in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + yskm.YskmCode + "]" + yskm.YskmMc;
                tnc.Value = yskm.YskmCode;
                tnc.ToolTip = yskm.IsEnd;
                tnc.Target = "list";
                tnc.NavigateUrl = "SaleDeptFeelist.aspx?yskmcode="+tnc.Value+"&deptcode=" + strdeptcode + "&datefrom=" + strdatefrom + "&dateto="+strdateto;
                //末级选中
                if (kmlist.Count > 0)
                {
                    int cont = (from temp in kmlist
                                where temp == yskm.YskmCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserFeeTypeTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }
    }
}
