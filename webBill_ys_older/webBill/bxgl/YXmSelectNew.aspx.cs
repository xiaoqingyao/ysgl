using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Bll.UserProperty;
using System.Text;
using System.Data;
using System.Web.Script.Serialization;

public partial class webBill_bxgl_YXmSelectNew : System.Web.UI.Page
{
    sqlHelper.sqlHelper server = new sqlHelper.sqlHelper();
    string hsxmModel = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userCode"] == null || Session["userCode"].ToString().Trim() == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "window.open('../../webBill.aspx','_top');", true);
        }
        else
        {
            hsxmModel = server.GetCellValue("select isnull(avalue,'') from T_config where akey='YbbxHsxmMode'");
            if (!IsPostBack)
            {
                IList<string> xmlist = new List<string>();
                string deptcode = Request.Params["deptcode"];
                deptcode = deptcode.Split(']')[0].Trim('[');
                string xmcode = Request.Params["xmcode"];
                string nd = Request.Params["billDate"];
                if (xmcode.Length > 0)
                {
                    xmcode = xmcode.Substring(0, xmcode.Length - 1);
                    string[] arry = xmcode.Split(':');
                    foreach (string code in arry)
                    {
                        xmlist.Add(code.Split(']')[0].Trim('['));
                    }
                }
                SysManager smgr = new SysManager();
                //Response.Write("<script>alert(" + nd + ");</script>");


                if (hsxmModel == "1")
                {
                    if (nd.Length > 4)
                    {
                        nd = nd.Substring(0, 4);
                    }
                    IList<bill_xm_dept_nd> list = smgr.GetXmByDepNd(deptcode, nd);
                    InserTree(TreeView1.Nodes[0], list, xmlist);
                }
                else
                {
                    IList<Bill_Xm> list = smgr.GetXmByDep(deptcode);
                    InserTree(TreeView1.Nodes[0], list, xmlist);
                }


            }
        }
    }


    private void InserTree(TreeNode tnd, IList<bill_xm_dept_nd> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.SjXm == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (bill_xm_dept_nd dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.xmCode + "]" + dept.XmName;
                tnc.Value = dept.xmCode;
                tnc.ShowCheckBox = true;
                if (kmlist.Count > 0)
                {
                    int cont = (from temp in kmlist
                                where temp == dept.xmCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
                    }
                }
                InserTree(tnc, list, kmlist);
                tnd.ChildNodes.Add(tnc);
            }
        }

    }
    private void InserTree(TreeNode tnd, IList<Bill_Xm> list, IList<string> kmlist)
    {
        var childs = from child in list
                     where child.SjXm == tnd.Value
                     select child;
        int sl = childs.Count();
        if (sl > 0)
        {
            foreach (Bill_Xm dept in childs)
            {
                TreeNode tnc = new TreeNode();

                tnc.Text = "[" + dept.XmCode + "]" + dept.XmName;
                tnc.Value = dept.XmCode;
                tnc.ShowCheckBox = true;
                if (kmlist.Count > 0)
                {
                    int cont = (from temp in kmlist
                                where temp == dept.XmCode
                                select temp).Count();
                    if (cont > 0)
                    {
                        tnc.Checked = true;
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
            if (hsxmModel == "1")
            {
                string billDate = Request.Params["billDate"];
                if (billDate.Length > 4)
                {
                    billDate = billDate.Substring(0, 4);
                }
                IList<JsonRet> retlist = new List<JsonRet>();
                foreach (string km in list)
                {
                    string t = km.Substring(1, km.LastIndexOf(']') - 1);
                    string sql = "	select * from bill_xm_dept_nd where xmCode='" + t + "' and  nd='" + billDate + "'";
                    DataTable dt = server.GetDataSet(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        JsonRet temp = new JsonRet();
                        temp.xmmc = km;
                        temp.xm = dt.Rows[0]["xmCode"].ToString();
                        if (string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["je"])) || Convert.ToString(dt.Rows[0]["je"]) == "0.00")
                        {
                            temp.je = "0.00";
                            temp.syje = "0.00";
                        }
                        else
                        {
                            temp.je = dt.Rows[0]["je"].ToString();
                            string sqlje = "select isnull(SUM(je),'0') as je  from bill_ybbxmxb_hsxm where xmCode='" + t + "'";
                            string xmzje = Convert.ToString(server.GetDataSet(sqlje).Tables[0].Rows[0]["je"]);

                            temp.syje = string.Format("{0:N}", Convert.ToString(Convert.ToDecimal(temp.je) - Convert.ToDecimal(xmzje)));
                        }
                        temp.ctrl = Convert.ToString(dt.Rows[0]["isCtrl"]) == "1" ? "是" : "否";

                        retlist.Add(temp);
                    }
                }
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                string script = jserializer.Serialize(retlist);
                ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
            }
            else
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
        }

        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "alert('请选择科目!');");
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
        public string xm { get; set; }
        public string xmmc { get; set; }
        public string je { get; set; }
        public string syje { get; set; }
        public string ctrl { get; set; }

    }

}

