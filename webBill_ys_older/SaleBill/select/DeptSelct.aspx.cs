﻿using System;
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
using Bll.UserProperty;
using Models;
using System.Collections.Generic;
using System.Text;

public partial class SaleBill_select_DeptSelct : System.Web.UI.Page
{
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
                IList<string> deplist = new List<string>();
              
                SysManager smgr = new SysManager();
                IList<Bill_Departments> list = smgr.GetAllDept();
                InserTree(TreeView1.Nodes[0], list, deplist);

            }
        }
    }

    private void InserTree(TreeNode tnd, IList<Bill_Departments> list, IList<string> deplist)
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
                tnc.ShowCheckBox = true;
                if (deplist.Count > 0)
                {
                    int cont = (from temp in deplist
                                where temp == dept.DeptCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserTree(tnc, list, deplist);
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
                list.Add(tnc.Text);
            }
            ForeachTree(tnc, ref list);
        }
    }
}
