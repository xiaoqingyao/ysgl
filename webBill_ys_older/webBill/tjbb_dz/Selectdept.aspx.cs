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
using Dal;

public partial class webBill_tjbb_dz_Selectdept : System.Web.UI.Page
{
    static bool boCheckFlg = true;
    private string strdydj = "02";//对应单据编号默认02 
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetSlidingExpiration(true);
        Response.Cache.SetNoStore();
        if (!IsPostBack)
        {
            //单据对应编号
            object objdydj = Request["dydj"];
            if (objdydj != null && objdydj.ToString() != "")
            {
                strdydj = objdydj.ToString();
            }
            IList<string> deptlist = new List<string>();

            string strdept = server.GetCellValue("select top 1 deptCode from bill_tcsql");

            if (!string.IsNullOrEmpty(strdept))
            {
                deptlist = strdept.Split(new string[] { "," }, StringSplitOptions.None);
            }
            SysManager smgr = new SysManager();

            IList<Bill_Departments> list1 = new List<Bill_Departments>();
            list1 = smgr.GetAllDept();
            InserTree(TreeView1.Nodes[0], list1, deptlist);


        }
    }


    private void InserTree(TreeNode tnd, IList<Bill_Departments> list, IList<string> deptlist)
    {
        var childs = from child in list
                     where child.DeptCode.Length == tnd.Value.Length + 2 && child.DeptCode.Substring(0, tnd.Value.Length) == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Departments dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
                tnc.Value = dept.DeptCode;
                tnc.SelectAction = TreeNodeSelectAction.None;
                ////末级选中
                if (deptlist.Count > 0)
                {
                    int cont = (from temp in deptlist
                                where temp == dept.DeptCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                else
                {
                    tnc.Checked = true;
                }
                InserTree(tnc, list, deptlist);
                tnd.ChildNodes.Add(tnc);
            }
        }
        else
        {
            tnd.ShowCheckBox = true;
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        IList<string> list = new List<string>();
        ForeachTree(TreeView1.Nodes[0], ref list);
        YsglDal ysDal = new YsglDal();
        DateTime dt = DateTime.Now;
        if (list.Count > 0)
        {
            IList<JsonRet> retlist = new List<JsonRet>();

            foreach (string dept in list)
            {
                JsonRet temp = new JsonRet();
                temp.deptcode = dept;

                YsManager ysmgr = new YsManager();

                string kmCode = string.IsNullOrEmpty(dept) ? "" : dept.Split(']')[0].Trim('[');
                if (!string.IsNullOrEmpty(Request["dydj"]))
                {
                    strdydj = Request["dydj"];
                }
                retlist.Add(temp);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string script = jserializer.Serialize(retlist);
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择部门!');", true);
        }
    }

    private void ForeachTree(TreeNode tnd, ref IList<string> list)
    {
        foreach (TreeNode tnc in tnd.ChildNodes)
        {
            if (tnc.Checked)
            {
                list.Add(tnc.Text);
            }
            ForeachTree(tnc, ref list);
        }
    }


    class JsonRet
    {
        public string deptcode { get; set; }

    }
    //protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    //{
    //    CheckOrUnCheckAllNodes(this.TreeView1.Nodes[0], boCheckFlg);
    //    boCheckFlg = !boCheckFlg;
    //    this.TreeView1.Nodes[0].Selected = !this.TreeView1.Nodes[0].Selected;
    //}
    /// <summary>
    /// 选中或者不选中所有的node
    /// </summary>
    /// <param name="tnd">开始操作的node节点</param>
    /// <param name="boCheck">状态</param>
    //private void CheckOrUnCheckAllNodes(TreeNode tnd, bool boCheck)
    //{
    //    foreach (TreeNode node in tnd.ChildNodes)
    //    {
    //        if (node.ShowCheckBox == true)
    //        {
    //            node.Checked = boCheck;
    //        }
    //        CheckOrUnCheckAllNodes(node, boCheck);
    //    }
    //}

}


