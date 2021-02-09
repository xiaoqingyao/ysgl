using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Bll.UserProperty;
using Models;
using System.Text;
using System.Data;

public partial class webBill_bxgl_YDeptSelectNew : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_self');", true);
        //}
        //else
        //{
            ClientScript.RegisterArrayDeclaration("availableTags", GetDeptAll());
            if (!IsPostBack)
            {
                IList<string> deplist = new List<string>();
                string deptcode = Request.Params["deptcode"];

                if (deptcode.Length > 0)
                {
                    deptcode = deptcode.Substring(0, deptcode.Length - 1);
                    string[] arry = deptcode.Split(':');
                    foreach (string code in arry)
                    {
                        deplist.Add(code.Split(']')[0]);//.Trim('[')  截取出来的是  金额$[编号
                    }
                }
                //dept = dept.Split(']')[0].Trim('[');
                SysManager smgr = new SysManager();
                IList<Bill_Departments> list = smgr.GetAllDept();
                InserTree(TreeView1.Nodes[0], list, deplist);

            }
        //}
    }

    private void InserTree(TreeNode tnd, IList<Bill_Departments> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.SjDeptCode == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Departments dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.DeptCode + "]" + dept.DeptName;
                tnc.Value = dept.DeptCode;
                //核算部门是否必须是末级部门配置项
                if (server.GetCellValue("select  avalue  from T_Config where akey='HsDeptIsLast' ") == "1")
                {
                    int count = Convert.ToInt32(server.GetCellValue("select count(*) from bill_departments where isnull(deptStatus,'1')='1' and sjDeptCode='" + dept.DeptCode                        + "'"));
                    if (count > 0 || dept.DeptCode == "000001")
                        tnc.ShowCheckBox = false;
                        else
                        tnc.ShowCheckBox = true;
                }
                else{

                    tnc.ShowCheckBox = true;
                }


                if (kmlist.Count > 0)
                {
                    //int cont = (from temp in kmlist
                    //            where temp.Substring(temp.IndexOf('[')) == dept.DeptCode
                    //            select temp).Count();
                    //if (cont > 0)
                    //{
                    //    tnc.Checked = true;

                    //}
                    var deptmsg = from temp in kmlist
                                  where temp.Substring(temp.IndexOf('[') + 1) == dept.DeptCode
                                  select temp;
                    if (deptmsg.Count() > 0)
                    {
                        tnc.Checked = true;
                        tnc.ToolTip = deptmsg.First().Split('$')[0];
                    }
                }
                InserTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        IList<string> list = new List<string>();
        ForeachTree(TreeView1.Nodes[0], ref list);
        if (list.Count > 0)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string km in list)
            {
                sb.Append(km);
                sb.Append("|");
            }
            sb.Remove(sb.Length - 1, 1);
            string script = sb.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue=\"" + script + "\"; self.close();</script>");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择部门!');");
        }
    }


    private void ForeachTree(TreeNode tnd, ref IList<string> list)
    {
        foreach (TreeNode tnc in tnd.ChildNodes)
        {
            if (tnc.Checked)
            {
                string hsje = tnc.ToolTip.Equals("") ? "0.00" : tnc.ToolTip.ToString();
                list.Add(hsje + "$" + tnc.Text);
            }
            ForeachTree(tnc, ref list);
        }
    }

    private string GetDeptAll()
    {
        find_txt_km.Text = "";
        SysManager smgr = new SysManager();
        IList<Bill_Departments> list = smgr.GetAllDept();
        StringBuilder arry = new StringBuilder();
        foreach (Bill_Departments dr in list)
        {
            arry.Append("'");
            arry.Append("[" + dr.DeptCode + "]" + dr.DeptName);
            arry.Append("',");
        }
        string script = "";
        if (arry.Length > 1)
        {
            script = arry.ToString().Substring(0, arry.Length - 1);
        }


        return script;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Txtchange(object sender, EventArgs e)
    {

        string strdeptcode = this.hddept.Value.Trim();
        if (strdeptcode.Equals(""))
        {
            return;
        }
        this.find_txt_km.Text = strdeptcode;
        //strdeptcode = strdeptcode.Substring(1, strdeptcode.IndexOf("]") - 1);
        ForeachTreeToCheck(this.TreeView1.Nodes[0], strdeptcode);
    }
    private void ForeachTreeToCheck(TreeNode tnd, string strdeptcode)
    {
        foreach (TreeNode tnc in tnd.ChildNodes)
        {
            if (tnc.Text.Equals(strdeptcode))
            {
                tnc.Checked = true;
            }
            ForeachTreeToCheck(tnc, strdeptcode);
        }
    }
}


