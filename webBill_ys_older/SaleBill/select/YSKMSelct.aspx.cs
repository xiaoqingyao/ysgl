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
using System.Web.Script.Serialization;
using Bll.UserProperty;
using System.Collections.Generic;
using Models;
using System.Text;

public partial class SaleBill_select_YSKMSelct : System.Web.UI.Page
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
                IList<string> kmlist = new List<string>();


                string dept = Request.Params["deptcode"];
                dept = dept.Split(']')[0].Trim('[');
                SysManager smgr = new SysManager();

                IList<Bill_Yskm> list1 = smgr.GetYskmByDep(dept);
                InserTree(TreeView1.Nodes[0], list1, kmlist);

            }
        }
    }


    private void InserTree(TreeNode tnd, IList<Bill_Yskm> list, IList<string> kmlist)
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
                InserTree(tnc, list, kmlist);
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
        string billDate = Request.Params["billDate"];

        DateTime dt = DateTime.Now;
        if (!string.IsNullOrEmpty(billDate))
        {
            dt = Convert.ToDateTime(billDate);
        }
        if (list.Count > 0)
        {
            //StringBuilder sb = new StringBuilder();
            IList<JsonRet> retlist = new List<JsonRet>();

            foreach (string km in list)
            {
                //根据配置查看是否开启月度预算。
                //string config = (new SysManager()).GetsysConfig()["MonthOrQuarter"];


                JsonRet temp = new JsonRet();
                temp.Yscode = km;

                YsManager ysmgr = new YsManager();

                string deptCode = Request.Params["deptcode"];
                deptCode = deptCode.Split(']')[0].Trim('[');
                string kmCode = km.Split(']')[0].Trim('[');

                //string[] gcCode = ysmgr.GetYsgcCode(dt);
                //string gcbh;
                //if (config == "1")
                //{
                //    //季度
                //    gcbh = gcCode[1];
                //}
                //else if (config == "0")
                //{
                //    //年度
                //    gcbh = gcCode[0];
                //}
                //else
                //{
                //    //月度
                //    gcbh = gcCode[2];
                //}
                //decimal hfje = ysmgr.GetYueHf(gcbh, deptCode, kmCode);
                //decimal ysje = ysmgr.GetYueYs(gcbh, deptCode, kmCode);
                //decimal syje = ysje - hfje;

                //sb.Append(" 预算:"+ysje.ToString()+" 剩余:"+syje.ToString());

                //sb.Append("|");
                //temp.Ysje = ysje;
                //temp.Syje = syje;
                retlist.Add(temp);
            }
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            string script = jserializer.Serialize(retlist);
            //sb.Remove(sb.Length - 1, 1);
            //string script = sb.ToString();
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script type='text/javascript'>window.returnValue='" + script + "'; self.close();</script>");
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
        public string Yscode { get; set; }
        public decimal Ysje { get; set; }
        public decimal Syje { get; set; }
    }
}
