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

public partial class webBill_bxgl_selectYskm : System.Web.UI.Page
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
                //string deptCode = server.GetCellValue("select userDept from bill_users where userCode='" + Session["userCode"].ToString().Trim() + "'");
                string deptCode = (new billCoding()).GetDeptLevel2_userCode(Session["userCode"].ToString().Trim());
                this.BindYskm_NoUrl(this.TreeView1.Nodes[0], "yskmList.aspx", "list", "../Resources/Images/treeView/treeNode.gif", true, deptCode);
            }    
        }
    }

    public void BindYskm_NoUrl(TreeNode pNode, string url, string target, string imgUrl, bool showCheckBox, string deptCode)
    {
        DataSet temp = server.GetDataSet("select * from bill_yskm where yskmCode in (select yskmCode from bill_yskm_dept where deptCode='" + deptCode + "') or tblx='02'");
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Length == 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                tNode.SelectAction = TreeNodeSelectAction.None;
                //if (url != "")
                //{
                //    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                //    tNode.Target = target;
                //}
                //if (imgUrl != "")
                //{
                //    tNode.ImageUrl = imgUrl;
                //}
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindYskm_No2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }
    private void BindYskm_No2(TreeNode pNode, DataSet temp, string url, string target, string imgUrl, bool showCheckBox)
    {
        for (int i = 0; i <= temp.Tables[0].Rows.Count - 1; i++)
        {
            string kmCode = temp.Tables[0].Rows[i]["yskmCode"].ToString().Trim();
            if (kmCode.Substring(0, kmCode.Length - 2) == pNode.Value
                && kmCode.Length == pNode.Value.Length + 2)
            {
                TreeNode tNode = new TreeNode("[" + temp.Tables[0].Rows[i]["yskmBm"].ToString().Trim() + "]" + temp.Tables[0].Rows[i]["yskmMc"].ToString().Trim(), kmCode);
                tNode.SelectAction = TreeNodeSelectAction.None;
                //if (url != "")
                //{
                //    tNode.NavigateUrl = url + "?kmCode=" + tNode.Value;
                //    tNode.Target = target;
                //}
                //if (imgUrl != "")
                //{
                //    tNode.ImageUrl = imgUrl;
                //}
                if (showCheckBox == true)
                {
                    tNode.ShowCheckBox = true;
                }
                pNode.ChildNodes.Add(tNode);
                this.BindYskm_No2(tNode, temp, url, target, imgUrl, showCheckBox);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string yskm = "";
        for (int i = 0; i <= this.TreeView1.Nodes[0].ChildNodes.Count - 1; i++)
        {
            if (this.TreeView1.Nodes[0].ChildNodes[i].Checked == true)
            {
                yskm += "" + this.TreeView1.Nodes[0].ChildNodes[i].Value + ",";
            }

            this.bindNextLevel(this.TreeView1.Nodes[0].ChildNodes[i], ref yskm);
        }

        string billCode = Page.Request.QueryString["billCode"].ToString().Trim();
        if (yskm == "")
        {
            Button2_Click(sender, e);
        }
        else
        {
            yskm = yskm.Substring(0, yskm.Length - 1);
            string[] arr = yskm.Split(',');
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            for (int i = 0; i <= arr.Length - 1; i++)
            {
                DataSet tmep = server.GetDataSet("select * from bill_qtbxmxb_fykm where billCode='" + billCode + "' and fykm='" + arr[i].ToString() + "'");
                if (tmep.Tables[0].Rows.Count == 0)
                {
                    string guid = (new GuidHelper()).getNewGuid();
                    //list.Add("insert into bill_qtbxmxb_fykm(billCode,fykm,je,mxGuid) select '" + billCode + "',yskmCode,'0','" + guid + "' from bill_yskm where yskmCode ='" + arr[i].ToString() + "' and yskmCode not in (select fykm from bill_qtbxmxb_fykm where billCode='" + billCode + "')");

                    string ms = (new yskm()).getYsxx(Page.Request.QueryString["deptCode"].ToString().Trim(),Page.Request.QueryString["sqrq"].ToString().Trim(),arr[i].ToString().Trim()) ;//获取当前的预算信息

                    list.Add("insert into bill_qtbxmxb_fykm(billCode,fykm,je,mxGuid,status,ms) values('" + billCode + "','" + arr[i].ToString() + "','0','" + guid + "','0','" + ms + "')");

                    //string dept = Page.Request.QueryString["deptCode"].ToString().Trim();
                    //dept = dept.Substring(1, dept.IndexOf("]") - 1);
                    //list.Add("insert into bill_qtbxmxb_fykm_dept values('" + guid + "','" + guid + "','" + dept + "',0,'0')");

                    string ftmxGuid = (new GuidHelper()).getNewGuid();
                    list.Add("insert into bill_qtbxmxb_fykm_ft(billCode,kmmxGuid,cbzx,je,ftmxGuid,status) select top 1 '" + billCode + "','" + guid + "',zxCode,0,'" + ftmxGuid + "','0' from bill_cbzx");
                }
            }

            if (server.ExecuteNonQuerysArray(list) == -1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "alert('保存失败！');", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"sucess\";self.close();", true);
            }
        }
    }

    public void bindNextLevel(TreeNode pNode, ref string yskm)
    {
        for (int i = 0; i <= pNode.ChildNodes.Count - 1; i++)
        { 
            if (pNode.ChildNodes[i].Checked == true)
            {
                yskm += "" + pNode.ChildNodes[i].Value + ","; 
            }

            this.bindNextLevel(pNode.ChildNodes[i], ref yskm);
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "", "window.returnValue=\"\";self.close();", true);
    }
}
