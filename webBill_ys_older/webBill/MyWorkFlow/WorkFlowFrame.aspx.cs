using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowDal;
using WorkFlowLibrary.WorkFlowBll;

public partial class webBill_MyWorkFlow_WorkFlowFrame : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<BillToWorkFlow> temp = new WorkFlowManager().GetBillAll();
            foreach (BillToWorkFlow btw in temp)
            {
                TreeNode tnc = new TreeNode();
                tnc.Text = btw.BillName;
                tnc.NavigateUrl = "WorkFlowList.aspx?flowid=" + btw.FlowId;
                tnc.Target = "right";
                tnc.ImageUrl="../Resources/images/treeview/treeNode.gif";
                TreeView1.Nodes[0].ChildNodes.Add(tnc);
            }
        }
    }
}
