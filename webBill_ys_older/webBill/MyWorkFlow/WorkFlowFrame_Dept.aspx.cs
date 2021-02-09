using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkFlowLibrary.WorkFlowBll;
using WorkFlowLibrary.WorkFlowDal;

public partial class webBill_MyWorkFlow_WorkFlowFrame_Dept : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
            return;
        }
        if (!IsPostBack)
        {
            //databind();
            string strusercode = Session["userCode"].ToString();// Session["userCode"];
            if (!string.IsNullOrEmpty(strusercode))
            {
                //string strsql = "exec [js_deptsbyuser]'" + strusercode + "'";

                string strDeptCodes = new Departments().GetUserRightDepartments(Session["userCode"].ToString().Trim(), "", "0");
                //strDeptCodes = strDeptCodes.Substring(1, strDeptCodes.Length-1);
                //strDeptCodes.Replace("','", ",");
                //string[] arrStrRel = strDeptCodes.Split(new string[] { "" }, StringSplitOptions.None);
                //int arrCount = arrStrRel.Length;
                if (strDeptCodes != "")
                {
                    DataTable dtRel = server.GetDataTable("select deptCode,deptName from bill_departments where sjdeptCode!='000001' and sjdeptCode!='' and deptCode in (" + strDeptCodes + ") order by deptCode", null);
                    for (int i = 0; i < dtRel.Rows.Count; i++)
                    {
                        ListItem li = new ListItem();
                        li.Text = "[" + dtRel.Rows[i]["deptCode"].ToString().Trim() + "]" + dtRel.Rows[i]["deptName"].ToString().Trim();
                        li.Value = dtRel.Rows[i]["deptCode"].ToString().Trim();
                        this.ddlBilldept.Items.Add(li);
                    }
                }

                //DataTable dt = new DataTable();
                //dt = server.GetDataTable(strsql, null);
            }
            bindtree();
        }
    }

    protected void ddlBilldept_TextChanged(object sender, EventArgs e)
    {
        bindtree();

    }

    private void bindtree()
    {
        TreeView1.Nodes[0].ChildNodes.Clear();
        IList<BillToWorkFlow> temp = new WorkFlowManager().GetBillAll();
        foreach (BillToWorkFlow btw in temp)
        {
          
            TreeNode tnc = new TreeNode();
            tnc.Text = btw.BillName;
            tnc.NavigateUrl = "WorkFlowList_Dept.aspx?flowid=" + btw.FlowId + "&billdept=" + ddlBilldept.SelectedValue;
            tnc.Target = "right";
            tnc.ImageUrl = "../Resources/Images/treeView/treeNode.gif";
      
            TreeView1.Nodes[0].ChildNodes.Add(tnc);
        }
    }
}